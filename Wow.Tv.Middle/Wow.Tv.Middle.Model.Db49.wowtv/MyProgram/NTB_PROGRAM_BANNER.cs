using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv
{
    public partial class NTB_PROGRAM_BANNER
    {
        public NTB_ATTACH_FILE AttachFile { get; set; }


        public string PublishYn
        {
            get
            {
                string result = "N";

                DateTime nowDate = DateTime.Now;
                if(START_DATE <= nowDate && END_DATE >= nowDate)
                {
                    result = "Y";
                }


                return result;
            }
        }
    }
}
