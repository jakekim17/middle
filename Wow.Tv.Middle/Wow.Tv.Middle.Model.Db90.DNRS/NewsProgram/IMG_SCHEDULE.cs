using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db90.DNRS
{
    public partial class IMG_SCHEDULE
    {
        public TAB_PROGRAM_LIST ProgramList { get; set; }


        public string ProgramTemplateType { get; set; }
        public string ProgramTemplateName { get; set; }

        public string SERVICE_NAME { get; set; }
        public string ENCODING { get; set; }
        public string PUBLISH_YN { get; set; }
        public string CP_NM { get; set; }
        public string PLAN_BROAD { get; set; }
        public string BroadTypeCode { get; set; }

        public string ParentProgramName { get; set; }
        public string ParentProgramCode { get; set; }
        public int ParentProgramPoint { get; set; }

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


                return result;
            }
        }


        public string StartDate
        {
            get
            {
                string result = "";

                if(String.IsNullOrEmpty(sdate) == false && sdate.Length >= 8)
                {
                    result = sdate.Substring(0,4) + "-" + sdate.Substring(4, 2) + "-" + sdate.Substring(6, 2);
                }

                return result;
            }
        }
        public string EndDate
        {
            get
            {
                string result = "";

                if (String.IsNullOrEmpty(edate) == false && edate.Length >= 8)
                {
                    result = edate.Substring(0, 4) + "-" + edate.Substring(4, 2) + "-" + edate.Substring(6, 2);
                }

                return result;
            }
        }

    }
}
