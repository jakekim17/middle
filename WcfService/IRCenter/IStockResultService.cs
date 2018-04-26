using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.StockResult;

namespace Wow.Tv.Middle.WcfService.IRCenter
{
    [ServiceContract]
    public interface IStockResultService
    {
        [OperationContract]
        StockResultModel<TAB_STOCK_RESULT> GetList(StockResultCondition condition);

        [OperationContract]
        int Save(StockResultDetail data, LoginUser loginUser);

        [OperationContract]
        StockResultDetail GetData(int seq);

        [OperationContract]
        void Delete(int[] deleteList);

        [OperationContract]
        StockResultModel<JOIN_STOCK_CONNECT> GetJoinList(StockResultCondition condition);

        [OperationContract]
        int GetMaxYear();
    }
}
