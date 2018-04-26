using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Pilot;
using Wow.Tv.Middle.Biz.Pilot;


namespace Wow.Tv.Middle.WcfService.Pilot
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "PilotService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 PilotService.svc나 PilotService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class PilotService : IPilotService
    {
        public ListModel<TAB_BOARD> SearchList(PilotCondition condition)
        {
            return new PilotBiz().SearchList(condition);
        }


        public TAB_BOARD GetAt(int seq)
        {
            return new PilotBiz().GetAt(seq);
        }

        public void Save(TAB_BOARD model)
        {
            new PilotBiz().Save(model);
        }

        public void Delete(int seq)
        {
            new PilotBiz().Delete(seq);
        }

    }
}
