using System.Collections.Generic;
using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.Finance;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;
using Wow.Tv.Middle.Model.Db49.editVOD;

namespace Wow.Tv.Middle.WcfService.Finance
{
    [ServiceContract]
    public interface ICharacterStockService
    {
        [OperationContract]
        usp_GetStockPrice_Result GetCurrentPrice(string stockCode);

        [OperationContract]
        List<usp_byStockCodeGetVOD_Result> GetStockVODList(string stockCode);

        [OperationContract]
        ListModel<CharacterStockModel> GetCharaterStockList(NewsCenterCondition condition);
        
    }
}
