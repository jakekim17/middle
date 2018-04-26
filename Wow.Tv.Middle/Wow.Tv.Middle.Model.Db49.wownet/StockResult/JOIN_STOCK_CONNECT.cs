using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wownet.StockResult
{
    public class JOIN_STOCK_CONNECT
    {
        public int SEQ { get; set; }
        public string DIVERGE { get; set; }
        public string SYEAR { get; set; }
        public string SMONTH { get; set; }
        public string SDAY { get; set; }
        public string SWEEK { get; set; }
        public string STIME1 { get; set; }
        public string STIME2 { get; set; }
        public string PLACE { get; set; }
        public Nullable<System.DateTime> REG_DATE { get; set; }
        public string REG_ID { get; set; }
        public Nullable<System.DateTime> MOD_DATE { get; set; }
        public string MOD_ID { get; set; }
        public string VIEW_FLAG { get; set; }
        public Nullable<int> MSEQ { get; set; }
        public string CONTENT { get; set; }
        public string STOCK { get; set; }
        public string STOCK_FLAG { get; set; }
        
    }
}
