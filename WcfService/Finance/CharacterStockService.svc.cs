using System;
using System.Collections.Generic;
using Wow.Tv.Middle.Biz.Finance;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.Finance;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;
using Wow.Tv.Middle.Model.Db49.editVOD;

namespace Wow.Tv.Middle.WcfService.Finance
{
    public class CharacterStockService : ICharacterStockService
    {
        public usp_GetStockPrice_Result GetCurrentPrice(string stockCode)
        {
            return new CharacterStockBiz().GetCurrentPrice(stockCode);
        }

        public List<usp_byStockCodeGetVOD_Result> GetStockVODList(string stockCode)
        {
            return new CharacterStockBiz().GetStockVODList(stockCode);
        }


        public ListModel<CharacterStockModel> GetCharaterStockList(NewsCenterCondition condition)
        {
            return new CharacterStockBiz().GetCharacterStock(condition);
        }
    }
}
