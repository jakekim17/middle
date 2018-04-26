using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wownet.Lecture
{
    public class JOIN_LECTURES_SCHEDULE
    {
        public int SEQ { get; set; }
        public string TYPE_FLAG { get; set; }
        public string VIEW_SITE { get; set; }
        public string VIEW_FLAG { get; set; }
        public string TITLE { get; set; }
        public string CONTENTS { get; set; }
        public int EXPENSE { get; set; }
        public string MANAGER { get; set; }
        public string PHONE { get; set; }
        public string ADMIN_ID { get; set; }
        public Nullable<System.DateTime> REG_DATE { get; set; }
        public string MAIN_CTGR { get; set; }
        public string GUBUN { get; set; }
        public int MSEQ { get; set; }
        public string PLACE { get; set; }
        public string LECTURES_DATE { get; set; }
        public string LECTURES_TIME { get; set; }
        public string LECTURER { get; set; }
        public string LECTURER_TYPE { get; set; }
        public Nullable<int> PARTNER_NO { get; set; }
        public string WG_IMAGE_FILE { get; set; }
        public string THUMNAIL_FILE { get; set; }
    }
}
