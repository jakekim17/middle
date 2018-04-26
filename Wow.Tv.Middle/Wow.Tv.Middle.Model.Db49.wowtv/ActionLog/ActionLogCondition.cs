using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.ActionLog
{
    public class ActionLogCondition : BaseCondition
    {
        public string ActionCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SearchType { get; set; }
        public string SearchText { get; set; }
    }
}
