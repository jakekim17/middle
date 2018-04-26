using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Db35.chinaguide;
using Wow.Tv.Middle.Model.Db35.chinaguide.Article;

namespace Wow.Tv.Middle.Biz.Finance
{
    public class ChinaBiz : BaseBiz
    {
        public List<ArticleInfo> GetArticle(string typeCode)
        {
            string executeSql =
                "EXEC usp_getArticleList\r\n" +
                "@Type={0},@Market={1},@Searchkey={2},@StockCode={3},@StartDT={4}," +
                "@EndDT={5},@Page={6},@PageSize={7}";

            List<ArticleInfo> retval = db35_chinaguide.Database.SqlQuery<ArticleInfo>(executeSql, typeCode, "SH", "", "", "", "", 1, 5).ToList();

            return retval;
        }

        public string GetIssue()
        {
            string result = "";

            N_MAIN_MNG model = db35_chinaguide.N_MAIN_MNG.FirstOrDefault();
            if(model != null)
            {
                result = model.VIDEO_CONTENT;
            }

            return result;
        }
    }
}