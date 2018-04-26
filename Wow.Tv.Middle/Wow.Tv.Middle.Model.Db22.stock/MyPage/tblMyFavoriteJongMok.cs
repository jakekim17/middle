using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db22.stock
{
    public partial class tblMyFavoriteJongMok
    {
        public string stock_code { get; set; }
        public string Groupid { get; set; }
        public string part_code1 { get; set; }
        public string part_code2 { get; set; }
        public string stock_nickname { get; set; }
        public string stock_wanname { get; set; }
        public string arj_code { get; set; }
        public int? curr_price { get; set; }
        public int? init_price { get; set; }
        public int? high_price { get; set; }
        public int? low_price { get; set; }
        public long? net_vol { get; set; }
        public long? net_turnover { get; set; }
        public string chg_type { get; set; }
        public int? final_price { get; set; }
        public int? list_price { get; set; }
        public int? net_chg { get; set; }
        public string mkt_halt { get; set; }
        public string data_day { get; set; }
        public long? list_num { get; set; }
        public long? list_sum { get; set; }
        public int? highest_price { get; set; }
        public int? lowest_price { get; set; }
        public int? capital { get; set; }
        public int? banner { get; set; }
        public string k_gbn { get; set; }
        public string gubun { get; set; }
        public long orgbuy { get; set; }
        public long foreinerbuy { get; set; }
    }
}
