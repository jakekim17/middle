using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db49.wowtv;

namespace Wow.Tv.Middle.Model.Db49.Article.NewsCenter
{
    public class NewsCmtManageModel 
    {
        //public NTB_ARTICLE_COMMENT comment { get; set; }
        //public String title { get; set; }
        public int COMMENT_SEQ { get; set; }
        public String ARTICLE_ID { get; set; }
        public String COMMENT { get; set; }
        public String DEL_YN { get; set; }
        public DateTime ? REG_DATE { get; set; }
        public String REG_ID { get; set; }
        public String OPEN_YN { get; set; }
        public String Title { get; set; }
    }
}
