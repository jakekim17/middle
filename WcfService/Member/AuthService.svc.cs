using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Member;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;

namespace Wow.Tv.Middle.WcfService.Member
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "AuthService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 AuthService.svc나 AuthService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class AuthService : IAuthService
    {
        /// <summary>
        /// 아이핀인증 초기화
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IpinInitResult IpinInit(string returnUrl)
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
        public IpinCheckResult IpinCheck(string encryptedData, string parameter1, string parameter2, string parameter3)
        {
            IpinCheckRequest request = new IpinCheckRequest();
            request.IDPCode = System.Configuration.ConfigurationManager.AppSettings["IPinIIDPCODE"];
            request.IDPPassword = System.Configuration.ConfigurationManager.AppSettings["IPinIDPPWD"];
            request.CPREQuestNum = System.Configuration.ConfigurationManager.AppSettings["IPinCPREQUESTNUM"];
            request.encryptedData = encryptedData;
            request.parameter1 = parameter1;
            request.parameter2 = parameter2;
            request.parameter3 = parameter3;

            return new AuthBiz().IpinCheck(request);
        }

        /// <summary>
        /// 휴대폰인증 초기화
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="errorUrl"></param>
        /// <returns></returns>
        public SmsInitResult SMSInit(string returnUrl, string errorUrl)
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
        public SmsCheckResult SmsCheck(string requestNo, string encryptedData)
        {
            SmsCheckRequest request = new SmsCheckRequest();
            request.SiteCode = System.Configuration.ConfigurationManager.AppSettings["SMSSiteCode"];
            request.SitePassword = System.Configuration.ConfigurationManager.AppSettings["SMSSitePassword"];
            request.RequestNo = requestNo;
            request.EncryptedData = encryptedData;

            return new AuthBiz().SmsCheck(request);
        }

        public EmailAuthResult EmailAuth(int userNumber)
        {
            return new AuthBiz().EmailAuth(userNumber);
        }
    }
}
