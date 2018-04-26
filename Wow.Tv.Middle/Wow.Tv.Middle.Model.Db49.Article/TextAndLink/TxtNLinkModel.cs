using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;

namespace Wow.Tv.Middle.Model.Db49.Article.TextAndLink
{
    public class TxtNLinkModel<T> : ListModel<T>
    {
        public List<NTB_COMMON_CODE> CodeList { get; set; }
        public Dictionary<string, int> GroupCodeCount { get; set; }
    }
}
