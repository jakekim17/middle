using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Wow.Fx;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.broadcast.NewsCenter;

namespace Wow.Tv.Middle.Biz.NewsCenter
{

    /// <summary>
    /// <para>  뉴스센터 Biz</para>
    /// <para>- 작  성  자 : ABC솔루션 오규환</para>
    /// <para>- 최초작성일 : 2017-08-18</para>
    /// <para>- 최종수정자 : ABC솔루션 오규환</para>
    /// <para>- 최종수정일 : 2017-08-18</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-08-18 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class NewsCenterBiz : BaseBiz
    {

        /// <summary>
        /// 뉴스 상세 내용
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <param name="prevArticleId">이전 기사 아이디</param>
        /// <param name="mediaGubun">pc,mobile 구분</param>
        /// <returns>NUP_NEWS_READ_SELECT_Result</returns>
        public NUP_NEWS_READ_SELECT_Result GetNewsReadInfo(string articleId, string prevArticleId, string mediaGubun)
        {
            NUP_NEWS_READ_SELECT_Result SingleRow = new NUP_NEWS_READ_SELECT_Result();

            try
            {
                SingleRow = db49_Article.NUP_NEWS_READ_SELECT(articleId, prevArticleId, mediaGubun).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Wow.Fx.WowLog.Write("GetNewsReadInfo => " + ex.Message);

                if (ex.InnerException != null)
                {
                    Wow.Fx.WowLog.Write("GetNewsReadInfo Inner => " + ex.InnerException.Message);
                }
            }
            
            return SingleRow;
        }

        /// <summary>
        /// 뉴스 상세 관련 기사 리스트
        /// </summary>
        /// <param name="articleIdList">기사 아이디</param>
        /// <param name="articleIdList">제외할 기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_READ_RELATION_SELECT_Result></returns>
        public ListModel<NUP_NEWS_READ_RELATION_SELECT_Result> GetNewsReadRelationList(string articleId, List<String> articleIdList)
        {
            string articleIdIn = String.Join(",", articleIdList.ToArray());

            //Wow.Fx.WowLog.Write("GetNewsReadRelationList");
            //Wow.Fx.WowLog.Write(articleIdIn);

            ListModel<NUP_NEWS_READ_RELATION_SELECT_Result> resultData = new ListModel<NUP_NEWS_READ_RELATION_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_NEWS_READ_RELATION_SELECT(articleId, articleIdIn).ToList();

            return resultData;
        }

        /// <summary>
        /// 기사의 좋아요 COUNT
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns></returns>
        public NTB_ARTICLE_LIKEIT GetArticleLikeit(string articleId)
        {
            NTB_ARTICLE_LIKEIT SingleRow = db49_Article.NTB_ARTICLE_LIKEIT.SingleOrDefault(p => p.ARTICLE_ID == articleId);

            return SingleRow;
        }

        /// <summary>
        /// 기사의 좋아요 Insert & Update
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <param name="likeitGubun">좋아요 구분</param>
        public void SetArticleLikeit(string articleId, string likeitGubun)
        {
            db49_Article.NUP_ARTICLE_LIKEIT_INSERT_UPDATE(articleId, likeitGubun);
        }

        /// <summary>
        /// 이전, 다음 기사
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <param name="condition">검색 조건</param>
        /// <returns>ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result></returns>
        public ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result> GetNewsPrevNext(string articleId, NewsCenterCondition condition)
        {
            ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result> resultData = new ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result>();

            string SearchSection = condition.SearchSection;
            string SearchWowCode = condition.SearchWowCode;
            string Class         = condition.Class;
            string SearchText    = condition.SearchText;

            resultData.ListData = db49_Article.NUP_NEWS_PREV_NEXT_SELECT(articleId, SearchSection, SearchWowCode, Class, SearchText).ToList();

            return resultData;
        }

        /// <summary>
        /// 이기사와 많이본 기사
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_THIS_MANY_SEE_SELECT_Result></returns>
        public ListModel<NUP_NEWS_THIS_MANY_SEE_SELECT_Result> GetNewsThisManySee(string articleId)
        {
            ListModel<NUP_NEWS_THIS_MANY_SEE_SELECT_Result> resultData = new ListModel<NUP_NEWS_THIS_MANY_SEE_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_NEWS_THIS_MANY_SEE_SELECT(articleId).ToList();

            return resultData;
        }

        /// <summary>
        /// 색션별 많이 본기사
        /// </summary>
        /// <param name="searchSection">섹션구분</param>
        /// <param name="searchWowCode">와우코드</param>
        /// <returns>ListModel<NUP_NEWS_SECTION_MANY_SEE_SELECT_Result></returns>
        public ListModel<NUP_NEWS_SECTION_MANY_SEE_SELECT_Result> GetNewsSectionManySee(string searchSection, string searchWowCode)
        {
            ListModel<NUP_NEWS_SECTION_MANY_SEE_SELECT_Result> resultData = new ListModel<NUP_NEWS_SECTION_MANY_SEE_SELECT_Result>();

            int topN = 6;
            //개발 DB 데이터가 없어 임시 -200일 
            string searchStardDate = DateTime.Now.AddDays(-14).ToString("yyyy-MM-dd");
            //string searchStardDate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
            string searchEndDate =  DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            resultData.ListData = db49_Article.NUP_NEWS_SECTION_MANY_SEE_SELECT(searchSection, searchWowCode, topN, searchStardDate, searchEndDate).ToList();

            return resultData;
        }

        /// <summary>
        /// 기사의 종목 코드
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns>ListModel<ArticleStock></returns>
        public ListModel<ArticleStock> GetArticleStockList(string articleId)
        {
            ListModel<ArticleStock> resultData = new ListModel<ArticleStock>();

            resultData.ListData = db49_Article.ArticleStock.Where(p => p.ArticleID.Equals(articleId)).ToList();

            return resultData;
        }

        /// <summary>
        /// 뉴스 상세 관련종목 정보
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_READ_SELECT_Result></returns>
        public ListModel<usp_GetStockPrice_Result> GetStockInfo(string articleId)
        {
            var resultData = new ListModel<usp_GetStockPrice_Result>();
            var list = new List<usp_GetStockPrice_Result>();
            List<ArticleStock> stockCode = db49_Article.ArticleStock.Where(a => a.ArticleID.Equals(articleId)).OrderBy(o => Guid.NewGuid()).Take(3).ToList();

            foreach (var item in stockCode)
            {
                var model = new usp_GetStockPrice_Result();
                model = db22_stock.usp_GetStockPrice(item.StockCode).FirstOrDefault();

                list.Add(model);

            }
            resultData.ListData = list;
            return resultData;
        }

        /// <summary>
        /// 뉴스 상세 파트너 기자 정보
        /// </summary>
        /// <param name="reporterId">기자 아이디</param>
        /// <param name="tag">파트너아이디</param>
        /// <returns>PartnerInfo</returns>
        public PartnerInfo GetReportPartnerInfo(NewsCenterCondition condition, string tag)
        {
            var TagVal = tag.Substring(1);
            var resultData = new PartnerInfo();
            //파트너 정보 호출
            var wowPro = db49_broadcast.Pro_wowList.Where(a => a.Pro_id.Equals(TagVal)).FirstOrDefault();

            var broadCast = db49_broadcast.USP_GetBroadcast1ByProId(wowPro.Pro_id).SingleOrDefault();

            if (!string.IsNullOrWhiteSpace(broadCast?.BMEMTYPE))
            {

                if (broadCast.BMEMTYPE.Equals("N") || broadCast.BMEMTYPE.Equals("U"))
                {
                    resultData.BoradType = "회원전용";
                }
                else if (broadCast.BMEMTYPE.Equals("F"))
                {
                    resultData.BoradType = "무료방송";
                }
            }


            if (string.IsNullOrWhiteSpace(broadCast?.BRSTARTTIME) || string.IsNullOrWhiteSpace(broadCast?.BRENDTIME))
            {
                resultData.BroadCastTime = "-";
            }
            else
            {
                resultData.BroadCastTime = broadCast.BRSTARTTIME.Substring(0, 4) + "-" + broadCast.BRSTARTTIME.Substring(4, 2) + "-" + broadCast.BRSTARTTIME.Substring(6, 2)
                                            + " [" + broadCast.BRSTARTTIME.Substring(8, 2) + ":" + broadCast.BRSTARTTIME.Substring(10, 2)
                                            + " ~ " + broadCast.BRENDTIME.Substring(8, 2) + ":" + broadCast.BRENDTIME.Substring(10, 2) + "]";
            }

            resultData.ProWowList = wowPro;//파트너정보
            resultData.BroadState = broadCast;//방송상태 체크

            return resultData;
        }

        /// <summary>
        /// 뉴스 상세 파트너 아닌 기자 정보
        /// </summary>
        /// <param name="reporterId">기자 아이디</param>
        /// <returns>ListModel<NUP_REPORTER_SELECT_Result></returns>
        public ListModel<NUP_REPORTER_SELECT_Result> GetReporterInfo(NewsCenterCondition condition)
        {
            var resultData = new ListModel<NUP_REPORTER_SELECT_Result>();
            //기자 정보 가져오기. 
            resultData.ListData = db49_Article.NUP_REPORTER_SELECT(condition.SearchText, null, null, 1, 1, null).Where(p => p.REPORTER_ID.Equals(condition.SearchText)).ToList();

            return resultData;
        }

        /// <summary>
        /// 색션별 뉴스 리스트
        /// </summary>
        /// <param name="condition">검색 조건</param>
        /// <returns>ListModel<NUP_NEWS_SECTION_SELECT_Result></returns>
        public ListModel<NUP_NEWS_SECTION_SELECT_Result> GetNewsSectionList(NewsCenterCondition condition)
        {
            ListModel<NUP_NEWS_SECTION_SELECT_Result> resultData = new ListModel<NUP_NEWS_SECTION_SELECT_Result>();

            string SearchSection = condition.SearchSection;
            string SearchText = condition.SearchText;
            string SearchWowCode = condition.SearchWowCode;
            string SearchComp = condition.SearchComp;
            string StartDate = condition.StartDate;
            string EndDate = condition.EndDate;
            int Page = condition.Page;
            int PageSize = condition.PageSize;

            try
            {
                resultData.ListData = db49_Article.NUP_NEWS_SECTION_SELECT(SearchSection, SearchText, SearchWowCode, SearchComp, StartDate, EndDate, Page, PageSize).ToList();
            }
            catch (Exception ex)
            {
                resultData.ListData = new List<NUP_NEWS_SECTION_SELECT_Result>();
                Wow.Fx.WowLog.Write("GetNewsSectionList => " + ex.Message);
                if (ex.InnerException != null)
                {
                    Wow.Fx.WowLog.Write("GetNewsSectionList Inner => " + ex.InnerException.Message);
                }
            }


            return resultData;
        }


        #region 기사 카운터 조회
        /// <summary>
        /// 기사 카운터 조회
        /// </summary>
        /// <param name="startDate">시작일</param>
        /// <param name="endDate">종료일</param>
        /// <param name="title">제목</param>
        /// <param name="articleID">기사 ID</param>
        /// <param name="userName">기자명</param>
        /// <returns>기사수</returns>
        public int GetArticleCount(string startDate, string endDate, string title, string articleID, string userName)
        {
            ObjectParameter output = new ObjectParameter("O_TOTALCOUNT", typeof(int));
            db49_Article.NUP_TBLARTICLELIST_COUNT_SELECT(startDate, endDate, title, articleID, userName, output);

            int artcleCount = int.Parse(output.Value.ToString());

            return artcleCount;
        }

        #endregion

        #region 관리자 뉴스 메인

        #region tblNewsStand
        /// <summary>
        /// 뉴스 스탠드 최종 기사 송출 시간
        /// </summary>
        /// <returns>업데이트 정보</returns>
        public tblNewsStandArticleManage GetNewsStandUpdateTime()
        {
            tblNewsStandArticleManage tNsam = new tblNewsStandArticleManage();
            tNsam.artid = "isList-isHead";

            tblNewsStandArticleManage SingleRow = db49_Article.tblNewsStandArticleManage.SingleOrDefault(p => p.artid == tNsam.artid);

            return SingleRow;
        }


        /// <summary>
        /// 뉴스 스탠드 최종 기사 송출 시간 UPDATE
        /// </summary>
        /// <returns>처리결과</returns>
        public bool SetNewsStandUpdateTime()
        {
            bool isSuccess = false;
            try
            {
                var tNewsStandManage = db49_Article.tblNewsStandArticleManage.Where(p => p.artid == "isList-isHead").First();
                tNewsStandManage.uptdate = DateTime.Now;
                db49_Article.SaveChanges();

                //키워드 추출기사 업데이트
                //db49_Article.usp_tblArticleListStandKeyword_Insert();

                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }


        /// <summary>
        /// 뉴스 스탠드 리스트
        /// </summary>
        /// <returns>List<NUP_NEWSSTAND_SELECT_Result></returns>
        public ListModel<NUP_NEWSSTAND_SELECT_Result> GetNewsStandList()
        {
            ListModel<NUP_NEWSSTAND_SELECT_Result> resultData = new ListModel<NUP_NEWSSTAND_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_NEWSSTAND_SELECT().ToList();

            return resultData;
        }


        /// <summary>
        /// 뉴스 스탠드 수정 정보
        /// </summary>
        /// <returns>tblNewsStand</returns>
        public tblNewsStand GetNewsStandLastUpdateInfo()
        {
            tblNewsStand SingleRow = db49_Article.tblNewsStand.OrderByDescending(p => p.udtDt).First();

            return SingleRow;
        }


        /// <summary>
        /// 뉴스 스탠드 가제목 수정
        /// </summary>
        /// <param name="updateInfo">수정할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>결과값</returns>
        public bool SetNewsStandTmpTitleUpdate(List<tblNewsStand> updateInfo, LoginUser loginUser)
        {
            bool isSuccess = false;

            if (updateInfo.Count > 0)
            {
                try
                {
                    foreach (tblNewsStand item in updateInfo)
                    {
                        var newsStand = db49_Article.tblNewsStand.Where(p => p.artid == item.artid).First();

                        newsStand.tmpTitle = item.tmpTitle;
                        newsStand.udtAdminId = loginUser.LoginId;
                        newsStand.udtAdminName = loginUser.UserName;
                        newsStand.udtDt = DateTime.Now;
                    }

                    db49_Article.SaveChanges();

                    isSuccess = true;
                }
                catch (Exception e)
                {
                    isSuccess = false;
                }
            }

            return isSuccess;
        }


        #endregion

        /// <summary>
        /// 뉴스 메인 리스트 설정
        /// </summary>
        /// <param name="updateInfo">수정할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>결과값</returns>
        public bool SetNewsMainListUpdate(List<NUP_NEWS_MAIN_MAGE_SELECT_Result> updateInfo, LoginUser loginUser)
        {
            bool isSuccess = false;

            if (updateInfo.Count > 0)
            {
                try
                {
                    foreach (NUP_NEWS_MAIN_MAGE_SELECT_Result item in updateInfo)
                    {
                        //NTB_ARTICLE_CONFIG Insert, Update
                        var articleConfig = db49_Article.NTB_ARTICLE_CONFIG.SingleOrDefault(p => p.ARTICLE_ID == item.ARTID);
                        if (articleConfig == null)
                        {
                            //Insert
                            NTB_ARTICLE_CONFIG model = new NTB_ARTICLE_CONFIG();

                            model.ARTICLE_ID = item.ARTID;
                            model.LIST_YN = String.IsNullOrEmpty(item.LIST_YN) ? "N" : item.LIST_YN;
                            model.BOLD_YN = String.IsNullOrEmpty(item.BOLD_YN) ? "N" : item.BOLD_YN;
                            model.GUBUN_CODE = item.GUBUN_CODE;
                            model.REG_ID = loginUser.LoginId;
                            model.REG_DATE = DateTime.Now;
                            model.MOD_ID = loginUser.LoginId;
                            model.MOD_DATE = DateTime.Now;

                            db49_Article.NTB_ARTICLE_CONFIG.Add(model);
                        }
                        else
                        {
                            //Update
                            articleConfig.LIST_YN = String.IsNullOrEmpty(item.LIST_YN) ? "N" : item.LIST_YN;
                            articleConfig.GUBUN_CODE = item.GUBUN_CODE;
                            articleConfig.BOLD_YN = String.IsNullOrEmpty(item.BOLD_YN) ? "N" : item.BOLD_YN;
                            articleConfig.MOD_ID = loginUser.LoginId;
                            articleConfig.MOD_DATE = DateTime.Now;
                        }

                        //tblNewsStand Insert, Update
                        if (item.RANK > 0)
                        {
                            var newsStand = db49_Article.tblNewsStand.SingleOrDefault(p => p.rank == item.RANK);        //설정할 뉴스스탠드
                            var articleInfo = db49_Article.tblArticleList.SingleOrDefault(p => p.ArtID == item.ARTID);  //기사정보

                            //설정할 RANK가 tblNewsStand 없다면 INSERT
                            if (newsStand == null)
                            {
                                //Insert                                
                                tblNewsStand model = new tblNewsStand();

                                model.rank = item.RANK;
                                model.artid = item.ARTID;
                                model.tmpTitle = articleInfo.Title;
                                model.udtAdminId = loginUser.LoginId;
                                model.udtAdminName = loginUser.UserName;
                                model.udtDt = DateTime.Now;

                                db49_Article.tblNewsStand.Add(model);
                            }
                            //설정할 RANK가 tblNewsStand 있다면
                            else
                            {
                                //선택한 기사가 tblNewsStand에 등록되어 있다면
                                var targetNewsStand = db49_Article.tblNewsStand.SingleOrDefault(p => p.artid == item.ARTID);
                                if (targetNewsStand != null)
                                {
                                    //설정되어 있는 RANK의 정보를 변경
                                    var targetRank = db49_Article.tblNewsStand.SingleOrDefault(p => p.rank == newsStand.rank);

                                    targetNewsStand.artid = targetRank.artid;
                                    targetNewsStand.tmpTitle = targetRank.tmpTitle;
                                    targetNewsStand.udtAdminId = loginUser.LoginId;
                                    targetNewsStand.udtAdminName = loginUser.UserName;
                                    targetNewsStand.udtDt = DateTime.Now;
                                }

                                //Update
                                newsStand.artid = item.ARTID;
                                newsStand.tmpTitle = articleInfo.Title;
                                newsStand.udtAdminId = loginUser.LoginId;
                                newsStand.udtAdminName = loginUser.UserName;
                                newsStand.udtDt = DateTime.Now;
                            }
                        }

                        //articleClass
                        db49_Article.usp_articleClassOrderNumChangeExceptNewStand(item.RANK, item.ARTID, loginUser.LoginId, loginUser.UserName);

                        //관련기사 노출순서 NTB_ARTICLE_RELATION_CONFIG Insert, Update
                        if (item.SHOW_NUM > 0)
                        {
                            var articleRelationConfig = db49_Article.NTB_ARTICLE_RELATION_CONFIG.SingleOrDefault(p => p.SHOW_NUM == item.SHOW_NUM); //설정할 관련기사
                            var articleInfo = db49_Article.tblArticleList.SingleOrDefault(p => p.ArtID == item.ARTID);  //기사정보

                            //설정할 SHOW_NUM이 NTB_ARTICLE_RELATION_CONFIG 없다면 INSERT
                            if (articleRelationConfig == null)
                            {
                                //Insert                                
                                NTB_ARTICLE_RELATION_CONFIG model = new NTB_ARTICLE_RELATION_CONFIG();

                                model.SHOW_NUM = item.SHOW_NUM;
                                model.AUTO_MANUAL = "MANUAL";
                                model.ARTICLE_ID = item.ARTID;
                                model.ARTICLE_TITLE = articleInfo.Title;

                                model.REG_ID = loginUser.LoginId;
                                model.REG_DATE = DateTime.Now;
                                model.MOD_ID = loginUser.LoginId;
                                model.MOD_DATE = DateTime.Now;

                                db49_Article.NTB_ARTICLE_RELATION_CONFIG.Add(model);
                            }
                            //설정할 SHOW_NUM이 NTB_ARTICLE_RELATION_CONFIG 있다면
                            else
                            {
                                //선택한 기사가 NTB_ARTICLE_RELATION_CONFIG에 등록되어 있다면
                                var targetData = db49_Article.NTB_ARTICLE_RELATION_CONFIG.SingleOrDefault(p => p.ARTICLE_ID == item.ARTID);
                                if (targetData != null)
                                {
                                    //설정되어 있는 RANK의 정보를 변경
                                    var targetShowNum = db49_Article.NTB_ARTICLE_RELATION_CONFIG.SingleOrDefault(p => p.SHOW_NUM == articleRelationConfig.SHOW_NUM);

                                    targetData.AUTO_MANUAL    = targetShowNum.AUTO_MANUAL;
                                    targetData.ARTICLE_ID     = targetShowNum.ARTICLE_ID;
                                    targetData.SHOW_CONDITION = targetShowNum.SHOW_CONDITION;
                                    targetData.DEPT_CODE      = targetShowNum.DEPT_CODE;
                                    targetData.ARTICLE_TITLE  = targetShowNum.ARTICLE_TITLE;
                                    targetData.MOD_ID         = loginUser.LoginId;
                                    targetData.MOD_DATE       = DateTime.Now;
                                }

                                //Update                                
                                articleRelationConfig.AUTO_MANUAL    = "MANUAL";
                                articleRelationConfig.ARTICLE_ID     = item.ARTID;
                                articleRelationConfig.SHOW_CONDITION = null;
                                articleRelationConfig.DEPT_CODE      = null;
                                articleRelationConfig.ARTICLE_TITLE  = articleInfo.Title;
                                articleRelationConfig.MOD_ID         = loginUser.LoginId;
                                articleRelationConfig.MOD_DATE       = DateTime.Now;
                            }
                        }

                    }

                    db49_Article.SaveChanges();

                    isSuccess = true;
                }
                catch (Exception e)
                {
                    isSuccess = false;
                }
            }

            return isSuccess;
        }

        /// <summary>
        /// 뉴스 메인 관리 리스트
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_MAGE_SELECT_Result></returns>
        public ListModel<NUP_NEWS_MAIN_MAGE_SELECT_Result> GetNewsMainMageList(NewsCenterCondition condition)
        {
            string SearchComp = condition.SearchComp;
            string SearchGubun = condition.SearchGubun;
            string SearchDept = condition.SearchDept;
            string SearchText = condition.SearchText;
            string StartDate = condition.StartDate;
            string EndDate = condition.EndDate;
            int Page = condition.Page;
            int PageSize = condition.PageSize;

            ListModel<NUP_NEWS_MAIN_MAGE_SELECT_Result> resultData = new ListModel<NUP_NEWS_MAIN_MAGE_SELECT_Result>();
            resultData.ListData = db49_Article.NUP_NEWS_MAIN_MAGE_SELECT(SearchComp, SearchGubun, SearchDept, SearchText, StartDate, EndDate, Page, PageSize).ToList();

            return resultData;
        }

        /// <summary>
        /// HTML 재변환
        /// </summary>
        /// <param name="articleId">기사 ID</param>
        /// <param name="compCode">Comp Code</param>
        /// <returns>isSuccess</returns>
        public bool SetArticleHtmlConvert(string articleId, string compCode)
        {
            bool isSuccess = false;

            try
            {
                //한국경제 TV
                if (compCode.Equals("WO"))
                {
                    var articleList_WO = db49_Article.ArticleList_WO.Where(p => p.ArticleID == articleId && p.CompCode == compCode).First();
                    articleList_WO.HTML = null;
                }
                //한경닷컴
                else if (compCode.Equals("HK"))
                {
                    var articleList_HK = db49_Article.ArticleList_HK.Where(p => p.ArticleID == articleId && p.CompCode == compCode).First();
                    articleList_HK.HTML = null;
                }
                //연합뉴스
                else if (compCode.Equals("YH"))
                {
                    var articleList_YH = db49_Article.ArticleList_YH.Where(p => p.ArticleID == articleId && p.CompCode == compCode).First();
                    articleList_YH.HTML = null;
                }

                db49_Article.SaveChanges();

                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        #endregion


        /// <summary>
        /// 기사 작성자 정보
        /// </summary>
        /// <param name="ArtID">기사 ID</param>
        /// <returns>기사 작성자 내용</returns>
        public tblArticleCreation GetArtcleCreationInfo(string ArtID)
        {
            tblArticleCreation SingleRow = db49_Article.tblArticleCreation.Where(p => p.ArtID == ArtID).FirstOrDefault() ;

            return SingleRow;
        }

        /// <summary>
        /// 기사 내용
        /// </summary>
        /// <param name="ArtID">기사 ID</param>
        /// <returns>기사내용</returns>
        public tblArticleList GetArtcleInfo(string ArtID)
        {
            tblArticleList SingleRow = db49_Article.tblArticleList.Where(p => p.ArtID == ArtID).FirstOrDefault();

            return SingleRow;
        }

        #region 기사 관련 이미지
        /// <summary>
        /// 기사 관련 이미지 정보
        /// </summary>
        /// <param name="ArtID">기사 ID</param>
        /// <returns>처리결과</returns>
        public tblRelationImage GetRelationImageInfo(string ArtID)
        {
            tblRelationImage SingleRow = db49_Article.tblRelationImage.Where(p => p.sParentID == ArtID).OrderByDescending(p => p.ID).FirstOrDefault();

            return SingleRow;
        }

        /// <summary>
        /// 기사 관련 이미지 등록
        /// </summary>
        /// <param name="relationImageInfo">이미지 정보</param>
        /// <returns>처리결과</returns>
        public bool SetRelationImageSave(tblRelationImage relationImageInfo)
        {
            bool isSuccess = false;
            try
            {
                db49_Article.tblRelationImage.Add(relationImageInfo);
                db49_Article.SaveChanges();
                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
                WowLog.Write(e.Message);

            }
            return isSuccess;
        }

        /// <summary>
        /// 기사 관련 이미지 삭제
        /// </summary>
        /// <param name="relationImageInfo">이미지 정보</param>
        /// <returns>처리결과</returns>
        public bool SetRelationImageDelete(tblRelationImage relationImageInfo)
        {
            bool isSuccess = false;
            try
            {
                db49_Article.tblRelationImage.RemoveRange(db49_Article.tblRelationImage.Where(p=>p.sParentID.Equals(relationImageInfo.sParentID)));
                db49_Article.SaveChanges();
                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
                WowLog.Write(e.Message);

            }
            return isSuccess;
        }

        #endregion

        /// <summary>
        /// 카드&VOD 뉴스 리스트
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns>ListModel<NUP_NEWS_CARD_VOD_SELECT_Result></returns>
        public ListModel<NUP_NEWS_CARD_VOD_SELECT_Result> GetNewsCardVodList(NewsCenterCondition condition)
        {
            ListModel<NUP_NEWS_CARD_VOD_SELECT_Result> resultData = new ListModel<NUP_NEWS_CARD_VOD_SELECT_Result>();

            string SearchGubun = condition.SearchGubun;
            string SearchText = condition.SearchText;
            string StartDate = condition.StartDate;
            string EndDate = condition.EndDate;
            int Page = condition.Page;
            int PageSize = condition.PageSize;

            resultData.ListData = db49_Article.NUP_NEWS_CARD_VOD_SELECT(SearchGubun, SearchText, StartDate, EndDate, Page, PageSize).ToList();

            return resultData;
        }


        /// <summary>
        /// 부동산&연예.스포츠 뉴스 리스트
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns>ListModel<NUP_NEWS_LAND_ENTSPO_SELECT_Result></returns>
        public ListModel<NUP_NEWS_LAND_ENTSPO_SELECT_Result> GetNewsLandEntSpoList(NewsCenterCondition condition)
        {
            ListModel<NUP_NEWS_LAND_ENTSPO_SELECT_Result> resultData = new ListModel<NUP_NEWS_LAND_ENTSPO_SELECT_Result>();

            string SearchGubun = condition.SearchGubun;
            string SearchSection = condition.SearchSection;
            string SearchText = condition.SearchText;
            string StartDate = condition.StartDate;
            string EndDate = condition.EndDate;
            int Page = condition.Page;
            int PageSize = condition.PageSize;

            resultData.ListData = db49_Article.NUP_NEWS_LAND_ENTSPO_SELECT(SearchGubun, SearchSection, SearchText, StartDate, EndDate, Page, PageSize).ToList();

            return resultData;
        }


        /// <summary>
        /// 뉴스 관련 뉴스 설정 정보 리스트
        /// </summary>
        /// <returns>ListModel<NTB_ARTICLE_RELATION_CONFIG></returns>
        public ListModel<NTB_ARTICLE_RELATION_CONFIG> GetNewsRelationConfigList()
        {
            ListModel<NTB_ARTICLE_RELATION_CONFIG> resultData = new ListModel<NTB_ARTICLE_RELATION_CONFIG>();

            resultData.ListData = db49_Article.NTB_ARTICLE_RELATION_CONFIG.OrderBy(p => p.SEQ).ToList();

            return resultData;
        }


        /// <summary>
        /// 관련 뉴스 설정
        /// </summary>
        /// <param name="saveInfo">설정할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>결과값</returns>
        public bool SetNewsRelationConfigSave(NTB_ARTICLE_RELATION_CONFIG saveInfo, LoginUser loginUser)
        {
            bool isSuccess = false;

            try
            {
                //설정할 순위 정보
                var targetArticle = db49_Article.NTB_ARTICLE_RELATION_CONFIG.SingleOrDefault(p => p.SHOW_NUM == saveInfo.SHOW_NUM);

                //설정할 기사 등록 정보.
                var thisArticle = db49_Article.NTB_ARTICLE_RELATION_CONFIG.SingleOrDefault(p => p.ARTICLE_ID == saveInfo.ARTICLE_ID);

                //설정할 SHOW_NUM 없다면..
                if (targetArticle == null)
                {
                    //Insert
                    if (thisArticle == null)
                    {
                        
                        NTB_ARTICLE_RELATION_CONFIG model = new NTB_ARTICLE_RELATION_CONFIG();

                        model.SHOW_NUM       = saveInfo.SHOW_NUM;
                        model.AUTO_MANUAL    = saveInfo.AUTO_MANUAL;
                        model.ARTICLE_ID     = saveInfo.ARTICLE_ID;
                        model.SHOW_CONDITION = saveInfo.SHOW_CONDITION;
                        model.DEPT_CODE      = saveInfo.DEPT_CODE;
                        model.ARTICLE_TITLE  = saveInfo.ARTICLE_TITLE;

                        model.REG_ID   = loginUser.LoginId;
                        model.REG_DATE = DateTime.Now;
                        model.MOD_ID   = loginUser.LoginId;
                        model.MOD_DATE = DateTime.Now;

                        db49_Article.NTB_ARTICLE_RELATION_CONFIG.Add(model);
                    }
                    else
                    {
                        thisArticle.SHOW_NUM       = saveInfo.SHOW_NUM;
                        thisArticle.AUTO_MANUAL    = saveInfo.AUTO_MANUAL;
                        thisArticle.SHOW_CONDITION = saveInfo.SHOW_CONDITION;
                        thisArticle.DEPT_CODE      = saveInfo.DEPT_CODE;
                        thisArticle.ARTICLE_TITLE  = saveInfo.ARTICLE_TITLE;

                        thisArticle.MOD_ID = loginUser.LoginId;
                        thisArticle.MOD_DATE = DateTime.Now;
                    }
                }
                else
                {
                    //설정할 기사가 설정되어 있지 않을때.. 이전 설정 기사 삭제 후 INSERT
                    if (thisArticle == null)
                    {
                        //삭제
                        db49_Article.NTB_ARTICLE_RELATION_CONFIG.Remove(targetArticle);

                        //Insert
                        NTB_ARTICLE_RELATION_CONFIG model = new NTB_ARTICLE_RELATION_CONFIG();

                        model.SHOW_NUM       = saveInfo.SHOW_NUM;
                        model.AUTO_MANUAL    = saveInfo.AUTO_MANUAL;
                        model.ARTICLE_ID     = saveInfo.ARTICLE_ID;
                        model.SHOW_CONDITION = saveInfo.SHOW_CONDITION;
                        model.DEPT_CODE      = saveInfo.DEPT_CODE;
                        model.ARTICLE_TITLE  = saveInfo.ARTICLE_TITLE;

                        model.REG_ID   = loginUser.LoginId;
                        model.REG_DATE = DateTime.Now;
                        model.MOD_ID   = loginUser.LoginId;
                        model.MOD_DATE = DateTime.Now;

                        db49_Article.NTB_ARTICLE_RELATION_CONFIG.Add(model);
                    }
                    //Update
                    else
                    {
                        //선택된 번호의 설정되어 있는 기사가 있다면 선택한 기사의 설정되어 있는 정보로 변경..
                        targetArticle.SHOW_NUM       = thisArticle.SHOW_NUM;
                        targetArticle.AUTO_MANUAL    = thisArticle.AUTO_MANUAL;
                        targetArticle.ARTICLE_ID     = thisArticle.ARTICLE_ID;
                        targetArticle.SHOW_CONDITION = thisArticle.SHOW_CONDITION;
                        targetArticle.DEPT_CODE      = thisArticle.DEPT_CODE;
                        targetArticle.ARTICLE_TITLE  = thisArticle.ARTICLE_TITLE;

                        targetArticle.MOD_ID         = loginUser.LoginId;
                        targetArticle.MOD_DATE       = DateTime.Now;

                        //설정할 기사 선택한 설정번호 및 정보 변경
                        thisArticle.SHOW_NUM       = saveInfo.SHOW_NUM;
                        thisArticle.AUTO_MANUAL    = saveInfo.AUTO_MANUAL;
                        thisArticle.ARTICLE_ID     = saveInfo.ARTICLE_ID;
                        thisArticle.SHOW_CONDITION = saveInfo.SHOW_CONDITION;
                        thisArticle.DEPT_CODE      = saveInfo.DEPT_CODE;
                        thisArticle.ARTICLE_TITLE  = saveInfo.ARTICLE_TITLE;

                        thisArticle.MOD_ID         = loginUser.LoginId;
                        thisArticle.MOD_DATE       = DateTime.Now;
                    }
                }

                db49_Article.SaveChanges();

                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }


        /// <summary>
        /// 관련 뉴스 설정 수정
        /// </summary>
        /// <param name="saveInfo">수정할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>결과값</returns>
        public bool SetNewsRelationConfigUpdate(NTB_ARTICLE_RELATION_CONFIG articleRealtionConfig, LoginUser loginUser)
        {
            bool isSuccess = false;

            try
            {
                //수정할 정보
                var targetArticle = db49_Article.NTB_ARTICLE_RELATION_CONFIG.SingleOrDefault(p => p.SHOW_NUM == articleRealtionConfig.SHOW_NUM);

                targetArticle.AUTO_MANUAL    = articleRealtionConfig.AUTO_MANUAL;
                targetArticle.SHOW_CONDITION = articleRealtionConfig.SHOW_CONDITION;
                targetArticle.DEPT_CODE      = articleRealtionConfig.DEPT_CODE;
                targetArticle.ARTICLE_TITLE  = articleRealtionConfig.ARTICLE_TITLE;
                targetArticle.MOD_ID         = loginUser.LoginId;
                targetArticle.MOD_DATE       = DateTime.Now;

                db49_Article.SaveChanges();

                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }


        /// <summary>
        /// 뉴스 노출설정 정보
        /// </summary>
        /// <returns>NTB_ARTICLE_SHOW_CONFIG</returns>
        public NTB_ARTICLE_SHOW_CONFIG GetNewsShowConfig(string ShowCode)
        {
            NTB_ARTICLE_SHOW_CONFIG SingleRow = db49_Article.NTB_ARTICLE_SHOW_CONFIG.SingleOrDefault(p => p.SHOW_CODE == ShowCode);

            return SingleRow;
        }


        /// <summary>
        /// 뉴스 노출설정(자동/수동) UPDATE
        /// </summary>
        /// <returns>처리결과</returns>
        public bool SetNewsShowConfig(NTB_ARTICLE_SHOW_CONFIG updateInfo, LoginUser loginUser)
        {
            bool isSuccess = false;
            try
            {
                NTB_ARTICLE_SHOW_CONFIG SingleRow = db49_Article.NTB_ARTICLE_SHOW_CONFIG.SingleOrDefault(p => p.SHOW_CODE == updateInfo.SHOW_CODE);

                if (SingleRow == null)
                {
                    NTB_ARTICLE_SHOW_CONFIG model = new NTB_ARTICLE_SHOW_CONFIG();

                    model.SHOW_CODE = updateInfo.SHOW_CODE;
                    model.AUTO_MANUAL = (updateInfo.AUTO_MANUAL == null ? "AUTO" : updateInfo.AUTO_MANUAL);
                    model.DEPT_CODE = (updateInfo.DEPT_CODE == null ? "" : updateInfo.DEPT_CODE);
                    model.SHOW_CONDITION = (updateInfo.SHOW_CONDITION == null ? "" : updateInfo.SHOW_CONDITION);
                    model.ACTIVE_YN = "Y";
                    model.DEL_YN = "N";

                    model.REG_ID = loginUser.LoginId;
                    model.REG_DATE = DateTime.Now;
                    model.MOD_ID = loginUser.LoginId;
                    model.MOD_DATE = DateTime.Now;

                    db49_Article.NTB_ARTICLE_SHOW_CONFIG.Add(model);
                }
                else
                {
                    SingleRow.AUTO_MANUAL = updateInfo.AUTO_MANUAL;
                    SingleRow.DEPT_CODE = updateInfo.DEPT_CODE;
                    SingleRow.SHOW_CONDITION = updateInfo.SHOW_CONDITION;
                    SingleRow.MOD_ID = loginUser.LoginId;
                    SingleRow.MOD_DATE = DateTime.Now;
                }

                db49_Article.SaveChanges();

                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }


        /// <summary>
        /// 뉴스 활성설정(ACTIVE_YN) UPDATE
        /// </summary>
        /// <returns>처리결과</returns>
        public bool SetNewsShowActiveConfig(NTB_ARTICLE_SHOW_CONFIG updateInfo, LoginUser loginUser)
        {
            bool isSuccess = false;
            try
            {
                NTB_ARTICLE_SHOW_CONFIG SingleRow = db49_Article.NTB_ARTICLE_SHOW_CONFIG.SingleOrDefault(p => p.SHOW_CODE == updateInfo.SHOW_CODE);

                if (SingleRow == null)
                {
                    NTB_ARTICLE_SHOW_CONFIG model = new NTB_ARTICLE_SHOW_CONFIG();

                    model.SHOW_CODE = updateInfo.SHOW_CODE;
                    model.AUTO_MANUAL = (updateInfo.AUTO_MANUAL == null ? "AUTO" : updateInfo.AUTO_MANUAL);
                    model.DEPT_CODE = (updateInfo.DEPT_CODE == null ? "" : updateInfo.DEPT_CODE);
                    model.SHOW_CONDITION = (updateInfo.SHOW_CONDITION == null ? "" : updateInfo.SHOW_CONDITION);
                    model.ACTIVE_YN = updateInfo.ACTIVE_YN;
                    model.DEL_YN = "N";

                    model.REG_ID = loginUser.LoginId;
                    model.REG_DATE = DateTime.Now;
                    model.MOD_ID = loginUser.LoginId;
                    model.MOD_DATE = DateTime.Now;

                    db49_Article.NTB_ARTICLE_SHOW_CONFIG.Add(model);
                }
                else
                {
                    SingleRow.ACTIVE_YN = updateInfo.ACTIVE_YN;
                    SingleRow.MOD_ID = loginUser.LoginId;
                    SingleRow.MOD_DATE = DateTime.Now;
                }

                db49_Article.SaveChanges();

                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }


        /// <summary>
        /// 뉴스(기사) 노출순서 설정
        /// </summary>
        /// <param name="saveInfo">저장할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>결과값</returns>
        public bool SetNewsShowNumSave(List<NTB_ARTICLE_SHOW_NUM> saveInfo, LoginUser loginUser)
        {
            bool isSuccess = false;

            if (saveInfo.Count > 0)
            {
                try
                {
                    foreach (NTB_ARTICLE_SHOW_NUM item in saveInfo)
                    {
                        var targetArticle = db49_Article.NTB_ARTICLE_SHOW_NUM.SingleOrDefault(p => p.SHOW_CODE == item.SHOW_CODE && p.SHOW_NUM == item.SHOW_NUM);
                        //var ArticleInfo = db49_Article.tblArticleList.SingleOrDefault(p => p.ArtID == item.ARTID);

                        //SHOW_NUM != 0
                        if(!item.SHOW_NUM.Equals(0))
                        {
                            if (targetArticle == null)
                            {
                                //변경할 자신 
                                var thisArticle = db49_Article.NTB_ARTICLE_SHOW_NUM.SingleOrDefault(p => p.SHOW_CODE == item.SHOW_CODE && p.ARTICLE_ID == item.ARTICLE_ID);

                                if (thisArticle == null)
                                {
                                    //Insert
                                    NTB_ARTICLE_SHOW_NUM model = new NTB_ARTICLE_SHOW_NUM();

                                    model.SHOW_CODE = item.SHOW_CODE;
                                    model.SHOW_NUM = item.SHOW_NUM;
                                    model.ARTICLE_ID = item.ARTICLE_ID;

                                    model.REG_ID = loginUser.LoginId;
                                    model.REG_DATE = DateTime.Now;
                                    model.MOD_ID = loginUser.LoginId;
                                    model.MOD_DATE = DateTime.Now;

                                    db49_Article.NTB_ARTICLE_SHOW_NUM.Add(model);
                                }
                                else
                                {
                                    thisArticle.SHOW_NUM = item.SHOW_NUM;
                                    thisArticle.MOD_ID = loginUser.LoginId;
                                    thisArticle.MOD_DATE = DateTime.Now;
                                }
                            }
                            else
                            {
                                //변경할 자신
                                var thisArticle = db49_Article.NTB_ARTICLE_SHOW_NUM.SingleOrDefault(p => p.SHOW_CODE == item.SHOW_CODE && p.ARTICLE_ID == item.ARTICLE_ID);

                                //설정할 번호의 기사가 설정되어 있지 않을때.. 이전 설정 기사 삭제 후 INSERT
                                if (thisArticle == null)
                                {
                                    //삭제
                                    db49_Article.NTB_ARTICLE_SHOW_NUM.Remove(targetArticle);

                                    //Insert
                                    NTB_ARTICLE_SHOW_NUM model = new NTB_ARTICLE_SHOW_NUM();

                                    model.SHOW_CODE = item.SHOW_CODE;
                                    model.SHOW_NUM = item.SHOW_NUM;
                                    model.ARTICLE_ID = item.ARTICLE_ID;

                                    model.REG_ID = loginUser.LoginId;
                                    model.REG_DATE = DateTime.Now;
                                    model.MOD_ID = loginUser.LoginId;
                                    model.MOD_DATE = DateTime.Now;

                                    db49_Article.NTB_ARTICLE_SHOW_NUM.Add(model);
                                }
                                //Update
                                else
                                {
                                    //선택된 번호의 기사가 있다면 선택한 기사의 설정되어 있는 번호로 변경..
                                    targetArticle.SHOW_NUM = thisArticle.SHOW_NUM;
                                    targetArticle.MOD_ID = loginUser.LoginId;
                                    targetArticle.MOD_DATE = DateTime.Now;

                                    //선택한 설정번호로 변경
                                    thisArticle.SHOW_NUM = item.SHOW_NUM;
                                    thisArticle.MOD_ID = loginUser.LoginId;
                                    thisArticle.MOD_DATE = DateTime.Now;
                                }
                            }
                        }

                        //BOLD YN
                        //NTB_ARTICLE_CONFIG Insert, Update
                        var articleConfig = db49_Article.NTB_ARTICLE_CONFIG.SingleOrDefault(p => p.ARTICLE_ID == item.ARTICLE_ID);
                        if (articleConfig == null)
                        {
                            //Insert
                            NTB_ARTICLE_CONFIG model = new NTB_ARTICLE_CONFIG();

                            model.ARTICLE_ID = item.ARTICLE_ID;
                            model.LIST_YN = "N";
                            model.BOLD_YN = String.IsNullOrEmpty(item.BOLD_YN) ? "N" : item.BOLD_YN;
                            model.REG_ID = loginUser.LoginId;
                            model.REG_DATE = DateTime.Now;
                            model.MOD_ID = loginUser.LoginId;
                            model.MOD_DATE = DateTime.Now;

                            db49_Article.NTB_ARTICLE_CONFIG.Add(model);
                        }
                        else
                        {
                            //Update
                            articleConfig.BOLD_YN = String.IsNullOrEmpty(item.BOLD_YN) ? "N" : item.BOLD_YN;
                            articleConfig.MOD_ID = loginUser.LoginId;
                            articleConfig.MOD_DATE = DateTime.Now;
                        }

                        db49_Article.SaveChanges();
                    }                    

                    isSuccess = true;
                }
                catch (Exception e)
                {
                    isSuccess = false;
                }
            }

            return isSuccess;
        }


        #region 오피니언
        /// <summary>
        /// 오피니언 리스트
        /// </summary>
        /// <param name="condition">검색 조건</param>
        /// <returns>ListModel<tblPlanArticle></returns>
        public ListModel<tblPlanArticle> GetOpinionList(NewsCenterCondition condition)
        {
            ListModel<tblPlanArticle> resultData = new ListModel<tblPlanArticle>();

            var list = db49_Article.tblPlanArticle.AsEnumerable().Where(p => p.DEL_YN != null && p.DEL_YN.Equals("N") && p.CLASS != null && p.CLASS.Equals(condition.SearchGubun));

            var listIng = list.AsEnumerable().Where(p => (DateTime.Parse(p.VW_FROM) <= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))) && (String.IsNullOrEmpty(p.VW_TO) || DateTime.Parse(p.VW_TO) >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))) );

            //검색조건(노출여부)
            if (!String.IsNullOrEmpty(condition.SearchViewYN))
            {
                list = list.Where(p => p.VIEW_FLAG.Equals(condition.SearchViewYN));
            }

            //검색조건(진행여부)
            if (!String.IsNullOrEmpty(condition.SearchOnOff))
            {
                //진행중
                if (condition.SearchOnOff.Equals("ON"))
                {
                    list = list.Where(p => (DateTime.Parse(p.VW_FROM) <= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))) && (String.IsNullOrEmpty(p.VW_TO) || DateTime.Parse(p.VW_TO) >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"))) );
                }
                //종료
                else if (condition.SearchOnOff.Equals("OFF"))
                {
                    list = list.Where(p => (!String.IsNullOrEmpty(p.VW_TO)) && DateTime.Parse(p.VW_TO) < DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
                }
            }

            //검색조건(일자)
            if (!String.IsNullOrEmpty(condition.StartDate) || !String.IsNullOrEmpty(condition.EndDate))
            {
                //등록일
                if (!String.IsNullOrEmpty(condition.StartDate) && String.IsNullOrEmpty(condition.EndDate))
                {
                    condition.EndDate = DateTime.Parse(condition.StartDate).AddDays(1).ToString("yyyy-MM-dd");

                    //list = list.Where(p => (p.REG_DATE != null && p.REG_DATE.Value.ToString("yyyy-MM-dd").Equals(condition.StartDate)));
                    list = list.Where(p => (p.REG_DATE != null && p.REG_DATE >= DateTime.Parse(condition.StartDate) && p.REG_DATE <= DateTime.Parse(condition.EndDate)));
                }
                else if (!String.IsNullOrEmpty(condition.StartDate) && !String.IsNullOrEmpty(condition.EndDate))
                {
                    list = list.Where(p => (p.REG_DATE != null && p.REG_DATE >= DateTime.Parse(condition.StartDate) && p.REG_DATE <= DateTime.Parse(condition.EndDate)));
                }
            }

            //검색조건(입력[제목])
            if (!String.IsNullOrEmpty(condition.SearchText))
            {
                list = list.Where(p => p.TITLE.Contains(condition.SearchText));
            }

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1 )
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 30;
                }
                //list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
                list = list.OrderByDescending(a => a.REG_DATE).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();            
            resultData.AddInfoInt1 = listIng.Count();

            return resultData;
        }


        /// <summary>
        /// 오피니언 정보
        /// </summary>
        /// <param name="SEQ">일렬번호</param>
        /// <returns>tblPlanArticle</returns>
        public tblPlanArticle GetOpinionInfo(int SEQ)
        {
            tblPlanArticle SingleRow = db49_Article.tblPlanArticle.SingleOrDefault(p => p.SEQ == SEQ);

            return SingleRow;
        }

        /// <summary>
        /// 오피니언 등록,수정
        /// </summary>
        /// <param name="tblPlanArticleInfo">오피니언 정보</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        public bool SetOpinionInfo(tblPlanArticle tblPlanArticleInfo, LoginUser loginUser)
        {
            bool isSuccess = false;
            try
            {
                //Insert
                if (tblPlanArticleInfo.SEQ <= 0)
                {
                    tblPlanArticleInfo.DEL_YN = "N";
                    tblPlanArticleInfo.REG_ID = loginUser.LoginId;
                    tblPlanArticleInfo.REG_DATE = DateTime.Now;
                    tblPlanArticleInfo.MOD_ID = loginUser.LoginId;
                    tblPlanArticleInfo.UP_DATE = DateTime.Now;

                    db49_Article.tblPlanArticle.Add(tblPlanArticleInfo);
                }
                //Upate
                else
                {

                    tblPlanArticle tblPlanArticleData = GetOpinionInfo(tblPlanArticleInfo.SEQ);

                    tblPlanArticleData.TITLE = tblPlanArticleInfo.TITLE;
                    tblPlanArticleData.EXTRACTION_TEXT = tblPlanArticleInfo.EXTRACTION_TEXT;
                    tblPlanArticleData.REMRK = tblPlanArticleInfo.REMRK;
                    tblPlanArticleData.COLUMNLIST_NAME = tblPlanArticleInfo.COLUMNLIST_NAME;
                    tblPlanArticleData.VW_FROM = tblPlanArticleInfo.VW_FROM;
                    tblPlanArticleData.VW_TO = tblPlanArticleInfo.VW_TO;
                    tblPlanArticleData.VIEW_FLAG = tblPlanArticleInfo.VIEW_FLAG;
                    tblPlanArticleData.MOD_ID = loginUser.LoginId;
                    tblPlanArticleData.UP_DATE = DateTime.Now;

                    //목록(허브) 이미지
                    if (!string.IsNullOrEmpty(tblPlanArticleInfo.IMG_MAIN))
                    {
                        tblPlanArticleData.IMG_MAIN = tblPlanArticleInfo.IMG_MAIN;
                    }

                    //프로필 이미지
                    if (!string.IsNullOrEmpty(tblPlanArticleInfo.IMG_HOTISSUE))
                    {
                        tblPlanArticleData.IMG_HOTISSUE = tblPlanArticleInfo.IMG_HOTISSUE;
                    }

                    //TV 메인 포커스 이미지
                    if (!string.IsNullOrEmpty(tblPlanArticleInfo.IMG_LIST))
                    {
                        tblPlanArticleData.IMG_LIST = tblPlanArticleInfo.IMG_LIST;
                    }

                    //공통 상단 타이틀 이미지
                    if (!string.IsNullOrEmpty(tblPlanArticleInfo.IMG_BANN))
                    {
                        tblPlanArticleData.IMG_BANN = tblPlanArticleInfo.IMG_BANN;
                    }
                }

                db49_Article.SaveChanges();

                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
                WowLog.Write(e.Message);
                
            }

            return isSuccess;
        }

        /// <summary>
        /// 오피니언 삭제
        /// </summary>
        /// <param name="deleteList">SEQ List</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        public bool SetOpinionDelete(int[] deleteList, LoginUser loginUser)
        {
            bool isSuccess = false;
            try
            {
                if (deleteList != null)
                {
                    foreach (var SEQ in deleteList)
                    {
                        tblPlanArticle SingleRow = db49_Article.tblPlanArticle.SingleOrDefault(p => p.SEQ == SEQ);

                        SingleRow.MOD_ID = loginUser.LoginId;
                        SingleRow.UP_DATE = DateTime.Now;
                        SingleRow.DEL_YN = "Y";
                    }

                    db49_Article.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
        #endregion

        #region 기사 뷰페이지 관리

        /// <summary>
        /// 기사 뷰페이지 관리 리스트
        /// </summary>
        /// <returns>ListModel<NTB_ARTICLE_VIEWPAGE_MANAGE></returns>
        public ListModel<NTB_ARTICLE_VIEWPAGE_MANAGE> GetNewsViewPageManageList()
        {
            ListModel<NTB_ARTICLE_VIEWPAGE_MANAGE> resultData = new ListModel<NTB_ARTICLE_VIEWPAGE_MANAGE>();

            var newsMenuList = db49_wowtv.NTB_MENU.Where(p => p.DEL_YN.Equals("N") 
                                    && p.ACTIVE_YN.Equals("Y") 
                                    && ((p.UP_MENU_SEQ.Equals(458) || p.UP_MENU_SEQ.Equals(594) || p.UP_MENU_SEQ.Equals(595) || p.UP_MENU_SEQ.Equals(596))
                                        || (p.UP_MENU_SEQ.Equals(787) || p.UP_MENU_SEQ.Equals(1449) || p.UP_MENU_SEQ.Equals(1443) || p.UP_MENU_SEQ.Equals(1452)))
                                    && p.DEPTH.Equals(2) && !p.MENU_SEQ.Equals(592) && !p.MENU_SEQ.Equals(555)
                                    && !p.MENU_NAME.Equals("홈") && !p.MENU_SEQ.Equals(1436)
                                ).AsQueryable().ToList();

            var resultList = from Menu in newsMenuList.OrderBy(o => o.UP_MENU_SEQ).ThenBy(o => o.SORD_ORDER)
                             join Manage in db49_Article.NTB_ARTICLE_VIEWPAGE_MANAGE on Menu.MENU_SEQ equals Manage.MENU_SEQ into ManageList
                             from resList in ManageList.DefaultIfEmpty()
                             select new NTB_ARTICLE_VIEWPAGE_MANAGE()
                             {
                               MENU_SEQ  = Menu.MENU_SEQ,
                               MENU_NAME = Menu.MENU_NAME,
                               CHANNEL_CODE = Menu.CHANNEL_CODE,
                               LIKE_YN   = ( resList == null ? string.Empty : resList.LIKE_YN),
                               RECOM_YN  = (resList == null ? string.Empty : resList.RECOM_YN),
                               SEE_YN    = (resList == null ? string.Empty : resList.SEE_YN),
                               SNS_PR_YN = (resList == null ? string.Empty : resList.SNS_PR_YN)
                             };          


            resultData.ListData = resultList.ToList();

            return resultData;
        }

        /// <summary>
        /// 기사 뷰페이지 관리 등록,수정
        /// </summary>
        /// <param name="viewPageManageInfo">설정 정보</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        public bool SetNewsViewPageManage(NTB_ARTICLE_VIEWPAGE_MANAGE viewPageManageInfo, LoginUser loginUser) 
        {
            bool isSuccess = false;
            try
            {
                var viewPageManageData = db49_Article.NTB_ARTICLE_VIEWPAGE_MANAGE.SingleOrDefault(p => p.MENU_SEQ.Equals(viewPageManageInfo.MENU_SEQ));

                //Insert
                if (viewPageManageData == null)
                {
                    viewPageManageInfo.REG_ID   = loginUser.LoginId;
                    viewPageManageInfo.REG_DATE = DateTime.Now;
                    viewPageManageInfo.MOD_ID   = loginUser.LoginId;
                    viewPageManageInfo.MOD_DATE = DateTime.Now;

                    db49_Article.NTB_ARTICLE_VIEWPAGE_MANAGE.Add(viewPageManageInfo);
                }
                //Upate
                else
                {
                    viewPageManageData.LIKE_YN   = viewPageManageInfo.LIKE_YN;
                    viewPageManageData.RECOM_YN  = viewPageManageInfo.RECOM_YN;
                    viewPageManageData.SEE_YN    = viewPageManageInfo.SEE_YN;
                    viewPageManageData.SNS_PR_YN = viewPageManageInfo.SNS_PR_YN;
                    viewPageManageData.MOD_ID    = loginUser.LoginId;
                    viewPageManageData.MOD_DATE  = DateTime.Now;
                }

                db49_Article.SaveChanges();

                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
                WowLog.Write(e.Message);

            }

            return isSuccess;
        }
        #endregion

        #region 기자 페이지 관리


        /// <summary>
        /// 기자의 최신 기사 리스트
        /// </summary>
        /// <param name="reporterId">reporterId</param>
        /// <returns>ListModel<NUP_REPORTER_RECENTLY_SELECT_Result></returns>
        public ListModel<NUP_REPORTER_RECENTLY_SELECT_Result> GetReporterRecentlySelect(string reporterId) 
        {
            ListModel<NUP_REPORTER_RECENTLY_SELECT_Result> resultData = new ListModel<NUP_REPORTER_RECENTLY_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_REPORTER_RECENTLY_SELECT(reporterId).ToList();

            return resultData;
        }

        /// <summary>
        /// 기자 페이지 관리 USER ID 등록 체크
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>결과</returns>
        public bool IsReporterManageUserIdDuplicated(string userId)
        {
            var reporterManage = db49_Article.NTB_REPORTER_MANAGE.SingleOrDefault(p => p.USER_ID.Equals(userId));

            if (reporterManage != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 기자 페이지 관리 리스트
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns>ListModel<NUP_REPORTER_MANAGE_SELECT_Result></returns>
        public ListModel<NUP_REPORTER_MANAGE_SELECT_Result> GetReporterMageList(NewsCenterCondition condition)
        {
            string SearchGubun = condition.SearchGubun;
            string SearchText = condition.SearchText;
            string StartDate = condition.StartDate;
            string EndDate = condition.EndDate;
            int Page = condition.Page;
            int PageSize = condition.PageSize;

            ListModel<NUP_REPORTER_MANAGE_SELECT_Result> resultData = new ListModel<NUP_REPORTER_MANAGE_SELECT_Result>();
            resultData.ListData = db49_Article.NUP_REPORTER_MANAGE_SELECT(SearchGubun, SearchText, StartDate, EndDate, Page, PageSize).ToList();

            return resultData;
        }

        /// <summary>
        /// 기자 페이지 관리 등록,수정
        /// </summary>
        /// <param name="reporterManageInfo">설정 정보</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        public bool SetReporterManage(NTB_REPORTER_MANAGE reporterManageInfo, LoginUser loginUser)
        {
            bool isSuccess = false;
            try
            { 
                var reporterManage = db49_Article.NTB_REPORTER_MANAGE.SingleOrDefault(p => p.REPORTER_ID.Equals(reporterManageInfo.REPORTER_ID));

                //Insert
                if (reporterManage == null)
                {
                    reporterManageInfo.REG_ID = loginUser.LoginId;
                    reporterManageInfo.REG_DATE = DateTime.Now;
                    reporterManageInfo.MOD_ID = loginUser.LoginId;
                    reporterManageInfo.MOD_DATE = DateTime.Now;

                    db49_Article.NTB_REPORTER_MANAGE.Add(reporterManageInfo);
                }
                //Upate
                else
                {
                    reporterManage.USER_ID = reporterManageInfo.USER_ID;
                    reporterManage.REPORTER_GUBUN = reporterManageInfo.REPORTER_GUBUN;
                    reporterManage.WORD_YN = reporterManageInfo.WORD_YN;
                    reporterManage.PAGE_YN = reporterManageInfo.PAGE_YN;

                    reporterManage.BYLINE_DEPT = reporterManageInfo.BYLINE_DEPT;
                    reporterManage.BYLINE_NAME = reporterManageInfo.BYLINE_NAME;
                    reporterManage.BYLINE_POSITION = reporterManageInfo.BYLINE_POSITION;
                    reporterManage.BYLINE_GROUP_ID = reporterManageInfo.BYLINE_GROUP_ID;

                    reporterManage.MOD_ID = loginUser.LoginId;
                    reporterManage.MOD_DATE = DateTime.Now;
                }

                db49_Article.SaveChanges();

                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
                WowLog.Write(e.Message);

            }

            return isSuccess;
        }
        #endregion

        /// <summary>
        /// 하나투어 여행 기사 리스트
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns></returns>
        public ListModel<ArticleClass_Hanatour> GetHanatourArticleList(NewsCenterCondition condition)
        {
            var resultData = new ListModel<ArticleClass_Hanatour>();
            var list = db49_Article.ArticleClass_Hanatour.AsQueryable();

            if (!String.IsNullOrEmpty(condition.SearchGubun))
            {
                list = list.Where(p => p.Category.Equals(condition.SearchGubun));
            }

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.CurrentIndex == 0)
                {
                    condition.CurrentIndex = 1;
                }

                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                
                list = list.OrderByDescending(o => o.ArticleDate).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();

            return resultData;
        }

        /// <summary>
        /// 하나투어 여행 기사
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public ArticleClass_Hanatour GetHanatourArticleInfo(string articleID)
        {

            ArticleClass_Hanatour SingleRow = db49_Article.ArticleClass_Hanatour.Where(p => p.ArticleID.Equals(articleID)).FirstOrDefault();

            return SingleRow;
            
        }


    }
}
