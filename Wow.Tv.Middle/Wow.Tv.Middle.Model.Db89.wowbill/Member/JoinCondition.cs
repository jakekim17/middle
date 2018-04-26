using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db89.wowbill.Member
{
    public class JoinCheckIpinResult
    {
        /// <summary>
        /// 아이핀 인증 결과
        /// </summary>
        public IpinCheckResult IpinCheckResult { get; set; }

        /// <summary>
        /// 회원가입불가처리여부
        /// </summary>
        public bool IsDenial { get; set; }

        /// <summary>
        /// 회원가입불가처리내용
        /// </summary>
        public string DenialText { get; set; }

        /// <summary>
        /// 나이제한
        /// </summary>
        public bool AgeRestriction { get; set; }

        /// <summary>
        /// 등록된 정보
        /// </summary>
        public bool Registered { get; set; }

        /// <summary>
        /// 등록된 아이디
        /// </summary>
        public string RegisteredUserID { get; set; }

        /// <summary>
        /// 등록일자
        /// </summary>
        public DateTime RegisteredDate { get; set; }
    }

    public class JoinCheckSmsResult
    {
        /// <summary>
        /// 휴대폰 인증 결과
        /// </summary>
        public SmsCheckResult SmsCheckResult { get; set; }

        /// <summary>
        /// 회원가입불가처리여부
        /// </summary>
        public bool IsDenial { get; set; }

        /// <summary>
        /// 회원가입불가처리내용
        /// </summary>
        public string DenialText { get; set; }

        /// <summary>
        /// 나이제한
        /// </summary>
        public bool AgeRestriction { get; set; }

        /// <summary>
        /// 등록된 정보
        /// </summary>
        public bool Registered { get; set; }

        /// <summary>
        /// 등록된 아이디
        /// </summary>
        public string RegisteredUserID { get; set; }

        /// <summary>
        /// 등록일자
        /// </summary>
        public DateTime RegisteredDate { get; set; }
    }

    public class JoinCheckForeignResult
    {
        public bool Validated { get; set; }
        public bool NameCheckSuccess { get; set; }
        public int NameCheckReturnCode { get; set; }
        public bool Registered { get; set; }
        public string RegisteredUserID { get; set; }
        public DateTime RegisteredDate { get; set; }
    }
}
