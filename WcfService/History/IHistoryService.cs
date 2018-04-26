using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.History;

namespace Wow.Tv.Middle.WcfService.History
{
    [ServiceContract]
    public interface IHistoryService
    {
        [OperationContract]
        HistoryModel<HIS_CTGR> GetList(HistoryCondition codition);

        [OperationContract]
        HistoryDetail GetDetail(int seq);

        [OperationContract]
        int Save(NTB_HIS_MNG model, LoginUser loginUser);

        [OperationContract]
        void Delete(int[] seqList);

        [OperationContract]
        List<DtlCTGRHistory> SearchHistory(string SearchYear);
    }
}
