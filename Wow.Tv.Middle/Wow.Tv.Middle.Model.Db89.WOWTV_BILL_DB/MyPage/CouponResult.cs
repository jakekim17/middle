using System;

namespace Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage
{
    public class CouponResult
    {
        public string CouponType { get;  set; }
        public int CouponId { get; set; }
        public string CouponNo { get; set; }
        public int CouponGroup { get; set; }
        public string Gubun { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Reason { get; set; }
        public int ApplyDetail { get; set; }
        public string ResultMessage { get; set; }
    }
}