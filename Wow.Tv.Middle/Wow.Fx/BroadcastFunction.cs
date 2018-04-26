using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Fx
{
    public class BroadcastFunction
    {
        public static string GetInvestClass(string invest)
        {
            switch (invest.Trim())
            {
                case "1": return "단기-급등주";
                case "2": return "단기-개별주";
                case "3": return "단기-테마주";
                case "4": return "단기-주도주";
                case "5": return "단기-가치주";
                case "6": return "단기-우량주";
                case "7": return "스윙-급등주";
                case "8": return "스윙-개별주";
                case "9": return "스윙-테마주";
                case "10": return "스윙-주도주";
                case "11": return "스윙-가치주";
                case "12": return "스윙-우량주";
                case "13": return "중기-테마주";
                case "14": return "중기-주도주";
                case "15": return "중기-가치주";
                case "16": return "중기-우량주";
                case "17": return "선물/옵션";
                case "18": return "장기-테마주";
                case "19": return "장기-주도주";
                case "20": return "장기-가치주";
                case "21": return "장기-우량주";
                case "22": return "스윙-재료주";
                case "23": return "중기-ELW";
                case "24": return "단/중기-주도주";
                case "25": return "스윙-ELW";
                case "26": return "FX거래";
                case "27": return "중기-스윙";
                case "28": return "주식-선물";
                case "29": return "해외선물";
                case "30": return "장외";
                case "31": return "공모주";
                default: return "";

            }

        }


        public static string GetBroadIconAndLink(string uploadWebPath, string strBPlayType, string strBMemType, int? state, string proID, string strRoom, string strPriceID)
        {
            if (string.IsNullOrWhiteSpace(strBMemType))
                strBMemType = "";
            if (state == null)
                return "";

            string link = "";

            if (!string.IsNullOrEmpty(uploadWebPath))
            {
                link = GetBroadIcon(uploadWebPath, strBMemType, state);
            
                if (state == 0 || state == 2)
                {
                    return link;
                }
            }
            

            string broadLink;
            if (strBPlayType.Equals("EST"))
            {
                broadLink = $"javascript:Messenger_Open('{strRoom}','{strPriceID}','{strBMemType}','L');";
            }
            else if (strBPlayType.Equals("ELV"))
            {
                broadLink = $"javascript:Pro_Open2('{strRoom.Trim()}','{strPriceID}','{strBMemType}','L');";
            }
            else
            {
                if (strBMemType.Equals("F") && !proID.Equals("P724"))
                {
                    broadLink = $"javascript:Pro_Open_Cast('{strRoom.Trim()}','{strPriceID}','{strBMemType}','L','net');";
                }
                else
                {
                    broadLink = $"javascript:Pro_Open('{strRoom.Trim()}','{strPriceID}','{strBMemType}','L');";

                }

            }

            if(string.IsNullOrEmpty(uploadWebPath))
            {
                if(link == "")
                {
                    link = broadLink;
                }
                else
                {
                    link = $"<a href=\"{broadLink}\" class=\"link ico-area\">{link}</a>";
                }
                
            }
            else
            {
                link = $"<a href=\"{broadLink}\" class=\"link ico-area\">{link}</a>";
            }

            return link;
        }

        public static string GetBroadIcon(string uploadWebPath, string strBMemType, int? state)
        {
            string tmpImgLink = "";
            string iconOffImage = "";
            string iconOnImage = "";
            string iconOffAlt = "";
            string iconOnAlt = "";


            if (strBMemType.Substring(0, 1).Equals("N") || strBMemType.Equals("F4"))
            {

                iconOffImage = uploadWebPath + "res/images/common/ico_state_small_broadcast_membership_new.gif";
                iconOnImage = uploadWebPath + "res/images/common/ico_state_small_broadcast_membership_onnew.gif";
                iconOffAlt = "회원전용/방송예정";
                iconOnAlt = "회원전용/방송중";
            }
            else if (strBMemType.Equals("U"))
            {
                iconOffImage = uploadWebPath + "res/images/common/ico_state_small_broadcast_membership_bynew.gif";
                iconOnImage = uploadWebPath + "res/images/common/ico_state_small_broadcast_membership_onbynew.gif";
                iconOffAlt = "건당결제/방송예정";
                iconOnAlt = "건당결제/방송중";
            }
            else
            {
                iconOffImage = uploadWebPath + "res/images/common/ico_state_small_broadcast_nomembership_new.gif";
                iconOnImage = uploadWebPath + "res/images/common/ico_state_small_broadcast_nomembership_onnew.gif";
                iconOffAlt = "무료방송/방송예정";
                iconOnAlt = "무료방송/방송중";
            }

            switch (state)
            {
                case 0:
                    tmpImgLink = $"<img src='{iconOffImage}' alt='{iconOffAlt}'/>";
                    break;
                case 2:
                    tmpImgLink = $"<img src='{uploadWebPath}/res/images/common/ico_state_small_broadcast_filmed.gif' alt='무료방송/녹화방송'/>";
                    break;
                case 1:
                case 3:
                case 4:
                    tmpImgLink = $"<img src='{iconOnImage}' alt='{iconOnAlt}'/>";
                    break;
            }
            return tmpImgLink;
        }


        /// <summary>
        /// 파트너 개별 홈 링크
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static string GetIntroduceLink(string proId)
        {
            switch (proId)
            {
                case "HI626":
                case "P007":
                    return "http://wowpro.wownet.co.kr/eventhome/event_main.asp?bcode=&mseq=&strpkid=20";
                case "P043":
                    return "http://wowpro.wownet.co.kr/pro/pro_main_new/sub_main.asp?proID=P111";
                case "P080":
                    return "http://wowpro.wownet.co.kr/pro/pro_main_new/editlist.asp?ptype=51&idx=161&proID=P079";
                default:
                    return "http://wowpro.wownet.co.kr/pro/pro_main_new/sub_main.asp?proId=" + proId;
            }
        }

        public static string GetJoinLink(string proId)
        {
            if (proId.Equals("HI626") || proId.Equals("P007"))
            {
                return "https://www.wownet.co.kr/eventhome/profile.asp?strpkid=20&proID=HI626&lcode=303";
            }
            if (proId.Equals("P074"))
            {
                return "https://www.wownet.co.kr//pro/pro_main_new/profile.asp?proID=P078&ptype=11";
            }
            if (proId.Equals("P254") || proId.Equals("P366") || proId.Equals("P303"))
            {
                return "http://wowpro.wownet.co.kr/pro/pro_main_new/profile.asp?proID=P234&ptype=11";
            }
            if (proId.Equals("P248") || proId.Equals("P043"))
            {
                return "http://wowpro.wownet.co.kr/pro/pro_main_new/profile.asp?proID=P111&ptype=11";
            }
            if (proId.Equals("P700") || proId.Equals("P122") || proId.Equals("P362"))
            {
                return "http://wowpro.wownet.co.kr/pro/pro_main_new/profile.asp?ptype=11&proID=P232";
            }
            if (proId.Equals("P080"))
            {
                return "http://wowpro.wownet.co.kr/pro/pro_main_new/editlist.asp?ptype=51&idx=161&proID=P079";
            }

            return "http://wowpro.wownet.co.kr/pro/pro_main_new/profile.asp?proID=" + proId + "&amp;ptype=11";

        }

//        ' 회원가입 링크
//Function getJoinLink(proID)

//    If proID = "HI626" OR proID = "P007" Then ' 홀짝박사 (P007은 세컨 아이디임)
//        'getJoinLink = "/eventhome/event_main.asp?strpkid=20"
//        getJoinLink = "/eventhome/profile.asp?strpkid=20&proID=HI626&lcode=303"
//    'ElseIf proID = "P074" OR proID = "P078" OR proID = "P079" OR proID = "P080" Then    ' 김종철소장
//    '    getJoinLink = "/pro/pro_main_new/profile.asp?proID=P074&amp;ptype=11"
//	'ElseIf proID = "P724"  Then '평생사부
//	'	getJoinLink = "/pro/pro_main_new/past/pro_join_info.asp?bcode=N52020100&mseq=1066&proID=P724"
//'	ElseIf proID = "P043" Then ' 명성욱
//'        getJoinLink = "/eventhome/event_main.asp?strpkid=5&bcode=N82000000&mseq=1772"
//'    ElseIf proID = "P084" Then ' 샤프슈터
//'        getJoinLink = "/eventhome/profile.asp?strpkid=24&proID=P084&lcode=267"
//	ElseIf proID = "P074" Then ' 김종철소장
//        getJoinLink = "/pro/pro_main_new/profile.asp?proID=P078&ptype=11"
//    ElseIf proID = "P254" OR proID = "P366" OR proID = "P303"   Then    ' 출동반
//        getJoinLink = "http://wowpro.wownet.co.kr/pro/pro_main_new/profile.asp?proID=P234&ptype=11"
//    ElseIf proID = "P248" OR proID = "P043" Then                                        ' 전투단
//        getJoinLink = "http://wowpro.wownet.co.kr/pro/pro_main_new/profile.asp?proID=P111&ptype=11"
//    ElseIf proID = "P700" OR proID = "P122" OR proID = "P362" Then                      ' 대박드림팀
//        getJoinLink = "http://wowpro.wownet.co.kr/pro/pro_main_new/profile.asp?ptype=11&proID=P232"
//    'ElseIf proID = "P138" OR proID = "P705"	OR proID = "P294" Then                      ' 베테랑S
//    '    getJoinLink = "http://wowpro.wownet.co.kr/pro/pro_main_new/profile.asp?ptype=11&proID=P237"
//    ElseIf proID = "P080" Then                                                          ' 김종철소장
//        getJoinLink = "http://wowpro.wownet.co.kr/pro/pro_main_new/editlist.asp?ptype=51&idx=161&proID=P079"
//    Else
//        getJoinLink = "http://wowpro.wownet.co.kr/pro/pro_main_new/profile.asp?proID=" & proID & "&amp;ptype=11"
//    End If

//End Function
    }
}
