using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Broad;

namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IProgramGroupService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IProgramGroupService
    {
        [OperationContract]
        NTB_PROGRAM_GROUP GetAtProgramGroup(int programGroupSeq);

        [OperationContract]
        NTB_PROGRAM_GROUP GetAtByMainCode(string masterProgramCode);

        [OperationContract]
        ListModel<NTB_PROGRAM_GROUP> SearchListProgramGroup(BroadGroupCondition condition);

        [OperationContract]
        int SaveProgramGroup(NTB_PROGRAM_GROUP model, LoginUser loginUser);

        [OperationContract]
        void DeleteProgramGroup(int programGroupSeq);

        [OperationContract]
        void DeleteProgramGroupList(List<int> seqList);
    }
}
