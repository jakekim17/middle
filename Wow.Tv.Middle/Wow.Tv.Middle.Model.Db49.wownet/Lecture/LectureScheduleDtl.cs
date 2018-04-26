using System.Collections.Generic;

namespace Wow.Tv.Middle.Model.Db49.wownet.Lecture
{
    public class LectureScheduleDtl
    {
        public TAB_LECTURES LectureData { get; set; }
        public List<DtlSchedule> ScheduleList { get; set; }
        public int[] SchDelList { get; set; }
        public int[] LecDelList { get; set; }
    }
}
