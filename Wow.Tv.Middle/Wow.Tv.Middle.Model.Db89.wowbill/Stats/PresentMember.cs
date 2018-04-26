using System;

namespace Wow.Tv.Middle.Model.Db89.wowbill
{
    [Serializable]
    public class PresentMember
    {
        public int TotalMember { get; set; } = 0;
        public int JoinMember { get; set; } = 0;
        public int WebToonMember { get; set; } = 0;
        public int QuitMember { get; set; } = 0;
    }
}