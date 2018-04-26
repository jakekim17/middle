using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Group;

using Wow.Tv.Middle.Biz.Menu;

namespace Wow.Tv.Middle.Biz.Group
{
    public class GroupBiz : BaseBiz
    {

        public ListModel<NTB_GROUP> SearchList(GroupCondition condition)
        {
            ListModel<NTB_GROUP> resultData = new ListModel<NTB_GROUP>();

            var list = db49_wowtv.NTB_GROUP.Where(a => a.DEL_YN == "N");


            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                if (condition.SearchType == "Name")
                {
                    list = list.Where(a => a.GROUP_NAME.Contains(condition.SearchText) == true);
                }
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.GROUP_SEQ);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();


            return resultData;
        }



        public NTB_GROUP GetAt(int groupSeq)
        {
            return db49_wowtv.NTB_GROUP.SingleOrDefault(a => a.GROUP_SEQ == groupSeq);
        }



        public int Save(NTB_GROUP model, LoginUser loginUser)
        {
            NTB_GROUP data = GetAt(model.GROUP_SEQ);
            if (data == null)
            {
                model.DEL_YN = "N";
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;
                db49_wowtv.NTB_GROUP.Add(model);
            }
            else
            {
                data.GROUP_NAME = model.GROUP_NAME;
                data.USE_YN = model.USE_YN;

                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();

            return model.GROUP_SEQ;
        }


        public void Delete(int groupSeq, LoginUser loginUser)
        {
            NTB_GROUP data = GetAt(groupSeq);
            if (data != null)
            {
                //db49_wowtv.NTB_GROUP.Remove(data);
                data.DEL_YN = "Y";
                db49_wowtv.SaveChanges();
            }
        }


        public void Copy(int groupSeq, LoginUser loginUser)
        {
            NTB_GROUP copyGroup = new NTB_GROUP();
            var org = GetAt(groupSeq);

            copyGroup.GROUP_NAME = org.GROUP_NAME + " 복사본";
            copyGroup.USE_YN = org.USE_YN;

            Save(copyGroup, loginUser);



            MenuGroupBiz menuGroupBiz = new MenuGroupBiz();
            var adminGroupList = db49_wowtv.NTB_MENU_GROUP.Where(a => a.GROUP_SEQ == org.GROUP_SEQ);

            foreach (var item in adminGroupList)
            {
                NTB_MENU_GROUP menuGroup = new NTB_MENU_GROUP();
                menuGroup.GROUP_SEQ = copyGroup.GROUP_SEQ;
                menuGroup.MENU_SEQ = item.MENU_SEQ;
                menuGroup.READ_YN = item.READ_YN;
                menuGroup.WRITE_YN = item.WRITE_YN;
                menuGroup.DEL_YN = item.DEL_YN;

                menuGroupBiz.Save(menuGroup, loginUser);
            }

        }


        #region Test

        public List<CCC> B(int seq)
        {
            var cc = db49_wowtv.CCC.Where(a => a.A1 == seq);
            return cc.ToList();
        }

        public CCC BB(int seq)
        {
            var cc = db49_wowtv.CCC.SingleOrDefault(a => a.C1 == seq);
            return cc;
        }


        public AAA BBB(int seq)
        {
            var aa = db49_wowtv.AAA.SingleOrDefault(a => a.A1 == seq);
            return aa;
        }

        public My BBBB(int seq)
        {
            var aa = BBB(seq);
            var cc = BB(3);

            My myClass = new My();
            myClass.y = new You();
            myClass.y.y1 = "SDFSDF";
            myClass.aClass = aa;
            //myClass.cClass = db49_wowtv.TAB_BOARD.SingleOrDefault(a => a.SEQ == 3);

            return myClass;
        }

        #endregion

    }
}
