using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;

namespace Wow.Tv.Middle.Biz.NewsCenter
{
    public class TravelBiz : BaseBiz
    {
        /// <summary>
        /// 여행 기사 리스트
        /// </summary>
        /// <param name="condition">검색 조건</param>
        /// <returns></returns>
        public ListModel<ArticleClass_Hanatour> TravelList(NewsCenterCondition condition)
        {
            ListModel<ArticleClass_Hanatour> resultData = new ListModel<ArticleClass_Hanatour>();

            var list = db49_Article.ArticleClass_Hanatour.AsQueryable();

            if (!String.IsNullOrEmpty(condition.SearchGubun))
            {
                list = list.Where(w => w.Category == condition.SearchGubun.ToUpper());
            }

            if (!String.IsNullOrEmpty(condition.SearchText))
            {
                list = list.Where(w => w.Writer == condition.SearchText);
            }

            if (!String.IsNullOrEmpty(condition.SearchArticleId))
            {
                list = list.Where(w => w.ArticleID != condition.SearchArticleId);
            }
            
            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(o => o.ArticleDate);

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

        /// <summary>
        /// 여행 기사 내용
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns>ArticleClass_Hanatour</returns>
        public ArticleClass_Hanatour GetTravelReadInfo(string articleId)
        {
            ArticleClass_Hanatour SingleRow = db49_Article.ArticleClass_Hanatour.Where(p => p.ArticleID == articleId).SingleOrDefault();

            return SingleRow;
        }

        /// <summary>
        /// 기사의 좋아요(여행) COUNT
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns></returns>
        public NTB_ARTICLE_LIKEIT_HANATOUR GetArticleLikeitHanatour(string articleId)
        {
            NTB_ARTICLE_LIKEIT_HANATOUR SingleRow = db49_Article.NTB_ARTICLE_LIKEIT_HANATOUR.SingleOrDefault(p => p.ARTICLE_ID == articleId);

            return SingleRow;
        }

        /// <summary>
        /// 기사의 좋아요(여행) Insert & Update
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <param name="likeitGubun">좋아요 구분</param>
        public void SetArticleLikeitHanatour(string articleId, string likeitGubun)
        {
            db49_Article.NUP_ARTICLE_LIKEIT_HANATOUR_INSERT_UPDATE(articleId, likeitGubun);
        }


        /// <summary>
        /// 이전, 다음 기사
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <param name="condition">검색 조건</param>
        /// <returns>ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result></returns>
        public ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result> GetTravelPrevNext(string articleId, NewsCenterCondition condition)
        {
            ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result> resultData = new ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result>();

            resultData.ListData = new List<NUP_NEWS_PREV_NEXT_SELECT_Result>();

            ArticleClass_Hanatour thisArticle = db49_Article.ArticleClass_Hanatour.SingleOrDefault(p => p.ArticleID == articleId);

            ArticleClass_Hanatour prevArticle = db49_Article.ArticleClass_Hanatour.Where(p => p.Category == condition.SearchGubun.ToUpper() && p.ArticleID != articleId && p.ArticleDate < thisArticle.ArticleDate).OrderByDescending(o => o.ArticleDate).Take(1).SingleOrDefault();
            if(prevArticle != null)
            {
                NUP_NEWS_PREV_NEXT_SELECT_Result articelPrevNext = new NUP_NEWS_PREV_NEXT_SELECT_Result();

                articelPrevNext.NEWS = "PREV_NEWS";
                articelPrevNext.ARTICLEID = prevArticle.ArticleID;
                articelPrevNext.TITLE = prevArticle.Title;
                articelPrevNext.ARTDATE = prevArticle.ArticleDate;
                resultData.ListData.Add(articelPrevNext);
            }

            ArticleClass_Hanatour nextArticle = db49_Article.ArticleClass_Hanatour.Where(p => p.Category == condition.SearchGubun.ToUpper() && p.ArticleID != articleId && p.ArticleDate > thisArticle.ArticleDate).OrderBy(o => o.ArticleDate).Take(1).SingleOrDefault();
            if (nextArticle != null)
            {
                NUP_NEWS_PREV_NEXT_SELECT_Result articelPrevNext = new NUP_NEWS_PREV_NEXT_SELECT_Result();

                articelPrevNext.NEWS = "NEXT_NEWS";
                articelPrevNext.ARTICLEID = nextArticle.ArticleID;
                articelPrevNext.TITLE = nextArticle.Title;
                articelPrevNext.ARTDATE = nextArticle.ArticleDate;
                resultData.ListData.Add(articelPrevNext);
            }

            return resultData;
        }



    }
}
