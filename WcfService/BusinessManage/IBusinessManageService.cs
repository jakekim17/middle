using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.BusinessManage;

namespace Wow.Tv.Middle.WcfService.BusinessManage
{
    [ServiceContract]
    public interface IBusinessManageService
    {
        [OperationContract]
        ListModel<BOARD_CONT_MENU> SearchList(BusinessCondition condition);

        [OperationContract]
        BOARD_CONT_MENU GetDetail(int seq);

        [OperationContract]
        int Save(NTB_BOARD_CONTENT model, LoginUser loginUser);

        [OperationContract]
        void Delete(int[] seqList);

        [OperationContract]
        NTB_BOARD_CONTENT SearchData(int menuSeq);

        [OperationContract]
        bool SendMobileSMS(string AppType, string mobileNum);
    }
}
