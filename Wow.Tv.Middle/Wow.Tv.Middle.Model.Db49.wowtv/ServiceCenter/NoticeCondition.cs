using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.ServiceCenter
{
    public class NoticeCondition : BaseCondition
    {
        public string Bcode { get; set; }
        public string ViewFlag { get; set; } = string.Empty;
        public string TopFlag { get; set; } = string.Empty;
        public string SearchFlag { get; set; } = string.Empty;
        public string SearchName { get; set; } = string.Empty;
        public string DelFlag { get; set; } = string.Empty;
    }
}
