using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wownet.StockResult
{
    public class StockResultDetail
    {
        public TAB_STOCK_RESULT StockData { get; set; }
        public List<TAB_STOCK_RESULT_CONNECT> ConnectList { get; set; }
        public int[] DeleteList { get; set; }
    }
}
