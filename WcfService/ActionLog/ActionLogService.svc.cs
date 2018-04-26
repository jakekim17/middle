using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;

using Wow.Tv.Middle.Biz.ActionLog;
using System.IO;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.ActionLog;

namespace Wow.Tv.Middle.WcfService.ActionLog
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "ActionLogService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 ActionLogService.svc나 ActionLogService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class ActionLogService : IActionLogService
    {
        public void Create(string menuSeq, string tableKey, ActionLogBiz.ActionCode actionCode, string addInfo1, string addInfo2, LoginUser loginUser)
        {
            new ActionLogBiz().Create(menuSeq, tableKey, actionCode, addInfo1, addInfo2, loginUser);
        }

        public void CreateIUCheck(string menuSeq, string tableKey, string addInfo1, string addInfo2, LoginUser loginUser)
        {
            new ActionLogBiz().Create(menuSeq, tableKey, addInfo1, addInfo2, loginUser);
        }


        public ListModel<NTB_ACTION_LOG> SearchList(ActionLogCondition condition)
        {
            return new ActionLogBiz().SearchList(condition);
        }

        public MemoryStream ExcelExport(ActionLogCondition condition)
        {
            return new ActionLogBiz().ExcelExport(condition);
        }

    }
}
