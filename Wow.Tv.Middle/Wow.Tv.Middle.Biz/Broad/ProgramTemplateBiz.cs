using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Broad;
using Wow.Tv.Middle.Model.Db90.DNRS;

namespace Wow.Tv.Middle.Biz.Broad
{
    public class ProgramTemplateBiz : BaseBiz
    {
        public NTB_PROGRAM_TEMPLATE GetAt(string programCode)
        {
            var model = db49_wowtv.NTB_PROGRAM_TEMPLATE.SingleOrDefault(a => a.PRG_CD == programCode);
            
            if (model != null)
            {
                model.ProgramName = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == model.PRG_CD).PRG_NM;
                model.MainAttachFile = new AttachFile.AttachFileBiz().GetAt("NTB_PROGRAM_TEMPLATE", model.PROGRAM_TEMPLATE_SEQ.ToString());
            }

            return model;
        }


        public int Save(NTB_PROGRAM_TEMPLATE model, T_NEWS_PRG newsProgram, LoginUser loginUser)
        {
            var prev = GetAt(model.PRG_CD);

            model.WOW_YN = (String.IsNullOrEmpty(model.WOW_YN) == true ? "N" : model.WOW_YN);
            model.NAVER_CAFE_YN = (String.IsNullOrEmpty(model.NAVER_CAFE_YN) == true ? "N" : model.NAVER_CAFE_YN);
            model.WOWNET_YN = (String.IsNullOrEmpty(model.WOWNET_YN) == true ? "N" : model.WOWNET_YN);
            model.WOW_CAFE_YN = (String.IsNullOrEmpty(model.WOW_CAFE_YN) == true ? "N" : model.WOW_CAFE_YN);
            model.NAVER_READ_YN = (String.IsNullOrEmpty(model.NAVER_READ_YN) == true ? "N" : model.NAVER_READ_YN);
            model.YOUTUBE_READ_YN = (String.IsNullOrEmpty(model.YOUTUBE_READ_YN) == true ? "N" : model.YOUTUBE_READ_YN);


            if (prev == null)
            {
                model.DEL_YN = "N";

                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;

                db49_wowtv.NTB_PROGRAM_TEMPLATE.Add(model);
            }
            else
            {
                prev.DEL_YN = "N";

                prev.TEMPLATE_TYPE = model.TEMPLATE_TYPE;
                prev.WOW_YN = model.WOW_YN;
                prev.WOW_URL = model.WOW_URL;
                prev.NAVER_CAFE_YN = model.NAVER_CAFE_YN;
                prev.NAVER_CAFE_URL = model.NAVER_CAFE_URL;
                prev.WOWNET_YN = model.WOWNET_YN;
                prev.WOWNET_URL = model.WOWNET_URL;
                prev.WOW_CAFE_YN = model.WOW_CAFE_YN;
                prev.WOW_CAFE_URL = model.WOW_CAFE_URL;
                prev.NAVER_READ_YN = model.NAVER_READ_YN;
                prev.NAVER_READ_URL = model.NAVER_READ_URL;
                prev.YOUTUBE_READ_YN = model.YOUTUBE_READ_YN;
                prev.YOUTUBE_READ_URL = model.YOUTUBE_READ_URL;
                prev.REMARK = model.REMARK;

                prev.MOD_ID = loginUser.LoginId;
                prev.MOD_DATE = DateTime.Now;
            }

            db49_wowtv.SaveChanges();


            var prevNewsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == model.PRG_CD);
            if(prevNewsProgram == null)
            {
                prevNewsProgram = new NTB_NEWS_PROGRAM();
                prevNewsProgram.PRG_CD = model.PRG_CD;
                prevNewsProgram.MAIN_VIEW_TYPE = newsProgram.MainViewType;
                prevNewsProgram.MAIN_BOTTOM_VIEW_YN = newsProgram.MainBottomViewYn;
                prevNewsProgram.ALL_PROGRAM_VIEW_YN = newsProgram.AllProgramViewYn;

                db90_DNRS.NTB_NEWS_PROGRAM.Add(prevNewsProgram);
            }
            else
            {
                prevNewsProgram.PRG_CD = model.PRG_CD;
                prevNewsProgram.MAIN_VIEW_TYPE = newsProgram.MainViewType;
                prevNewsProgram.MAIN_BOTTOM_VIEW_YN = newsProgram.MainBottomViewYn;
                prevNewsProgram.ALL_PROGRAM_VIEW_YN = newsProgram.AllProgramViewYn;
            }
            db90_DNRS.SaveChanges();

            //if (model.MainAttachFile != null)
            //{
            //    model.MainAttachFile.TABLE_CODE = "NTB_PROGRAM_TEMPLATE";
            //    model.MainAttachFile.TABLE_KEY = model.PROGRAM_TEMPLATE_SEQ.ToString();

            //    new AttachFile.AttachFileBiz().Create(model.MainAttachFile);
            //}

            if (model.MainAttachFile != null)
            {
                model.MainAttachFile.TABLE_CODE = "T_NEWS_PRG-MAIN";
                model.MainAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(model.MainAttachFile);
            }
            if (model.SubAttachFile != null)
            {
                model.SubAttachFile.TABLE_CODE = "T_NEWS_PRG-SUB";
                model.SubAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(model.SubAttachFile);
            }
            if (model.RectangleAttachFile != null)
            {
                model.RectangleAttachFile.TABLE_CODE = "T_NEWS_PRG-RECTANGLE";
                model.RectangleAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(model.RectangleAttachFile);
            }
            if (model.ThumbnailAttachFile != null)
            {
                model.ThumbnailAttachFile.TABLE_CODE = "T_NEWS_PRG-THUMBNAIL";
                model.ThumbnailAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(model.ThumbnailAttachFile);
            }


            return model.PROGRAM_TEMPLATE_SEQ;
        }

        public void Delete(string programCode)
        {
            var prev = GetAt(programCode);

            prev.TEMPLATE_TYPE = "A";
            prev.WOW_YN = "";
            prev.WOW_URL = "";
            prev.NAVER_CAFE_YN = "";
            prev.NAVER_CAFE_URL = "";
            prev.WOWNET_YN = "";
            prev.WOWNET_URL = "";
            prev.WOW_CAFE_YN = "";
            prev.WOW_CAFE_URL = "";
            prev.NAVER_READ_YN = "";
            prev.NAVER_READ_URL = "";
            prev.YOUTUBE_READ_YN = "";
            prev.YOUTUBE_READ_URL = "";
            prev.REMARK = "";
            prev.DEL_YN = "Y";

            db49_wowtv.SaveChanges();
        }


        #region 파트너

        //public List<NTB_PROGRAM_TEMPLATE_PARTNER> GetPartnerList(int programTemplateSeq)
        //{
        //    return db49_wowtv.NTB_PROGRAM_TEMPLATE_PARTNER.Where(a => a.PROGRAM_TEMPLATE_SEQ == programTemplateSeq).OrderBy(a => a.PARTNER_NAME).ToList();
        //}

        //public void AddPartner(int programTemplateSeq, int payNo, LoginUser loginUser)
        //{
        //    string partnerName = "";
        //    var partner = db49_broadcast.Pro_wowList.SingleOrDefault(a => a.Pay_no == payNo);
        //    if (partner != null)
        //    {
        //        partnerName = partner.FullName;
        //    }

        //    NTB_PROGRAM_TEMPLATE_PARTNER model = new NTB_PROGRAM_TEMPLATE_PARTNER();
        //    model.PROGRAM_TEMPLATE_SEQ = programTemplateSeq;
        //    model.pay_no = payNo;
        //    model.PARTNER_NAME = partnerName;

        //    model.REG_DATE = DateTime.Now;
        //    model.REG_ID = loginUser.LoginId;
        //    model.MOD_DATE = DateTime.Now;
        //    model.MOD_ID = loginUser.LoginId;

        //    db49_wowtv.NTB_PROGRAM_TEMPLATE_PARTNER.Add(model);
        //    db49_wowtv.SaveChanges();
        //}


        //public void DeletePartner(int programTemplateSeq, int payNo)
        //{
        //    var prev = db49_wowtv.NTB_PROGRAM_TEMPLATE_PARTNER.SingleOrDefault(a => a.PROGRAM_TEMPLATE_SEQ == programTemplateSeq && a.pay_no == payNo);

        //    db49_wowtv.NTB_PROGRAM_TEMPLATE_PARTNER.Remove(prev);
        //    db49_wowtv.SaveChanges();
        //}

        //public void DeletePartnerList(int programTemplateSeq)
        //{
        //    var prevList = db49_wowtv.NTB_PROGRAM_TEMPLATE_PARTNER.Where(a => a.PROGRAM_TEMPLATE_SEQ == programTemplateSeq).ToList();

        //    foreach (var item in prevList)
        //    {
        //        DeletePartner(item.PROGRAM_TEMPLATE_SEQ, item.pay_no);
        //    }
        //}



        #endregion




        #region 그룹 매핑


        public NTB_PROGRAM_GROUP GetGroupFirst(int programTemplateSeq)
        {
            NTB_PROGRAM_GROUP model = new NTB_PROGRAM_GROUP();
            var item = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Where(a => a.PROGRAM_TEMPLATE_SEQ == programTemplateSeq).FirstOrDefault();
            if(item != null)
            {
                model = db49_wowtv.NTB_PROGRAM_GROUP.SingleOrDefault(a => a.PROGRAM_GROUP_SEQ == item.PROGRAM_GROUP_SEQ);
            }

            return model;
        }

        public List<NTB_PROGRAM_GROUP> GetGroupList(int programTemplateSeq)
        {
            return db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Where(a => a.PROGRAM_TEMPLATE_SEQ == programTemplateSeq).OrderBy(a => a.ORDER_INDEX).Select(a => a.NTB_PROGRAM_GROUP).ToList();
        }


        public List<NTB_PROGRAM_TEMPLATE_GROUP> GetGroupListByGroupSeq(int programGroupSeq)
        {
            List<NTB_PROGRAM_TEMPLATE_GROUP> model = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Where(a => a.PROGRAM_GROUP_SEQ == programGroupSeq).OrderBy(a => a.ORDER_INDEX).ToList();

            foreach(var item in model)
            {
                string programCode = db49_wowtv.NTB_PROGRAM_TEMPLATE.SingleOrDefault(a => a.PROGRAM_TEMPLATE_SEQ == item.PROGRAM_TEMPLATE_SEQ).PRG_CD;
                T_NEWS_PRG newsProgram = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == programCode);
                if (newsProgram != null)
                {
                    item.ProgramCode = newsProgram.PRG_CD;
                    item.ProgramName = newsProgram.PRG_NM;
                }
                item.NTB_PROGRAM_GROUP = null;
                item.NTB_PROGRAM_TEMPLATE = null;
            }

            return model;
        }

        public void AddGroup(int programTemplateSeq, int programGroupSeq, LoginUser loginUser)
        {
            NTB_PROGRAM_TEMPLATE_GROUP model = new NTB_PROGRAM_TEMPLATE_GROUP();
            model.PROGRAM_TEMPLATE_SEQ = programTemplateSeq;
            model.PROGRAM_GROUP_SEQ = programGroupSeq;

            model.REG_DATE = DateTime.Now;
            model.REG_ID = loginUser.LoginId;

            int orderIndex = 0;
            var maxOrder = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Where(a => a.PROGRAM_GROUP_SEQ == model.PROGRAM_GROUP_SEQ).OrderByDescending(a => a.ORDER_INDEX).FirstOrDefault();
            if(maxOrder != null)
            {
                orderIndex = maxOrder.ORDER_INDEX + 1;
            }
            model.ORDER_INDEX = orderIndex;

            db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Add(model);
            db49_wowtv.SaveChanges();
        }


        public void DeleteGroup(int programTemplateSeq, int programGroupSeq)
        {
            var prev = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.SingleOrDefault(a => a.PROGRAM_TEMPLATE_SEQ == programTemplateSeq && a.PROGRAM_GROUP_SEQ == programGroupSeq);

            db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Remove(prev);
            db49_wowtv.SaveChanges();
        }


        public void DeleteGroupList(int programTemplateSeq)
        {
            var prevList = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Where(a => a.PROGRAM_TEMPLATE_SEQ == programTemplateSeq).ToList();

            foreach (var item in prevList)
            {
                DeleteGroup(item.PROGRAM_TEMPLATE_SEQ, item.PROGRAM_GROUP_SEQ);
            }
        }




        public void UpDownGroup(int programTemplateSeq, int programGroupSeq, bool isUp)
        {
            NTB_PROGRAM_TEMPLATE_GROUP upDown = null;

            var prev = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.SingleOrDefault(a =>
                a.PROGRAM_TEMPLATE_SEQ == programTemplateSeq
                && a.PROGRAM_GROUP_SEQ == programGroupSeq);

            if(isUp == true)
            {
                upDown = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Where(a =>a.PROGRAM_GROUP_SEQ == programGroupSeq && a.ORDER_INDEX < prev.ORDER_INDEX).OrderByDescending(a => a.ORDER_INDEX).FirstOrDefault();
            }
            else
            {
                upDown = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Where(a => a.PROGRAM_GROUP_SEQ == programGroupSeq && a.ORDER_INDEX > prev.ORDER_INDEX).OrderBy(a => a.ORDER_INDEX).FirstOrDefault();
            }

            if(upDown != null)
            {
                int tempOrder = upDown.ORDER_INDEX;
                upDown.ORDER_INDEX = prev.ORDER_INDEX;
                prev.ORDER_INDEX = tempOrder;

                db49_wowtv.SaveChanges();
            }

        }



        #endregion




        #region 프로그램 홈 관련

        /// <summary>
        /// 내 프로그램의 패밀리 목록을 조회
        /// </summary>
        /// <param name="programCode"></param>
        /// <returns></returns>
        public List<T_NEWS_PRG> GetFamilyList(string programCode)
        {
            List<T_NEWS_PRG> model = new List<T_NEWS_PRG>();

            NTB_PROGRAM_TEMPLATE programTemplate = GetAt(programCode);
            if (programTemplate == null || programTemplate.DEL_YN == "Y")
            {
            }
            else
            {
                // 맵핑테이블 조회
                var programTemplateGroup = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.FirstOrDefault(a => a.PROGRAM_TEMPLATE_SEQ == programTemplate.PROGRAM_TEMPLATE_SEQ);
                if (programTemplateGroup != null)
                {
                    // 그룹정보 조회
                    var programGroup = db49_wowtv.NTB_PROGRAM_GROUP.FirstOrDefault(a => a.PROGRAM_GROUP_SEQ == programTemplateGroup.PROGRAM_GROUP_SEQ/* && a.TYPE_CODE == "Union"*/);

                    if (programGroup != null)
                    {
                        // 그룹에 해당하는 넘들(그룹매핑)을 조회하고
                        var list = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Where(a => a.PROGRAM_GROUP_SEQ == programGroup.PROGRAM_GROUP_SEQ);
                        list = list.OrderBy(a => a.ORDER_INDEX);
                        List<int> programTemplateList = list.Select(a => a.PROGRAM_TEMPLATE_SEQ).ToList();

                        // 그룹에 해당하는 넘들(템플릿)을 조회
                        List<string> programCodeList = db49_wowtv.NTB_PROGRAM_TEMPLATE.Where(a => a.DEL_YN == "N" && programTemplateList.Contains(a.PROGRAM_TEMPLATE_SEQ) == true).Select(a => a.PRG_CD).ToList();

                        model = db90_DNRS.T_NEWS_PRG.Where(a => programCodeList.Contains(a.PRG_CD) == true).ToList();
                    }
                }
            }

            return model;
        }


        /// <summary>
        /// 내 프로그램의 자식 목록을 조회
        /// </summary>
        /// <param name="programCode"></param>
        /// <returns></returns>
        public List<T_NEWS_PRG> GetChildList(string programCode)
        {
            List<T_NEWS_PRG> model = new List<T_NEWS_PRG>();
            var programGroup = db49_wowtv.NTB_PROGRAM_GROUP.FirstOrDefault(a => a.MASTER_PRG_CD == programCode);
            if (programGroup != null)
            {
                var list = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Where(a => a.PROGRAM_GROUP_SEQ == programGroup.PROGRAM_GROUP_SEQ);
                list = list.OrderBy(a => a.ORDER_INDEX);
                List<int> programTemplateList = list.Select(a => a.PROGRAM_TEMPLATE_SEQ).ToList();

                // 그룹에 해당하는 넘들(템플릿)을 조회
                List<string> programCodeList = db49_wowtv.NTB_PROGRAM_TEMPLATE.Where(a => a.DEL_YN == "N" && programTemplateList.Contains(a.PROGRAM_TEMPLATE_SEQ) == true).Select(a => a.PRG_CD).ToList();

                model = db90_DNRS.T_NEWS_PRG.Where(a => programCodeList.Contains(a.PRG_CD) == true).ToList();
            }
            
            return model;
        }


        /// <summary>
        /// 일반형인지(Normal) 그룹형인지(Group) 연합형인지(Union)  Group
        /// </summary>
        /// <param name="programCode"></param>
        /// <returns></returns>
        public string GetProgramType(string programCode)
        {
            string result = "Normal";

            List<T_NEWS_PRG> model = new List<T_NEWS_PRG>();
            
            // 그룹정보 조회
            var programGroup = db49_wowtv.NTB_PROGRAM_GROUP.FirstOrDefault(a => a.DEL_YN == "N" && a.MASTER_PRG_CD == programCode);

            if (programGroup != null)
            {
                result = programGroup.TYPE_CODE;
            }

            return result;
        }


        #endregion


    }
}
