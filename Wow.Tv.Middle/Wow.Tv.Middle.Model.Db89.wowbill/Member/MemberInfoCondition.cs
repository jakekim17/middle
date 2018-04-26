using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db89.wowbill.Member
{
    public class MemberInfoResult
    {
        /// <summary>
        /// 기본정보
        /// </summary>
        public NUP_MEMBER_INFO_SELECT_Result MemberInfo { get; set; }

        /// <summary>
        /// 와우캐시
        /// </summary>
        public decimal WowCash { get; set; }

        /// <summary>
        /// 쿠폰 카운트
        /// </summary>
        public int CouponCount { get; set; }
        
        /// <summary>
        /// 사용자 구분
        /// </summary>
        public int UserClass { get; set; }

        /// <summary>
        /// 사용한 금액
        /// </summary>
        public decimal UsedPrice { get; set; }
    }

    public class MemberInfoGeneralResult
    {
        /// <summary>
        /// 기본정보
        /// </summary>
        public NUP_MEMBER_INFO_SELECT_GENERAL_Result MemberInfo { get; set; }

        /// <summary>
        /// 시/도
        /// </summary>
        public List<string> Sido { get; set; }

        /// <summary>
        /// 구/군
        /// </summary>
        public List<string> Gugun { get; set; }

        /// <summary>
        /// 연간수입
        /// </summary>
        public List<MemberInfoCode> Salary { get; set; }

        /// <summary>
        /// 투자선호대상
        /// </summary>
        public List<MemberInfoCode> InvestmentPreferenceObject { get; set; }

        /// <summary>
        /// 기존정보 습득처
        /// </summary>
        public List<MemberInfoCode> InfoAcquirement { get; set; }

        /// <summary>
        /// 투자기간
        /// </summary>
        public List<MemberInfoCode> InvestmentPeriod { get; set; }

        /// <summary>
        /// 투자성향
        /// </summary>
        public List<MemberInfoCode> InvestmentPropensity { get; set; }

        /// <summary>
        /// 주요증권거래처
        /// </summary>
        public List<MemberInfoCode> StockCompany { get; set; }

        /// <summary>
        /// 투자규모
        /// </summary>
        public List<MemberInfoCode> InvestmentScale { get; set; }

        /// <summary>
        /// 관심분야
        /// </summary>
        public List<MemberInfoCode> Interest { get; set; }

        /// <summary>
        /// 직업
        /// </summary>
        public List<MemberInfoCode> Job { get; set; }

        /// <summary>
        /// 가입경로
        /// </summary>
        public List<MemberInfoCode> RegistRoute { get; set; }
    }

    public class MemberInfoCompanyResult
    {
        /// <summary>
        /// 기본정보
        /// </summary>
        public NUP_MEMBER_INFO_SELECT_COMPANY_Result MemberInfo { get; set; }

        /// <summary>
        /// 연간수입
        /// </summary>
        public List<MemberInfoCode> Salary { get; set; }

        /// <summary>
        /// 투자선호대상
        /// </summary>
        public List<MemberInfoCode> InvestmentPreferenceObject { get; set; }

        /// <summary>
        /// 기존정보 습득처
        /// </summary>
        public List<MemberInfoCode> InfoAcquirement { get; set; }

        /// <summary>
        /// 투자기간
        /// </summary>
        public List<MemberInfoCode> InvestmentPeriod { get; set; }

        /// <summary>
        /// 투자성향
        /// </summary>
        public List<MemberInfoCode> InvestmentPropensity { get; set; }

        /// <summary>
        /// 주요증권거래처
        /// </summary>
        public List<MemberInfoCode> StockCompany { get; set; }

        /// <summary>
        /// 투자규모
        /// </summary>
        public List<MemberInfoCode> InvestmentScale { get; set; }

        /// <summary>
        /// 관심분야
        /// </summary>
        public List<MemberInfoCode> Interest { get; set; }

        /// <summary>
        /// 직업
        /// </summary>
        public List<MemberInfoCode> Job { get; set; }

        /// <summary>
        /// 가입경로
        /// </summary>
        public List<MemberInfoCode> RegistRoute { get; set; }
    }

    public class SaveUserRequestGeneral
    {
        public int UserNumber { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Tel3 { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Mobile3 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public bool IsSendSms { get; set; }
        public bool IsSendSmsAd { get; set; }
        public bool IsSendEmail { get; set; }
        public bool IsSendEmailAd { get; set; }
        public string Sido { get; set; }
        public string Gugun { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PasswordOriginal { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordConfirm { get; set; }

        /// <summary>
        /// 연간수입
        /// </summary>
        public int? SalaryId { get; set; }

        /// <summary>
        /// 투자선호대상
        /// </summary>
        public int? InvestmentPreferenceObjectId { get; set; }

        /// <summary>
        /// 기존정보 습득처
        /// </summary>
        public int? InfoAcquirementId { get; set; }

        /// <summary>
        /// 투자기간
        /// </summary>
        public int? InvestmentPeriodId { get; set; }

        /// <summary>
        /// 투자성향
        /// </summary>
        public int? InvestmentPropensityId { get; set; }

        /// <summary>
        /// 주요증권거래처
        /// </summary>
        public int? StockCompany { get; set; }

        /// <summary>
        /// 투자규모
        /// </summary>
        public int? InvestmentScaleId { get; set; }

        /// <summary>
        /// 관심분야
        /// </summary>
        public string InterestArea { get; set; }

        /// <summary>
        /// 직업
        /// </summary>
        public int? JobId { get; set; }

        /// <summary>
        /// 가입경로
        /// </summary>
        public int? RegistRouteId { get; set; }

        /// <summary>
        /// 카카오 아이디
        /// </summary>
        public long? KakaoId { get; set; }

        /// <summary>
        /// 카카오 이메일
        /// </summary>
        public string KakaoEmail { get; set; }

        /// <summary>
        /// 카카오 닉네임
        /// </summary>
        public string KakaoNickname { get; set; }

        /// <summary>
        /// 페이스북 아이디
        /// </summary>
        public long? FacebookId { get; set; }

        /// <summary>
        /// 페이스북 이메일
        /// </summary>
        public string FacebookEmail { get; set; }

        /// <summary>
        /// 페이스북 이름
        /// </summary>
        public string FacebookName { get; set; }

        /// <summary>
        /// 네이버 아이디
        /// </summary>
        public long? NaverId { get; set; }

        /// <summary>
        /// 네이버 이메일
        /// </summary>
        public string NaverEmail { get; set; }

        /// <summary>
        /// 네이버 이름
        /// </summary>
        public string NaverkName { get; set; }

        /// <summary>
        /// 모바일 기기 여부
        /// </summary>
        public bool IsMobile { get; set; }
    }

    public class SaveUserRequestCompany
    {
        public int UserNumber { get; set; }
        public string NickName { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Tel3 { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Mobile3 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public bool IsSendSms { get; set; }
        public bool IsSendSmsAd { get; set; }
        public bool IsSendEmail { get; set; }
        public bool IsSendEmailAd { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PasswordOriginal { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordConfirm { get; set; }

        /// <summary>
        /// 대표자
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 사업종목
        /// </summary>
        public string Businessitem { get; set; }

        /// <summary>
        /// 사업상태
        /// </summary>
        public string BusinessCondition { get; set; }

        /// <summary>
        /// 사업분류
        /// </summary>
        public string BusinessCategory { get; set; }

        /// <summary>
        /// 법인등록번호
        /// </summary>
        public string CompanyNo { get; set; }

        /// <summary>
        /// 창립기념일
        /// </summary>
        public string EstablishmentAnniversary { get; set; }

        /// <summary>
        /// 상장여부
        /// </summary>
        public bool Listed { get; set; }

        /// <summary>
        /// 연간수입
        /// </summary>
        public int? SalaryId { get; set; }

        /// <summary>
        /// 투자선호대상
        /// </summary>
        public int? InvestmentPreferenceObjectId { get; set; }

        /// <summary>
        /// 기존정보 습득처
        /// </summary>
        public int? InfoAcquirementId { get; set; }

        /// <summary>
        /// 투자기간
        /// </summary>
        public int? InvestmentPeriodId { get; set; }

        /// <summary>
        /// 투자성향
        /// </summary>
        public int? InvestmentPropensityId { get; set; }

        /// <summary>
        /// 주요증권거래처
        /// </summary>
        public int? StockCompany { get; set; }

        /// <summary>
        /// 투자규모
        /// </summary>
        public int? InvestmentScaleId { get; set; }

        /// <summary>
        /// 관심분야
        /// </summary>
        public string InterestArea { get; set; }

        /// <summary>
        /// 직업
        /// </summary>
        public int? JobId { get; set; }

        /// <summary>
        /// 가입경로
        /// </summary>
        public int? RegistRouteId { get; set; }

        /// <summary>
        /// 카카오 아이디
        /// </summary>
        public long? KakaoId { get; set; }

        /// <summary>
        /// 카카오 이메일
        /// </summary>
        public string KakaoEmail { get; set; }

        /// <summary>
        /// 카카오 닉네임
        /// </summary>
        public string KakaoNickname { get; set; }

        /// <summary>
        /// 페이스북 아이디
        /// </summary>
        public long? FacebookId { get; set; }

        /// <summary>
        /// 페이스북 이메일
        /// </summary>
        public string FacebookEmail { get; set; }

        /// <summary>
        /// 페이스북 이름
        /// </summary>
        public string FacebookName { get; set; }

        /// <summary>
        /// 네이버 아이디
        /// </summary>
        public long? NaverId { get; set; }

        /// <summary>
        /// 네이버 이메일
        /// </summary>
        public string NaverEmail { get; set; }

        /// <summary>
        /// 네이버 이름
        /// </summary>
        public string NaverkName { get; set; }

        /// <summary>
        /// 모바일 기기 여부
        /// </summary>
        public bool IsMobile { get; set; }
    }

    public class SaveUserResult
    {
        public bool IsSuccess { get; set; }
        public int ReturnNumber { get; set; }
        public string ReturnMessage { get; set; }
        public bool EmailChanged { get; set; }
        public List<string> ModifiedItemList { get; set; }
    }

    public class ARSRequestLog
    {
        public string USERID { get; set; }
        public string ARSTID { get; set; }
        public string SYMD { get; set; }
        public string EYMD { get; set; }
    }

    public class NameChangeResult
    {
        /// <summary>
        /// 휴대폰 인증 결과
        /// </summary>
        public SmsCheckResult SmsCheckResult { get; set; }

        /// <summary>
        /// 성공여부
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 리턴 메시지
        /// </summary>
        public string ReturnMessage { get; set; }

        /// <summary>
        /// 변경된 회원 이름
        /// </summary>
        public string ChangedUserName { get; set; }
    }

    public class MemberInfoCode
    {
        public int Id { get; set; }
        public string Descript { get; set; }
    }

    public class PasswordChangeResult
    {
        public string RETURN_MESSAGE { get; set; }
    }

    public class AgreementOption2ChangeResult
    {
        public bool IsSuccess { get; set; }
        public string ReturnMessage { get; set; }
    }

    public class MemberSecessionCheckResult
    {
        public bool PossibleSecession { get; set; }
        public bool HasCash { get; set; }
        public bool HasCashAttr { get; set; }
        public bool HasEventCash { get; set; }
        public bool HasAnalItem { get; set; }
        public double RealCash { get; set; }
        public double BonusCash { get; set; }
        public string CashLimitYear { get; set; }
        public string CashLimitMonth { get; set; }
        public string CashLimitDay { get; set; }
    }

    public class MemberSecessionEventSelect
    { }

    public class MemberSecessionResult
    {
        public bool IsSuccess { get; set; }
        public string ReturnMessage { get; set; }
    }

    public class ChangeMemberSNSLinkInfo
    {
        public string SNSType { get; set; }
        public string IsLink { get; set; }
        public int UserNumber { get; set; }
        public string SNSId { get; set; }
        public string SNSEmail { get; set; }
        public string SNSName { get; set; }
    }
}
