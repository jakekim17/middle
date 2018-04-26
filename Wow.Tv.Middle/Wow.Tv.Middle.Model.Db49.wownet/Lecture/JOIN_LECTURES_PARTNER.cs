using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wownet.Lecture
{
    public class JOIN_LECTURES_PARTNER
    {
        public int SEQ { get; set; }
        public int CSEQ { get; set; }
        public string TITLE { get; set; }
        public string MAIN_CTGR { get; set; }
        public string LECTURES_DATE { get; set; }
        public string LECTURES_TIME { get; set; }
        public string LECTURER { get; set; }
        public string LECTURER_TYPE { get; set; }
        public Nullable<int> PARTNER_NO { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
    }
}
