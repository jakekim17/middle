using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db49.editVOD;
using Wow.Tv.Middle.Model.Db22.stock.Finance;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;

namespace Wow.Tv.Middle.Biz.Finance
{
    public class CharacterStockBiz :BaseBiz
    {
        //현재가 가져오기
        public usp_GetStockPrice_Result GetCurrentPrice(string stockCode)
        {
            return db22_stock.usp_GetStockPrice(stockCode).FirstOrDefault();
        }

        //VOD 가져오기
        public List<usp_byStockCodeGetVOD_Result> GetStockVODList(string stockCode)
        {
            return db49_editVOD.usp_byStockCodeGetVOD(stockCode, null).ToList();
        }

        //STOCKCODE 가져오기
        public usp_getArticleContents_NEW_Result GetStockResult(string articleId)
        {
            return db49_Article.usp_getArticleContents_NEW(articleId).FirstOrDefault();
        }

        //특징주 리스트
        public ListModel<CharacterStockModel> GetCharacterStock(NewsCenterCondition condition)
        {
            
            var list = new List<CharacterStockModel>();
            var resultData = new ListModel<CharacterStockModel>();

            string SearchSection = condition.SearchSection;
            string SearchText = condition.SearchText;
            string SearchComp = condition.SearchComp;
            string SearchWowCode = condition.SearchWowCode;
            string StartDate = condition.StartDate;
            string EndDate = condition.EndDate;
            int Page = condition.Page;
            int PageSize = condition.PageSize;

            List<NUP_NEWS_SECTION_SELECT_Result> newsList = db49_Article.NUP_NEWS_SECTION_SELECT(SearchSection, SearchText, SearchWowCode, SearchComp, StartDate, EndDate, Page, PageSize).ToList();
            
            foreach(var item in newsList)
            {
                var stockResult = GetStockResult(item.ARTICLEID);
                var model = new CharacterStockModel();

                if(stockResult.STOCKCODE != null)
                {
                    model.StockData = GetCurrentPrice(stockResult.STOCKCODE);
                }
                
                model.NewsData = item;

                list.Add(model);
            }

            resultData.ListData = list;

            return resultData;
        }

    }
    
}