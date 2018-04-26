namespace Wow.Tv.Middle.Model.Db90.DNRS
{
    public partial class T_RUNDOWN
    {
        public string TimePx { get; set; }
        public bool IsFirst { get; set; }
        public bool IsRenewal { get; set; }
        public System.Collections.Generic.List<string> DayOfWeekString { get; set; }
        public string BroadWatchStatus { get; set; }
        public string Status { get; set; }
        public string SUB_IMG { get; set; }
        public string REC_IMG { get; set; }

        public System.Collections.Generic.List<string> CastNameList { get; set; }
    }
}