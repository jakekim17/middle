using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Member;
using Wow.Tv.Middle.Model.Db89.Sleep_wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;

namespace Wow.Tv.Middle.WcfService.Member
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "MemberInfoService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 MemberInfoService.svc나 MemberInfoService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class MemberInfoService : IMemberInfoService
    {
        /// <summary>
        /// 회원 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public MemberInfoResult MemberInfo(int userNumber)
        {
            return new MemberInfoBiz().MemberInfo(userNumber);
        }

        /// <summary>
        /// 일반/외국인 회원 상세 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public MemberInfoGeneralResult MemberInfoGeneral(int userNumber)
        {
            return new MemberInfoBiz().MemberInfoGeneral(userNumber);
        }

        /// <summary>
        /// 법인 회원 상세 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public MemberInfoCompanyResult MemberInfoCompany(int userNumber)
        {
            return new MemberInfoBiz().MemberInfoCompany(userNumber);
        }

        ///// <summary>
        ///// 이메일 인증 여부
        ///// </summary>
        ///// <param name="userNumber"></param>
        ///// <returns></returns>
        //public bool GetEmailAuth(int userNumber)
        //{
        //    return new MemberInfoBiz().GetEmailAuth(userNumber);
        //}

        /// <summary>
        /// 비밀번호 변경 유효성 검사
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool PasswordValidation(int userNumber, string password)
        {
            return new MemberInfoBiz().PasswordValidation(userNumber, password);
        }

        /// <summary>
        /// 필명 중복 체크 (회원)
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public int IsNickNameDuplicated(string nickName, int userNumber)
        {
            return new MemberInfoBiz().IsNickNameDuplicated(nickName, userNumber);
        }

        /// <summary>
        /// 일반/외국인 회원 정보 수정
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SaveUserResult MemberSaveGeneral(SaveUserRequestGeneral request)
        {
            string wowTvMemberUrl = System.Configuration.ConfigurationManager.AppSettings["WowTvMemberUrl"];
            string wowTvMemberStyle = System.Configuration.ConfigurationManager.AppSettings["WowTvMemberStyle"];
            string wowTvMemberScript = System.Configuration.ConfigurationManager.AppSettings["WowTvMemberScript"];

            return new MemberInfoBiz().MemberSaveGeneral(request, wowTvMemberUrl, wowTvMemberStyle, wowTvMemberScript);
        }

        /// <summary>
        /// 법인 회원 정보 수정
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SaveUserResult MemberSaveCompany(SaveUserRequestCompany request)
        {
            string wowTvMemberUrl = System.Configuration.ConfigurationManager.AppSettings["WowTvMemberUrl"];
            string wowTvMemberStyle = System.Configuration.ConfigurationManager.AppSettings["WowTvMemberStyle"];
            string wowTvMemberScript = System.Configuration.ConfigurationManager.AppSettings["WowTvMemberScript"];

            return new MemberInfoBiz().MemberSaveCompany(request, wowTvMemberUrl, wowTvMemberStyle, wowTvMemberScript);
        }

        /// <summary>
        /// 문자인증을 통한 이름변경
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="requestNo"></param>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        public NameChangeResult MemberNameChange(int userNumber, string requestNo, string encryptedData)
        {
            SmsCheckRequest request = new SmsCheckRequest();
            request.SiteCode = System.Configuration.ConfigurationManager.AppSettings["SMSSiteCode"];
            request.SitePassword = System.Configuration.ConfigurationManager.AppSettings["SMSSitePassword"];
            request.RequestNo = requestNo;
            request.EncryptedData = encryptedData;

            return new MemberInfoBiz().MemberNameChange(userNumber, request);
        }

        /// <summary>
        /// 회원 비밀번호 변경
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="passwordOriginal"></param>
        /// <param name="passwordNew"></param>
        /// <param name="passwordConfirm"></param>
        /// <returns></returns>
        public PasswordChangeResult MemberPasswordChange(int userNumber, string passwordOriginal, string passwordNew, string passwordConfirm)
        {
            return new MemberInfoBiz().MemberPasswordChange(userNumber, passwordOriginal, passwordNew, passwordConfirm);
        }

        /// <summary>
        /// 회원 비밀번호 변경 연기
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="alarmDate"></param>
        public void MemberPasswordDelay(int userNumber, DateTime alarmDate)
        {
            new MemberInfoBiz().MemberPasswordDelay(userNumber, alarmDate);
        }

        /// <summary>
        /// 카카오 계정 등록 여부
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="kakaoId"></param>
        /// <returns></returns>
        public tblUserSNSKakao GetKakaoUserInfoExists(int? userNumber, long kakaoId)
        {
            return new MemberInfoBiz().GetKakaoUserInfoExists(userNumber, kakaoId);
        }

        /// <summary>
        /// 카카오 계정 등록 여부
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="facebookId"></param>
        /// <returns></returns>
        public tblUserSNSFacebook GetFacebookUserInfoExists(int? userNumber, long facebookId)
        {
            return new MemberInfoBiz().GetFacebookUserInfoExists(userNumber, facebookId);
        }

        /// <summary>
        /// 네이버 계정 등록 여부
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="naverId"></param>
        /// <returns></returns>
        public tblUserSNSNaver GetNaverUserInfoExists(int? userNumber, long naverId)
        {
            return new MemberInfoBiz().GetNaverUserInfoExists(userNumber, naverId);
        }

        /// <summary>
        /// 카카오 계정 등록 여부
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public tblUserSNSKakao GetKakaoUserInfo(int userNumber)
        {
            return new MemberInfoBiz().GetKakaoUserInfo(userNumber);
        }

        /// <summary>
        /// 카카오 계정 등록 여부
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public tblUserSNSFacebook GetFacebookUserInfo(int userNumber)
        {
            return new MemberInfoBiz().GetFacebookUserInfo(userNumber);
        }

        /// <summary>
        /// 네이버 계정 등록 여부
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public tblUserSNSNaver GetNaverUserInfo(int userNumber)
        {
            return new MemberInfoBiz().GetNaverUserInfo(userNumber);
        }

        /// <summary>
        /// 선택 동의사항 변경
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="option2"></param>
        /// <returns></returns>
        public AgreementOption2ChangeResult MemberAgreementOption2Change(int userNumber, string option2)
        {
            return new MemberInfoBiz().MemberAgreementOption2Change(userNumber, option2);
        }

        /// <summary>
        /// 탈퇴 가능여부 체크
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public MemberSecessionCheckResult MemberSecessionCheck(int userNumber)
        {
            string bOQv5BillHost = System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"];
            return new MemberInfoBiz().MemberSecessionCheck(userNumber, bOQv5BillHost);
        }

        /// <summary>
        /// 탈퇴처리
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="secessionReasonKey"></param>
        /// <param name="secessionReasonDescription"></param>
        /// <returns></returns>
        public MemberSecessionResult MemberSecession(int userNumber, string secessionReasonKey, string secessionReasonDescription)
        {
            return new MemberInfoBiz().MemberSecession(userNumber, secessionReasonKey, secessionReasonDescription);
        }

        /// <summary>
        /// 비밀번호 일치여부 확인
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool PasswordConfirm(int userNumber, string password)
        {
            return new MemberInfoBiz().PasswordConfirm(userNumber, password);
        }

        /// <summary>
        /// 회원등급
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public MemberGradeModel GetMemberGrade(int userNumber)
        {
            return new MemberInfoBiz().GetMemberGrade(userNumber);
        }

        /// <summary>
        /// TV 다시보기 권한 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public TvReplayAuthorityInfoModel GetTvReplayAuthorityInfo(int userNumber, int num)
        {
            TvReplayAuthorityInfoParameter parameter = new TvReplayAuthorityInfoParameter();
            parameter.BOQv5BillHost = System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"];
            parameter.UserNumber = userNumber;
            parameter.Num = num;
            parameter.PriceId = System.Configuration.ConfigurationManager.AppSettings["TvReplayPriceId"];
            return new MemberInfoBiz().GetTvReplayAuthorityInfo(parameter);
        }

        /// <summary>
        /// TV 다시보기 포인트 결재
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="num"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public TvReplayPaymentByPointModel TvReplayPaymentByPoint(int userNumber, int num, string ipAddress)
        {
            string bOQv5BillHost = System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"];
            return new MemberInfoBiz().TvReplayPaymentByPoint(userNumber, num, ipAddress, bOQv5BillHost);
        }

        /// <summary>
        /// 회원 정보 가져오기
        /// </summary>
        public tblUser GetMemberInfo(string userId)
        {
            return new MemberInfoBiz().GetMemberInfo(userId);
        }

        /// <summary>
        /// 회원 정보 가져오기
        /// </summary>
        public bak_tblUser GetDormancyMemberInfo(string userId)
        {
            return new MemberInfoBiz().GetDormancyMemberInfo(userId);
        }

        public tblUserDetail GetUserDetail(int userNumber)
        {
            return new MemberInfoBiz().GetUserDetail(userNumber);
        }

        public tblUserSMS GetUserSMS(int userNumber)
        {
            return new MemberInfoBiz().GetUserSMS(userNumber);
        }

        public bool UserIdExists(string userId, bool onlyApplied)
        {
            return new MemberInfoBiz().UserIdExists(userId, onlyApplied);
        }

        /// <summary>
        /// 광고성 SMS 동의 변경
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="smsAd"></param>
        public void ChangeSmsAdAggrement(int userNumber, string smsAd)
        {
            new MemberInfoBiz().ChangeSmsAdAggrement(userNumber, smsAd);
        }

        /// <summary>
        /// SNS 계정 연동 변경
        /// </summary>
        /// <param name="memberSNSLink"></param>
        public void ChangeSNSLinkInfo(ChangeMemberSNSLinkInfo memberSNSLink)
        {
            new MemberInfoBiz().ChangeSNSLinkInfo(memberSNSLink);
        }
    }
}
