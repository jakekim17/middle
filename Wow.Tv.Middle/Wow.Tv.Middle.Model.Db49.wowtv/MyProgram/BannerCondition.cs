using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.MyProgram
{
    public class BannerCondition : BaseCondition
    {
        public string ProgramCode { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsNow { get; set; }
        public string PublishYn { get; set; }
        public string BannerName { get; set; }
    }
}
