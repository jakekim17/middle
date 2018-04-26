using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public class StockInfoModel<T, M> : IStockModel<T, M>
    {
        public T StockInfo { get; set; }

        public M ThemaInfo { get; set; }
    }
}
