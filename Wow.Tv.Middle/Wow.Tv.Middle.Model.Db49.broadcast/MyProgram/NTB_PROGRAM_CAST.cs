using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.broadcast
{
    public partial class NTB_PROGRAM_CAST
    {
        public System.Collections.Generic.List<string> DayOfWeekString
        {
            get
            {
                List<string> list = new List<string>();
                if (MONDAY_YN == "Y")
                {
                    list.Add("월");
                }
                if (TUESDAY_YN == "Y")
                {
                    list.Add("화");
                }
                if (WEDNESDAY_YN == "Y")
                {
                    list.Add("수");
                }
                if (THURSDAY_YN == "Y")
                {
                    list.Add("목");
                }
                if (FRIDAY_YN == "Y")
                {
                    list.Add("금");
                }
                if (STURDAY_YN == "Y")
                {
                    list.Add("토");
                }
                if (SUNDAY_YN == "Y")
                {
                    list.Add("일");
                }

                return list;
            }
        }

        public string CastTypeName
        {
            get
            {
                string result = "";

                if(CAST_TYPE == "Make")
                {
                    result = "제작진";
                }
                else
                {
                    result = "출연진";
                }

                return result;
            }
        }
    }
}
