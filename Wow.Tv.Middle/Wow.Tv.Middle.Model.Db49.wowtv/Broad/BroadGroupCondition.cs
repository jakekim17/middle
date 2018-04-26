using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.Broad
{
    public class BroadGroupCondition : BaseCondition
    {
        public string GroupName { get; set; }
        public List<string> programCodeList { get; set; }
    }
}
