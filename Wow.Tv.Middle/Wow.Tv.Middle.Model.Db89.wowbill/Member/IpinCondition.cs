using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db89.wowbill.Member
{
    public class IpinInitRequest
    {
        public string IDPCode { get; set; }
        public string IDPPassword { get; set; }
        public string CPREQuestNum { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class IpinInitResult
    {
        public string RequestData { get; set; }
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
    }

    public class IpinCheckRequest
    {
        public string UserId { get; set; }
        public string IDPCode { get; set; }
        public string IDPPassword { get; set; }
        public string CPREQuestNum { get; set; }
        public string encryptedData { get; set; }
        public string parameter1 { get; set; }
        public string parameter2 { get; set; }
        public string parameter3 { get; set; }
    }

    public class IpinCheckResult
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
        /// 가상주민번호
        /// </summary>
        public string VirtualNumber { get; set; }

        /// <summary>
        /// 이름
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 중복가입 확인값 (64 Byte 고정 값)
        /// </summary>
        public string DupInfo { get; set; }

        /// <summary>
        /// 연령대 코드
        /// </summary>
        public string AgeCode { get; set; }

        /// <summary>
        /// 성별 코드
        /// </summary>
        public string GenderCode { get; set; }

        /// <summary>
        /// 생년월일(YYYYMMDD)
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 내외국인정보
        /// </summary>
        public string NationalInfo { get; set; }

        /// <summary>
        /// 인증수단
        /// </summary>
        public string AuthInfo { get; set; }

        /// <summary>
        /// 연결정보 (88 Byte 고정 값)
        /// </summary>
        public string CoInfo1 { get; set; }

        /// <summary>
        /// CI 갱신정보
        /// </summary>
        public string CIUpdate { get; set; }
    }
}
