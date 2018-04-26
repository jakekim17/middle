using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db22.stock
{
    public partial class TAB_MY_STOCK
    {
        /// <summary>
        /// tblStockBatch에 korName 컬럼값
        /// </summary>
        public string k_stock_wanname { get;  set; }

        /// <summary>
        /// tblStockBatch에 stockcode컬럼값
        /// </summary>
        public string k_stock_code { get; set; }

        /// <summary>
        /// tblOnlineSise에 tradePrice 컬럼값
        /// </summary>
        public int now_price { get;  set; }
    }
}
