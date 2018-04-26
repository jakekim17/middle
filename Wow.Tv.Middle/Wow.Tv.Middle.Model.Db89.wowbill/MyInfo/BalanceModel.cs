using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db89.wowbill.MyInfo
{
    public class BalanceModel
    {
        public double nBalance { get; set; }
        public string sb_userno        { get; set; }
        public double sb_cashreal      { get; set; }
        public double sb_cashbonus     { get; set; }
        public double sb_mileage       { get; set; }
        public double sb_tincashreal 	{ get; set; }
        public double sb_tincashbonus	{ get; set; }
        public double sb_tinmileage    { get; set; }
        public double sb_toutcashreal	{ get; set; }
        public double sb_toutcashbonus	{ get; set; }
        public double sb_toutmileage   { get; set; }
        public DateTime sb_regdate { get; set; }

    }
}
