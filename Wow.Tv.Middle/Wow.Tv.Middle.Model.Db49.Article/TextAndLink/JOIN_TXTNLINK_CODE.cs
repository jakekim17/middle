using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.Article.TextAndLink
{
    public class JOIN_TXTNLINK_CODE
    {
        public int SEQ { get; set; }
        public string CODE { get; set; }
        public string KEYWORD { get; set; }
        public string LINK { get; set; }
        public string ARTICLE_ID { get; set; }
        public string REG_ID { get; set; }
        public System.DateTime REG_DATE { get; set; }
        public string MOD_ID { get; set; }
        public Nullable<System.DateTime> MOD_DATE { get; set; }
        public string COMMON_CODE { get; set; }
        public string UP_COMMON_CODE { get; set; }
        public string CODE_NAME { get; set; }
        public int SORT_ORDER { get; set; }
        public string CODE_VALUE1 { get; set; }
    }
}
