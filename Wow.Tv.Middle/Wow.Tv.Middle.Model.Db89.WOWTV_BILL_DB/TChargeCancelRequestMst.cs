//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 템플릿에서 생성되었습니다.
//
//     이 파일을 수동으로 변경하면 응용 프로그램에서 예기치 않은 동작이 발생할 수 있습니다.
//     이 파일을 수동으로 변경하면 코드가 다시 생성될 때 변경 내용을 덮어씁니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class TChargeCancelRequestMst
    {
        public int SeqNo { get; set; }
        public int UserNo { get; set; }
        public long ChargeNo { get; set; }
        public Nullable<int> DayPrice { get; set; }
        public Nullable<int> UsedDay { get; set; }
        public Nullable<int> CancelFee { get; set; }
        public Nullable<byte> CancelFlag { get; set; }
        public string AdminID { get; set; }
        public string CallTime { get; set; }
        public string CallMsg { get; set; }
        public byte UseState { get; set; }
        public string RetMsg { get; set; }
        public Nullable<System.DateTime> UpdDate { get; set; }
        public System.DateTime RegDate { get; set; }
        public string AcctBank { get; set; }
        public string AcctNo { get; set; }
    }
}