using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.CommonCode;


namespace Wow.Tv.Middle.WcfService.CommonCode
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "ICommconCodeService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface ICommconCodeService
    {
        [OperationContract]
        ListModel<NTB_COMMON_CODE> SearchList(CommonCodeCondition condition);

        [OperationContract]
        NTB_COMMON_CODE GetAt(string commonCode);

        [OperationContract]
        NTB_COMMON_CODE GetAtFromValue(string upCommonCode, string codeValue1);

        [OperationContract]
        string Save(NTB_COMMON_CODE model, LoginUser loginUser);

        [OperationContract]
        void Delete(string commonCode, LoginUser loginUser);


    }
}
