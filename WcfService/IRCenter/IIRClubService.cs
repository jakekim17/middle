using System.Collections.Generic;
using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.IRClub;

namespace Wow.Tv.Middle.WcfService.IRCenter
{
    [ServiceContract]
    public interface IIRClubService
    {
        [OperationContract]
        ListModel<TAB_IR_CLUB_2010> GetList(IRClubCondition condition);

        [OperationContract]
        int Save(TAB_IR_CLUB_2010 model);

        [OperationContract]
        void Delete(int[] deleteList);

        [OperationContract]
        TAB_IR_CLUB_2010 GetData(int seq);

        [OperationContract]
        List<view_AllStockCode> GetStockCode(string searchText);
    }
}
