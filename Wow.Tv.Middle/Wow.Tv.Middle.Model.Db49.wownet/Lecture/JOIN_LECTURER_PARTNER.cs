using System;

namespace Wow.Tv.Middle.Model.Db49.wownet.Lecture
{
    public class JOIN_LECTURER_PARTNER
    {
        public int SEQ { get; set; }
        public int MSEQ { get; set; }
        public string LECTURER { get; set; }
        public DateTime REG_DATE { get; set; }
        public string LECTURER_TYPE { get; set; }
        public Nullable<int> PARTNER_NO { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string Pro_id { get; set; }
        public string Wowtv_id { get; set; }
        public string CafeDomain { get; set; }
    }
}