using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db90.DNRS;
using Wow.Tv.Middle.Model.Db90.DNRS.NewsProgram;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Broad;
using Wow.Tv.Middle.Biz.Broad;

namespace Wow.Tv.Middle.Biz.MyProgram
{
    public class BroadWatchBiz : BaseBiz
    {
        public tv_program GetAt(int num)
        {
            tv_program model = db90_DNRS.tv_program.SingleOrDefault(a => a.Num == num);

            if (model != null)
            {
                NTB_BROAD_WATCH broadWatch = db90_DNRS.NTB_BROAD_WATCH.SingleOrDefault(a => a.Num == model.Num);
                if (broadWatch == null)
                {
                    model.PublishYn = "N";
                }
                else
                {
                    model.PublishYn = broadWatch.PUBLISH_YN;
                    model.CreateDate = broadWatch.REG_DATE;
                    model.ModifyDate = broadWatch.MOD_DATE;
                }
            }

            return model;
        }

        public ListModel<tv_program> SearchList(BroadWatchCondition condition)
        {
            ListModel<tv_program> resultData = new ListModel<tv_program>();

            var list = db90_DNRS.tv_program.AsQueryable();
            
            list = list.Where(a => a.State == 1);

            if (String.IsNullOrEmpty(condition.ProgramCode) == false)
            {
                var programGroup = new ProgramGroupBiz().GetAtByMainCode(condition.ProgramCode);
                if (programGroup == null)
                {
                    list = list.Where(a => a.Dep == condition.ProgramCode);
                }
                else
                {
                    var childList = new ProgramTemplateBiz().GetGroupListByGroupSeq(programGroup.PROGRAM_GROUP_SEQ);
                    List<string> chilCodeList = childList.Select(a => a.ProgramCode).ToList();

                    list = list.Where(a => a.Dep == condition.ProgramCode || chilCodeList.Contains(a.Dep));
                }
                
            }

            if (String.IsNullOrEmpty(condition.ProgramName) == false)
            {
                list = list.Where(a => db90_DNRS.T_NEWS_PRG.Where(b => b.PRG_CD == a.Dep && b.PRG_NM.Contains(condition.ProgramName) == true).Count() > 0);
            }
            if (String.IsNullOrEmpty(condition.BroadTypeCode) == false)
            {
                list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.Dep && b.BROAD_TYPE_CODE == condition.BroadTypeCode).Count() > 0);
            }
            if (String.IsNullOrEmpty(condition.IngYn) == false)
            {
                list = list.Where(a => db90_DNRS.TAB_PROGRAM_LIST.Where(b => b.PGM_ID == a.Dep && b.ING_YN == condition.IngYn).Count() > 0);
            }

            if (String.IsNullOrEmpty(condition.PublishYn) == false)
            {
                if (condition.PublishYn == "N")
                {
                    list = list.Where(a =>
                        db90_DNRS.NTB_BROAD_WATCH.Where(b => b.Num == a.Num).Count() == 0
                        || db90_DNRS.NTB_BROAD_WATCH.Where(b => b.Num == a.Num && b.PUBLISH_YN == "N").Count() > 0
                        );
                }
                else
                {
                    list = list.Where(a => db90_DNRS.NTB_BROAD_WATCH.Where(b => b.Num == a.Num && b.PUBLISH_YN == "Y").Count() > 0);
                }
            }

            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                switch(condition.SearchType)
                {
                    case "All":
                        list = list.Where(a => a.Title.Contains(condition.SearchText) == true || a.WriterID.Contains(condition.SearchText) == true);
                        break;
                    case "Title":
                        list = list.Where(a => a.Title.Contains(condition.SearchText) == true);
                        break;
                    case "Writer":
                        list = list.Where(a => a.WriterID.Contains(condition.SearchText) == true);
                        break;
                }
            }


            if (String.IsNullOrEmpty(condition.UploadTimeGreater) == false)
            {
                list = list.Where(a => a.UploadTime.CompareTo(condition.UploadTimeGreater) >= 0);
            }


            if (String.IsNullOrEmpty(condition.BroadSectionCode) == false)
            {
                list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.Dep && b.BROAD_SECTION_CODE == condition.BroadSectionCode).Count() > 0);
            }



            if (String.IsNullOrEmpty(condition.FameYn) == false)
            {
                if (condition.FameYn == "Y")
                {
                    list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.Dep && b.FAME_YN == "Y").Count() > 0);
                }
                else
                {
                    list = list.Where(a => db90_DNRS.NTB_NEWS_PROGRAM.Where(b => b.PRG_CD == a.Dep && b.FAME_YN == "Y").Count() == 0);
                }
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.broad_date);
            //if (String.IsNullOrEmpty(condition.OrderByType) == false)
            //{
            //    switch (condition.OrderByType)
            //    {
            //        case "Fame":
            //            list = list.OrderByDescending(a => a.ClickNum);
            //            break;
            //    }
            //}
            //else
            //{
            //    list = list.OrderByDescending(a => a.broad_date);
            //}

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
                NTB_BROAD_WATCH broadWatch = db90_DNRS.NTB_BROAD_WATCH.SingleOrDefault(a => a.Num == item.Num);
                if (broadWatch == null)
                {
                    item.PublishYn = "N";
                }
                else
                {
                    item.PublishYn = broadWatch.PUBLISH_YN;
                }

                NTB_ATTACH_FILE subImage = db49_wowtv.NTB_ATTACH_FILE.Where(a => a.TABLE_CODE == "T_NEWS_PRG-SUB" && a.TABLE_KEY == item.Dep).OrderByDescending(a => a.ATTACH_FILE_SEQ).FirstOrDefault();
                if (subImage != null)
                {
                    item.SubImageUrl = subImage.REAL_WEB_PATH;
                }
                NTB_ATTACH_FILE rectangleImage = db49_wowtv.NTB_ATTACH_FILE.Where(a => a.TABLE_CODE == "T_NEWS_PRG-RECTANGLE" && a.TABLE_KEY == item.Dep).OrderByDescending(a => a.ATTACH_FILE_SEQ).FirstOrDefault();
                if (rectangleImage != null)
                {
                    item.RectangleImageUrl = rectangleImage.REAL_WEB_PATH;
                }
                NTB_ATTACH_FILE thumImage = db49_wowtv.NTB_ATTACH_FILE.Where(a => a.TABLE_CODE == "T_NEWS_PRG-THUMBNAIL" && a.TABLE_KEY == item.Dep).OrderByDescending(a => a.ATTACH_FILE_SEQ).FirstOrDefault();
                if (thumImage != null)
                {
                    item.ThumImageUrl = thumImage.REAL_WEB_PATH;
                }

                T_NEWS_PRG newsProgram = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == item.Dep);
                if(newsProgram != null)
                {
                    item.broadStart = newsProgram.BRO_START;

                    item.DayOfWeekString = new List<string>();
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


                #region 가격구하기

                int price = 0;
                bool isFirstFree = false;
                var ntbNewsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == item.Dep);
                var imgSchedule = db90_DNRS.IMG_SCHEDULE.FirstOrDefault(a => a.prog_id == item.Dep);

                if (ntbNewsProgram != null)
                {
                    // =========================
                    // 1편 무료여부
                    if (ntbNewsProgram.FIRST_FREE_YN == "Y")
                    {
                        isFirstFree = true;
                    }
                    // 1편 무료여부
                    // =========================
                }
                if (imgSchedule != null)
                {
                    // =========================
                    // 가격

                    price = imgSchedule.point;

                    // 가격
                    // =========================


                    // =========================
                    // 1편 무료여부
                    if (String.IsNullOrEmpty(imgSchedule.parentid) == false)
                    {
                        var parentNews = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == item.Dep);
                        if (parentNews != null)
                        {
                            if (parentNews.FIRST_FREE_YN == "Y")
                            {
                                isFirstFree = true;
                            }
                        }
                    }
                    // 1편 무료여부
                    // =========================
                }

                // 1편무료여부가 체크되어 있다면
                if(isFirstFree == true)
                {
                    // 1편인지 판단
                    if (String.IsNullOrEmpty(item.Dep) == false)
                    {
                        var first = db90_DNRS.tv_program.Where(a => a.Dep == item.Dep && a.State == 1).OrderBy(a => a.broad_date).FirstOrDefault();
                        // 1편이면 0원
                        if(item.Num == first.Num)
                        {
                            price = 0;
                        }
                    }
                }

                item.Price = price;

                #endregion
            }




            #region 테스트용

            //resultData.TotalDataCount = 55;
            //List<tv_program> aa = new List<tv_program>();
            //for(int i = 0; i < resultData.TotalDataCount; i++)
            //{
            //    tv_program a = new tv_program();
            //    a.broad_date = new DateTime(2017, 10, 27, 13, i, 0).ToString("yyyyMMddHHmm");
            //    a.Program_Name = i.ToString();
            //    aa.Add(a);
            //}
            //aa = aa.OrderByDescending(q => q.broad_date).ToList();

            //if (condition.PageSize > -1)
            //{
            //    if (condition.PageSize == 0)
            //    {
            //        condition.PageSize = 20;
            //    }
            //    aa = aa.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            //}
            //resultData.ListData = aa;

            #endregion


            return resultData;
        }



        public ListModel<tv_program> SearchList2(BroadWatchCondition condition)
        {
            //string str = "";
            //int aa = 10;
            //str = aa.ToString();
            //string str2 = "";
            //int aa2 = 10;
            //str2 = aa2.ToString();

            ListModel<tv_program> resultData = new ListModel<tv_program>();
            List<tv_program> resultDataList = new List<tv_program>();

            var list = db90_DNRS.NUP_GetBroadWatchList(condition.BroadSectionCode, condition.IngYn, condition.BroadTypeCode, condition.FameYn, condition.CurrentIndex + 1, condition.CurrentIndex + condition.PageSize).ToList();
            foreach(var item in list)
            {
                resultData.TotalDataCount = (item.total == null ? 0 : item.total.Value);

                tv_program tvProgram = new tv_program();
                tvProgram.Program_Name = item.Program_Name;
                tvProgram.Num = item.Num;
                tvProgram.Dep = item.Dep;
                tvProgram.broad_date = item.broad_date;

                resultDataList.Add(tvProgram);
            }

            resultData.ListData = resultDataList;

            foreach (var item in resultData.ListData)
            {
                NTB_BROAD_WATCH broadWatch = db90_DNRS.NTB_BROAD_WATCH.SingleOrDefault(a => a.Num == item.Num);
                if (broadWatch == null)
                {
                    item.PublishYn = "N";
                }
                else
                {
                    item.PublishYn = broadWatch.PUBLISH_YN;
                }

                NTB_ATTACH_FILE subImage = db49_wowtv.NTB_ATTACH_FILE.Where(a => a.TABLE_CODE == "T_NEWS_PRG-SUB" && a.TABLE_KEY == item.Dep).OrderByDescending(a => a.ATTACH_FILE_SEQ).FirstOrDefault();
                if (subImage != null)
                {
                    item.SubImageUrl = subImage.REAL_WEB_PATH;
                }
                NTB_ATTACH_FILE rectangleImage = db49_wowtv.NTB_ATTACH_FILE.Where(a => a.TABLE_CODE == "T_NEWS_PRG-RECTANGLE" && a.TABLE_KEY == item.Dep).OrderByDescending(a => a.ATTACH_FILE_SEQ).FirstOrDefault();
                if (rectangleImage != null)
                {
                    item.RectangleImageUrl = rectangleImage.REAL_WEB_PATH;
                }
                NTB_ATTACH_FILE thumImage = db49_wowtv.NTB_ATTACH_FILE.Where(a => a.TABLE_CODE == "T_NEWS_PRG-THUMBNAIL" && a.TABLE_KEY == item.Dep).OrderByDescending(a => a.ATTACH_FILE_SEQ).FirstOrDefault();
                if (thumImage != null)
                {
                    item.ThumImageUrl = thumImage.REAL_WEB_PATH;
                }

                T_NEWS_PRG newsProgram = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == item.Dep);
                if (newsProgram != null)
                {
                    item.broadStart = newsProgram.BRO_START;

                    item.DayOfWeekString = new List<string>();
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


                #region 가격구하기

                int price = 0;
                bool isFirstFree = false;
                var ntbNewsProgram = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == item.Dep);
                var imgSchedule = db90_DNRS.IMG_SCHEDULE.FirstOrDefault(a => a.prog_id == item.Dep);

                if (ntbNewsProgram != null)
                {
                    // =========================
                    // 1편 무료여부
                    if (ntbNewsProgram.FIRST_FREE_YN == "Y")
                    {
                        isFirstFree = true;
                    }
                    // 1편 무료여부
                    // =========================
                }
                if (imgSchedule != null)
                {
                    // =========================
                    // 가격

                    price = imgSchedule.point;

                    // 가격
                    // =========================


                    // =========================
                    // 1편 무료여부
                    if (String.IsNullOrEmpty(imgSchedule.parentid) == false)
                    {
                        var parentNews = db90_DNRS.NTB_NEWS_PROGRAM.SingleOrDefault(a => a.PRG_CD == item.Dep);
                        if (parentNews != null)
                        {
                            if (parentNews.FIRST_FREE_YN == "Y")
                            {
                                isFirstFree = true;
                            }
                        }
                    }
                    // 1편 무료여부
                    // =========================
                }

                // 1편무료여부가 체크되어 있다면
                if (isFirstFree == true)
                {
                    // 1편인지 판단
                    if (String.IsNullOrEmpty(item.Dep) == false)
                    {
                        var first = db90_DNRS.tv_program.Where(a => a.Dep == item.Dep && a.State == 1).OrderBy(a => a.broad_date).FirstOrDefault();
                        // 1편이면 0원
                        if (item.Num == first.Num)
                        {
                            price = 0;
                        }
                    }
                }

                item.Price = price;

                #endregion
            }
            


            return resultData;
        }


        public int Save(tv_program model, NTB_ATTACH_FILE attachFile, LoginUser loginUser)
        {
            Wow.Fx.WowLog.Write("BroadWatch 1");

            try
            {
                var prev = GetAt(model.Num);

                Wow.Fx.WowLog.Write("BroadWatch 2");

                if (prev == null)
                {
                    //model.State = 1;
                    //model.WriterID = loginUser.LoginId;
                    //model.UploadTime = DateTime.Now.ToString("yyyyMMdd");

                    //db90_DNRS.tv_program.Add(model);
                }
                else
                {
                    prev.Title = model.Title;
                    prev.Contents = model.Contents;
                    //prev.PUBLISH_YN = model.PUBLISH_YN;
                    prev.broad_date = model.broad_date;

                }
                Wow.Fx.WowLog.Write("BroadWatch 3");
                db90_DNRS.SaveChanges();


                Wow.Fx.WowLog.Write("BroadWatch 4");

                var broadWatch = db90_DNRS.NTB_BROAD_WATCH.SingleOrDefault(a => a.Num == model.Num);
                if (broadWatch == null)
                {
                    Wow.Fx.WowLog.Write("BroadWatch 5");

                    broadWatch = new NTB_BROAD_WATCH();
                    broadWatch.Num = model.Num;
                    broadWatch.PUBLISH_YN = model.PublishYn;

                    broadWatch.REG_DATE = DateTime.Now.ToString("yyyyMMddHHmmss");
                    broadWatch.REG_ID = loginUser.LoginId;
                    broadWatch.MOD_DATE = DateTime.Now.ToString("yyyyMMddHHmmss");
                    broadWatch.MOD_ID = loginUser.LoginId;
                    db90_DNRS.NTB_BROAD_WATCH.Add(broadWatch);
                }
                else
                {
                    Wow.Fx.WowLog.Write("BroadWatch 6");

                    broadWatch.PUBLISH_YN = model.PublishYn;

                    broadWatch.MOD_DATE = DateTime.Now.ToString("yyyyMMddHHmmss");
                    broadWatch.MOD_ID = loginUser.LoginId;
                }
                Wow.Fx.WowLog.Write("BroadWatch 7");

                //db90_DNRS.Database.Log = sql => Wow.Fx.WowLog.Write(sql);
                db90_DNRS.SaveChanges();

                Wow.Fx.WowLog.Write("BroadWatch 8");

                if (attachFile != null)
                {
                    attachFile.TABLE_CODE = "tv_program";
                    attachFile.TABLE_KEY = model.Num.ToString();

                    Wow.Fx.WowLog.Write("BroadWatch 9");
                    new AttachFile.AttachFileBiz().Create(attachFile);

                    Wow.Fx.WowLog.Write("BroadWatch 10");
                }
            }
            catch(Exception ex)
            {
                Wow.Fx.WowLog.Write("BroadWatch Exception : " + ex.Message);
                if(ex.InnerException != null)
                {
                    Wow.Fx.WowLog.Write("BroadWatch Inner Exception : " + ex.InnerException.Message);
                }

                throw ex;
            }

            return model.Num;
        }


        /// <summary>
        /// 방송다시보기 생성
        /// </summary>
        /// <param name="today"></param>
        public void ExecuteInsertSp(string today, string programCode)
        {
            //db90_DNRS.usp_HDVOD_setEncodingList(today.Replace("-", "") + "01:00".Replace(":", ""), "대박천국 2부 [출동해결반의 골든타임]", "goldentime"
            //    , "goldentime", "0", "대박천국 2부 [출동해결반의 골든타임]", "P2459", today.Replace("-", "")
            //    , "<HTML>  <HEAD>  <TITLE></TITLE><!-- saved from url=(0019)http://tagfree.com/ --><!-- saved from url=(0019)http://tagfree.com/ -->  <META content=\"text / html; charset = ks_c_5601 - 1987\" http-equiv=Content-Type>  <META name=GENERATOR content=\"TAGFREE Active Designer v1.7\">  </HEAD>  <BODY style=\"FONT - SIZE: 10pt\"></BODY></HTML>");
            
            Db90_DNRS dnrs90 = new Db90_DNRS();
            var list = db90_DNRS.NUP_GetVodTodayList(today, programCode);

            today = today.Replace("-", "");

            foreach (var item in list)
            {
                //@Broad_date char(12)                 -- 방송일시 YYYYMMDDHHMM 방송일 + run_start조합
                //,@Title varchar(500)                 --프로그램 제목(프로그램명 대체가능)
                //,@FileDir varchar(50)                --폴더명
                //,@FileName varchar(20)               --파일명
                //,@bu char(1)                         -- 부 0,1,2,3... (파일명 중복을 배제하기 위함)
                //,@ProgramNM varchar(50)              --프로그램 명
                //,@Dep char(5)                        -- 프로그램 ID(P0000)
                //,@Today char(8)                      -- yyyymmdd
                //,@Content text                       --내용
                //db90_DNRS.usp_HDVOD_setEncodingList(today + item.run_start.Replace(":", ""), item.prg_nm, item.folder_name, item.file_front_name
                //    , item.bu, item.prg_nm, item.prg_cd, today, item.prog_content);

                dnrs90.usp_HDVOD_setEncodingList(today + item.run_start.Replace(":", ""), item.prg_nm, item.folder_name, item.file_front_name
                    , item.bu, item.prg_nm, item.prg_cd, today, item.prog_content);
                
                //db90_DNRS.Database.ExecuteSqlCommand("dbo.usp_HDVOD_setEncodingList", today + item.run_start.Replace(":", ""), item.prg_nm, item.folder_name, item.file_front_name
                //    , item.bu, item.prg_nm, item.prg_cd, today, item.prog_content);

            }

            var data = db90_DNRS.tv_program.SingleOrDefault(a => a.Dep.Equals(programCode) && a.UploadTime.Equals(today));
            if (data != null)
            {
                if (data.State == 0)
                {
                    data.State = 1;
                    db90_DNRS.SaveChanges();
                }

            }
        }
    }
}
