using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.History
{
    public class HistoryCondition : BaseCondition
    {
        public int CTGYR { get; set; }
        public string DispYN { get; set; }
        public string SearchType { get; set; }
        public string SearchText { get; set; }
        public string Orderby { get; set; } = "ASC";
    }
}
