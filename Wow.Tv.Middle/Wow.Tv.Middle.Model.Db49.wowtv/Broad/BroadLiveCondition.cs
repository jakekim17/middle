using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.Broad
{
    public class BroadLiveCondition : BaseCondition
    {
        public DateTime? BroadStartDate { get; set; }
        public DateTime? BroadEndDate { get; set; }
        public string PublishYn { get; set; }
        public string ProgramName { get; set; }
    }
}
