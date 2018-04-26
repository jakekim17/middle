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
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IMemberInfoService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IMemberInfoService
    {
        [OperationContract]
        MemberInfoResult MemberInfo(int userNumber);

        [OperationContract]
        MemberInfoGeneralResult MemberInfoGeneral(int userNumber);

        [OperationContract]
        MemberInfoCompanyResult MemberInfoCompany(int userNumber);

        //[OperationContract]
        //bool GetEmailAuth(int userNumber);

        [OperationContract]
        bool PasswordValidation(int userNumber, string password);

        [OperationContract]
        int IsNickNameDuplicated(string nickName, int userNumber);

        [OperationContract]
        SaveUserResult MemberSaveGeneral(SaveUserRequestGeneral request);

        [OperationContract]
        SaveUserResult MemberSaveCompany(SaveUserRequestCompany request);

        [OperationContract]
        NameChangeResult MemberNameChange(int userNumber, string requestNo, string encryptedData);

        [OperationContract]
        tblUserSNSKakao GetKakaoUserInfoExists(int? userNumber, long kakaoId);

        [OperationContract]
        tblUserSNSFacebook GetFacebookUserInfoExists(int? userNumber, long facebookId);

        [OperationContract]
        tblUserSNSNaver GetNaverUserInfoExists(int? userNumber, long naverId);

        [OperationContract]
        tblUserSNSKakao GetKakaoUserInfo(int userNumber);

        [OperationContract]
        tblUserSNSFacebook GetFacebookUserInfo(int userNumber);

        [OperationContract]
        tblUserSNSNaver GetNaverUserInfo(int userNumber);

        [OperationContract]
        PasswordChangeResult MemberPasswordChange(int userNumber, string passwordOriginal, string passwordNew, string passwordConfirm);

        [OperationContract]
        void MemberPasswordDelay(int userNumber, DateTime alarmDate);

        [OperationContract]
        AgreementOption2ChangeResult MemberAgreementOption2Change(int userNumber, string option2);

        [OperationContract]
        MemberSecessionCheckResult MemberSecessionCheck(int userNumber);

        [OperationContract]
        MemberSecessionResult MemberSecession(int userNumber, string secessionReasonKey, string secessionReasonDescription);

        [OperationContract]
        bool PasswordConfirm(int userNumber, string password);

        [OperationContract]
        MemberGradeModel GetMemberGrade(int userNumber);

        [OperationContract]
        TvReplayAuthorityInfoModel GetTvReplayAuthorityInfo(int userNumber, int num);

        [OperationContract]
        TvReplayPaymentByPointModel TvReplayPaymentByPoint(int userNumber, int num, string ipAddress);

        [OperationContract]
        tblUser GetMemberInfo(string userId);

        [OperationContract]
        bak_tblUser GetDormancyMemberInfo(string userId);

        [OperationContract]
        tblUserDetail GetUserDetail(int userNumber);

        [OperationContract]
        tblUserSMS GetUserSMS(int userNumber);

        [OperationContract]
        bool UserIdExists(string userId, bool onlyApplied);

        [OperationContract]
        void ChangeSmsAdAggrement(int userNumber, string smsAd);

        [OperationContract]
        void ChangeSNSLinkInfo(ChangeMemberSNSLinkInfo memberSNSLink);
    }
}
