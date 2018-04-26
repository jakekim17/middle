using System;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.MemberManage
{
    public class MemInfoOpenChkCondition : BaseCondition
    {
        public DateTime? RegistDateFrom { get; set; }
        public DateTime? RegistDateTo { get; set; }
        public string SearchType { get; set; }
        public string SearchText { get; set; }
    }

    public class MemInfoOpenChkResult
    {
        public int Seq { get; set; }
        public string AdminId { get; set; }
        public string OpenDate { get; set; }
        public string IpAddress { get; set; }
        public int OpenCount { get; set; }
        public int LatestActionLogSeq { get; set; }
        public DateTime? LatestOpenDate { get; set; }
    }
}
