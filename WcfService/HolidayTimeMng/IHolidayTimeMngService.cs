using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.Admin;

namespace Wow.Tv.Middle.WcfService.HolidayTimeMng
{
    [ServiceContract]
    public interface IHolidayTimeMngService
    {
        [OperationContract]
        ListModel<NTB_SISE_TIME> GetList(HolidayMngCondition condition);

        [OperationContract]
        bool DateConfirm(NTB_SISE_TIME model);

        [OperationContract]
        int SaveTime(NTB_SISE_TIME model, LoginUser loginUser);

        [OperationContract]
        int SaveMaster(NTB_SISE_MASTER model, LoginUser loginUser);

        [OperationContract]
        void Delete(int seq, LoginUser loginUser);

        [OperationContract]
        NTB_SISE_TIME GetTimeData(int seq);

        [OperationContract]
        NTB_SISE_MASTER GetMasterData();
    }
}
