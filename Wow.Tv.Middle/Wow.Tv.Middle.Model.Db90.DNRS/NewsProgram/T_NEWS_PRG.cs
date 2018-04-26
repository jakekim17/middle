using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Wow.Tv.Middle.Model.Db90.DNRS
{
    public partial class T_NEWS_PRG
    {
        public IMG_SCHEDULE ImgSchedule { get; set; }
        public TAB_PROGRAM_LIST ProgramList { get; set; }


        public string ProgramTemplateType { get; set; }
        public string ProgramTemplateName { get; set; }


        public string SERVICE_NAME { get; set; }
        public string ENCODING { get; set; }
        public string PUBLISH_YN { get; set; }
        public string CP_NM { get; set; }
        public string PLAN_BROAD { get; set; }
        public string BroadTypeCode { get; set; }
        public string FameYn { get; set; }
        public string FirstFreeYn { get; set; }
        public string MainViewType { get; set; }
        public string MainBottomViewYn { get; set; }
        public string AllProgramViewYn { get; set; }

        public string CategoryCode1 { get; set; }
        public string CategoryCode2 { get; set; }
        public string CategoryCode3 { get; set; }


        public bool IsFirst { get; set; }
        public bool IsRenewal { get; set; }
        public System.Collections.Generic.List<string> DayOfWeekString { get; set; }
        public System.Collections.Generic.List<string> PartnerNameList { get; set; }
        public string NaverReadYn { get; set; }
        public string NaverReadUrl { get; set; }
        public string YouTubeReadYn { get; set; }
        public string YouTubeReadUrl { get; set; }

        public string BroadSectionCode { get; set; }

        public string ThumImageUrl { get; set; }

        public string BroadTypeCodeName
        {
            get
            {
                string result = "";

                if (BroadTypeCode == "Proc")
                {
                    result = "장중";
                }
                else if (BroadTypeCode == "After")
                {
                    result = "장후";
                }
                else if (BroadTypeCode == "Spec")
                {
                    result = "특집";
                }

                //if (BRO_START.CompareTo("09:00") >= 0 && BRO_START.CompareTo("15:00") <= 0)
                //{
                //    result = "장중";
                //}
                //if (BRO_START.CompareTo("09:00") < 0 || BRO_START.CompareTo("15:00") > 0)
                //{
                //    result = "장후";
                //}
                //if(ProgramList != null && ProgramList.SPC_YN == "Y")
                //{
                //    result = "특집";
                //}


                return result;
            }
        }


        public string BroadTypeCode_Time
        {
            get
            {
                string result = "";

                if (BRO_START.CompareTo("09:00") >= 0 && BRO_START.CompareTo("15:00") <= 0)
                {
                    result = "Proc";
                }
                if (BRO_START.CompareTo("09:00") < 0 || BRO_START.CompareTo("15:00") > 0)
                {
                    result = "After";
                }
                if (ProgramList != null && ProgramList.SPC_YN == "Y")
                {
                    result = "Spec";
                }


                return result;
            }
        }

        public int StartHour
        {
            get
            {
                int result = 0;

                if(String.IsNullOrEmpty(BRO_START) == false)
                {
                    string[] dim = BRO_START.Split(':');
                    if(dim.Length == 2)
                    {
                        int.TryParse(dim[0], out result);
                    }
                }

                return result;
            }
        }

        public int StartMinute
        {
            get
            {
                int result = 0;

                if (String.IsNullOrEmpty(BRO_START) == false)
                {
                    string[] dim = BRO_START.Split(':');
                    if (dim.Length == 2)
                    {
                        int.TryParse(dim[1], out result);
                    }
                }

                return result;
            }
        }


        public int EndHour
        {
            get
            {
                int result = 0;

                if (String.IsNullOrEmpty(BRO_END) == false)
                {
                    string[] dim = BRO_END.Split(':');
                    if (dim.Length == 2)
                    {
                        int.TryParse(dim[0], out result);
                    }
                }

                return result;
            }
        }

        public int EndMinute
        {
            get
            {
                int result = 0;

                if (String.IsNullOrEmpty(BRO_END) == false)
                {
                    string[] dim = BRO_END.Split(':');
                    if (dim.Length == 2)
                    {
                        int.TryParse(dim[1], out result);
                    }
                }

                return result;
            }
        }




        public bool IsMonday
        {
            get
            {
                bool result = false;

                if((PGMDAY & 1) > 0)
                {
                    result = true;
                }
                return result;
            }
        }
        public bool IsTuesday
        {
            get
            {
                bool result = false;

                if ((PGMDAY & 16) > 0)
                {
                    result = true;
                }
                return result;
            }
        }
        public bool IsWednesday
        {
            get
            {
                bool result = false;

                if ((PGMDAY & 256) > 0)
                {
                    result = true;
                }
                return result;
            }
        }
        public bool IsThursday
        {
            get
            {
                bool result = false;

                if ((PGMDAY & 4096) > 0)
                {
                    result = true;
                }
                return result;
            }
        }
        public bool IsFriday
        {
            get
            {
                bool result = false;

                if ((PGMDAY & 65536) > 0)
                {
                    result = true;
                }
                return result;
            }
        }
        public bool IsSaturday
        {
            get
            {
                bool result = false;

                if ((PGMDAY & 1048576) > 0)
                {
                    result = true;
                }
                return result;
            }
        }
        public bool IsSunday
        {
            get
            {
                bool result = false;

                if ((PGMDAY & 16777216) > 0)
                {
                    result = true;
                }
                return result;
            }
        }


        public string GetPdTdNames(string separator)
        {
            List<string> list = new List<string>();

            if (String.IsNullOrEmpty(PD_NM) == false)
            {
                list.Add(PD_NM);
            }
            if (String.IsNullOrEmpty(PD2_NM) == false)
            {
                list.Add(PD2_NM);
            }
            if (String.IsNullOrEmpty(TD_NM) == false)
            {
                list.Add(TD_NM);
            }
            if (String.IsNullOrEmpty(TD2_NM) == false)
            {
                list.Add(TD2_NM);
            }
            if (String.IsNullOrEmpty(CP_NM) == false)
            {
                list.Add(CP_NM);
            }

            return String.Join(separator, list);
        }
    }
}
