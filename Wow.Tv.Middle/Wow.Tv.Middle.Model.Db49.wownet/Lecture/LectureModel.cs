using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wownet.Lecture
{
    public class LectureModel<T> : ListModel<T> 
    {
        public List<JOIN_LECTURES_PARTNER> AMonthList { get; set; }
    }
}
