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
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "LoginService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 LoginService.svc나 LoginService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class LoginService : ILoginService
    {
        public string[] EncryptCheck(string plainText)
        {
            return new LoginBiz().EncryptCheck(plainText);
        }

        /// <summary>
        /// 로크인 체크
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public tblUser LoginCheck(string userId, string password, RegistUserRequest registUserRequest)
        {
            return new LoginBiz().LoginCheck(userId, password, registUserRequest);
        }

        /// <summary>
        /// 로크인 체크
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="encryptedPassword"></param>
        /// <returns></returns>
        public tblUser LoginCheckEncrypt(string encryptedUserId, string encryptedPassword)
        {
            return new LoginBiz().LoginCheckEncrypt(encryptedUserId, encryptedPassword);
        }

        /// <summary>
        /// 로크인 체크 테스트
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public tblUser LoginCheckTest(string userId)
        {
            return new LoginBiz().LoginCheckTest(userId);
        }

        /// <summary>
        /// 카카오 통한 로그인 체크
        /// </summary>
        /// <param name="kakaoId"></param>
        /// <param name="kakaoEmail"></param>
        /// <param name="kakaoNickname"></param>
        /// <returns></returns>
        public tblUser LoginCheckByKakao(long kakaoId, string kakaoEmail, string kakaoNickname)
        {
            return new LoginBiz().LoginCheckByKakao(kakaoId, kakaoEmail, kakaoNickname);
        }

        /// <summary>
        /// 페이스북 통한 로그인 체크
        /// </summary>
        /// <param name="facebookId"></param>
        /// <param name="facebookEmail"></param>
        /// <param name="facebookName"></param>
        /// <returns></returns>
        public tblUser LoginCheckByFacebook(long facebookId, string facebookEmail, string facebookName)
        {
            return new LoginBiz().LoginCheckByFacebook(facebookId, facebookEmail, facebookName);
        }

        /// <summary>
        /// 네이버 통한 로그인 체크
        /// </summary>
        /// <param name="naverId"></param>
        /// <param name="naverEmail"></param>
        /// <param name="naverName"></param>
        /// <returns></returns>
        public tblUser LoginCheckByNaver(long naverId, string naverEmail, string naverName)
        {
            return new LoginBiz().LoginCheckByNaver(naverId, naverEmail, naverName);
        }

        /// <summary>
        /// 휴대폰 통한 휴면회원 체크
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile1"></param>
        /// <param name="mobile2"></param>
        /// <param name="mobile3"></param>
        /// <returns></returns>
        public AuthDormancyCheckMemberResult AuthDormancyCheckMember(string userId, string mobile1, string mobile2, string mobile3)
        {
            return new LoginBiz().AuthDormancyCheckMember(userId, mobile1, mobile2, mobile3);
        }

        /// <summary>
        /// 아이핀인증 초기화
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IpinInitResult AuthDormancyIpinInit(string returnUrl)
        {
            IpinInitRequest request = new IpinInitRequest();
            request.IDPCode = System.Configuration.ConfigurationManager.AppSettings["IPinIIDPCODE"];
            request.IDPPassword = System.Configuration.ConfigurationManager.AppSettings["IPinIDPPWD"];
            request.CPREQuestNum = System.Configuration.ConfigurationManager.AppSettings["IPinCPREQUESTNUM"];
            request.ReturnUrl = returnUrl;

            return new AuthBiz().IpinInit(request);
        }

        /// <summary>
        /// 아이핀인증 결과 체크
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        public AuthDormancyCheckIpinResult AuthDormancyIpinCheck(string encryptedData, string parameter1, string parameter2, string parameter3)
        {
            IpinCheckRequest request = new IpinCheckRequest();
            request.IDPCode = System.Configuration.ConfigurationManager.AppSettings["IPinIIDPCODE"];
            request.IDPPassword = System.Configuration.ConfigurationManager.AppSettings["IPinIDPPWD"];
            request.CPREQuestNum = System.Configuration.ConfigurationManager.AppSettings["IPinCPREQUESTNUM"];
            request.encryptedData = encryptedData;
            request.parameter1 = parameter1;
            request.parameter2 = parameter2;
            request.parameter3 = parameter3;

            return new LoginBiz().AuthDormancyCheckIpin(request);
        }

        /// <summary>
        /// 휴대폰인증 초기화
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="errorUrl"></param>
        /// <returns></returns>
        public SmsInitResult AuthDormancySmsInit(string returnUrl, string errorUrl)
        {
            SmsInitRequest request = new SmsInitRequest();
            request.SiteCode = System.Configuration.ConfigurationManager.AppSettings["SMSSiteCode"];
            request.SitePassword = System.Configuration.ConfigurationManager.AppSettings["SMSSitePassword"];
            request.ReturnUrl = returnUrl;
            request.ErrorUrl = errorUrl;

            return new AuthBiz().SmsInit(request);
        }

        /// <summary>
        /// 휴대폰인증 결과 체크
        /// </summary>
        /// <param name="requestNo"></param>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        public AuthDormancyCheckSmsResult AuthDormancySmsCheck(string requestNo, string encryptedData)
        {
            SmsCheckRequest request = new SmsCheckRequest();
            request.SiteCode = System.Configuration.ConfigurationManager.AppSettings["SMSSiteCode"];
            request.SitePassword = System.Configuration.ConfigurationManager.AppSettings["SMSSitePassword"];
            request.RequestNo = requestNo;
            request.EncryptedData = encryptedData;

            return new LoginBiz().AuthDormancyCheckSms(request);
        }

        public string MobileSendSms(string mobile1, string mobile2, string mobile3)
        {
            return new LoginBiz().MobileSendSms(mobile1, mobile2, mobile3);
        }

        /// <summary>
        /// 휴대폰인증 초기화
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="errorUrl"></param>
        /// <returns></returns>
        public SmsInitResult FindIdSmsInit(string returnUrl, string errorUrl)
        {
            SmsInitRequest request = new SmsInitRequest();
            request.SiteCode = System.Configuration.ConfigurationManager.AppSettings["SMSSiteCode"];
            request.SitePassword = System.Configuration.ConfigurationManager.AppSettings["SMSSitePassword"];
            request.ReturnUrl = returnUrl;
            request.ErrorUrl = errorUrl;

            return new AuthBiz().SmsInit(request);
        }

        public FindIdResult FindIdCheckSms(string requestNo, string encryptedData)
        {
            SmsCheckRequest request = new SmsCheckRequest();
            request.SiteCode = System.Configuration.ConfigurationManager.AppSettings["SMSSiteCode"];
            request.SitePassword = System.Configuration.ConfigurationManager.AppSettings["SMSSitePassword"];
            request.RequestNo = requestNo;
            request.EncryptedData = encryptedData;

            return new LoginBiz().FindIdCheckSms(request);
        }

        /// <summary>
        /// 아이핀인증 초기화
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IpinInitResult FindIdIpinInit(string returnUrl)
        {
            IpinInitRequest request = new IpinInitRequest();
            request.IDPCode = System.Configuration.ConfigurationManager.AppSettings["IPinIIDPCODE"];
            request.IDPPassword = System.Configuration.ConfigurationManager.AppSettings["IPinIDPPWD"];
            request.CPREQuestNum = System.Configuration.ConfigurationManager.AppSettings["IPinCPREQUESTNUM"];
            request.ReturnUrl = returnUrl;

            return new AuthBiz().IpinInit(request);
        }

        public FindIdResult FindIdCheckIpin(string encryptedData, string parameter1, string parameter2, string parameter3)
        {
            IpinCheckRequest request = new IpinCheckRequest();
            request.IDPCode = System.Configuration.ConfigurationManager.AppSettings["IPinIIDPCODE"];
            request.IDPPassword = System.Configuration.ConfigurationManager.AppSettings["IPinIDPPWD"];
            request.CPREQuestNum = System.Configuration.ConfigurationManager.AppSettings["IPinCPREQUESTNUM"];
            request.encryptedData = encryptedData;
            request.parameter1 = parameter1;
            request.parameter2 = parameter2;
            request.parameter3 = parameter3;

            return new LoginBiz().FindIdCheckIpin(request);
        }

        public FindIdResult FindIdCheckByMobileInfo(string name, string mobile1, string mobile2, string mobile3)
        {
            return new LoginBiz().FindIdCheckByMobileInfo(name, mobile1, mobile2, mobile3);
        }

        public FindIdResult FindIdCheckBySSNoInfo(string name, string ssno, bool isCompany)
        {
            return new LoginBiz().FindIdCheckBySSNoInfo(name, ssno, isCompany);
        }

        /// <summary>
        /// 휴대폰인증 초기화
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="errorUrl"></param>
        /// <returns></returns>
        public SmsInitResult FindPasswordSmsInit(string returnUrl, string errorUrl)
        {
            SmsInitRequest request = new SmsInitRequest();
            request.SiteCode = System.Configuration.ConfigurationManager.AppSettings["SMSSiteCode"];
            request.SitePassword = System.Configuration.ConfigurationManager.AppSettings["SMSSitePassword"];
            request.ReturnUrl = returnUrl;
            request.ErrorUrl = errorUrl;

            return new AuthBiz().SmsInit(request);
        }

        public FindPasswordResult FindPasswordCheckSms(string userId, string requestNo, string encryptedData)
        {
            SmsCheckRequest request = new SmsCheckRequest();
            request.SiteCode = System.Configuration.ConfigurationManager.AppSettings["SMSSiteCode"];
            request.SitePassword = System.Configuration.ConfigurationManager.AppSettings["SMSSitePassword"];
            request.UserId = userId;
            request.RequestNo = requestNo;
            request.EncryptedData = encryptedData;

            return new LoginBiz().FindPasswordCheckSms(request);
        }

        /// <summary>
        /// 아이핀인증 초기화
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IpinInitResult FindPasswordIpinInit(string returnUrl)
        {
            IpinInitRequest request = new IpinInitRequest();
            request.IDPCode = System.Configuration.ConfigurationManager.AppSettings["IPinIIDPCODE"];
            request.IDPPassword = System.Configuration.ConfigurationManager.AppSettings["IPinIDPPWD"];
            request.CPREQuestNum = System.Configuration.ConfigurationManager.AppSettings["IPinCPREQUESTNUM"];
            request.ReturnUrl = returnUrl;

            return new AuthBiz().IpinInit(request);
        }

        public FindPasswordResult FindPasswordCheckIpin(string userId, string encryptedData, string parameter1, string parameter2, string parameter3)
        {
            IpinCheckRequest request = new IpinCheckRequest();
            request.IDPCode = System.Configuration.ConfigurationManager.AppSettings["IPinIIDPCODE"];
            request.IDPPassword = System.Configuration.ConfigurationManager.AppSettings["IPinIDPPWD"];
            request.CPREQuestNum = System.Configuration.ConfigurationManager.AppSettings["IPinCPREQUESTNUM"];
            request.encryptedData = encryptedData;
            request.UserId = userId;
            request.parameter1 = parameter1;
            request.parameter2 = parameter2;
            request.parameter3 = parameter3;

            return new LoginBiz().FindPasswordCheckIpin(request);
        }

        public FindPasswordResult FindPasswordCheckByMobileInfo(string userId, string name, string mobile1, string mobile2, string mobile3)
        {
            return new LoginBiz().FindPasswordCheckByMobileInfo(userId, name, mobile1, mobile2, mobile3);
        }

        public SetPasswordResult PasswordInitialize(string userId)
        {
            return new LoginBiz().PasswordInitialize(userId);
        }

        public SetPasswordResult SendTempPasswordToEmail(string userId)
        {
            return new LoginBiz().SendTempPasswordToEmail(userId);
        }

        public SetPasswordResult ModifyPassword(string userId, string password)
        {
            return new LoginBiz().ModifyPassword(userId, password);
        }

        /// <summary>
        /// 로그인 로그 기록 (AS-IS 유지용)
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="ip"></param>
        public void LoginLog(int userNumber, string ip)
        {
            new LoginBiz().LoginLog(userNumber, ip);
        }

        /// <summary>
        /// 로그인 로그 기록 (도메인 별)
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="userId"></param>
        /// <param name="webType"></param>
        /// <param name="webFrom"></param>
        /// <param name="webServerName"></param>
        /// <param name="loginSite"></param>
        /// <param name="loginDate"></param>
        /// <param name="clientIp"></param>
        /// <param name="requestUrl"></param>
        public void DomainLoginLog(int? userNumber, string userId, string webType, string webFrom, string webServerName, string loginSite, DateTime loginDate, string clientIp, string requestUrl)
        {
            new LoginBiz().DomainLoginLog(userNumber, userId, webType, webFrom, webServerName, loginSite, loginDate, clientIp, requestUrl);
        }
    }
}
