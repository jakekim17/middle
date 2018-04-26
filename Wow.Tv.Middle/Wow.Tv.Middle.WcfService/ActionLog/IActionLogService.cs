using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.ActionLog;
using Wow.Tv.Middle.Biz.ActionLog;

namespace Wow.Tv.Middle.WcfService.ActionLog
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IActionLogService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IActionLogService
    {
        [OperationContract]
        void Create(string menuSeq, string tableKey, ActionLogBiz.ActionCode actionCode, string addInfo1, string addInfo2, LoginUser loginUser);

        [OperationContract]
        void CreateIUCheck(string menuSeq, string tableKey, string addInfo1, string addInfo2, LoginUser loginUser);



        [OperationContract]
        ListModel<NTB_ACTION_LOG> SearchList(ActionLogCondition condition);


        [OperationContract]
        System.IO.MemoryStream ExcelExport(ActionLogCondition condition);
    }
}
