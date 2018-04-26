using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Fx;
using Wow.Tv.Middle.Biz.Member;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;

namespace Wow.Tv.Middle.WcfService.Member
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "JoinService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 JoinService.svc나 JoinService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class JoinService : IJoinService
    {
        /// <summary>
        /// 회원가입 가능여부 > 아이핀 체크
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="parameter1"></param>
        /// <param name="parameter2"></param>
        /// <param name="parameter3"></param>
        /// <returns></returns>
        public JoinCheckIpinResult JoinCheckIpin(string encryptedData, string parameter1, string parameter2, string parameter3)
        {
            IpinCheckRequest request = new IpinCheckRequest();
            request.IDPCode = System.Configuration.ConfigurationManager.AppSettings["IPinIIDPCODE"];
            request.IDPPassword = System.Configuration.ConfigurationManager.AppSettings["IPinIDPPWD"];
            request.CPREQuestNum = System.Configuration.ConfigurationManager.AppSettings["IPinCPREQUESTNUM"];
            request.encryptedData = encryptedData;
            request.parameter1 = parameter1;
            request.parameter2 = parameter2;
            request.parameter3 = parameter3;

            return new JoinBiz().JoinCheckIpin(request);
        }

        /// <summary>
        /// 회원가입 가능여부 > 휴대폰인증 체크
        /// </summary>
        /// <param name="requestNo"></param>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        public JoinCheckSmsResult JoinCheckSms(string requestNo, string encryptedData)
        {
            SmsCheckRequest request = new SmsCheckRequest();
            request.SiteCode = System.Configuration.ConfigurationManager.AppSettings["SMSSiteCode"];
            request.SitePassword = System.Configuration.ConfigurationManager.AppSettings["SMSSitePassword"];
            request.RequestNo = requestNo;
            request.EncryptedData = encryptedData;

            return new JoinBiz().JoinCheckSms(request);
        }

        /// <summary>
        /// 회원가입 가능여부 > 외국인번호 체크
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ssno1"></param>
        /// <param name="ssno2"></param>
        /// <returns></returns>
        public JoinCheckForeignResult JoinCheckForeign(string name, string ssno1, string ssno2)
        {
            string cbNameSiteCode = System.Configuration.ConfigurationManager.AppSettings["CbNameSiteCode"];
            string cbNameSitePassword = System.Configuration.ConfigurationManager.AppSettings["CbNameSitePassword"];
            return new JoinBiz().JoinCheckForeign(name, ssno1, ssno2, cbNameSiteCode, cbNameSitePassword);
        }

        /// <summary>
        /// 회원가입 처리 > 아이핀 인증 사용자
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RegistUserResult RegistUserIpin(RegistUserRequest request)
        {
            request.BOQv5BillHost = System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"];
            request.WowTvFrontUrl = System.Configuration.ConfigurationManager.AppSettings["WowTvFrontUrl"];
            request.WowNetCafeUrl = System.Configuration.ConfigurationManager.AppSettings["WowNetCafeUrl"];
            request.NBoxRequest = request.NBoxRequest;
            request.AuthType = "I";
            return new JoinBiz().RegistUserGeneral(request);
        }

        /// <summary>
        /// 회원가입 처리 > 휴대폰 인증 사용자
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RegistUserResult RegistUserSms(RegistUserRequest request)
        {
            request.BOQv5BillHost = System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"];
            request.WowTvFrontUrl = System.Configuration.ConfigurationManager.AppSettings["WowTvFrontUrl"];
            request.WowNetCafeUrl = System.Configuration.ConfigurationManager.AppSettings["WowNetCafeUrl"];
            request.NBoxRequest = request.NBoxRequest;
            request.AuthType = "M";
            RegistUserResult retval = new JoinBiz().RegistUserGeneral(request);
            return retval;
        }

        /// <summary>
        /// 회원가입 처리 > 외국인 사용자
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RegistUserResult RegistUserForeign(RegistUserRequest request)
        {
            request.BOQv5BillHost = System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"];
            request.WowTvFrontUrl = System.Configuration.ConfigurationManager.AppSettings["WowTvFrontUrl"];
            request.WowNetCafeUrl = System.Configuration.ConfigurationManager.AppSettings["WowNetCafeUrl"];
            request.NBoxRequest = request.NBoxRequest;
            request.AuthType = "F";
            if (string.IsNullOrEmpty(request.Option1) == true)
            {
                request.Option1 = "U";
            }
            return new JoinBiz().RegistUserGeneral(request);
        }

        /// <summary>
        /// 회원가입 처리 > 법인 사용자
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RegistUserResult RegistUserCompany(RegistUserRequest request)
        {
            return new JoinBiz().RegistUserCompany(request);
        }

        /// <summary>
        /// 시/도 리스트 가져오기
        /// </summary>
        /// <returns></returns>
        public List<string> GetSido()
        {
            return new JoinBiz().GetSido();
        }

        /// <summary>
        /// 구/군 리스트 가져오기
        /// </summary>
        /// <param name="sido"></param>
        /// <returns></returns>
        public List<string> GetGugun(string sido)
        {
            return new JoinBiz().GetGugun(sido);
        }

        /// <summary>
        /// 주소 리스트 가져오기
        /// </summary>
        /// <param name="searchKeyword"></param>
        /// <returns></returns>
        public List<tblPost> GetAddress(string searchKeyword)
        {
            return new JoinBiz().GetAddress(searchKeyword);
        }

        /// <summary>
        /// 아이디 중복 체크
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int IsUserIdDuplicated(string userId)
        {
            return new JoinBiz().IsUserIdDuplicated(userId);
        }

        /// <summary>
        /// 필명 중복 체크
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public int IsNickNameDuplicated(string nickName)
        {
            return new JoinBiz().IsNickNameDuplicated(nickName);
        }

        /// <summary>
        /// SMS 인증
        /// </summary>
        /// <param name="mobile1"></param>
        /// <param name="mobile2"></param>
        /// <param name="mobile3"></param>
        public string MobileSendSMS(string mobile1, string mobile2, string mobile3)
        {
            return new JoinBiz().MobileSendSMS(mobile1, mobile2, mobile3);
        }

        /// <summary>
        /// 가입된 기업정보 체크
        /// </summary>
        /// <param name="ssno1"></param>
        /// <param name="ssno2"></param>
        /// <param name="ssno3"></param>
        /// <returns></returns>
        public bool CompanyExists(string ssno1, string ssno2, string ssno3)
        {
            return new JoinBiz().CompanyExists(ssno1 + "-" + ssno2 + "-" + ssno3);
        }

        /// <summary>
        /// 이메일 인증 메일 완료 처리
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public EmailAuthCompleteResult EmailAuthComplete(int userNumber, string email)
        {
            return new JoinBiz().EmailAuthComplete(userNumber, email);
        }

        public void BillingServiceTest(string userId)
        {
            string bOQv5BillHost = System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"];
            new JoinBiz().BillingServiceTest(bOQv5BillHost, userId);
        }

        public void EmailSendTest(string mailCode, string toName, string toEmail, string fromName, string fromEmail, string subject, string contents)
        {
            new JoinBiz().EmailSendTest(mailCode, toName, toEmail, fromName, fromEmail, subject, contents);
        }
    }
}
