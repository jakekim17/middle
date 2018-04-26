
using System;

namespace Wow.Tv.Middle.Model.Db49.wownet.ServiceCenter
{
    public class usp_tblEvent_select_Result
    {
        
        public int Seq { set; get; }
        public String ViewSite { set; get; }
        public String EventFlag { set; get; }
        public String Title { set; get; }
        public String NickName { set; get; }
        public String ProId { set; get; }
        public String WowtvId { set; get; }
        public String ProductId { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public DateTime RegDate { set; get; }
        public int? ReadCount { set; get; }
        public String ViewFlag { set; get; }
        public String Content { set; get; }
        public String BannerImage { set; get; }
        public String LinkUrl { set; get; }
        public Nullable<DateTime> WinnerDate { set; get; }
        public String WinViewFlag { set; get; }
        public String WinContent { set; get; }
        public String CodeName { set; get; }
        public String UpCommonCode { set; get; }
        public string NEWphoto_small2 { get; set; }
    }
}