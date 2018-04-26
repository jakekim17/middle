using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public interface IStockModel<T, M>
    {
        T StockInfo { get; }
        M ThemaInfo { get; }
    }
}
