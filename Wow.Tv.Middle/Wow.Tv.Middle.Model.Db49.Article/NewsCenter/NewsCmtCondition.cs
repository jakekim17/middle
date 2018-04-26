using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.Article.NewsCenter
{
    public class NewsCmtCondition : BaseCondition
    {
        public String COMMENT_SEQ { get; set; }
        public String ARTICLE_ID { get; set; }
        public String COMMENT { get; set; }
        public String DEL_YN { get; set; }
        public DateTime? REG_DATE { get; set; }
        public String REG_ID { get; set; }
        public String OPEN_YN { get; set; }
        public String Sort { get; set; }
        public String SearchText { get; set; }
        public String SearchType { get; set; }
        public String NewsGubun { get; set; }
    }
}
