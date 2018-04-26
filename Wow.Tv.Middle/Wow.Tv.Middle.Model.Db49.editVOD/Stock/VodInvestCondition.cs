using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.editVOD.Stock
{
    public class VodInvestCondition : BaseCondition
    {
        public string CtCode { get; set; }
        public string SearchType { get; set; }
        public string SearchText { get; set; }
    }
}
