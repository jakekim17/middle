using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections;

namespace Wow.Tv.Middle.Model.Common
{
    public class NBox
    {
        public List<KeyValuePair<string, string>> RequestToNBox(NameValueCollection QueryString, NameValueCollection Form)
        {
            List<KeyValuePair<string, string>> retval = new List<KeyValuePair<string, string>>();

            foreach (KeyValuePair<string, string> query in QueryString)
            {
                KeyValuePair<string, string> item = new KeyValuePair<string, string>(query.Key, query.Value);
                retval.Add(item);
            }

            foreach (string key in Form.Keys)
            {
                KeyValuePair<string, string> item = new KeyValuePair<string, string>(key, Form[key]);
                retval.Add(item);
            }


            return retval;
        }

        public string ToString(List<KeyValuePair<string, string>> list)
        {
            List<string> keys = new List<string>();
            foreach (KeyValuePair<string, string> item in list)
            {
                if (keys.Contains(item.Key) == false)
                {
                    keys.Add(item.Key);
                }
            }

            StringBuilder outBuffer = new StringBuilder();
            outBuffer.Append("NBox={");

            for (int i = 0; i < keys.Count; i++)
            {
                string key = keys[i];
                outBuffer.Append(key + "=[");
                string value = "";
                foreach (KeyValuePair<string, string> item in list)
                {
                    if (item.Key == key)
                    {
                        value += "," + item.Value;
                    }
                }
                if (value.Length > 0)
                {
                    value = value.Substring(1);
                }

                outBuffer.Append(value + "]");

                if (i + 1 < keys.Count)
                {
                    outBuffer.Append(", ");
                }
            }
            outBuffer.Append("} ");
            return outBuffer.ToString();
        }
    }
}
