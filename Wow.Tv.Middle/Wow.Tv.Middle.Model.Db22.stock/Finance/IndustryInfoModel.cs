using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public class IndustryInfoModel
    {
        public usp_web_getDomesticIndustryIndex_Result StockInfo { get; set; }
        public List<t_part> KospiIndustryPart { get; set; }
        public List<t_kosdaq_part> KosdaqIndustryPart { get; set; }
        public ListModel<usp_GetIndustryDetail_Result> DetailList { get; set; }
        public ListModel<MarketResearchModel> MarketSearchTop3List { get; set; }
    }
}
