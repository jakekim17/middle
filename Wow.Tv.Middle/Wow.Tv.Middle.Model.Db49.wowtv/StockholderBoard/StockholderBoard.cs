using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.StockholderBoard
{
    public class StockholderBoard
    {
        public int STOCKHOLDER_SEQ { get; set; }
        public int UP_STOCKHOLDER_SEQ { get; set; }
        public string TITLE { get; set; }
        public string CONTENTS { get; set; }
        public string EMAIL { get; set; }
        public string BLIND_YN { get; set; }
        public string REG_ID { get; set; }
        public System.DateTime REG_DATE { get; set; }
        public string MOD_ID { get; set; }
        public Nullable<System.DateTime> MOD_DATE { get; set; }
        public int READ_CNT { get; set; }
        public string USER_NAME { get; set; }

        public List<NTB_STOCKHOLDER_BOARD> ReplyList { get; set; }
        public NTB_STOCKHOLDER_BOARD ReplyData { get; set; }
    }
}
