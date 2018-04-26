using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv
{
    public partial class TAB_CMS_ADMIN
    {
        public DateTime PREV_LAST_LOGIN_DATE { get; set; }
        public string PREV_LAST_LOGIN_IP { get; set; }

        public string GroupName { get; set; }
        public string PartCodeName { get; set; }

        public string CheckIp
        {
            get
            {
                string checkIp = "";

                if (String.IsNullOrEmpty(IP_TYPE) == true || IP_TYPE == "IPv4")
                {
                    checkIp = IP;
                }
                else if (IP_TYPE == "IPv6")
                {
                    checkIp = IP6;
                }

                return checkIp;
            }
        }
    }
}
