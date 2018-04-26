using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wownet.Lecture
{
    public class LectureCondition : BaseCondition
    {
        public string ViewSite { get; set; }   
        public string MainCtgr { get; set; }   
        public string TypeFlag { get; set; }   
        public string SearchText { get; set; }  
        public string SearchType { get; set; }
        public string CurrentSite { get; set; } = "Admin";
        public string LecturesDate { get; set; }
    }
}
