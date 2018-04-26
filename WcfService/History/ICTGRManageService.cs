using System.Collections.Generic;
using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.History;

namespace Wow.Tv.Middle.WcfService.History
{
    [ServiceContract]
    public interface ICTGRManageService
    {
        [OperationContract]
        CTGRModel<NTB_CTGR> GetList();

        [OperationContract]
        int Save(NTB_CTGR model, LoginUser loginUser);

        [OperationContract]
        void Delete(int seq);

        [OperationContract]
        string GetMaxYear();

        [OperationContract]
        List<NTB_CTGR> GetCTGRList();
    }
}
