using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;
using Wow.Tv.Middle.Model.Db49.Article.Opinion;

namespace Wow.Tv.Middle.Biz.Opinion
{
    public class OpinionBiz : BaseBiz
    {
        /// <summary>
        /// 오피니언 연재컬럼 (상세포함) 리스트 
        /// </summary>
        public ListModel<OpinionColumnModel> GetColumnList(OpinionCondition condition)
        {
            var resultData = new ListModel<OpinionColumnModel>();
            NewsCenterCondition newsCondition = new NewsCenterCondition();

            var planList = GetPlanList(condition);

            var list = new List<OpinionColumnModel>();


            resultData.TotalDataCount = planList.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                planList = planList.OrderByDescending(a => a.UP_DATE).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            //연재목록만큼 for를 돌림
            foreach (var item in planList)
            {
                var model = new OpinionColumnModel();
                model.SubList = null;

                //Class : S (연재컬럼), P (기획취재)
                if (condition.Class == "S")
                {
                    newsCondition.PageSize = 3;

                }
                else if (condition.Class == "P")
                {
                    newsCondition.PageSize = 5;
                }

                newsCondition.SearchSection = "OPINION";
                newsCondition.Page = 1;
                
                if (!String.IsNullOrEmpty(item.EXTRACTION_TEXT))
                {
                    model.SubList = GetDetailList(newsCondition, item.EXTRACTION_TEXT).ListData;
                }
                else
                {
                    model.SubList = new List<NUP_NEWS_SECTION_SELECT_Result>();
                }
                
                model.List = item;

                list.Add(model);

            }

            resultData.ListData = list;

            return resultData;
        }

        /// <summary>
        /// 오피니언 연재컬럼 상세 리스트 
        /// </summary>
        public ListModel<NUP_NEWS_SECTION_SELECT_Result> GetDetailList(NewsCenterCondition condition, string text)
        {
            var resultData = new ListModel<NUP_NEWS_SECTION_SELECT_Result>();

            string SearchSection = condition.SearchSection;
            string StartDate = "";
            string EndDate = "";
            string WowCode = "";
            string CompCode = "";
            int Page = condition.Page;
            int PageSize = condition.PageSize;

            text = text.Replace("&amp;", "&");
            var list = db49_Article.NUP_NEWS_SECTION_SELECT(condition.SearchSection, text, WowCode, CompCode, StartDate, EndDate, Page, PageSize).ToList();

            resultData.ListData = list;

            return resultData;
        }

        /// <summary>
        /// 오피니언 연재컬럼/기획취재 리스트 
        /// </summary>
        public IQueryable<tblPlanArticle> GetPlanList(OpinionCondition condition)
        {
            //미포함 SEQ
            var seqList = new[] { 27, 26, 25, 24, 23, 28, 29, 30, 31, 33, 34, 35, 36, 38, 39 };

            var planList = db49_Article.tblPlanArticle.Where(a => a.DEL_YN.Equals("N")).AsQueryable();

            //Class : S (연재컬럼), P (기획취재)
            if (condition.Class == "S")
            {
                planList = db49_Article.tblPlanArticle.Where(a => a.CLASS.Equals("S") && a.VIEW_FLAG.Equals("y") && !seqList.Contains(a.SEQ) && a.EXTRACTION_TEXT != null)
                            .OrderByDescending(a => a.VW_TO)
                            .OrderByDescending(a => a.VW_FROM).AsQueryable();

            }
            else if (condition.Class == "P")
            {
                planList = db49_Article.tblPlanArticle.Where(a => a.CLASS.Equals("P") && a.VIEW_FLAG.Equals("y") && a.EXTRACTION_TEXT != null)
                           .OrderByDescending(a => a.VW_TO)
                           .OrderByDescending(a => a.VW_FROM).AsQueryable();
            }


            return planList;
        }

        public tblPlanArticle ColumnBannerImg(OpinionCondition condition)
        {
            var list = GetPlanList(condition);

            var data = list.Where(a => a.SEQ.Equals(condition.Seq)).FirstOrDefault();

            return data;
        }
    }
}
