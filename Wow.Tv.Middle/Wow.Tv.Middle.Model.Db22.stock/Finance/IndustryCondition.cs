using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public class IndustryCondition : BaseCondition
    {
        public string Market { get; set; }
        public string OrderBy { get; set; }
        public string TargetDT { get; set; } 
        public bool ViewMode { get; set; }
        public string PartCode { get; set; }
        public string PartName { get; set; }
        public int MenuSeq { get; set; }
    }
}
