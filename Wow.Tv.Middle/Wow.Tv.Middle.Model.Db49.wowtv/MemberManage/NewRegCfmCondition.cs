using System;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.MemberManage
{
    public class NewRegCfmCondition : BaseCondition
    {
        public DateTime? RegistDateFrom { get; set; }
        public DateTime? RegistDateTo { get; set; }
        public string SearchType { get; set; }
        public string SearchText { get; set; }
    }

    public class NewRegCfmResult
    {
        public int Seq { get; set; }
        public string AdminId { get; set; }
        public string AdminName { get; set; }
        public string GroupName { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
        public DateTime? LatestConnectedDate { get; set; }
        public DateTime? RegisteredDate { get; set; }
    }
}
