using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Member;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;

namespace Wow.Tv.Middle.WcfService.Member
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "ILoginService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        string[] EncryptCheck(string plainText);

        [OperationContract]
        tblUser LoginCheck(string userId, string password, RegistUserRequest registUserRequest);

        [OperationContract]
        tblUser LoginCheckEncrypt(string encryptedUserId, string encryptedPassword);

        [OperationContract]
        tblUser LoginCheckTest(string userId);

        [OperationContract]
        tblUser LoginCheckByKakao(long kakaoId, string kakaoEmail, string kakaoNickname);

        [OperationContract]
        tblUser LoginCheckByFacebook(long facebookId, string facebookEmail, string facebookName);

        [OperationContract]
        tblUser LoginCheckByNaver(long naverId, string naverEmail, string naverName);

        [OperationContract]
        AuthDormancyCheckMemberResult AuthDormancyCheckMember(string userId, string mobile1, string mobile2, string mobile3);

        [OperationContract]
        IpinInitResult AuthDormancyIpinInit(string returnUrl);

        [OperationContract]
        AuthDormancyCheckIpinResult AuthDormancyIpinCheck(string encryptedData, string parameter1, string parameter2, string parameter3);

        [OperationContract]
        SmsInitResult AuthDormancySmsInit(string returnUrl, string errorUrl);

        [OperationContract]
        AuthDormancyCheckSmsResult AuthDormancySmsCheck(string requestNo, string encryptedData);

        [OperationContract]
        string MobileSendSms(string mobile1, string mobile2, string mobile3);

        [OperationContract]
        SmsInitResult FindIdSmsInit(string returnUrl, string errorUrl);

        [OperationContract]
        FindIdResult FindIdCheckSms(string requestNo, string encryptedData);

        [OperationContract]
        IpinInitResult FindIdIpinInit(string returnUrl);

        [OperationContract]
        FindIdResult FindIdCheckIpin(string encryptedData, string parameter1, string parameter2, string parameter3);

        [OperationContract]
        FindIdResult FindIdCheckByMobileInfo(string name, string mobile1, string mobile2, string mobile3);

        [OperationContract]
        FindIdResult FindIdCheckBySSNoInfo(string name, string ssno, bool isCompany);

        [OperationContract]
        SmsInitResult FindPasswordSmsInit(string returnUrl, string errorUrl);

        [OperationContract]
        FindPasswordResult FindPasswordCheckSms(string userId, string requestNo, string encryptedData);

        [OperationContract]
        IpinInitResult FindPasswordIpinInit(string returnUrl);

        [OperationContract]
        FindPasswordResult FindPasswordCheckIpin(string userId, string encryptedData, string parameter1, string parameter2, string parameter3);

        [OperationContract]
        FindPasswordResult FindPasswordCheckByMobileInfo(string userId, string name, string mobile1, string mobile2, string mobile3);

        [OperationContract]
        SetPasswordResult PasswordInitialize(string userId);

        [OperationContract]
        SetPasswordResult SendTempPasswordToEmail(string userId);

        [OperationContract]
        SetPasswordResult ModifyPassword(string userId, string password);

        [OperationContract]
        void LoginLog(int userNumber, string ip);

        [OperationContract]
        void DomainLoginLog(int? userNumber, string userId, string webType, string webFrom, string webServerName, string loginSite, DateTime loginDate, string clientIp, string requestUrl);
    }
}
