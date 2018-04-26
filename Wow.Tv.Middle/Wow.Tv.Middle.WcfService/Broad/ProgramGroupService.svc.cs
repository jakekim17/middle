using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Broad;

using Wow.Tv.Middle.Biz.Broad;

namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "ProgramGroupService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 ProgramGroupService.svc나 ProgramGroupService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public partial class BroadService : IProgramGroupService
    {
        public NTB_PROGRAM_GROUP GetAtProgramGroup(int programGroupSeq)
        {
            return new ProgramGroupBiz().GetAt(programGroupSeq);
        }

        public NTB_PROGRAM_GROUP GetAtByMainCode(string masterProgramCode)
        {
            return new ProgramGroupBiz().GetAtByMainCode(masterProgramCode);
        }

        public ListModel<NTB_PROGRAM_GROUP> SearchListProgramGroup(BroadGroupCondition condition)
        {
            return new ProgramGroupBiz().SearchList(condition);
        }

        public int SaveProgramGroup(NTB_PROGRAM_GROUP model, LoginUser loginUser)
        {
            return new ProgramGroupBiz().Save(model, loginUser);
        }

        public void DeleteProgramGroup(int programGroupSeq)
        {
            new ProgramGroupBiz().Delete(programGroupSeq);
        }

        public void DeleteProgramGroupList(List<int> seqList)
        {
            new ProgramGroupBiz().DeleteList(seqList);
        }
    }
}
