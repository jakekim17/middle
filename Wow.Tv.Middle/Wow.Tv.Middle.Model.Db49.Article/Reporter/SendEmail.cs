using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.Article.Reporter
{
    public class SendEmail
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string ReporterEmail { get; set; }
        public string ReporterName { get; set; }
    }
}
