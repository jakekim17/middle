using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.Model.Db49.Article.HubBusiness
{
    public class HubBusinessModel<T> : ListModel<T>
    {
        public NTB_HUB_BUSINESS HubData { get; set; }
    }
}
