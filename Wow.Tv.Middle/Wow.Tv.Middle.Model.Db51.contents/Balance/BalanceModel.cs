using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db51.contents;
using Wow.Tv.Middle.Model.Db51.contents.Balance;

namespace Wow.Tv.Middle.Model.Db51.contents
{
    public class BalanceModel<T> : ListModel<T>
    {
        public List<BalanceYear> YearList { get; set; }
        public List<BalanceYear> MonthList { get; set; }
        public string[] DeleteList { get; set; }
        public NTB_FINANCE_STATUS AccountData1 { get; set; }
        public NTB_FINANCE_STATUS AccountData2 { get; set; }
        public NTB_FINANCE_STATUS AccountData3 { get; set; }
    }
}
