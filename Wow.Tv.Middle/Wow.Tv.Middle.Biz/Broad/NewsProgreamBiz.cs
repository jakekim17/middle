using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.editVOD;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db90.DNRS;
using Wow.Tv.Middle.Model.Db90.DNRS.NewsProgram;
using Wow.Tv.Middle.Model.Db123.WOW4989;
using Wow.Tv.Middle.Model.Db16.wowfa;

using Wow.Tv.Middle.Biz.AttachFile;

namespace Wow.Tv.Middle.Biz.Broad
{
    public class NewsProgreamBiz : BaseBiz
    {
        public const string TableCode = "BroadSchedule";
        public const string TableKey = "OnlyOne";

        public ListModel<TAB_CODE> GetCategoryList(int pseq)
        {
            ListModel<TAB_CODE> model = new ListModel<TAB_CODE>();

            var list = db49_wowtv.TAB_CODE.Where(a => a.PSEQ == pseq).OrderBy(a => a.DISP_ORDER);

            model.ListData = list.ToList();

            return model;
        }

        public T_NEWS_PRG GetAt(string programCode)
        {
            T_NEWS_PRG model = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == programCode);

            model.ImgSchedule = db90_DNRS.IMG_SCHEDULE.FirstOrDefault(a => a.prog_id == model.PRG_CD);
            if (model.ImgSchedule == null)
            {
                model.ImgSchedule = new IMG_SCHEDULE();
            }

            model.ProgramList = db90_DNRS.TAB_PROGRAM_LIST.FirstOrDefault(a => a.PGM_ID == model.PRG_CD);
            if (model.ProgramList == null)
            {
                model.ProgramList = new TAB_PROGRAM_LIST();
            }
            else
            {
                var category1 = db49_wowtv.TAB_CODE.SingleOrDefault(a => a.MSEQ == model.ProgramList.CATE_CODE);
                if (category1 != null)
                {
                    model.CategoryCode1 = category1.MSEQ.ToString();

                    if (category1.PSEQ > 174)
                    {
                        var category2 = db49_wowtv.TAB_CODE.SingleOrDefault(a => a.MSEQ == category1.PSEQ);
                        if (category2 != null)
                        {
                            model.CategoryCode1 = category2.MSEQ.ToString();
                            model.CategoryCode2 = category1.MSEQ.ToString();

                            if (category2.PSEQ > 174)
                            {
                                var category3 = db49_wowtv.TAB_CODE.SingleOrDefault(a => a.MSEQ == category2.PSEQ);
                                if (category3 != null)
                                {
                                    model.CategoryCode1 = category3.MSEQ.ToString();
                                    model.CategoryCode2 = category2.MSEQ.ToString();
                                    model.CategoryCode3 = category1.MSEQ.ToString();
                                }
                            }
                        }
                    }
                }
            }


            ProgramTemplateBiz programTemplateBiz = new ProgramTemplateBiz();
            var programTemplate = programTemplateBiz.GetAt(model.PRG_CD);
            if (programTemplate == null)
            {
            }
            else
            {
                if (programTemplate.DEL_YN == "N")
                {
                    model.ProgramTemplateType = programTemplate.TEMPLATE_TYPE;
                    var programGroup = programTemplateBiz.GetGroupFirst(programTemplate.PROGRAM_TEMPLATE_SEQ);
                    if (programGroup != null)
                    {
                        model.ProgramTemplateName = programGroup.GROUP_NAME;
                    }
                }
            }

            var newsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == model.PRG_CD);
            if (newsProgram != null)
            {
                model.SERVICE_NAME = newsProgram.SERVICE_NAME;
                model.ENCODING = newsProgram.ENCODING;
                model.PUBLISH_YN = newsProgram.PUBLISH_YN;
                model.CP_NM = newsProgram.CP_NM;
                model.PLAN_BROAD = newsProgram.PLAN_BROAD;
                model.BroadTypeCode = newsProgram.BROAD_TYPE_CODE;
                model.FameYn = newsProgram.FAME_YN;
                model.FirstFreeYn = newsProgram.FIRST_FREE_YN;
                model.MainViewType = newsProgram.MAIN_VIEW_TYPE;
                model.MainBottomViewYn = newsProgram.MAIN_BOTTOM_VIEW_YN;
                model.AllProgramViewYn = newsProgram.ALL_PROGRAM_VIEW_YN;
                model.BroadSectionCode = newsProgram.BROAD_SECTION_CODE;


                if (newsProgram.PLAN_BROAD == "FIRST")
                {
                    model.IsFirst = true;
                }
                if (newsProgram.PLAN_BROAD == "OPEN")
                {
                    model.IsRenewal = true;
                }


                model.DayOfWeekString = new List<string>();
                if (model.IsMonday == true)
                {
                    model.DayOfWeekString.Add("월");
                }
                if (model.IsTuesday == true)
                {
                    model.DayOfWeekString.Add("화");
                }
                if (model.IsWednesday == true)
                {
                    model.DayOfWeekString.Add("수");
                }
                if (model.IsThursday == true)
                {
                    model.DayOfWeekString.Add("목");
                }
                if (model.IsFriday == true)
                {
                    model.DayOfWeekString.Add("금");
                }
                if (model.IsSaturday == true)
                {
                    model.DayOfWeekString.Add("토");
                }
                if (model.IsSunday == true)
                {
                    model.DayOfWeekString.Add("일");
                }



                model.PartnerNameList = new List<string>();
                var partnerList = db49_wowtv.NTB_PROGRAM_PARTNER.Where(a => a.PRG_CD == model.PRG_CD);
                foreach (var item in partnerList)
                {
                    model.PartnerNameList.Add(item.PARTNER_NAME);
                }

                
            }



            return model;
        }


        public IMG_SCHEDULE GetAtImgSchedule(string programCode)
        {
            IMG_SCHEDULE model = db90_DNRS.IMG_SCHEDULE.SingleOrDefault(a => a.prog_id == programCode);

            if (model != null)
            {
                model.ProgramList = db90_DNRS.TAB_PROGRAM_LIST.FirstOrDefault(a => a.PGM_ID == model.prog_id);
                if (model.ProgramList == null)
                {
                    model.ProgramList = new TAB_PROGRAM_LIST();
                }


                ProgramTemplateBiz programTemplateBiz = new ProgramTemplateBiz();
                var programTemplate = programTemplateBiz.GetAt(model.prog_id);
                if (programTemplate == null)
                {
                }
                else
                {
                    if (programTemplate.DEL_YN == "N")
                    {
                        model.ProgramTemplateType = programTemplate.TEMPLATE_TYPE;
                        var programGroup = programTemplateBiz.GetGroupFirst(programTemplate.PROGRAM_TEMPLATE_SEQ);
                        if (programGroup != null)
                        {
                            model.ProgramTemplateName = programGroup.GROUP_NAME;
                        }
                    }
                }

                var newsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == model.prog_id);
                if (newsProgram != null)
                {
                    model.SERVICE_NAME = newsProgram.SERVICE_NAME;
                    model.ENCODING = newsProgram.ENCODING;
                    model.PUBLISH_YN = newsProgram.PUBLISH_YN;
                    model.CP_NM = newsProgram.CP_NM;
                    model.PLAN_BROAD = newsProgram.PLAN_BROAD;
                }
            }

            return model;
        }



        public NTB_ATTACH_FILE GetMainAttachFile(string programCode)
        {
            return new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-MAIN", programCode);
        }


        public NTB_ATTACH_FILE GetSubAttachFile(string programCode)
        {
            return new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-SUB", programCode);
        }


        public NTB_ATTACH_FILE GetRectangleAttachFile(string programCode)
        {
            return new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-RECTANGLE", programCode);
        }


        public NTB_ATTACH_FILE GetThumbnailAttachFile(string programCode)
        {
            return new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-THUMBNAIL", programCode);
        }


        public ListModel<T_NEWS_PRG> SearchList(NewsProgramCondition condition)
        {
            ListModel<T_NEWS_PRG> resultData = new ListModel<T_NEWS_PRG>();

            var list = db90_DNRS.T_NEWS_PRG.Where(a => a.DELFLAG == "0");

            if (condition.IsMonday == true)
            {
                list = list.Where(a => (a.PGMDAY & 1) > 0);
            }
            if (condition.IsTuesday == true)
            {
                list = list.Where(a => (a.PGMDAY & 16) > 0);
            }
            if (condition.IsWednesday == true)
            {
                list = list.Where(a => (a.PGMDAY & 256) > 0);
            }
            if (condition.IsThursday == true)
            {
                list = list.Where(a => (a.PGMDAY & 4096) > 0);
            }
            if (condition.IsFriday == true)
            {
                list = list.Where(a => (a.PGMDAY & 65536) > 0);
            }
            if (condition.IsSaturday == true)
            {
                list = list.Where(a => (a.PGMDAY & 1048576) > 0);
            }
            if (condition.IsSunday == true)
            {
                list = list.Where(a => (a.PGMDAY & 16777216) > 0);
            }



            if (String.IsNullOrEmpty(condition.ProgramName) == false)
            {
                list = list.Where(a => a.PRG_NM.Contains(condition.ProgramName) == true);
            }

            if (String.IsNullOrEmpty(condition.ProgramNameTermStart) == false && String.IsNullOrEmpty(condition.ProgramNameTermEnd) == false)
            {
                if (condition.ProgramNameTermStart == "Etc")
                {
                    list = list.Where(a => a.PRG_NM.CompareTo("ㄱ") < 0 || a.PRG_NM.CompareTo("힣") >= 0);
                }
                else
                {
                    list = list.Where(a => a.PRG_NM.CompareTo(condition.ProgramNameTermStart) >= 0);
                    list = list.Where(a => a.PRG_NM.CompareTo(condition.ProgramNameTermEnd) < 0);
                }
            }

            if (String.IsNullOrEmpty(condition.PublishYn) == false)
            {
                if(condition.PublishYn == "N")
                {
                    list = list.Where(a =>
                        db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.PUBLISH_YN == "Y").Count() == 0
                    );
                }
                else
                {
                    list = list.Where(a =>
                        db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.PUBLISH_YN == "Y").Count() > 0
                    );
                }
            }

            if (String.IsNullOrEmpty(condition.PointYn) == false)
            {
                if (condition.PointYn == "Y")
                {
                    list = list.Where(a => db90_DNRS.IMG_SCHEDULE.Where(b => b.prog_id == a.PRG_CD && b.point > 0).Count() > 0);
                }
                else
                {
                    list = list.Where(a => db90_DNRS.IMG_SCHEDULE.Where(b => b.prog_id == a.PRG_CD && b.point > 0).Count() == 0);
                }
            }

            if (String.IsNullOrEmpty(condition.BroadTypeCode) == false)
            {
                list = list.Where(a =>
                        db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.BROAD_TYPE_CODE == condition.BroadTypeCode).Count() > 0
                );

                //switch (condition.BroadTypeCode)
                //{
                //    // 장중
                //    case "Ing":
                //        list = list.Where(a => a.BRO_START.CompareTo("09:00") >= 0 && a.BRO_START.CompareTo("17:30") <= 0);
                //        break;
                //        // 장후
                //    case "After":
                //        list = list.Where(a => a.BRO_START.CompareTo("09:00") < 0 || a.BRO_START.CompareTo("17:30") > 0);
                //        break;
                //        // 특집
                //    case "Special":
                //        list = list.Where(a => db90_DNRS.TAB_PROGRAM_LIST.Where(b => b.PGM_ID == a.PRG_CD && b.SPC_YN == "Y").Count() > 0);
                //        break;
                //}
            }


            if (String.IsNullOrEmpty(condition.IngYn) == false)
            {
                if (condition.IngYn == "Y")
                {
                    list = list.Where(a => db90_DNRS.TAB_PROGRAM_LIST.Where(b => b.PGM_ID == a.PRG_CD && b.ING_YN == "Y").Count() > 0);
                }
                else
                {
                    list = list.Where(a => db90_DNRS.TAB_PROGRAM_LIST.Where(b => b.PGM_ID == a.PRG_CD && b.ING_YN == "Y").Count() == 0);
                }
            }

            if (condition.CategoryCode > 0)
            {
                list = list.Where(a => db90_DNRS.TAB_PROGRAM_LIST.Where(b => b.PGM_ID == a.PRG_CD && b.CATE_CODE == condition.CategoryCode).Count() > 0);
            }

            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                switch (condition.SearchType)
                {
                    case "ProgramCd":
                        list = list.Where(a => a.PRG_CD.Contains(condition.SearchText) == true);
                        break;
                    case "ProgramName":
                        list = list.Where(a => a.PRG_NM.Contains(condition.SearchText) == true);
                        break;
                    case "PdName":
                        list = list.Where(a => a.PD_NM.Contains(condition.SearchText) == true);
                        break;
                }
            }


            if(condition.AdminSeq > 0)
            {
                List<string> programCodeList = GetAdminProgram(condition.AdminSeq);
                list = list.Where(a => programCodeList.Contains(a.PRG_CD));
            }



            if (String.IsNullOrEmpty(condition.FameYn) == false)
            {
                if (condition.FameYn == "Y")
                {
                    list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.FAME_YN == "Y").Count() > 0);
                }
                else
                {
                    list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.FAME_YN == "Y").Count() == 0);
                }
            }

            if(String.IsNullOrEmpty(condition.MainBottomViewYn) == false)
            {
                // 노출이 비노출로 바뀌어서 YN 이 반대임 -_-
                if (condition.MainBottomViewYn == "Y")
                {
                    list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.MAIN_BOTTOM_VIEW_YN == "Y").Count() == 0);
                }
                else
                {
                    list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.MAIN_BOTTOM_VIEW_YN == "Y").Count() > 0);
                }
            }

            if (String.IsNullOrEmpty(condition.AllProgramViewYn) == false)
            {
                if (condition.AllProgramViewYn == "Y")
                {
                    list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.ALL_PROGRAM_VIEW_YN == "N").Count() == 0);
                }
                else
                {
                    list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.ALL_PROGRAM_VIEW_YN == "N").Count() > 0);
                }
            }

            if (String.IsNullOrEmpty(condition.Year) == false)
            {
                list = list.Where(a => db90_DNRS.tv_program.Where(b => b.Dep == a.PRG_CD).OrderByDescending(b => b.broad_date).FirstOrDefault().broad_date.Substring(0, 4) == condition.Year);
            }


            if (String.IsNullOrEmpty(condition.BroadSectionCode) == false)
            {
                list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.BROAD_SECTION_CODE == condition.BroadSectionCode).Count() > 0);
            }


            resultData.TotalDataCount = list.Count();

            list = list.OrderBy(a => db90_DNRS.IMG_SCHEDULE.FirstOrDefault(b => b.prog_id == a.PRG_CD && String.IsNullOrEmpty(b.parentid.Replace(" ", "")) == false).parentid).ThenBy(a => a.PRG_CD);
            //list = list.OrderByDescending(a => a.PRG_CD);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();

            foreach (var item in resultData.ListData)
            {
                item.ImgSchedule = db90_DNRS.IMG_SCHEDULE.FirstOrDefault(a => a.prog_id == item.PRG_CD);

                if (item.ImgSchedule == null)
                {
                    item.ImgSchedule = new IMG_SCHEDULE();
                }
                else
                {
                    if (String.IsNullOrEmpty(item.ImgSchedule.parentid) == false)
                    {
                        var parentNews = db90_DNRS.T_NEWS_PRG.FirstOrDefault(a => a.PRG_CD == item.ImgSchedule.parentid);
                        if (parentNews != null)
                        {
                            item.ImgSchedule.ParentProgramCode = parentNews.PRG_CD;
                            item.ImgSchedule.ParentProgramName = parentNews.PRG_NM;
                        }

                        var parent = db90_DNRS.IMG_SCHEDULE.FirstOrDefault(a => a.prog_id == item.ImgSchedule.parentid);
                        if (parent != null)
                        {
                            item.ImgSchedule.ParentProgramPoint = parent.point;
                        }
                    }
                }

                item.ProgramList = db90_DNRS.TAB_PROGRAM_LIST.FirstOrDefault(a => a.PGM_ID == item.PRG_CD);
                if (item.ProgramList == null)
                {
                    item.ProgramList = new TAB_PROGRAM_LIST();
                }

                ProgramTemplateBiz programTemplateBiz = new ProgramTemplateBiz();
                var programTemplate = programTemplateBiz.GetAt(item.PRG_CD);
                if (programTemplate == null)
                {
                }
                else
                {
                    programTemplate.ProgramName = item.PRG_NM;
                    if (programTemplate.DEL_YN == "N")
                    {
                        item.ProgramTemplateType = programTemplate.TEMPLATE_TYPE;
                        var programGroup = programTemplateBiz.GetGroupFirst(programTemplate.PROGRAM_TEMPLATE_SEQ);
                        if (programGroup != null)
                        {
                            item.ProgramTemplateName = programGroup.GROUP_NAME;
                        }
                    }
                }


                var newsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == item.PRG_CD);
                if (newsProgram != null)
                {
                    item.SERVICE_NAME = newsProgram.SERVICE_NAME;
                    item.ENCODING = newsProgram.ENCODING;
                    item.PUBLISH_YN = newsProgram.PUBLISH_YN;
                    item.CP_NM = newsProgram.CP_NM;
                    item.PLAN_BROAD = newsProgram.PLAN_BROAD;
                    item.BroadTypeCode = newsProgram.BROAD_TYPE_CODE;
                    item.FameYn = newsProgram.FAME_YN;
                    item.FirstFreeYn = newsProgram.FIRST_FREE_YN;
                }
            }

            return resultData;
        }



        public ListModel<IMG_SCHEDULE> SearchListImgSchedule(NewsProgramCondition condition)
        {
            ListModel<IMG_SCHEDULE> resultData = new ListModel<IMG_SCHEDULE>();

            var list = db90_DNRS.IMG_SCHEDULE.AsQueryable();


            if (String.IsNullOrEmpty(condition.ProgramName) == false)
            {
                list = list.Where(a => a.prog_name.Contains(condition.ProgramName) == true);
            }

            if (String.IsNullOrEmpty(condition.PublishYn) == false)
            {
                if (condition.PublishYn == "N")
                {
                    list = list.Where(a =>
                        db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.prog_id && b.PUBLISH_YN == "N").Count() > 0
                        || db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.prog_id).Count() == 0
                    );
                }
                else
                {
                    list = list.Where(a =>
                        db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.prog_id && b.PUBLISH_YN == "Y").Count() > 0
                    );
                }
            }

            if (String.IsNullOrEmpty(condition.BroadTypeCode) == false)
            {
                list = list.Where(a =>
                    db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.prog_id && b.BROAD_TYPE_CODE == condition.BroadTypeCode).Count() > 0
                );
            }


            if (String.IsNullOrEmpty(condition.IngYn) == false)
            {
                list = list.Where(a => db90_DNRS.TAB_PROGRAM_LIST.Where(b => b.PGM_ID == a.prog_id && b.ING_YN == condition.IngYn).Count() > 0);
            }

            if (condition.CategoryCode > 0)
            {
                list = list.Where(a => db90_DNRS.TAB_PROGRAM_LIST.Where(b => b.PGM_ID == a.prog_id && b.CATE_CODE == condition.CategoryCode).Count() > 0);
            }

            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                switch (condition.SearchType)
                {
                    case "ProgramCd":
                        list = list.Where(a => a.prog_id.Contains(condition.SearchText) == true);
                        break;
                    case "ProgramName":
                        list = list.Where(a => a.prog_name.Contains(condition.SearchText) == true);
                        break;
                }
            }


            if (condition.AdminSeq > 0)
            {
                List<string> programCodeList = GetAdminProgram(condition.AdminSeq);
                list = list.Where(a => programCodeList.Contains(a.prog_id));
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderBy(a => a.parentid).ThenBy(a => a.prog_id);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();

            foreach (var item in resultData.ListData)
            {
                item.ProgramList = db90_DNRS.TAB_PROGRAM_LIST.FirstOrDefault(a => a.PGM_ID == item.prog_id);
                if (item.ProgramList == null)
                {
                    item.ProgramList = new TAB_PROGRAM_LIST();
                }

                ProgramTemplateBiz programTemplateBiz = new ProgramTemplateBiz();
                var programTemplate = programTemplateBiz.GetAt(item.prog_id);
                if (programTemplate == null)
                {
                }
                else
                {
                    programTemplate.ProgramName = item.prog_name;
                    if (programTemplate.DEL_YN == "N")
                    {
                        //item.ProgramTemplateType = programTemplate.TEMPLATE_TYPE;
                        var programGroup = programTemplateBiz.GetGroupFirst(programTemplate.PROGRAM_TEMPLATE_SEQ);
                        if (programGroup != null)
                        {
                            item.ProgramTemplateName = programGroup.GROUP_NAME;
                        }
                    }
                }


                var newsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == item.prog_id);
                if (newsProgram != null)
                {
                    item.SERVICE_NAME = newsProgram.SERVICE_NAME;
                    item.ENCODING = newsProgram.ENCODING;
                    item.PUBLISH_YN = newsProgram.PUBLISH_YN;
                    item.CP_NM = newsProgram.CP_NM;
                    item.PLAN_BROAD = newsProgram.PLAN_BROAD;
                    item.BroadTypeCode = newsProgram.BROAD_TYPE_CODE;
                }

                if (String.IsNullOrEmpty(item.parentid) == false)
                {
                    var parent = db90_DNRS.IMG_SCHEDULE.FirstOrDefault(a => a.prog_id == item.parentid);
                    if(parent != null)
                    {
                        item.ParentProgramCode = parent.prog_id;
                        item.ParentProgramName = parent.prog_name;
                        item.ParentProgramPoint = parent.point;
                    }
                }
            }

            return resultData;
        }


        public void Save(T_NEWS_PRG model, NTB_ATTACH_FILE mainAttachFile, NTB_ATTACH_FILE subAttachFile
            , NTB_ATTACH_FILE rectangleAttachFile, NTB_ATTACH_FILE thumbnailAttachFile)
        {
            var data = GetAt(model.PRG_CD);

            if (data == null)
            {
                // 신규로 생성하는 기능은 제공 안함
                //db90_DNRS.T_NEWS_PRG.Add()
            }
            else
            {
                NTB_NEWS_PROGRAM newsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == model.PRG_CD);
                if (newsProgram == null)
                {
                    newsProgram = new NTB_NEWS_PROGRAM();
                    newsProgram.PRG_CD = model.PRG_CD;

                    newsProgram.FAME_YN = model.FameYn;
                    newsProgram.BROAD_TYPE_CODE = model.BroadTypeCode;
                    newsProgram.ENCODING = model.ENCODING;
                    newsProgram.PUBLISH_YN = model.PUBLISH_YN;
                    newsProgram.CP_NM = model.CP_NM;
                    newsProgram.PLAN_BROAD = model.PLAN_BROAD;
                    newsProgram.FIRST_FREE_YN = model.FirstFreeYn;

                    db90_DNRS.NTB_NEWS_PROGRAM.Add(newsProgram);
                }
                else
                {
                    newsProgram.FAME_YN = model.FameYn;
                    newsProgram.BROAD_TYPE_CODE = model.BroadTypeCode;
                    newsProgram.ENCODING = model.ENCODING;
                    newsProgram.PUBLISH_YN = model.PUBLISH_YN;
                    newsProgram.CP_NM = model.CP_NM;
                    newsProgram.PLAN_BROAD = model.PLAN_BROAD;
                    newsProgram.FIRST_FREE_YN = model.FirstFreeYn;
                    newsProgram.BROAD_SECTION_CODE = model.BroadSectionCode;
                }

                data.PRG_NM = model.PRG_NM;
                data.PD_NM = model.PD_NM;
                data.PD2_NM = model.PD2_NM;
                data.TD_NM = model.TD_NM;
                data.TD2_NM = model.TD2_NM;

                db90_DNRS.SaveChanges();



                if (model.ImgSchedule != null)
                {
                    if (String.IsNullOrEmpty(data.ImgSchedule.prog_id) == true)
                    {
                        IMG_SCHEDULE imgSchedule = new IMG_SCHEDULE();
                        imgSchedule.prog_id = data.PRG_CD;
                        imgSchedule.folder_name = model.ImgSchedule.folder_name;
                        imgSchedule.file_front_name = model.ImgSchedule.file_front_name;
                        imgSchedule.prog_content = model.ImgSchedule.prog_content;
                        imgSchedule.web_state = "";
                        imgSchedule.point = model.ImgSchedule.point;
                        imgSchedule.runtime = 0;
                        imgSchedule.formation = "";
                        imgSchedule.bitrate = "";
                        imgSchedule.bitrate = "";
                        imgSchedule.AutoEnc = 0;
                        imgSchedule.SendFlag = false;
                        imgSchedule.parentid = model.ImgSchedule.parentid;
                        imgSchedule.sdate = (String.IsNullOrEmpty(model.ImgSchedule.sdate) == true ? "" : model.ImgSchedule.sdate.Replace("-", ""));
                        imgSchedule.edate = (String.IsNullOrEmpty(model.ImgSchedule.edate) == true ? "" : model.ImgSchedule.edate.Replace("-", ""));

                        //db90_DNRS.IMG_SCHEDULE.Add(imgSchedule);
                    }
                    else
                    {
                        data.ImgSchedule.point = model.ImgSchedule.point;
                        data.ImgSchedule.parentid = model.ImgSchedule.parentid;
                        data.ImgSchedule.sdate = (String.IsNullOrEmpty(model.ImgSchedule.sdate) == true ? "" : model.ImgSchedule.sdate.Replace("-", ""));
                        data.ImgSchedule.edate = (String.IsNullOrEmpty(model.ImgSchedule.edate) == true ? "" : model.ImgSchedule.edate.Replace("-", ""));
                        data.ImgSchedule.folder_name = model.ImgSchedule.folder_name;
                        data.ImgSchedule.file_front_name = model.ImgSchedule.file_front_name;
                        data.ImgSchedule.prog_content = model.ImgSchedule.prog_content;
                    }
                }


                if (model.ProgramList != null)
                {
                    if (String.IsNullOrEmpty(data.ProgramList.PGM_ID) == true)
                    {
                        TAB_PROGRAM_LIST programList = new TAB_PROGRAM_LIST();
                        programList.PGM_ID = model.PRG_CD;
                        programList.CATE_CODE = model.ProgramList.CATE_CODE;
                        programList.ING_YN = model.ProgramList.ING_YN;

                        //db90_DNRS.TAB_PROGRAM_LIST.Add(programList);
                    }
                    else
                    {
                        data.ProgramList.PGM_ID = model.PRG_CD;
                        data.ProgramList.CATE_CODE = model.ProgramList.CATE_CODE;
                        data.ProgramList.ING_YN = model.ProgramList.ING_YN;
                    }
                }
            }

            db90_DNRS.SaveChanges();



            if (mainAttachFile != null)
            {
                mainAttachFile.TABLE_CODE = "T_NEWS_PRG-MAIN";
                mainAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(mainAttachFile);
            }
            if (subAttachFile != null)
            {
                subAttachFile.TABLE_CODE = "T_NEWS_PRG-SUB";
                subAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(subAttachFile);
            }
            if (rectangleAttachFile != null)
            {
                rectangleAttachFile.TABLE_CODE = "T_NEWS_PRG-RECTANGLE";
                rectangleAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(rectangleAttachFile);
            }
            if (thumbnailAttachFile != null)
            {
                thumbnailAttachFile.TABLE_CODE = "T_NEWS_PRG-THUMBNAIL";
                thumbnailAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(thumbnailAttachFile);
            }
        }


        public void SaveSchedule(T_NEWS_PRG model)
        {
            var data = GetAt(model.PRG_CD);

            if (data == null)
            {
                // 신규로 생성하는 기능은 제공 안함
                //db90_DNRS.T_NEWS_PRG.Add()
            }
            else
            {
                NTB_NEWS_PROGRAM newsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == model.PRG_CD);
                if (newsProgram == null)
                {
                    newsProgram = new NTB_NEWS_PROGRAM();
                    newsProgram.PRG_CD = model.PRG_CD;
                    newsProgram.SERVICE_NAME = model.SERVICE_NAME;

                    db90_DNRS.NTB_NEWS_PROGRAM.Add(newsProgram);
                }
                else
                {
                    newsProgram.SERVICE_NAME = model.SERVICE_NAME;
                }
                db90_DNRS.SaveChanges();

                //data.BRO_START = model.BRO_START;
                //data.BRO_END = model.BRO_END;
                data.ANC1_NM = model.ANC1_NM;
                data.ANC2_NM = model.ANC2_NM;
            }

            db90_DNRS.SaveChanges();
        }




        /// <summary>
        /// 내 프로그램에서 수정할때 쓰는 함수
        /// </summary>
        /// <param name="model"></param>
        public void SaveFromMyProgram(T_NEWS_PRG model, NTB_ATTACH_FILE mainAttachFile, NTB_ATTACH_FILE subAttachFile
            , NTB_ATTACH_FILE rectangleAttachFile, NTB_ATTACH_FILE thumbnailAttachFile)
        {
            var data = GetAt(model.PRG_CD);

            if (data == null)
            {
                // 신규로 생성하는 기능은 제공 안함
                //db90_DNRS.T_NEWS_PRG.Add()
            }
            else
            {
                NTB_NEWS_PROGRAM newsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == model.PRG_CD);
                if (newsProgram == null)
                {
                    newsProgram = new NTB_NEWS_PROGRAM();
                    newsProgram.PRG_CD = model.PRG_CD;
                    newsProgram.CP_NM = model.CP_NM;

                    db90_DNRS.NTB_NEWS_PROGRAM.Add(newsProgram);
                }
                else
                {
                    newsProgram.CP_NM = model.CP_NM;
                }
                db90_DNRS.SaveChanges();

                data.PD_NM = model.PD_NM;
                data.PD2_NM = model.PD2_NM;
                data.ANC1_NM = model.ANC1_NM;
                data.ANC2_NM = model.ANC2_NM;

                data.PGMDAY = model.PGMDAY;
                //data.BRO_START = model.BRO_START;
                //data.BRO_END = model.BRO_END;

            }

            db90_DNRS.SaveChanges();



            if (mainAttachFile != null)
            {
                mainAttachFile.TABLE_CODE = "T_NEWS_PRG-MAIN";
                mainAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(mainAttachFile);
            }
            if (subAttachFile != null)
            {
                subAttachFile.TABLE_CODE = "T_NEWS_PRG-SUB";
                subAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(subAttachFile);
            }
            if (rectangleAttachFile != null)
            {
                rectangleAttachFile.TABLE_CODE = "T_NEWS_PRG-RECTANGLE";
                rectangleAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(rectangleAttachFile);
            }
            if (thumbnailAttachFile != null)
            {
                thumbnailAttachFile.TABLE_CODE = "T_NEWS_PRG-THUMBNAIL";
                thumbnailAttachFile.TABLE_KEY = model.PRG_CD;

                new AttachFile.AttachFileBiz().Create(thumbnailAttachFile);
            }
        }





        public void ExcelFileSave(NTB_ATTACH_FILE model)
        {
            model.TABLE_CODE = TableCode;
            model.TABLE_KEY = TableKey; // 해당 분류에 첨부파일은 무조건 하나만 있다는 전제

            new AttachFileBiz().Create(model);
        }

        public NTB_ATTACH_FILE GetExcelFile()
        {
            return new AttachFileBiz().GetAt(TableCode, TableKey);
        }


        public void Delete(string programCode)
        {
            var prev = GetAt(programCode);
            prev.DELFLAG = "1";

            db90_DNRS.SaveChanges();
        }


        public void DeleteImgSchedule(string programCode)
        {
            var prev = GetAtImgSchedule(programCode);
            if (prev != null)
            {
                db90_DNRS.IMG_SCHEDULE.Remove(prev);
                db90_DNRS.SaveChanges();
            }
        }





        #region 파트너

        public List<Pro_wowList> GetWowListPartnerList(string programCode)
        {
            List<Pro_wowList> list = new List<Pro_wowList>();
            List<NTB_PROGRAM_PARTNER> partnerList = GetPartnerList(programCode);
            List<int> payNoList = partnerList.Select(a => a.pay_no).ToList();

            list = db49_broadcast.Pro_wowList.Where(a => payNoList.Contains(a.Pay_no) == true).ToList();

            foreach(var item in list)
            {
                var cafeMemberInfo = db49_wowcafe.CafeMemberInfo.Where(a => a.GradeLevel == 1 && a.UserID == item.Wowtv_id);
                cafeMemberInfo = cafeMemberInfo.Where(a => a.CafeInfo.DelFlag == "N" && a.CafeInfo.UseFlag == "N");
                var cafeMemberItem = cafeMemberInfo.FirstOrDefault();
                if(cafeMemberItem != null)
                {
                    if(cafeMemberItem.CafeInfo != null)
                    {
                        item.CafeCode = cafeMemberItem.CafeInfo.CafeCode.ToString();
                    }
                }
            }
            return list;
        }

        public List<NTB_PROGRAM_PARTNER> GetPartnerList(string programCode)
        {
            return db49_wowtv.NTB_PROGRAM_PARTNER.Where(a => a.PRG_CD == programCode).OrderBy(a => a.PARTNER_NAME).ToList();
        }

        public void AddPartner(string programCode, int payNo, LoginUser loginUser)
        {
            string partnerName = "";
            var partner = db49_broadcast.Pro_wowList.SingleOrDefault(a => a.Pay_no == payNo);
            if (partner != null)
            {
                partnerName = partner.FullName;
            }

            NTB_PROGRAM_PARTNER model = new NTB_PROGRAM_PARTNER();
            model.PRG_CD = programCode;
            model.pay_no = payNo;
            model.PARTNER_NAME = partnerName;

            model.REG_DATE = DateTime.Now;
            model.REG_ID = loginUser.LoginId;
            model.MOD_DATE = DateTime.Now;
            model.MOD_ID = loginUser.LoginId;

            db49_wowtv.NTB_PROGRAM_PARTNER.Add(model);
            db49_wowtv.SaveChanges();
        }


        public void DeletePartner(string programCode, int payNo)
        {
            var prev = db49_wowtv.NTB_PROGRAM_PARTNER.SingleOrDefault(a => a.PRG_CD == programCode && a.pay_no == payNo);

            db49_wowtv.NTB_PROGRAM_PARTNER.Remove(prev);
            db49_wowtv.SaveChanges();
        }

        public void DeletePartnerList(string programCode)
        {
            var prevList = db49_wowtv.NTB_PROGRAM_PARTNER.Where(a => a.PRG_CD == programCode).ToList();

            foreach (var item in prevList)
            {
                DeletePartner(item.PRG_CD, item.pay_no);
            }
        }



        #endregion





        #region 관리자

        public List<NTB_PROGRAM_ADMIN> GetAdminList(string programCode)
        {
            return db49_wowtv.NTB_PROGRAM_ADMIN.Where(a => a.PRG_CD == programCode).OrderBy(a => a.ADMIN_NAME).ToList();
        }

        public void AddAdmin(string programCode, int adminSeq, LoginUser loginUser)
        {
            string adminName = "";
            var admin = db49_wowtv.TAB_CMS_ADMIN.SingleOrDefault(a => a.SEQ == adminSeq);
            if (admin != null)
            {
                adminName = admin.NAME;
            }

            NTB_PROGRAM_ADMIN model = new NTB_PROGRAM_ADMIN();
            model.PRG_CD = programCode;
            model.SEQ = adminSeq;
            model.ADMIN_NAME = adminName;

            model.REG_DATE = DateTime.Now;
            model.REG_ID = loginUser.LoginId;
            model.MOD_DATE = DateTime.Now;
            model.MOD_ID = loginUser.LoginId;

            db49_wowtv.NTB_PROGRAM_ADMIN.Add(model);
            db49_wowtv.SaveChanges();
        }


        public void DeleteAdmin(string programCode, int adminSeq)
        {
            var prev = db49_wowtv.NTB_PROGRAM_ADMIN.SingleOrDefault(a => a.PRG_CD == programCode && a.SEQ == adminSeq);

            db49_wowtv.NTB_PROGRAM_ADMIN.Remove(prev);
            db49_wowtv.SaveChanges();
        }

        public void DeleteAdminList(string programCode)
        {
            var prevList = db49_wowtv.NTB_PROGRAM_ADMIN.Where(a => a.PRG_CD == programCode).ToList();

            foreach (var item in prevList)
            {
                DeleteAdmin(item.PRG_CD, item.SEQ);
            }
        }





        public List<string> GetAdminProgram(int adminSeq)
        {
            return db49_wowtv.NTB_PROGRAM_ADMIN.Where(b => b.SEQ == adminSeq).Select(b => b.PRG_CD).ToList();
        }

        #endregion




        public List<T_RUNDOWN> SearchListRunDown(string date)
        {
            //string broadDate = DateTime.Now.ToString("yyyyMMddHHmm");

            //Wow.Fx.WowLog.Write("1");

            List<T_RUNDOWN> list = new List<T_RUNDOWN>();

            try
            {
                list = db90_DNRS.T_RUNDOWN.Where(a => a.INNING != "0" && a.DELETE_FLAG == "N" && a.RUN_DATE == date).OrderBy(a => a.RUN_START).ToList();

                //Wow.Fx.WowLog.Write("2");

                #region TODO 주석풀기

                foreach (var item in list)
                {
                    NTB_ATTACH_FILE subImage = GetSubAttachFile(item.PRG_CD);
                    if (subImage != null)
                    {
                        item.SUB_IMG = subImage.REAL_WEB_PATH;
                    }
                    NTB_ATTACH_FILE rectangleImage = GetRectangleAttachFile(item.PRG_CD);
                    if (rectangleImage != null)
                    {
                        item.REC_IMG = rectangleImage.REAL_WEB_PATH;
                    }

                    string temp = item.RUN_DATE.Replace("-", "");
                    var tvProgram = db90_DNRS.tv_program.Where(a => a.Dep == item.PRG_CD && a.broad_date == temp).FirstOrDefault();
                    if (tvProgram != null)
                    {
                        if (tvProgram.State != null && tvProgram.State != 0)
                        {
                            item.BroadWatchStatus = "Complete";
                        }
                    }

                    item.DayOfWeekString = new List<string>();
                    var newsProgram = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == item.PRG_CD);
                    if (newsProgram != null)
                    {
                        var ntbNewsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == item.PRG_CD);

                        if (ntbNewsProgram != null)
                        {
                            if (ntbNewsProgram.PLAN_BROAD == "FIRST")
                            {
                                item.IsFirst = true;
                            }
                            if (ntbNewsProgram.PLAN_BROAD == "OPEN")
                            {
                                item.IsRenewal = true;
                            }
                        }

                        if (newsProgram.IsMonday == true)
                        {
                            item.DayOfWeekString.Add("월");
                        }
                        if (newsProgram.IsTuesday == true)
                        {
                            item.DayOfWeekString.Add("화");
                        }
                        if (newsProgram.IsWednesday == true)
                        {
                            item.DayOfWeekString.Add("수");
                        }
                        if (newsProgram.IsThursday == true)
                        {
                            item.DayOfWeekString.Add("목");
                        }
                        if (newsProgram.IsFriday == true)
                        {
                            item.DayOfWeekString.Add("금");
                        }
                        if (newsProgram.IsSaturday == true)
                        {
                            item.DayOfWeekString.Add("토");
                        }
                        if (newsProgram.IsSunday == true)
                        {
                            item.DayOfWeekString.Add("일");
                        }
                    }
                }

                #endregion

                //Wow.Fx.WowLog.Write("3");
            }
            catch(Exception ex)
            {
                //Wow.Fx.WowLog.Write(ex.Message);

                if(ex.InnerException != null)
                {
                    //Wow.Fx.WowLog.Write("Inner : " + ex.Message);
                }
            }

            return list;
        }




        public T_RUNDOWN GetNowRunDown()
        {
            DateTime nowDate = DateTime.Now;
            //nowDate = new DateTime(2017, 11, 3, 7, 0, 0);
            string nowDateString = nowDate.ToString("yyyy-MM-dd");
            string nowTime = nowDate.ToString("HH:mm");

            var list = db90_DNRS.T_RUNDOWN.Where(a => a.INNING != "0" && a.DELETE_FLAG == "N" && a.RUN_DATE == nowDateString);
            list = list.Where(a => a.RUN_START.CompareTo(nowTime) <= 0 && a.RUN_END.CompareTo(nowTime) > 0);
            list = list.OrderBy(a => a.RUN_START);


            T_RUNDOWN model = list.FirstOrDefault();

            if(model != null)
            {
                model.DayOfWeekString = new List<string>();
                var newsProgram = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == model.PRG_CD);
                if (newsProgram != null)
                {
                    var ntbNewsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == model.PRG_CD);

                    if (ntbNewsProgram != null)
                    {
                        if (ntbNewsProgram.PLAN_BROAD == "FIRST")
                        {
                            model.IsFirst = true;
                        }
                        if (ntbNewsProgram.PLAN_BROAD == "OPEN")
                        {
                            model.IsRenewal = true;
                        }
                    }

                    if (newsProgram.IsMonday == true)
                    {
                        model.DayOfWeekString.Add("월");
                    }
                    if (newsProgram.IsTuesday == true)
                    {
                        model.DayOfWeekString.Add("화");
                    }
                    if (newsProgram.IsWednesday == true)
                    {
                        model.DayOfWeekString.Add("수");
                    }
                    if (newsProgram.IsThursday == true)
                    {
                        model.DayOfWeekString.Add("목");
                    }
                    if (newsProgram.IsFriday == true)
                    {
                        model.DayOfWeekString.Add("금");
                    }
                    if (newsProgram.IsSaturday == true)
                    {
                        model.DayOfWeekString.Add("토");
                    }
                    if (newsProgram.IsSunday == true)
                    {
                        model.DayOfWeekString.Add("일");
                    }



                    //model.CastNameList = new List<string>();
                    //var castList = db49_broadcast.NTB_PROGRAM_CAST.Where(a => a.PRG_CD == model.PRG_CD);
                    //foreach(var item in castList)
                    //{
                    //    model.CastNameList.Add(item.CAST_NAME);
                    //}

                    model.CastNameList = new List<string>();
                    var partnerList = db49_wowtv.NTB_PROGRAM_PARTNER.Where(a => a.PRG_CD == model.PRG_CD);
                    foreach (var item in partnerList)
                    {
                        model.CastNameList.Add(item.PARTNER_NAME);
                    }
                }
            }

            return model;
        }




        #region Front Main 용

        #region 기본

        public List<tblStockClass> GetListStockVod()
        {
            var list = db49_editVOD.tblStockClass.Where(a => a.tblVODList.DelFlag == 0).OrderByDescending(a => a.vodNum).Take(4);
            foreach(var item in list)
            {
                var vodEdit = db49_editVOD.tblVODEdit.FirstOrDefault(a => a.vodNum == item.vodNum);
                if (vodEdit != null)
                {
                    item.EditFolder = vodEdit.EditFolder;
                    item.EditFile = vodEdit.EditFile;
                    item.ImageS = vodEdit.Image_S;
                }
            }

            return list.ToList();
        }

        public List<NUP_BROAD_MAIN_ARTICLE_SELECT_Result> GetListMarket()
        {
            Wow.Fx.WowLog.Write("GetListMarket 1");

            List<NUP_BROAD_MAIN_ARTICLE_SELECT_Result> list = new List<NUP_BROAD_MAIN_ARTICLE_SELECT_Result>();
            var planArticleList = db49_Article.tblPlanArticle.Where(a => a.SEQ > 0 && a.CLASS == "M" && a.VIEW_FLAG == "Y").OrderBy(a => a.SEQ).Take(4);

            Wow.Fx.WowLog.Write("GetListMarket 2");

            foreach (var item in planArticleList)
            {
                Wow.Fx.WowLog.Write("GetListMarket 3");

                NUP_BROAD_MAIN_ARTICLE_SELECT_Result result = db49_Article.NUP_BROAD_MAIN_ARTICLE_SELECT(item.SEQ).FirstOrDefault();

                Wow.Fx.WowLog.Write("GetListMarket 4");

                list.Add(result);
            }

            return list;
        }


        public void ProgramOrder(TAB_PGM_ORDER model, NTB_ATTACH_FILE attachFile)
        {
            model.CATE_CODE = 178;
            model.TITLE = "";
            model.BROAD_TIME = "";
            model.STIME = ":";
            model.ETIME = ":";
            model.STATUS = "1";
            model.BROAD_DATE = model.BROAD_DATE.Replace("/", "-");
            model.REG_DATE = DateTime.Now;
            db49_wowtv.TAB_PGM_ORDER.Add(model);
            db49_wowtv.SaveChanges();

            if (attachFile != null)
            {
                attachFile.TABLE_CODE = "TAB_PGM_ORDER_TvMainOrder";
                attachFile.TABLE_KEY = model.SEQ.ToString();
                new AttachFile.AttachFileBiz().Create(attachFile);
            }
        }


        public List<NTB_BOARD_CONTENT> GetNotice1()
        {
            List<NTB_BOARD_CONTENT> list = null;

            List<int> contentSeqList = db49_wowtv.NTB_MENU.Where(a => a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                                            && a.FIX_YN == "Y"
                                            && a.CONTENT_SEQ != null
                                            && db49_wowtv.NTB_BOARD.Where(b => b.BOARD_TYPE_CODE == "Notice" && b.BOARD_SEQ == a.CONTENT_SEQ).Count() > 0
                                        ).Select(a => a.CONTENT_SEQ.Value).ToList();

            list = db49_wowtv.NTB_BOARD_CONTENT.Where(a => a.DEL_YN == "N" && a.VIEW_YN == "Y" && contentSeqList.Contains(a.BOARD_SEQ))
                .OrderByDescending(a => a.REG_DATE).Take(6).ToList();

            foreach(var item in list)
            {
                var menu = db49_wowtv.NTB_MENU.Where(a => a.CONTENT_SEQ == item.BOARD_SEQ && a.FIX_YN == "Y").SingleOrDefault();
                if (menu != null)
                {
                    item.MenuSeq = menu.MENU_SEQ;
                    item.ProgramCode = menu.PRG_CD;

                    if(String.IsNullOrEmpty(item.ProgramCode) == false)
                    {
                        var newsProgram = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == item.ProgramCode);
                        if(newsProgram != null)
                        {
                            item.ProgramName = newsProgram.PRG_NM;
                        }
                    }
                }
            }

            return list;
        }
        public List<TAB_LECTURES_SCHEDULE> GetNotice2()
        {
            List<TAB_LECTURES_SCHEDULE> model = new List<TAB_LECTURES_SCHEDULE>();
            string nowDate = DateTime.Now.ToString("yyyy-MM-dd");

            var list = db49_wownet.TAB_LECTURES_SCHEDULE.Where(a => a.LECTURES_DATE.CompareTo(nowDate) >= 0);
            list = list.Where(a => db49_wownet.TAB_LECTURES.Where(b => b.SEQ == a.MSEQ && b.VIEW_FLAG == "Y" && (b.VIEW_SITE == "A" || b.VIEW_SITE == "N")).Count() > 0);
            list = list.OrderByDescending(a => a.LECTURES_DATE);
            list = list.Take(6);

            model = list.ToList();
            foreach (var item in model)
            {
                item.Title = db49_wownet.TAB_LECTURES.FirstOrDefault(a => a.SEQ == item.MSEQ).TITLE;
            }

            return model;
        }
        public List<NTB_BOARD_CONTENT> GetNotice3()
        {
            List<NTB_BOARD_CONTENT> list = null;

            List<int> contentSeqList = db49_wowtv.NTB_MENU.Where(a => a.CHANNEL_CODE == CommonCodeStatic.MENU_BROAD_DUAL_CHANNEL_CODE
                                            && a.FIX_YN == "Y"
                                            && a.CONTENT_SEQ != null
                                            && db49_wowtv.NTB_BOARD.Where(b => b.BOARD_TYPE_CODE == "FeedBack" && b.BOARD_SEQ == a.CONTENT_SEQ).Count() > 0
                                        ).Select(a => a.CONTENT_SEQ.Value).ToList();

            list = db49_wowtv.NTB_BOARD_CONTENT.Where(a => a.DEL_YN == "N" && (a.VIEW_YN == null || a.VIEW_YN == "Y") && contentSeqList.Contains(a.BOARD_SEQ))
                .OrderByDescending(a => a.REG_DATE).Take(6).ToList();

            foreach (var item in list)
            {
                var menu = db49_wowtv.NTB_MENU.Where(a => a.CONTENT_SEQ == item.BOARD_SEQ && a.FIX_YN == "Y").SingleOrDefault();
                if (menu != null)
                {
                    item.MenuSeq = menu.MENU_SEQ;
                    item.ProgramCode = menu.PRG_CD;

                    if (String.IsNullOrEmpty(item.ProgramCode) == false)
                    {
                        var newsProgram = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == item.ProgramCode);
                        if (newsProgram != null)
                        {
                            item.ProgramName = newsProgram.PRG_NM;
                        }
                    }
                }
            }

            return list;
        }



        public bool IsHoliDayCheck()
        {
            bool isHoliDay = false;
            DateTime nowDate = DateTime.Now;
            string nowDateString = nowDate.ToString("yyyyMMdd");
            var list = db22_stock.NTB_SISE_TIME.Where(a => a.MARKET_DT == nowDateString && a.GUBUN == "T" && a.HOLY_YN == "Y" && a.DEL_YN != "Y");
            if(list.Count() > 0)
            {
                isHoliDay = true;
            }

            return isHoliDay;
        }

        #endregion



        #region 와우넷/와우파/와우스타



        // =====================================
        // -------------------------------------

        #region 와우넷

        public List<BestStockPro> GetWowNetData()
        {
            DateTime nowDate = DateTime.Now;
            string nowDateString = nowDate.ToString("yyyyMMddHHmmss");
            List<BestStockPro> model = new List<BestStockPro>();

            var list = db49_broadcast.BestStockPro.AsQueryable();
            list = list.OrderBy(a => a.proRank);
            list = list.Take(3);

            model = list.ToList();
            foreach(var item in model)
            {
                var pro = db49_broadcast.Pro_wowList.Where(a => a.NickName == item.nickname && a.State == "1").FirstOrDefault();
                if(pro != null)
                {
                    item.NewPhotoSmall = pro.NEWphoto_small;
                    item.NewPhotoSmall2 = pro.NEWphoto_small2;
                    item.InvestKing = pro.InvestKind;


                    var junmungaList = db49_broadcast.Pro_wow_junmunga_broadcast.Where(a => a.Pro_id == pro.Pro_id).AsQueryable();
                    junmungaList = junmungaList.Where(a => a.state != 2 && a.state != 5);
                    junmungaList = junmungaList.Where(a => a.brStartTime.CompareTo(nowDateString) > 0 || a.state == 1);
                    junmungaList = junmungaList.OrderByDescending(a => a.state).ThenBy(a => a.OnAirTime).ThenBy(a => a.brStartTime);
                    var junmunga = junmungaList.FirstOrDefault();

                    if (junmunga != null)
                    {
                        item.State = junmunga.state;
                    }

                }

            }
            return model;
        }


        public List<USP_GetRecommendPro3_Result> GetWowNetData2()
        {
            var list = new MyPage.MyPageBiz().GetRecommendPartner().ToList();
            list = list.Where(a => a.BMEMTYPE == "F").ToList();
            if(list.Count < 3)
            {
                var addList = db49_broadcast.usp_GetlivePro("F", 3).ToList();
                foreach (var addItem in addList)
                {
                    if (list.Count < 3)
                    {
                        USP_GetRecommendPro3_Result item = new USP_GetRecommendPro3_Result();
                        item.PRO_ID = addItem.PRO_ID;
                        item.NEWPHOTO_SMALL2 = addItem.NEWPHOTO_SMALL2;
                        item.NICKNAME = addItem.NICKNAME;
                        item.INVESTKIND = addItem.INVESTKIND;
                        item.BROOM = addItem.BROOM;
                        item.PRODUCT_ID_1 = addItem.PRODUCT_ID_1;
                        item.BMEMTYPE = addItem.BMEMTYPE;

                        list.Add(item);
                    }
                }
            }

            foreach (var item in list)
            {
                var cafeMemberInfo = db49_wowcafe.CafeMemberInfo.Where(a => a.GradeLevel == 1 && a.UserID == item.WOWTV_ID);
                cafeMemberInfo = cafeMemberInfo.Where(a => a.CafeInfo.DelFlag == "N" && a.CafeInfo.UseFlag == "N");
                var cafeMemberItem = cafeMemberInfo.FirstOrDefault();
                if (cafeMemberItem != null)
                {
                    if (cafeMemberItem.CafeInfo != null)
                    {
                        item.CafeCode = cafeMemberItem.CafeInfo.CafeCode.ToString();
                    }
                }
            }

            return list;
        }


        #endregion

        // -------------------------------------
        // =====================================




        // =====================================
        // -------------------------------------

        #region 와우파

        public List<WOW_M_BANNER> GetWowFaData()
        {
            List<WOW_M_BANNER> model = new List<WOW_M_BANNER>();

            DateTime nowDate = DateTime.Now;
            var list = db16_wowfa.WOW_M_BANNER.Where(a => a.post_on == "Y");
            list = list.Where(a => a.edate >= nowDate);
            list = list.Where(a => a.position == "MAIN");
            //list = list.Where(a => a.title.Substring(0, 4) == "[증권]");
            list = list.Where(a => (a.link_url.Contains("gnbMenu1") == true || a.link_url.Contains("gnbMenu3") == true));
            list = list.OrderBy(a => a.edate);

            list = list.Take(6);

            //Wow.Fx.WowLog.Write("!!!! => " + list.ToString());


            model = list.ToList();

            if(model.Count % 2 > 0)
            {
                WOW_M_BANNER item = new WOW_M_BANNER();
                model.Add(item);
            }

            return model;

        }

        public List<NUP_MAIN_COMMON_LECTURE_SELECT_Result> GetWowFaDataLecture()
        {
            List<NUP_MAIN_COMMON_LECTURE_SELECT_Result> list = db49_wownet.NUP_MAIN_COMMON_LECTURE_SELECT().ToList();

            return list;
        }



        public Pro_wow_junmunga_broadcast GetJunMunGaData(string proId)
        {
            return db49_broadcast.Pro_wow_junmunga_broadcast.FirstOrDefault(a => a.Pro_id == proId);
        }

        #endregion

        // -------------------------------------
        // =====================================




        // =====================================
        // -------------------------------------

        #region 와우스타

        public List<wowstar_video_Result> GetWowStarData()
        {
            var wowStarList = db123_WOw4989.wowstar_video(2).ToList();
            return wowStarList;
        }
        public wowstar_p2p_wowtv_Result GetWowStarDataStock()
        {
            string date = DateTime.Now.ToString("yyyyMMddHHmm");
            var wowStarStock = db123_WOw4989.wowstar_p2p_wowtv().FirstOrDefault();
            return wowStarStock;
        }

        #endregion

        // -------------------------------------
        // =====================================


        #endregion


        #region 티커뉴스

        public List<DumyModel> TickerList()
        {
            DateTime nowDate = DateTime.Now;
            string nowTime = nowDate.ToString("HH:mm");
            DumyModel addItem = new DumyModel();
            List<DumyModel> model = new List<DumyModel>();

            //Wow.Fx.WowLog.Write("1");

            try
            {
                // 최신뉴스
                var model1 = db49_Article.NUP_TICKER_SELECT1().ToList();
                foreach (var item in model1)
                {
                    addItem = new DumyModel();
                    addItem.StringValue1 = "1";
                    addItem.StringValue2 = item.ARTICLEID;
                    addItem.StringValue3 = item.TITLE;
                    model.Add(addItem);
                }

                //Wow.Fx.WowLog.Write("2");

                // 특정시간 조회순
                var model2 = db49_Article.NUP_TICKER_SELECT2().ToList();
                foreach (var item in model2)
                {
                    addItem = new DumyModel();
                    addItem.StringValue1 = "2";
                    addItem.StringValue2 = item.ARTICLEID;
                    addItem.StringValue3 = item.TITLE;
                    model.Add(addItem);
                }

                //Wow.Fx.WowLog.Write("3");

                // 오늘날자 기사
                var model3 = db49_Article.NUP_TICKER_SELECT3().ToList();
                foreach (var item in model3)
                {
                    addItem = new DumyModel();
                    addItem.StringValue1 = "3";
                    addItem.StringValue2 = item.ARTICLEID;
                    addItem.StringValue3 = item.TITLE;
                    model.Add(addItem);
                }

                //Wow.Fx.WowLog.Write("4");

                // 지금방영
                var model4 = GetNowRunDown();
                if (model4 != null)
                {
                    addItem = new DumyModel();
                    addItem.StringValue1 = "4";
                    addItem.StringValue2 = model4.PRG_CD;
                    addItem.StringValue3 = model4.PRG_NM;
                    model.Add(addItem);
                }

                //Wow.Fx.WowLog.Write("5");

                // 인기방송
                List<string> programCodeList = SearchListRunDown(nowDate.ToDate()).Select(a => a.PRG_CD).ToList();
                var model5 = db90_DNRS.T_NEWS_PRG.Where(a => a.DELFLAG == "0");
                model5 = model5.Where(a => programCodeList.Contains(a.PRG_CD) == true);
                model5 = model5.Where(a =>
                    db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.PUBLISH_YN == "Y").Count() == 0
                );
                model5 = model5.OrderBy(x => Guid.NewGuid());
                model5 = model5.Take(1);
                foreach (var item in model5)
                {
                    addItem = new DumyModel();
                    addItem.StringValue1 = "5";
                    addItem.StringValue2 = item.PRG_CD;
                    addItem.StringValue3 = item.PRG_NM;
                    model.Add(addItem);
                }

                //Wow.Fx.WowLog.Write("6");


                // SNS
                Wow.Tv.Middle.Model.Db49.wowtv.Broad.BroadLiveCondition broadLiveCondition = new Wow.Tv.Middle.Model.Db49.wowtv.Broad.BroadLiveCondition();
                broadLiveCondition.PublishYn = "Y";
                broadLiveCondition.PageSize = -1;
                var model6 = new BroadLiveBiz().SearchList(broadLiveCondition);
                model6.ListData = model6.ListData.Take(1).ToList();
                foreach (var item in model6.ListData)
                {
                    addItem = new DumyModel();
                    addItem.StringValue1 = "6";
                    addItem.StringValue2 = item.BROAD_LIVE_ID.ToString();
                    addItem.StringValue3 = item.PROGRAM_NAME;
                    model.Add(addItem);
                }

                //Wow.Fx.WowLog.Write("7");

                // 공지 이벤트
                List<DumyModel> model7 = new List<DumyModel>();
                List<DumyModel> tempModel = new List<DumyModel>();
                var model7_1 = new Admin.IntegratedBoardBiz().GetMainNoticeContent();
                if (model7_1 != null)
                {
                    addItem = new DumyModel();
                    addItem.StringValue1 = "7-1";
                    addItem.StringValue2 = model7_1.BOARD_SEQ.ToString();
                    addItem.StringValue3 = model7_1.TITLE;

                    addItem.StringValue4 = model7_1.REG_DATE.ToDate();
                    tempModel.Add(addItem);
                }

                //Wow.Fx.WowLog.Write("8");


                Wow.Tv.Middle.Model.Db49.wownet.ServiceCenter.EventCondition eventCondition = new Model.Db49.wownet.ServiceCenter.EventCondition();
                eventCondition.EventType = "1";
                eventCondition.PageSize = 1;
                var mnodel7_2 = new ServiceCenter.EventBiz().GetFrontList(eventCondition);
                foreach (var item in mnodel7_2.ListData)
                {
                    addItem = new DumyModel();
                    addItem.StringValue1 = "7-2";
                    addItem.StringValue2 = item.Seq.ToString();
                    addItem.StringValue3 = item.Title;

                    addItem.StringValue4 = item.RegDate.ToDate();
                    tempModel.Add(addItem);
                }


                //Wow.Fx.WowLog.Write("9");

                tempModel = tempModel.OrderByDescending(a => a.StringValue4).Take(1).ToList();
                foreach (var item in tempModel)
                {
                    addItem = new DumyModel();
                    addItem.StringValue1 = item.StringValue1;
                    addItem.StringValue2 = item.StringValue2;
                    addItem.StringValue3 = item.StringValue3;
                    model7.Add(addItem);
                }
            }
            catch(Exception ex)
            {
                //Wow.Fx.WowLog.Write(ex.Message);

                if (ex.InnerException != null)
                {
                    //Wow.Fx.WowLog.Write("Inner : " + ex.Message);
                }
            }

            //Wow.Fx.WowLog.Write("10");



            return model;
        }

        #endregion



        public List<int> GetPartnerEventItemPrice(string proId)
        {
            List<int> itemIdList = new List<int>();
            int idx = 0;

            //Wow.Fx.WowLog.Write("PartnerEvent 1");
            var exportPersonalMenu = db49_broadcast.export_personal_menu.Where(a => a.proID == proId).OrderByDescending(a => a.regdate).FirstOrDefault();
            //Wow.Fx.WowLog.Write("PartnerEvent 2");
            if (exportPersonalMenu != null)
            {
                //Wow.Fx.WowLog.Write("PartnerEvent 3");
                idx = exportPersonalMenu.idx;
            }
            if (idx > 0)
            {
                //Wow.Fx.WowLog.Write("PartnerEvent 4");
                itemIdList = db49_broadcast.EXPORT_PRODUCT.Where(a => a.EPKID == idx).Select(a => a.ITEMID).ToList();
                
            }


            return itemIdList;
        }



        public List<Model.Db89.WOWTV_BILL_DB.TItemPriceMst> GetPartnerEvent(List<int> itemIdList)
        {
            List<Model.Db89.WOWTV_BILL_DB.TItemPriceMst> tItemPriceMstList = new List<Model.Db89.WOWTV_BILL_DB.TItemPriceMst>();
            string currentDateString = DateTime.Now.ToString("yyyyMMddHHmmss");

            //Wow.Fx.WowLog.Write("PartnerEvent 6");
            var list = db89_WOWTV_BILL_DB.TItemPriceMst.Where(a => itemIdList.Contains(a.ItemPriceID) == true);
            //Wow.Fx.WowLog.Write("PartnerEvent 7");
            list = list.OrderBy(a => a.Price);
            //Wow.Fx.WowLog.Write("PartnerEvent 8");
            tItemPriceMstList = list.Take(2).ToList();
            Wow.Fx.WowLog.Write("PartnerEvent 9");

            foreach (var item in tItemPriceMstList)
            {
                //Wow.Fx.WowLog.Write("PartnerEvent 10");
                var eventIdList = db89_WOWTV_BILL_DB.TEventDtl.Where(a => a.ItemID == item.ItemPriceID).Select(a => a.EventID).ToList();
                //Wow.Fx.WowLog.Write("PartnerEvent 11");
                //SELECT REGDATE, APPLYDETAIL,*FROM TEVENTMST WHERE  STARTDATE <= '20171204204000' AND ENDDATE >= '20171204204000'-- EventID =
                var tEventMstQuery = db89_WOWTV_BILL_DB.TEventMst.Where(a => eventIdList.Contains(a.EventID) == true && a.StartDate.CompareTo(currentDateString) <= 0 && a.EndDate.CompareTo(currentDateString) >= 0 && a.EventType == 9);
                //Wow.Fx.WowLog.Write("PartnerEvent => " + tEventMstQuery.ToString());
                var eventDetail = tEventMstQuery.FirstOrDefault();
                //Wow.Fx.WowLog.Write("PartnerEvent 12");
                if (eventDetail != null)
                {
                    //Wow.Fx.WowLog.Write("PartnerEvent 13");
                    item.ApplyDetail = eventDetail.ApplyDetail;
                }
                //Wow.Fx.WowLog.Write("PartnerEvent 14");
            }



            return tItemPriceMstList;
        }


        #endregion




        #region 연합형에 보이는 전문가 목록 가져오기

        public List<USP_GetBroadcast1ByWellList_Result> GetProWowWellList()
        {
            List<USP_GetBroadcast1ByWellList_Result> list = new List<USP_GetBroadcast1ByWellList_Result>();

            USP_GetBroadcast1ByWellList_Result model1 = db49_broadcast.USP_GetBroadcast1ByWellList("RECOMMEND_PRO1").FirstOrDefault();
            USP_GetBroadcast1ByWellList_Result model2 = db49_broadcast.USP_GetBroadcast1ByWellList("RECOMMEND_PRO2").FirstOrDefault();

            if (model1 != null)
            {
                list.Add(model1);
            }
            if (model2 != null)
            {
                list.Add(model2);
            }

            return list;
            //var wellList = db49_broadcast.Pro_wowWellList.Where(a => a.Gubun == "RECOMMEND_PRO1" || a.Gubun == "RECOMMEND_PRO2");
            //List<string> proIdList = wellList.Select(a => a.Pro_id).ToList();

            //return db49_broadcast.Pro_wowList.Where(a => proIdList.Contains(a.Pro_id) == true).ToList();
        }

        public List<Pro_wowListStockKing> GetStockKing3()
        {
            var list = db49_broadcast.Pro_wowListStockKing.Where(a => db49_broadcast.Pro_wowList.Where(b => b.Pro_id == a.Pro_id && b.State == "1").Count() > 0);
            list = list.OrderBy(a => a.Rank);
            list = list.Take(3);

            List<Pro_wowListStockKing> resultList = list.ToList();
            foreach(var item in resultList)
            {
                var pro = db49_broadcast.Pro_wowList.FirstOrDefault(a => a.Pro_id == item.Pro_id);
                item.ProName = pro.FullName;
                item.NickName = pro.NickName;
            }

            return resultList;

        }

        #endregion



        #region 온에어 에서 보여지는 부분


        public List<USP_GetTabStrategetApplication_Result> TodayStrategy()
        {
            return db49_wownet.USP_GetTabStrategetApplication(5).ToList();
        }

        public List<usp_GetlivePro_Result> GetOnAirPartnerList()
        {
            //List<Pro_wowList> resultList = new List<Pro_wowList>();

            //List<int> payNoList = db49_broadcast.PRO_SETOPTION.Where(a => a.SetOption.StartsWith("Main") == true).Select(a => a.Pay_No.Value).ToList();

            //var list = db49_broadcast.Pro_wowList.Where(a => String.IsNullOrEmpty(a.Pro_id) == false && a.State == "1");
            //list = list.Where(a => payNoList.Contains(a.Pay_no) == true);
            //list = list.Where(a => a.Specialist_gubun.Substring(1, 1) == "1");
            //list = list.Where(a => a.Pro_id.StartsWith("P") == true 
            //                    || a.Pro_id.StartsWith("F") == true
            //                    || a.Pro_id.StartsWith("H") == true
            //                    );

            //list = list.OrderBy(x => Guid.NewGuid()).Take(20);

            //resultList = list.ToList();
            //foreach(var item in resultList)
            //{
            //    var tempList = db49_broadcast.Pro_wow_junmunga_broadcast.Where(a => a.state != 2 && a.state != 5);
            //    tempList = tempList.Where(a => a.btype == "1");
            //    tempList = tempList.Where(a => a.Pro_id == item.Pro_id);
            //    tempList = tempList.OrderByDescending(a => a.state).ThenBy(a => a.bMemType).ThenBy(a => a.OnAirTime).ThenBy(a => a.brStartTime);
            //    var temp = tempList.FirstOrDefault();

            //    if (temp != null)
            //    {
            //        item.BROOM = temp.bRoom;
            //        item.BMEMTYPE = temp.bMemType;
            //        item.STATE = temp.state;
            //        item.Title = temp.bTitle;
            //    }
            //}

            //return resultList;

            List<usp_GetlivePro_Result> list = db49_broadcast.usp_GetlivePro("F", 5).ToList();
            return list;
        }



        #endregion



        #region 코너가져오기

        public ListModel<usp_GetCornerVODList_WEB_Result> GetCornerVod(string scCode, int currentIndex, int pageSize)
        {
            ListModel<usp_GetCornerVODList_WEB_Result> listModel = new ListModel<usp_GetCornerVODList_WEB_Result>();

            listModel.ListData = db49_editVOD.usp_GetCornerVODList_WEB("177", scCode, currentIndex, pageSize).ToList();
            listModel.TotalDataCount = db49_editVOD.usp_GetCornerVODList_WEB_COUNT("177", scCode, currentIndex, pageSize).FirstOrDefault().Value;

            return listModel;
        }


        public ListModel<NUP_GetCornerVODList_WEB_Result> GetCornerVodAll(int currentIndex, int pageSize)
        {
            ListModel<NUP_GetCornerVODList_WEB_Result> listModel = new ListModel<NUP_GetCornerVODList_WEB_Result>();

            listModel.ListData = db49_editVOD.NUP_GetCornerVODList_WEB("177", currentIndex, pageSize).ToList();

            return listModel;
        }


        public List<NTB_MENU> GetMenuByProgramCode(string programCode)
        {
            var list = db49_wowtv.NTB_MENU.Where(a => a.PRG_CD == programCode);
            list = list.Where(a => a.CONTENT_TYPE_CODE == "Corner");
            list = list.Where(a => a.ACTIVE_YN == "Y");
            list = list.Where(a => a.DEL_YN == "N");

            return list.ToList();
        }



        public NTB_MENU GetMenuByCornerSeq(string programCode, string scCode)
        {
            var list = db49_wowtv.NTB_MENU.Where(a => a.PRG_CD == programCode);
            list = list.Where(a => a.CONTENT_TYPE_CODE == "Corner");
            list = list.Where(a => a.CONTENT_SEQ.ToString() == scCode);

            return list.FirstOrDefault();
        }


        #endregion


        #region 수익률 가져오기

        public NTB_BOARD_CONTENT GetTrade(int boardContentSeq)
        {
            return db49_wowtv.NTB_BOARD_CONTENT.SingleOrDefault(a => a.BOARD_CONTENT_SEQ == boardContentSeq);
        }

        #endregion




        #region 메인의 우측스크롤 관련


        public ListModel<T_NEWS_PRG> SearchListRandom()
        {
            ListModel<T_NEWS_PRG> resultData = new ListModel<T_NEWS_PRG>();

            //var list = db90_DNRS.T_NEWS_PRG.Where(a => a.DELFLAG == "0");

            //list = list.Where(a =>
            //    db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.PRG_CD && b.PUBLISH_YN == "Y").Count() > 0
            //);

            //list = list.Where(a => db90_DNRS.TAB_PROGRAM_LIST.Where(b => b.PGM_ID == a.PRG_CD && b.ING_YN == "Y").Count() > 0);

            //resultData.TotalDataCount = list.Count();

            //list = list.OrderBy(x => Guid.NewGuid()).Take(4);

            //resultData.ListData = list.ToList();

            List<T_NEWS_PRG> resultList = new List<T_NEWS_PRG>();
            var list = db90_DNRS.NUP_GetRandomProgramList().ToList();
            foreach(var item in list)
            {
                T_NEWS_PRG programItem = new T_NEWS_PRG();
                programItem.PRG_CD = item.PRG_CD;
                programItem.PRG_NM = item.PRG_NM;

                resultList.Add(programItem);
            }
            resultData.TotalDataCount = resultList.Count();
            resultData.ListData = resultList;

            foreach (var item in resultData.ListData)
            {
                NTB_ATTACH_FILE thumImage = db49_wowtv.NTB_ATTACH_FILE.Where(a => a.TABLE_CODE == "T_NEWS_PRG-THUMBNAIL" && a.TABLE_KEY == item.PRG_CD).OrderByDescending(a => a.ATTACH_FILE_SEQ).FirstOrDefault();
                if (thumImage != null)
                {
                    item.ThumImageUrl = thumImage.REAL_WEB_PATH;
                }

                item.ImgSchedule = db90_DNRS.IMG_SCHEDULE.FirstOrDefault(a => a.prog_id == item.PRG_CD);

                if (item.ImgSchedule == null)
                {
                    item.ImgSchedule = new IMG_SCHEDULE();
                }
                else
                {
                    if (String.IsNullOrEmpty(item.ImgSchedule.parentid) == false)
                    {
                        var parentNews = db90_DNRS.T_NEWS_PRG.FirstOrDefault(a => a.PRG_CD == item.ImgSchedule.parentid);
                        if (parentNews != null)
                        {
                            item.ImgSchedule.ParentProgramCode = parentNews.PRG_CD;
                            item.ImgSchedule.ParentProgramName = parentNews.PRG_NM;
                        }

                        var parent = db90_DNRS.IMG_SCHEDULE.FirstOrDefault(a => a.prog_id == item.ImgSchedule.parentid);
                        if (parent != null)
                        {
                            item.ImgSchedule.ParentProgramPoint = parent.point;
                        }
                    }
                }

                item.ProgramList = db90_DNRS.TAB_PROGRAM_LIST.FirstOrDefault(a => a.PGM_ID == item.PRG_CD);
                if (item.ProgramList == null)
                {
                    item.ProgramList = new TAB_PROGRAM_LIST();
                }

                ProgramTemplateBiz programTemplateBiz = new ProgramTemplateBiz();
                var programTemplate = programTemplateBiz.GetAt(item.PRG_CD);
                if (programTemplate == null)
                {
                }
                else
                {
                    programTemplate.ProgramName = item.PRG_NM;
                    if (programTemplate.DEL_YN == "N")
                    {
                        item.ProgramTemplateType = programTemplate.TEMPLATE_TYPE;
                        var programGroup = programTemplateBiz.GetGroupFirst(programTemplate.PROGRAM_TEMPLATE_SEQ);
                        if (programGroup != null)
                        {
                            item.ProgramTemplateName = programGroup.GROUP_NAME;
                        }
                    }
                }


                var newsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == item.PRG_CD);
                if (newsProgram != null)
                {
                    item.SERVICE_NAME = newsProgram.SERVICE_NAME;
                    item.ENCODING = newsProgram.ENCODING;
                    item.PUBLISH_YN = newsProgram.PUBLISH_YN;
                    item.CP_NM = newsProgram.CP_NM;
                    item.PLAN_BROAD = newsProgram.PLAN_BROAD;
                    item.BroadTypeCode = newsProgram.BROAD_TYPE_CODE;
                    item.FameYn = newsProgram.FAME_YN;
                    item.FirstFreeYn = newsProgram.FIRST_FREE_YN;
                }
            }

            return resultData;
        }


        #endregion


        public void Migration()
        {
            var list = db90_DNRS.T_NEWS_PRG.AsQueryable();

            foreach(var item in list.ToList())
            {
                NTB_NEWS_PROGRAM newsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == item.PRG_CD);
                if (newsProgram == null)
                {
                    newsProgram = new NTB_NEWS_PROGRAM();
                    newsProgram.PRG_CD = item.PRG_CD;
                    newsProgram.BROAD_TYPE_CODE = item.BroadTypeCode_Time;

                    db90_DNRS.NTB_NEWS_PROGRAM.Add(newsProgram);
                }
                else
                {
                    newsProgram.BROAD_TYPE_CODE = item.BroadTypeCode_Time;
                }
                db90_DNRS.SaveChanges();
            }
        }


        public List<T_NEWS_PRG> GetMigrationProgramList()
        {
            var list = db90_DNRS.T_NEWS_PRG.AsQueryable();

            return list.ToList();
        }

        public List<tblSubConerList> MigrationProgramCornerList(string programCode, LoginUser loginUser)
        {
            Wow.Tv.Middle.Biz.Menu.MenuBiz menuBiz = new Menu.MenuBiz();

            var list = db49_editVOD.tblSubConerList.Where(a => a.Delflag == 0 && a.cnCode == "177" && a.PGMID == programCode);

            int existCount = db49_wowtv.NTB_MENU.Count(a => a.CONTENT_TYPE_CODE == "Corner" && a.PRG_CD == programCode);

            if (existCount == 0 && list.Count() > 0)
            {
                NTB_MENU model = new NTB_MENU();
                model.CHANNEL_CODE = "BroadProgramFront";
                model.DEPTH = 1;
                model.PRG_CD = programCode;
                model.REMARK = ".";
                model.MENU_NAME = "코너";
                model.MENU_NAME_DEPTH_1 = "코너";
                model.CONTENT_TYPE_CODE = "Corner";
                model.LINK_URL = "javascript:void(0);";
                model.ACTIVE_YN = "Y";
                model.DEL_YN = "N";

                menuBiz.SaveMig(model, loginUser);

                int upMenuSeq = model.MENU_SEQ;

                foreach (var item in list)
                {
                    NTB_MENU model2 = new NTB_MENU();

                    model2.CHANNEL_CODE = "BroadProgramFront";
                    model2.PRG_CD = programCode;
                    model2.CONTENT_TYPE_CODE = "Corner";
                    model2.UP_MENU_SEQ = upMenuSeq;
                    model2.ACTIVE_YN = "Y";
                    model2.DEL_YN = "N";
                    model2.DEPTH = 2;
                    model2.CONTENT_SEQ = int.Parse(item.scCode);
                    model2.REMARK = item.scName;
                    model2.MENU_NAME = item.scName;
                    model2.MENU_NAME_DEPTH_1 = "코너";
                    model2.MENU_NAME_DEPTH_2 = item.scName;
                    model2.LINK_URL = null;

                    menuBiz.SaveMig(model2, loginUser);
                }
            }

            return list.ToList();
        }




        public List<NUP_GetAllProgramList_Result> GetAllProgramList(string programNameStart, string programNameEnd, string year, int startIndex, int endIndex)
        {
            var list = db90_DNRS.NUP_GetAllProgramList(programNameStart, programNameEnd, year, startIndex, endIndex).ToList();
            
            return list;
        }

        public List<NUP_GetAllProgramEtcList_Result> GetAllProgramEtcList(string year, int startIndex, int endIndex)
        {
            var list = db90_DNRS.NUP_GetAllProgramEtcList(year, startIndex, endIndex).ToList();

            return list;
        }



    }
}
