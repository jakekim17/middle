using System;
using System.Collections.Generic;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;


namespace Wow.Tv.Middle.Model.Db49.wownet.ServiceCenter
{
    public class EventModel<T> : ListModel<T>
    {
        public List<NTB_COMMON_CODE> CodeList { get; set; }
        public tblEvent EventData { get; set; }
        public List<EventContent> EventList{ get; set; }
        public EventContent FrontData { get; set; }
        public BroadModel BroadData { get; set; }
        public String ProImage { get; set; }
    }
}
