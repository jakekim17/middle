using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db89.wowbill.Member
{
    public class AuthDormancyCheckMemberResult
    {
        /// <summary>
        /// 성공여부
        /// </summary>
        public bool IsSuccess { get; set; }
    }

    public class AuthDormancyCheckIpinResult
    {
        /// <summary>
        /// 아이핀 인증 결과
        /// </summary>
        public IpinCheckResult IpinCheckResult { get; set; }

        /// <summary>
        /// 성공여부
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 회원가입불가처리여부
        /// </summary>
        public bool IsDenial { get; set; }

        /// <summary>
        /// 회원가입불가처리내용
        /// </summary>
        public string DenialText { get; set; }
    }

    public class AuthDormancyCheckSmsResult
    {
        /// <summary>
        /// 아이핀 인증 결과
        /// </summary>
        public SmsCheckResult SmsCheckResult { get; set; }

        /// <summary>
        /// 성공여부
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 회원가입불가처리여부
        /// </summary>
        public bool IsDenial { get; set; }

        /// <summary>
        /// 회원가입불가처리내용
        /// </summary>
        public string DenialText { get; set; }
    }

    public class RegistUserRequest
    {
        public List<KeyValuePair<string, string>> NBoxRequest { get; set; }

        /// <summary>
        /// 인증타입
        ///  - 개인회원: I(아이핀), M(휴대폰)
        ///  - 외국회원: F
        ///  - 기업회원: C
        /// </summary>
        public string AuthType { get; set; }

        public string UserId { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string SSNo1 { get; set; }
        public string SSNo2 { get; set; }
        public string SSNo3 { get; set; }
        public string EcUserId { get; set; }
        public string LottoId { get; set; }
        public string SFrom { get; set; }
        public string SPageFrom { get; set; }
        public string SPRO { get; set; }
        public string DupInfo { get; set; }
        public string ConnInfo { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Is969mobile { get; set; }
        public string Isstockloan { get; set; }
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
        public Nullable<byte> PasswordConfirmId { get; set; }
        public string PasswordConfirmAnswer { get; set; }
        public bool IsMale { get; set; } // 남자: true, 여자: false
        public string BirthDate { get; set; }
        public bool Issolar { get; set; }
        public string BOQv5BillHost { get; set; }
        public string WowTvFrontUrl { get; set; }
        public string WowNetCafeUrl { get; set; }

        public string Owner { get; set; }
        public string Businessitem { get; set; }
        public string BusinessCondition { get; set; }
        public string BusinessType { get; set; }
        public string CompanyNo { get; set; }
        public bool Listed { get; set; }
        public string EstablishmentAnniversary { get; set; }
        public bool Landcenter { get; set; }

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
    }

    public class RegistUserResult
    {
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public int UserNumber { get; set; }
        public bool OKCashbagRegistYN { get; set; }
        public bool OKCashbagRegistSuccess { get; set; }
        public string OKCashbagRegistMessage { get; set; }

        public bool CafeGoldMemberRegist { get; set; }

        public bool CouponBill1Regist { get; set; }
        public int CouponBill1ReturnCode { get; set; }
        public string CouponBill1ReturnMessage { get; set; }

        public bool CouponBill2Regist { get; set; }
        public int CouponBill2ReturnCode { get; set; }
        public string CouponBill2ReturnMessage { get; set; }

        public bool CouponBill3Regist { get; set; }
        public int CouponBill3ReturnCode { get; set; }
        public string CouponBill3ReturnMessage { get; set; }

        public int BonusCash { get; set; }
    }

    public class FindIdResult
    {
        public MemberType MemberType { get; set; }
        public string UserId { get; set; }
        public DateTime RegistDate { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
    public enum MemberType
    {
        Unknown = 0, General = 1, Foreign = 2, Company = 3
    }

    public class FindPasswordResult
    {
        public int UserNumber { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public bool Success { get; set; }
    }

    public class EmailAuthResult
    {
        public bool Success { get; set; }
        public string ReturnMessage { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public class EmailAuthCompleteResult
    {
        public bool UserMatched { get; set; }
        public bool EmailMatched { get; set; }
        public bool EmailAuthCompleted { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
