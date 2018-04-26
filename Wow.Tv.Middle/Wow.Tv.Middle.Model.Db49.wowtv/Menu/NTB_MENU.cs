using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv
{
    public partial class NTB_MENU
    {
        public string BoardTypeCode { get; set; }
        public string ContentName { get; set; }
        public string sccode { get; set; }

        public string Href
        {
            get
            {
                string result = "";

                if((String.IsNullOrEmpty(CONTENT_TYPE_CODE) == true || CONTENT_TYPE_CODE == "Page") 
                    && LINK_URL != null && LINK_URL.ToLower() == "javascript:void(0);")
                {
                    result = LINK_URL;
                }
                else
                {
                    if (CONTENT_TYPE_CODE == "Board")
                    {
                        switch (BoardTypeCode)
                        {
                            case "Notice":
                                result = "/IntegratedBoard/Notice/Index" + "?menuSeq=" + MENU_SEQ;
                                break;
                            case "Normal":
                                result = "/IntegratedBoard/Basic/Index" + "?menuSeq=" + MENU_SEQ;
                                break;
                            case "Official":
                                result = "/IntegratedBoard/Official/Index" + "?menuSeq=" + MENU_SEQ;
                                break;
                            case "FAQ":
                                result = "/IntegratedBoard/FAQ/Index" + "?menuSeq=" + MENU_SEQ;
                                break;
                            case "Inquiry":
                                result = "/IntegratedBoard/Inquiry/Index" + "?menuSeq=" + MENU_SEQ;
                                break;
                            case "FeedBack":
                                result = "/IntegratedBoard/FeedBack/Index" + "?menuSeq=" + MENU_SEQ;
                                break;
                        }
                    }
                    else if (CONTENT_TYPE_CODE == "Html")
                    {
                        result = "/BusinessGuide/Business/Index?menuSeq=" + MENU_SEQ;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(LINK_URL) == false)
                        {
                            if (LINK_URL.IndexOf("?") >= 0)
                            {
                                result = LINK_URL + "&menuSeq=" + MENU_SEQ;
                            }
                            else
                            {
                                result = LINK_URL + "?menuSeq=" + MENU_SEQ;
                            }
                        }
                    }
                }

                return result;
            }
        }


        public string HrefProgram
        {
            get
            {
                string result = "";

                if (LINK_URL != null && LINK_URL.ToLower() == "javascript:void(0);")
                {
                    result = LINK_URL;
                }
                else
                {
                    if (CONTENT_TYPE_CODE == "Board")
                    {
                        switch (BoardTypeCode)
                        {
                            case "Notice":
                                result = "/ProgramBoard/Notice/Index" + "?menuSeq=" + MENU_SEQ + "&ProgramCode=" + PRG_CD;
                                break;
                            case "Normal":
                                result = "/ProgramBoard/Basic/Index" + "?menuSeq=" + MENU_SEQ + "&ProgramCode=" + PRG_CD;
                                break;
                            case "Official":
                                result = "/ProgramBoard/Official/Index" + "?menuSeq=" + MENU_SEQ + "&ProgramCode=" + PRG_CD;
                                break;
                            case "FAQ":
                                result = "/ProgramBoard/FAQ/Index" + "?menuSeq=" + MENU_SEQ + "&ProgramCode=" + PRG_CD;
                                break;
                            case "Inquiry":
                                result = "/ProgramBoard/Inquiry/Index" + "?menuSeq=" + MENU_SEQ + "&ProgramCode=" + PRG_CD;
                                break;
                            case "FeedBack":
                                result = "/ProgramBoard/FeedBack/Index" + "?menuSeq=" + MENU_SEQ + "&ProgramCode=" + PRG_CD;
                                break;
                        }
                    }
                    else
                    {
                        result = LINK_URL + "?menuSeq=" + MENU_SEQ + "&ProgramCode=" + PRG_CD;
                    }
                }

                return result;
            }
        }
    }
}
