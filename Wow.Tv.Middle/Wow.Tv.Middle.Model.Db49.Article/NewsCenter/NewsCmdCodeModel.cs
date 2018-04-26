using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;

namespace Wow.Tv.Middle.Model.Db49.Article.NewsCenter
{
    public class NewsCmdCodeModel<T> : ListModel<T>
    {
        public List<NTB_COMMON_CODE> CodeList { get; set; }
    }
}
