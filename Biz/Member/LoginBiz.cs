using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Fx;
using Wow.Tv.Middle.Model.Db51.ARSsms;
using Wow.Tv.Middle.Model.Db89.Sleep_wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;

namespace Wow.Tv.Middle.Biz.Member
{
    public class LoginBiz: BaseBiz
    {
        private MemberCommonBiz commonBiz = null;
        public LoginBiz()
        {
            commonBiz = new MemberCommonBiz();
        }

        public string[] EncryptCheck(string plainText)
        {
            string[] retval = new string[4];

            ObjectResult<string> dbSelect1 = db89_wowbill.NUP_ENC_SELECT("GENERAL_ENC", "NORMAL", plainText);
            string dbEncryptedPassword = dbSelect1.FirstOrDefault();

            ObjectResult<string> dbSelect2 = db89_wowbill.NUP_ENC_SELECT("GENERAL_DEC", "NORMAL", dbEncryptedPassword);
            string dbDecryptedPassword = dbSelect2.FirstOrDefault();

            string xEncryptedPassword = XdbCrypto.Encrypt(plainText);

            string xDecryptedPassword = XdbCrypto.Decrypt(xEncryptedPassword);

            retval[0] = dbEncryptedPassword;
            retval[1] = dbDecryptedPassword;
            retval[2] = xEncryptedPassword;
            retval[3] = xDecryptedPassword;

            return retval;
        }

        /// <summary>
        /// 로그인 체크 (아이디/비밀번호)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public tblUser LoginCheck(string userId, string password, RegistUserRequest request)
        {
            bool passwordChecked = false;
            tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userId == userId).Take(1).OrderByDescending(a => a.userNumber).SingleOrDefault();
            if (userInfo != null)
            {
                string encPassword = commonBiz.Encrypt("HASH", "SHA256", password);
                if (encPassword == userInfo.password)
                {
                    passwordChecked = true;

                    if(request != null)
                    {
                        if(request.NaverId != null)
                        {
                            string sqlQuery = "INSERT INTO tblUserSNSNaver (userNumber, id, email, name) VALUES ({0}, {1}, {2}, {3})";
                            db89_wowbill.Database.ExecuteSqlCommand(sqlQuery, userInfo.userNumber, request.NaverId, request.NaverEmail, request.NaverkName);
                        }
                        
                        if(request.KakaoId != null)
                        {
                            string sqlQuery = "INSERT INTO tblUserSNSKakao (userNumber, id, email, nickname) VALUES ({0}, {1}, {2}, {3})";
                            db89_wowbill.Database.ExecuteSqlCommand(sqlQuery, userInfo.userNumber, request.KakaoId, request.KakaoEmail, request.KakaoNickname);
                        }

                        if (request.FacebookId != null)
                        {
                            string sqlQuery = "INSERT INTO tblUserSNSFacebook (userNumber, id, email, name) VALUES ({0}, {1}, {2}, {3})";
                            db89_wowbill.Database.ExecuteSqlCommand(sqlQuery, userInfo.userNumber, request.FacebookId, request.FacebookEmail, request.FacebookName);
                        }

                    }

                }
            }
            return LoginCheckProcess(userInfo, passwordChecked);
        }

        public tblUser LoginCheckEncrypt(string encryptedUserId, string encryptedPassword)
        {
            bool passwordChecked = false;
            string userId = commonBiz.Encrypt("GENERAL_DEC", "NORMAL", encryptedUserId);
            string password = commonBiz.Encrypt("GENERAL_DEC", "NORMAL", encryptedPassword);
            tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userId == userId).Take(1).OrderByDescending(a => a.userNumber).SingleOrDefault();

            if (userInfo != null)
            {
                if (password == userInfo.password)
                {
                    passwordChecked = true;
                }
            }
            return LoginCheckProcess(userInfo, passwordChecked);
        }

        public tblUser LoginCheckTest(string userId)
        {
            tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userId == userId).Take(1).OrderByDescending(a => a.userNumber).SingleOrDefault();
            return LoginCheckProcess(userInfo, true);
        }

        private tblUser LoginCheckProcess(tblUser userInfo, bool passwordChecked)
        {
            tblUser retval = null;
            if (userInfo != null)
            {
                if (passwordChecked == true)
                {
                    if (userInfo.apply == true)
                    {
                        // 특정 회원 로그인ID 접속 제한 조치
                        List<TblMemberBlackList> blackList = db89_wowbill.TblMemberBlackList.Where(a => a.UserId == userInfo.userId).ToList();
                        if (blackList.Count > 0)
                        {
                            userInfo.IsBlackList = true;
                            userInfo.AlertText = blackList[0].AlertText;
                        }
                        else
                        {
                            userInfo.IsBlackList = false;
                            userInfo.AlertText = "";
                            userInfo.ShareUserId = commonBiz.Encrypt("GENERAL_ENC", "NORMAL", userInfo.userId);
                            userInfo.SharePassword = commonBiz.Encrypt("GENERAL_ENC", "NORMAL", userInfo.password);

                            if (userInfo.name == "휴면회원")
                            {
                                userInfo.IsHumanUser = true;
                            }
                            else
                            {
                                userInfo.IsHumanUser = false;
                            }

                            userInfo.RequiredPasswordChange = false;
                            ObjectResult<usp_MemberPasswordHistoryCheck_Result> historyCheck = db89_wowbill.usp_MemberPasswordHistoryCheck(userInfo.userId, DateTime.Now.AddMonths(6).ToString("yyyyMMdd"));
                            List<usp_MemberPasswordHistoryCheck_Result> historyCheckList = historyCheck.ToList();
                            if (historyCheckList.Count > 0)
                            {
                                usp_MemberPasswordHistoryCheck_Result historyCheckData = historyCheckList[0];
                                if (string.IsNullOrEmpty(historyCheckData.alarmDt) == false)
                                {
                                    try
                                    {
                                        if (historyCheckData.alarmDt.Length == 8)
                                        {
                                            int nTemp;
                                            if (int.TryParse(historyCheckData.alarmDt, out nTemp) == true)
                                            {
                                                int alarmYear = int.Parse(historyCheckData.alarmDt.Substring(0, 4));
                                                int alarmMonth = int.Parse(historyCheckData.alarmDt.Substring(4, 2));
                                                int alarmDay = int.Parse(historyCheckData.alarmDt.Substring(6, 2));
                                                DateTime alarmDate = new DateTime(alarmYear, alarmMonth, alarmDay);
                                                if (DateTime.Now >= alarmDate)
                                                {
                                                    userInfo.RequiredPasswordChange = true;
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                        }
                        retval = userInfo;
                    }
                    else
                    {
                        tblUser_approvalLog approvalLog = db89_wowbill.tblUser_approvalLog.Where(a => a.usernumber == userInfo.userNumber).OrderByDescending(a => a.seq).Take(1).SingleOrDefault();
                        if (approvalLog != null)
                        {
                            userInfo.Rejected = true;
                            userInfo.RejectedReason = approvalLog.reason;

                            retval = userInfo;
                        }
                    }
                }
            }

            return retval;
        }

        /// <summary>
        /// 카카오 통한 로그인 체크
        /// </summary>
        /// <param name="kakaoId"></param>
        /// <param name="kakaoEmail"></param>
        /// <param name="kakaoNickname"></param>
        /// <returns></returns>
        public tblUser LoginCheckByKakao(long kakaoId, string kakaoEmail, string kakaoNickname)
        {
            tblUserSNSKakao kakaoInfo = db89_wowbill.tblUserSNSKakao.Where(a => a.id == kakaoId).SingleOrDefault();
            if (kakaoInfo == null)
            {
                return null;
            }

            bool kakaoInfoChanged = false;
            if (kakaoInfo.email != kakaoEmail)
            {
                kakaoInfoChanged = true;
                kakaoInfo.email = kakaoEmail;
            }
            if (kakaoInfo.nickname != kakaoNickname)
            {
                kakaoInfoChanged = true;
                kakaoInfo.nickname = kakaoNickname;
            }
            if (kakaoInfoChanged == true)
            {
                db89_wowbill.SaveChanges();
            }

            tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == kakaoInfo.userNumber).SingleOrDefault();
            return LoginCheckProcess(userInfo, true);
        }

        /// <summary>
        /// 페이스북 통한 로그인 체크
        /// </summary>
        /// <param name="facebookId"></param>
        /// <param name="facebookEmail"></param>
        /// <param name="facebookName"></param>
        /// <returns></returns>
        public tblUser LoginCheckByFacebook(long facebookId, string facebookEmail, string facebookName)
        {
            tblUserSNSFacebook facebookInfo = db89_wowbill.tblUserSNSFacebook.Where(a => a.id == facebookId).SingleOrDefault();
            if (facebookInfo == null)
            {
                return null;
            }

            bool facebookInfoChanged = false;
            if (facebookInfo.email != facebookEmail)
            {
                facebookInfoChanged = true;
                facebookInfo.email = facebookEmail;
            }
            if (facebookInfo.name != facebookName)
            {
                facebookInfoChanged = true;
                facebookInfo.name = facebookName;
            }
            if (facebookInfoChanged == true)
            {
                db89_wowbill.SaveChanges();
            }

            tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == facebookInfo.userNumber).SingleOrDefault();
            return LoginCheckProcess(userInfo, true);
        }

        /// <summary>
        /// 네이버 통한 로그인 체크
        /// </summary>
        /// <param name="naverId"></param>
        /// <param name="naverEmail"></param>
        /// <param name="naverName"></param>
        /// <returns></returns>
        public tblUser LoginCheckByNaver(long naverId, string naverEmail, string naverName)
        {
            tblUserSNSNaver naverInfo = db89_wowbill.tblUserSNSNaver.Where(a => a.id == naverId).SingleOrDefault();
            if (naverInfo == null)
            {
                return null;
            }

            bool naverInfoChanged = false;
            if (naverInfo.email != naverEmail)
            {
                naverInfoChanged = true;
                naverInfo.email = naverEmail;
            }
            if (naverInfo.name != naverName)
            {
                naverInfoChanged = true;
                naverInfo.name = naverName;
            }
            if (naverInfoChanged == true)
            {
                db89_wowbill.SaveChanges();
            }

            tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == naverInfo.userNumber).SingleOrDefault();
            return LoginCheckProcess(userInfo, true);
        }

        /// <summary>
        /// 휴대폰 통한 휴면회원 체크
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile1"></param>
        /// <param name="mobile2"></param>
        /// <param name="mobile3"></param>
        /// <returns></returns>
        public AuthDormancyCheckMemberResult AuthDormancyCheckMember(string userId, string mobile1, string mobile2, string mobile3)
        {
            string mobile = mobile1 + "-" + mobile2 + "-" + mobile3;

            bak_tblUser backupUserInfo = db89_Sleep_wowbill.bak_tblUser.AsNoTracking().Where(a => a.userId == userId).OrderByDescending(a => a.userNumber).FirstOrDefault();
            //List<tblUser> userList = db89_wowbill.tblUser.Where(a => a.userId == userId && a.Mobile == mobile && a.apply == true).ToList();

            AuthDormancyCheckMemberResult retval = new AuthDormancyCheckMemberResult();

            if (backupUserInfo != null)
            {
                if (mobile == backupUserInfo.Mobile)
                {
                    int userNumber = backupUserInfo.userNumber;
                    db89_wowbill.usp_MemberRestoreDormancy(userNumber);
                    retval.IsSuccess = true;
                }
                else
                {
                    retval.IsSuccess = false;
                }
            }
            else
            {
                retval.IsSuccess = false;
            }
            
            return retval;
        }

        /// <summary>
        /// 아이핀 통한 휴면회원 체크
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AuthDormancyCheckIpinResult AuthDormancyCheckIpin(IpinCheckRequest request)
        {
            AuthDormancyCheckIpinResult retval = new AuthDormancyCheckIpinResult();

            AuthBiz authBiz = new AuthBiz();
            retval.IpinCheckResult = authBiz.IpinCheck(request);
            retval.IsSuccess = false;

            if (retval.IpinCheckResult.ReturnCode == 1) // 인증성공
            {
                retval.IsSuccess = true;

                string denialText;
                retval.IsDenial = DenialCheck(retval.IpinCheckResult.DupInfo, out denialText);
                retval.DenialText = denialText;

                if (retval.IsDenial == false)
                {
                    int? userNumber = commonBiz.GetDupInfoUserNumber(retval.IpinCheckResult.DupInfo);
                    if (userNumber.HasValue == true)
                    {
                        List<tblUser> userList = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber.Value && a.apply == true).ToList();
                        if (userList.Count > 0)
                        {
                            retval.IsSuccess = true;

                            db89_wowbill.usp_MemberRestoreDormancy(userNumber);
                        }

                    }
                }
            }

            authBiz.MemberIdSearchLog(retval.IpinCheckResult.VirtualNumber, retval.IpinCheckResult.Name, retval.IpinCheckResult.DupInfo, "", "", "IPIN_DOR");

            return retval;
        }

        public AuthDormancyCheckSmsResult AuthDormancyCheckSms(SmsCheckRequest request)
        {
            AuthDormancyCheckSmsResult retval = new AuthDormancyCheckSmsResult();

            AuthBiz authBiz = new AuthBiz();

            try
            {
                retval.SmsCheckResult = authBiz.SmsCheck(request);
                retval.IsSuccess = false;

                if (retval.SmsCheckResult.ReturnCode == 0)
                {
                    string denialText;
                    retval.IsDenial = DenialCheck(retval.SmsCheckResult.DI, out denialText);
                    retval.DenialText = denialText;

                    if (retval.IsDenial == false)
                    {
                        int? userNumber = commonBiz.GetDupInfoUserNumber(retval.SmsCheckResult.DI);
                        if (userNumber.HasValue == true)
                        {
                            List<tblUser> userList = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber.Value && a.apply == true).ToList();
                            if (userList.Count > 0)
                            {
                                retval.IsSuccess = true;
                                string executeSql = "EXEC dbo.usp_MemberRestoreDormancy {0}";
                                db89_wowbill.Database.ExecuteSqlCommand(executeSql, userNumber.Value);
                                //db89_wowbill.usp_MemberRestoreDormancy(userNumber.Value);
                            }

                        }
                    }
                }

                authBiz.MemberIdSearchLog("", retval.SmsCheckResult.Name, retval.SmsCheckResult.DI, request.RequestNo, request.RequestNo, "MOBILE_DOR");
            }
            catch (Exception ex)
            {
                WowLog.Write("[AuthDormancyCheckSms Failed] RequestNo: " + request.RequestNo + ", EncryptedData: " + request.EncryptedData + "\r\nError Message: " + ex.Message + "\r\nStackTrace: " + ex.StackTrace);
                throw ex;
            }
            return retval;
        }

        private bool DenialCheck(string dupInfo, out string denialText)
        {
            bool isDenial = true;
            denialText = "";
            if (dupInfo == "MC0GCCqGSIb3DQIJAyEAg1ikCBj6t6D0GNcvqt7SiJ+R+bOZHpJGOqciChdu9SU=")
            {
                denialText = "고객님께서는 한국경제TV 이용약관 제 2장 제 10조에 의거해 회원가입이 승낙되지 않았습니다.\n문의 : 1599-0700";
            }
            else if (
                dupInfo == "MC0GCCqGSIb3DQIJAyEAveKpF/2zG4rCYoW2CPKxke/xczgbLtRDjLlt8d2MUkk=" ||
                dupInfo == "MC0GCCqGSIb3DQIJAyEAhw45MSizGvRxk59jO8P4V3ba00ysvbtAgJ6rhPzs+0M=" ||
                dupInfo == "MC0GCCqGSIb3DQIJAyEAvbxcDUzl0LMmIbI08bHi0lkRDkPR30c4fzYRkFKMtMw=" ||
                dupInfo == "MC0GCCqGSIb3DQIJAyEADXBdpvWzSAVxV7legmxn1rlPEPi6qtYzynX7AMUSNwA=" ||
                dupInfo == "MC0GCCqGSIb3DQIJAyEA+Cc+RUkdUojQUhNakTLgot+8rRn5BLhj5rsntjHoApM=" ||
                dupInfo == "MC0GCCqGSIb3DQIJAyEAdqmSKczfPrj+zrp71RJ5CS53gnOHZsa3dTiGKBgtg54="
                )
            {
                denialText = "재가입 불가 회원입니다.";
            }
            else
            {
                isDenial = false;
            }
            return isDenial;
        }

        public string MobileSendSms(string mobile1, string mobile2, string mobile3)
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
            data.TR_CALLBACK = "15990700";
            data.TR_MSG = "[한국경제TV] 인증번호 [" + authNumber + "]를 3분내로 입력해주세요.";
            data.TR_ID = "wowsms-member";
            data.TR_ETC1 = "휴대폰인증아이디찾기";
            db51_ARSsms.SC_TRAN.Add(data);
            db51_ARSsms.SaveChanges();

            return authNumber;
        }

        public FindIdResult FindIdCheckSms(SmsCheckRequest request)
        {
            FindIdResult retval = new FindIdResult();
            retval.Success = false;

            AuthBiz authBiz = new AuthBiz();
            SmsCheckResult smsCheckResult = authBiz.SmsCheck(request);

            if (smsCheckResult.ReturnCode == 0)
            {
                int? userNumber = commonBiz.GetDupInfoUserNumber(smsCheckResult.DI);
                if (userNumber.HasValue == true)
                {
                    tblUser user = db89_wowbill.tblUser.AsNoTracking().Where(a => a.userNumber == userNumber.Value).Single();
                    retval.UserId = user.userId;
                    retval.RegistDate = user.registDt;
                    if (user.companyDetailId.HasValue == true)
                    {
                        retval.MemberType = MemberType.Company;
                        retval.Success = true;
                    }
                    else
                    {
                        string decSSNo = commonBiz.Encrypt("GENERAL_DEC", "NORMAL", user.ssno);
                        //string decSSNo = XdbCrypto.Decrypt(user.ssno);
                        //ObjectResult<string> ssnoSelect = db89_wowbill.NUP_ENC_SELECT("GENERAL_DEC", "NORMAL", user.ssno);
                        //string decSSNo = ssnoSelect.SingleOrDefault();
                        if (string.IsNullOrEmpty(decSSNo) == false && decSSNo.Length == 13)
                        {
                            char genderCode = decSSNo[6];
                            if (genderCode == '1' || genderCode == '2' || genderCode == '3' || genderCode == '4')
                            {
                                retval.MemberType = MemberType.General;
                            }
                            else if (genderCode == '5' || genderCode == '6' || genderCode == '7' || genderCode == '8')
                            {
                                retval.MemberType = MemberType.Foreign;
                            }
                            else
                            {
                                retval.MemberType = MemberType.Unknown;
                            }
                            retval.Success = true;
                        }
                        else
                        {
                            retval.Success = false;
                            retval.ErrorMessage = "잘못된 주민(외국인)번호 입니다.";
                        }
                    }
                }
            }

            authBiz.MemberIdSearchLog("", smsCheckResult.Name, smsCheckResult.DI, request.RequestNo, request.RequestNo, "MOBILE_DOR");

            return retval;
        }

        public FindIdResult FindIdCheckIpin(IpinCheckRequest request)
        {
            FindIdResult retval = new FindIdResult();
            retval.Success = false;

            AuthBiz authBiz = new AuthBiz();
            IpinCheckResult ipinCheckResult = authBiz.IpinCheck(request);

            if (ipinCheckResult.ReturnCode == 1) // 인증성공
            {
                int? userNumber = commonBiz.GetDupInfoUserNumber(ipinCheckResult.DupInfo);
                if (userNumber.HasValue == true)
                {
                    tblUser user = db89_wowbill.tblUser.AsNoTracking().Where(a => a.userNumber == userNumber).Single();
                    retval.UserId = user.userId;
                    retval.RegistDate = user.registDt;
                    if (user.companyDetailId.HasValue == true)
                    {
                        retval.MemberType = MemberType.Company;
                        retval.Success = true;
                    }
                    else
                    {
                        string decSSNo = commonBiz.Encrypt("GENERAL_DEC", "NORMAL", user.ssno);
                        //string decSSNo = XdbCrypto.Decrypt(user.ssno);
                        //ObjectResult<string> ssnoSelect = db89_wowbill.NUP_ENC_SELECT("GENERAL_DEC", "NORMAL", user.ssno);
                        //string decSSNo = ssnoSelect.SingleOrDefault();
                        if (string.IsNullOrEmpty(decSSNo) == false && decSSNo.Length == 13)
                        {
                            char genderCode = decSSNo[6];
                            if (genderCode == '1' || genderCode == '2' || genderCode == '3' || genderCode == '4')
                            {
                                retval.MemberType = MemberType.General;
                            }
                            else if (genderCode == '5' || genderCode == '6' || genderCode == '7' || genderCode == '8')
                            {
                                retval.MemberType = MemberType.Foreign;
                            }
                            else
                            {
                                retval.MemberType = MemberType.Unknown;
                            }
                            retval.Success = true;
                        }
                        else
                        {
                            retval.Success = false;
                            retval.ErrorMessage = "잘못된 주민(외국인)번호 입니다.";
                        }
                    }
                }
            }

            authBiz.MemberIdSearchLog(ipinCheckResult.VirtualNumber, ipinCheckResult.Name, ipinCheckResult.DupInfo, "", "", "IPIN_DOR");

            return retval;
        }

        /// <summary>
        /// 휴대폰번호 통한 아이디 정보 반환
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mobile1"></param>
        /// <param name="mobile2"></param>
        /// <param name="mobile3"></param>
        /// <returns></returns>
        public FindIdResult FindIdCheckByMobileInfo(string name, string mobile1, string mobile2, string mobile3)
        {
            FindIdResult retval = new FindIdResult();
            retval.Success = false;

            string mobile = mobile1 + "-" + mobile2 + "-" + mobile3;
            List<tblUser> userList = db89_wowbill.tblUser.AsNoTracking().Where(a => a.name == name && a.Mobile == mobile && a.apply == true).ToList();
            if (userList.Count > 0)
            {
                tblUser user = userList[0];
                retval.UserId = user.userId;
                retval.RegistDate = user.registDt;
                if (user.companyDetailId.HasValue == true)
                {
                    retval.MemberType = MemberType.Company;
                    retval.Success = true;
                }
                else
                {
                    string decSSNo = commonBiz.Encrypt("GENERAL_DEC", "NORMAL", user.ssno);
                    //string decSSNo = XdbCrypto.Decrypt(user.ssno);
                    //ObjectResult<string> ssnoSelect = db89_wowbill.NUP_ENC_SELECT("GENERAL_DEC", "NORMAL", user.ssno);
                    //string decSSNo = ssnoSelect.SingleOrDefault();
                    if (string.IsNullOrEmpty(decSSNo) == false && decSSNo.Length == 13)
                    {
                        char genderCode = decSSNo[6];
                        if (genderCode == '1' || genderCode == '2' || genderCode == '3' || genderCode == '4')
                        {
                            retval.MemberType = MemberType.General;
                        }
                        else if (genderCode == '5' || genderCode == '6' || genderCode == '7' || genderCode == '8')
                        {
                            retval.MemberType = MemberType.Foreign;
                        }
                        else
                        {
                            retval.MemberType = MemberType.Unknown;
                        }
                        retval.Success = true;
                    }
                    else
                    {
                        retval.Success = false;
                        retval.ErrorMessage = "잘못된 주민(외국인)번호 입니다.";
                    }
                }
            }
            return retval;
        }

        /// <summary>
        /// 아이디찾기 > SSNO (외국인등록번호/사업자번호)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ssno"></param>
        /// <param name="isCompany"></param>
        /// <returns></returns>
        public FindIdResult FindIdCheckBySSNoInfo(string name, string ssno, bool isCompany)
        {
            FindIdResult retval = new FindIdResult();
            retval.Success = false;

            if (isCompany == false)
            {
                ssno = ssno.Replace("-", "");
            }
            string encSSNo = commonBiz.Encrypt("GENERAL_ENC", "NORMAL", ssno);

			List<tblUser> userList = db89_wowbill.tblUser.AsNoTracking().Where(a => a.name == name && a.ssno == encSSNo && a.apply == true).ToList();
            if (userList.Count > 0)
            {
                tblUser user = userList[0];
                retval.UserId = user.userId;
                retval.RegistDate = user.registDt;
                if (user.companyDetailId.HasValue == true)
                {
                    retval.MemberType = MemberType.Company;
                    retval.Success = true;
                }
                else
                {
                    if (ssno.Length == 13)
                    {
                        char genderCode = ssno[6];
                        if (genderCode == '1' || genderCode == '2' || genderCode == '3' || genderCode == '4')
                        {
                            retval.MemberType = MemberType.General;
                        }
                        else if (genderCode == '5' || genderCode == '6' || genderCode == '7' || genderCode == '8')
                        {
                            retval.MemberType = MemberType.Foreign;
                        }
                        else
                        {
                            retval.MemberType = MemberType.Unknown;
                        }
                        retval.Success = true;
                    }
                    else
                    {
                        retval.Success = false;
                        retval.ErrorMessage = "잘못된 주민(외국인)번호 입니다.";
                    }
                }
            }
            return retval;
        }

        public FindPasswordResult FindPasswordCheckSms(SmsCheckRequest request)
        {
            FindPasswordResult retval = new FindPasswordResult();
            retval.Success = false;

            AuthBiz authBiz = new AuthBiz();
            SmsCheckResult smsCheckResult = authBiz.SmsCheck(request);

            if (smsCheckResult.ReturnCode == 0)
            {
                retval.MobileNo = smsCheckResult.MobileNo;

                int? userNumber = commonBiz.GetDupInfoUserNumber(smsCheckResult.DI, request.UserId);
                //int? userNumber = commonBiz.GetDupInfoUserNumber(smsCheckResult.DI);
                if (userNumber.HasValue == true)
                {
                    tblUser user = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber).Single();
                    retval.UserNumber = user.userNumber;
                    retval.UserId = user.userId;
                    retval.Email = user.email;
                    retval.Success = true;
                }
            }

            authBiz.MemberIdSearchLog("", smsCheckResult.Name, smsCheckResult.DI, request.RequestNo, request.RequestNo, "MOBILE_PW");

            return retval;
        }

        public FindPasswordResult FindPasswordCheckIpin(IpinCheckRequest request)
        {
            FindPasswordResult retval = new FindPasswordResult();
            retval.Success = false;

            AuthBiz authBiz = new AuthBiz();
            IpinCheckResult ipinCheckResult = authBiz.IpinCheck(request);

            if (ipinCheckResult.ReturnCode == 1) // 인증성공
            {
                int? userNumber = commonBiz.GetDupInfoUserNumber(ipinCheckResult.DupInfo, request.UserId);
                //int? userNumber = commonBiz.GetDupInfoUserNumber(ipinCheckResult.DupInfo);
                if (userNumber.HasValue == true)
                {
                    List<tblUser> userList = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber.Value && a.apply == true).ToList();                   
                    if (userList.Count > 0)
                    {
                        tblUser user = userList[0];
                        retval.UserNumber = user.userNumber;
                        retval.UserId = user.userId;
                        retval.MobileNo = user.Mobile;
                        retval.Email = user.email;
                        retval.Success = true;
                    }
                }
            }

            authBiz.MemberIdSearchLog(ipinCheckResult.VirtualNumber, ipinCheckResult.Name, ipinCheckResult.DupInfo, "", "", "IPIN_PW");

            return retval;
        }

        public FindPasswordResult FindPasswordCheckByMobileInfo(string userId, string name, string mobile1, string mobile2, string mobile3)
        {
            FindPasswordResult retval = new FindPasswordResult();
            retval.Success = false;

            string mobile = mobile1 + "-" + mobile2 + "-" + mobile3;
            retval.MobileNo = mobile;
            List<tblUser> userList = db89_wowbill.tblUser.Where(a => a.userId == userId && a.name == name && a.Mobile == mobile && a.apply == true).ToList();
            if (userList.Count > 0)
            {
                tblUser user = userList[0];
                retval.UserNumber = user.userNumber;
                retval.UserId = user.userId;
                retval.Email = user.email;
                retval.Success = true;
            }
            return retval;
        }


        public SetPasswordResult PasswordInitialize(string userId)
        {
            tblUser user = db89_wowbill.tblUser.SingleOrDefault(a => a.userId == userId && a.apply == true);

            SetPasswordResult retval = commonBiz.PasswordInitialize(user.userNumber, "user");

            return retval;
        }

        public SetPasswordResult SendTempPasswordToEmail(string userId)
        {
            SetPasswordResult retval = new SetPasswordResult();
            retval.Success = false;
            retval.TempPassword = commonBiz.GetTempPassword(6);

            string encPassword = commonBiz.Encrypt("HASH", "SHA256", retval.TempPassword);
            //string encPassword = XdbCrypto.Hash(tempPassword);
            //ObjectResult<string> passwordSelect = db89_wowbill.NUP_ENC_SELECT("HASH", "SHA256", tempPassword);
            //string encPassword = passwordSelect.FirstOrDefault();

            tblUser user = db89_wowbill.tblUser.SingleOrDefault(a => a.userId == userId && a.apply == true);
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.email) == true)
                {
                    retval.ReturnMessage = "올바르지 않은 메일주소 입니다.";
                }
                else if (user.email.Split('@').Length != 2)
                {
                    retval.ReturnMessage = "올바르지 않은 메일주소 입니다.";
                }
                else
                {
                    user.password = encPassword;
                    db89_wowbill.SaveChanges();

                    //'메일폼 작성----------------------------------------------------------------------------------------------
                    //Dim map_content
                    //Set fs = Server.CreateObject("Scripting.FileSystemObject")
                    //Set file = fs.OpenTextFile(Server.MapPath("/mailing/") & "/static/" & "pw.html", 1)
                    //map_content = file.readall
                    //map_content = replace(map_content, "@@webroot@@", "http://www.wownet.co.kr")    ' 웹경로
                    //map_content = replace(map_content, "@@imgroot@@", Application("CONF_IMG_SERVER"))  ' 이미지경로
                    //map_content = replace(map_content, "@@tvroot@@", Application("CONF_WOWTV_SERVER")) ' 티비경로
                    //map_content = replace(map_content, "@@caferoot@@", Application("CONF_WOWCAFE_SERVER")) ' 카페경로
                    //map_content = replace(map_content, "@@USER_NAME@@", pwBox.getString("name"))
                    //map_content = replace(map_content, "@@USER_ID@@", pwBox.getString("userid"))
                    //map_content = replace(map_content, "@@USER_PW@@", temp_password)
                    //map_content = replace(map_content, "@@include@@", Footer)  ' 하단 
                    //reqData.setString "map_content", map_content
                    //'----------------------------------------------------------------------------------------------------------
                    //reqData.setData "to_email", pwBox.getString("email")
                    //reqData.setData "to_name", pwBox.getString("name")
                    //reqData.setData "from_name", "개인정보관리자"
                    //reqData.setData "from_email", "webmaster@wownet.co.kr"
                    //reqData.setData "mail_title", "안녕하세요. 한국경제TV 고객센터 개인정보관리자입니다. "
                    //Set service2 = New emsService

                    retval.Success = true;
                }
            }
            else
            {
                retval.ReturnMessage = "사용자 정보가 없습니다.";
            }

            return retval;
        }

        public SetPasswordResult ModifyPassword(string userId, string password)
        {
            SetPasswordResult retval = new SetPasswordResult();
            retval.Success = false;

            string encPassword = commonBiz.Encrypt("HASH", "SHA256", password);
            //string encPassword = XdbCrypto.Hash(password);
            //ObjectResult<string> passwordSelect = db89_wowbill.NUP_ENC_SELECT("HASH", "SHA256", password);
            //string encPassword = passwordSelect.FirstOrDefault();

            tblUser user = db89_wowbill.tblUser.SingleOrDefault(a => a.userId == userId && a.apply == true);

            //20180130 로그추가
            //if (user != null)
            //{
            //    Wow.Fx.WowLog.Write("adminId: " + user.adminId + "/ email: " + user.email);
            //}
            //else
            //{
            //    Wow.Fx.WowLog.Write("유저가 없습니다.");
            //}
            //20180130 로그추가 끝


            if (user != null)
            {
                //if (string.IsNullOrEmpty(user.email) == true)
                //{
                //    retval.ReturnMessage = "올바르지 않은 이메일입니다.";
                //}
                //else if (user.email.Split('@').Length != 2)
                //{
                //    retval.ReturnMessage = "올바르지 않은 이메일입니다.";
                //}
                //else
                //{
                    user.password = encPassword;
                    db89_wowbill.SaveChanges();

                    retval.Success = true;
                //}
            }
            else
            {
                retval.ReturnMessage = "사용자 정보가 없습니다.";
            }

            return retval;
        }

        /// <summary>
        /// 로그인 로그 기록
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="ip"></param>
        public void LoginLog(int userNumber, string ip)
        {
            string executeSql = "EXEC dbo.NUP_MEMBER_LOGIN_LOG_INSERT {0}, {1}";
            db89_wowbill.Database.ExecuteSqlCommand(executeSql, userNumber, ip);
        }

        public void DomainLoginLog(int? userNumber, string userId, string webType, string webFrom, string webServerName, string loginSite, DateTime loginDate, string clientIp, string requestUrl)
        {
            NTB_USER_LOGIN_LOG loginLog = new NTB_USER_LOGIN_LOG();
            loginLog.USER_NUMBER = userNumber;
            loginLog.USER_ID = userId;
            loginLog.WEB_TYPE = webType;
            loginLog.WEB_FROM = webFrom;
            loginLog.WEB_SERVER_NAME = webServerName;
            loginLog.LOGIN_SITE = loginSite;
            loginLog.LOGIN_DT = loginDate;
            loginLog.CLIENT_IP = clientIp;
            loginLog.REQUEST_URL = requestUrl;
            db89_wowbill.NTB_USER_LOGIN_LOG.Add(loginLog);
            db89_wowbill.SaveChanges();
        }

        public void MakeLoginProcSSOUrl(string userId)
        {

        }
    }
}
