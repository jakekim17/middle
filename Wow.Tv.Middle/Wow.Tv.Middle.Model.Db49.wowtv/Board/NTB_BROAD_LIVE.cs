using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv
{
    public partial class NTB_BROAD_LIVE
    {
        public string CurrentStatus
        {
            get
            {
                string result = "";

                if(PUBLISH_YN == "Y")
                {
                    DateTime nowDate = DateTime.Now;
                    if (BroadStartDateTime <= nowDate && BroadEndDateTime >= nowDate)
                    {
                        result = "방송중";
                    }
                    else
                    {
                        result = "종영";
                    }
                }
                else
                {
                    result = "준비중";
                }

                return result;
            }
        }


        public DateTime BroadStartDateTime
        {
            get
            {
                DateTime result = new DateTime(BROAD_DATE.Year, BROAD_DATE.Month, BROAD_DATE.Day, START_HOUR, START_MINUT, 0);

                return result;
            }
        }
        public DateTime BroadEndDateTime
        {
            get
            {
                DateTime result = new DateTime(BROAD_DATE.Year, BROAD_DATE.Month, BROAD_DATE.Day, END_HOUR, END_MINUT, 0);

                return result;
            }
        }
    }
}
