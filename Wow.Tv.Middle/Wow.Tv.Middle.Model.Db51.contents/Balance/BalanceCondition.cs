using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db51.contents.Balance
{
    public class BalanceCondition :BaseCondition

    {
        public String Year { get; set; }
        public String Month { get; set; }
        public int No { get; set; }
    }
}
