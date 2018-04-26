using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.Menu
{
    public class MenuCondition : BaseCondition
    {
        public string ChannelCode { get; set; }
        public string ContentTypeCode { get; set; }
        public string SearchType { get; set; }
        public string SearchText { get; set; }
        public string ActiveYn { get; set; }

        public int UpMenuSeq { get; set; }
        public int Depth { get; set; }

        public string SearchProgramCode { get; set; }
    }
}
