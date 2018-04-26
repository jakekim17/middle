using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wownet.StockSituation;

namespace Wow.Tv.Middle.Model.Db49.wownet.StockResult
{
    public class StockResultModel<T> : ListModel<T>
    {
        public List<string> YearList { get; set; }
        public List<string> RoundsList { get; set; }
        public List<TAB_STOCK_RESULT_CONNECT> ConnectList { get; set; }
        public List<JoinConGroup> ConCountList { get; set; }
    }
}
