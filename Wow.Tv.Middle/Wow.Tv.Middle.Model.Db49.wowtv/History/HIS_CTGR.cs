using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv.History
{
    public class HIS_CTGR
    {
        public int HIS_SEQ { get; set; }
        public int CTGR_SEQ { get; set; }
        public string HIS_DATE { get; set; }
        public string HIS_CONT { get; set; }
        public string HIS_DISP_YN { get; set; }
        public string REG_ID { get; set; }
        public DateTime REG_DATE { get; set; }
        public string MOD_ID { get; set; }
        public Nullable<System.DateTime> MOD_DATE { get; set; }
        public string CTGR_YR { get; set; }
    }
}
