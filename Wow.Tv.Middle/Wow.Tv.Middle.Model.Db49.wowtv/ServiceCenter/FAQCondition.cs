using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.ServiceCenter
{

    public class FAQCondition : BaseCondition
    {
        public string Bcode { get; set; }
        public string FAQ_CODE { get; set; } = string.Empty;
        public string VIEW_YN { get; set; } = string.Empty;
        public string SearchFlag { get; set; } = string.Empty;
        public string SearchName { get; set; } = string.Empty;
        public string DEL_YN { get; set; } = string.Empty;
    }
}
