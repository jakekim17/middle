using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.Stats;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db89.wowbill;

namespace Wow.Tv.Middle.Biz.NewsCenter
{
    public class StatsBiz : BaseBiz
    {
        /// <summary>
        /// 기사 조회수 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<NUP_ARTICLE_SELECT_Result> GetArticleSearchList(StatsCondition condition)
        {
            ListModel<NUP_ARTICLE_SELECT_Result> resultData = new ListModel<NUP_ARTICLE_SELECT_Result>();


            string title = "", writer = "", artid = "";

            //검색
            if (!string.IsNullOrEmpty(condition.SearchText))
            {
                if (condition.SearchType.Equals("TITLE"))
                {
                    title = condition.SearchText;
                }
                else if (condition.SearchType.Equals("WRITER"))
                {
                    writer = condition.SearchText;
                }
                else if (condition.SearchType.Equals("ARTID"))
                {
                    artid = condition.SearchText;
                }
            }

            if (string.IsNullOrEmpty(condition.FreelancerID))
            {
                condition.FreelancerID = "";
            }

            List<NUP_ARTICLE_SELECT_Result> articleList = db49_Article.NUP_ARTICLE_SELECT(condition.StartDate, condition.EndDate, condition.CompCode,condition.DeptCD, writer, title, artid, condition.FreelancerID).ToList();

            resultData.TotalDataCount = articleList.Count();
            List<NUP_ARTICLE_SELECT_Result> pageList = null;
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                //pageList = articleList.OrderByDescending(a => a.IDX).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
                pageList = articleList.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }
            resultData.ListData = pageList;

            return resultData;
        }

        /// <summary>
        /// 기사 제공사 트래픽 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<NUP_ARTICLE_PROVIDE_SELECT_Result> GetProvideSearchList(StatsCondition condition)
        {
            List<NUP_ARTICLE_PROVIDE_SELECT_Result> provideList = db49_Article.NUP_ARTICLE_PROVIDE_SELECT(condition.StartDate, condition.EndDate, condition.CompCode).ToList();

            return provideList;
        }

        /// <summary>
        /// 프리랜서 트래픽 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<NUP_FREELANCER_TRAFFIC_SELECT_Result> GetFreelancerTrafficList(StatsCondition condition)
        {
            db49_Article.Database.CommandTimeout = 300;
           

            List<NUP_FREELANCER_TRAFFIC_SELECT_Result> freelancerTrafficList = db49_Article.NUP_FREELANCER_TRAFFIC_SELECT(condition.StartDate, condition.EndDate, condition.CompCode).ToList();

            foreach (var item in freelancerTrafficList)
            {
                item.User_NM = db90_DNRS.T_USER.SingleOrDefault(x => x.USER_ID == item.USER_ID)?.USER_NM;
            }

            return freelancerTrafficList;
        }

        /// <summary>
        /// 기자별 기사 TOP 10 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<NUP_FREELANCER_ARTICLE_RANK_SELECT_Result> GetFreelancerArticleRankList(StatsCondition condition)
        {
            db49_Article.Database.CommandTimeout = 300;

            List<NUP_FREELANCER_ARTICLE_RANK_SELECT_Result> freelancerTrafficList = db49_Article.NUP_FREELANCER_ARTICLE_RANK_SELECT(condition.StartDate, condition.EndDate, condition.CompCode,condition.SearchText).ToList();

            return freelancerTrafficList;
        }

        /// <summary>
        /// 회원 현황 조회
        /// </summary>
        /// <returns></returns>
        public PresentMember GetPresentMember(DateTime startDateTime,DateTime endDateTime)
        {
            var tbluser = db89_wowbill.tblUser.AsQueryable();
            PresentMember presentMember = new PresentMember();

            int totalMember = tbluser.Count(x => x.apply.Equals(true));
            // Difference in days, hours, and minutes.
            TimeSpan endDatets = endDateTime - DateTime.Now;
            TimeSpan startDatets = startDateTime - DateTime.Now;

            // Difference in days.
            int startDifferenceInDays = startDatets.Days;
            int endDifferenceInDays = endDatets.Days;
            int joinMember;
            int quitMember;

            var tblUserHst = db89_wowbill.tblUserHistory.AsQueryable();

            if (startDifferenceInDays == 0 && endDifferenceInDays == 0)
            { //당일 조회시 
                joinMember = tbluser.Count(x => x.apply.Equals(true) && x.registDt >= startDateTime && x.registDt <= endDateTime);
                quitMember = tblUserHst.Count(x => x.operationType.Equals("삭제") && x.registDt >= startDateTime && x.registDt <= endDateTime);
            }
            else
            {
                string sqlQuery = $@"
                    select  isnull(sum(joinCountAll),0) as joinCountAll
                          , isnull(sum(secessionCount),0) as secessionCount
                      from dbo.tbluserLoginStatistics
                     where baseDate between '{startDateTime}' and '{endDateTime.AddDays(-1)}'";

                var tbluserLoginStatisticsData = db89_wowbill.Database.SqlQuery<tbluserLoginStatistics>(sqlQuery).SingleOrDefault();

                int joinCountAll = 0;
                int secessionCount = 0;


                if (tbluserLoginStatisticsData != null)
                {
                    joinCountAll = tbluserLoginStatisticsData.JoinCountAll;
                    secessionCount = tbluserLoginStatisticsData.SecessionCount;
                }

                //종료일이 당일이면...
                if (endDifferenceInDays == 0)
                {
                    int todayMember = tbluser.Count(x => x.apply.Equals(true) && x.registDt >= endDateTime);
                    int todayQuitMember = tblUserHst.Count(x => x.operationType.Equals("삭제") && x.registDt >= endDateTime);

                    joinMember = joinCountAll + todayMember;
                    quitMember = secessionCount + todayQuitMember;
                }
                else
                {
                    joinMember = joinCountAll;
                    quitMember = secessionCount;
                }
            }

            // 웹툰 가입자수 조회 신규가입체크 기준일시 : 6/27일 14:30부터 ~ 현재까지(누적 데이터)
            //AS-IS 동일하게 조회
            DateTime registDt = DateTime.Parse("2017-06-27 14:30:00");
            int webToonMember = tbluser.Count(x => x.option1.Contains("JOY") && x.apply.Equals(true) && x.registDt >= registDt);

            presentMember.TotalMember = totalMember;
            presentMember.JoinMember = joinMember;
            presentMember.WebToonMember = webToonMember;
            presentMember.QuitMember = quitMember;
            return presentMember;
        }

        /// <summary>
        /// 주간 트래픽 순위 저장
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="loginUser"></param>
        public void SaveTrafficRank(int rank, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    NTB_COMMON_CODE commonCode = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(x => x.COMMON_CODE.Equals("025002000"));
                                        
                    if (commonCode != null)
                    {
                        commonCode.CODE_VALUE1 = rank.ToString();
                        commonCode.MOD_DATE = DateTime.Now;
                        commonCode.MOD_ID = loginUser.LoginId;
                        db49_wowtv.NTB_COMMON_CODE.AddOrUpdate(commonCode);
                        db49_wowtv.SaveChanges();
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
        /// 기사번호로 기자명을 가져온다.
        /// </summary>
        /// <param name="compCode">compCode</param>
        /// <param name="artId">기사번호</param>
        /// <returns></returns>
        public string GetArtIdbyWriter(string compCode, string artId)
        {
            var tblArticleList = db49_Article.tblArticleList.AsQueryable();
            string writer = tblArticleList.Where(x => x.CompCode.Equals(compCode) && x.ArtID.Equals(artId)).Select(x => x.Writer).SingleOrDefault();
            return writer;
        }

        /// <summary>
        /// 기사 제보수
        /// </summary>
        /// <param name="menuSeq"></param>
        /// <returns></returns>
        public int ReportCount(DateTime startDateTime, DateTime endDateTime,string contentSeq)
        {
            var list = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().AsQueryable();
            //기간 검색
            list = list.Where(x => x.REG_DATE >= startDateTime && x.REG_DATE <= endDateTime);
            list = list.Where(x => x.BOARD_CONTENT_SEQ.ToString().Equals(contentSeq));
            list = list.Where(x => x.DEPTH.Equals(0));
            list = list.Where(x => x.DEL_YN.Equals("N"));

            var reportCount = list.Count();

            return reportCount;
        }
    }

}