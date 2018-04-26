using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.ServiceCenter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.ServiceCenter;

namespace Wow.Tv.Middle.WcfService.ServiceCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "EventService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 EventService.svc나 EventService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class EventService : IEventService
    {
        public EventModel<EventContent> GetList(EventCondition condition)
        {
            return new EventBiz().GetList(condition);
        }

        public EventModel<EventContent> GetData(int seq)
        {
            return new EventBiz().GetData(seq);
        }

        public ListModel<Pro_wowList> GetBroadList()
        {
            return new EventBiz().GetBroadList();
        }

        public int Save(tblEvent model, LoginUser loginUser)
        {
            return new EventBiz().Save(model, loginUser);
        }

        public void Delete(int[] deleteList)
        {
            new EventBiz().Delete(deleteList);
        }

        public ListModel<EventContent> GetFrontList(EventCondition condition)
        {
            return new EventBiz().GetFrontList(condition);
        }

        public EventModel<EventContent> GetFrontDetail(EventCondition condition)
        {
            return new EventBiz().GetFrontDetail(condition);
        }

        public void ReadCountIncrease(EventCondition condition)
        {
            new EventBiz().ReadCountIncrease(condition);
        }

        public ListModel<EventContent> GetMainEventList()
        {
            return new EventBiz().GetMainEventList();
        }

        public ListModel<EventContent> GetQuickEventList()
        {
            return new EventBiz().GetQuickEventList();
        }

        public List<usp_tblEvent_select_Result> GetEventData()
        {
            return new EventBiz().GetEventData();
        }

        public ListModel<Pro_wowList> GetPartenrRandom()
        {
            return new EventBiz().GetPartenrRandom();
        }
    }
}
