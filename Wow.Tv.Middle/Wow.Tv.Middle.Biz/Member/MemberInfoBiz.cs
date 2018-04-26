using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Fx;
using Wow.Tv.Middle.Biz.MyPage;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db89.Sleep_wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;
using Wow.Tv.Middle.Model.Db89.wowbill.MyInfo;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.Member;
using Wow.Tv.Middle.Model.Db90.DNRS;

namespace Wow.Tv.Middle.Biz.Member
{
    public class MemberInfoBiz : BaseBiz
    {
        private MemberCommonBiz commonBiz = null;
        public MemberInfoBiz()
        {
            commonBiz = new MemberCommonBiz();
        }

        /// <summary>
        /// 회원 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public MemberInfoResult MemberInfo(int userNumber)
        {
            MemberInfoResult retval = new MemberInfoResult();

            ObjectResult<NUP_MEMBER_INFO_SELECT_Result> memberInfo = db89_wowbill.NUP_MEMBER_INFO_SELECT(userNumber);
            retval.MemberInfo = memberInfo.FirstOrDefault();

            NUP_MEMBER_INFO_SUB_SELECT_Result memberSubInfo = db89_WOWTV_BILL_DB.NUP_MEMBER_INFO_SUB_SELECT(userNumber).FirstOrDefault();
            if (memberSubInfo != null)
            {
                retval.WowCash = memberSubInfo.WOW_CASH;
                retval.CouponCount = memberSubInfo.COUPON_COUNT;
                retval.UserClass = memberSubInfo.USER_CLASS;
                retval.UsedPrice = memberSubInfo.USED_PRICE;
            }
            
            return retval;
        }

        /// <summary>
        /// 일반/외국인 회원 상세 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public MemberInfoGeneralResult MemberInfoGeneral(int userNumber)
        {
            MemberInfoGeneralResult retval = new MemberInfoGeneralResult();

            ObjectResult<NUP_MEMBER_INFO_SELECT_GENERAL_Result> memberInfo = db89_wowbill.NUP_MEMBER_INFO_SELECT_GENERAL(userNumber);
            retval.MemberInfo = memberInfo.FirstOrDefault(); // 기본정보
            retval.Sido = commonBiz.GetSidoList();// 시/도
            retval.Gugun = commonBiz.GetGugunList(retval.MemberInfo.SIDO);// 구/군
            retval.Salary = commonBiz.GetSalaryList();// 연간수입
            retval.InvestmentPreferenceObject = commonBiz.GetInvestmentPreferenceObjectList();// 투자선호대상
            retval.InfoAcquirement = commonBiz.GetInfoAcquirementList();// 기존정보 습득처
            retval.InvestmentPeriod = commonBiz.GetInvestmentPeriodList();// 투자기간
            retval.InvestmentPropensity = commonBiz.GetInvestmentPropensityList();// 투자성향
            retval.StockCompany = commonBiz.GetStockCompanyList();// 주요증권거래처            
            retval.InvestmentScale = commonBiz.GetInvestmentScaleList();// 투자규모
            retval.Interest = commonBiz.GetInterestList();// 관심분야
            retval.Job = commonBiz.GetJobList();// 직업
            retval.RegistRoute = commonBiz.GetRegistRouteList();// 가입경로

            return retval;
        }

        /// <summary>
        /// 법인 회원 상세 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public MemberInfoCompanyResult MemberInfoCompany(int userNumber)
        {
            MemberInfoCompanyResult retval = new MemberInfoCompanyResult();

            ObjectResult<NUP_MEMBER_INFO_SELECT_COMPANY_Result> memberInfo = db89_wowbill.NUP_MEMBER_INFO_SELECT_COMPANY(userNumber);
            retval.MemberInfo = memberInfo.FirstOrDefault(); // 기본정보
            retval.Salary = commonBiz.GetSalaryList();// 연간수입
            retval.InvestmentPreferenceObject = commonBiz.GetInvestmentPreferenceObjectList();// 투자선호대상
            retval.InfoAcquirement = commonBiz.GetInfoAcquirementList();// 기존정보 습득처
            retval.InvestmentPeriod = commonBiz.GetInvestmentPeriodList();// 투자기간
            retval.InvestmentPropensity = commonBiz.GetInvestmentPropensityList();// 투자성향
            retval.StockCompany = commonBiz.GetStockCompanyList();// 주요증권거래처            
            retval.InvestmentScale = commonBiz.GetInvestmentScaleList();// 투자규모
            retval.Interest = commonBiz.GetInterestList();// 관심분야
            retval.Job = commonBiz.GetJobList();// 직업
            retval.RegistRoute = commonBiz.GetRegistRouteList();// 가입경로

            return retval;
        }

        /// <summary>
        /// 비밀번호 변경 유효성 검사
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool PasswordValidation(int userNumber, string password)
        {
            return commonBiz.PasswordValidation(userNumber, password);
        }

        /// <summary>
        /// 필명 중복 체크 (회원)
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="userNumber"></param>
        /// <returns>0: 정상, 1: 중복, 2: 특수문자포함, 3: 중복&특수문자포함</returns>
        public int IsNickNameDuplicated(string nickName, int userNumber)
        {
            int retval = -1;
            bool withSpecialCharacter = new MemberCommonBiz().CheckNickName(nickName);
            bool withSpaceCharacter = nickName.IndexOf(" ") > -1;
            bool isDuplicated = false;

            tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber).Single();
            if (userInfo.NickName == nickName)
            {
                isDuplicated = false;
            }
            else
            {
                List<tblUser> userList = db89_wowbill.tblUser.Where(a => a.NickName == nickName && a.userNumber != userNumber).ToList();
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
        /// 일반/외국인 회원 정보 수정
        /// </summary>
        /// <param name="request"></param>
        /// <param name="memberUrl"></param>
        /// <param name="memberStyleUrl"></param>
        /// <param name="memberScriptUrl"></param>
        /// <returns></returns>
        public SaveUserResult MemberSaveGeneral(SaveUserRequestGeneral request, string memberUrl, string memberStyleUrl, string memberScriptUrl)
        {
            SaveUserResult result = new SaveUserResult();

            try
            {
                tblUser userInfo = db89_wowbill.tblUser.AsNoTracking().Where(tbl => tbl.userNumber == request.UserNumber).SingleOrDefault();
                tblUserDetail userDetailInfo = db89_wowbill.tblUserDetail.Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSMS userSmsSendInfo = db89_wowbill.tblUserSMS.Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserEmailSend userEmailSendInfo = db89_wowbill.tblUserEmailSend.Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserPost userPostInfo = db89_wowbill.tblUserPost.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSNSNaver naverInfo = db89_wowbill.tblUserSNSNaver.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSNSKakao kakaoInfo = db89_wowbill.tblUserSNSKakao.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSNSFacebook facebookInfo = db89_wowbill.tblUserSNSFacebook.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();

                string tel = request.Tel1 + "-" + request.Tel2 + "-" + request.Tel3;
                string mobile = request.Mobile1 + "-" + request.Mobile2 + "-" + request.Mobile3;
                string email = request.Email1 + "@" + request.Email2;
                string zipCode = string.IsNullOrEmpty(request.ZipCode) == false ? request.ZipCode.Replace("-", "") : request.ZipCode;
                string address = request.Address1 + " " + request.Address2;

                // 정보 수정
                string executeSql =
                    "DECLARE @RETURN_NUMBER INT\r\n" +
                    "DECLARE @RETURN_MESSAGE VARCHAR(4000)\r\n" +
                    "EXEC NUP_MEMBER_UPDATE_GENERAL\r\n" +
                    "@USERNUMBER={0},@NICKNAME={1},@TEL={2},@MOBILE={3},@EMAIL={4},@IS_SEND_SMS={5},@IS_SEND_SMS_AD={6},@IS_SEND_EMAIL={7},@IS_SEND_EMAIL_AD={8}," +
                    "@SIDO={9},@GUGUN={10},@ZIPCODE={11},@ADDRESS={12},@SALARY_ID={13},@INVESTMENT_PERIOD_ID={14},@INVESTMENT_SCALE_ID={15},@INVESTMENT_PROPENSITY_ID={16}," +
                    "@INVESTMENT_PREFERENCE_OBJECT_ID={17},@STOCK_COMPANY_ID={18},@INFO_ACQUIREMENT_ID={19},@INTEREST_AREA={20},@JOB_ID={21},@REGIST_ROUTE_ID={22}," +
                    "@RETURN_NUMBER=@RETURN_NUMBER OUTPUT,@RETURN_MESSAGE=@RETURN_MESSAGE OUTPUT\r\n" +
                    "SELECT @RETURN_NUMBER AS ReturnNumber, @RETURN_MESSAGE AS ReturnMessage, CASE WHEN @RETURN_NUMBER = 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END AS IsSuccess";

                result = db89_wowbill.Database.SqlQuery<SaveUserResult>(executeSql, request.UserNumber, request.NickName, tel, mobile, email, request.IsSendSms, request.IsSendSmsAd,
                    request.IsSendEmail, request.IsSendEmailAd, request.Sido, request.Gugun, zipCode, address, request.SalaryId, request.InvestmentPeriodId, request.InvestmentScaleId, request.InvestmentPropensityId,
                    request.InvestmentPreferenceObjectId, request.StockCompany, request.InfoAcquirementId, request.InterestArea, request.JobId, request.RegistRouteId).SingleOrDefault();

                if (result.IsSuccess == false)
                {
                    return result;
                }

                // 메일정보 수정 시 인증메일 발송
                result.EmailChanged = userInfo.email != email;

                // 비밀번호 변경
                PasswordChangeResult passwordChagned = MemberPasswordChange(request.UserNumber, request.PasswordOriginal, request.PasswordNew, request.PasswordConfirm);

                // SNS 정보 수정
                //string snsSql = "EXEC NUP_MEMBER_UPDATE_SNS @USERNUMBER={0}, @KAKAO_ID={1}, @KAKAO_EMAIL={2}, @KAKAO_NICKNAME={3}, @FACEBOOK_ID={4}, @FACEBOOK_EMAIL={5}, @FACEBOOK_NAME={6}, @NAVER_ID={7}, @NAVER_EMAIL={8}, @NAVER_NAME={9}";
                //db89_wowbill.Database.ExecuteSqlCommand(snsSql, request.UserNumber, request.KakaoId, request.KakaoEmail, request.KakaoNickname,
                //    request.FacebookId, request.FacebookEmail, request.FacebookName, request.NaverId, request.NaverEmail, request.NaverkName);

                // SMS 수신여부
                bool isSendSmsFromDB = userInfo.isSendSMS == "0" || string.IsNullOrEmpty(userInfo.isSendSMS) == true ? false : true;
                bool validatedMobile = string.IsNullOrEmpty(request.Mobile1) == false && string.IsNullOrEmpty(request.Mobile2) == false && string.IsNullOrEmpty(request.Mobile3) == false;
                if (isSendSmsFromDB != request.IsSendSms && validatedMobile == true)
                {
                    string coupledMobileNo = request.Mobile1 + request.Mobile2 + request.Mobile3;
                    if (coupledMobileNo != "01000000000")
                    {
                        DateTime now = DateTime.Now;

                        string smsMent = "";
                        switch (request.IsSendSms)
                        {
                            case true: smsMent = "수신동의가"; break;
                            case false: smsMent = "수신거부가"; break;
                            //default: smsMent = "수신여부 변경이"; break;
                        }
                        smsMent = "[한국경제TV] " + now.Month + "월 " + now.Day + "일 한국경제TV에 요청하신 " + smsMent + " 정상처리되었습니다.";

                        db51_ARSsms.USP_SC_TRAN_INSERT(coupledMobileNo, "15990700", smsMent, "wowsms-member", "문자수신여부변경(회원정보수정)", userInfo.userId, "", "", "", "");
                    }
                }

                // 카페 정보 수정
                db49_wowcafe.Database.ExecuteSqlCommand("UPDATE CAFEMEMBERINFO SET EMAIL = {0}, HPHONE = {1} WHERE USERID = {2}", email, mobile, userInfo.userId);

                // 변경사항
                result.ModifiedItemList = GetModifiedItemList(true, result.EmailChanged, request, null, userInfo, userDetailInfo, userPostInfo, userSmsSendInfo,
                    userEmailSendInfo, null, passwordChagned, naverInfo, kakaoInfo, facebookInfo);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                WowLog.Write("[MemberSaveGeneral]StackTrace: " + ex.StackTrace + "\r\nSource: " + ex.Source + "\r\nInnerException: " + ex.InnerException + "\r\nMessage: " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 법인 회원 정보 수정
        /// </summary>
        /// <param name="request"></param>
        /// <param name="memberUrl"></param>
        /// <param name="memberStyleUrl"></param>
        /// <param name="memberScriptUrl"></param>
        /// <returns></returns>
        public SaveUserResult MemberSaveCompany(SaveUserRequestCompany request, string memberUrl, string memberStyleUrl, string memberScriptUrl)
        {
            SaveUserResult result = new SaveUserResult();

            try
            {
                tblUser userInfo = db89_wowbill.tblUser.Where(tbl => tbl.userNumber == request.UserNumber).SingleOrDefault();
                tblUserDetail userDetailInfo = db89_wowbill.tblUserDetail.Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSMS userSmsSendInfo = db89_wowbill.tblUserSMS.Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserEmailSend userEmailSendInfo = db89_wowbill.tblUserEmailSend.Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserPost userPostInfo = db89_wowbill.tblUserPost.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblCompanyDetail companyDetailInfo = db89_wowbill.tblCompanyDetail.AsNoTracking().Where(a => a.companyDetailId == userInfo.companyDetailId).SingleOrDefault();
                tblUserSNSNaver naverInfo = db89_wowbill.tblUserSNSNaver.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSNSKakao kakaoInfo = db89_wowbill.tblUserSNSKakao.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSNSFacebook facebookInfo = db89_wowbill.tblUserSNSFacebook.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();

                string tel = request.Tel1 + "-" + request.Tel2 + "-" + request.Tel3;
                string mobile = request.Mobile1 + "-" + request.Mobile2 + "-" + request.Mobile3;
                string email = request.Email1 + "@" + request.Email2;
                string zipCode = string.IsNullOrEmpty(request.ZipCode) == false ? request.ZipCode.Replace("-", "") : request.ZipCode;
                string address = request.Address1 + " " + request.Address2;

                string listedString = request.Listed == true ? "Y" : "N";

                // 정보 수정
                string executeSql =
                    "DECLARE @RETURN_NUMBER INT\r\n" +
                    "DECLARE @RETURN_MESSAGE VARCHAR(4000)\r\n" +
                    "EXEC NUP_MEMBER_UPDATE_COMPANY\r\n" +
                    "@USERNUMBER={0},@NICKNAME={1},@TEL={2},@MOBILE={3},@EMAIL={4},@IS_SEND_SMS={5},@IS_SEND_SMS_AD={6},@IS_SEND_EMAIL={7},@IS_SEND_EMAIL_AD={8}," +
                    "@ZIPCODE={9},@ADDRESS={10},@SALARY_ID={11},@INVESTMENT_PERIOD_ID={12},@INVESTMENT_SCALE_ID={13},@INVESTMENT_PROPENSITY_ID={14}," +
                    "@INVESTMENT_PREFERENCE_OBJECT_ID={15},@STOCK_COMPANY_ID={16},@INFO_ACQUIREMENT_ID={17},@INTEREST_AREA={18},@JOB_ID={19},@REGIST_ROUTE_ID={20}," +
                    "@BUSINESS_ITEM={21},@BUSINESS_CONDITION={22},@BUSINESS_CATEGORY={23}, @COMPANY_NO={24},@CEO_NAME={25},@ESTABLISHMENT_ANNIVERSARY={26},@LISTED={27}," +
                    "@RETURN_NUMBER=@RETURN_NUMBER OUTPUT,@RETURN_MESSAGE=@RETURN_MESSAGE OUTPUT\r\n" +
                    "SELECT @RETURN_NUMBER AS ReturnNumber, @RETURN_MESSAGE AS ReturnMessage, CASE WHEN @RETURN_NUMBER = 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END AS IsSuccess";

                result = db89_wowbill.Database.SqlQuery<SaveUserResult>(executeSql, request.UserNumber, request.NickName, tel, mobile, email, request.IsSendSms, request.IsSendSmsAd,
                    request.IsSendEmail, request.IsSendEmailAd, zipCode, address, request.SalaryId, request.InvestmentPeriodId, request.InvestmentScaleId, request.InvestmentPropensityId,
                    request.InvestmentPreferenceObjectId, request.StockCompany, request.InfoAcquirementId, request.InterestArea, request.JobId, request.RegistRouteId,
                    request.Businessitem, request.BusinessCondition, request.BusinessCategory, request.CompanyNo, request.Owner, request.EstablishmentAnniversary, listedString).SingleOrDefault();

                if (result.IsSuccess == false)
                {
                    return result;
                }

                // 메일정보 수정 시 인증메일 발송
                result.EmailChanged = userInfo.email != email;

                // 비밀번호 변경
                PasswordChangeResult passwordChagned = MemberPasswordChange(request.UserNumber, request.PasswordOriginal, request.PasswordNew, request.PasswordConfirm);

                // SNS 정보 수정
                //string snsSql = "EXEC NUP_MEMBER_UPDATE_SNS @USERNUMBER={0}, @KAKAO_ID={1}, @KAKAO_EMAIL={2}, @KAKAO_NICKNAME={3}, @FACEBOOK_ID={4}, @FACEBOOK_EMAIL={5}, @FACEBOOK_NAME={6}, @NAVER_ID={7}, @NAVER_EMAIL={8}, @NAVER_NAME={9}";
                //db89_wowbill.Database.ExecuteSqlCommand(snsSql, request.UserNumber, request.KakaoId, request.KakaoEmail, request.KakaoNickname,
                //    request.FacebookId, request.FacebookEmail, request.FacebookName, request.NaverId, request.NaverEmail, request.NaverkName);

                // SMS 수신여부
                bool isSendSmsFromDB = userInfo.isSendSMS == "0" || string.IsNullOrEmpty(userInfo.isSendSMS) == true ? false : true;
                bool validatedMobile = string.IsNullOrEmpty(request.Mobile1) == false && string.IsNullOrEmpty(request.Mobile2) == false && string.IsNullOrEmpty(request.Mobile3) == false;
                if (isSendSmsFromDB != request.IsSendSms && validatedMobile == true)
                {
                    string mobile2 = request.Mobile1 + request.Mobile2 + request.Mobile3;
                    if (mobile2 != "01000000000")
                    {
                        DateTime now = DateTime.Now;

                        string smsMent = "";
                        switch (request.IsSendSms)
                        {
                            case true: smsMent = "수신동의가"; break;
                            case false: smsMent = "수신거부가"; break;
                            //default: smsMent = "수신여부 변경이"; break;
                        }
                        smsMent = "[한국경제TV] " + now.Month + "월 " + now.Day + "일 한국경제TV에 요청하신 " + smsMent + " 정상처리되었습니다.";

                        db51_ARSsms.USP_SC_TRAN_INSERT(mobile2, "15990700", smsMent, "wowsms-member", "문자수신여부변경(회원정보수정)", userInfo.userId, "", "", "", "");
                    }
                }

                // 카페 정보 수정
                db49_wowcafe.Database.ExecuteSqlCommand("UPDATE CAFEMEMBERINFO SET EMAIL = {0}, HPHONE = {1} WHERE USERID = {2}", email, mobile, userInfo.userId);

                // 변경사항
                result.ModifiedItemList = GetModifiedItemList(false, result.EmailChanged, null, request, userInfo, userDetailInfo, userPostInfo, userSmsSendInfo, 
                    userEmailSendInfo, companyDetailInfo, passwordChagned, naverInfo, kakaoInfo, facebookInfo);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                WowLog.Write("[MemberSaveGeneral]StackTrace: " + ex.StackTrace + "\r\nSource: " + ex.Source + "\r\nInnerException: " + ex.InnerException + "\r\nMessage: " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 문자인증을 통한 이름변경
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public NameChangeResult MemberNameChange(int userNumber, SmsCheckRequest request)
        {
            NameChangeResult retval = new NameChangeResult();
            retval.IsSuccess = false;

            AuthBiz authBiz = new AuthBiz();
            retval.SmsCheckResult = authBiz.SmsCheck(request);

            if (retval.SmsCheckResult.ReturnCode == 0)
            {
                List<tblUser_IPIN> ipinUserList = (from ipin in db89_wowbill.tblUser_IPIN.AsNoTracking()
                                                   join user in db89_wowbill.tblUser.AsNoTracking() on ipin.usernumber equals user.userNumber
                                                   where user.userNumber == userNumber && user.apply.Equals(true)
                                                   select ipin).ToList();

                if (ipinUserList.Count > 0)
                {
                    tblUser_IPIN ipinInfo = ipinUserList[0];
                    if (ipinInfo.DupInfo != retval.SmsCheckResult.DI)
                    {
                        retval.ReturnMessage = "본인인증 모듈 결과의 DI값과 회원님의 DI값이 서로 다릅니다.";
                    }
                    else
                    {
                        tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber).SingleOrDefault();
                        if (userInfo != null)
                        {
                            if (retval.SmsCheckResult.Name == userInfo.name)
                            {
                                retval.ReturnMessage = "본인인증 모듈의 이름결과값과 회원님의 이름정보값이 이미 동일합니다.";
                            }
                            else
                            {
                                userInfo.name = retval.SmsCheckResult.Name;
                                db89_wowbill.SaveChanges();
                                db89_wowbill.Database.ExecuteSqlCommand("EXEC usp_memberNameChange '{0}', '{1}'", retval.SmsCheckResult.Name, userInfo.userId);
                                retval.IsSuccess = true;
                                retval.ChangedUserName = retval.SmsCheckResult.Name;
                            }
                        }
                    }
                }
                else
                {
                    retval.ReturnMessage = retval.SmsCheckResult.ReturnMessage;
                }
            }

            return retval;
        }

        /// <summary>
        /// 비밀번호 변경
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="passwordOriginal"></param>
        /// <param name="passwordNew"></param>
        /// <param name="passwordConfirm"></param>
        /// <returns></returns>
        public PasswordChangeResult MemberPasswordChange(int userNumber, string passwordOriginal, string passwordNew, string passwordConfirm)
        {
            PasswordChangeResult passwordChangeResult = new PasswordChangeResult();
            passwordChangeResult.RETURN_MESSAGE = "NOT_CHANGED";

            if (string.IsNullOrEmpty(passwordOriginal) == false && string.IsNullOrEmpty(passwordNew) == false && string.IsNullOrEmpty(passwordConfirm) == false)
            {
                if (passwordNew == passwordConfirm)
                {
                    string encryptedPasswordOriginal = commonBiz.Encrypt("HASH", "SHA256", passwordOriginal);
                    string encryptedPasswordNew = commonBiz.Encrypt("HASH", "SHA256", passwordNew);
                    string setDate = DateTime.Now.AddMonths(6).ToString("yyyyMMdd");

                    passwordChangeResult = db89_wowbill.Database.SqlQuery<PasswordChangeResult>("EXEC NUP_MEMBER_PASSWORD_UPDATE {0}, {1}, {2}, {3}", userNumber, encryptedPasswordOriginal, encryptedPasswordNew, setDate).SingleOrDefault();
                }
            }

            return passwordChangeResult;
        }

        public void MemberPasswordDelay(int userNumber, DateTime alarmDate)
        {
            TblMemberPasswordHistory histroy = db89_wowbill.TblMemberPasswordHistory.Where(a => a.userNumber == userNumber).SingleOrDefault();
            if (histroy != null)
            {
                histroy.alarmDt = alarmDate.ToString("yyyyMMdd");
                db89_wowbill.SaveChanges();
            }
        }

        /// <summary>
        /// 카카오 계정 등록 여부
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="kakaoId"></param>
        /// <returns></returns>
        public tblUserSNSKakao GetKakaoUserInfoExists(int? userNumber, long kakaoId)
        {
            tblUserSNSKakao kakaoInfo = null;
            if (userNumber.HasValue == true)
            {
                kakaoInfo = db89_wowbill.tblUserSNSKakao.Where(a => a.userNumber != userNumber.Value && a.id == kakaoId).SingleOrDefault();
            }
            else
            {
                kakaoInfo = db89_wowbill.tblUserSNSKakao.Where(a => a.id == kakaoId).SingleOrDefault();
            }
            return kakaoInfo;
        }

        /// <summary>
        /// 페이스북 계정 등록 여부
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="facebookId"></param>
        /// <returns></returns>
        public tblUserSNSFacebook GetFacebookUserInfoExists(int? userNumber, long facebookId)
        {
            List< tblUserSNSFacebook> facebookInfoList = null;
            if (userNumber.HasValue == true)
            {
                facebookInfoList = db89_wowbill.tblUserSNSFacebook.AsNoTracking().Where(a => a.userNumber != userNumber.Value && a.id == facebookId).ToList();
            }
            else
            {
                facebookInfoList = db89_wowbill.tblUserSNSFacebook.AsNoTracking().Where(a => a.id == facebookId).ToList();
            }

            if (facebookInfoList != null && facebookInfoList.Count > 0)
            {
                return facebookInfoList[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 네이버 계정 등록 여부
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="naverId"></param>
        /// <returns></returns>
        public tblUserSNSNaver GetNaverUserInfoExists(int? userNumber, long naverId)
        {
            tblUserSNSNaver naverUserInfo = null;
            if (userNumber.HasValue == true)
            {
                naverUserInfo = db89_wowbill.tblUserSNSNaver.Where(a => a.userNumber != userNumber.Value && a.id == naverId).SingleOrDefault();
            }
            else
            {
                naverUserInfo = db89_wowbill.tblUserSNSNaver.Where(a => a.id == naverId).SingleOrDefault();
            }
            return naverUserInfo;
        }

        /// <summary>
        /// 카카오 계정 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public tblUserSNSKakao GetKakaoUserInfo(int userNumber)
        {
            tblUserSNSKakao kakaoInfo = db89_wowbill.tblUserSNSKakao.Where(a => a.userNumber == userNumber).SingleOrDefault();
            return kakaoInfo;
        }

        /// <summary>
        /// 페이스북 계정 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public tblUserSNSFacebook GetFacebookUserInfo(int userNumber)
        {
            tblUserSNSFacebook facebookInfo = db89_wowbill.tblUserSNSFacebook.Where(a => a.userNumber == userNumber).SingleOrDefault();
            return facebookInfo;
        }

        /// <summary>
        /// 네이버 계정 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public tblUserSNSNaver GetNaverUserInfo(int userNumber)
        {
            tblUserSNSNaver naverUserInfo = db89_wowbill.tblUserSNSNaver.Where(a => a.userNumber == userNumber).SingleOrDefault();
            return naverUserInfo;
        }

        /// <summary>
        /// 선택 동의사항 변경
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="option2"></param>
        /// <returns></returns>
        public AgreementOption2ChangeResult MemberAgreementOption2Change(int userNumber, string option2)
        {
            AgreementOption2ChangeResult retval = new AgreementOption2ChangeResult();
            retval.IsSuccess = false;
            retval.ReturnMessage = "";

            tblUser userInfo = db89_wowbill.tblUser.Where(tbl => tbl.userNumber == userNumber).SingleOrDefault();
            if (userInfo != null)
            {
                userInfo.option2 = option2;
                try
                {
                    db89_wowbill.SaveChanges();
                    retval.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    retval.IsSuccess = false;
                    retval.ReturnMessage = ex.Message;
                }
            }

            return retval;
        }

        public MemberSecessionCheckResult MemberSecessionCheck(int userNumber, string bOQv5BillHost)
        {
            MemberSecessionCheckResult retval = new MemberSecessionCheckResult();
            retval.PossibleSecession = true;

            tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber).SingleOrDefault();
            string userId = userInfo.userId;

            MyPageBiz myPageBiz = new MyPageBiz();
            BillBalance billBalance = myPageBiz.GetWowTvBalance(new LoginUserInfo() { UserId = userId }, bOQv5BillHost);
            retval.RealCash = billBalance.sb_cashreal;
            retval.BonusCash = billBalance.sb_cashbonus;

            if (billBalance.sb_cashreal > 0)
            {
                retval.PossibleSecession = false;
                retval.HasCash = true;
            }

            // 이벤트 캐시 보유여부 (이벤트 캐시는 탈퇴 가능)
            retval.HasEventCash = db89_WOWTV_BILL_DB.TAccountCashMst.Where(a => a.UserNo == userNumber && a.Cash > 0 && a.CashAttr == "event(m)-all").Count() > 0;

            // 최대 사용가능한 기간 정보
            TCashMst tCashMst = db89_WOWTV_BILL_DB.TCashMst.Where(a => a.UserNo == userNumber).OrderByDescending(a => a.EYMD).Take(1).SingleOrDefault();
            if (tCashMst != null && tCashMst.EYMD != null && tCashMst.EYMD.Length == 8)
            {
                retval.CashLimitYear = tCashMst.EYMD.Substring(0, 4);
                retval.CashLimitMonth = tCashMst.EYMD.Substring(4, 2);
                retval.CashLimitDay = tCashMst.EYMD.Substring(6, 2);
            }

            // 와우캐시 사용가능 개수
            int accountCashCount = db89_WOWTV_BILL_DB.TAccountCashMst.Where(a => a.UserNo == userNumber && a.CashAttr != "event(m)-all" && a.Cash > 0).Count();
            if (accountCashCount > 0)
            {
                retval.PossibleSecession = false;
                retval.HasCashAttr = true;
            }

            DateTime now = DateTime.Now;
            string nowString = now.ToString("yyyyMMdd");

            // 사용자가 보유하고 있는 전문가 상품 수
            int analCount = (from A in db89_WOWTV_BILL_DB.TItemHoldMst.AsNoTracking()
                             join B in db89_WOWTV_BILL_DB.TItemMst.AsNoTracking() on A.ItemPackID equals B.ItemID
                             join C in db89_WOWTV_BILL_DB.TItemPriceMst.AsNoTracking() on A.ItemPackPriceID equals C.ItemPriceID
                             join D in db89_WOWTV_BILL_DB.TItemPriceMst.AsNoTracking() on A.ItemPackPriceID equals D.ItemPriceID
                             where
                              A.UserNo == userNumber && C.ChargeType != 1 && A.ItemOrPackage == 1 && A.UseState == 1 &&
                              A.ConsumeDate <= now && A.ExpDate >= now && (A.MaxAccessCnt == 0 || A.MaxAccessCnt > A.AccessedCnt) &&
                              (
                                (string.IsNullOrEmpty(A.StopStartYMD) == true && string.IsNullOrEmpty(A.StopEndYMD) == true)
                                ||
                                A.StopStartYMD.CompareTo(nowString) < 0
                                ||
                                A.StopEndYMD.CompareTo(nowString) > 0
                              )
                             select A.ItemPackPriceID).ToList().Count();
            //int analCount = db89_WOWTV_BILL_DB.UP_PORTAL_USER_ANAL_LST(userNumber).ToList().Count;
            if (analCount > 0)
            {
                retval.PossibleSecession = false;
            }
            retval.HasAnalItem = analCount > 0;

            return retval;
        }


        /// <summary>
        /// 탈퇴처리
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="secessionReasonKey"></param>
        /// <param name="secessionReasonDescription"></param>
        /// <returns></returns>
        public MemberSecessionResult MemberSecession(int userNumber, string secessionReasonKey, string secessionReasonDescription)
        {
            MemberSecessionResult retval = new MemberSecessionResult();
            retval.IsSuccess = false;

            tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber).SingleOrDefault();
            if (userInfo == null)
            {
                retval.IsSuccess = false;
                retval.ReturnMessage = "NO_USER";
                return retval;
            }

            string userId = userInfo.userId;

            try
            {
                // 탈퇴 (wowbill)
                db89_wowbill.Database.ExecuteSqlCommand("EXEC usp_memberSecession {0}", userNumber);

                // 사용자 변경 이력 저장 (wowbill)
                db89_wowbill.Database.ExecuteSqlCommand("EXEC sp_InsertUserHistoryByUserNumber {0}, {1}, {2}", userNumber, "삭제", "user");

                // K70419: 카페탈퇴 (wowcafe)
                db49_wowcafe.Database.ExecuteSqlCommand("EXEC usp_Update_MemberSecedeALL {0}", userId);

                // K80424: 클럽서비스 탈퇴 (wowbill)
                string clubServiceSql = "UPDATE TBL_Master_MEMBERS SET CLUBID = SUBSTRING(CLUBID, 1, 1) + '0', UNREGISTDT = GETDATE()," +
                    "APPLY = CASE SUBSTRING(CLUBID, 2, 1) WHEN '2' THEN '0' WHEN '0' THEN '0' ELSE APPLY END " +
                    "WHERE USERNUMBER = {0} AND APPLY = '1'";
                db89_wowbill.Database.ExecuteSqlCommand(clubServiceSql, userNumber);

                // K100218: 아웃바운드 삭제 (wowbill)
                string outboundSql = "UPDATE TBLUSER_DREAMNET SET APPLY = '0' WHERE USER_ID = {0} AND APPLY = '1'";
                db89_wowbill.Database.ExecuteSqlCommand(outboundSql, userId);

                // 탈퇴사유 (wownet)
                string reasonSql = "INSERT INTO TAB_MEMBER_SECESSION (USERNUMBER, USERID, SECESSIONID, SECESSIONVALUE) VALUES ({0}, {1}, {2}, {3})";
                db49_wownet.Database.ExecuteSqlCommand(reasonSql, userNumber, userId, secessionReasonKey, secessionReasonDescription);

                // SNS 정보 삭제
                string naverSql = "DELETE FROM tblUserSNSNaver WHERE userNumber = {0}";
                db89_wowbill.Database.ExecuteSqlCommand(naverSql, userNumber);
                string kakaoSql = "DELETE FROM tblUserSNSKakao WHERE userNumber = {0}";
                db89_wowbill.Database.ExecuteSqlCommand(kakaoSql, userNumber);
                string facebookSql = "DELETE FROM tblUserSNSFacebook WHERE userNumber = {0}";
                db89_wowbill.Database.ExecuteSqlCommand(facebookSql, userNumber);

                // 성공처리
                retval.IsSuccess = true;
                retval.ReturnMessage = "";
            }
            catch (Exception ex)
            {
                retval.IsSuccess = false;
                retval.ReturnMessage = ex.Message;
            }

            return retval;
        }

        /// <summary>
        /// 비밀번호 일치여부 확인
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool PasswordConfirm(int userNumber, string password)
        {
            bool retval = false;
            tblUser user = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber).SingleOrDefault();
            if (user != null)
            {
                string encPassword = commonBiz.Encrypt("HASH", "SHA256", password);
                if (encPassword == user.password)
                {
                    retval = true;
                }
            }
            return retval;
        }

        ///// <summary>
        ///// 개인 충전/캐시 정보
        ///// </summary>
        //public int CheckPaidCash(int userNumber, PriceIdType priceId, string bOQv5BillHost)
        //{
        //    BOQv7BillLib.BillClass bill = new BOQv7BillLib.BillClass();
        //    bill.TxCmd = "checkpaid";
        //    bill.HOST = bOQv5BillHost;
        //    bill.SetField("userno", userNumber.ToString());
        //    bill.SetField("itemorpackage", "1");
        //    bill.SetField("priceid", ((int)priceId).ToString());
        //    bill.SetField("checkflag", "0");

        //    int retval = bill.StartAction();
        //    //string errorMessage = bill.ErrMsg;

        //    return retval;
        //}

        public MemberGradeModel GetMemberGrade(int userNumber)
        {
            MemberGradeModel retval = MemberGradeModel.Free;
            List<NUP_MEMBER_CAFE_CHARGE_SELECT_Result> goldPlusItemInfo = db89_WOWTV_BILL_DB.NUP_MEMBER_CAFE_CHARGE_SELECT(userNumber, 1212).ToList();
            if (goldPlusItemInfo.Count > 0)
            {
                retval = MemberGradeModel.GoldPlus;
            }
            else
            {
                List<NUP_MEMBER_CAFE_CHARGE_SELECT_Result> goldItemInfo = db89_WOWTV_BILL_DB.NUP_MEMBER_CAFE_CHARGE_SELECT(userNumber, 1211).ToList();
                if (goldItemInfo.Count > 0)
                {
                    retval = MemberGradeModel.Gold;
                }
            }
            return retval;
        }

        /// <summary>
        /// TV다시보기 권한정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="num"></param>
        /// <param name="bOQv5BillHost"></param>
        /// <returns></returns>
        public TvReplayAuthorityInfoModel GetTvReplayAuthorityInfo(TvReplayAuthorityInfoParameter parameter)
        {
            TvReplayAuthorityInfoModel tvReplayAuthorityInfo = new TvReplayAuthorityInfoModel();

            string userId = db89_wowbill.tblUser.Where(a => a.userNumber == parameter.UserNumber).Select(a => a.userId).SingleOrDefault();
            tvReplayAuthorityInfo.UserInfoExist = !string.IsNullOrEmpty(userId);
            tvReplayAuthorityInfo.PriceId = parameter.PriceId;

            if (tvReplayAuthorityInfo.UserInfoExist == true)
            {
                tvReplayAuthorityInfo.MemberGrade = GetMemberGrade(parameter.UserNumber);
                if (tvReplayAuthorityInfo.MemberGrade == MemberGradeModel.GoldPlus)
                {
                    tvReplayAuthorityInfo.PointPaid = true;
                }
                else
                {
                    BOQv7BillLib.BillClass checkPaidBill = new BOQv7BillLib.BillClass();
                    checkPaidBill.TxCmd = "checkpaid";
                    checkPaidBill.HOST = parameter.BOQv5BillHost;
                    checkPaidBill.CodePage = 0;
                    checkPaidBill.SetField("userno", parameter.UserNumber.ToString());
                    checkPaidBill.SetField("itemorpackage", "1");
                    checkPaidBill.SetField("priceid", parameter.PriceId);
                    checkPaidBill.SetField("checkflag", "0");
                    checkPaidBill.SetField("cpitemid", "VOD_" + parameter.Num.ToString());
                    int checkPaidBillResult = checkPaidBill.StartAction();
                    string paid = checkPaidBill.GetVal("paid");
                    if (paid == "1")
                    {
                        tvReplayAuthorityInfo.PointPaid = true;
                    }
                    WowLog.Write("[CheckPaidBill] UserNumber: " + parameter.UserNumber + ", ReturnCode: " + checkPaidBillResult + ", ReturnMessage: " + checkPaidBill.ErrMsg);

                    //NUP_TVREPLAY_PAYMENT paymentInfo = db89_WOWTV_BILL_DB.NUP_TVREPLAY_PAYMENT
                    //    .Where(a => a.usernumber == userNumber && a.num == num).Take(1).OrderByDescending(a => a.seq).SingleOrDefault();

                    //if (paymentInfo != null)
                    //{
                    //    string paymentYear = paymentInfo.paydt.Year.ToString();
                    //    string paymentMonth = string.Format("{0:00}", paymentInfo.paydt.Month);
                    //    long paymentChargeNo = paymentInfo.chargeno;

                    //    string query = "DECLARE @po_intRecordCnt INT\r\n" +
                    //        "EXEC[dbo].[UP_PORTAL_PRODUCT_NEW_LST]\r\n" +
                    //        "@pi_intSearchType = 2,\r\n" +
                    //        "@pi_strSearchKey = '" + userNumber + "',\r\n" +
                    //        "@pi_strYear = '"+ paymentYear + "',\r\n" +
                    //        "@pi_strMonth = '"+ paymentMonth + "',\r\n" +
                    //        "@pi_intCategoryFlag = 0,\r\n" +
                    //        "@pi_strCategoryID = NULL,\r\n" +
                    //        "@pi_intTabFlag = 2,\r\n" +
                    //        "@pi_intUseState = 1,\r\n" +
                    //        "@pi_intPageNo = 1,\r\n" +
                    //        "@pi_intPageSize = 10,\r\n" +
                    //        "@po_intRecordCnt = @po_intRecordCnt OUTPUT";
                    //    //string query = "DECLARE @po_intRecordCnt INT\r\n" +
                    //    //    "EXEC[dbo].[UP_PORTAL_PRODUCT_NEW_LST]\r\n" +
                    //    //    "@pi_intSearchType = 2,\r\n" +
                    //    //    "@pi_strSearchKey = '100919739',\r\n" +
                    //    //    "@pi_strYear = '2017',\r\n" +
                    //    //    "@pi_strMonth = '12',\r\n" +
                    //    //    "@pi_intCategoryFlag = 0,\r\n" +
                    //    //    "@pi_strCategoryID = NULL,\r\n" +
                    //    //    "@pi_intTabFlag = 2,\r\n" +
                    //    //    "@pi_intUseState = 1,\r\n" +
                    //    //    "@pi_intPageNo = 1,\r\n" +
                    //    //    "@pi_intPageSize = 1,\r\n" +
                    //    //    "@po_intRecordCnt = @po_intRecordCnt OUTPUT\r\n";

                    //    List<UP_PORTAL_PRODUCT_NEW_LST_Result> billingInfo = db89_WOWTV_BILL_DB.Database.SqlQuery<UP_PORTAL_PRODUCT_NEW_LST_Result>(query).ToList();

                    //    foreach (UP_PORTAL_PRODUCT_NEW_LST_Result item in billingInfo)
                    //    {
                    //        if (item.CHARGENO == paymentChargeNo)
                    //        {
                    //            tvReplayAuthorityInfo.PointPaid = true;
                    //            break;
                    //        }
                    //    }
                    //}
                }

                NUP_MEMBER_INFO_SUB_SELECT_Result memberSubInfo = db89_WOWTV_BILL_DB.NUP_MEMBER_INFO_SUB_SELECT(parameter.UserNumber).FirstOrDefault();
                if (memberSubInfo != null)
                {
                    tvReplayAuthorityInfo.WowCash = memberSubInfo.WOW_CASH;
                }
            }

            return tvReplayAuthorityInfo;
        }

        /// <summary>
        /// TV 다시보기 포인트 결재
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="num"></param>
        /// <param name="ipAddress"></param>
        /// <param name="bOQv5BillHost"></param>
        public TvReplayPaymentByPointModel TvReplayPaymentByPoint(int userNumber, int num, string ipAddress, string bOQv5BillHost)
        {
            TvReplayPaymentByPointModel tvReplayPaymentByPoint = new TvReplayPaymentByPointModel();

            tv_program tvProgramInfo = db90_DNRS.tv_program.Where(a => a.Num == num).SingleOrDefault();
            if (tvProgramInfo != null)
            {
                string tvProgramId = tvProgramInfo.Dep;
                IMG_SCHEDULE scheduleInfo = db90_DNRS.IMG_SCHEDULE.Where(a => a.prog_id == tvProgramId).SingleOrDefault();
                if (scheduleInfo != null)
                {
                    tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber).SingleOrDefault();
                    tvReplayPaymentByPoint.UserInfoExist = userInfo != null;

                    if (tvReplayPaymentByPoint.UserInfoExist == true)
                    {
                        BOQv7BillLib.BillClass paymentBill = new BOQv7BillLib.BillClass();
                        paymentBill.TxCmd = "givefreeitem";
                        paymentBill.HOST = bOQv5BillHost;
                        paymentBill.CodePage = 0;
                        paymentBill.SetField("userno", userNumber.ToString());
                        paymentBill.SetField("userid", userInfo.userId);
                        paymentBill.SetField("username", userInfo.name);
                        paymentBill.SetField("itempriceid", "");
                        paymentBill.SetField("gamecode", "wowtv");
                        paymentBill.SetField("ipaddr", ipAddress);
                        paymentBill.SetField("adminid", "system");
                        paymentBill.SetField("chargeamt", scheduleInfo.point.ToString());
                        int billingResult = paymentBill.StartAction();
                        string billingMessage = paymentBill.ErrMsg;
                        WowLog.Write("[TvPaidPoint] UserNumber: " + userNumber + ", ReturnCode: " + billingResult + ", ReturnMessage: " + billingMessage);

                        string chargeNo = paymentBill.GetVal("chargeno");
                        if ((billingResult == 0 || billingResult == 2913) && string.IsNullOrEmpty(chargeNo) == false)
                        {
                            tvReplayPaymentByPoint.PaymentSuccess = true;
                        }
                        else
                        {
                            tvReplayPaymentByPoint.PaymentSuccess = false;
                            if (billingResult == 2029)
                            {// 와우캐시 부족
                                tvReplayPaymentByPoint.LowBalance = true;
                            }
                        }
                    }
                }
                /*
                       0: OK (캐시차감 및 상품지급 성공)
                    2001: 사용자 계정정보를 조회하지 못했습니다.
                    2002: 상품 정보를 조회하지 못했습니다.
                    2281: 상품 정보를 조회하지 못했습니다
                    5001: 상품 정보를 조회하지 못했습니다.
                    2913: 해당 상품의 구매는 1회만 가능합니다.
                    2029: 해당 상품의 구매는 1회만 가능합니다.
                */
            }

            return tvReplayPaymentByPoint;
        }

        /// <summary>
        /// 회원 정보 가져오기
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public tblUser GetMemberInfo(string userId)
        {
            var memberInfo = db89_wowbill.tblUser.AsQueryable().SingleOrDefault(x => x.userId.Equals(userId));
            return memberInfo;
        }

        /// <summary>
        /// 회원 정보 가져오기
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bak_tblUser GetDormancyMemberInfo(string userId)
        {
            var memberInfo = db89_Sleep_wowbill.bak_tblUser.AsQueryable().Where(x => x.userId.Equals(userId)).OrderByDescending(x => x.userNumber).FirstOrDefault();
            return memberInfo;
        }

        /// <summary>
        /// 일반/외국인 회원 정보 수정(쿠폰 사용 시 개인정보 동의화면에서 업데이트)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="memberUrl"></param>
        /// <param name="memberStyleUrl"></param>
        /// <param name="memberScriptUrl"></param>
        /// <returns></returns>
        public SaveUserResult CouponMemberSaveGeneral(SaveUserRequestGeneral request, string memberUrl, string memberStyleUrl, string memberScriptUrl)
        {
            SaveUserResult result = new SaveUserResult();

            try
            {
                tblUser userInfo = db89_wowbill.tblUser.AsNoTracking().Where(tbl => tbl.userNumber == request.UserNumber).SingleOrDefault();
                tblUserDetail userDetailInfo = db89_wowbill.tblUserDetail.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSMS userSmsSendInfo = db89_wowbill.tblUserSMS.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserEmailSend userEmailSendInfo = db89_wowbill.tblUserEmailSend.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserPost userPostInfo = db89_wowbill.tblUserPost.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSNSNaver naverInfo = db89_wowbill.tblUserSNSNaver.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSNSKakao kakaoInfo = db89_wowbill.tblUserSNSKakao.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();
                tblUserSNSFacebook facebookInfo = db89_wowbill.tblUserSNSFacebook.AsNoTracking().Where(a => a.userNumber == request.UserNumber).SingleOrDefault();

                string tel = request.Tel1 + "-" + request.Tel2 + "-" + request.Tel3;
                string mobile = request.Mobile1 + "-" + request.Mobile2 + "-" + request.Mobile3;
                string email = request.Email1 + "@" + request.Email2;
                string address = request.Address1 + " " + request.Address2;

                // 정보 수정
                string executeSql =
                    "DECLARE @RETURN_NUMBER INT\r\n" +
                    "DECLARE @RETURN_MESSAGE VARCHAR(4000)\r\n" +
                    "EXEC NUP_MEMBER_UPDATE_GENERAL\r\n" +
                    "@USERNUMBER={0},@NICKNAME={1},@TEL={2},@MOBILE={3},@EMAIL={4},@IS_SEND_SMS={5},@IS_SEND_SMS_AD={6},@IS_SEND_EMAIL={7},@IS_SEND_EMAIL_AD={8}," +
                    "@SIDO={9},@GUGUN={10},@ZIPCODE={11},@ADDRESS={12},@SALARY_ID={13},@INVESTMENT_PERIOD_ID={14},@INVESTMENT_SCALE_ID={15},@INVESTMENT_PROPENSITY_ID={16}," +
                    "@INVESTMENT_PREFERENCE_OBJECT_ID={17},@STOCK_COMPANY_ID={18},@INFO_ACQUIREMENT_ID={19},@INTEREST_AREA={20},@JOB_ID={21},@REGIST_ROUTE_ID={22}," +
                    "@RETURN_NUMBER=@RETURN_NUMBER OUTPUT,@RETURN_MESSAGE=@RETURN_MESSAGE OUTPUT\r\n" +
                    "SELECT @RETURN_NUMBER AS ReturnNumber, @RETURN_MESSAGE AS ReturnMessage, CASE WHEN @RETURN_NUMBER = 0 THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END AS IsSuccess";

                result = db89_wowbill.Database.SqlQuery<SaveUserResult>(executeSql, request.UserNumber, request.NickName, tel, mobile, email, request.IsSendSms, request.IsSendSmsAd,
                    request.IsSendEmail, request.IsSendEmailAd, request.Sido, request.Gugun, request.ZipCode, address, request.SalaryId, request.InvestmentPeriodId, request.InvestmentScaleId, request.InvestmentPropensityId,
                    request.InvestmentPreferenceObjectId, request.StockCompany, request.InfoAcquirementId, request.InterestArea, request.JobId, request.RegistRouteId).SingleOrDefault();

                if (result.IsSuccess == false)
                {
                    return result;
                }

                // 메일정보 수정 시 인증메일 발송
                result.EmailChanged = commonBiz.ToStringNullToEmpty(userInfo.email) != commonBiz.ToStringNullToEmpty(email);

                // SMS 수신여부
                bool isSendSmsFromDB = userInfo.isSendSMS == "0" || string.IsNullOrEmpty(userInfo.isSendSMS) == true ? false : true;
                bool validatedMobile = string.IsNullOrEmpty(request.Mobile1) == false && string.IsNullOrEmpty(request.Mobile2) == false && string.IsNullOrEmpty(request.Mobile3) == false;
                if (isSendSmsFromDB != request.IsSendSms && validatedMobile == true)
                {
                    string coupledMobileNo = request.Mobile1 + request.Mobile2 + request.Mobile3;
                    if (coupledMobileNo != "01000000000")
                    {
                        DateTime now = DateTime.Now;

                        string smsMent = "";
                        switch (request.IsSendSms)
                        {
                            case true: smsMent = "수신동의가"; break;
                            case false: smsMent = "수신거부가"; break;
                                //default: smsMent = "수신여부 변경이"; break;
                        }
                        smsMent = "[한국경제TV] " + now.Month + "월 " + now.Day + "일 한국경제TV에 요청하신 " + smsMent + " 정상처리되었습니다.";

                        db51_ARSsms.USP_SC_TRAN_INSERT(coupledMobileNo, "15990700", smsMent, "wowsms-member", "문자수신여부변경(회원정보수정)", userInfo.userId, "", "", "", "");
                    }
                }

                // 카페 정보 수정
                db49_wowcafe.Database.ExecuteSqlCommand("UPDATE CAFEMEMBERINFO SET EMAIL = {0}, HPHONE = {1} WHERE USERID = {2}", email, mobile, userInfo.userId);

                // 변경사항
                result.ModifiedItemList = GetModifiedItemList(true, result.EmailChanged, request, null, userInfo, userDetailInfo, userPostInfo, userSmsSendInfo, userEmailSendInfo, null, null, naverInfo, kakaoInfo, facebookInfo);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
            }

            return result;
        }

        public List<string> GetModifiedItemList(bool isGeneral, bool emailChanged, SaveUserRequestGeneral requestGeneral, SaveUserRequestCompany requestCompany,
            tblUser userInfo, tblUserDetail userDetailInfo, tblUserPost userPostInfo, tblUserSMS userSmsSendInfo, tblUserEmailSend userEmailSendInfo, tblCompanyDetail companyDetailInfo,
            PasswordChangeResult passwordChagned, tblUserSNSNaver naverInfo, tblUserSNSKakao kakaoInfo, tblUserSNSFacebook facebookInfo)
        {
            SaveUserRequestGeneral _requestGeneral = new SaveUserRequestGeneral(); if (requestGeneral != null) _requestGeneral = requestGeneral;
            SaveUserRequestCompany _requestCompany = new SaveUserRequestCompany(); if (requestCompany != null) _requestCompany = requestCompany;
            tblUser _userInfo = new tblUser(); if (userInfo != null) _userInfo = userInfo;
            tblUserDetail _userDetailInfo = new tblUserDetail(); if (userDetailInfo != null) _userDetailInfo = userDetailInfo;
            tblUserPost _userPostInfo = new tblUserPost(); if (userPostInfo != null) _userPostInfo = userPostInfo;
            tblUserSMS _userSmsSendInfo = new tblUserSMS(); if (userSmsSendInfo != null) _userSmsSendInfo = userSmsSendInfo;
            tblUserEmailSend _userEmailSendInfo = new tblUserEmailSend(); if (userEmailSendInfo != null) _userEmailSendInfo = userEmailSendInfo;
            tblCompanyDetail _companyDetailInfo = new tblCompanyDetail(); if (companyDetailInfo != null) _companyDetailInfo = companyDetailInfo;
            PasswordChangeResult _passwordChagned = new PasswordChangeResult(); if (passwordChagned != null) _passwordChagned = passwordChagned;
            tblUserSNSNaver _naverInfo = new tblUserSNSNaver(); if (naverInfo != null) _naverInfo = naverInfo;
            tblUserSNSKakao _kakaoInfo = new tblUserSNSKakao(); if (kakaoInfo != null) _kakaoInfo = kakaoInfo;
            tblUserSNSFacebook _facebookInfo = new tblUserSNSFacebook(); if (facebookInfo != null) _facebookInfo = facebookInfo;

            string nickName = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.NickName) : commonBiz.ToStringNullToEmpty(_requestCompany.NickName);
            string tel1 = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Tel1) : commonBiz.ToStringNullToEmpty(_requestCompany.Tel1);
            string tel2 = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Tel2) : commonBiz.ToStringNullToEmpty(_requestCompany.Tel2);
            string tel3 = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Tel3) : commonBiz.ToStringNullToEmpty(_requestCompany.Tel3);
            string mobile1 = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Mobile1) : commonBiz.ToStringNullToEmpty(_requestCompany.Mobile1);
            string mobile2 = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Mobile2) : commonBiz.ToStringNullToEmpty(_requestCompany.Mobile2);
            string mobile3 = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Mobile3) : commonBiz.ToStringNullToEmpty(_requestCompany.Mobile3);
            string email1 = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Email1) : commonBiz.ToStringNullToEmpty(_requestCompany.Email1);
            string email2 = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Email2) : commonBiz.ToStringNullToEmpty(_requestCompany.Email2);
            string address1 = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Address1) : commonBiz.ToStringNullToEmpty(_requestCompany.Address1);
            string address2 = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Address2) : commonBiz.ToStringNullToEmpty(_requestCompany.Address2);
            bool isSendSms = isGeneral == true ? _requestGeneral.IsSendSms : _requestCompany.IsSendSms;
            bool isSendSmsAd = isGeneral == true ? _requestGeneral.IsSendSmsAd : _requestCompany.IsSendSmsAd;
            bool isSendEmail = isGeneral == true ? _requestGeneral.IsSendEmail : _requestCompany.IsSendEmail;
            bool isSendEmailAd = isGeneral == true ? _requestGeneral.IsSendEmailAd : _requestCompany.IsSendEmailAd;
            string sido = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Sido) : "";
            string gugun = isGeneral == true ? commonBiz.ToStringNullToEmpty(_requestGeneral.Gugun) : "";
            string ceoName = isGeneral == true ? "" : commonBiz.ToStringNullToEmpty(_companyDetailInfo.ceoName);
            string businessItem = isGeneral == true ? "" : commonBiz.ToStringNullToEmpty(_companyDetailInfo.businessItem);
            string businessCondition = isGeneral == true ? "" : commonBiz.ToStringNullToEmpty(_companyDetailInfo.businessCondition);
            string businessCategory = isGeneral == true ? "" : commonBiz.ToStringNullToEmpty(_companyDetailInfo.businessCategory);
            string corporationNumber = isGeneral == true ? "" : commonBiz.ToStringNullToEmpty(_companyDetailInfo.corporationNumber);
            string establishmentAnniversary = isGeneral == true ? "" : commonBiz.ToStringNullToEmpty(_companyDetailInfo.establishmentAnniversary);
            bool listed = isGeneral == true ? false : companyDetailInfo.listed;

            int? salaryId = isGeneral == true ? _requestGeneral.SalaryId : _requestCompany.SalaryId;
            int? investmentPeriodId = isGeneral == true ? _requestGeneral.InvestmentPeriodId : _requestCompany.InvestmentPeriodId;
            int? investmentScaleId = isGeneral == true ? _requestGeneral.InvestmentScaleId : _requestCompany.InvestmentScaleId;
            int? investmentPropensityId = isGeneral == true ? _requestGeneral.InvestmentPropensityId : _requestCompany.InvestmentPropensityId;
            int? stockCompanyId = isGeneral == true ? _requestGeneral.StockCompany : _requestCompany.StockCompany;
            int? investmentPreferenceObjectId = isGeneral == true ? _requestGeneral.InvestmentPreferenceObjectId : _requestCompany.InvestmentPreferenceObjectId;
            int? infoAcquirementId = isGeneral == true ? _requestGeneral.InfoAcquirementId : _requestCompany.InfoAcquirementId;
            int? jobId = isGeneral == true ? _requestGeneral.JobId : _requestCompany.JobId;
            string interestArea = isGeneral == true ? _requestGeneral.InterestArea : _requestCompany.InterestArea;
            long? naverId = isGeneral == true ? requestGeneral.NaverId : requestCompany.NaverId;
            long? kakaoId = isGeneral == true ? requestGeneral.KakaoId : requestCompany.KakaoId;
            long? facebookId = isGeneral == true ? requestGeneral.FacebookId : requestCompany.FacebookId;
            if (naverId.HasValue == false) naverId = 0;
            if (kakaoId.HasValue == false) kakaoId = 0;
            if (facebookId.HasValue == false) facebookId = 0;


            string tel = tel1 + tel2 + tel3;
            string mobile = mobile1 + mobile2 + mobile3;
            string email = email1 + email2;
            string address = address1 + address2;

            List<string> modifiedItemList = new List<string>();

            if (isGeneral == false)
            {
                if (commonBiz.ToStringNullToEmpty(_companyDetailInfo.ceoName) != commonBiz.ToStringNullToEmpty(ceoName))
                {
                    modifiedItemList.Add("대표자 이름");
                }
                if (commonBiz.ToStringNullToEmpty(_companyDetailInfo.businessItem) != commonBiz.ToStringNullToEmpty(businessItem))
                {
                    modifiedItemList.Add("사업종목");
                }
                if (commonBiz.ToStringNullToEmpty(_companyDetailInfo.businessCondition) != commonBiz.ToStringNullToEmpty(businessCondition))
                {
                    modifiedItemList.Add("사업상태");
                }
                if (commonBiz.ToStringNullToEmpty(_companyDetailInfo.businessCategory) != commonBiz.ToStringNullToEmpty(businessCategory))
                {
                    modifiedItemList.Add("사업분류(업종)");
                }
                if (commonBiz.ToStringNullToEmpty(_companyDetailInfo.corporationNumber) != commonBiz.ToStringNullToEmpty(corporationNumber))
                {
                    modifiedItemList.Add("법인 등록번호");
                }
                if (_companyDetailInfo.listed != listed)
                {
                    modifiedItemList.Add("상장여부");
                }
                if (commonBiz.ToStringNullToEmpty(_companyDetailInfo.establishmentAnniversary) != commonBiz.ToStringNullToEmpty(establishmentAnniversary))
                {
                    modifiedItemList.Add("창립기념일");
                }
            }
            if (_passwordChagned.RETURN_MESSAGE == "OK")
            {
                modifiedItemList.Add("비밀번호");
            }
            if (commonBiz.ToStringNullToEmpty(_userInfo.NickName) != commonBiz.ToStringNullToEmpty(nickName))
            {
                modifiedItemList.Add("필명");
            }
            if (commonBiz.ToStringNullToEmpty(_userInfo.tel).Replace("-", "") != commonBiz.ToStringNullToEmpty(tel))
            {
                modifiedItemList.Add("전화번호");
            }
            if (commonBiz.ToStringNullToEmpty(_userInfo.Mobile).Replace("-", "") != commonBiz.ToStringNullToEmpty(mobile))
            {
                modifiedItemList.Add("휴대폰번호");
            }
            if (emailChanged == true)
            {
                modifiedItemList.Add("이메일");
            }

            bool isSendSmsFromDb = _userSmsSendInfo.smsInfo == "0" || string.IsNullOrEmpty(_userSmsSendInfo.smsInfo) == true ? false : true;
            bool isSendSmsAdFromDb = _userSmsSendInfo.smsAd == "0" || string.IsNullOrEmpty(_userSmsSendInfo.smsAd) == true ? false : true;
            bool isSendEmailFromDb = _userInfo.isSendEmail;
            bool isSendEmailAdFromDb = _userEmailSendInfo.emailAD == "0" || string.IsNullOrEmpty(_userEmailSendInfo.emailAD) == true ? false : true;

            if (isSendSms != isSendSmsFromDb || isSendSmsAd != isSendSmsAdFromDb)
            {
                modifiedItemList.Add("SMS 수신 동의");
            }
            if (isSendEmail != isSendEmailFromDb || isSendEmailAd != isSendEmailAdFromDb)
            {
                modifiedItemList.Add("이메일 수신 동의");
            }

            if (isGeneral == true)
            {
                if (commonBiz.ToStringNullToEmpty(_userPostInfo.sido) != commonBiz.ToStringNullToEmpty(sido) ||
                    commonBiz.ToStringNullToEmpty(_userPostInfo.gugun) != commonBiz.ToStringNullToEmpty(gugun))
                {
                    modifiedItemList.Add("거주지역");
                }
            }

            if (_userDetailInfo != null)
            {
                if (commonBiz.ToStringNullToEmpty(_userDetailInfo.address).Replace(" ", "") != commonBiz.ToStringNullToEmpty(address).Replace(" ", ""))
                {
                    modifiedItemList.Add("주소");
                }
            }

            if (_userDetailInfo.salaryId != salaryId)
            {
                modifiedItemList.Add("연간수입");
            }
            if (_userDetailInfo.investmentPeriodId != investmentPeriodId)
            {
                modifiedItemList.Add("투자기간");
            }
            if (_userDetailInfo.investmentScaleId != investmentScaleId)
            {
                modifiedItemList.Add("투자규모");
            }
            if (_userDetailInfo.investmentPropensityId != investmentPropensityId)
            {
                modifiedItemList.Add("투자성향");
            }
            if (_userDetailInfo.stockCompanyId != stockCompanyId)
            {
                modifiedItemList.Add("주거래증권사");
            }
            if (_userDetailInfo.investmentPreferenceObjectId != investmentPreferenceObjectId)
            {
                modifiedItemList.Add("투자선호대상");
            }
            if (_userDetailInfo.infoAcquirementId != infoAcquirementId)
            {
                modifiedItemList.Add("기본정보습득처");
            }
            if (_userDetailInfo.jobId != jobId)
            {
                modifiedItemList.Add("직업");
            }
            if (commonBiz.ToStringNullToEmpty(_userDetailInfo.interestArea) != commonBiz.ToStringNullToEmpty(interestArea))
            {
                modifiedItemList.Add("관심분야");
            }
            if (_naverInfo.id != naverId.Value)
            {
                modifiedItemList.Add("네이버 계정 연결");
            }
            if (_kakaoInfo.id != kakaoId.Value)
            {
                modifiedItemList.Add("카카오 계정 연결");
            }
            if (_facebookInfo.id != facebookId.Value)
            {
                modifiedItemList.Add("페이스북 계정 연결");
            }

            return modifiedItemList;
        }

        /// <summary>
        /// SNS 계정 연동 변경
        /// </summary>
        /// <param name="memberSNSLink"></param>
        public void ChangeSNSLinkInfo(ChangeMemberSNSLinkInfo memberSNSLink)
        {
            if(memberSNSLink != null)
            {
                string sqlQuery = "";
                if (memberSNSLink.IsLink == "Y")
                {
                    switch (memberSNSLink.SNSType)
                    {
                        case "F":
                            sqlQuery = "INSERT INTO tblUserSNSFacebook (userNumber, id, email, name) VALUES ({0}, {1}, {2}, {3})";
                            db89_wowbill.Database.ExecuteSqlCommand(sqlQuery, memberSNSLink.UserNumber, memberSNSLink.SNSId, memberSNSLink.SNSEmail, memberSNSLink.SNSName);
                            break;
                        case "N":
                            sqlQuery = "INSERT INTO tblUserSNSNaver (userNumber, id, email, name) VALUES ({0}, {1}, {2}, {3})";
                            db89_wowbill.Database.ExecuteSqlCommand(sqlQuery, memberSNSLink.UserNumber, memberSNSLink.SNSId, memberSNSLink.SNSEmail, memberSNSLink.SNSName);
                            break;
                        case "K":
                            sqlQuery = "INSERT INTO tblUserSNSKakao (userNumber, id, email, nickname) VALUES ({0}, {1}, {2}, {3})";
                            db89_wowbill.Database.ExecuteSqlCommand(sqlQuery, memberSNSLink.UserNumber, memberSNSLink.SNSId, memberSNSLink.SNSEmail, memberSNSLink.SNSName);
                            break;
                    }
                }
                else if (memberSNSLink.IsLink == "N")
                {
                    switch (memberSNSLink.SNSType)
                    {
                        case "F":
                            sqlQuery = "DELETE tblUserSNSFacebook WHERE userNumber = {0} ";
                            db89_wowbill.Database.ExecuteSqlCommand(sqlQuery, memberSNSLink.UserNumber);
                            break;
                        case "N":
                            sqlQuery = "DELETE tblUserSNSNaver WHERE userNumber = {0}";
                            db89_wowbill.Database.ExecuteSqlCommand(sqlQuery, memberSNSLink.UserNumber);
                            break;
                        case "K":
                            sqlQuery = "DELETE tblUserSNSKakao WHERE userNumber = {0}";
                            db89_wowbill.Database.ExecuteSqlCommand(sqlQuery, memberSNSLink.UserNumber);
                            break;
                    }
                }
            }
        }

        public tblUserDetail GetUserDetail(int userNumber)
        {

            return db89_wowbill.tblUserDetail.SingleOrDefault(a => a.userNumber == userNumber);
        }

        public tblUserSMS GetUserSMS(int userNumber)
        {

            return db89_wowbill.tblUserSMS.SingleOrDefault(a => a.userNumber == userNumber);
        }

        public bool UserIdExists(string userId, bool onlyApplied)
        {
            bool retval = false;
            tblUser userInfo = null;
            if (onlyApplied == true)
            {
                userInfo = db89_wowbill.tblUser.Where(a => a.userId == userId && a.apply == true).FirstOrDefault();
            }
            else
            {
                userInfo = db89_wowbill.tblUser.Where(a => a.userId == userId).FirstOrDefault();
            }

            if (userInfo != null)
            {
                retval = true;
            }

            return retval;

        }

        /// <summary>
        /// 광고성SMS동의 변경
        /// </summary>
        public void ChangeSmsAdAggrement(int userNumber, string smsAd)
        {
            var data = db89_wowbill.tblUserSMS.SingleOrDefault(a => a.userNumber.Equals(userNumber));
            if(data != null)
            {
                data.smsAd = smsAd;
                db89_wowbill.SaveChanges();
            }
        }
    }

    public enum PriceIdType
    {
        /// <summary>
        /// 와우넷 증권대학
        /// </summary>
        WowFinanceEducation = 311,
        /// <summary>
        /// Gold회원
        /// </summary>
        Gold = 551
    }

    /// <summary>
    /// 회원등급
    /// </summary>
    public enum MemberGradeModel
    {
        GoldPlus,
        Gold,
        Free
    }

    public class TvReplayAuthorityInfoParameter
    {
        public int UserNumber { get; set; }
        public string BOQv5BillHost { get; set; }
        public int Num { get; set; }
        public string PriceId { get; set; }
    }


    public class TvReplayAuthorityInfoModel
    {
        public bool UserInfoExist { get; set; }
        public MemberGradeModel MemberGrade { get; set; }
        public bool PointPaid { get; set; }
        public decimal WowCash { get; set; }
        public string PriceId { get; set; }
    }

    public class TvReplayPaymentByPointModel
    {
        public bool UserInfoExist { get; set; }
        public bool PaymentSuccess { get; set; }
        public bool LowBalance { get; set; }
    }
}
