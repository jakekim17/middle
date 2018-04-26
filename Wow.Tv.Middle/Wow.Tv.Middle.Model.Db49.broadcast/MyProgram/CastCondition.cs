using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.broadcast.MyProgram
{
    public class CastCondition : BaseCondition
    {
        public string ProgramCode { get; set; }

        public string CastType { get; set; }
        public string PublishYn { get; set; }
        public string SearchType { get; set; }
        public string SearchText { get; set; }
    }
}
