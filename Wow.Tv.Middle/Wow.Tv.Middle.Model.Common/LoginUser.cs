using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Common
{
    [Serializable]
    public class LoginUser
    {
        public int AdminSeq { get; set; }
        public string LoginId { get; set; } = "guest";
        public string UserName { get; set; } = "guest";
        public DateTime LastLoginDate { get; set; }
        public string PartCodeName { get; set; }
        public string Ip { get; set; }
        public string ConnectableIp { get; set; }

        public List<AdminMenuItem> MenuList { get; set; }
    }

    [Serializable]
    public class MemberLoginUser
    {
        public string LoginId { get; set; }
        public string UserName { get; set; }
        public DateTime LoginDate { get; set; }
        public string Ip { get; set; }
    }

    public class AdminMenuItem
    {
        public int MenuSeq { get; set; }
        public bool IsRead { get; set; }
        public bool IsWrite { get; set; }
        public bool IsDelete { get; set; }

        public string Href { get; set; }
    }

    //public class FrontLoginUser
    //{
    //    public string LoginId { get; set; }
    //    public int UserNumber { get; set; }
    //    public string UserName { get; set; }
    //    public string NickName { get; set; }
    //    public DateTime LoginDate { get; set; }
    //    public string Ip { get; set; }
    //}

    public class LoginUserInfo
    {
        public string UserId { get; set; }
        public int UserNumber { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public LoginDomain LoginFrom { get; set; }
        public DateTime LoginDate { get; set; }
        public string Ip { get; set; }
        public KakaoUserInfo KakaoInfo { get; set; }
        public NaverUserInfo NaverInfo { get; set; }
        public FacebookUserInfo FacebookInfo { get; set; }
        public string MobileNo { get;  set; }
        public bool LoginByAdmin { get; set; } = false;

        public long? SnsId
        {
            get
            {
                long? retval = null;
                switch (LoginFrom)
                {
                    case LoginDomain.Naver: retval = NaverInfo?.Id; break;
                    case LoginDomain.Kakao: retval = KakaoInfo?.Id; break;
                    case LoginDomain.Facebook: retval = FacebookInfo?.Id; break;
                }
                return retval;
            }
        }
        public string SnsEmail
        {
            get
            {
                string retval = null;
                switch (LoginFrom)
                {
                    case LoginDomain.Naver: retval = NaverInfo?.Email; break;
                    case LoginDomain.Kakao: retval = KakaoInfo?.Email; break;
                    case LoginDomain.Facebook: retval = FacebookInfo?.Email; break;
                }
                return retval;
            }
        }
        public string SnsName
        {
            get
            {
                string retval = null;
                switch (LoginFrom)
                {
                    case LoginDomain.Naver: retval = NaverInfo?.Name; break;
                    case LoginDomain.Kakao: retval = KakaoInfo?.Nickname; break;
                    case LoginDomain.Facebook: retval = FacebookInfo?.Name; break;
                }
                return retval;
            }
        }

        public LoginUserInfo()
        {
            KakaoInfo = new KakaoUserInfo();
            FacebookInfo = new FacebookUserInfo();
            NaverInfo = new NaverUserInfo();
        }
    }

    //public enum LoginType
    //{
    //    /// <summary>
    //    /// 정식 로그인
    //    /// </summary>
    //    FormalLogin = 1,

    //    /// <summary>
    //    /// 간편 로그인
    //    /// </summary>
    //    EasyLogin = 2
    //}

    public enum LoginDomain
    {
        /// <summary>
        /// 아이디/비밀번호
        /// </summary>
        IdPw,

        /// <summary>
        /// 와우넷/와우파 등 멤버 사이트
        /// </summary>
        Membership,

        /// <summary>
        /// 카카오
        /// </summary>
        Kakao,

        /// <summary>
        /// 페이스북
        /// </summary>
        Facebook,

        /// <summary>
        /// 네이버
        /// </summary>
        Naver
    }

    public class KakaoUserInfo
    {
        public long? Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
    }

    public class NaverUserInfo
    {
        public long? Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class FacebookUserInfo
    {
        public long? Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// SNS 로그인 파라미터
    /// </summary>
    /// <remarks>
    /// 정식로그인 시 간편로그인은 실행되지 않는다.
    /// </remarks>
    public class SnsLoginParameter
    {
        /// <summary>
        /// 자동 로그인 여부
        /// </summary>
        public SnsLoginActionType LoginActionType { get; set; } = SnsLoginActionType.AutoWowLogin;

        /// <summary>
        /// 로그인 Form
        /// </summary>
        public string LoginFormId { get; set; }

        /// <summary>
        /// 네이버 호출 후 리턴 Function 경로
        /// </summary>
        public string NaverReturnFunction { get; set; }

        /// <summary>
        /// 카카오 호출 후 리턴 Function 경로
        /// </summary>
        public string KakaoReturnFunction { get; set; }

        /// <summary>
        /// 페이스북 호출 후 리턴 Function 경로
        /// </summary>
        public string FacebookReturnFunction { get; set; }

        public bool KakaoJavascriptIncrude { get; set; } = true;
    }

    public enum SnsLoginActionType
    {
        /// <summary>
        /// 자동 정식 로그인
        /// </summary>
        AutoWowLogin,

        /// <summary>
        /// 자동 간편 로그인
        /// </summary>
        AutoEasyLogin,

        /// <summary>
        /// 수동처리
        /// </summary>
        Manual
    }

    public class KakaoLoginResult
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
    }

    public class KakaoInfoResult
    {
        public KakaoInfoResult()
        {
            properties = new KakaoInfoPropertyResult();
        }

        public string kaccount_email { get; set; }
        public bool kaccount_email_verified { get; set; }
        public long id { get; set; }
        public KakaoInfoPropertyResult properties { get; set; }

        public class KakaoInfoPropertyResult
        {
            public string profile_image { get; set; }
            public string nickname { get; set; }
            public string thumbnail_image { get; set; }
        }
    }
}
