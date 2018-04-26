using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.StockholderBoard
{
    public class StockholderBoardCondition : BaseCondition
    {
        public string SearchType { get; set; }
        public string SearchText { get; set; }
        public string BlindYn { get; set; }
    }
}
