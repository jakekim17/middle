using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public class NoticeStockCondition : BaseCondition
    {
        public string StockCode { get; set; }
        public string ArjCode { get; set; }
        public string FSeq { get; set; }
        public string FDataDay { get; set; }
        public string FContent { get; set; }

    }
}
