using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;

namespace Wow.Tv.Middle.WcfService.Member
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IJoinService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IJoinService
    {
        [OperationContract]
        JoinCheckIpinResult JoinCheckIpin(string encryptedData, string parameter1, string parameter2, string parameter3);

        [OperationContract]
        JoinCheckSmsResult JoinCheckSms(string requestNo, string encryptedData);

        [OperationContract]
        JoinCheckForeignResult JoinCheckForeign(string name, string ssno1, string ssno2);

        [OperationContract]
        RegistUserResult RegistUserIpin(RegistUserRequest request);

        [OperationContract]
        RegistUserResult RegistUserSms(RegistUserRequest request);

        [OperationContract]
        RegistUserResult RegistUserForeign(RegistUserRequest request);

        [OperationContract]
        RegistUserResult RegistUserCompany(RegistUserRequest request);

        [OperationContract]
        List<string> GetSido();

        [OperationContract]
        List<string> GetGugun(string sido);

        [OperationContract]
        List<tblPost> GetAddress(string searchKeyword);

        [OperationContract]
        int IsUserIdDuplicated(string userId);

        [OperationContract]
        int IsNickNameDuplicated(string nickName);

        [OperationContract]
        string MobileSendSMS(string mobile1, string mobile2, string mobile3);

        [OperationContract]
        bool CompanyExists(string ssno1, string ssno2, string ssno3);

        [OperationContract]
        EmailAuthCompleteResult EmailAuthComplete(int userNumber, string email);

        [OperationContract]
        void BillingServiceTest(string userId);

        [OperationContract]
        void EmailSendTest(string mailCode, string toName, string toEmail, string fromName, string fromEmail, string subject, string contents);
    }
}
