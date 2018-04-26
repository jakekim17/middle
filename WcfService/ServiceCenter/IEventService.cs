using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.ServiceCenter;

namespace Wow.Tv.Middle.WcfService.ServiceCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IEventService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IEventService
    {
        [OperationContract]
        EventModel<EventContent> GetList(EventCondition condition);

        [OperationContract]
        EventModel<EventContent> GetData(int seq);

        [OperationContract]
        ListModel<Pro_wowList> GetBroadList();

        [OperationContract]
        int Save(tblEvent model, LoginUser loginUser);

        [OperationContract]
        void Delete(int[] deleteList);

        [OperationContract]
        ListModel<EventContent> GetFrontList(EventCondition condition);

        [OperationContract]
        EventModel<EventContent> GetFrontDetail(EventCondition condition);

        [OperationContract]
        void ReadCountIncrease(EventCondition condition);

        [OperationContract]
        ListModel<EventContent> GetMainEventList();

        [OperationContract]
        ListModel<EventContent> GetQuickEventList();

        [OperationContract]
        List<usp_tblEvent_select_Result> GetEventData();

        [OperationContract]
        ListModel<Pro_wowList> GetPartenrRandom();
    }
}
