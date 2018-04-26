using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Menu;
using Wow.Tv.Middle.Model.Db49.editVOD;

namespace Wow.Tv.Middle.Biz.Menu
{
    public class MenuBiz : BaseBiz
    {
        
        public ListModel<NTB_MENU> SearchList(MenuCondition condition)
        {
            ListModel<NTB_MENU> resultData = new ListModel<NTB_MENU>();

            var list = db49_wowtv.NTB_MENU.Where(a => a.DEL_YN == "N");

            if (condition.UpMenuSeq >= 0)
            {
                list = list.Where(a => a.UP_MENU_SEQ == condition.UpMenuSeq);
            }

            if (String.IsNullOrEmpty(condition.ChannelCode) == false)
            {
                if (condition.ChannelCode == CommonCodeStatic.MENU_BROAD_ADMIN_OR_DUAL_CHANNEL_CODE)
                {
                    list = list.Where(a => a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE);
                }
                else if (condition.ChannelCode == CommonCodeStatic.MENU_BROAD_FRONT_OR_DUAL_CHANNEL_CODE)
                {
                    list = list.Where(a => a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE);
                }
                else
                {
                    list = list.Where(a => a.CHANNEL_CODE == condition.ChannelCode);
                }
            }


            if (String.IsNullOrEmpty(condition.ContentTypeCode) == false)
            {
                list = list.Where(a => a.CONTENT_TYPE_CODE == condition.ContentTypeCode);
            }


            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                if (condition.SearchType == "Name")
                {
                    list = list.Where(a => a.MENU_NAME.Contains(condition.SearchText) == true);
                }
            }


            if (String.IsNullOrEmpty(condition.ActiveYn) == false)
            {
                list = list.Where(a => a.ACTIVE_YN == condition.ActiveYn);
            }


            if(String.IsNullOrEmpty(condition.SearchProgramCode) == false)
            {
                list = list.Where(a => a.PRG_CD == condition.SearchProgramCode);
            }


            if (condition.Depth > 0)
            {
                list = list.Where(a => a.DEPTH == condition.Depth);
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderBy(a => a.DEPTH).ThenBy(a => a.SORD_ORDER);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            foreach(var item in resultData.ListData)
            {
                if (item.CONTENT_TYPE_CODE == "Board")
                {
                    var boardContent = new Board.BoardBiz().GetAt(item.CONTENT_SEQ.Value);
                    if (boardContent != null)
                    {
                        item.BoardTypeCode = boardContent.BOARD_TYPE_CODE;
                        item.ContentName = boardContent.BOARD_NAME;
                    }
                }
            }
                

            return resultData;
        }


        public ListModel<MenuGroupItem> SearchListMenuGroup(int groupSeq, MenuCondition condition)
        {
            MenuGroupBiz menuGroupBiz = new MenuGroupBiz();

            ListModel<MenuGroupItem> resultData = new ListModel<MenuGroupItem>();
            List<MenuGroupItem> resultList = new List<MenuGroupItem>();
            ListModel<NTB_MENU> list = SearchList(condition);

            foreach(var item in list.ListData)
            {
                MenuGroupItem menuGroupItem = new MenuGroupItem();
                menuGroupItem.menu = item;
                menuGroupItem.menuGroup = menuGroupBiz.GetAt(item.MENU_SEQ, groupSeq);
                if(menuGroupItem.menuGroup == null)
                {
                    menuGroupItem.menuGroup = new NTB_MENU_GROUP();
                }
                resultList.Add(menuGroupItem);
            }
            resultData.ListData = resultList;

            return resultData;
        }



        public NTB_MENU GetAt(int menuSeq)
        {
            //db49_wowtv.Database.Log = sql => System.Diagnostics.Debug.Write(sql);
            NTB_MENU model = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == menuSeq);

            if (model != null && model.CONTENT_SEQ != null)
            {
                if (model.CONTENT_TYPE_CODE == "Html")
                {
                    var htmlContent = new BusinessManage.BusinessManageBiz().GetData(model.CONTENT_SEQ.Value);
                    if (htmlContent != null)
                    {
                        model.ContentName = htmlContent.TITLE;
                    }
                }
                else if (model.CONTENT_TYPE_CODE == "Board")
                {
                    var boardContent = new Board.BoardBiz().GetAt(model.CONTENT_SEQ.Value);
                    if (boardContent != null)
                    {
                        model.BoardTypeCode = boardContent.BOARD_TYPE_CODE;
                        model.ContentName = boardContent.BOARD_NAME;
                    }
                }
            }

            return model;
        }




        public int Save(NTB_MENU model, LoginUser loginUser)
        {
            NTB_MENU upMenu = null;

            NTB_MENU data = GetAt(model.MENU_SEQ);
            if (data == null)
            {
                int sortOrder = 1;
                NTB_MENU maxOrder = null;
                if (model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE)
                {
                    maxOrder = db49_wowtv.NTB_MENU.Where(a => 
                        (
                            a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                            || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                        )
                        && a.UP_MENU_SEQ == model.UP_MENU_SEQ
                        && a.DEPTH == model.DEPTH).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                else if (model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE)
                {
                    maxOrder = db49_wowtv.NTB_MENU.Where(a =>
                        (
                            a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                            || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                        )
                        && a.UP_MENU_SEQ == model.UP_MENU_SEQ
                        && a.DEPTH == model.DEPTH).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                else if (model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE)
                {
                    maxOrder = db49_wowtv.NTB_MENU.Where(a =>
                        (
                            a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                            || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                            || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                        )
                        && a.UP_MENU_SEQ == model.UP_MENU_SEQ
                        && a.DEPTH == model.DEPTH).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                else
                {
                    maxOrder = db49_wowtv.NTB_MENU.Where(a => a.UP_MENU_SEQ == model.UP_MENU_SEQ && a.CHANNEL_CODE == model.CHANNEL_CODE && a.DEPTH == model.DEPTH).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                if(maxOrder != null)
                {
                    sortOrder = maxOrder.SORD_ORDER + 1;
                }
                model.SORD_ORDER = sortOrder;
                
                switch(model.DEPTH)
                {
                    case 1:
                        model.MENU_NAME_DEPTH_1 = model.MENU_NAME;
                        break;
                    case 2:
                        model.MENU_NAME_DEPTH_2 = model.MENU_NAME;
                        upMenu = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == model.UP_MENU_SEQ);
                        model.MENU_NAME_DEPTH_1 = upMenu.MENU_NAME;
                        break;
                    case 3:
                        model.MENU_NAME_DEPTH_3 = model.MENU_NAME;
                        upMenu = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == model.UP_MENU_SEQ);
                        model.MENU_NAME_DEPTH_2 = upMenu.MENU_NAME;
                        upMenu = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == upMenu.UP_MENU_SEQ);
                        model.MENU_NAME_DEPTH_1 = upMenu.MENU_NAME;
                        break;
                }



                // 방송쪽 메뉴라면
                if (model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                    || model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                    || model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE)
                {
                    // 코너일경우
                    if(model.CONTENT_TYPE_CODE == "Corner")
                    {
                        // 코너생성
                        string programName = "";
                        tblSubConerList newCorner = new tblSubConerList();
                        tblSubConerList corner = db49_editVOD.tblSubConerList.OrderByDescending(a => a.scCode).FirstOrDefault();
                        int scCode = int.Parse(corner.scCode) + 1;

                        programName = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == model.PRG_CD).PRG_NM;
                        
                        newCorner.cnCode = "177";
                        newCorner.scCode = scCode.ToString();
                        newCorner.PGMID = model.PRG_CD;
                        newCorner.PGMName = programName;
                        newCorner.insDate = DateTime.Now;
                        newCorner.scName = model.MENU_NAME;
                        newCorner.scName = model.MENU_NAME;
                        newCorner.Writer = loginUser.LoginId;
                        newCorner.Delflag = 0;
                        newCorner.EditView = "Y";
                        db49_editVOD.tblSubConerList.Add(newCorner);
                        db49_editVOD.SaveChanges();

                        model.CONTENT_SEQ = scCode;
                    }
                }

                model.DEL_YN = "N";
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;
                db49_wowtv.NTB_MENU.Add(model);
            }
            else
            {
                //data.CHANNEL_CODE = model.CHANNEL_CODE;
                data.UP_MENU_SEQ = model.UP_MENU_SEQ;
                data.DEPTH = model.DEPTH;
                //data.MENU_SEQ_DEPTH1 = model.MENU_SEQ_DEPTH1;
                //data.MENU_SEQ_DEPTH2 = model.MENU_SEQ_DEPTH2;
                //data.MENU_SEQ_DEPTH3 = model.MENU_SEQ_DEPTH3;
                //data.SORD_ORDER = model.SORD_ORDER;
                data.MENU_NAME = model.MENU_NAME;
                data.CONTENT_TYPE_CODE = model.CONTENT_TYPE_CODE;
                data.CONTENT_SEQ = model.CONTENT_SEQ;
                data.ACTIVE_YN = model.ACTIVE_YN;
                data.REMARK = model.REMARK;

                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;

                // // 방송쪽 메뉴가 아니라면
                if ((model.CHANNEL_CODE != CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                        && model.CHANNEL_CODE != CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                        && model.CHANNEL_CODE != CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE))
                {
                    data.LINK_TYPE_CODE = model.LINK_TYPE_CODE;
                    data.LINK_URL = model.LINK_URL;
                }
            }
            db49_wowtv.SaveChanges();

            // 2뎁스일 경우
            if(model.DEPTH == 2)
            {
                // 방송쪽 메뉴라면
                if (model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                    || model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                    || model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE)
                {
                    upMenu = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == model.UP_MENU_SEQ);
                    if(upMenu != null)
                    {
                        //upMenu.CONTENT_TYPE_CODE = "Page";
                        upMenu.CONTENT_SEQ = null;
                        upMenu.LINK_URL = "javascript:void(0);";
                        db49_wowtv.SaveChanges();
                    }
                }
            }
            return model.MENU_SEQ;
        }


        public void Delete(int menuSeq, LoginUser loginUser)
        {
            NTB_MENU data = GetAt(menuSeq);
            if (data != null)
            {
                //db49_wowtv.NTB_MENU.Remove(data);
                data.DEL_YN = "Y";
                db49_wowtv.SaveChanges();
            }
        }


        /// <summary>
        /// 메뉴순서 위아래로 변경
        /// </summary>
        /// <param name="menuSeq"></param>
        /// <param name="isUp"></param>
        /// <param name="loginUser"></param>
        public void UpDown(int menuSeq, bool isUp, LoginUser loginUser)
        {
            NTB_MENU upDownMenu = null;
            
            var prev = GetAt(menuSeq);

            if (isUp == true)
            {
                // 바로위 메뉴를 조회
                if (prev.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE)
                {
                    upDownMenu = db49_wowtv.NTB_MENU.Where(a => 
                        a.DEL_YN == "N" 
                        && (
                                a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                                || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                            )
                        && a.UP_MENU_SEQ == prev.UP_MENU_SEQ
                        && a.DEPTH == prev.DEPTH && a.SORD_ORDER < prev.SORD_ORDER).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                else if (prev.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE)
                {
                    upDownMenu = db49_wowtv.NTB_MENU.Where(a =>
                        a.DEL_YN == "N"
                        && (
                                a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                                || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                            )
                        && a.UP_MENU_SEQ == prev.UP_MENU_SEQ
                        && a.DEPTH == prev.DEPTH && a.SORD_ORDER < prev.SORD_ORDER).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                else if (prev.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE)
                {
                    upDownMenu = db49_wowtv.NTB_MENU.Where(a =>
                        a.DEL_YN == "N"
                        && (
                                a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                                || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                                || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                            )
                        && a.UP_MENU_SEQ == prev.UP_MENU_SEQ
                        && a.DEPTH == prev.DEPTH && a.SORD_ORDER < prev.SORD_ORDER).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                else
                {
                    upDownMenu = db49_wowtv.NTB_MENU.Where(a => a.UP_MENU_SEQ == prev.UP_MENU_SEQ && a.DEL_YN == "N" && a.CHANNEL_CODE == prev.CHANNEL_CODE && a.DEPTH == prev.DEPTH && a.SORD_ORDER < prev.SORD_ORDER).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
            }
            else
            {
                // 바로아래 메뉴를 조회
                if (prev.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE)
                {
                    upDownMenu = db49_wowtv.NTB_MENU.Where(a => 
                        a.DEL_YN == "N" 
                        && (
                                a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                                || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                            )
                        && a.UP_MENU_SEQ == prev.UP_MENU_SEQ
                        && a.DEPTH == prev.DEPTH && a.SORD_ORDER > prev.SORD_ORDER).OrderBy(a => a.SORD_ORDER).FirstOrDefault();
                }
                else if (prev.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE)
                {
                    upDownMenu = db49_wowtv.NTB_MENU.Where(a =>
                        a.DEL_YN == "N"
                        && (
                                a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                                || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                            )
                        && a.UP_MENU_SEQ == prev.UP_MENU_SEQ
                        && a.DEPTH == prev.DEPTH && a.SORD_ORDER > prev.SORD_ORDER).OrderBy(a => a.SORD_ORDER).FirstOrDefault();
                }
                else if (prev.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE)
                {
                    upDownMenu = db49_wowtv.NTB_MENU.Where(a =>
                        a.DEL_YN == "N"
                        && (
                                a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                                || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                                || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                            )
                        && a.UP_MENU_SEQ == prev.UP_MENU_SEQ
                        && a.DEPTH == prev.DEPTH && a.SORD_ORDER > prev.SORD_ORDER).OrderBy(a => a.SORD_ORDER).FirstOrDefault();
                }
                else
                {
                    upDownMenu = db49_wowtv.NTB_MENU.Where(a => a.UP_MENU_SEQ == prev.UP_MENU_SEQ && a.DEL_YN == "N" && a.CHANNEL_CODE == prev.CHANNEL_CODE && a.DEPTH == prev.DEPTH && a.SORD_ORDER > prev.SORD_ORDER).OrderBy(a => a.SORD_ORDER).FirstOrDefault();
                }
            }

            if(upDownMenu != null)
            {
                int upDownOrder = upDownMenu.SORD_ORDER;
                upDownMenu.SORD_ORDER = prev.SORD_ORDER;
                prev.SORD_ORDER = upDownOrder;


                upDownMenu.MOD_ID = loginUser.LoginId;
                upDownMenu.MOD_DATE = DateTime.Now;
                prev.MOD_ID = loginUser.LoginId;
                prev.MOD_DATE = DateTime.Now;

                db49_wowtv.SaveChanges();
            }
        }



        /// <summary>
        /// 픽스된 페이지의 경우 MenuSeq 를 가져와야 하는데 키가 고정이 아니라서 페이지URL 로 조회하는 기능 만듬
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public NTB_MENU GetAtByUrl(string url)
        {
            NTB_MENU model = db49_wowtv.NTB_MENU.FirstOrDefault(a => a.LINK_URL == url);

            return model;
        }
        public NTB_MENU GetAtByName(string channelCode, string name)
        {
            NTB_MENU model = db49_wowtv.NTB_MENU.FirstOrDefault(a => a.CHANNEL_CODE == channelCode && a.MENU_NAME == name);

            return model;
        }




        public Dictionary<string, int?> GetMenuList()
        {
            Dictionary<string, int?> menus = new Dictionary<string, int?>();

            using (Db49_wowtv db49 = new Db49_wowtv())
            {
                var ntb_menu = db49_wowtv.NTB_MENU.AsQueryable();
                var list = ntb_menu.Where(ntbmenu => ntbmenu.DEPTH == 1).ToList();
                foreach (var element in list)
                {
                    menus.Add(element.MENU_NAME, element.CONTENT_SEQ);
                    var childMenus = (GetNTB_MENU_Hierachy(db49.NTB_MENU, element));

                    foreach (var child in childMenus)
                    {
                        menus.Add(element.MENU_NAME + ">" + child.Ntb_Meun.MENU_NAME, element.CONTENT_SEQ);

                        foreach (var menu in child.NTB_MENUs)
                        {
                            menus.Add(element.MENU_NAME + ">" + child.Ntb_Meun.MENU_NAME + ">" + menu.Ntb_Meun.MENU_NAME, element.CONTENT_SEQ);
                        }
                    }

                }
                return menus;
            }
        }

        private class Ntb_MeunsHierarchy
        {
            public NTB_MENU Ntb_Meun { get; set; }
            public IEnumerable<Ntb_MeunsHierarchy> NTB_MENUs { get; set; }
        }

        private IEnumerable<Ntb_MeunsHierarchy> GetNTB_MENU_Hierachy(IEnumerable<NTB_MENU> allNTB_MENUs, NTB_MENU parentNTB_MENU)
        {
            int? parentMENU_SEQ = null;

            if (parentNTB_MENU != null)
                parentMENU_SEQ = parentNTB_MENU.MENU_SEQ;

            List<Ntb_MeunsHierarchy> menuCollection = new List<Ntb_MeunsHierarchy>();

            var childMenus = allNTB_MENUs.Where(x => x.UP_MENU_SEQ == parentMENU_SEQ);
            foreach (NTB_MENU ntb_Menu in childMenus)
                menuCollection.Add(new Ntb_MeunsHierarchy() { Ntb_Meun = ntb_Menu, NTB_MENUs = GetNTB_MENU_Hierachy(allNTB_MENUs, ntb_Menu) });

            return menuCollection;
        }





        public int SaveMig(NTB_MENU model, LoginUser loginUser)
        {
            NTB_MENU upMenu = null;

            NTB_MENU data = GetAt(model.MENU_SEQ);
            if (data == null)
            {
                int sortOrder = 1;
                NTB_MENU maxOrder = null;
                if (model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE)
                {
                    maxOrder = db49_wowtv.NTB_MENU.Where(a =>
                        (
                            a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                            || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                        )
                        && a.UP_MENU_SEQ == model.UP_MENU_SEQ
                        && a.DEPTH == model.DEPTH).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                else if (model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE)
                {
                    maxOrder = db49_wowtv.NTB_MENU.Where(a =>
                        (
                            a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                            || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                        )
                        && a.UP_MENU_SEQ == model.UP_MENU_SEQ
                        && a.DEPTH == model.DEPTH).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                else if (model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE)
                {
                    maxOrder = db49_wowtv.NTB_MENU.Where(a =>
                        (
                            a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                            || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                            || a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                        )
                        && a.UP_MENU_SEQ == model.UP_MENU_SEQ
                        && a.DEPTH == model.DEPTH).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                else
                {
                    maxOrder = db49_wowtv.NTB_MENU.Where(a => a.UP_MENU_SEQ == model.UP_MENU_SEQ && a.CHANNEL_CODE == model.CHANNEL_CODE && a.DEPTH == model.DEPTH).OrderByDescending(a => a.SORD_ORDER).FirstOrDefault();
                }
                if (maxOrder != null)
                {
                    sortOrder = maxOrder.SORD_ORDER + 1;
                }
                model.SORD_ORDER = sortOrder;

                switch (model.DEPTH)
                {
                    case 1:
                        model.MENU_NAME_DEPTH_1 = model.MENU_NAME;
                        break;
                    case 2:
                        model.MENU_NAME_DEPTH_2 = model.MENU_NAME;
                        upMenu = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == model.UP_MENU_SEQ);
                        model.MENU_NAME_DEPTH_1 = upMenu.MENU_NAME;
                        break;
                    case 3:
                        model.MENU_NAME_DEPTH_3 = model.MENU_NAME;
                        upMenu = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == model.UP_MENU_SEQ);
                        model.MENU_NAME_DEPTH_2 = upMenu.MENU_NAME;
                        upMenu = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == upMenu.UP_MENU_SEQ);
                        model.MENU_NAME_DEPTH_1 = upMenu.MENU_NAME;
                        break;
                }



                // 방송쪽 메뉴라면
                if (model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                    || model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                    || model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE)
                {
                    // 코너일경우
                    if (model.CONTENT_TYPE_CODE == "Corner")
                    {
                    }
                }

                model.DEL_YN = "N";
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;
                db49_wowtv.NTB_MENU.Add(model);
            }
            else
            {
                //data.CHANNEL_CODE = model.CHANNEL_CODE;
                data.UP_MENU_SEQ = model.UP_MENU_SEQ;
                data.DEPTH = model.DEPTH;
                //data.MENU_SEQ_DEPTH1 = model.MENU_SEQ_DEPTH1;
                //data.MENU_SEQ_DEPTH2 = model.MENU_SEQ_DEPTH2;
                //data.MENU_SEQ_DEPTH3 = model.MENU_SEQ_DEPTH3;
                //data.SORD_ORDER = model.SORD_ORDER;
                data.MENU_NAME = model.MENU_NAME;
                data.CONTENT_TYPE_CODE = model.CONTENT_TYPE_CODE;
                data.CONTENT_SEQ = model.CONTENT_SEQ;
                data.LINK_TYPE_CODE = model.LINK_TYPE_CODE;
                data.LINK_URL = model.LINK_URL;
                data.ACTIVE_YN = model.ACTIVE_YN;
                data.REMARK = model.REMARK;

                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();

            // 2뎁스일 경우
            if (model.DEPTH == 2)
            {
                // 방송쪽 메뉴라면
                if (model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_ADMIN_CHANNEL_CODE
                    || model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_FRONT_CHANNEL_CODE
                    || model.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE)
                {
                    upMenu = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == model.UP_MENU_SEQ);
                    if (upMenu != null)
                    {
                        //upMenu.CONTENT_TYPE_CODE = "Page";
                        upMenu.CONTENT_SEQ = null;
                        upMenu.LINK_URL = "javascript:void(0);";
                        db49_wowtv.SaveChanges();
                    }
                }
            }
            return model.MENU_SEQ;
        }



    }
}
