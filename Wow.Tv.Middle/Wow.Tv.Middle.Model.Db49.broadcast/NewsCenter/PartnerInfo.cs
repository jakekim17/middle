using System.Collections.Generic;

namespace Wow.Tv.Middle.Model.Db49.broadcast.NewsCenter
{
    public partial class PartnerInfo
    {
        public Pro_wowList ProWowList { get; set; }
        public USP_GetBroadcast1ByProId_Result BroadState { get; set; }
        public string HtmlTag { get; set; }
        public string BroadCastTime { get; set; }
        public string BoradType { get; set; }
    }
}
