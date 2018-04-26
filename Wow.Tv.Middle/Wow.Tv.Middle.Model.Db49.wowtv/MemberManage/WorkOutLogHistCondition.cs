using System;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.MemberManage
{
    public class WorkOutLogHistCondition : BaseCondition
    {
        public DateTime? RegistDateFrom { get; set; }
        public DateTime? RegistDateTo { get; set; }
        public string SearchType { get; set; }
        public string SearchText { get; set; }
    }

    public class WorkOutLogHistResult
    {
        public int MenuSeq { get; set; }
        public string MenuName { get; set; }
        public int ActionSeq { get; set; }
        public string ActionCode { get; set; }
        public string ActionType { get; set; }
        public string TableKey { get; set; }
        public string ActionDescription { get; set; }
        public string IpAddress { get; set; }
        public string AdminId { get; set; }
        public DateTime? ActionDate { get; set; }
    }
}
