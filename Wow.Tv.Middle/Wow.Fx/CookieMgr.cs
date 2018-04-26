using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;

namespace Wow.Fx
{
    public enum EncryptTypeEnum
    {
        DES,
        XdbCrypto
    }

    /// <summary>
    /// <para>쿠키 조회/설정에 사용하기 위한 유틸 클래스입니다</para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-08-03</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-08-03</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-08-03 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class CookieMgr
    {
        /// <summary>
        /// 도메인 설정 나중에 Config로 변경
        /// </summary>
        private const string Domain = "";

        /// <summary>
        /// 만료 시간 설정(단위 : 분) 나중에 Config로 변경
        /// </summary>
        private static readonly int _expire = 100;// int.Parse(ConfigurationSettings.AppSettings["EXPIRE"]);

        /// <summary>
        /// 쿠키에 여러개의 값을 등록한다
        /// </summary>
        /// <param name="cookieName">Cookie 이름</param>
        /// <param name="values">NameValueCollection 값</param>
        /// <param name="encrypt">암호화 여부</param>
        /// <param name="encryptTypeEnum">암호화 방식(encrypt = true일 때)</param>
        public static void SetMultyCookie(string cookieName, NameValueCollection values, bool encrypt, EncryptTypeEnum encryptTypeEnum )
        {
            try
            {
                if (HttpContext.Current.Request.Cookies[cookieName] != null)
                {
                    HttpContext.Current.Request.Cookies.Remove(cookieName);
                }
                var oCookies = GetBaseCookie(cookieName);

                foreach (string key in values)
                {
                    string value;
                    if (encrypt)
                        value = encryptTypeEnum == EncryptTypeEnum.XdbCrypto ? XdbCrypto.Encrypt(values[key]) : Crypt.DESEncrypt(values[key]);
                    else 
                        value = values[key];
                    
                    oCookies.Values.Add(key, HttpUtility.UrlEncode(value));
                }
                HttpContext.Current.Response.Cookies.Add(oCookies);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("쿠키 설정 오류", ex);
            }
        }


        /// <summary>
        /// 기본 쿠키 정보를 가져온다.
        /// </summary>
        /// <param name="cookieName">쿠키 이름</param>
        /// <returns>HttpCookie</returns>
        private static HttpCookie GetBaseCookie(string cookieName)
        {
            var oCookies = new HttpCookie(cookieName);//실사용자 정보
            var strImsi = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            IPAddress ipAddress;
            bool isIp = IPAddress.TryParse(strImsi, out ipAddress);
            if (!isIp && strImsi.ToUpper() != "LOCALHOST")
            {
                string domain = Domain;
                if (!string.IsNullOrEmpty(domain))
                {
                    oCookies.Domain = domain;
                }
            }
            
            // 쿠키 Path 설정
            oCookies.Path = "/";

            // 만료 시간 설정
            // 만료 시간 설정이 0이면 in-memory cookie 
            // -> 브라우저가 닫히면 쿠키도 사라진다.
            var expire = _expire;
            if (expire != 0)
            {
                oCookies.Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_expire));
            }
            return oCookies;
        }

        /// <summary>
        /// 쿠키를 설정하기 위한 유틸 메소드입니다.
        /// </summary>
        /// <param name="cookieName">쿠키 네임. </param>
        /// <param name="key">키</param>
        /// <param name="value">값</param>
        /// <param name="encrypt">값의 암호화여부</param>
        /// <param name="encryptTypeEnum"></param>
        /// <remarks>
        /// 쿠키에는 복수개의 (키,값)쌍이 저장될 수 있습니다. 
        /// 이 메소드는  하나의 (키, 값)쌍을 저장하는데 사용하는 단순한 버전입니다.
        /// </remarks>
        public static void SetCookie(string cookieName, string key, string value, bool encrypt, EncryptTypeEnum encryptTypeEnum)
        {
            try
            {
                if (encrypt)
                    value = encryptTypeEnum == EncryptTypeEnum.XdbCrypto ? XdbCrypto.Encrypt(value) : Crypt.DESEncrypt(value);
             

                //2. 쿠키 객체 준비 
                //쿠키가 이미 존재하면 삭제
                if (HttpContext.Current.Request.Cookies[cookieName] != null)
                {
                    HttpContext.Current.Request.Cookies.Remove(cookieName);
                }

                // 쿠키 객체 생성
                HttpCookie oCookies = new HttpCookie(cookieName);//실사용자 정보

                // Domain 속성 결정
                string strImsi = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
                IPAddress ipAddress;
                bool isIp = IPAddress.TryParse(strImsi, out ipAddress);
                if (isIp || strImsi.ToUpper() == "LOCALHOST")
                {
                    //로컬인경우에는 도메인을 셋팅하지 않는다.                                       
                }
                else
                {
                    string domain = Domain;
                    if (!string.IsNullOrEmpty(domain))
                    {
                        oCookies.Domain = domain;
                    }
                }
                // 쿠키 Path 설정
                oCookies.Path = "/";

                // 만료 시간 설정
                // 만료 시간 설정이 0이면 in-memory cookie 
                // -> 브라우저가 닫히면 쿠키도 사라진다.
                int expire = _expire;
                if (expire != 0)
                {
                    oCookies.Expires = DateTime.Now.AddMinutes(Convert.ToDouble(expire));
                }

                //3. 쿠키에 정보 추가 
                oCookies.Values.Add(key, HttpUtility.UrlEncode(value));

                // Response 객체에 쿠키 추가
                HttpContext.Current.Response.Cookies.Add(oCookies);

            }
            catch (Exception ex)
            {
                throw new Exception("사용자 identity 정보를 캐싱하는 과정에서 에러가 발생했습니다.", ex);
            }
        }

        /// <summary>
        /// 쿠키 값을 조회하기 위한 메소드입니다.
        /// </summary>
        /// <param name="cookieName">쿠키명</param>
        /// <param name="key">키</param>
        /// <param name="decript">값의 복호화여부</param>
        /// <param name="encryptTypeEnum">복호화 방식</param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, string key, bool decript, EncryptTypeEnum encryptTypeEnum)
        {
            string strCookieValue = string.Empty;

            if (key.Trim() == string.Empty) return strCookieValue;
            
            var httpCookie = HttpContext.Current.Request.Cookies[cookieName];
                
            if (httpCookie != null)
                strCookieValue = httpCookie[key];

            if (string.IsNullOrEmpty(strCookieValue)) return strCookieValue;
                
            strCookieValue = HttpUtility.UrlDecode(strCookieValue);
            
            if (decript)
            {
                strCookieValue = encryptTypeEnum == EncryptTypeEnum.XdbCrypto ? XdbCrypto.Encrypt(strCookieValue) : Crypt.DESEncrypt(strCookieValue);
            }
            
            return strCookieValue;
        }
        
        /// <summary>
        /// 쿠키를 삭제합니다.
        /// </summary>
        /// <param name="cookieName">쿠키명</param>
        public static void ClearCookie(string cookieName)
        {
            if (HttpContext.Current != null) 
            {
                var httpCookie = HttpContext.Current.Response.Cookies[cookieName];
                if (httpCookie != null)
                    httpCookie.Expires = DateTime.Now.AddYears(-30);
            }
        }

        /// <summary>
        /// Cookie를 수정한다.
        /// </summary>
        /// <param name="cookieName">Cookie 이름</param>
        /// <param name="cookies">NameValueCollection 값</param>
        /// <param name="encrypt">암호화 여부</param>
        /// <param name="encryptTypeEnum">암호화 방식(encrypt = true일 때)</param>
        internal static void ModifyCookie(string cookieName, NameValueCollection cookies, bool encrypt, EncryptTypeEnum encryptTypeEnum)
        {
            try
            {
                var oCookies = HttpContext.Current.Request.Cookies[cookieName];
                if (oCookies != null)
                {
                    foreach (string key in cookies)
                    {

                        string value;
                        if (encrypt)
                            value = encryptTypeEnum == EncryptTypeEnum.XdbCrypto ? XdbCrypto.Encrypt(cookies[key]) : Crypt.DESEncrypt(cookies[key]);
                        else
                            value = cookies[key];

                        oCookies.Values.Set(key, HttpUtility.UrlEncode(value));
                    }
                    HttpContext.Current.Response.Cookies.Add(oCookies);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("쿠키 설정 오류", ex);
            }
        }
    }

    /// <summary>
    /// <para>암호화 클래스입니다</para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-08-03</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-08-03</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-08-03 생성</para>
    /// </summary>
    /// <remarks></remarks>
    internal class Crypt
    {
        // 암호화 키
        private const string DesKey = "wowtv!!!";

        //------------------------------------------------------------------------
        #region DES암복호화

        // Public Function
        public static string DESEncrypt(string inStr)
        {
            return DesEncrypt(inStr, DesKey);
        }

        //문자열 암호화
        private static string DesEncrypt(string str, string key)
        {
            //키 유효성 검사
            byte[] btKey = ConvertStringToByteArrayA(key);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw new Exception("Invalid key. Key length must be 8 byte.");
            }

            //소스 문자열
            byte[] btSrc = ConvertStringToByteArray(str);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                Key = btKey,
                IV = btKey
            };


            ICryptoTransform desencrypt = des.CreateEncryptor();

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, desencrypt,CryptoStreamMode.Write);

            cs.Write(btSrc, 0, btSrc.Length);
            cs.FlushFinalBlock();


            byte[] btEncData = ms.ToArray();

            return ConvertByteArrayToStringB(btEncData);
        }//end of func DesEncrypt

        // Public Function
        //문자열 복호화
        private static string DesDecrypt(string str, string key)
        {
            //키 유효성 검사
            byte[] btKey = ConvertStringToByteArrayA(key);

            //키가 8Byte가 아니면 예외발생
            if (btKey.Length != 8)
            {
                throw (new Exception("Invalid key. Key length must be 8 byte."));
            }


            byte[] btEncData = ConvertStringToByteArrayB(str);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                Key = btKey,
                IV = btKey
            };


            ICryptoTransform desdecrypt = des.CreateDecryptor();

            MemoryStream ms = new MemoryStream();

            CryptoStream cs = new CryptoStream(ms, desdecrypt,CryptoStreamMode.Write);

            cs.Write(btEncData, 0, btEncData.Length);

            cs.FlushFinalBlock();

            byte[] btSrc = ms.ToArray();


            return ConvertByteArrayToString(btSrc);

        }//end of func DesDecrypt

        //문자열->유니코드 바이트 배열
        private static byte[] ConvertStringToByteArray(string s)
        {
            return new UnicodeEncoding().GetBytes(s);
        }

        //유니코드 바이트 배열->문자열
        private static string ConvertByteArrayToString(byte[] b)
        {
            return new UnicodeEncoding().GetString(b, 0, b.Length);
        }

        //문자열->안시 바이트 배열
        private static byte[] ConvertStringToByteArrayA(string s)
        {
            return new ASCIIEncoding().GetBytes(s);
        }

        //문자열->Base64 바이트 배열
        private static byte[] ConvertStringToByteArrayB(string s)
        {
            return Convert.FromBase64String(s);
        }

        //Base64 바이트 배열->문자열
        private static string ConvertByteArrayToStringB(byte[] b)
        {
            return Convert.ToBase64String(b);
        }

        #endregion //DES암복호화
    }
}
