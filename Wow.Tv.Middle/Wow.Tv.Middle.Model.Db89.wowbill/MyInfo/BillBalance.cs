using System;

namespace Wow.Tv.Middle.Model.Db89.wowbill.MyInfo
{
    public class BillBalance
    {
        public double nBalance { get; set; }
        public string sb_userno { get; set; }
        public double sb_cashreal { get; set; }
        public double sb_cashbonus { get; set; }
        public double sb_mileage { get; set; }
        public double sb_tincashreal { get; set; }
        public double sb_tincashbonus { get; set; }
        public double sb_tinmileage { get; set; }
        public double sb_toutcashreal { get; set; }
        public double sb_toutcashbonus { get; set; }
        public double sb_toutmileage { get; set; }
        public double EventBalance { get; set; } = 0;
        public DateTime sb_regdate { get; set; }
    }
}