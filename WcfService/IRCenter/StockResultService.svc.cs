using System;
using Wow.Tv.Middle.Biz.IRCenter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.StockResult;

namespace Wow.Tv.Middle.WcfService.IRCenter
{
    public class StockResultService : IStockResultService
    {
        public StockResultDetail GetData(int seq)
        {
            return new StockResultBiz().GetDetail(seq);
        }

        public StockResultModel<TAB_STOCK_RESULT> GetList(StockResultCondition condition)
        {
            return new StockResultBiz().GetList(condition);
        }

        public int Save(StockResultDetail data, LoginUser loginUser)
        {
            return new StockResultBiz().Save(data, loginUser);
        }

        public void Delete(int[] deleteList)
        {
            new StockResultBiz().Delete(deleteList);
        }

        public StockResultModel<JOIN_STOCK_CONNECT> GetJoinList(StockResultCondition condition)
        {
            return new StockResultBiz().GetJoinList(condition);
        }
        public int GetMaxYear()
        {
            return new StockResultBiz().GetMaxYear();
        }
    }
}
