using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wownet.StockSituation
{
    public class StockModel<T>
    {
        public List<T> ListData { get; set; }
        public int totalStockCnt { get; set; }
    }
}
