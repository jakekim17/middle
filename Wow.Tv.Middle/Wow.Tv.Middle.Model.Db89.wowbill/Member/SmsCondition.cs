using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db89.wowbill.Member
{
    public class SmsInitRequest
    {
        public string SiteCode { get; set; }
        public string SitePassword { get; set; }
        public bool isMobile { get; set; }
        public string ReturnUrl { get; set; }
        public string ErrorUrl { get; set; }
    }

    public class SmsInitResult
    {
        public string RequestNo { get; set; }
        public string EncryptedData { get; set; }
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
    }

    public class SmsCheckRequest
    {
        public string UserId { get; set; }
        public string RequestNo { get; set; }
        public string SiteCode { get; set; }
        public string SitePassword { get; set; }
        public string EncryptedData { get; set; }
    }

    public class SmsCheckResult
    {
        /// <summary>
        /// 리턴 코드
        /// </summary>
        public int ReturnCode { get; set; }

        /// <summary>
        /// 리턴 메시지
        /// </summary>
        public string ReturnMessage { get; set; }

        /// <summary>
        /// 복호화 시간
        /// </summary>
        public string CipherTime { get; set; }

        /// <summary>
        /// 요청 번호
        /// </summary>
        public string RequestSeq { get; set; }

        /// <summary>
        /// 응답고유번호
        /// </summary>
        public string ResponseSeq { get; set; }

        /// <summary>
        /// 인증수단
        /// </summary>
        public string AuthType { get; set; }

        /// <summary>
        /// 성명
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 생년월일(YYYYMMDD)
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 성별
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 내/외국인정보
        /// </summary>
        public string NationalInfo { get; set; }

        /// <summary>
        /// DI
        /// </summary>
        public string DI { get; set; }

        /// <summary>
        /// CI
        /// </summary>
        public string CI { get; set; }

        /// <summary>
        /// 휴대폰번호
        /// </summary>
        public string MobileNo { get; set; }

        /// <summary>
        /// 통신사
        /// </summary>
        public string MobileCompany { get; set; }
    }
}
