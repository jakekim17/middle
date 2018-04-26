using Wow.Tv.Middle.Biz.HolidayTimeMng;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.Admin;

namespace Wow.Tv.Middle.WcfService.HolidayTimeMng
{
    public class HolidayTimeMngService : IHolidayTimeMngService
    {
        public ListModel<NTB_SISE_TIME> GetList(HolidayMngCondition condition)
        {
            return new HolidayTimeMngBiz().GetList(condition);
        }

        public NTB_SISE_TIME GetTimeData(int seq)
        {
            return new HolidayTimeMngBiz().GetTimeData(seq);
        }

        public bool DateConfirm(NTB_SISE_TIME model)
        {
            return new HolidayTimeMngBiz().DateConfirm(model);
        }

        public int SaveMaster(NTB_SISE_MASTER model, LoginUser loginUser)
        {
            return new HolidayTimeMngBiz().SaveMaster(model, loginUser);
        }

        public int SaveTime(NTB_SISE_TIME model, LoginUser loginUser)
        {
            return new HolidayTimeMngBiz().SaveTime(model, loginUser);
        }

        public void Delete(int seq, LoginUser loginUser)
        {
            new HolidayTimeMngBiz().Delete(seq, loginUser);
        }

        public NTB_SISE_MASTER GetMasterData()
        {
            return new HolidayTimeMngBiz().GetMasterData();
        }
    }
}
