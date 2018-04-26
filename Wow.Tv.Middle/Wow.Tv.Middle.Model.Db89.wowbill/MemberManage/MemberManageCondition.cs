using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db89.wowbill.MemberAdminManage
{
    public class MemberManageCondition : BaseCondition
    {
        public DateTime? RegistDateFrom { get; set; }
        public DateTime? RegistDateTo { get; set; }
        public DateTime? LatestConnectDateFrom { get; set; }
        public DateTime? LatestConnectDateTo { get; set; }

        public string SearchType { get; set; }
        public string SearchText { get; set; }

        public int UserNumber { get; set; }
        public string ParameterMessage { get; set; }
    }

    public class MemberManageListResult
    {
        public Nullable<long> ROW_NUM { get; set; }
        public string USERID { get; set; }
        public string USERNAME { get; set; }
        public string NICKNAME { get; set; }
        public string MOBILE_NO { get; set; }
        public string TEL_NO { get; set; }
        public string EMAIL { get; set; }
        public decimal? WOW_CASH { get; set; }
        public string USER_SECTION { get; set; }
        public string COMPANY_NAME { get; set; }
        public int? USER_CLASS { get; set; }
        public string USER_CLASS_TEXT
        {
            get
            {
                string retval = "";
                if (USER_CLASS.HasValue == true)
                {
                    switch (USER_CLASS.Value)
                    {
                        case 0: retval = "패밀리"; break;
                        case 1: retval = "우수"; break;
                        case 2: retval = "최우수"; break;
                    }
                }
                return retval;
            }
        }
        public string USER_TYPE { get; set; }
        public string ALIVE_TYPE { get; set; }
        public int USERNUMBER { get; set; }
        public Nullable<int> JOIN_TODAY_COUNT { get; set; }
        public Nullable<int> SEARCHED_TOTAL_COUNT { get; set; }
    }

    public class MemberManageInfoResult
    {
        public NUP_ADMIN_MEMBER_INFO_SELECT_Result MemberInfo { get; set; }
        public ListModel<NUP_ADMIN_MEMBER_INFO_HISTORY_SELECT_Result> MemberHistory { get; set; }
        public List<string> MemberInterestList { get; set; }

        public decimal? WOW_CASH { get; set; }
        public int? USER_CLASS { get; set; }
        public string USER_CLASS_TEXT
        {
            get
            {
                string retval = "";
                if (USER_CLASS.HasValue == true)
                {
                    switch (USER_CLASS.Value)
                    {
                        case 0: retval = "패밀리"; break;
                        case 1: retval = "우수"; break;
                        case 2: retval = "최우수"; break;
                    }
                }
                return retval;
            }
        }
        public int COUPON_COUNT { get; set; }

        public string LatestApprovalLog { get; set; }

        /// <summary>
        /// 연간수입
        /// </summary>
        public List<MemberManageCode> Salary { get; set; }

        /// <summary>
        /// 투자선호대상
        /// </summary>
        public List<MemberManageCode> InvestmentPreferenceObject { get; set; }

        /// <summary>
        /// 기존정보 습득처
        /// </summary>
        public List<MemberManageCode> InfoAcquirement { get; set; }

        /// <summary>
        /// 투자기간
        /// </summary>
        public List<MemberManageCode> InvestmentPeriod { get; set; }

        /// <summary>
        /// 투자성향
        /// </summary>
        public List<MemberManageCode> InvestmentPropensity { get; set; }

        /// <summary>
        /// 주요증권거래처
        /// </summary>
        public List<MemberManageCode> StockCompany { get; set; }

        /// <summary>
        /// 투자규모
        /// </summary>
        public List<MemberManageCode> InvestmentScale { get; set; }

        /// <summary>
        /// 관심분야
        /// </summary>
        public List<MemberManageCode> Interest { get; set; }

        public List<MemberManageHistory> History { get; set; }

        /// <summary>
        /// 휴면회원 모바일번호
        /// </summary>
        public string DormancyMobileNo { get; set; }
    }

    public class MemberManageCode
    {
        public int Id { get; set; }
        public string Descript { get; set; }
    }

    public class MemberManageHistory
    {
        public string AdminId { get; set; }
        public DateTime ChangedDate { get; set; }
        public string Descript { get; set; }
		public string OperationType { get; set; }
	}

    public class UserInfoSaveRequest
    {
        /// <summary>
        /// 사용자PK
        /// </summary>
        public int UserNumber { get; set; }

        // 정보성 이메일 수신동의
        public string IsSendEmail { get; set; }

        // 광고성 이메일 수신동의
        public string IsSendEmailAd { get; set; }

        // 정보성 SMS 수신여부
        public string IsSendSms { get; set; }

        // 광고성 SMS 수신여부
        public string IsSendSmsAd { get; set; }

        /// <summary>
        /// 전화번호
        /// </summary>
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string TelNo3 { get; set; }

        /// <summary>
        /// 팩스번호
        /// </summary>
        public string FaxNo1 { get; set; }
        public string FaxNo2 { get; set; }
        public string FaxNo3 { get; set; }

        /// <summary>
        /// 휴대폰번호
        /// </summary>
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string MobileNo3 { get; set; }

        /// <summary>
        /// 우편번호
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 주소
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 연간수입
        /// </summary>
        public byte? Salary { get; set; }

        /// <summary>
        /// 투자선호대상
        /// </summary>
        public byte? InvestmentPreferenceObject { get; set; }

        /// <summary>
        /// 기존정보 습득처
        /// </summary>
        public byte? InfoAcquirement { get; set; }

        /// <summary>
        /// 투자기간
        /// </summary>
        public byte? InvestmentPeriod { get; set; }

        /// <summary>
        /// 투자성향
        /// </summary>
        public byte? InvestmentPropensity { get; set; }

        /// <summary>
        /// 주요증권거래처
        /// </summary>
        public byte? StockCompany { get; set; }

        /// <summary>
        /// 투자규모
        /// </summary>
        public byte? InvestmentScale { get; set; }

        /// <summary>
        /// 관심분야
        /// </summary>
        public string InterestList { get; set; }

        /// <summary>
        /// 회사명
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 사업상태
        /// </summary>
        public string BusinessCondition { get; set; }

        /// <summary>
        /// 종목
        /// </summary>
        public string BusinessItem { get; set; }

        /// <summary>
        /// 사업자번호
        /// </summary>
        public string BusinessNumber { get; set; }

        /// <summary>
        /// 관리자 아이디
        /// </summary>
        public string AdminId { get; set; }
    }

    public class UserInfoModifyResult
    {
        public bool IsSuccess { get; set; }
        public int ReturnNumber { get; set; }
        public string ReturnMessage { get; set; }
    }

    public class MemberInfoStatisticsResult
    {
        public MemberInfoStatistics Statistics { get; set; }
        public List<MemberInfoAge> AgeList { get; set; }
    }

    public class MemberInfoStatistics
    {
        /// <summary>
        /// 전체 회원수
        /// </summary>
        public int TOTAL_MEMBER_COUNT { get; set; }

        /// <summary>
        /// 일반 회원수
        /// </summary>
        public int ALIVE_MEMBER_COUNT { get; set; }

        /// <summary>
        /// 금일 가입 회원 수
        /// </summary>
        public int JOIN_TODAY_COUNT { get; set; }

        /// <summary>
        /// SMS 수신동의
        /// </summary>
        public int SMS_RECEIVE_MEMBER_COUNT { get; set; }

        /// <summary>
        /// EMail 수신동의
        /// </summary>
        public int EMAIL_RECEIVE_MEMBER_COUNT { get; set; }

        /// <summary>
        /// 성별 - 전체
        /// </summary>
        public int TOTAL_GENDER_COUNT { get; set; }

        /// <summary>
        /// 성별 - 남자
        /// </summary>
        public int MAN_COUNT { get; set; }

        /// <summary>
        /// 성별 - 여자
        /// </summary>
        public int WOMAN_COUNT { get; set; }
    }

    public class MemberInfoAge
    {
        public int YEAR_FROM { get; set; }
        public int YEAR_TO { get; set; }
        public int YEAR_AREA { get; set; }
        public int MEMBER_COUNT { get; set; }
    }

    public class MemberInfoSimple
    {
        public int UserNumber { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}
