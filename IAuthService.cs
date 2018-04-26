using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;

namespace Wow.Tv.Middle.WcfService.Member
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IAuthService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IAuthService
    {
        [OperationContract]
        IpinInitResult IpinInit(string returnUrl);

        [OperationContract]
        IpinCheckResult IpinCheck(string encryptedData, string parameter1, string parameter2, string parameter3);

        [OperationContract]
        SmsInitResult SMSInit(string returnUrl, string errorUrl);

        [OperationContract]
        SmsCheckResult SmsCheck(string requestNo, string encryptedData);

        [OperationContract]
        EmailAuthResult EmailAuth(int userNumber);
    }
}
