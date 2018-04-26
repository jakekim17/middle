using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.Biz.NewsCenter
{

    /// <summary>
    /// <para>  뉴스메인 Biz</para>
    /// <para>- 작  성  자 : ABC솔루션 오규환</para>
    /// <para>- 최초작성일 : 2017-10-23</para>
    /// <para>- 최종수정자 : ABC솔루션 오규환</para>
    /// <para>- 최종수정일 : 2017-10-23</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-10-23 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class NewsMainBiz : BaseBiz
    {

        /// <summary>
        /// 메인 뉴스 스탠드
        /// </summary>
        /// <returns>ListModel<NUP_NEWS_MAIN_NEWSSTAND_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_NEWSSTAND_SELECT_Result> GetNewsMainNewsstand()
        {
            ListModel<NUP_NEWS_MAIN_NEWSSTAND_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_NEWSSTAND_SELECT_Result>();

            try
            {
                resultData.ListData = db49_Article.NUP_NEWS_MAIN_NEWSSTAND_SELECT().ToList();
            }
            catch (Exception ex)
            {
                Wow.Fx.WowLog.Write("GetNewsMainNewsstand => " + ex.Message);
                if (ex.InnerException != null)
                {
                    Wow.Fx.WowLog.Write("GetNewsMainNewsstand Inner => " + ex.InnerException.Message);
                }
            }

            return resultData;
        }        

        /// <summary>
        /// 메인 오피니언
        /// </summary>
        /// <returns>ListModel<NUP_NEWS_MAIN_OPINION_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_OPINION_SELECT_Result> GetNewsMainOpinionList()
        {
            ListModel<NUP_NEWS_MAIN_OPINION_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_OPINION_SELECT_Result>();

            try
            {
                resultData.ListData = db49_Article.NUP_NEWS_MAIN_OPINION_SELECT().ToList();
            }
            catch (Exception ex)
            {
                Wow.Fx.WowLog.Write("GetNewsMainOpinionList => " + ex.Message);
                if (ex.InnerException != null)
                {
                    Wow.Fx.WowLog.Write("GetNewsMainOpinionList Inner => " + ex.InnerException.Message);
                }
            }

            return resultData;
        }

        /// <summary>
        /// 메인 마켓 & 이슈
        /// </summary>
        /// <returns>ListModel<NUP_NEWS_MAIN_MARKET_ISSUE_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_MARKET_ISSUE_SELECT_Result> GetNewsMainMarketIssueList()
        {

            ListModel<NUP_NEWS_MAIN_MARKET_ISSUE_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_MARKET_ISSUE_SELECT_Result>();

            try
            {
                resultData.ListData = db49_Article.NUP_NEWS_MAIN_MARKET_ISSUE_SELECT().ToList();
            }
            catch (Exception ex)
            {
                Wow.Fx.WowLog.Write("GetNewsMainMarketIssueList => " + ex.Message);
                if (ex.InnerException != null)
                {
                    Wow.Fx.WowLog.Write("GetNewsMainMarketIssueList Inner => " + ex.InnerException.Message);
                }
            }

            return resultData;
        }

        /// <summary>
        /// 연예.스포츠 메인 리스트
        /// </summary>
        /// <param name="searchGubun">검색 조건</param>
        /// <param name="articleIdList">제외할 기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_ENT_SPO_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_ENT_SPO_SELECT_Result> GetNewsMainEntSpoList(string searchGubun, List<String> articleIdList)
        {
            //string articleIdIn = String.Join(",", articleIdList.ToArray());
            string articleIdIn = String.Join(",", articleIdList.Select(x => $"'{x}'"));

            Wow.Fx.WowLog.Write(articleIdIn);

            ListModel<NUP_NEWS_MAIN_ENT_SPO_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_ENT_SPO_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_NEWS_MAIN_ENT_SPO_SELECT(searchGubun, articleIdIn).ToList();

            return resultData;
        }

        /// <summary>
        /// 메인 동영상뉴스 N개 리스트
        /// </summary>
        /// <param name="searchGubun">검색 조건</param>
        /// <param name="topN">리스트 개수</param>
        /// <param name="articleIdList">제외할 기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_VOD_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_VOD_SELECT_Result> GetNewsMainVodList(string searchGubun, int? topN, List<String> articleIdList)
        {
            //string articleIdIn = String.Join(",", articleIdList.ToArray());
            string articleIdIn = String.Join(",", articleIdList.Select(x => $"'{x}'"));

            ListModel<NUP_NEWS_MAIN_VOD_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_VOD_SELECT_Result>();

            try
            {
                resultData.ListData = db49_Article.NUP_NEWS_MAIN_VOD_SELECT(searchGubun, topN, articleIdIn).ToList();
            }
            catch(Exception ex)
            {
                resultData.ListData = new List<NUP_NEWS_MAIN_VOD_SELECT_Result>();
                Wow.Fx.WowLog.Write("GetNewsMainVodList => " + ex.Message);
                if(ex.InnerException != null)
                {
                    Wow.Fx.WowLog.Write("GetNewsMainVodList Inner => " + ex.InnerException.Message);
                }
            }

            return resultData;
        }

        /// <summary>
        /// 뉴스 메인 & 컨텐츠 카드뉴스 N개 리스트
        /// </summary>
        /// <param name="searchGubun">검색 조건</param>
        /// <param name="topN">리스트 개수</param>
        /// <param name="articleIdList">제외할 기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_CARD_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_CARD_SELECT_Result> GetNewsMainCardList(string searchGubun, int? topN, List<String> articleIdList)
        {
            //string articleIdIn = String.Join(",", articleIdList.ToArray());
            string articleIdIn = String.Join(",", articleIdList.Select(x => $"'{x}'"));

            ListModel<NUP_NEWS_MAIN_CARD_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_CARD_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_NEWS_MAIN_CARD_SELECT(searchGubun, topN, articleIdIn).ToList();

            return resultData;
        }

        /// <summary>
        /// 뉴스 메인 & 컨텐츠 셕션 N개 리스트
        /// </summary>
        /// <param name="searchGubun">검색 조건</param>
        /// <param name="topN">리스트 개수</param>
        /// <param name="articleIdList">제외할 기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_SECTION_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_SECTION_SELECT_Result> GetNewsMainSectionList(string searchGubun, int? topN, List<String> articleIdList)
        {
            //string articleIdIn = String.Join(",", articleIdList.ToArray());
            string articleIdIn = String.Join(",", articleIdList.Select(x => $"'{x}'"));

            ListModel<NUP_NEWS_MAIN_SECTION_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_SECTION_SELECT_Result>();

            try
            {
                resultData.ListData = db49_Article.NUP_NEWS_MAIN_SECTION_SELECT(searchGubun, topN, articleIdIn).ToList();
            }
            catch (Exception ex)
            {
                resultData.ListData = new List<NUP_NEWS_MAIN_SECTION_SELECT_Result>();

                Wow.Fx.WowLog.Write("searchGubun => " + searchGubun);
                Wow.Fx.WowLog.Write("GetNewsMainSectionList => " + ex.Message);
                if (ex.InnerException != null)
                {
                    Wow.Fx.WowLog.Write("GetNewsMainSectionList Inner => " + ex.InnerException.Message);
                }
            }

            return resultData;
        }

        /// <summary>
        /// 뉴스 메인 리스트(관리자 LIST "Y" 체크된 리스트)
        /// </summary>
        /// <param name="articleIdList">기사 ID</param>
        /// <returns>List<NUP_NEWS_MAIN_LIST_Y_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_LIST_Y_SELECT_Result> GetNewsMainYList(List<String> articleIdList)
        {
            //string articleIdIn = String.Join(",", articleIdList.ToArray());
            string articleIdIn = String.Join(",", articleIdList.Select(x => $"'{x}'"));

            ListModel<NUP_NEWS_MAIN_LIST_Y_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_LIST_Y_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_NEWS_MAIN_LIST_Y_SELECT(articleIdIn).ToList();

            return resultData;
        }

        /// <summary>
        /// 주요시세[특징주,종목,공시,와우넷]
        /// </summary>
        /// <param name="searchGubun">검색구분(특징주,종목,공시,와우넷)</param>
        /// <param name="topN">노출 개수</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_MARKET_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_MARKET_SELECT_Result> GetNewsMainMarketList(string searchGubun, int topN, List<String> articleIdList)
        {
            //string articleIdIn = String.Join(",", articleIdList.ToArray());
            string articleIdIn = String.Join(",", articleIdList.Select(x => $"'{x}'"));

            ListModel<NUP_NEWS_MAIN_MARKET_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_MARKET_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_NEWS_MAIN_MARKET_SELECT(searchGubun, topN, articleIdIn).ToList();

            //Wow.Fx.WowLog.Write(searchGubun);
            //Wow.Fx.WowLog.Write(topN.ToString());
            //Wow.Fx.WowLog.Write(articleIdIn);

            return resultData;
        }

        /// <summary>
        /// 부동산 메인 SELECT
        /// </summary>
        /// <returns>ListModel<NUP_NEWS_MAIN_LAND_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_LAND_SELECT_Result> GetNewsMainLandList()
        {
            ListModel<NUP_NEWS_MAIN_LAND_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_LAND_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_NEWS_MAIN_LAND_SELECT().ToList();

            return resultData;
        }

    }
}
