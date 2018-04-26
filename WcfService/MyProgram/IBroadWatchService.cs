using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db90.DNRS;
using Wow.Tv.Middle.Model.Db90.DNRS.NewsProgram;
using Wow.Tv.Middle.Model.Db49.wowtv;

namespace Wow.Tv.Middle.WcfService.MyProgram
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IBroadWatchService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IBroadWatchService
    {
        [OperationContract]
        tv_program GetAt(int uid);

        [OperationContract]
        ListModel<tv_program> SearchList(BroadWatchCondition condition);

        [OperationContract]
        ListModel<tv_program> SearchList2(BroadWatchCondition condition);

        [OperationContract]
        int Save(tv_program model, NTB_ATTACH_FILE attachFile, LoginUser loginUser);


        [OperationContract]
        void ExecuteInsertSp(string today, string programCode);
    }
}
