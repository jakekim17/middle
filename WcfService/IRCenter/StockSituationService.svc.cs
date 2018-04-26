using System;
using System.Collections.Generic;
using Wow.Tv.Middle.Biz.IRCenter;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.StockSituation;

namespace Wow.Tv.Middle.WcfService.IRCenter
{
    public class StockSituationService : IStockSituationService
    {
        public void Delete(int seq)
        {
            new StockSituationBiz().Delete(seq);
        }

        public StockModel<TAB_STOCK_SITUATION> GetList()
        {
            return new StockSituationBiz().GetList();
        }

        public int Save(TAB_STOCK_SITUATION model)
        {
            return new StockSituationBiz().Save(model);
        }

        public void UpdateOrder(int seq, bool isUp)
        {
            new StockSituationBiz().UpdateOrder(seq, isUp);
        }
    }
}
