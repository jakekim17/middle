using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public class CharacterStockModel
    {
        public NUP_NEWS_SECTION_SELECT_Result NewsData { set; get; }
        public usp_GetStockPrice_Result StockData { set; get; }
    }
}
