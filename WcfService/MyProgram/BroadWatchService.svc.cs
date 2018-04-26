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

using Wow.Tv.Middle.Biz.MyProgram;

namespace Wow.Tv.Middle.WcfService.MyProgram
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "BroadWatchService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 BroadWatchService.svc나 BroadWatchService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class BroadWatchService : IBroadWatchService
    {
        public tv_program GetAt(int uid)
        {
            return new BroadWatchBiz().GetAt(uid);
        }

        public ListModel<tv_program> SearchList(BroadWatchCondition condition)
        {
            return new BroadWatchBiz().SearchList(condition);
        }

        public ListModel<tv_program> SearchList2(BroadWatchCondition condition)
        {
            return new BroadWatchBiz().SearchList2(condition);
        }

        public int Save(tv_program model, NTB_ATTACH_FILE attachFile, LoginUser loginUser)
        {
            return new BroadWatchBiz().Save(model, attachFile, loginUser);
        }

        public void ExecuteInsertSp(string today, string programCode)
        {
            new BroadWatchBiz().ExecuteInsertSp(today, programCode);
        }
    }
}
