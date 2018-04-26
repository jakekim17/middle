using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public class CurrentAnalysisCondition : BaseCondition
    {
        public string ArjCode { get; set; }
        public string SearchStr { get; set; }
        public string StockCode { get; set; }
        //public string[] InvestOpinion { get; set; }
    }
}
