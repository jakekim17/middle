using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.Group
{
    public class GroupCondition : BaseCondition
    {
        public string SearchType { get; set; }
        public string SearchText { get; set; }
    }
}
