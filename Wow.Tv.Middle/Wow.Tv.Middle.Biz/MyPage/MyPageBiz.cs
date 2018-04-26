using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using ClosedXML;
using Wow.Fx;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.MyPage;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.MyPage;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.MyPage;
using Wow.Tv.Middle.Model.Db51.ARSConsult;
using Wow.Tv.Middle.Model.Db51.WOWMMS;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.MyInfo;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage;
using Wow.Tv.Middle.Model.Db90.DNRS;
using System.Text;

namespace Wow.Tv.Middle.Biz.MyPage
{
    /// <summary>
    /// <para>  Front-나의 활동 Biz</para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-09-29</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-10-21</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-09-29 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class MyPageBiz : BaseBiz
    {
        public ListModel<NUP_SC_LOG_SELECT_Result> GetSmsSearchList(ScLogCondition condition)
        {
            try
            {
                ListModel<NUP_SC_LOG_SELECT_Result> listModel=new ListModel<NUP_SC_LOG_SELECT_Result>();
                db51_ARSConsult.Database.CommandTimeout = 300;

                List<NUP_SC_LOG_SELECT_Result> scLogList = db51_ARSConsult.NUP_SC_LOG_SELECT(condition.SearchType, condition.SearchDate, condition.Phone, condition.Msg, condition.CurrentIndex, condition.PageSize).ToList();
                listModel.ListData = scLogList;

                return listModel;
            }
            catch (Exception e)
            {
                WowLog.Write("[MyPageBiz.GetSmsSearchList] " + "\r\nException: " + e);
            }

            return null;
        }


        public ListModel<NUP_MMS_LOG_SELECT_Result> GetLmsSearchList(ScLogCondition condition)
        {

            db51_ARSConsult.Database.CommandTimeout = 300;
            ListModel<NUP_MMS_LOG_SELECT_Result> listModel = new ListModel<NUP_MMS_LOG_SELECT_Result>();
            List<NUP_MMS_LOG_SELECT_Result> scLogList = Db51_WOWMMS.NUP_MMS_LOG_SELECT(condition.SearchDate,
                condition.Phone, condition.Msg, condition.CurrentIndex, condition.PageSize).ToList();
            listModel.ListData = scLogList;

            return listModel;
        }

        #region 뉴스핀

        public IList<TAB_SCRAP_CATEGORY> GetScrapCategory(LoginUserInfo loginUser)
        {
            var list = db49_wownet.TAB_SCRAP_CATEGORY.Where(x => x.USER_ID.Equals(loginUser.UserId)).ToList();
            return list;
        }

        private TAB_SCRAP_CATEGORY SingleScrapCategory(int seq)
        {
            var item = db49_wownet.TAB_SCRAP_CATEGORY.SingleOrDefault(x => x.SEQ.Equals(seq));
            return item;
        }

        public void UpdateScrapCategory(int seq, string folder)
        {
            using (var dbContextTransaction = db49_wownet.Database.BeginTransaction())
            {
                try
                {
                    TAB_SCRAP_CATEGORY scrapCategory = SingleScrapCategory(seq);
                    if (scrapCategory != null)
                    {
                        scrapCategory.FOLDER = folder;
                        db49_wownet.TAB_SCRAP_CATEGORY.AddOrUpdate(scrapCategory);
                        db49_wownet.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void ScrapCategoryDelete(int seq)
        {
            using (var dbContextTransaction = db49_wownet.Database.BeginTransaction())
            {
                try
                {
                    TAB_SCRAP_CATEGORY scrapCategory = SingleScrapCategory(seq);
                    if (scrapCategory == null) return;

                    db49_wownet.TAB_SCRAP_CATEGORY.Remove(scrapCategory);
                    db49_wownet.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 마이핀 폴더 조회 리스트
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public IList<TAB_SCRAP_MENU> GetScrapMenu(LoginUserInfo loginUser)
        {
            var list = db49_wownet.TAB_SCRAP_MENU.Where(x => x.USER_ID.Equals(loginUser.UserId)).ToList();


            if (!list.Any())
            {
                //기본 폴더
                SaveScrapMenu("기본 폴더", loginUser);
                list = db49_wownet.TAB_SCRAP_MENU.Where(x => x.USER_ID.Equals(loginUser.UserId)).ToList();
            }
            return list;
        }

        /// <summary>
        /// 마이핀 폴더 정보 가져오기
        /// </summary>
        /// <param name="mseq"></param>
        /// <returns></returns>
        private TAB_SCRAP_MENU SingleScrapMenu(int mseq)
        {
            var item = db49_wownet.TAB_SCRAP_MENU.SingleOrDefault(x => x.MSEQ.Equals(mseq));
            return item;
        }

        /// <summary>
        /// 마이핀 폴더 삭제
        /// </summary>
        /// <param name="mseq"></param>
        public void ScrapMenuDelete(int mseq, LoginUserInfo loginUser)
        {
            using (var dbContextTransaction = db49_wownet.Database.BeginTransaction())
            {
                try
                {
                    TAB_SCRAP_MENU scrapMenu = SingleScrapMenu(mseq);
                    if (scrapMenu == null) return;

                    db49_wownet.TAB_SCRAP_MENU.Remove(scrapMenu);

                    List<TAB_SCRAP_CONTENT> list = db49_wownet.TAB_SCRAP_CONTENT
                        .Where(x => x.USER_ID.Equals(loginUser.UserId) && x.BSEQ.Value.Equals(mseq)).ToList();

                    foreach (var item in list)
                    {
                        db49_wownet.TAB_SCRAP_CONTENT.Remove(item);
                    }

                    db49_wownet.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 마이핀 폴더 수정
        /// </summary>
        /// <param name="mseq"></param>
        /// <param name="folderName"></param>
        public void UpdateScrapMenu(int mseq, string folderName)
        {
            using (var dbContextTransaction = db49_wownet.Database.BeginTransaction())
            {
                try
                {
                    TAB_SCRAP_MENU scrapMenu = SingleScrapMenu(mseq);
                    if (scrapMenu != null)
                    {
                        scrapMenu.FOLDER_NAME = folderName;
                        db49_wownet.TAB_SCRAP_MENU.AddOrUpdate(scrapMenu);
                        db49_wownet.SaveChanges();
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 마이핀 폴더 추가
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="loginUser"></param>
        public void SaveScrapMenu(string folderName, LoginUserInfo loginUser)
        {
            TAB_SCRAP_MENU scrapMenu = new TAB_SCRAP_MENU
            {
                FOLDER_NAME = folderName,
                PSEQ = 0,
                USER_ID = loginUser.UserId,
                DISP_ORDER = 0,
                REG_DATE = DateTime.Now
            };
            db49_wownet.TAB_SCRAP_MENU.Add(scrapMenu);
            db49_wownet.SaveChanges();
        }

        /// <summary>
        /// 뉴스 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public ListModel<JOIN_TAB_SCRAP_CONTENT> GetNewsPin(MyPinCondition condition, LoginUserInfo loginUser)
        {

            ListModel<JOIN_TAB_SCRAP_CONTENT> resultData = new ListModel<JOIN_TAB_SCRAP_CONTENT>();

            var newsPinList = from scrapContent in db49_wownet.TAB_SCRAP_CONTENT.AsNoTracking()
                              join scrapMenu in db49_wownet.TAB_SCRAP_MENU.AsNoTracking() on scrapContent.BSEQ equals scrapMenu.MSEQ
                              where scrapMenu.USER_ID == loginUser.UserId
                                    && scrapContent.USER_ID == loginUser.UserId
                                    && scrapContent.BSEQ != 0
                              select new JOIN_TAB_SCRAP_CONTENT
                              {
                                  BSEQ = scrapContent.BSEQ,
                                  TITLE = scrapContent.TITLE,
                                  CNAME = scrapContent.CNAME,
                                  HashTag = "",
                                  PRO_ID = scrapContent.PRO_ID,
                                  REGDATE = scrapContent.REGDATE,
                                  SCRAPDATE = scrapContent.SCRAPDATE,
                                  ArticleID = scrapContent.ArticleID,
                                  SEQ = scrapContent.SEQ,
                                  URL = scrapContent.URL,
                                  USER_ID = scrapContent.USER_ID,
                                  FOLDER_NAME = scrapMenu.FOLDER_NAME
                              };

            if (condition.GroupSeq != null)
            {
                newsPinList = newsPinList.Where(a => a.BSEQ.ToString().Equals(condition.GroupSeq.ToString()));
            }

            //검색
            if (!string.IsNullOrEmpty(condition.SearchText))
            {
                if (condition.SearchType.Equals("TITLE"))
                {
                    newsPinList = newsPinList.Where(a => a.TITLE.Contains(condition.SearchText));
                }
                else if (condition.SearchType.Equals("FOLDER"))
                {
                    newsPinList = newsPinList.Where(a => a.FOLDER_NAME.Contains(condition.SearchText));
                }
                else
                {
                    newsPinList = newsPinList.Where(a =>
                        a.FOLDER_NAME.Contains(condition.SearchText) || a.TITLE.Contains(condition.SearchText));
                }

            }

            resultData.TotalDataCount = newsPinList.Count();
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                newsPinList = newsPinList.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex)
                    .Take(condition.PageSize);
            }
            resultData.ListData = newsPinList.ToList();

            //result: "삼성전자,반도체,수원"
            foreach (var item in resultData.ListData)
            {
                var hashTag = db49_Article.tblArticleList.SingleOrDefault(x => x.ArtID.Equals(item.ArticleID));

                if (!string.IsNullOrWhiteSpace(hashTag?.Tag))
                {
                    item.HashTag = hashTag.Tag;
                }
                else if (!string.IsNullOrWhiteSpace(hashTag?.Keywords))
                {
                    item.HashTag = hashTag.Keywords;
                }
            }

            return resultData;

        }

        private TAB_SCRAP_CONTENT SingleScrapContent(int seq)
        {
            var item = db49_wownet.TAB_SCRAP_CONTENT.SingleOrDefault(x => x.SEQ.Equals(seq));
            return item;
        }

        /// <summary>
        /// 뉴스 핀 정보를 삭제한다.
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="loginUser">로그인유저정보</param>
        public void NewsPinDelete(int seq, LoginUserInfo loginUser)
        {
            //db49_wownet.TAB_SCRAP_CONTENT
            TAB_SCRAP_CONTENT content = SingleScrapContent(seq);

            if (content == null) return;

            using (var dbContextTransaction = db49_wownet.Database.BeginTransaction())
            {
                try
                {
                    db49_wownet.TAB_SCRAP_CONTENT.Remove(content);
                    db49_wownet.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region 프로그램 핀

        /// <summary>
        /// 프로그램 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public ListModel<NTB_MYPIN_PROGRAM> GetProgramPin(MyPinCondition condition, LoginUserInfo loginUser)
        {

            ListModel<NTB_MYPIN_PROGRAM> resultData = new ListModel<NTB_MYPIN_PROGRAM>();

            var programPinList = db49_wowtv.NTB_MYPIN_PROGRAM.AsNoTracking().AsQueryable()
                .Where(x => x.USER_ID.Equals(loginUser.UserId)).ToList();


            foreach (var item in programPinList)
            {
                var programInfo = db90_DNRS.T_NEWS_PRG.AsNoTracking().AsQueryable()
                    .SingleOrDefault(x => x.PRG_CD.Equals(item.PRG_CD) && x.DELFLAG.Equals("0"));
                var tvProgramInfo = db90_DNRS.tv_program.AsNoTracking().AsQueryable()
                    .OrderByDescending(x => x.UploadTime).FirstOrDefault(x => x.Dep.Equals(item.PRG_CD));


                if (programInfo != null)
                {

                    item.SUB_IMG = new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-SUB", programInfo.PRG_CD)
                        ?.REAL_WEB_PATH;
                    item.REC_IMG = new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-RECTANGLE", programInfo.PRG_CD)
                        ?.REAL_WEB_PATH;
                    item.BRO_START = programInfo.BRO_START;
                    item.BRO_END = programInfo.BRO_END;
                    item.PRG_NM = programInfo.PRG_NM;
                    item.PGMDAY = ConvertBroadDay(programInfo.PGMDAY);
                }

                if (tvProgramInfo != null)
                {
                    item.TV_REPLAY = tvProgramInfo.Program_Name;
                    item.BroadWatchNumber = tvProgramInfo.Num;
                }
            }

            //검색
            if (!string.IsNullOrEmpty(condition.SearchText))
            {
                programPinList = programPinList.Where(a => a.PRG_NM.Contains(condition.SearchText)).ToList();
            }

            resultData.TotalDataCount = programPinList.Count;
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                programPinList = programPinList.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex)
                    .Take(condition.PageSize).ToList();
            }

            resultData.ListData = programPinList;

            return resultData;
        }


        private string ConvertBroadDay(int? pgmday)
        {
            string dayName = "";
            if (pgmday == null)
            {
                dayName = "매일";
            }

            if ((pgmday & 1) > 0)
            {
                dayName += "월";
            }
            if ((pgmday & 16) > 0)
            {
                dayName += "화";
            }
            if ((pgmday & 256) > 0)
            {
                dayName += "수";
            }
            if ((pgmday & 4096) > 0)
            {
                dayName += "목";
            }
            if ((pgmday & 65536) > 0)
            {
                dayName += "금";
            }
            if ((pgmday & 1048576) > 0)
            {
                dayName += "토";
            }

            if ((pgmday & 16777216) > 0)
            {
                dayName += "일";
            }

            return dayName;
        }

        private NTB_MYPIN_PROGRAM SingleProgramPin(int seq)
        {
            var item = db49_wowtv.NTB_MYPIN_PROGRAM.SingleOrDefault(x => x.SEQ.Equals(seq));
            return item;
        }

        /// <summary>
        /// 프로그램 핀 정보를 삭제한다.
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="loginUser">로그인유저정보</param>
        public void ProgramPinDelete(int seq, LoginUserInfo loginUser)
        {
            //db49_wownet.TAB_SCRAP_CONTENT
            NTB_MYPIN_PROGRAM pin = SingleProgramPin(seq);

            if (pin == null) return;

            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    db49_wowtv.NTB_MYPIN_PROGRAM.Remove(pin);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 이 시간 추천 프로그램
        /// </summary>
        /// <returns></returns>
        public IList<T_RUNDOWN> GetRecommendProgram()
        {
            IList<T_RUNDOWN> recommendProgramList = new List<T_RUNDOWN>();

            string today = DateTime.Now.ToString("yyyy-MM-dd");
            var todayList = db90_DNRS.T_RUNDOWN
                .Where(x => x.RUN_DATE.Equals(today) && !x.INNING.Equals("0") && x.DELETE_FLAG.Equals("N"))
                .OrderBy(x => x.RUN_START).ToList();


            int runTime = int.Parse(DateTime.Now.ToString("HHmm"));


            for (int i = 0; i < todayList.Count; i++)
            {
                int start = int.Parse(todayList[i].RUN_START.Replace(":", ""));


                WowLog.Write("런타임 값 : " + runTime.GetType() + " 실제값 : " + runTime);
                if (string.Compare(todayList[i].RUN_START, runTime.ToString(), StringComparison.Ordinal) <= 0 && string.Compare(todayList[i].RUN_END, runTime.ToString(), StringComparison.Ordinal) > 0)
                {
                    todayList[i].Status = "Ing";
                }

                if (recommendProgramList.Count == 0 && runTime <= start)
                {
                    todayList[i - 2].SUB_IMG = new AttachFile.AttachFileBiz()
                        .GetAt("T_NEWS_PRG-SUB", todayList[i - 2].PRG_CD)?.REAL_WEB_PATH;
                    todayList[i - 1].SUB_IMG = new AttachFile.AttachFileBiz()
                        .GetAt("T_NEWS_PRG-SUB", todayList[i - 1].PRG_CD)?.REAL_WEB_PATH;
                    todayList[i].SUB_IMG = new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-SUB", todayList[i].PRG_CD)
                        ?.REAL_WEB_PATH;


                    todayList[i - 2].REC_IMG = new AttachFile.AttachFileBiz()
                        .GetAt("T_NEWS_PRG-RECTANGLE", todayList[i - 2].PRG_CD)?.REAL_WEB_PATH;
                    todayList[i - 1].REC_IMG = new AttachFile.AttachFileBiz()
                        .GetAt("T_NEWS_PRG-RECTANGLE", todayList[i - 1].PRG_CD)?.REAL_WEB_PATH;
                    todayList[i].REC_IMG = new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-RECTANGLE", todayList[i].PRG_CD)
                        ?.REAL_WEB_PATH;


                    recommendProgramList.Add(todayList[i - 2]);
                    recommendProgramList.Add(todayList[i - 1]);
                    recommendProgramList.Add(todayList[i]);
                    break;
                }
            }


            return recommendProgramList;

        }

        #endregion

        #region 파트너 핀

        /// <summary>
        /// 파트너 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public ListModel<NTB_MYPIN_PARTNER> GetPartnerPin(MyPinCondition condition, LoginUserInfo loginUser)
        {

            ListModel<NTB_MYPIN_PARTNER> resultData = new ListModel<NTB_MYPIN_PARTNER>();

            var partnerPinList = db49_wowtv.NTB_MYPIN_PARTNER.AsNoTracking().AsQueryable();

            partnerPinList = partnerPinList.Where(a => a.USER_ID.Equals(loginUser.UserId));

            resultData.ListData = partnerPinList.ToList();

            foreach (var ntbMypinPartner in resultData.ListData)
            {
                var partnerinfo = db49_broadcast.Pro_wowList.AsNoTracking().AsQueryable()
                    .SingleOrDefault(x => x.Pay_no.Equals(ntbMypinPartner.PAY_NO));

                ntbMypinPartner.NEWphoto_small = partnerinfo?.NEWphoto_small;
                ntbMypinPartner.FullName = partnerinfo?.NickName; //필명으로 수정
                ntbMypinPartner.PRO_ID = partnerinfo?.Pro_id;

                var broadCast = db49_broadcast.USP_GetBroadcast1ByProId(partnerinfo?.Pro_id).SingleOrDefault();

                if (string.IsNullOrWhiteSpace(broadCast?.BRSTARTTIME) ||
                    string.IsNullOrWhiteSpace(broadCast?.BRENDTIME))
                {
                    ntbMypinPartner.BroadCastTime = "금일 예정된 방송이 없습니다.";
                }
                else
                {
                    string startTime = $"{broadCast.BRSTARTTIME.Substring(0, 4)}-{broadCast.BRSTARTTIME.Substring(4, 2)}-{broadCast.BRSTARTTIME.Substring(6, 2)} {broadCast.BRSTARTTIME.Substring(8, 2)}:{broadCast.BRSTARTTIME.Substring(10, 2)}";
                    string endTime = $"{broadCast.BRENDTIME.Substring(0, 4)}-{broadCast.BRENDTIME.Substring(4, 2)}-{broadCast.BRENDTIME.Substring(6, 2)} {broadCast.BRENDTIME.Substring(8, 2)}:{broadCast.BRENDTIME.Substring(10, 2)}";

                    ntbMypinPartner.BroadCastTime = startTime + "~" + endTime;
                }

                var cafeMasterInfo = db49_wownet.usp_Select_MasterOpenCafeInfo(partnerinfo?.Wowtv_id).SingleOrDefault();

                if(cafeMasterInfo != null)
                {
                    var cafeGradeLevel = db49_wowcafe.CafeMemberInfo.SingleOrDefault(x =>
                    x.UserID.Equals(loginUser.UserId) && x.CafeCode.Equals(cafeMasterInfo.CafeCode));

                    if (cafeGradeLevel == null)
                    {
                        var boardInfo = db49_wowcafe.usp_Select_TopNewBoard(cafeMasterInfo?.CafeCode, 1, 0)
                            .SingleOrDefault();
                        ntbMypinPartner.BoardTitle = boardInfo?.Title;
                        ntbMypinPartner.CafeDomain = cafeMasterInfo?.CafeDomain;
                        ntbMypinPartner.BoardType = boardInfo?.BoardType;
                        ntbMypinPartner.CafeCode = cafeMasterInfo?.CafeCode.ToString();
                        ntbMypinPartner.Num = boardInfo?.Num.ToString();
                        ntbMypinPartner.BoardCode = boardInfo?.boardcode;
                    }
                    else
                    {
                        var boardInfo = db49_wowcafe
                            .usp_Select_TopNewBoard(cafeMasterInfo?.CafeCode, 1, cafeGradeLevel.GradeLevel)
                            .SingleOrDefault();
                        ntbMypinPartner.BoardTitle = boardInfo?.Title;
                        ntbMypinPartner.CafeDomain = cafeMasterInfo?.CafeDomain;
                        ntbMypinPartner.BoardType = boardInfo?.BoardType;
                        ntbMypinPartner.CafeCode = cafeMasterInfo?.CafeCode.ToString();
                        ntbMypinPartner.Num = boardInfo?.Num.ToString();
                        ntbMypinPartner.BoardCode = boardInfo?.boardcode;

                    }
                }
                
            }

            //검색
            if (!string.IsNullOrEmpty(condition.SearchText))
            {
                resultData.ListData =
                    resultData.ListData.Where(a => a.FullName.Contains(condition.SearchText)).ToList();
            }

            resultData.TotalDataCount = resultData.ListData.Count;
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                resultData.ListData = resultData.ListData.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex)
                    .Take(condition.PageSize).ToList();
            }

            return resultData;
        }

        private NTB_MYPIN_PARTNER SinglePartnerPin(int seq)
        {
            var item = db49_wowtv.NTB_MYPIN_PARTNER.SingleOrDefault(x => x.SEQ.Equals(seq));
            return item;
        }

        /// <summary>
        /// 파트너 핀 정보를 삭제한다.
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="loginUser">로그인유저정보</param>
        public void PartnerPinDelete(int seq, LoginUserInfo loginUser)
        {
            //db49_wownet.TAB_SCRAP_CONTENT
            NTB_MYPIN_PARTNER pin = SinglePartnerPin(seq);

            if (pin == null) return;

            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    db49_wowtv.NTB_MYPIN_PARTNER.Remove(pin);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 오늘의 공개방송 파트너
        /// </summary>
        /// <returns></returns>
        public IList<USP_GetRecommendPro3_Result> GetRecommendPartner(bool isStockDate = false)
        {
            IList<USP_GetRecommendPro3_Result> resultData = db49_broadcast.USP_GetRecommendPro3().ToList();

            foreach (var item in resultData)
            {
                var broadCast = db49_broadcast.USP_GetBroadcast1ByProId(item.PRO_ID).SingleOrDefault();

                if (!string.IsNullOrWhiteSpace(broadCast?.BMEMTYPE))
                {

                    if (broadCast.BMEMTYPE.Equals("N") || broadCast.BMEMTYPE.Equals("U"))
                    {
                        item.BoradType = "회원전용";
                    }
                    else if (broadCast.BMEMTYPE.Equals("F"))
                    {
                        item.BoradType = "무료방송";
                    }
                }

                //와우넷으로 하드코딩
                item.NEWPHOTO_SMALL = "http://image.wownet.co.kr/" + item.NEWPHOTO_SMALL.Replace("\\", "/");

                if (string.IsNullOrWhiteSpace(broadCast?.BRSTARTTIME) ||
                    string.IsNullOrWhiteSpace(broadCast?.BRENDTIME))
                {
                    item.BroadCastTime = "-";
                }
                else
                {
                    if (isStockDate)
                    {
                        //2017.12.05 (15:40~19:40)
                        item.BroadCastTime =
                            $"{broadCast.BRSTARTTIME.Substring(0, 4)}.{broadCast.BRSTARTTIME.Substring(5, 2)}.{broadCast.BRSTARTTIME.Substring(7, 2)} ({broadCast.BRSTARTTIME.Substring(8, 2)}:{broadCast.BRSTARTTIME.Substring(10, 2)}" +
                            "~" + $"{broadCast.BRENDTIME.Substring(8, 2)}:{broadCast.BRENDTIME.Substring(10, 2)})";
                    }
                    else
                    {
                        item.BroadCastTime =
                            $"{broadCast.BRSTARTTIME.Substring(8, 2)}:{broadCast.BRSTARTTIME.Substring(10, 2)}" + "~" +
                            $"{broadCast.BRENDTIME.Substring(8, 2)}:{broadCast.BRENDTIME.Substring(10, 2)}";
                    }

                }
            }

            return resultData;
        }

        #endregion

        #region 기자 핀

        /// <summary>
        /// 기자 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public ListModel<NUP_MYPIN_REPORTER_SELECT_Result> GetReporterPin(MyPinCondition condition,
            LoginUserInfo loginUser)
        {

            ListModel<NUP_MYPIN_REPORTER_SELECT_Result> resultData = new ListModel<NUP_MYPIN_REPORTER_SELECT_Result>();
            List<NUP_MYPIN_REPORTER_SELECT_Result> mypinReporterList =
                db49_Article.NUP_MYPIN_REPORTER_SELECT(condition.SearchText, loginUser.UserId).ToList();

            resultData.TotalDataCount = mypinReporterList.Count;
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                mypinReporterList = mypinReporterList.OrderByDescending(a => a.SCRAPDATE).Skip(condition.CurrentIndex)
                    .Take(condition.PageSize).ToList();
            }
            resultData.ListData = mypinReporterList;

            return resultData;
        }

        private NTB_MYPIN_REPORTER SingleReporterPin(int seq)
        {
            var item = db49_wowtv.NTB_MYPIN_REPORTER.SingleOrDefault(x => x.SEQ.Equals(seq));
            return item;
        }

        /// <summary>
        /// 기자 핀 정보를 삭제한다.
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="loginUser">로그인유저정보</param>
        public void ReporterPinDelete(int seq, LoginUserInfo loginUser)
        {
            //db49_wownet.TAB_SCRAP_CONTENT
            NTB_MYPIN_REPORTER pin = SingleReporterPin(seq);

            if (pin == null) return;

            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    db49_wowtv.NTB_MYPIN_REPORTER.Remove(pin);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region 글 모음

        public ListModel<TAB_CONSULTATION_APPLICATION> GetStockConsultList(MyWriteCollectCondition condition)
        {

            ListModel<TAB_CONSULTATION_APPLICATION> resultData = new ListModel<TAB_CONSULTATION_APPLICATION>();


            var list = db49_wownet.TAB_CONSULTATION_APPLICATION.AsQueryable();

            list = list.Where(a =>
                a.BCODE.Equals("N03010100") && a.MY_WRITE.Equals("1") && a.USER_ID.Equals(condition.LoginId));


            resultData.TotalDataCount = list.Count();
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            return resultData;
        }

        public ListModel<TAB_BOARD_AA> GetStockDebateList(MyWriteCollectCondition condition)
        {

            ListModel<TAB_BOARD_AA> resultData = new ListModel<TAB_BOARD_AA>();


            var list = db49_wownet.TAB_BOARD.AsQueryable();


            list = list.Where(a =>
                a.VIEW_FLAG.Equals("Y") && a.BCODE.Equals("N03040000") && a.MY_WRITE.Value.Equals(1) &&
                a.USER_ID.Equals(condition.LoginId));


            resultData.TotalDataCount = list.Count();
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            return resultData;
        }

        public ListModel<TAB_BOARD_AA> GetDealList(MyWriteCollectCondition condition)
        {

            ListModel<TAB_BOARD_AA> resultData = new ListModel<TAB_BOARD_AA>();


            var list = db49_wownet.TAB_BOARD.AsQueryable();

            list = list.Where(a =>
                a.VIEW_FLAG.Equals("Y") && a.BCODE.Equals("N03030200") && a.MY_WRITE.Value.Equals(1) &&
                a.USER_ID.Equals(condition.LoginId));


            resultData.TotalDataCount = list.Count();
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            return resultData;
        }

        public ListModel<TAB_BOARD_AA> GetStockInfoList(MyWriteCollectCondition condition)
        {

            ListModel<TAB_BOARD_AA> resultData = new ListModel<TAB_BOARD_AA>();


            var list = db49_wownet.TAB_BOARD.AsQueryable();

            list = list.Where(a =>
                a.VIEW_FLAG.Equals("Y") && a.BCODE.Equals("N01099900") &&
                a.USER_ID.Equals(condition.LoginId));


            resultData.TotalDataCount = list.Count();
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            return resultData;
        }

        #endregion


        #region Web에서 마이핀 등록

        /// <summary>
        /// 기자 폴더 추가
        /// </summary>
        /// <param name="mypin"></param>
        /// <param name="loginUser"></param>
        public void SaveReporter(NTB_MYPIN_REPORTER mypin, LoginUserInfo loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {

                    if (string.IsNullOrWhiteSpace(mypin.REPORTER_ID))
                    {
                        var exceptionMessage = "기자 정보를 확인하세요.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    var checkItem =
                        db49_wowtv.NTB_MYPIN_REPORTER.SingleOrDefault(x =>
                            x.USER_ID.Equals(loginUser.UserId) && x.REPORTER_ID.Equals(mypin.REPORTER_ID));

                    if (checkItem != null)
                    {
                        var exceptionMessage = "이미 등록 되었습니다.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    mypin.REGDATE = DateTime.Now;
                    mypin.SCRAPDATE = DateTime.Now;
                    mypin.USER_ID = loginUser.UserId;
                    db49_wowtv.NTB_MYPIN_REPORTER.AddOrUpdate(mypin);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 파트너 마이핀 추가
        /// </summary>
        /// <param name="mypin"></param>
        /// <param name="loginUser"></param>
        public void SavePartner(NTB_MYPIN_PARTNER mypin, LoginUserInfo loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {

                    if (mypin.PAY_NO < 0)
                    {
                        var exceptionMessage = "파트너 정보를 확인하세요.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    var checkItem = db49_wowtv.NTB_MYPIN_PARTNER.SingleOrDefault(x =>
                        x.USER_ID.Equals(loginUser.UserId) && x.PAY_NO.Equals(mypin.PAY_NO));

                    if (checkItem != null)
                    {
                        var exceptionMessage = "이미 등록 되었습니다.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    mypin.REGDATE = DateTime.Now;
                    mypin.SCRAPDATE = DateTime.Now;
                    mypin.USER_ID = loginUser.UserId;
                    db49_wowtv.NTB_MYPIN_PARTNER.AddOrUpdate(mypin);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 프로그램 마이핀 추가
        /// </summary>
        /// <param name="mypin"></param>
        /// <param name="loginUser"></param>
        public void SaveProgram(NTB_MYPIN_PROGRAM mypin, LoginUserInfo loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {

                    if (string.IsNullOrWhiteSpace(mypin.PRG_CD))
                    {
                        var exceptionMessage = "프로그램 정보를 확인하세요.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    var checkItem = db49_wowtv.NTB_MYPIN_PROGRAM.SingleOrDefault(x =>
                        x.USER_ID.Equals(loginUser.UserId) && x.PRG_CD.Equals(mypin.PRG_CD));

                    if (checkItem != null)
                    {
                        var exceptionMessage = "이미 등록 되었습니다.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    mypin.REGDATE = DateTime.Now;
                    mypin.SCRAPDATE = DateTime.Now;
                    mypin.USER_ID = loginUser.UserId;
                    db49_wowtv.NTB_MYPIN_PROGRAM.AddOrUpdate(mypin);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 뉴스 마이핀 추가
        /// </summary>
        /// <param name="mypin"></param>
        /// <param name="loginUser"></param>
        public void SaveNews(TAB_SCRAP_CONTENT mypin, LoginUserInfo loginUser)
        {
            using (var dbContextTransaction = db49_wownet.Database.BeginTransaction())
            {
                try
                {

                    if (mypin.BSEQ == null)
                    {
                        var exceptionMessage = "뉴스 정보를 확인하세요.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    var checkItem = db49_wownet.TAB_SCRAP_CONTENT.SingleOrDefault(x =>
                        x.BSEQ.Value.Equals(mypin.BSEQ.Value) && x.USER_ID.Equals(loginUser.UserId) &&
                        x.ArticleID.Contains(mypin.ArticleID));

                    if (checkItem != null)
                    {
                        var exceptionMessage = "이미 등록 되었습니다.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    mypin.REGDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    mypin.SCRAPDATE = DateTime.Now;
                    mypin.USER_ID = loginUser.UserId;
                    mypin.PRO_ID = ""; //이게 뭐지
                    mypin.CNAME = "";
                    db49_wownet.TAB_SCRAP_CONTENT.AddOrUpdate(mypin);
                    db49_wownet.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }


        /// <summary>
        /// 뉴스 마이핀 등록 여부 (아이콘 처리 시 사용)
        /// </summary>
        /// <param name="mypin"></param>
        /// <param name="loginUser"></param>
        public TAB_SCRAP_CONTENT SingleNewsPin(string articleId, LoginUserInfo loginUser)
        {
            var item = db49_wownet.TAB_SCRAP_CONTENT.SingleOrDefault(x =>
                x.USER_ID.Equals(loginUser.UserId) && x.ArticleID.Contains(articleId));
            return item;
        }

        #endregion

        #region 나의 캐시

        public USP_GetMYCurrentCash_Result GetMyCashInfo(LoginUserInfo loginUser)
        {
            var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUser.UserId));

            if (userInfo != null)
            {
                return db89_wowbill.USP_GetMYCurrentCash(userInfo.userNumber.ToString()).SingleOrDefault();
            }
            return new USP_GetMYCurrentCash_Result();


        }

        public string TestGetBillBalance(string usernumber, string gamecode, string boQv5BillHost)
        {
            try
            {

                BOQv7BillLib.BillClass couponBill1 = new BOQv7BillLib.BillClass
                {
                    TxCmd = "getbalance",
                    HOST = boQv5BillHost,
                    CodePage = 0
                };
                couponBill1.SetField("userno", usernumber); // 골드플러스 1개월 무료서비스 쿠폰ID / 62
                couponBill1.SetField("gamecode", gamecode); // 유효기간 유형 (4:일, 5:월)	// K100204
                int billReturnValue = couponBill1.StartAction();

                if (billReturnValue != 0)
                {
                    WowLog.Write("MyPageBiz.GetBillBalance > 이벤트/제휴사 캐시 조회용 실패: " + "boQv5BillHost : " + boQv5BillHost +
                                 " 메시지: " + couponBill1.ErrMsg);
                }
                //00311getbalance      0000N484plsrlno632889656-13556-11228-3userno0useridusernamepersonnobirthdategender3cashreal0.00cashbonus0.00mileage0tincashreal0.00tincashbonus0.00tinmileage0toutcashreal0.00toutcashbonus0.00toutmileage0signflag1coopcash0.00usestate99regdateupddate
                return couponBill1.GetVal("toutcashreal");
            }
            catch (Exception ex)
            {
                WowLog.Write("MyPageBiz.GetBillBalance > 이벤트/제휴사 캐시 조회용 실패: " + ex.Message);

            }
            return "";
        }


        public double GetBillBalance(LoginUserInfo loginUserInfo, string gamecode, string boQv5BillHost)
        {
            try
            {
                var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));

                if (userInfo != null)
                {

                    BOQv7BillLib.BillClass couponBill1 = new BOQv7BillLib.BillClass
                    {
                        TxCmd = "getbalance",
                        HOST = boQv5BillHost,
                        CodePage = 0
                    };
                    couponBill1.SetField("userno", userInfo.userNumber.ToString()); // 골드플러스 1개월 무료서비스 쿠폰ID / 62
                    couponBill1.SetField("gamecode", gamecode); // 유효기간 유형 (4:일, 5:월)	// K100204
                    int billReturnValue = couponBill1.StartAction();

                    if (billReturnValue != 0)
                    {
                        WowLog.Write("MyPageBiz.GetBillBalance > 이벤트/제휴사 캐시 조회용 실패: " + couponBill1.ErrMsg);
                    }

                    double returnValue = double.Parse(couponBill1.GetVal("cashreal")) + double.Parse(couponBill1.GetVal("cashbonus"));


                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                WowLog.Write("MyPageBiz.GetBillBalance > 이벤트/제휴사 캐시 조회용 실패: " + ex.Message);

            }
            return 0;
        }


        public ListModel<UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result> GetCashList(LoginUserInfo loginUserInfo,
            CashCondition condition)
        {

            ListModel<UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result> resultData = new ListModel<UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result>();

            var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));
            string startDate;
            string endDate;
            //기간 검색
            if (condition.START_DATE.Year < 1900 || condition.END_DATE.Year < 1900)
            {
                startDate = DateTime.Now.ToString("yyyyMMdd");
                endDate = DateTime.Now.ToString("yyyyMMdd");
            }
            else
            {
                startDate = condition.START_DATE.ToString("yyyyMMdd");
                endDate = condition.END_DATE.ToString("yyyyMMdd");
            }


            if (userInfo != null)
            {
                ObjectParameter output = new ObjectParameter("po_intRecordCnt", typeof(Int32));
                var list = db89_WOWTV_BILL_DB
                    .UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST(userInfo.userNumber, startDate, endDate, condition.CurrentIndex, condition.PageSize, output)
                    .ToList();

                resultData.TotalDataCount = (int)output.Value;
                //if (condition.PageSize > -1)
                //{
                //    if (condition.PageSize == 0)
                //    {
                //        condition.PageSize = 10;
                //    }
                //    list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
                //}
                resultData.ListData = list;
            }
            return resultData;
        }


        /// <summary>
        /// [S] 개인 충전/캐시 정보 -------------------------------------------------------------------------------
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="gamecode"></param>
        /// <param name="boQv5BillHost"></param>
        /// <returns></returns>
        public BillBalance GetWowTvBalance(LoginUserInfo loginUserInfo, string boQv5BillHost)
        {
            BillBalance balance = new BillBalance();
            try
            {
                var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));

                if (userInfo != null)
                {

                    BOQv7BillLib.BillClass couponBill1 = new BOQv7BillLib.BillClass
                    {
                        TxCmd = "getbalance",
                        HOST = boQv5BillHost,
                        CodePage = 0
                    };
                    couponBill1.SetField("userno", userInfo.userNumber.ToString());
                    couponBill1.SetField("gamecode", "wowtv");
                    int billReturnValue = couponBill1.StartAction();

                    if (billReturnValue != 0)
                    {
                        WowLog.Write("MyPageBiz.GetWowTvBalance > 개인 충전/캐시 정보 실패: " + couponBill1.ErrMsg);
                    }

                    if (double.TryParse(couponBill1.GetVal("cashreal"), out var cashreal))
                    {
                        balance.nBalance = cashreal;
                        balance.sb_cashreal = cashreal;
                    }

                    if (double.TryParse(couponBill1.GetVal("cashbonus"), out var cashbonus))
                    {
                        balance.nBalance += cashbonus;
                        balance.sb_cashbonus = cashbonus;
                    }

                    balance.sb_userno = couponBill1.GetVal("userno");

                    if (double.TryParse(couponBill1.GetVal("mileage"), out var mileage))
                    {
                        balance.sb_mileage += mileage;
                    }
                    if (double.TryParse(couponBill1.GetVal("tincashreal"), out var tincashreal))
                    {
                        balance.sb_tincashreal += tincashreal;
                    }
                    if (double.TryParse(couponBill1.GetVal("tincashbonus"), out var tincashbonus))
                    {
                        balance.sb_tincashbonus += tincashbonus;
                    }
                    if (double.TryParse(couponBill1.GetVal("tinmileage"), out var tinmileage))
                    {
                        balance.sb_tinmileage += tinmileage;
                    }
                    if (double.TryParse(couponBill1.GetVal("toutcashreal"), out var toutcashreal))
                    {
                        balance.sb_toutcashreal += toutcashreal;
                    }
                    if (double.TryParse(couponBill1.GetVal("toutcashbonus"), out var toutcashbonus))
                    {
                        balance.sb_toutcashbonus += toutcashbonus;
                    }
                    if (double.TryParse(couponBill1.GetVal("toutmileage"), out var toutmileage))
                    {
                        balance.sb_toutmileage += toutmileage;
                    }

                    if (DateTime.TryParse(couponBill1.GetVal("regdate"), out var regdate))
                    {
                        balance.sb_regdate = regdate;
                    }
                }
            }
            catch (Exception ex)
            {
                WowLog.Write("MyPageBiz.GetWowTvBalance > 개인 충전/캐시 정보실패: " + ex.Message);

            }
            return balance;
        }

        public ListModel<UP_PORTAL_REFUND_LST_Result> GetRefundList(LoginUserInfo loginUserInfo,
            CashCondition condition)
        {

            ListModel<UP_PORTAL_REFUND_LST_Result> resultData = new ListModel<UP_PORTAL_REFUND_LST_Result>();

            var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));
            string startDate;
            string endDate;
            //기간 검색
            if (condition.START_DATE.Year < 1900 || condition.END_DATE.Year < 1900)
            {
                startDate = DateTime.Now.ToString("yyyyMMdd");
                endDate = DateTime.Now.ToString("yyyyMMdd");
            }
            else
            {
                startDate = condition.START_DATE.ToString("yyyyMMdd");
                endDate = condition.END_DATE.Year.ToString("yyyyMMdd");
            }


            if (userInfo != null)
            {
                ObjectParameter output = new ObjectParameter("po_intRecordCnt", typeof(Int32));
                var list = db89_WOWTV_BILL_DB
                    .UP_PORTAL_REFUND_LST(null, userInfo.userNumber, startDate, endDate, 999, 1, output).ToList();

                resultData.TotalDataCount = (int)output.Value;
                //if (condition.PageSize > -1)
                //{
                //    if (condition.PageSize == 0)
                //    {
                //        condition.PageSize = 20;
                //    }
                //    list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
                //}
                resultData.ListData = list;
            }
            return resultData;
        }

        #endregion

        #region 마이페이지 메인화면

        public int GetCouponCount(LoginUserInfo loginUserInfo, int useState)
        {
            ListModel<UP_PORTAL_COUPON_LST_Result> resultData = new ListModel<UP_PORTAL_COUPON_LST_Result>();

            var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));

            if (userInfo != null)
            {
                var list = db89_WOWTV_BILL_DB.UP_PORTAL_COUPON_LST(userInfo.userNumber, useState).ToList();

                resultData.TotalDataCount = list.Count;
            }
            return resultData.TotalDataCount;
        }


        #endregion

        #region 나의 쿠폰

        public ListModel<UP_PORTAL_COUPON_LST_Result> GetCouponList(LoginUserInfo loginUserInfo,
            BaseCondition condition, int useState)
        {
            ListModel<UP_PORTAL_COUPON_LST_Result> resultData = new ListModel<UP_PORTAL_COUPON_LST_Result>();

            var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));

            if (userInfo != null)
            {
                var list = db89_WOWTV_BILL_DB.UP_PORTAL_COUPON_LST(userInfo.userNumber, useState).ToList();

                resultData.TotalDataCount = list.Count;
                if (condition.PageSize > -1)
                {
                    if (condition.PageSize == 0)
                    {
                        condition.PageSize = 20;
                    }
                    list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
                }
                resultData.ListData = list;
            }
            return resultData;
        }


        public CouponResult CouponChceck(string couponNo,string chkmyinfo)
        {
            CouponResult result = new CouponResult();

            var couponItem = (from couponMst in db89_WOWTV_BILL_DB.TCouponMst.AsNoTracking()
                              join couponPubMst in db89_WOWTV_BILL_DB.TCouponPubMst.AsNoTracking().DefaultIfEmpty() on
                                  couponMst.CouponID equals couponPubMst.CouponID
                              where couponMst.CouponNo.Equals(couponNo)
                              select couponPubMst
            ).AsEnumerable().SingleOrDefault();


            result.CouponGroup = 1;
            result.Reason = "";
            result.CouponNo = couponNo;

            if (couponItem == null)
            {
                result.ResultMessage = "유효하지 않은 쿠폰입니다.";
                //return result;
            }
            else
            {
                result.CouponType = couponItem.CouponType.ToString();
                result.ApplyDetail = couponItem.ApplyDetail;
                if (couponItem.CouponType.Equals(2) && !chkmyinfo.Equals("Y"))
                {
                    result.CouponNo = couponNo;
                    result.ResultMessage = "상품지급 쿠폰(약관/동의 처리하지 않은 경우 팝업 호출)";
                    //return result;
                }

            }

           string tempCouponNo= couponNo.Replace("-", "");


            if (tempCouponNo.ToUpper().Equals("WOWNETTV15996676"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 112;
                result.Gubun = "2013";
                result.ExpireDate = DateTime.Parse("2013-12-31");
                result.Reason = "2013 대박타입 책구매";
                result.ResultMessage = "쿠폰이 발행되었습니다. \n\n발급받은 쿠폰은 파트너방송, 머니톡 플러스 결재시\n\n사용하시면 됩니다.";
                //return result;
            }

            if (tempCouponNo.ToUpper().Equals("PBYAHBYUGH000001"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 515;
                result.Gubun = "PLUS";
                result.ExpireDate = DateTime.Parse("2015-12-31");
                result.Reason = "카톡 플러스 친구";
                result.ResultMessage = "쿠폰이 발행되었습니다. \n\n발급받은 쿠폰은 파트너방송 결재시\n\n사용하시면 됩니다.";
                //return result;
            }
            if (tempCouponNo.ToUpper().Equals("WOWFA5352500HS40"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 468;
                result.Gubun = "HS40";
                result.ExpireDate = DateTime.Parse("2014-12-31");
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
               // return result;
            }
            if (tempCouponNo.ToUpper().Equals("WNETABI166760494"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 634;
                result.Gubun = "ABI1";
                result.ExpireDate = DateTime.Parse("2015-04-30");
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
               // return result;
            }

            if (tempCouponNo.ToUpper().Equals("WOW1123456780220"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 314;
                result.Gubun = "LII1";
                result.ExpireDate = DateTime.Parse("2015-07-31");
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
               // return result;
            }

            if (tempCouponNo.ToUpper().Equals("6676000066760100"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 857;
                result.Gubun = "JTD1";
                result.ExpireDate = DateTime.Parse("2016-12-31");
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
               // return result;
            }

            if (tempCouponNo.ToUpper().Equals("6676000066760100"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 857;
                result.Gubun = "JTD1";
                result.ExpireDate = DateTime.Parse("2016-12-31");
                result.Reason = "주식클리닉 전투단 7일 체험";
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
                //return result;
            }

            if (tempCouponNo.ToUpper().Equals("1599070066760100"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 823;
                result.Gubun = "LE03";
                result.ExpireDate = DateTime.Parse("2016-09-30");
                result.Reason = "하반기 증시대전망 참석자";
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
               // return result;
            }

            if (tempCouponNo.ToUpper().Equals("1599070000000100"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 833;
                result.Gubun = "KTM1";
                result.ExpireDate = DateTime.Parse("2016-12-31");
                result.Reason = "상산권태민 7일 체험";
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
                //return result;
            }

            if (tempCouponNo.ToUpper().Equals("GRCJPEEZCH969339"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 877;
                result.Gubun = "JTD1";
                result.ExpireDate = DateTime.Parse("2016-12-31");
                result.Reason = "홍은주 7일 체험";
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
              //  return result;
            }

            if (tempCouponNo.ToUpper().Equals("LIONKING15990700"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 823;
                result.Gubun = "LE04";
                result.ExpireDate = DateTime.Parse("2017-04-30");
                result.Reason = "라이온킹 강연회 참석자";
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
               // return result;
            }

            if (tempCouponNo.ToUpper().Equals("GRCJPEEZCH776676"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 652;
                result.Gubun = "JDW2";
                result.ExpireDate = DateTime.Parse("2017-12-31");
                result.Reason = "시청자 이벤트";
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
               // return result;
            }

            if (tempCouponNo.ToUpper().Equals("BAND159907006676"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 952;
                result.Gubun = "BAND";
                result.ExpireDate = DateTime.Parse("2017-12-31");
                result.Reason = "BAND";
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
               // return result;
            }

            if (tempCouponNo.ToUpper().Equals("LDGNPEEZCH996676"))
            {

                result.CouponGroup = 2;//                         '  빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 952;                          
                result.Gubun = "LDGN";
                result.ExpireDate = DateTime.Parse("2017-12-31"); //빌링 만료가 아니고 등록 시 미들단에서 유효기간으로 설정
                result.Reason = "TEST";
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
                // return result;
            }
            if (tempCouponNo.ToUpper().Equals("CHSWPEEZCH556676"))
            {
                result.CouponGroup = 2;//                         ' 빌링에서 발행하지 않는 대표번호 쿠폰은 2으로 설정
                result.CouponId = 952;
                result.Gubun = "CHSW";
                result.ExpireDate = DateTime.Parse("2017-12-31"); //빌링 만료가 아니고 등록 시 미들단에서 유효기간으로 설정
                result.Reason = "CHSW";
                result.ResultMessage = "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
                // return result;
            }

            WowLog.Write("[MyPageBiz.CouponChceck] CouponGroup : " + result.CouponGroup + " CouponNo : " + result.CouponNo + " ResultMessage : " + result.ResultMessage);
            return result;
        }

        public string RegisterCoupon(CouponResult couponResult, LoginUserInfo loginUserInfo, string boQv5BillHost)
        {

            try
            {
                var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));

                if (userInfo != null)
                {
                    couponResult.CouponNo = couponResult.CouponNo.Replace("-", "");

                    WowLog.Write("[MyPageBiz.RegisterCoupon] CouponGroup :" + couponResult.CouponGroup + " CouponNo :" + couponResult.CouponNo);
                    if (couponResult.CouponGroup.Equals(1))
                    {

                        if (string.IsNullOrWhiteSpace(couponResult.ResultMessage))
                        {


                            BOQv7BillLib.BillClass couponBill1 = new BOQv7BillLib.BillClass
                            {
                                TxCmd = "registercoupon",
                                HOST = boQv5BillHost,
                                //CodePage = 0
                            };
                            couponBill1.SetField("gamecode", "wowtv");
                            couponBill1.SetField("couponno", couponResult.CouponNo);
                            couponBill1.SetField("userno", userInfo.userNumber.ToString());
                            couponBill1.SetField("userid", userInfo.userId);
                            couponBill1.SetField("username", userInfo.name);
                            couponBill1.SetField("ipaddr", loginUserInfo.Ip);

                            int billReturnValue = couponBill1.StartAction();

                            if (billReturnValue != 0)
                            {
                                string errmsg = "";//couponBill1.ErrMsg;
                                switch (billReturnValue)
                                {
                                    case 5541:
                                        errmsg = "5회 이상 등록 실패로, 오늘은 더 이상 쿠폰 등록을 하실 수 없습니다.";
                                        break;
                                    case 5549:
                                        errmsg = "해당 쿠폰의 정보를 확인할 수 없습니다." + couponResult.CouponNo;
                                        break;
                                    case 5550:
                                        errmsg = "이미 사용된 쿠폰입니다.";
                                        break;
                                    case 5551:
                                        errmsg = "사용이 중지된 쿠폰입니다.";
                                        break;
                                    case 5552:
                                        errmsg = "사용 기간이 만료된 쿠폰입니다.";
                                        break;
                                    case 5554:
                                        errmsg = "특정 고객만 사용할 수 있도록 발행된 쿠폰으로, 사용 권한이 없습니다.";
                                        break;
                                    case 5555:
                                        errmsg = "등록 가능한 쿠폰 개수를 초과하였습니다.";
                                        break;
                                    case 5556:
                                        errmsg = "귀하는 결제서비스 사용이 제한되어 있습니다.";
                                        break;
                                    case 5557:
                                        errmsg = "귀하의 구매서비스 사용이 제한되어 있습니다.";
                                        break;
                                    default:
                                        errmsg = "";
                                        break;
                                }
                                

                                return "쿠폰 등록이 실패하였습니다.\n\n" + errmsg + "\n\n(문의 : 1599-0700)";
                            }

                            if (couponBill1.GetVal("coupontype").Equals("2"))
                            {
                                return "쿠폰서비스가 정상 등록되었습니다.\n\n감사합니다.";
                            }
                            if (couponBill1.GetVal("coupontype").Equals("5"))
                            {

                                return "하단의 보유쿠폰 목록에서 쿠폰사용 클릭후\n\n원하는 전문가를 선택하세요.";
                            }
                            return "쿠폰이 정상 등록되었습니다.\n\n쿠폰사용안내를 참고하시기 바랍니다.";
                        }
                    }
                    else
                    {

                        WowLog.Write("[MyPageBiz.RegisterCoupon] 유효기간 체크 :" + couponResult.CouponGroup + " CouponNo :" + couponResult.CouponNo);

                        TimeSpan gap = DateTime.Now - couponResult.ExpireDate;
                        if (gap.Ticks < 0) //유효기간이 있는 경우만...
                        {
                            string coupon = couponResult.CouponNo.Replace("-", "");
                            var item = db89_wowbill.tblcoupon_Daebak.SingleOrDefault(x =>
                                x.userid.Equals(loginUserInfo.UserId) && x.c_no.Equals(coupon) &&
                                x.Gubun.Equals(couponResult.Gubun));
                            bool isUsedCoupon = false;
                            int billReturnValue2 = 0;

                            if (item == null)
                            {
                                isUsedCoupon = true;
                            }

                            if (isUsedCoupon)
                            {
                                BOQv7BillLib.BillClass couponBill1 = new BOQv7BillLib.BillClass
                                {
                                    TxCmd = "issueonecoupon",
                                    HOST = boQv5BillHost,
                                    CodePage = 0
                                };
                                couponBill1.SetField("couponid", couponResult.CouponId.ToString());

                                string durationtype = ""; // 유효기간 유형 (4:일, 5:월)	// K100204
                                string duration = ""; //'유효기간 값(일수 or 월수)	// K100204
                                switch (couponResult.CouponId)
                                {
                                    case 515:
                                        durationtype = "4";
                                        duration = "30";
                                        break;
                                    case 634:
                                        durationtype = "4";
                                        duration = gap.Days.ToString();
                                        break;
                                    case 314:
                                        durationtype = "4";
                                        duration = "5";
                                        break;
                                    case 738:
                                        durationtype = "5";
                                        duration = "1";
                                        break;
                                    case 823:
                                        durationtype = "4";
                                        duration = gap.Days.ToString();
                                        break;
                                    case 833:
                                        durationtype = "5";
                                        duration = "1";
                                        break;
                                    case 877:
                                        durationtype = "5";
                                        duration = "1";
                                        break;
                                    case 652:
                                        durationtype = "5";
                                        duration = "1";
                                        break;
                                    case 952:
                                        durationtype = "5";
                                        duration = "3";
                                        break;
                                }

                                couponBill1.SetField("durationtype", durationtype);
                                couponBill1.SetField("duration", duration);
                                couponBill1.SetField("userid", userInfo.userId);
                                couponBill1.SetField("reason", couponResult.Reason);
                                couponBill1.SetField("adminid", "system");

                                int billReturnValue = couponBill1.StartAction();

                                if (billReturnValue != 0)
                                {
                                    WowLog.Write("쿠폰 등록이 실패하였습니다.\n\n" + couponBill1.ErrMsg + "\n\n(문의 : 1599-0700)");
                                    return "쿠폰 등록이 실패하였습니다.\n\n" + couponBill1.ErrMsg + "\n\n(문의 : 1599-0700)";
                                }

                                if (couponResult.CouponNo.Equals("WOWNETTV15996676"))
                                {
                                    couponBill1.SetField("couponid", "253");
                                    couponBill1.SetField("userid", userInfo.userId);
                                    couponBill1.SetField("reason", couponResult.Reason);
                                    couponBill1.SetField("adminid", "system");

                                    billReturnValue2 = couponBill1.StartAction();
                                    if (billReturnValue2 != 0)
                                    {
                                        WowLog.Write(
                                            "쿠폰 등록이 실패하였습니다.\n\n" + couponBill1.ErrMsg + "\n\n(문의 : 1599-0700)");
                                        return "쿠폰 등록이 실패하였습니다.\n\n" + couponBill1.ErrMsg + "\n\n(문의 : 1599-0700)";
                                    }
                                }

                                if (billReturnValue.Equals(0) && billReturnValue2.Equals(0))
                                {
                                    InsertCouponDaebak(couponResult, loginUserInfo);
                                }

                                if (couponBill1.GetVal("coupontype").Equals("2"))
                                {
                                    return "쿠폰서비스가 정상 등록되었습니다.\n\n감사합니다.";
                                }
                                if (couponBill1.GetVal("coupontype").Equals("5"))
                                {

                                    return "하단의 보유쿠폰 목록에서 쿠폰사용 클릭후\n\n원하는 전문가를 선택하세요.";
                                }

                            }
                            else
                            {
                                return "해당 쿠폰은 1인당 1회만 사용가능합니다.";
                            }
                        }
                        else
                        {

                            return "해당 쿠폰은 유효기간이 만료 되었습니다.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WowLog.Write("MyPageBiz.RegisterCoupon > 개인 쿠폰 등록실패: " + ex.Message);
                return "개인 쿠폰 등록실패: " + ex.Message;

            }
            return "";
        }

        private void InsertCouponDaebak(CouponResult couponResult, LoginUserInfo loginUserInfo)
        {
            using (var dbContextTransaction = db89_wowbill.Database.BeginTransaction())
            {
                try
                {
                    tblcoupon_Daebak coupon = new tblcoupon_Daebak();
                    coupon.Gubun = couponResult.Gubun;
                    coupon.userid = loginUserInfo.UserId;
                    coupon.c_no = couponResult.CouponNo;
                    coupon.c_regdate = DateTime.Now;
                    db89_wowbill.tblcoupon_Daebak.AddOrUpdate(coupon);
                    db89_wowbill.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 개인정보의 열람 및 이용 주체 가져오기
        /// </summary>
        /// <param name="priceid"></param>
        /// <returns></returns>
        public string GetCPNAME(decimal priceid)
        {
            int castPriceId = (int)priceid;


            var itemPriceList = db89_WOWTV_BILL_DB.TItemPriceMst.AsNoTracking().Where(x => x.ItemPriceID.Equals(castPriceId)).Select(x => x.ItemID).ToList();
            var itemMstList = db89_WOWTV_BILL_DB.TItemMst.AsNoTracking().Where(x =>itemPriceList.Contains(x.ItemID)).Select(x => x.CPID).ToList();
            var cpname = db89_WOWTV_BILL_DB.TCPMst.AsNoTracking().Where(x => itemMstList.Contains(x.CPID)).Select(x => x.CPName).SingleOrDefault();

            return cpname;
        }


        #endregion

        #region 나의 서비스,주문,배송 이용내역
        public ListModel<MyOrderDelivery> GetOrderList(LoginUserInfo loginUserInfo, MyOrderCondition condition)
        {
            ListModel<MyOrderDelivery> resultData = new ListModel<MyOrderDelivery>();
            List<MyOrderDelivery> resultList = new List<MyOrderDelivery>();
            var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));

            if (userInfo != null)
            {
                var giftList = db49_wownet.TBLCMSMNU_GIFT_USER.AsNoTracking().AsQueryable();


                giftList = giftList.Where(x => x.USER_ID.Equals(loginUserInfo.UserId));

                //기간 검색
                if (!string.IsNullOrWhiteSpace(condition.START_DATE) && !string.IsNullOrWhiteSpace(condition.END_DATE))
                {
                    giftList = giftList.Where(x =>
                        string.Compare(x.WIN_DATE, condition.START_DATE, StringComparison.Ordinal) >= 0 &&
                        string.Compare(x.WIN_DATE, condition.END_DATE, StringComparison.Ordinal) <= 0);
                }


                giftList.ToList().ForEach(x => resultList.Add(new MyOrderDelivery()
                {
                    DataType = "경품",
                    DelyCo = x.DELY_CO,
                    InvoiceNum = x.INVOICE_NUM,
                    Pkid = x.PKID.ToString(),
                    Pname = x.PNAME,
                    Price = 0,
                    Status = x.STATUS.Equals("0") ? "배송대기" : "발송완료",
                    Unitprice = 0,
                    Itemcnt = 0,
                    WinDate = x.WIN_DATE
                }));


                ObjectParameter output = new ObjectParameter("po_intRecordCnt", typeof(Int32));
                var etcList = db89_WOWTV_BILL_DB.UP_PORTAL_DELIVERY_LST(null, 2, userInfo.userNumber.ToString(), "", "",
                    "", "", 0, 0, 1, 999, output).ToList();


                etcList.ForEach(x => resultList.Add(new MyOrderDelivery
                {
                    DataType = "교재 및 VOD",
                    DelyCo = x.DELIVERYDESC,
                    InvoiceNum = "",
                    Pkid = x.CHARGENO.ToString(),
                    Pname = x.PRODNAME,
                    Unitprice = x.UNITPRICE.Value,
                    Price = x.PRICE.Value,
                    Status = x.USESTATE,
                    Itemcnt = x.ITEMCNT.Value,
                    WinDate = x.YMD
                }));
                resultData.TotalDataCount = (int)output.Value;
                //if (condition.PageSize > -1)
                //{
                //    if (condition.PageSize == 0)
                //    {
                //        condition.PageSize = 10;
                //    }
                //    resultList = resultList.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
                //}
                resultData.ListData = resultList;
            }
            return resultData;
        }

        #endregion

        #region 나의 가입카페

        public List<FN_JOIN_CAFE_LIST_New_Result> GetJoinCafeList(LoginUserInfo loginUserInfo)
        {
            List<FN_JOIN_CAFE_LIST_New_Result> list = new List<FN_JOIN_CAFE_LIST_New_Result>();
            list = db49_wownet.FN_JOIN_CAFE_LIST_New(loginUserInfo.UserId).ToList();



            foreach (var item in list)
            {
                if (item.CAFECODE.Equals(101) || item.CAFECODE.Equals(106) || item.CAFECODE.Equals(220))
                {
                    item.TopnewBoardSakalList = db49_wowcafe
                        .usp_Select_TopNewBoard_sakal(item.CAFECODE.Value, 3, int.Parse(item.GRADELEVEL)).ToList();
                }
                else
                {
                    item.TopNewBoardList = db49_wowcafe
                        .usp_Select_TopNewBoard(item.CAFECODE.Value, 3, int.Parse(item.GRADELEVEL)).ToList();
                }
            }


            return list;
        }

        #endregion

        #region 메인화면

        public Pro_wowList GetMyPartnerAt(LoginUserInfo loginUserInfo)
        {
            var partner = db22_stock.tblMyInvestmentPartners.Where(x => x.userId.Equals(loginUserInfo.UserId)).OrderByDescending(x =>x.regdate).Take(1)
                .SingleOrDefault();
            Pro_wowList proWow = null;
            if (partner != null)
            {
                proWow = db49_broadcast.Pro_wowList.SingleOrDefault(x =>
                    x.State.Equals("1") && !x.Pro_Type.Value.Equals(6) && x.NickName.Equals(partner.InvestPartner1));
            }

            return proWow;
        }

        public WOWSP_GET_BALANCE_Result GetMyCashAt(LoginUserInfo loginUserInfo)
        {
            var myCash = db89_WOWTV_BILL_DB.WOWSP_GET_BALANCE(loginUserInfo.UserNumber).SingleOrDefault();
            return myCash;
        }

        /// <summary>
        /// 메인 뉴스 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public ListModel<JOIN_TAB_SCRAP_CONTENT> GetMainNewsPin(LoginUserInfo loginUser)
        {

            ListModel<JOIN_TAB_SCRAP_CONTENT> resultData = new ListModel<JOIN_TAB_SCRAP_CONTENT>();

            var newsPinList = from scrapContent in db49_wownet.TAB_SCRAP_CONTENT.AsNoTracking()
                              join scrapMenu in db49_wownet.TAB_SCRAP_MENU.AsNoTracking() on scrapContent.BSEQ equals scrapMenu.MSEQ
                              where scrapContent.USER_ID == loginUser.UserId
                                    && scrapContent.USER_ID == loginUser.UserId
                                    && scrapContent.BSEQ != 0
                              select new JOIN_TAB_SCRAP_CONTENT
                              {
                                  BSEQ = scrapContent.BSEQ,
                                  TITLE = scrapContent.TITLE,
                                  CNAME = scrapContent.CNAME,
                                  HashTag = "",
                                  ArticleID = scrapContent.ArticleID,
                                  PRO_ID = scrapContent.PRO_ID,
                                  REGDATE = scrapContent.REGDATE,
                                  SCRAPDATE = scrapContent.SCRAPDATE,
                                  SEQ = scrapContent.SEQ,
                                  URL = scrapContent.URL,
                                  USER_ID = scrapContent.USER_ID,
                                  FOLDER_NAME = scrapMenu.FOLDER_NAME
                              };

            resultData.TotalDataCount = newsPinList.Count();
            newsPinList = newsPinList.OrderByDescending(a => a.SCRAPDATE).Take(3);
            resultData.ListData = newsPinList.ToList();

            //result: "삼성전자,반도체,수원"
            foreach (var item in resultData.ListData)
            {
                var hashTag = db49_Article.tblArticleList.SingleOrDefault(x => x.ArtID.Equals(item.ArticleID));

                if (!string.IsNullOrWhiteSpace(hashTag?.Tag))
                {
                    item.HashTag = hashTag.Tag;
                }
                else if (!string.IsNullOrWhiteSpace(hashTag?.Keywords))
                {
                    item.HashTag = hashTag.Keywords;
                }

            }

            return resultData;

        }

        /// <summary>
        /// 파트너 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public ListModel<NTB_MYPIN_PARTNER> GetMainPartnerPin(LoginUserInfo loginUser)
        {

            ListModel<NTB_MYPIN_PARTNER> resultData = new ListModel<NTB_MYPIN_PARTNER>();

            var partnerPinList = db49_wowtv.NTB_MYPIN_PARTNER.AsNoTracking().AsQueryable();
            partnerPinList = partnerPinList.Where(x => x.USER_ID.Equals(loginUser.UserId));


            resultData.TotalDataCount = partnerPinList.Count();
            partnerPinList = partnerPinList.OrderByDescending(a => a.SCRAPDATE).Take(3);
            resultData.ListData = partnerPinList.ToList();

            foreach (var ntbMypinPartner in resultData.ListData)
            {
                var partnerinfo = db49_broadcast.Pro_wowList.AsNoTracking().AsQueryable()
                    .SingleOrDefault(x => x.Pay_no.Equals(ntbMypinPartner.PAY_NO));

                ntbMypinPartner.NEWphoto_small = partnerinfo?.NEWphoto_small;
                ntbMypinPartner.FullName = partnerinfo?.NickName; //필명으로 변경
                ntbMypinPartner.PRO_ID = partnerinfo?.Pro_id;

                var broadCast = db49_broadcast.USP_GetBroadcast1ByProId(partnerinfo?.Pro_id).SingleOrDefault();

                if (string.IsNullOrWhiteSpace(broadCast?.BRSTARTTIME) ||
                    string.IsNullOrWhiteSpace(broadCast?.BRENDTIME))
                {
                    ntbMypinPartner.BroadCastTime = "금일 예정된 방송이 없습니다.";
                }
                else
                {
                    string startTime = $"{broadCast.BRSTARTTIME.Substring(0, 4)}-{broadCast.BRSTARTTIME.Substring(4, 2)}-{broadCast.BRSTARTTIME.Substring(6, 2)} {broadCast.BRSTARTTIME.Substring(8, 2)}:{broadCast.BRSTARTTIME.Substring(10, 2)}";
                    string endTime = $"{broadCast.BRENDTIME.Substring(0, 4)}-{broadCast.BRENDTIME.Substring(4, 2)}-{broadCast.BRENDTIME.Substring(6, 2)} {broadCast.BRENDTIME.Substring(8, 2)}:{broadCast.BRENDTIME.Substring(10, 2)}";

                    ntbMypinPartner.BroadCastTime = startTime + "~" + endTime;
                }

                var cafeMasterInfo = db49_wownet.usp_Select_MasterOpenCafeInfo(partnerinfo?.Wowtv_id).SingleOrDefault();

                if (cafeMasterInfo != null)
                {
                    var cafeGradeLevel = db49_wowcafe.CafeMemberInfo.SingleOrDefault(x =>
                    x.UserID.Equals(loginUser.UserId) && x.CafeCode.Equals(cafeMasterInfo.CafeCode));

                    if (cafeGradeLevel == null)
                    {
                        var boardInfo = db49_wowcafe.usp_Select_TopNewBoard(cafeMasterInfo?.CafeCode, 1, 0)
                            .SingleOrDefault();
                        ntbMypinPartner.BoardTitle = boardInfo?.Title;
                        ntbMypinPartner.CafeDomain = cafeMasterInfo?.CafeDomain;
                        ntbMypinPartner.BoardType = boardInfo?.BoardType;
                        ntbMypinPartner.CafeCode = cafeMasterInfo?.CafeCode.ToString();
                        ntbMypinPartner.Num = boardInfo?.Num.ToString();
                        ntbMypinPartner.BoardCode = boardInfo?.boardcode;
                    }
                    else
                    {
                        var boardInfo = db49_wowcafe
                            .usp_Select_TopNewBoard(cafeMasterInfo?.CafeCode, 1, cafeGradeLevel.GradeLevel)
                            .SingleOrDefault();
                        ntbMypinPartner.BoardTitle = boardInfo?.Title;
                        ntbMypinPartner.CafeDomain = cafeMasterInfo?.CafeDomain;
                        ntbMypinPartner.BoardType = boardInfo?.BoardType;
                        ntbMypinPartner.CafeCode = cafeMasterInfo?.CafeCode.ToString();
                        ntbMypinPartner.Num = boardInfo?.Num.ToString();
                        ntbMypinPartner.BoardCode = boardInfo?.boardcode;
                    }
                }
                

            }

            return resultData;
        }

        /// <summary>
        /// 프로그램 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public ListModel<NTB_MYPIN_PROGRAM> GetMainProgramPin(LoginUserInfo loginUser)
        {

            ListModel<NTB_MYPIN_PROGRAM> resultData = new ListModel<NTB_MYPIN_PROGRAM>();

            var programPinList = db49_wowtv.NTB_MYPIN_PROGRAM.AsNoTracking().AsQueryable()
                .Where(x => x.USER_ID.Equals(loginUser.UserId)).ToList();

            foreach (var item in programPinList)
            {
                var programInfo = db90_DNRS.T_NEWS_PRG.AsNoTracking().AsQueryable()
                    .SingleOrDefault(x => x.PRG_CD.Equals(item.PRG_CD) && x.DELFLAG.Equals("0"));
                var tvProgramInfo = db90_DNRS.tv_program.AsNoTracking().AsQueryable()
                    .OrderByDescending(x => x.UploadTime).FirstOrDefault(x => x.Dep.Equals(item.PRG_CD));

                if (programInfo != null)
                {

                    item.SUB_IMG = new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-SUB", programInfo.PRG_CD)
                        ?.REAL_WEB_PATH;
                    item.REC_IMG = new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-RECTANGLE", programInfo.PRG_CD)
                        ?.REAL_WEB_PATH;
                    item.BRO_START = programInfo.BRO_START;
                    item.BRO_END = programInfo.BRO_END;
                    item.PRG_NM = programInfo.PRG_NM;
                    item.PGMDAY = ConvertBroadDay(programInfo.PGMDAY);
                }

                if (tvProgramInfo != null) item.TV_REPLAY = tvProgramInfo.Program_Name;
            }


            resultData.TotalDataCount = programPinList.Count;
            programPinList = programPinList.OrderByDescending(a => a.SCRAPDATE).Take(3).ToList();

            resultData.ListData = programPinList;

            return resultData;
        }

        /// <summary>
        /// 기자 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public ListModel<NUP_MYPIN_REPORTER_SELECT_Result> GetMainReporterPin(LoginUserInfo loginUser)
        {

            ListModel<NUP_MYPIN_REPORTER_SELECT_Result> resultData = new ListModel<NUP_MYPIN_REPORTER_SELECT_Result>();
            List<NUP_MYPIN_REPORTER_SELECT_Result> mypinReporterList =
                db49_Article.NUP_MYPIN_REPORTER_SELECT(null, loginUser.UserId).ToList();

            resultData.TotalDataCount = mypinReporterList.Count;
            mypinReporterList = mypinReporterList.OrderByDescending(a => a.SCRAPDATE).Take(3).ToList();
            resultData.ListData = mypinReporterList;

            return resultData;
        }

        #endregion

        #region 나의 투자파트너

        public List<usp_getInvestmentPartners_Result> GetInvestmentPartnerList(LoginUserInfo loginUserInfo)
        {
            var partner = db22_stock.tblMyInvestmentPartners.Where(x => x.userId.Equals(loginUserInfo.UserId))
                .OrderByDescending(x => x.regdate).Take(1).SingleOrDefault();

            if (partner == null)
                return null;

            var list = db49_broadcast.usp_getInvestmentPartners(partner.InvestPartner1, partner.InvestPartner2,
                partner.InvestPartner3, partner.InvestPartner4, partner.InvestPartner5
                , partner.InvestPartner6, partner.InvestPartner7, partner.InvestPartner8, partner.InvestPartner9,
                partner.InvestPartner10, partner.InvestPartner11, partner.InvestPartner12).ToList();


            foreach (var item in list)
            {
                var broadCast = db49_broadcast.USP_GetBroadcast1ByProId(item.PRO_ID).SingleOrDefault();

                if (string.IsNullOrWhiteSpace(broadCast?.BRSTARTTIME) ||
                    string.IsNullOrWhiteSpace(broadCast?.BRENDTIME))
                {
                    item.BroadTime = "금일 예정된 방송이 없습니다.";
                }
                else
                {
                    string startTime = $"{broadCast.BRSTARTTIME.Substring(0, 4)}-{broadCast.BRSTARTTIME.Substring(4, 2)}-{broadCast.BRSTARTTIME.Substring(6, 2)} {broadCast.BRSTARTTIME.Substring(8, 2)}:{broadCast.BRSTARTTIME.Substring(10, 2)}";
                    string endTime = $"{broadCast.BRENDTIME.Substring(0, 4)}-{broadCast.BRENDTIME.Substring(4, 2)}-{broadCast.BRENDTIME.Substring(6, 2)} {broadCast.BRENDTIME.Substring(8, 2)}:{broadCast.BRENDTIME.Substring(10, 2)}";
                    item.BroadTime = startTime + "~" + endTime;// broadCast.BRSTARTTIME + "~" + broadCast.BRENDTIME;
                }

                var cafeHomeInfo = db49_broadcast.Pro_wowList.AsNoTracking()
                    .SingleOrDefault(x => x.Pro_id.Equals(item.PRO_ID) && x.State.Equals("1"));

                if (cafeHomeInfo != null)
                {
                    //USP_SELECT_MASTEROPENCAFEINFO
                    var cafeMasterInfo = db49_wownet.usp_Select_MasterOpenCafeInfo(cafeHomeInfo.Wowtv_id)
                        .SingleOrDefault();

                    item.CafeName = cafeMasterInfo?.CafeDomain;
                }
            }

            return list;
        }

        #endregion

        #region 강연회 참여현황

        public ListModel<NUP_MY_LECTURES_SELECT_Result> GetLecturesList(MyLecturesCondition condition)
        {
            ListModel<NUP_MY_LECTURES_SELECT_Result> resultData = new ListModel<NUP_MY_LECTURES_SELECT_Result>();
            var list = db49_wownet.NUP_MY_LECTURES_SELECT(condition.loginId, condition.START_DATE, condition.END_DATE)
                .ToList();

            resultData.TotalDataCount = list.Count;
            List<NUP_MY_LECTURES_SELECT_Result> pageList = null;
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                pageList = list.OrderByDescending(a => a.REGDATE).Skip(condition.CurrentIndex).Take(condition.PageSize)
                    .ToList();
            }
            resultData.ListData = pageList;
            return resultData;
        }

        #endregion

        #region 나의 관심종목 보유종목

        public void SaveMyFavoriteJongMok(LoginUserInfo loginUserInfo, string stockcode)
        {
            using (var dbContextTransaction = db22_stock.Database.BeginTransaction())
            {
                try
                {

                    if (string.IsNullOrWhiteSpace(stockcode))
                    {
                        var exceptionMessage = "종목 코드 정보를 확인하세요.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    var checkItem = db22_stock.tblMyFavoriteJongMok.SingleOrDefault(x =>
                        x.usernumber.Value.Equals(loginUserInfo.UserNumber) && x.stockcode.Equals(stockcode));

                    if (checkItem != null)
                    {
                        var exceptionMessage = "이미 등록 되었습니다.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }
                    tblMyFavoriteJongMok jongMok = new tblMyFavoriteJongMok();

                    int? orderNo = db22_stock.tblMyFavoriteJongMok
                        .Where(x => x.usernumber.Value.Equals(loginUserInfo.UserNumber)).Max(x => x.orderNo);

                    if (orderNo == null)
                    {
                        orderNo = 0;
                    }
                    jongMok.orderNo = orderNo + 1;
                    jongMok.usernumber = loginUserInfo.UserNumber;
                    jongMok.stockcode = stockcode;
                    db22_stock.tblMyFavoriteJongMok.AddOrUpdate(jongMok);
                    db22_stock.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 종목코드 등록 여부 확인 (시세에서 사용)
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="stockcode"></param>
        /// <returns></returns>
        public bool IsMyFavoriteJongMok(LoginUserInfo loginUserInfo, string stockcode)
        {
            var checkItem = db22_stock.tblMyFavoriteJongMok.SingleOrDefault(x =>
                x.usernumber.Value.Equals(loginUserInfo.UserNumber) && x.stockcode.Equals(stockcode));

            return checkItem != null;
        }

        public void SaveMyFavoriteJongMokRange(LoginUserInfo loginUserInfo, IList<string> items)
        {
            using (var dbContextTransaction = db22_stock.Database.BeginTransaction())
            {
                try
                {
                    var beforeList = db22_stock.tblMyFavoriteJongMok
                        .Where(x => x.usernumber.Value.Equals(loginUserInfo.UserNumber)).ToList();

                    foreach (var item in items)
                    {

                        var checkItem = db22_stock.tblMyFavoriteJongMok.SingleOrDefault(x =>
                            x.usernumber.Value.Equals(loginUserInfo.UserNumber) && x.stockcode.Equals(item));

                        beforeList.Remove(checkItem);

                        if (checkItem == null) //없으면 저장
                        {
                            tblMyFavoriteJongMok jongMok = new tblMyFavoriteJongMok();

                            int? orderNo = db22_stock.tblMyFavoriteJongMok
                                .Where(x => x.usernumber.Value.Equals(loginUserInfo.UserNumber)).Max(x => x.orderNo);

                            jongMok.orderNo = orderNo + 1;
                            jongMok.usernumber = loginUserInfo.UserNumber;
                            jongMok.stockcode = item;
                            db22_stock.tblMyFavoriteJongMok.AddOrUpdate(jongMok);
                        }

                    }


                    foreach (var removeItem in beforeList)
                    {
                        db22_stock.tblMyFavoriteJongMok.Remove(removeItem);
                    }


                    db22_stock.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void RemoveMyFavoriteJongMok(LoginUserInfo loginUserInfo, string stockcode)
        {
            using (var dbContextTransaction = db22_stock.Database.BeginTransaction())
            {
                try
                {

                    if (string.IsNullOrWhiteSpace(stockcode))
                    {
                        var exceptionMessage = "종목 코드 정보를 확인하세요.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    var checkItem = db22_stock.tblMyFavoriteJongMok.SingleOrDefault(x =>
                        x.usernumber.Value.Equals(loginUserInfo.UserNumber) && x.stockcode.Equals(stockcode));

                    if (checkItem == null)
                    {
                        var exceptionMessage = "이미 삭제 되었습니다.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    db22_stock.tblMyFavoriteJongMok.Remove(checkItem);
                    db22_stock.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }


        public ListModel<tblMyFavoriteJongMok> GetMyFavoriteJongMokList(LoginUserInfo loginUserInfo,
            MyJongMokCondition condition)
        {
            ListModel<tblMyFavoriteJongMok> resultData = new ListModel<tblMyFavoriteJongMok>();

            //var list = db22_stock.tblMyFavoriteJongMok.AsNoTracking().AsQueryable();

            
            var list = from myfavoriteJongMok in db22_stock.tblMyFavoriteJongMok.AsNoTracking()
                join stockBatch in db22_stock.tblStockBatch.AsNoTracking() on ("A" + myfavoriteJongMok.stockcode) equals
                    stockBatch.ShortCode.Substring(0,7)
                join onlineSise in db22_stock.tblOnlineSise.AsNoTracking() on stockBatch.StockCode equals onlineSise
                    .StockCode
                where myfavoriteJongMok.usernumber == loginUserInfo.UserNumber && myfavoriteJongMok.stockcode !=null
                              select myfavoriteJongMok;

            resultData.TotalDataCount = list.Count();
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.OrderByDescending(a => a.orderNo).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();


            foreach (var item in resultData.ListData)
            {
                if (db22_stock.tblStockBatch.Count(x => x.korName.Equals(item.stockcode) || x.ShortCode.Equals("A" + item.stockcode)) > 0)
                {
                    var stockPriceResult = db22_stock.usp_GetStockPrice(item.stockcode).FirstOrDefault();

                    if (stockPriceResult != null)
                    {
                        item.chg_type = stockPriceResult.chg_type;
                        item.curr_price = stockPriceResult.curr_price;
                        item.net_chg = stockPriceResult.net_chg;
                        item.high_price = stockPriceResult.high_price;
                        item.highest_price = stockPriceResult.highest_price;
                        item.net_vol = stockPriceResult.net_vol;
                        item.init_price = stockPriceResult.init_price;
                        item.low_price = stockPriceResult.low_price;
                        item.lowest_price = stockPriceResult.lowest_price;
                        item.net_turnover = stockPriceResult.net_turnover;
                        item.data_day = stockPriceResult.data_day;
                        item.mkt_halt = stockPriceResult.mkt_halt;
                        item.stock_wanname = stockPriceResult.stock_wanname;
                        item.Groupid = stockPriceResult.Groupid;
                        item.stock_code = stockPriceResult.stock_code;
                    }

                    var todayBuyResult = db22_stock
                        .usp_web_getStockInvestorToday(item.stock_code, DateTime.Now.ToString("yyyyMMdd"))
                        .SingleOrDefault();

                    if (todayBuyResult != null)
                    {
                        item.foreinerbuy = todayBuyResult.foreinerbuy;
                        item.orgbuy = todayBuyResult.orgbuy;
                    }
                }
            }
            return resultData;

        }

        public double GetMyChangePercent(LoginUserInfo loginUserInfo)
        {
            ListModel<tblMyFavoriteJongMok> resultData = new ListModel<tblMyFavoriteJongMok>();

            var list = db22_stock.tblMyFavoriteJongMok.AsNoTracking().AsQueryable();


            list = list.Where(x => x.usernumber.Value.Equals(loginUserInfo.UserNumber));
            list = list.Where(x => x.stockcode != null);

            //CONVERT(NUMERIC(10, 2),
            //    CASE WHEN C.F_CHG_TYPE = 1 OR C.F_CHG_TYPE = 2 THEN
            //    (CONVERT(FLOAT, C.F_CURR_PRICE) / (convert(float, C.f_curr_price) - convert(float, C.f_net_chg))) * 100 - 100

            //WHEN C.f_chg_type = 4 or C.f_chg_type = 5 THEN

            //100 - (convert(float, C.f_curr_price) / (convert(float, C.f_curr_price) + convert(float, C.f_net_chg))) * 100

            //ELSE 0

            //END)


            resultData.TotalDataCount = list.Count();
            resultData.ListData = list.ToList();

            double allPersent = 0;
            foreach (var item in resultData.ListData)
            {
                if (db22_stock.tblStockBatch.Count(x =>
                        x.korName.Equals(item.stockcode) || x.ShortCode.Equals("A" + item.stockcode)) > 0)
                {
                    var stockPriceResult = db22_stock.usp_GetStockPrice(item.stockcode).FirstOrDefault();
                    double stockPersent = 0;
                    if (stockPriceResult != null)
                    {

                        if (stockPriceResult.chg_type.Equals("1") || stockPriceResult.chg_type.Equals("2"))
                        {
                            if (stockPriceResult.curr_price != null && stockPriceResult.net_chg != null)
                                stockPersent =
                                    Math.Round(
                                        Convert.ToDouble(stockPriceResult.curr_price.Value) /
                                        (stockPriceResult.curr_price.Value - stockPriceResult.net_chg.Value) * 100 -
                                        100,
                                        2);
                        }
                        else if (stockPriceResult.chg_type.Equals("4") || stockPriceResult.chg_type.Equals("5"))
                        {
                            if (stockPriceResult.curr_price != null && stockPriceResult.net_chg != null)
                                stockPersent =
                                    100 - Math.Round(
                                        Convert.ToDouble(stockPriceResult.curr_price.Value) /
                                        (stockPriceResult.curr_price.Value - stockPriceResult.net_chg.Value) * 100, 2);

                        }
                        else
                        {
                            stockPersent = 0;
                        }
                        allPersent = allPersent + stockPersent;


                    }
                }
            }

            if (resultData.ListData.Count > 0)
            {

                return Math.Round(allPersent / resultData.ListData.Count, 2);
            }

            return allPersent;

        }


        public List<tblStockBatch> GetStockList(LoginUserInfo loginUserInfo, string searchText, string searchValue)
        {
            var list = db22_stock.tblStockBatch.AsQueryable();


            list.Where(x => x.korName.Substring(0, 1).Equals("A"));

            //종목 검색
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                list = list.Where(x => x.korName.Contains(searchText));
            }


            var myFavoriteJongMoklist = db22_stock.tblMyFavoriteJongMok.AsNoTracking().AsQueryable()
                .Where(x => x.usernumber.Value.Equals(loginUserInfo.UserNumber)).ToList();
            var resultData = list.OrderBy(x => x.korName).ToList()
                .Where(x => myFavoriteJongMoklist.All(a => a.stockcode != x.ShortCode.Substring(1, 6))).ToList();



            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                resultData = WhereChosungList(resultData, searchValue);
            }

            foreach (var item in resultData)
            {
                item.ShortCode = item.ShortCode.Substring(1, 6);
            }



            return resultData;
        }

        private List<tblStockBatch> WhereChosungList(List<tblStockBatch> list, string word)
        {
            char[] chr =
            {
                'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ',
                'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ'
            };

            string[] str =
            {
                "가", "까", "나", "다", "따", "라", "마", "바", "빠", "사", "싸",
                "아", "자", "짜", "차", "카", "타", "파", "하"
            };

            int[] chrint =
            {
                44032, 44620, 45208, 45796, 46384, 46972, 47560, 48148, 48736, 49324, 49912,
                50500, 51088, 51676, 52264, 52852, 53440, 54028, 54616, 55204
            };

            string pattern = "";

            if (word.Equals("A-Z"))
            {
                pattern = "[A-Z]";
            }
            else
            {

                for (int i = 0; i < word.Length; i++)
                {
                    if (word[i] >= 'ㄱ' && word[i] < '가')
                    {
                        for (int j = 0; j < chr.Length; j++)
                        {
                            if (word[i] == chr[j])
                            {
                                pattern += "[" + str[j] + "-" + (char)(chrint[j + 1] - 1) + "]";
                            }
                        }
                    }
                    else
                    {
                        pattern += word[i];
                    }
                }
            }
            return list.Where(e => Regex.IsMatch(e.korName, pattern)).ToList();
        }


        public List<tblMyFavoriteJongMok> GetPopupMyFavoriteJongMokList(LoginUserInfo loginUserInfo)
        {
            //var list = db22_stock.tblMyFavoriteJongMok.AsNoTracking().AsQueryable();

            //list = list.Where(x => x.usernumber.Value.Equals(loginUserInfo.UserNumber));
            //list = list.Where(x => x.stockcode != null);


            var list = from myfavoriteJongMok in db22_stock.tblMyFavoriteJongMok.AsNoTracking()
                join stockBatch in db22_stock.tblStockBatch.AsNoTracking() on ("A" + myfavoriteJongMok.stockcode) equals
                    stockBatch.ShortCode.Substring(0, 7)
                join onlineSise in db22_stock.tblOnlineSise.AsNoTracking() on stockBatch.StockCode equals onlineSise
                    .StockCode
                where myfavoriteJongMok.usernumber == loginUserInfo.UserNumber && myfavoriteJongMok.stockcode != null
                select myfavoriteJongMok;

            var resultData = list.ToList();

            foreach (var item in resultData)
            {
                try
                {
                    var stockPriceResult = db22_stock.usp_GetStockPrice(item.stockcode).FirstOrDefault();

                    if (stockPriceResult != null)
                    {
                        item.stock_wanname = stockPriceResult.stock_wanname;
                        item.Groupid = stockPriceResult.Groupid;
                        item.stock_code = stockPriceResult.stock_code;
                    }
                }
                catch (Exception e)
                {
                    item.stock_wanname = "거래가 정지 된 종목입니다.";
                    item.Groupid = "";
                    item.stock_code = item.stock_code;
                }

            }
            return resultData;

        }


        public List<TAB_MY_STOCK> GetMyStock(LoginUserInfo loginUserInfo)
        {
            var list = db22_stock.TAB_MY_STOCK.AsNoTracking().AsQueryable();


            list = list.Where(x => x.USERNUMBER.Value.Equals(loginUserInfo.UserNumber))
                .OrderByDescending(x => x.REG_DATE);
            var resultData = list.ToList();

            foreach (var item in resultData)
            {
                string stockCode = "A" + item.STOCKCODE.Trim();
                var stockBatchResult = db22_stock.tblStockBatch.AsNoTracking().AsQueryable()
                    .FirstOrDefault(x => x.ShortCode.Equals(stockCode));

                if (stockBatchResult != null)
                {
                    item.k_stock_wanname = stockBatchResult.korName;
                    item.k_stock_code = stockBatchResult.StockCode;

                    var onlineSiseResult = db22_stock.tblOnlineSise.AsNoTracking().AsQueryable()
                        .FirstOrDefault(x => x.StockCode.Equals(stockBatchResult.StockCode));

                    if (onlineSiseResult != null)
                    {
                        item.now_price = onlineSiseResult.TradePrice is null ? 0 : onlineSiseResult.TradePrice.Value;
                    }
                }

            }
            return resultData;
        }


        public void RemoveMyStock(LoginUserInfo loginUserInfo, int seq)
        {
            using (var dbContextTransaction = db22_stock.Database.BeginTransaction())
            {
                try
                {

                    //if (string.IsNullOrWhiteSpace(seq))
                    //{
                    //    var exceptionMessage = "종목 코드 정보를 확인하세요.";
                    //    throw new DbEntityValidationException(exceptionMessage);
                    //}

                    var checkItem = db22_stock.TAB_MY_STOCK.SingleOrDefault(x =>
                        x.USERNUMBER.Value.Equals(loginUserInfo.UserNumber) && x.SEQ.Equals(seq));

                    if (checkItem == null)
                    {
                        var exceptionMessage = "이미 삭제 되었습니다.";
                        throw new DbEntityValidationException(exceptionMessage);
                    }

                    db22_stock.TAB_MY_STOCK.Remove(checkItem);
                    db22_stock.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void SaveMyStock(LoginUserInfo loginUserInfo, TAB_MY_STOCK tabMyStock)
        {
            using (var dbContextTransaction = db22_stock.Database.BeginTransaction())
            {
                try
                {

                    var singleMyStock = db22_stock.TAB_MY_STOCK.SingleOrDefault(x =>
                        x.USERNUMBER.Value.Equals(loginUserInfo.UserNumber) && x.SEQ.Equals(tabMyStock.SEQ));

                    if (singleMyStock != null)
                    {
                        singleMyStock.QTY = tabMyStock.QTY;
                        singleMyStock.BUY_PRICE = tabMyStock.BUY_PRICE;
                        db22_stock.TAB_MY_STOCK.AddOrUpdate(singleMyStock);
                    }
                    else
                    {
                        tabMyStock.STOCKCODE = tabMyStock.STOCKCODE.Trim();
                        tabMyStock.USERNUMBER = loginUserInfo.UserNumber;
                        tabMyStock.REG_DATE = DateTime.Now;
                        db22_stock.TAB_MY_STOCK.AddOrUpdate(tabMyStock);
                    }
                    db22_stock.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e )
                {
                    dbContextTransaction.Rollback();
                    WowLog.Write(e.Message);
                    WowLog.Write(e.StackTrace);
                    throw;
                }
            }
        }

        #endregion

        #region 서비스 항목

        public List<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMainServiceList(LoginUserInfo loginUserInfo)
        {
            ObjectParameter output = new ObjectParameter("po_intRecordCnt", typeof(Int32));
            var list = db89_WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST(loginUserInfo.UserNumber, null, null, null, 0,
                3, 1, output);

            return list.ToList();
        }


        public ListModel<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMyServiceList(LoginUserInfo loginUserInfo,
            MyServiceCondition condition)
        {
            ListModel<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> resultData =
                new ListModel<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result>();

            ObjectParameter output = new ObjectParameter("po_intRecordCnt", typeof(Int32));
            resultData.ListData = db89_WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST(loginUserInfo.UserNumber,
                    (String.IsNullOrEmpty(condition.START_DATE) == true ? "" : condition.START_DATE.Replace("-","")), (String.IsNullOrEmpty(condition.END_DATE) == true ? "" : condition.END_DATE.Replace("-", "")),
                    condition.SearchText, condition.ServiceStatus, condition.PageSize, condition.CurrentIndex, output).ToList();

            // string executeSql =
            //     "EXEC UP_PORTAL_PRODUCT_NEW_LST2 \r\n" +
            //     "{0}," +
            //     "{1},{2},{3},{4}," +
            //     "{5},{6},{7} output";

            //resultData.ListData = db89_WOWTV_BILL_DB.Database.SqlQuery<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result>(executeSql, loginUserInfo.UserNumber,
            //    condition.START_DATE.ToString("yyyy-MM-dd"), condition.END_DATE.ToString("yyyy-MM-dd"),
            //    condition.SearchText, condition.ServiceStatus, condition.PageSize, condition.CurrentIndex, output).ToList();

            foreach (var item in resultData.ListData)
            {
                var cancelCount = GetCancelCount(item.CHARGENO);
                if (cancelCount > 0)
                {
                    item.SERVICESTATUS = 40;//해지 접수중 표시
                    item.CHARGENO = GetCancelSeqNo(item.CHARGENO); // 취소 접수건은 chargeNo에 SeqNo를 넣어준다.
                }

                if (item.STOPCNT > 0 && !item.SERVICESTATUS.ToString().Equals("4"))
                {
                    item.SERVICESTATUS = 50;//중지중
                }

                if (item.STOPCNT > 0 && item.SERVICESTATUS.ToString().Equals("4"))
                {
                    item.SERVICESTATUS = 60;// 서비스 해지
                }
            }


            resultData.TotalDataCount = (int)output.Value;

            return resultData;
        }

        private int GetCancelCount(long chargeno)
        {
            //' 해지 접수중 표시
            //    strSql = ""
            //strSql = strSql & "SELECT count(*) FROM TCHARGECANCELREQUESTMST WITH (NOLOCK) "
            //strSql = strSql & "WHERE USESTATE <> 4 AND REGDATE > GETDATE() - 3 AND ChargeNo = '" & onServiceBox.getMultiString("chargeno", i) & "' "
            //cancelCount = execSqlReturnVar("wowtvbilldb", strSql)

            //If cancelCount > 0 Then
            //    status = "<font color='red'>해지 접수중</font>"
            //viewCancelButton = false
            //End If
            var date = DateTime.Now.AddDays(-3);
            var cancelCount = db89_WOWTV_BILL_DB.TChargeCancelRequestMst.AsNoTracking().Count(x => !x.UseState.Equals(4) && x.RegDate > date && x.ChargeNo.Equals(chargeno));

            return cancelCount;
        }

        // 해지신청-접수취소를 위한 취소신청접수번호
        private int GetCancelSeqNo(long chargeno)
        {
            var date = DateTime.Now.AddDays(-3);
            var cancelSeqNo = db89_WOWTV_BILL_DB.TChargeCancelRequestMst.AsNoTracking().Where(x => !x.UseState.Equals(4) && x.RegDate > date && x.ChargeNo.Equals(chargeno)).Select(x => x.SeqNo).SingleOrDefault();

            return cancelSeqNo;
        }

        public string CancelRollback(int seqNo, LoginUserInfo loginUserInfo, string boQv5BillHost)
        {

            try
            {
                var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));
                if (userInfo != null)
                {
                    WowLog.Write("[MyPageBiz.CancelRollback] seqNo :" + seqNo.ToString() + " userid :" + userInfo.userId);

                    if ( (string.IsNullOrWhiteSpace(seqNo.ToString())) == false)
                    {


                        BOQv7BillLib.BillClass serviceBill1 = new BOQv7BillLib.BillClass
                        {
                            TxCmd = "updrequestcancel",
                            HOST = boQv5BillHost,
                            //CodePage = 0
                        };
                        serviceBill1.SetField("seqno", seqNo.ToString());
                        serviceBill1.SetField("callupdmsg", "Mypage-UserCancel");
                        serviceBill1.SetField("usestate", "4");
                        serviceBill1.SetField("adminid", userInfo.userId);

                        int billReturnValue = serviceBill1.StartAction();

                        //couponBill1.ErrMsg; 0 성공 그외실패
                        if (billReturnValue != 0)
                        {
                            return "해지 접수신청이 실패하였습니다.\n\n(CODE:" + billReturnValue.ToString() + ")\n\n(문의 : 1599-0700)";
                        }
                        return "해지접수 취소신청이 정상적으로 처리되었습니다.";
                    }
                }
            }

            catch (Exception ex)
            {
                WowLog.Write("MyPageBiz.CancelRollback > 해지접수 취소신청 실패: " + ex.Message);
                return "해지접수 취소신청 실패: " + ex.Message;

            }
            return "";
        }
        #endregion

        #region 주문 밎 배송내역

        public ListModel<UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result> GetVodDelivery(LoginUserInfo loginUserInfo,
            MyOrderCondition condition)
        {
            ListModel<UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result> resultData =
                new ListModel<UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result>();
            ObjectParameter output = new ObjectParameter("po_intRecordCnt", typeof(Int32));
            var list = db89_WOWTV_BILL_DB.UP_PORTAL_MYPAGE_DELIVERY_UR_LST(loginUserInfo.UserNumber,
                condition.START_DATE, condition.END_DATE, condition.PageSize, condition.CurrentIndex, output).ToList();
            resultData.ListData = list;
            resultData.TotalDataCount = (int)output.Value;

            return resultData;
        }

        public ListModel<MyOrderDelivery> GetGiftDelivery(LoginUserInfo loginUserInfo, MyOrderCondition condition)
        {
            ListModel<MyOrderDelivery> resultData = new ListModel<MyOrderDelivery>();
            List<MyOrderDelivery> resultList = new List<MyOrderDelivery>();
            var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));

            if (userInfo != null)
            {
                var giftList = db49_wownet.TBLCMSMNU_GIFT_USER.AsNoTracking().AsQueryable();


                giftList = giftList.Where(x => x.USER_ID.Equals(loginUserInfo.UserId));

                //기간 검색
                if (!string.IsNullOrWhiteSpace(condition.START_DATE) && !string.IsNullOrWhiteSpace(condition.END_DATE))
                {
                    giftList = giftList.Where(x =>
                        string.Compare(x.WIN_DATE, condition.START_DATE, StringComparison.Ordinal) >= 0 &&
                        string.Compare(x.WIN_DATE, condition.END_DATE, StringComparison.Ordinal) <= 0);
                }


                giftList.ToList().ForEach(x => resultList.Add(new MyOrderDelivery()
                {
                    DataType = "경품",
                    DelyCo = x.DELY_CO,
                    InvoiceNum = x.INVOICE_NUM,
                    Pkid = x.PKID.ToString(),
                    Pname = x.PNAME,
                    Price = 0,
                    Status = x.STATUS.Equals("0") ? "배송대기" : "발송완료",
                    Unitprice = 0,
                    Itemcnt = 0,
                    WinDate = x.WIN_DATE
                }));


                //ObjectParameter output = new ObjectParameter("po_intRecordCnt", typeof(Int32));
                //var etcList = db89_WOWTV_BILL_DB.UP_PORTAL_DELIVERY_LST(null, 2, userInfo.userNumber.ToString(), "", "", "", "", 0, 0, 1, 999, output).ToList();


                //etcList.ForEach(x => resultList.Add(new MyOrderDelivery
                //{
                //    DataType = "교재 및 VOD",
                //    DelyCo = x.DELIVERYDESC,
                //    InvoiceNum = "",
                //    Pkid = x.CHARGENO.ToString(),
                //    Pname = x.PRODNAME,
                //    Unitprice = x.UNITPRICE.Value,
                //    Price = x.PRICE.Value,
                //    Status = x.USESTATE,
                //    Itemcnt = x.ITEMCNT.Value,
                //    WinDate = x.YMD
                //}));

                resultData.TotalDataCount = resultList.Count;
                if (condition.PageSize > -1)
                {
                    if (condition.PageSize == 0)
                    {
                        condition.PageSize = 10;
                    }
                    resultList = resultList.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
                }
                resultData.ListData = resultList;
            }
            return resultData;

        }

        #endregion

        #region 불공정거래 센터

        public ListModel<TAB_BOARD_AA> GetReport119(LoginUserInfo loginUserInfo, MyPageCondition condition)
        {
            ListModel<TAB_BOARD_AA> resultData = new ListModel<TAB_BOARD_AA>();
                //var refList = db49_wowtv.TAB_BOARD.AsNoTracking().AsQueryable().Where(x =>x.BCODE.Equals("N04400000") && x.USER_ID.Equals(loginUserInfo.UserId)).Select(x=>x.REF).ToList();
                var refList = db49_wownet.TAB_BOARD.AsNoTracking().AsQueryable()
                    .Where(x => x.USER_ID.Equals(loginUserInfo.UserId)).Select(x => x.REF).ToList();

                var boardList = from board in db49_wownet.TAB_BOARD.AsNoTracking()
                                where board.VIEW_FLAG.Equals("Y")
                                      && board.BCODE.Equals("N04400000")
                                      && board.USER_ID.Equals(loginUserInfo.UserId)
                                select board;

                if (!string.IsNullOrWhiteSpace(condition.SearchText))
                {
                    boardList = boardList.Where(x =>
                        x.TITLE.Contains(condition.SearchText) || x.CONTENT.Contains(condition.SearchText));
                }
                boardList = boardList.Where(x => refList.Contains(x.REF));

                resultData.TotalDataCount = boardList.Count();
                if (condition.PageSize > -1)
                {
                    if (condition.PageSize == 0)
                    {
                        condition.PageSize = 10;
                    }
                    boardList = boardList.OrderByDescending(x => x.REF).ThenBy(x => x.REF_STEP)
                        .Skip(condition.CurrentIndex).Take(condition.PageSize);
                }
                resultData.ListData = boardList.ToList();

            foreach (var item in resultData.ListData)
            {
                if (item != null)
                {
                    item.REPORT_CODE_NAME = db49_wownet.TAB_CODE.SingleOrDefault(x => x.CODE_NAME.Equals(item.CODE))
                        ?.CODE_VAL;
                }
            }

            return resultData;
        }

        public TAB_BOARD_AA GetReport119Detail(int seq)
        {
            var singleReport = db49_wownet.TAB_BOARD.AsNoTracking().AsQueryable()
                .SingleOrDefault(x => x.SEQ.Equals(seq));

            if (singleReport != null)
            {
                singleReport.REPORT_CODE_NAME = db49_wownet.TAB_CODE
                    .SingleOrDefault(x => x.CODE_NAME.Equals(singleReport.CODE))?.CODE_VAL;
            }

            return singleReport;
        }

        #endregion

        #region 등급별 혜택

        public MyClassResult GetMyClass(LoginUserInfo loginUserInfo)
        {
            var userInfo = db89_wowbill.tblUser.SingleOrDefault(x => x.userId.Equals(loginUserInfo.UserId));
            MyClassResult myClassResult = new MyClassResult();

            var outputNextAgainClassAmount = new ObjectParameter("po_intNextAgainClassAmount", typeof(Int32));
            var outputUsedPrice = new ObjectParameter("po_intUsedPrice", typeof(Int32));
            var outputUserClass = new ObjectParameter("po_intUserClass", typeof(Int32));
            var outputNextClassAmount = new ObjectParameter("po_intNextClassAmount", typeof(Int32));
            if (userInfo == null) return myClassResult;

            db89_WOWTV_BILL_DB.UP_PORTAL_USERCLASS_INFO_GET(loginUserInfo.UserNumber, outputUsedPrice, outputUserClass, outputNextClassAmount, outputNextAgainClassAmount);

            myClassResult.UsedPrice = (int)outputUsedPrice.Value;
            myClassResult.UserClass = (int)outputUserClass.Value;
            myClassResult.NextClassAmount = (int)outputNextClassAmount.Value;
            myClassResult.NextAgainClassAmount = (int)outputNextAgainClassAmount.Value;
            return myClassResult;

        }

        public List<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetUsedServiceList(LoginUserInfo loginUserInfo)
        {
            ObjectParameter output = new ObjectParameter("po_intRecordCnt", typeof(Int32));
            var list = db89_WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST(loginUserInfo.UserNumber, null, null, null, 2, 3, 1, output);

            return list.ToList();
        }

        #endregion

        #region 메인 퀵메뉴
        /// <summary>
        /// 메인 퀵메뉴 뉴스 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        //public ListModel<JOIN_TAB_SCRAP_CONTENT> GetMainQuick(LoginUserInfo loginUser)
        //{

        //    ListModel<JOIN_TAB_SCRAP_CONTENT> resultData = new ListModel<JOIN_TAB_SCRAP_CONTENT>();

        //    var newsPinList = from scrapContent in db49_wownet.TAB_SCRAP_CONTENT.AsNoTracking()
        //                      join scrapMenu in db49_wownet.TAB_SCRAP_MENU.AsNoTracking() on scrapContent.BSEQ equals scrapMenu.MSEQ
        //                      where scrapContent.USER_ID == loginUser.UserId
        //                            && scrapContent.BSEQ != 0
        //                      select new JOIN_TAB_SCRAP_CONTENT
        //                      {
        //                          BSEQ = scrapContent.BSEQ,
        //                          TITLE = scrapContent.TITLE,
        //                          CNAME = scrapContent.CNAME,
        //                          HashTag = "",
        //                          ArticleID = scrapContent.ArticleID,
        //                          PRO_ID = scrapContent.PRO_ID,
        //                          REGDATE = scrapContent.REGDATE,
        //                          SCRAPDATE = scrapContent.SCRAPDATE,
        //                          SEQ = scrapContent.SEQ,
        //                          URL = scrapContent.URL,
        //                          USER_ID = scrapContent.USER_ID,
        //                          FOLDER_NAME = scrapMenu.FOLDER_NAME,
        //                          IMGFile = "",
        //                          THUMBNAIL_FILENM = "",

        //                      };

        //    resultData.TotalDataCount = newsPinList.Count();
        //    newsPinList = newsPinList.OrderByDescending(a => a.SCRAPDATE).Take(4);
        //    resultData.ListData = newsPinList.ToList();

        //    //result: "삼성전자,반도체,수원"
        //    foreach (var item in resultData.ListData)
        //    {
        //        var hashTag = db49_Article.tblArticleList.SingleOrDefault(x => x.ArtID.Equals(item.ArticleID));

        //        if (!string.IsNullOrWhiteSpace(hashTag?.Tag))
        //        {
        //            item.HashTag = hashTag.Tag;
        //        }
        //        else if (!string.IsNullOrWhiteSpace(hashTag?.Keywords))
        //        {
        //            item.HashTag = hashTag.Keywords;
        //        }

        //    }

        //    foreach(var item)


        //    return resultData;

        //}

        /// <summary>
        /// 메인 퀵메뉴 프로그램 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public ListModel<NTB_MYPIN_PROGRAM> GetQuickProgramPin(LoginUserInfo loginUser)
        {

            ListModel<NTB_MYPIN_PROGRAM> resultData = new ListModel<NTB_MYPIN_PROGRAM>();

            var programPinList = db49_wowtv.NTB_MYPIN_PROGRAM.AsNoTracking().AsQueryable()
                .Where(x => x.USER_ID.Equals(loginUser.UserId)).ToList();

            foreach (var item in programPinList)
            {
                var programInfo = db90_DNRS.T_NEWS_PRG.AsNoTracking().AsQueryable()
                    .SingleOrDefault(x => x.PRG_CD.Equals(item.PRG_CD) && x.DELFLAG.Equals("0"));
                var tvProgramInfo = db90_DNRS.tv_program.AsNoTracking().AsQueryable()
                    .OrderByDescending(x => x.UploadTime).FirstOrDefault(x => x.Dep.Equals(item.PRG_CD));

                if (programInfo != null)
                {

                    item.SUB_IMG = new AttachFile.AttachFileBiz().GetAt("T_NEWS_PRG-SUB", programInfo.PRG_CD)
                        ?.REAL_WEB_PATH;
                    item.BRO_START = programInfo.BRO_START;
                    item.BRO_END = programInfo.BRO_END;
                    item.PRG_NM = programInfo.PRG_NM;
                    item.PGMDAY = ConvertBroadDay(programInfo.PGMDAY);
                }

                if (tvProgramInfo != null) item.TV_REPLAY = tvProgramInfo.Program_Name;
            }


            resultData.TotalDataCount = programPinList.Count;
            programPinList = programPinList.OrderByDescending(a => a.SCRAPDATE).Take(4).ToList();

            resultData.ListData = programPinList;

            return resultData;
        }

        /// <summary>
        /// 메인 퀵메뉴 파트너 핀 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public ListModel<NTB_MYPIN_PARTNER> GetQuickPartnerPin(LoginUserInfo loginUser)
        {

            ListModel<NTB_MYPIN_PARTNER> resultData = new ListModel<NTB_MYPIN_PARTNER>();

            var partnerPinList = db49_wowtv.NTB_MYPIN_PARTNER.AsNoTracking().AsQueryable();
            partnerPinList = partnerPinList.Where(x => x.USER_ID.Equals(loginUser.UserId));


            resultData.TotalDataCount = partnerPinList.Count();
            partnerPinList = partnerPinList.OrderByDescending(a => a.SCRAPDATE).Take(4);
            resultData.ListData = partnerPinList.ToList();

            foreach (var ntbMypinPartner in resultData.ListData)
            {
                var partnerinfo = db49_broadcast.Pro_wowList.AsNoTracking().AsQueryable()
                    .SingleOrDefault(x => x.Pay_no.Equals(ntbMypinPartner.PAY_NO));

                ntbMypinPartner.NEWphoto_small = partnerinfo?.NEWphoto_small;
                ntbMypinPartner.FullName = partnerinfo?.NickName; //필명으로 수정

            }  
            return resultData;
        }
        #endregion

        public string GetPartnerCafeDomain(string wowtvId)
        {
            var CafeDomain = "";
            var cafeInfo = db49_wownet.usp_Select_MasterOpenCafeInfo(wowtvId).SingleOrDefault();
            if (cafeInfo != null)
            {
                CafeDomain = cafeInfo.CafeDomain;
            }
            else
            {
                CafeDomain = "";
            }

            return CafeDomain;
        }
    }
}
