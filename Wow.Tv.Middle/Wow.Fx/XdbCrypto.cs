using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XdbNet;

namespace Wow.Fx
{
    public class XdbCrypto
    {
        public static string Hash(string value)
        {
            var hash = xCrypto.Hash(6, value);

            return hash;
        }

        public static string Encrypt(string value)
        {
            xCrypto.RegisterEx("normal", 2, @"C:\\xecuredb\\conf\\xdsp_pool.properties", "pool1", "wowtv_db", "wowtv_owner", "wowtv_table", "normal");
            string encrypt = xCrypto.Encrypt("normal", value);

            return encrypt;
        }


        public static string Decrypt(string value)
        {
            xCrypto.RegisterEx("normal", 2, @"C:\\xecuredb\\conf\\xdsp_pool.properties", "pool1", "wowtv_db", "wowtv_owner", "wowtv_table", "normal");
            var decrypt = xCrypto.Decrypt("normal", value);

            return decrypt;
        }


    }
}
