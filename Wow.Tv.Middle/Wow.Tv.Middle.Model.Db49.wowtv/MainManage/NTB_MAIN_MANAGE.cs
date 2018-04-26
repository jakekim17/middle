using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv
{
    public partial class NTB_MAIN_MANAGE
    {
        public NTB_ATTACH_FILE AttachFile { get; set; }

        public int PublishStartHour { get; set; }
        public int PublishStartMinute { get; set; }
        public int PublishEndHour { get; set; }
        public int PublishEndMinute { get; set; }

    }
}
