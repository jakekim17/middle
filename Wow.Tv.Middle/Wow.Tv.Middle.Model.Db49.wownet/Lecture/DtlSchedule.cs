using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wownet.Lecture
{
    public class DtlSchedule
    {
        public int SEQ { get; set; }
        public int MSEQ { get; set; }
        public string PLACE { get; set; }
        public string LECTURES_DATE { get; set; }
        public string LECTURES_TIME { get; set; }
        public string ETC { get; set; }
        //public TAB_LECTURES_SCHEDULE ScheduleData { get; set; }
        public List<JOIN_LECTURER_PARTNER> LecturerList { get; set; }
    }
}
