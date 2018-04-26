using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Recruit;

namespace Wow.Tv.Middle.WcfService.Recruit
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IRecruitService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IRecruitService
    {
        [OperationContract]
        List<NUP_RECRUIT_SELECT_Result> SearchList(RecruitCondition condition);

        [OperationContract]
        NUP_RECRUIT_SELECT_Result GetApplicantInfo(RecruitCondition condition);

        [OperationContract]
        void IncreaseViewCnt(int seq);

        [OperationContract]
        void SaveRecruit(tblRecruit model);
    }
}
