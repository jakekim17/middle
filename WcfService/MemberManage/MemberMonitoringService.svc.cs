using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.MemberManage;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv.MemberManage;

namespace Wow.Tv.Middle.WcfService.MemberManage
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "MemberMonitoringService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 MemberMonitoringService.svc나 MemberMonitoringService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class MemberMonitoringService : IMemberMonitoringService
    {
        public ListModel<TotalListResult> TotalList(TotalListCondition condition)
        {
            return new MemberMonitoringBiz().TotalList(condition);
        }

        public ListModel<NewRegCfmResult> NewRegCfm(NewRegCfmCondition condition)
        {
            return new MemberMonitoringBiz().NewRegCfm(condition);
        }

        public ListModel<AccAuthResult> AccAuth(AccAuthCondition condition)
        {
            return new MemberMonitoringBiz().AccAuth(condition);
        }

        public ListModel<AccLogResult> AccLog(AccLogCondition condition)
        {
            return new MemberMonitoringBiz().AccLog(condition);
        }

        public ListModel<WorkOutLogHistResult> WorkOutLogHist(WorkOutLogHistCondition condition)
        {
            return new MemberMonitoringBiz().WorkOutLogHist(condition);
        }

        public ListModel<MemInfoOpenChkResult> MemInfoOpenChk(MemInfoOpenChkCondition condition)
        {
            return new MemberMonitoringBiz().MemInfoOpenChk(condition);
        }
    }
}
