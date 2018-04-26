using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Wow.Fx;
using Wow.Tv.Middle.Biz.Component;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db51.ARSsms;
using Wow.Tv.Middle.Model.Db51.ems50;
using Wow.Tv.Middle.Model.Db51.WOWMMS;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.Member;

namespace Wow.Tv.Middle.Biz.Member
{
    public class JoinBiz : BaseBiz
    {
        private MemberCommonBiz commonBiz = null;
        public JoinBiz()
        {
            commonBiz = new MemberCommonBiz();
        }

        /// <summary>
        /// 회원가입 가능여부 > 아이핀 체크
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JoinCheckIpinResult JoinCheckIpin(IpinCheckRequest request)
        {
            JoinCheckIpinResult retval = new JoinCheckIpinResult();

            AuthBiz authBiz = new AuthBiz();
            retval.IpinCheckResult = authBiz.IpinCheck(request);

            if (retval.IpinCheckResult.ReturnCode == 1) // 인증성공
            {
                // 특정 사용자 회원가입 불가처리
                List<TblMemberDIDenialList> denialList = db89_wowbill.TblMemberDIDenialList.Where(a => a.DupInfo == retval.IpinCheckResult.DupInfo).ToList();
                if (denialList.Count == 0)
                {
                    retval.IsDenial = false;
                    retval.DenialText = "";
                }
                else
                {
                    retval.IsDenial = true;
                    retval.DenialText = denialList[0].AlertText;
                }

                // 14세 이하 가입불가처리
                int nowYear = DateTime.Now.Year;
                int birthYear;
                if (int.TryParse(retval.IpinCheckResult.BirthDate.Substring(0, 4), out birthYear) == true)
                {
                    int userAge = nowYear - birthYear;
                    retval.AgeRestriction = userAge < 14 ? true : false;
                }

                // SMS/아이핀 정보 확인
                int? ipinUserNumber = commonBiz.GetDupInfoUserNumber(retval.IpinCheckResult.DupInfo);
                if (ipinUserNumber.HasValue == true)
                {
                    tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == ipinUserNumber.Value).SingleOrDefault();
                    if (userInfo != null)
                    {
                        retval.Registered = true;
                        retval.RegisteredUserID = userInfo.userId;
                        retval.RegisteredDate = userInfo.registDt;
                    }
                }
                else
                {
                    retval.Registered = false;
                }
            }

            authBiz.MemberIdSearchLog(retval.IpinCheckResult.VirtualNumber, retval.IpinCheckResult.Name, retval.IpinCheckResult.DupInfo, "", "", "IPIN_REG");

            return retval;
        }

        /// <summary>
        /// 회원가입 가능여부 > 휴대폰인증 체크
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JoinCheckSmsResult JoinCheckSms(SmsCheckRequest request)
        {
            JoinCheckSmsResult retval = new JoinCheckSmsResult();

            AuthBiz authBiz = new AuthBiz();
            retval.SmsCheckResult = authBiz.SmsCheck(request);

            if (retval.SmsCheckResult.ReturnCode == 0)
            {
                // 특정 사용자 회원가입 불가처리
                TblMemberDIDenialList denialInfo = db89_wowbill.TblMemberDIDenialList.SingleOrDefault(a => a.DupInfo == retval.SmsCheckResult.DI);
                if (denialInfo == null)
                {
                    retval.IsDenial = false;
                    retval.DenialText = "";
                }
                else
                {
                    retval.IsDenial = true;
                    retval.DenialText = denialInfo.AlertText;
                }

                // 14세 이하 가입불가처리
                int nowYear = DateTime.Now.Year;
                int birthYear;
                if (int.TryParse(retval.SmsCheckResult.BirthDate.Substring(0, 4), out birthYear) == true)
                {
                    int userAge = nowYear - birthYear;
                    retval.AgeRestriction = userAge < 14 ? true : false;
                }

                // SMS/아이핀 정보 확인
                retval.Registered = false;
                int? ipinUserNumber = commonBiz.GetDupInfoUserNumber(retval.SmsCheckResult.DI);
                if (ipinUserNumber.HasValue == true)
                {
                    tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == ipinUserNumber.Value).SingleOrDefault();
                    if (userInfo != null)
                    {
                        retval.Registered = true;
                        retval.RegisteredUserID = userInfo.userId;
                        retval.RegisteredDate = userInfo.registDt;
                    }
                }
            }

			authBiz.MemberIdSearchLog("", retval.SmsCheckResult.Name, retval.SmsCheckResult.DI, request.RequestNo, request.RequestNo, "MOBILE_REG");

			return retval;
        }

        /// <summary>
        /// 회원가입 가능여부 > 외국인번호 체크
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ssno1"></param>
        /// <param name="ssno2"></param>
        /// <param name="cbNameSiteCode"></param>
        /// <param name="cbNameSitePassword"></param>
        /// <returns></returns>
        public JoinCheckForeignResult JoinCheckForeign(string name, string ssno1, string ssno2, string cbNameSiteCode, string cbNameSitePassword)
        {
            JoinCheckForeignResult retval = new JoinCheckForeignResult();

            // 외국인번호 검증
            CB_NAMELib.NameChkClass nameCheck = new CB_NAMELib.NameChkClass();
            int nameCheckReturnCode = nameCheck.fnNameChk(cbNameSiteCode, cbNameSitePassword, ssno1 + ssno2, name, 21, 5);

            retval.NameCheckReturnCode = nameCheckReturnCode;
            if (nameCheckReturnCode == 1)
            {
                retval.NameCheckSuccess = true;
            }
            else
            {
                retval.NameCheckSuccess = false;
            }

            // 회원가입정보 검색
            string ssno = ssno1 + ssno2;
            string encSSNo = commonBiz.Encrypt("GENERAL_ENC", "NORMAL", ssno);
            //string encSSNo = XdbCrypto.Encrypt(ssno);
            //ObjectResult<string> encSSNoSelect = db89_wowbill.NUP_ENC_SELECT("GENERAL_ENC", "NORMAL", ssno);
            //string encSSNo = encSSNoSelect.FirstOrDefault();

            tblUser userInfo = db89_wowbill.tblUser.SingleOrDefault(a => a.ssno == encSSNo);
            if (userInfo != null)
            {
                retval.Registered = true;
                retval.RegisteredUserID = userInfo.userId;
                retval.RegisteredDate = userInfo.registDt;
            }
            else
            {
                retval.Registered = false;
            }

            if (retval.Registered == false && nameCheckReturnCode == 1)
            {
                retval.Validated = true;
            }
            else
            {
                retval.Validated = false;
            }


            return retval;
        }

        /// <summary>
        /// 회원가입 처리 > 일반/외국인 사용자
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RegistUserResult RegistUserGeneral(RegistUserRequest request)
        {
            WowLog.Write("JoinBiz.RegistUserGeneral > UserId: " + request.UserId);
            RegistUserResult result = new RegistUserResult();

            if (string.IsNullOrEmpty(request.Name) == true || string.IsNullOrEmpty(request.UserId) == true || string.IsNullOrEmpty(request.NickName) == true)
            {
                result.ReturnCode = "LOSS_DATA";
                result.ReturnMessage = "누락된 데이터가 있습니다.";
                return result;
            }

            tblUser user = db89_wowbill.tblUser.SingleOrDefault(a => a.userId == request.UserId);
            if (user != null)
            {
                result.ReturnCode = "DUPLICATE_USERID";
                result.ReturnMessage = "중복된 아이디가 있습니다.";
                return result;
            }

            int? registeredUserNumber = null;
            if (request.AuthType == "F")
            {
                request.SSNo2 = request.SSNo2.Substring(0, 1) + request.SSNo1;
            }
            else
            {
                registeredUserNumber = commonBiz.GetDupInfoUserNumber(request.DupInfo);
                if (registeredUserNumber.HasValue == true)
                {
                    result.ReturnCode = "DUPLICATE_DI";
                    result.ReturnMessage = "이미 가입된 인증 값이 있습니다.";
                    return result;
                }
            }

            NBox nbox = new NBox();
            string nboxString = nbox.ToString(request.NBoxRequest);

            tblUser_Log log = new tblUser_Log();
            log.userid = request.UserId;
            log.mode = "아이핀";
            log.nbox = nboxString;
            log.regdate = DateTime.Now;
            db89_wowbill.tblUser_Log.Add(log);
            db89_wowbill.SaveChanges();

            string encPassword = commonBiz.Encrypt("HASH", "SHA256", request.Password);

            string ssno = request.SSNo1 + request.SSNo2;
            string encSSNo = commonBiz.Encrypt("GENERAL_ENC", "NORMAL", ssno);

            string tel = request.Tel1 + "-" + request.Tel2 + "-" + request.Tel3;
            string mobile = request.Mobile1 + "-" + request.Mobile2 + "-" + request.Mobile3;
            string email = request.Email1 + "@" + request.Email2;

            //if (string.IsNullOrEmpty(request.Option1) == true)
            //{
            //    // NICE 모듈코드값 세분화(I/B) , 아이핀으로 가입한 경우 : I, 본인인증(휴대폰)으로 가입한 경우: B
            //    if (request.AuthType == "I")
            //    {
            //        request.Option1 = "I";
            //    }
            //    else if (request.AuthType == "M")
            //    {
            //        request.Option1 = "B";
            //    }
            //}
            if (string.IsNullOrEmpty(request.Option2) == true)
            {
                request.Option2 = "DD5"; // 아이핀이나 본인인증으로 가입한 회원가입의 경우(본인인증 모듈)
            }

            ObjectParameter returnNumber = new ObjectParameter("RETURN_NUMBER", typeof(int));
            ObjectParameter returnMessage = new ObjectParameter("RETURN_MESSAGE", typeof(string));
            ObjectParameter userNumber = new ObjectParameter("USER_NUMBER", typeof(int));

            byte userSectionId = 1;
            if (request.AuthType == "F")
            {
                userSectionId = 5;
            }
            db89_wowbill.NUP_MEMBER_CREATE_GENERAL(
                request.UserId, request.NickName, encPassword, request.Name, encSSNo, tel, mobile, request.PasswordConfirmId, request.PasswordConfirmAnswer,
                email, request.IsSendSms, request.IsSendSmsAd, request.IsSendEmail, request.IsSendEmailAd, request.Option1, request.Option2, request.Sido, request.Gugun,
                request.BirthDate, request.Issolar, request.IsMale, request.DupInfo, request.ConnInfo, userSectionId, returnNumber, returnMessage, userNumber
            );

            if (returnNumber.Value.ToString() != "0")
            {
                tblUser_FailLog failLog = new tblUser_FailLog();
                failLog.userid = request.UserId;
                if (request.AuthType == "I")
                {
                    failLog.mode = "아이핀";
                }
                else if (request.AuthType == "M")
                {
                    failLog.mode = "휴대폰";
                }
                failLog.nbox = nboxString;
                failLog.regdate = DateTime.Now;
                db89_wowbill.tblUser_FailLog.Add(failLog);
                db89_wowbill.SaveChanges();

                result.ReturnCode = returnNumber.Value.ToString();
                result.ReturnMessage = returnMessage.Value.ToString();

                return result;
            }

            //WowLog.Write("JoinBiz.RegistUserGeneral > db89_wowbill.NUP_MEMBER_CREATE_GENERAL 완료");

            result.UserNumber = int.Parse(userNumber.Value.ToString());

            //SNS 회원 연동 정보 저장
            if(request.KakaoId != null || request.FacebookId != null || request.NaverId != null)
            {
                string snsSql = "EXEC NUP_MEMBER_UPDATE_SNS @USERNUMBER={0}, @KAKAO_ID={1}, @KAKAO_EMAIL={2}, @KAKAO_NICKNAME={3}, @FACEBOOK_ID={4}, @FACEBOOK_EMAIL={5}, @FACEBOOK_NAME={6}, @NAVER_ID={7}, @NAVER_EMAIL={8}, @NAVER_NAME={9}";
                db89_wowbill.Database.ExecuteSqlCommand(snsSql, result.UserNumber, request.KakaoId, request.KakaoEmail, request.KakaoNickname, request.FacebookId, request.FacebookEmail, request.FacebookName, request.NaverId, request.NaverEmail, request.NaverkName);
            }

            // OK 캐쉬백 회원가입
            if (string.IsNullOrEmpty(request.EcUserId) == false && string.IsNullOrEmpty(request.LottoId) == false && string.IsNullOrEmpty(request.SFrom) == false)
            {
                string webReturnMessage = "";
                try
                {
                    result.OKCashbagRegistYN = true;
                    //sReturn = ""
                    //returnTmp = tmpOKLotto & "|" & tmpOKUser & "|" & tmpOKFrom
                    //FOR i = 1 TO len(returnTmp)
                    //    sReturn = sReturn & CStr(hex(Asc(mid(returnTmp, i, 1))))
                    //NEXT
                    string okCashbagReturn = string.Format("{0:X}", Encoding.ASCII.GetBytes(request.LottoId + "|" + request.EcUserId + "|" + request.SFrom));

                    //WebRequest webRequest = WebRequest.Create("http://plus.okcashbag.com:13009/cashbag/receive/receive.jsp?returnstr=" + okCashbagReturn);
                    WebRequest webRequest = WebRequest.Create("http://plus.okcashbag.com:13009/cashbag/receive/receive_test.jsp?returnstr=" + okCashbagReturn);
                    webRequest.Credentials = CredentialCache.DefaultCredentials;
                    WebResponse webResponse = webRequest.GetResponse();
                    Console.WriteLine(((HttpWebResponse)webResponse).StatusDescription);
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    webReturnMessage = reader.ReadToEnd();
                    reader.Close();
                    webResponse.Close();
                }
                catch (Exception ex)
                {
                    if (string.IsNullOrEmpty(webReturnMessage) == true)
                    {
                        webReturnMessage = ex.Message;
                    }
                }

                if (webReturnMessage == "SUCCESS")
                {
                    result.OKCashbagRegistSuccess = true;
                }
                else
                {
                    result.OKCashbagRegistSuccess = false;
                    result.OKCashbagRegistMessage = webReturnMessage;
                }
            }
            else
            {
                result.OKCashbagRegistYN = false;
            }

            // WOW스탁론 상담신청정보
            if (request.Isstockloan == "1")
            {
                try
                {
                    TBL_STOCKLOAN_CONSLT stockloanConslt = new TBL_STOCKLOAN_CONSLT();
                    stockloanConslt.userid = request.UserId;
                    stockloanConslt.reg_date = DateTime.Now;
                    stockloanConslt.state = "0";
                    db89_wowbill.TBL_STOCKLOAN_CONSLT.Add(stockloanConslt);
                    db89_wowbill.SaveChanges();

                    BOQv7BillLib.BillClass couponBill1 = new BOQv7BillLib.BillClass();
                    couponBill1.TxCmd = "issueonecoupon";
                    couponBill1.HOST = request.BOQv5BillHost;
                    couponBill1.CodePage = 0;
                    couponBill1.SetField("couponid", "62"); // 골드플러스 1개월 무료서비스 쿠폰ID / 62
                    couponBill1.SetField("durationtype", "4"); // 유효기간 유형 (4:일, 5:월)	// K100204
                    couponBill1.SetField("duration", "31"); // 유효기간 값(일수 or 월수)	// K100204
                    couponBill1.SetField("userid", request.UserId); // 고객아이디
                    couponBill1.SetField("reason", "스탁론 상담");
                    couponBill1.SetField("adminid", "지급관리자ID");
                    int couponBill1Result = couponBill1.StartAction();
                    result.CouponBill1Regist = true;
                    result.CouponBill1ReturnCode = couponBill1Result;
                    result.CouponBill1ReturnMessage = couponBill1.ErrMsg;
                    WowLog.Write("[CouponBill-1] UserId: " + request.UserId + ", ReturnCode: " + result.CouponBill1ReturnCode + ", ReturnMessage: " + result.CouponBill1ReturnMessage);
                }
                catch (Exception ex)
                {
                    WowLog.Write("JoinBiz.RegistUserGeneral > WOW스탁론 상담신청정보 실패: " + ex.Message);
                }
            }
            else
            {
                result.CouponBill1Regist = false;
            }

            // 동부골드클럽 회원가입
            if (request.SPageFrom == "DongbuGold")
            {
                try
                {

                    result.CafeGoldMemberRegist = true;

                    ObjectParameter wowcafeOutput = new ObjectParameter("strResultCafe", typeof(string));
                    db49_wowcafe.usp_RegistCafememberGold(request.SPRO, request.UserId, wowcafeOutput);
                    //string outValue = wowcafeOutput.Value as string;
                }
                catch (Exception ex)
                {
                    WowLog.Write("JoinBiz.RegistUserGeneral > 동부골드클럽 회원가입 실패: " + ex.Message);
                }

                return result;
            }
            else
            {
                result.CafeGoldMemberRegist = false;
            }


            // MMS 문자 발송
            if (string.IsNullOrEmpty(mobile) == false)
            {
                string mobileNumber = mobile.Replace("-", "");
                string callback = "15990700";

                try
                {

                    mms_msg msg = new mms_msg();
                    msg.REQDATE = DateTime.Now;
                    msg.PHONE = mobileNumber;
                    msg.CALLBACK = callback;
                    msg.SUBJECT = "한국경제TV";
                    msg.MSG =
                        "(광고)[한국경제TV] 회원가입을 축하드립니다.\n\n" +
                        "신규회원분들에게 제공되는 헤택 3가지를 꼼꼼히 확인하시기 바랍니다!\n\n" +
                        "<신규회원 3가지 혜택>\n" +
                        "1. 김종철 패키지(종가공략주+인공지능차트, 매직양봉팀(박완필,이효근) 스마트밴드 3일 체험쿠폰\n" +
                        "2. 와우캐시(이벤트) 10만원\n" +
                        "3. 주식비타민 앱 1주일 이용권\n\n" +
                        "각 혜택별 자세히보기(주소클릭) : http://goo.gl/aAQBGk" + "\n" +
                        "쿠폰&이벤트캐시 만료일은 로그인후 마이페이지 확인 가능\n" +
                        "사용문의 1599-0700\n\n" +
                        "▲무료수신거부0808635563";
                    msg.FILE_CNT = 0;
                    msg.ETC1 = "wowsms-admin";
                    msg.ETC2 = "system";
                    msg.ETC3 = request.UserId;

                    msg.REQDATE = DateTime.Now;
                    msg.FILE_CNT_REAL = 0;
                    msg.STATUS = "2";
                    msg.EXPIRETIME = "43200";
                    msg.REPCNT = 0;
                    msg.TYPE = "0";

                    Db51_WOWMMS.mms_msg.Add(msg);
                    Db51_WOWMMS.SaveChanges();
                }
                catch (Exception ex)
                {
                    WowLog.Write("JoinBiz.RegistUserGeneral > MMS 문자 발송 실패: " + ex.Message);
                }

                try
                {
                    if (request.IsSendSms == true)
                    {
                        db51_ARSsms.USP_SC_TRAN_INSERT(mobileNumber, callback,
                            "(광고)[와우넷 필수앱]와우밴드 무료설치클릭: http://goo.gl/ATnkgy▲무료수신거부0808635563", "wowsms-member", "system", request.UserId, "회원가입", "", "", "");
                    }
                }
                catch (Exception ex)
                {
                    WowLog.Write("JoinBiz.RegistUserGeneral > 광고문자 발송 실패: " + ex.Message);
                }
            }

            try
            {
                BOQv7BillLib.BillClass couponBill2 = new BOQv7BillLib.BillClass();
                couponBill2.TxCmd = "issueonecoupon";
                couponBill2.HOST = request.BOQv5BillHost;
                couponBill2.CodePage = 0;
                couponBill2.SetField("couponid", "917"); // 김종철 패키지
                couponBill2.SetField("durationtype", "4"); // 유효기간 유형 (4:일, 5:월)	// K100204
                couponBill2.SetField("duration", "31"); // 유효기간 값(일수 or 월수)	// K100204
                couponBill2.SetField("reason", "신규회원 쿠폰");
                couponBill2.SetField("adminid", "system"); // 지급관리자ID
                couponBill2.SetField("userid", request.UserId); // 고객아이디
                result.CouponBill2Regist = true;
                result.CouponBill2ReturnCode = couponBill2.StartAction();
                result.CouponBill2ReturnMessage = couponBill2.ErrMsg;
                WowLog.Write("[CouponBill-2] UserId: " + request.UserId + ", ReturnCode: " + result.CouponBill2ReturnCode + ", ReturnMessage: " + result.CouponBill2ReturnMessage);
            }
            catch (Exception ex)
            {
                WowLog.Write("JoinBiz.RegistUserGeneral > 김종철 패키지 쿠폰 발행 실패: " + ex.Message);
            }

            try
            {
                BOQv7BillLib.BillClass couponBill3 = new BOQv7BillLib.BillClass();
                couponBill3.TxCmd = "issueonecoupon";
                couponBill3.HOST = request.BOQv5BillHost;
                couponBill3.CodePage = 0;
                couponBill3.SetField("couponid", "924"); // 매직양봉팀 스마트밴드 3일체험 쿠폰
                couponBill3.SetField("durationtype", "4"); // 유효기간 유형 (4:일, 5:월)	// K100204
                couponBill3.SetField("duration", "31"); // 유효기간 값(일수 or 월수)	// K100204
                couponBill3.SetField("reason", "신규회원 쿠폰");
                couponBill3.SetField("adminid", "system"); // 지급관리자ID
                couponBill3.SetField("userid", request.UserId); // 고객아이
                result.CouponBill3Regist = true;
                result.CouponBill3ReturnCode = couponBill3.StartAction();
                result.CouponBill3ReturnMessage = couponBill3.ErrMsg;
                WowLog.Write("[CouponBill-3] UserId: " + request.UserId + ", ReturnCode: " + result.CouponBill3ReturnCode + ", ReturnMessage: " + result.CouponBill3ReturnMessage);
            }
            catch (Exception ex)
            {
                WowLog.Write("JoinBiz.RegistUserGeneral > 매직양봉팀 스마트밴드 3일체험 쿠폰 발행 실패: " + ex.Message);
            }

            try
            {
                // 와우캐시 10만원 무료지급
                ObjectParameter cashReal = new ObjectParameter("po_intCashReal", typeof(int));
                ObjectParameter cashBonus = new ObjectParameter("po_intCashBonus", typeof(int));
                ObjectParameter cashNo = new ObjectParameter("po_intCashNo", typeof(long));
                ObjectParameter cashErrorMessage = new ObjectParameter("po_strErrMsg", typeof(string));
                ObjectParameter cashReturnCode = new ObjectParameter("po_intRetVal", typeof(int));
                tblUser userInfo = db89_wowbill.tblUser.SingleOrDefault(a => a.userId == request.UserId);
                int wowCash = 100000;

                string executeSql =
                    "DECLARE @po_intCashReal MONEY\r\n" +
                    "DECLARE @po_intCashBonus MONEY\r\n" +
                    "DECLARE @po_intCashNo BIGINT\r\n" +
                    "DECLARE @po_strErrMsg VARCHAR(256)\r\n" +
                    "DECLARE @po_intRetVal INT\r\n" +
                    "EXEC UP_PORTAL_BONUSCASH_TX_INS\r\n" +
                    "@pi_intUserNo={0},@pi_strUserID={1},@pi_strUserName={2},@pi_strPersonNo={3},@pi_strCashAttr={4}," +
                    "@pi_intCashAmt={5},@pi_strPayToolName={6},@pi_strAdminID={7},@pi_strIPAddr={8},@pi_strEYMD={9}," +
                    "@po_intCashReal=@po_intCashReal OUTPUT,@po_intCashBonus=@po_intCashBonus OUTPUT,@po_intCashNo=@po_intCashNo OUTPUT," +
                    "@po_strErrMsg=@po_strErrMsg OUTPUT,@po_intRetVal=@po_intRetVal OUTPUT\r\n" +
                    "SELECT @po_intRetVal AS RET_VALUE, @po_strErrMsg AS RET_MESSAGE";

                //db89_WOWTV_BILL_DB.Database.ExecuteSqlCommand(executeSql, userInfo.userNumber, request.UserId, DBNull.Value, DBNull.Value, "event(m)-all", 100000,
                //    "'회원가입 감사'", "system(web)", "null", DateTime.Now.ToString("yyyyMMdd"));

                //db89_WOWTV_BILL_DB.UP_PORTAL_BONUSCASH_TX_INS(userInfo.userNumber, request.UserId, null, null, "event(m)-all", 100000, "회원가입 감사",
                //    "system(web)", null, DateTime.Now.ToString("yyyyMMdd"), cashReal, cashBonus, cashNo, cashErrorMessage, cashReturnCode);

                RegistUserReturn retval = db89_WOWTV_BILL_DB.Database.SqlQuery<RegistUserReturn>(executeSql, userInfo.userNumber, request.UserId, DBNull.Value, DBNull.Value, "event(m)-all", wowCash,
                    "회원가입 감사", "system(web)", DBNull.Value, DateTime.Now.AddDays(30).ToString("yyyyMMdd")).SingleOrDefault();

                result.BonusCash = wowCash;
            }
            catch (Exception ex)
            {
                WowLog.Write("JoinBiz.RegistUserGeneral > 와우캐시 지급 실패: " + ex.Message);
            }


            // 주식비타민 7일 무료지급 추가
            if (string.IsNullOrEmpty(mobile) == false)
            {// TODO
                string mobileNumber = mobile.Replace("-", "");
                if (request.AuthType == "I")
                {
                    //strSql = strSql & "_insert_vitjuno_phon '" & tmpPhone & "', '14', '와우넷 신규회원 가입 1주일 무료이용'" ' 1주일 코드번호 : 14
                    //result = execSql("threetempo", strSql)
                }
                else if (request.AuthType == "M")
                {
                    //strSql = strSql & "usp_vix_user_reg_phon '" & tmpPhone & "', '11207000007', '와우넷 신규회원 가입 1주일 무료이용'" ' 1주일 코드번호 : 11207000007
                    //result = execSql("threetempo", strSql)
                }

                // 실행안함 (ASP에서도 주석처리 되어있음)
                //mms_msg msg7 = new mms_msg();
                //msg7.REQDATE = DateTime.Now;
                //msg7.PHONE = mobileNumber;
                //msg7.CALLBACK = "15990700";
                //msg7.SUBJECT = "한국경제TV";
                //msg7.MSG =
                //    "(광고)와우넷 회원가입을 축하드립니다.\n" +
                //    "주식비타민 1주일 무료이용권이 발급되었습니다. \n\n" +
                //    "주식비타민이란? \n\n" +
                //    "하루 3번 투자 유망종목과 시황을 스마트폰으로 볼 수 있는 모바일 서비스입니다 \n\n" +
                //    "지금 주식창 앱을 설치하고 주식비타민을 이용해보세요 \n" +
                //    "주식창 앱 설치 바로가기: \n" +
                //    "http://stockwin.wowtv.co.kr/sw.html \n" +
                //    "▲무료수신거부0808635563\n\n";
                //msg7.FILE_CNT = 0;
                //msg7.ETC1 = "wowsms-admin";
                //msg7.ETC2 = "system";
                //msg7.ETC3 = request.UserId;

                //Db51_WOWMMS.mms_msg.Add(msg7);
                //db51_ARSsms.SaveChanges();
            }

            if (string.IsNullOrEmpty(result.ReturnCode) == true)
            {
                result.ReturnCode = "OK";
                result.ReturnMessage = "";
            }

            return result;
        }

        /// <summary>
        /// 회원가입 처리 > 법인 사용자
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RegistUserResult RegistUserCompany(RegistUserRequest request)
        {
            RegistUserResult result = new RegistUserResult();

            if (string.IsNullOrEmpty(request.Name) == true || string.IsNullOrEmpty(request.UserId) == true || string.IsNullOrEmpty(request.NickName) == true)
            {
                result.ReturnCode = "LOSS_DATA";
                result.ReturnMessage = "누락된 데이터가 있습니다.";
                return result;
            }

            tblUser user = db89_wowbill.tblUser.SingleOrDefault(a => a.userId == request.UserId);
            if (user != null)
            {
                result.ReturnCode = "DUPLICATE_USERID";
                result.ReturnMessage = "중복된 아이디가 있습니다.";
                return result;
            }

            int? registeredUserNumber = commonBiz.GetDupInfoUserNumber(request.DupInfo);
            if (registeredUserNumber.HasValue == true)
            {
                result.ReturnCode = "DUPLICATE_DI";
                result.ReturnMessage = "이미 가입된 인증 값이 있습니다.";

                return result;
            }

            string encPassword = commonBiz.Encrypt("HASH", "SHA256", request.Password);
            //string encPassword = XdbCrypto.Hash(request.Password);
            //ObjectResult<string> encPasswordSelect = db89_wowbill.NUP_ENC_SELECT("HASH", "SHA256", request.Password);
            //string encPassword = encPasswordSelect.FirstOrDefault();

            string ssno = request.SSNo1 + "-" + request.SSNo2 + "-" + request.SSNo3;
            string encSSNo = commonBiz.Encrypt("GENERAL_ENC", "NORMAL", ssno);
            //string encSSNo = XdbCrypto.Encrypt(ssno);
            //ObjectResult<string> encSSNoSelect = db89_wowbill.NUP_ENC_SELECT("GENERAL_ENC", "NORMAL", ssno);
            //string encSSNo = encSSNoSelect.FirstOrDefault();

            string tel = request.Tel1 + "-" + request.Tel2 + "-" + request.Tel3;
            string mobile = request.Mobile1 + "-" + request.Mobile2 + "-" + request.Mobile3;
            string email = request.Email1 + "@" + request.Email2;
            string address = request.Address1 + " " + request.Address2;

            if (string.IsNullOrEmpty(request.Option1) == true)
            {
                request.Option1 = null; // 법인회원의 경우 NICE본인인증없이 가입됨 : 기존실명확인과 같이 U
            }
            if (string.IsNullOrEmpty(request.Option2) == true)
            {
                request.Option2 = "DD5"; // 아이핀이나 본인인증으로 가입한 회원가입의 경우(본인인증 모듈)
            }

            ObjectParameter returnNumber = new ObjectParameter("RETURN_NUMBER", typeof(int));
            ObjectParameter returnMessage = new ObjectParameter("RETURN_MESSAGE", typeof(string));
            ObjectParameter userNumber = new ObjectParameter("USER_NUMBER", typeof(int));

            string zipCode = string.IsNullOrEmpty(request.ZipCode) == false ? request.ZipCode.Replace("-", "") : request.ZipCode;

            db89_wowbill.NUP_MEMBER_CREATE_COMPANY(
                request.UserId, request.NickName, encPassword, request.Name, encSSNo, tel, mobile, request.PasswordConfirmId, request.PasswordConfirmAnswer,
                email, request.IsSendSms, request.IsSendSmsAd, request.IsSendEmail, request.IsSendEmailAd, request.Option1, request.Option2, zipCode, address, request.Owner, request.Businessitem,
                request.BusinessCondition, request.BusinessType, request.CompanyNo, request.Listed, request.EstablishmentAnniversary, request.Landcenter, returnNumber, returnMessage, userNumber
            );

            if (returnNumber.Value.ToString() != "0")
            {
                tblUser_FailLog failLog = new tblUser_FailLog();
                failLog.userid = request.UserId;
                failLog.mode = "기업회원";
                failLog.regdate = DateTime.Now;
                db89_wowbill.tblUser_FailLog.Add(failLog);
                db89_wowbill.SaveChanges();

                result.ReturnCode = returnNumber.Value.ToString();
                result.ReturnMessage = returnMessage.Value.ToString();

                return result;
            }

            result.UserNumber = int.Parse(returnNumber.Value.ToString());
            if (string.IsNullOrEmpty(result.ReturnCode) == true)
            {
                result.ReturnCode = "OK";
                result.ReturnMessage = "";
            }

            return result;
        }

        /// <summary>
        /// 아이디 중복 체크
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int IsUserIdDuplicated(string userId)
        {
            int retval = -1;
            bool withSpecialCharacter = new MemberCommonBiz().CheckUserId(userId);
            bool withSpaceCharacter = userId.IndexOf(" ") > -1;
            bool isDuplicated = false;

            List<tblUser> user = db89_wowbill.tblUser.Where(a => a.userId == userId).ToList();
            if (user.Count > 0)
            {
                isDuplicated = true;
            }

            // 0: 정상, 1: 띄어쓰기, 2: 특수문자포함, 3: 중복
            if (isDuplicated == false && withSpecialCharacter == false && withSpaceCharacter == false)
            {
                retval = 0;
            }
            else if (withSpaceCharacter == true)
            {
                retval = 1;
            }
            else if (withSpecialCharacter == true)
            {
                retval = 2;
            }
            else if (isDuplicated == true)
            {
                retval = 3;
            }

            return retval;
        }

        /// <summary>
        /// 필명 중복 체크
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        /// <returns>0: 정상, 1: 특수문자포함, 2: 중복, 3: 띄어쓰기</returns>
        public int IsNickNameDuplicated(string nickName)
        {
            int retval = -1;
            bool withSpecialCharacter = new MemberCommonBiz().CheckNickName(nickName);
            bool withSpaceCharacter = nickName.IndexOf(" ") > -1;
            bool isDuplicated = false;

            List<tblUser> userList = db89_wowbill.tblUser.Where(a => a.NickName == nickName).ToList();
            if (userList.Count > 0)
            {
                isDuplicated = true;
            }
            else
            {
                // 사용제한 닉네임 체크(전문가유사필명 등)
                List<tbl_Nickname> restrictNicknameList = db89_wowbill.tbl_Nickname.Where(a => a.nickname == nickName).ToList();
                if (restrictNicknameList.Count > 0)
                {
                    isDuplicated = true;
                }
                else
                {
                    isDuplicated = false;
                }
            }

            // 0: 정상, 1: 띄어쓰기, 2: 특수문자포함, 3: 중복
            if (isDuplicated == false && withSpecialCharacter == false && withSpaceCharacter == false)
            {
                retval = 0;
            }
            else if (withSpaceCharacter == true)
            {
                retval = 1;
            }
            else if (withSpecialCharacter == true)
            {
                retval = 2;
            }
            else if (isDuplicated == true)
            {
                retval = 3;
            }

            return retval;
        }

        /// <summary>
        /// SMS 인증
        /// </summary>
        /// <param name="mobile1"></param>
        /// <param name="mobile2"></param>
        /// <param name="mobile3"></param>
        /// <returns></returns>
        public string MobileSendSMS(string mobile1, string mobile2, string mobile3)
        {
            string mobileNumber = mobile1 + mobile2 + mobile3;

            Random rnd = new Random();
            int number = rnd.Next(0, 999999);
            string authNumber = string.Format("{0:D6}", number);

            SC_TRAN data = new SC_TRAN();
            data.TR_SENDDATE = DateTime.Now;
            data.TR_SENDSTAT = "0";
            data.TR_RSLTSTAT = "00";
            data.TR_PHONE = mobileNumber;
            data.TR_CALLBACK = "0266760000";
            data.TR_MSG = "[한국경제TV] 인증번호 [" + authNumber + "]를 입력해주세요.";
            data.TR_ID = "wowsms-member";
            data.TR_ETC1 = "휴대폰인증 통합회원";
            db51_ARSsms.SC_TRAN.Add(data);
            db51_ARSsms.SaveChanges();

            return authNumber;
        }

        /// <summary>
        /// 가입된 기업정보 체크
        /// </summary>
        /// <param name="ssno"></param>
        /// <returns></returns>
        public bool CompanyExists(string ssno)
        {
            string encSSNo = commonBiz.Encrypt("GENERAL_ENC", "NORMAL", ssno);
            //string encSSNo = XdbCrypto.Encrypt(ssno);
            //ObjectResult<string> ssnoSelect = db89_wowbill.NUP_ENC_SELECT("GENERAL_ENC", "NORMAL", ssno);
            //string encSSNo = ssnoSelect.SingleOrDefault();

            List<tblUser> user = db89_wowbill.tblUser.Where(a => a.ssno == encSSNo).ToList();
            return user.Count > 0 ? true : false;
        }

        /// <summary>
        /// 이메일 인증 메일 완료 처리
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public EmailAuthCompleteResult EmailAuthComplete(int userNumber, string email)
        {
            return commonBiz.EmailAuthComplete(userNumber, email);
        }

        public List<string> GetSido()
        {
            List<string> result = (from tbl in db89_wowbill.tblPost orderby tbl.sido select tbl.sido).Distinct().ToList();
            return result;
        }

        public List<string> GetGugun(string sido)
        {
            List<string> result = (from tbl in db89_wowbill.tblPost where tbl.sido == sido orderby tbl.gugun select tbl.gugun).Distinct().ToList();
            return result;
        }

        public List<tblPost> GetAddress(string searchKeyword)
        {
            List<tblPost> result = (from tbl in db89_wowbill.tblPost
                                    where tbl.sido.Contains(searchKeyword) || tbl.gugun.Contains(searchKeyword) || tbl.dong.Contains(searchKeyword)
                                    orderby tbl.sido ascending, tbl.gugun ascending, tbl.dong ascending
                                    select tbl).ToList();
            return result;
        }

        public void BillingServiceTest(string bOQv5BillHost, string userId)
        {
            BOQv7BillLib.BillClass couponBill1 = new BOQv7BillLib.BillClass();
            couponBill1.TxCmd = "issueonecoupon";
            couponBill1.HOST = bOQv5BillHost;
            couponBill1.CodePage = 0;
            couponBill1.SetField("couponid", "62"); // 골드플러스 1개월 무료서비스 쿠폰ID / 62
            couponBill1.SetField("durationtype", "4"); // 유효기간 유형 (4:일, 5:월)	// K100204
            couponBill1.SetField("duration", "31"); // 유효기간 값(일수 or 월수)	// K100204
            couponBill1.SetField("userid", userId); // 고객아이디
            couponBill1.SetField("reason", "스탁론 상담");
            couponBill1.SetField("adminid", "지급관리자ID");
            int couponBill1Result = couponBill1.StartAction();
            WowLog.Write("[CouponBill-1] UserId: " + userId + ", ReturnCode: " + couponBill1Result + ", ReturnMessage: " + couponBill1.ErrMsg);

            BOQv7BillLib.BillClass couponBill2 = new BOQv7BillLib.BillClass();
            couponBill2.TxCmd = "issueonecoupon";
            couponBill2.HOST = bOQv5BillHost;
            couponBill2.CodePage = 0;
            couponBill2.SetField("couponid", "917"); // 김종철 패키지
            couponBill2.SetField("durationtype", "4"); // 유효기간 유형 (4:일, 5:월)	// K100204
            couponBill2.SetField("duration", "31"); // 유효기간 값(일수 or 월수)	// K100204
            couponBill2.SetField("reason", "신규회원 쿠폰");
            couponBill2.SetField("adminid", "system"); // 지급관리자ID
            couponBill2.SetField("userid", userId); // 고객아이
            int couponBill2Result = couponBill2.StartAction();
            WowLog.Write("[CouponBill-2] UserId: " + userId + ", ReturnCode: " + couponBill2Result + ", ReturnMessage: " + couponBill2.ErrMsg);

            BOQv7BillLib.BillClass couponBill3 = new BOQv7BillLib.BillClass();
            couponBill3.TxCmd = "issueonecoupon";
            couponBill3.HOST = bOQv5BillHost;
            couponBill3.CodePage = 0;
            couponBill3.SetField("couponid", "924"); // 매직양봉팀 스마트밴드 3일체험 쿠폰
            couponBill3.SetField("durationtype", "4"); // 유효기간 유형 (4:일, 5:월)	// K100204
            couponBill3.SetField("duration", "31"); // 유효기간 값(일수 or 월수)	// K100204
            couponBill3.SetField("reason", "신규회원 쿠폰");
            couponBill3.SetField("adminid", "system"); // 지급관리자ID
            couponBill3.SetField("userid", userId); // 고객아이
            int couponBill3Result = couponBill3.StartAction();
            WowLog.Write("[CouponBill-3] UserId: " + userId + ", ReturnCode: " + couponBill3Result + ", ReturnMessage: " + couponBill3.ErrMsg);
        }

        public void EmailSendTest(string mailCode, string fromName, string fromEmail, string toName, string toEmail, string subject, string contents)
        {
            new EmailBiz().Send(new EmailSendParameter()
            {
                EmailCode = (EmailCodeModel)int.Parse(mailCode),
                FromName = fromName,
                FromEmail = fromEmail,
                ToName = toName,
                ToEmail = toEmail,
                Subject = subject,
                Contents = contents
            });
        }
    }
}
