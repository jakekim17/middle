using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Fx
{
    public class RSACrypto
    {
        private string _PrivateKey { get; set; }
        private string _PublicKey { get; set; }

        /// <summary>
        /// Key 크기
        /// * Key Size별 보안강도 (인수분해 방식에 대한 강도. RSA는 인수분해 방식임)
        /// 80 Bit: 1024
        /// 112 Bit: 2048
        /// 128 Bit: 3072
        /// 192 Bit: 7680
        /// 256 Bit: 15360
        /// </summary>
        private readonly int _KeySize = 2048;

        public RSACrypto(string privateKey, string publicKey)
        {
            _PrivateKey = privateKey;
            _PublicKey = publicKey;
        }

        /*           삭제하지 마세요 !!!           */
        ///// <summary>
        ///// Web.Config에 새로운 Private/Public Key 설정 시 사용
        ///// </summary>
        ///// <returns></returns>
        //public string[] CreateKeysForWebConfig()
        //{
        //    string[] retval = new string[2];

        //    // 암호화 개체 생성
        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(_KeySize);

        //    // 개인키 생성
        //    RSAParameters privateKey = RSA.Create().ExportParameters(true);
        //    rsa.ImportParameters(privateKey);
        //    retval[0] = rsa.ToXmlString(true).Replace("<", "[").Replace(">", "]");

        //    // 공개키 생성
        //    RSAParameters publicKey = new RSAParameters();
        //    publicKey.Modulus = privateKey.Modulus;
        //    publicKey.Exponent = privateKey.Exponent;
        //    rsa.ImportParameters(publicKey);
        //    retval[1] = rsa.ToXmlString(false).Replace("<", "[").Replace(">", "]");

        //    // 개인키 및 공개키 반환
        //    return retval;
        //}

        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText) == false)
            {
                string encryptedText = _EncryptProcessing(plainText, _PublicKey);
                return encryptedText;
            }
            else
            {
                return "";
            }
        }

        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText) == false)
            {
                return _DecryptProcessing(encryptedText, _PrivateKey);
            }
            else
            {
                return "";
            }
        }

        // RSA 암호화
        private string _EncryptProcessing(string getValue, string pubKey)
        {
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(_KeySize); //암호화
            rsa.FromXmlString(pubKey);

            //암호화할 문자열을 UFT8인코딩
            byte[] inbuf = (new UTF8Encoding()).GetBytes(getValue);

            //암호화
            byte[] encbuf = rsa.Encrypt(inbuf, false);

            //암호화된 문자열 Base64인코딩
            return Convert.ToBase64String(encbuf);
        }

        // RSA 복호화
        private string _DecryptProcessing(string getValue, string priKey)
        {
            //RSA객체생성
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(_KeySize); //복호화
            rsa.FromXmlString(priKey);

            //sValue문자열을 바이트배열로 변환
            byte[] srcbuf = Convert.FromBase64String(getValue);

            //바이트배열 복호화
            byte[] decbuf = rsa.Decrypt(srcbuf, false);

            //복호화 바이트배열을 문자열로 변환
            string sDec = (new UTF8Encoding()).GetString(decbuf, 0, decbuf.Length);
            return sDec;
        }

    }
}
