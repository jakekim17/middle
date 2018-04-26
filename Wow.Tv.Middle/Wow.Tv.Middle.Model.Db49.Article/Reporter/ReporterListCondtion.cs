using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.Article.Reporter
{
    public class ReporterListCondtion: BaseCondition
    {
        public string SearchId { get; set; }
        public string SearchInitial { get; set; }
        public string SearchName { get; set; }
        public string IsRandom { get; set; }
    }
}
