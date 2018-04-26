using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.StockSituation;

namespace Wow.Tv.Middle.WcfService.IRCenter
{
    [ServiceContract]
    public interface IStockSituationService
    {
        [OperationContract]
        StockModel<TAB_STOCK_SITUATION> GetList();

        [OperationContract]
        int Save(TAB_STOCK_SITUATION model);

        [OperationContract]
        void Delete(int seq);

        [OperationContract]
        void UpdateOrder(int seq, bool isUp);
    }
}
