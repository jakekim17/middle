using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{

    public class FinanceCondition : BaseCondition
    {
        public string SearchType { get; set; }
        public string SearchText { get; set; }
        public string SearchKey { get; set; }
        public string SearchStr { get; set; }
        public string TabSeq { get; set; }
        public string BoardType { get; set; }
        public string AriticleId { get; set; }
    }
}
