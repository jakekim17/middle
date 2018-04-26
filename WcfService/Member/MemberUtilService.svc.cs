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
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "MemberUtilService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 MemberUtilService.svc나 MemberUtilService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class MemberUtilService : IMemberUtilService
    {
        /// <summary>
        /// 비밀번호 초기화 메시지 전송
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="tempPassword"></param>
        public void SmsTempPassword(string mobileNo, string tempPassword)
        {
            new MemberUtilBiz().SmsTempPassword(mobileNo, tempPassword);
        }

        /// <summary>
        /// 휴면회원 해지 SMS 인증
        /// </summary>
        /// <param name="mobileNo"></param>
        public string SmsDormancyAuth(string mobileNo)
        {
            return new MemberUtilBiz().SmsDormancyAuth(mobileNo);
        }

        /// <summary>
        /// 이메일 인증 메일 전송
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="email"></param>
        /// <param name="expireDate"></param>
        /// <returns></returns>
        public void EmailAuth(int userNumber, string email, DateTime expireDate)
        {
            MemberEmailModel parameter = new MemberEmailModel();
            parameter.UserNumber = userNumber;
            parameter.Email = email;
            parameter.ExpireDate = expireDate;
            _EmailParameterSetting(parameter);
            new MemberUtilBiz().EmailAuth(parameter);
        }

        /// <summary>
        /// 회원가입 축하 메일 > 일반/외국인 사용자
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="nickName"></param>
        /// <param name="email"></param>
        /// <param name="bounsCash"></param>
        public void EmailRegistGeneral(string userName, string nickName, string email, int bounsCash)
        {
            MemberEmailModel parameter = new MemberEmailModel();
            parameter.UserName = userName;
            parameter.NickName = nickName;
            parameter.Email = email;
            parameter.BonusCash = bounsCash;
            _EmailParameterSetting(parameter);
            new MemberUtilBiz().EmailRegistGeneral(parameter);
        }

        /// <summary>
        /// 회원가입 축하 메일 > 법인 사용자
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="nickName"></param>
        /// <param name="email"></param>
        /// <param name="bounsCash"></param>
        public void EmailRegistCompany(string userName, string nickName, string email, int bounsCash)
        {
            MemberEmailModel parameter = new MemberEmailModel();
            parameter.UserName = userName;
            parameter.NickName = nickName;
            parameter.Email = email;
            parameter.BonusCash = bounsCash;
            _EmailParameterSetting(parameter);
            new MemberUtilBiz().EmailRegistCompany(parameter);
        }

        /// <summary>
        /// 메일 > 회원정보 수정 완료
        /// </summary>
        /// <param name="parameter"></param>
        public void EmailUserEdit(string userId, string nickName, List<string> changedItemList, string email)
        {
            MemberEmailModel parameter = new MemberEmailModel();
            parameter.UserId = userId;
            parameter.NickName = nickName;
            parameter.ChangedItemList = changedItemList;
            parameter.Email = email;
            _EmailParameterSetting(parameter);
            new MemberUtilBiz().EmailUserEdit(parameter);
        }

        /// <summary>
        /// 메일 > 법인회원 승인거부 안내
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        public void EmailCompanyRegistRejectAlarm(string nickName, string email, string userId)
        {
            MemberEmailModel parameter = new MemberEmailModel();
            parameter.NickName = nickName;
            parameter.Email = email;
            parameter.UserId = userId;
            _EmailParameterSetting(parameter);
            new MemberUtilBiz().EmailCompanyRegistRejectAlarm(parameter);
        }

        /// <summary>
        /// 메일 > 회원 아이디 찾기 안내
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        public void EmailFindIdAlarm(string nickName, string email, string userId)
        {
            MemberEmailModel parameter = new MemberEmailModel();
            parameter.NickName = nickName;
            parameter.Email = email;
            parameter.UserId = userId;
            _EmailParameterSetting(parameter);
            new MemberUtilBiz().EmailFindIdAlarm(parameter);
        }

        /// <summary>
        /// 메일 > 회원 임시 비밀번호 안내
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="email"></param>
        /// <param name="tempPassword"></param>
        public void EmailFindPasswordAlarm(string nickName, string email, string tempPassword)
        {
            MemberEmailModel parameter = new MemberEmailModel();
            parameter.NickName = nickName;
            parameter.Email = email;
            parameter.TempPassword = tempPassword;
            _EmailParameterSetting(parameter);
            new MemberUtilBiz().EmailFindPasswordAlarm(parameter);
        }

        /// <summary>
        /// 메일 > 회원 탈퇴 완료
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="email"></param>
        public void EmailUserSecession(string nickName, string email)
        {
            MemberEmailModel parameter = new MemberEmailModel();
            parameter.NickName = nickName;
            parameter.Email = email;
            _EmailParameterSetting(parameter);
            new MemberUtilBiz().EmailUserSecession(parameter);
        }

        private void _EmailParameterSetting(MemberEmailModel parameter)
        {
            parameter.WowTvFrontUrl = System.Configuration.ConfigurationManager.AppSettings["WowTvFrontUrl"];
            parameter.WowNetFrontUrl = System.Configuration.ConfigurationManager.AppSettings["WowNetFrontUrl"];
            parameter.WowNetCafeUrl = System.Configuration.ConfigurationManager.AppSettings["WowNetCafeUrl"];
            parameter.WowFaFrontUrl = System.Configuration.ConfigurationManager.AppSettings["WowFaFrontUrl"];
            parameter.WowStarFrontUrl = System.Configuration.ConfigurationManager.AppSettings["WowStarFrontUrl"];
            parameter.WowTvHelpInquiryUrl = System.Configuration.ConfigurationManager.AppSettings["WowTvHelpInquiryUrl"];
            parameter.WowTvMemberInfoEdit = System.Configuration.ConfigurationManager.AppSettings["WowTvMemberInfoEdit"];
            parameter.WowTvMemberStyle = System.Configuration.ConfigurationManager.AppSettings["WowTvMemberStyle"];
            parameter.WowTvMemberUrl = System.Configuration.ConfigurationManager.AppSettings["WowTvMemberUrl"];
            parameter.WowTvMyPageUrl = System.Configuration.ConfigurationManager.AppSettings["WowTvMyPageUrl"];
        }

        public void EmailTemplateTest()
        {
            int userNumber = 100924879;
            string userId = "mrpyj1";
            string userName = "박용진";
            string nickName = "박용진1";
            string email = "mrpyj@naver.com";
            string tempPassword = "1234567890";
            DateTime expireDate = DateTime.Now.AddDays(1);
            int bonusCash = 10000;
            List<string> changedItemList = new List<string>();
            changedItemList.Add("이름");
            changedItemList.Add("닉네임");
            changedItemList.Add("휴대폰번호");

            EmailAuth(userNumber, email, expireDate);
            EmailRegistGeneral(userName, nickName, email, bonusCash);
            EmailRegistCompany(userName, nickName, email, bonusCash);
            EmailUserEdit(userId, nickName, changedItemList, email);
            EmailCompanyRegistRejectAlarm(nickName, email, userId);
            EmailFindIdAlarm(nickName, email, userId);
            EmailFindPasswordAlarm(nickName, email, tempPassword);
            EmailUserSecession(nickName, email);
        }
    }
}
