using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db90.DNRS.NewsProgram
{
    public class NewsProgramCondition : BaseCondition
    {
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }

        public string ProgramName { get; set; }
        public string ProgramNameTermStart { get; set; }
        public string ProgramNameTermEnd { get; set; }

        public string SearchType { get; set; }
        public string SearchText { get; set; }
        public string PublishYn { get; set; }
        public int CategoryCode { get; set; }
        public string BroadTypeCode { get; set; }
        public string IngYn { get; set; }
        public string PointYn { get; set; }


        public int AdminSeq { get; set; }

        public string FameYn { get; set; }
        public string MainBottomViewYn { get; set; }
        public string AllProgramViewYn { get; set; }
        public string Year { get; set; }

        public string BroadSectionCode { get; set; }

        public string OrderByType { get; set; }
    }
}
