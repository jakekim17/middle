using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.CommonCode;
using Wow.Tv.Middle.Biz.CommonCode;

namespace Wow.Tv.Middle.WcfService.CommonCode
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "CommconCodeService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 CommconCodeService.svc나 CommconCodeService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class CommconCodeService : ICommconCodeService
    {
        public ListModel<NTB_COMMON_CODE> SearchList(CommonCodeCondition condition)
        {
            return new CommonCodeBiz().SearchList(condition);
        }


        public NTB_COMMON_CODE GetAt(string commonCode)
        {
            return new CommonCodeBiz().GetAt(commonCode);
        }
        public NTB_COMMON_CODE GetAtFromValue(string upCommonCode, string codeValue1)
        {
            return new CommonCodeBiz().GetAt(upCommonCode, codeValue1);
        }

        public string Save(NTB_COMMON_CODE model, LoginUser loginUser)
        {
            return new CommonCodeBiz().Save(model, loginUser);
        }

        public void Delete(string commonCode, LoginUser loginUser)
        {
            new CommonCodeBiz().Delete(commonCode, loginUser);
        }
    }
}
