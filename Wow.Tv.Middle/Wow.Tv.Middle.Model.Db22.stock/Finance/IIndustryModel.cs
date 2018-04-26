using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public interface IIndustryModel<T, M, N>
    {
        T StockInfo { get; set; }
        M IndustryPartList { get; set; }
        N DetailList { get; set; }
}
}
