using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.Board
{
    public class BoardCondition : BaseCondition
    {
        public string BoardTypeCode { get; set; }
        public string BoardName { get; set; }

        public string ActiveYn { get; set; }
    }
}
