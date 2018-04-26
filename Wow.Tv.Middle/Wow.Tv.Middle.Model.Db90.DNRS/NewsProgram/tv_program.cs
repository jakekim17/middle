using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db90.DNRS
{
    public partial class tv_program
    {
        public int Price { get; set; }

        public string PublishYn { get; set; }

        public string CreateDate { get; set; }
        //{
        //    get
        //    {
        //        string result = "";

        //        if(String.IsNullOrEmpty(UploadTime) == false && UploadTime.Length >= 8)
        //        {
        //            result = UploadTime.Substring(0, 4) + "-" + UploadTime.Substring(4, 2) + "-" + UploadTime.Substring(6, 2);
        //        }

        //        return result;
        //    }
        //}
        public string ModifyDate { get; set; }

        public string BroadDate
        {
            get
            {
                string result = "";

                if(String.IsNullOrEmpty(broad_date) == false && broad_date.Length >= 8)
                {
                    result = broad_date.Substring(0, 4) + "-" + broad_date.Substring(4, 2) + "-" + broad_date.Substring(6, 2);
                }

                return result;
            }
        }
        public string BroadHour
        {
            get
            {
                string result = "";

                if (String.IsNullOrEmpty(broad_date) == false && broad_date.Length >= 10)
                {
                    result = broad_date.Substring(8, 2);
                }

                return result;
            }
        }
        public string BroadMinute
        {
            get
            {
                string result = "";

                if (String.IsNullOrEmpty(broad_date) == false && broad_date.Length >= 12)
                {
                    result = broad_date.Substring(10, 2);
                }

                return result;
            }
        }

        public System.Collections.Generic.List<string> DayOfWeekString { get; set; }
        public string broadStart { get; set; }
        public string SubImageUrl { get; set; }
        public string RectangleImageUrl { get; set; }
        public string ThumImageUrl { get; set; }

        public string frontTimeString
        {
            get
            {
                string result = BroadDate;

                if (String.IsNullOrEmpty(result) == false)
                {
                    if (String.IsNullOrEmpty(broad_date) == false && broad_date.Length >= 12)
                    {
                        DateTime dtBroad = new DateTime(int.Parse(broad_date.Substring(0, 4)), int.Parse(broad_date.Substring(4, 2)), int.Parse(broad_date.Substring(6, 2))
                            , int.Parse(broad_date.Substring(8, 2)), int.Parse(broad_date.Substring(10, 2)), 0);

                        TimeSpan sp = DateTime.Now - dtBroad;
                        if(sp.TotalHours < 1)
                        {
                            result = sp.TotalMinutes.ToString("N0") + "분전";
                        }
                        else if (sp.TotalHours < 24)
                        {
                            result = sp.TotalHours.ToString("N0") + "시간전";
                        }
                    }
                }

                return result;
            }
        }
        public bool IsOneDayInner
        {
            get
            {
                bool result = false;

                if (String.IsNullOrEmpty(broad_date) == false && broad_date.Length >= 12)
                {
                    DateTime dtBroad = new DateTime(int.Parse(broad_date.Substring(0, 4)), int.Parse(broad_date.Substring(4, 2)), int.Parse(broad_date.Substring(6, 2))
                        , int.Parse(broad_date.Substring(8, 2)), int.Parse(broad_date.Substring(10, 2)), 0);

                    TimeSpan sp = DateTime.Now - dtBroad;
                    if (sp.TotalHours < 24)
                    {
                        result = true;
                    }
                }

                return result;
            }
        }
    }
}
