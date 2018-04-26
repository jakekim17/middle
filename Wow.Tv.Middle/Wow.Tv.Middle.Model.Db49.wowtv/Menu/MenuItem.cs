using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv.Menu
{
    public class MenuItem
    {
        public int MenuSeq { get; set; }
        public bool? ReadYn { get; set; }
        public bool? WriteYn { get; set; }
        public bool? DeleteYn { get; set; }
    }
}
