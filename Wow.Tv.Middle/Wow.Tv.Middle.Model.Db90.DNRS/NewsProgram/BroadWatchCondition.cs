using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db90.DNRS.NewsProgram
{
    public class BroadWatchCondition : BaseCondition
    {
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public string PublishYn { get; set; }
        public string SearchType { get; set; }
        public string SearchText { get; set; }

        public string BroadTypeCode { get; set; }
        public string BroadSectionCode { get; set; }
        public string IngYn { get; set; }
        public string FameYn { get; set; }

        public string UploadTimeGreater { get; set; }

        public string OrderByType { get; set; }
    }
}
