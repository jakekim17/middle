using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wow.Fx;
using Wow.Tv.Middle.Biz.Component;
using Wow.Tv.Middle.Model.Db51.ARSsms;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;

namespace Wow.Tv.Middle.Biz.Member
{
    public class MemberCommonBiz : BaseBiz
    {
        public string Encrypt(string type, string algorithm, string word)
        {
            string enc = "";
            string callType = "XdbCrypto";
            #if DEBUG
            callType = "DB";
            #endif

            try
            {
                if (callType == "DB")
                {
                    ObjectResult<string> select = db89_wowbill.NUP_ENC_SELECT(type, algorithm, word);
                    enc = select.FirstOrDefault();
                }
                else
                {
                    if (type == "GENERAL_ENC")
                    {
                        enc = XdbCrypto.Encrypt(word);
                    }
                    else if (type == "GENERAL_DEC")
                    {
                        enc = XdbCrypto.Decrypt(word);
                    }
                    else if (type == "HASH")
                    {
                        enc = XdbCrypto.Hash(word);
                    }
                }
            }
            catch (Exception ex)
            {
                WowLog.Write("[Encrypt Error] CallType: " + callType + ", Algorithm: " + algorithm + ", Word: " + word);
                throw ex;
            }

            return enc;
        }

        public SetPasswordResult PasswordInitialize(int userNumber, string adminId)
        {
            SetPasswordResult retval = new SetPasswordResult();
            retval.Success = false;
            retval.ReturnMessage = "";

            string tempPassword = GetTempPassword(6);

            string encPassword = Encrypt("HASH", "SHA256", tempPassword);

            tblUser user = db89_wowbill.tblUser.AsNoTracking().Where(a => a.userNumber == userNumber && a.apply == true).SingleOrDefault();
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.Mobile) == true)
                {
                    retval.ReturnMessage = "올바르지 않은 휴대폰번호 입니다.";
                }
                else
                {
                    string executeSql = "EXEC NUP_ADMIN_MEMBER_PASSWORD_UPDATE @USER_NUMBER={0}, @PASSWORD={1}, @ADMIN_ID={2}";
                    db89_wowbill.Database.ExecuteSqlCommand(executeSql, userNumber, encPassword, adminId);

                    retval.TempPassword = tempPassword;
                    retval.MobileNo = user.Mobile;
                    retval.Success = true;


                }
            }
            else
            {
                retval.ReturnMessage = "사용자 정보가 없습니다.";
            }

            return retval;
        }

        /// <summary>
        /// 비밀번호 변경 유효성 검사
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="passwordOriginal"></param>
        /// <param name="passwordNew"></param>
        /// <returns></returns>
        public bool PasswordValidation(int userNumber, string password)
        {
            string encryptedPasswordOriginal = Encrypt("HASH", "SHA256", password);
            bool validated = false;

            List<tblUser> userList = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber && a.apply == true).ToList();
            if (userList.Count > 0)
            {
                tblUser user = userList[0];

                if (encryptedPasswordOriginal == user.password)
                {
                    validated = true;
                }
            }

            return validated;
        }

        public string GetTempPassword(int digit)
        {
            string tempPassword = "";
            string[] ruleStrings = "a,b,c,d,e,f,g,h,i,j,k,l,n,m,o,p,q,r,s,t,u,v,w,x,y,z,0,1,2,3,4,5,6,7,8,9".Split(',');
            Random rnd = new Random();

            for (int i = 0; i < digit; i++)
            {
                int applyIndex = rnd.Next(0, ruleStrings.Length - 1);
                tempPassword += ruleStrings[applyIndex];
            }
            return tempPassword;
        }

        /// <summary>
        /// 시/도
        /// </summary>
        public List<string> GetSidoList()
        {
            List<string> retval = (from tbl in db89_wowbill.tblPost orderby tbl.sido select tbl.sido).Distinct().ToList();
            return retval;
        }

        /// <summary>
        /// 구/군
        /// </summary>
        /// <param name="sido"></param>
        /// <returns></returns>
        public List<string> GetGugunList(string sido)
        {
            List<string> retval = (from tbl in db89_wowbill.tblPost where tbl.sido == sido orderby tbl.gugun select tbl.gugun).Distinct().ToList();
            return retval;
        }

        /// <summary>
        /// 연간수입
        /// </summary>
        /// <returns></returns>
        public List<MemberInfoCode> GetSalaryList()
        {
            List<MemberInfoCode> retval = (from tblA in db89_wowbill.tblCodeSalary
                                           join tblB in db89_wowbill.tblCodeSalaryDetail on tblA.salaryId equals tblB.salaryId into _c
                                           from c in _c.DefaultIfEmpty()
                                           where tblA.apply == true
                                           orderby (c != null ? c.sort : 0) ascending, tblA.salaryId ascending
                                           select new MemberInfoCode()
                                           {
                                               Id = tblA.salaryId,
                                               Descript = tblA.descript
                                           }).ToList();
            return retval;
        }

        /// <summary>
        /// 투자선호대상
        /// </summary>
        /// <returns></returns>
        public List<MemberInfoCode> GetInvestmentPreferenceObjectList()
        {
            List<MemberInfoCode> retval = (from tblA in db89_wowbill.tblCodeInvestmentPreferenceObject
                                           join tblB in db89_wowbill.tblCodeInvestmentPreferenceObjectDetail
                                           on tblA.investmentPreferenceObjectId equals tblB.investmentPreferenceObjectId into _c
                                           from c in _c.DefaultIfEmpty()
                                           where tblA.apply == true
                                           orderby (c != null ? c.sort : 0) ascending, tblA.investmentPreferenceObjectId ascending
                                           select new MemberInfoCode()
                                           {
                                               Id = tblA.investmentPreferenceObjectId,
                                               Descript = tblA.descript
                                           }).ToList();
            return retval;
        }

        /// <summary>
        /// 기존정보 습득처
        /// </summary>
        /// <returns></returns>
        public List<MemberInfoCode> GetInfoAcquirementList()
        {
            List<MemberInfoCode> retval = (from tblA in db89_wowbill.tblCodeInfoAcquirement
                                           join tblB in db89_wowbill.tblCodeInfoAcquirementDetail
                                           on tblA.infoAcquirementId equals tblB.infoAcquirementId into _c
                                           from c in _c.DefaultIfEmpty()
                                           where tblA.apply == true
                                           orderby (c != null ? c.sort : 0) ascending, tblA.infoAcquirementId ascending
                                           select new MemberInfoCode()
                                           {
                                               Id = tblA.infoAcquirementId,
                                               Descript = tblA.descript
                                           }).ToList();
            return retval;
        }

        /// <summary>
        /// 투자기간
        /// </summary>
        /// <returns></returns>
        public List<MemberInfoCode> GetInvestmentPeriodList()
        {
            List<MemberInfoCode> retval = (from tblA in db89_wowbill.tblCodeInvestmentPeriod
                                           join tblB in db89_wowbill.tblCodeInvestmentPeriodDetail
                                           on tblA.investmentPeriodId equals tblB.investmentPeriodId into _c
                                           from c in _c.DefaultIfEmpty()
                                           where tblA.apply == true
                                           orderby (c != null ? c.sort : 0) ascending, tblA.investmentPeriodId ascending
                                           select new MemberInfoCode()
                                           {
                                               Id = tblA.investmentPeriodId,
                                               Descript = tblA.descript
                                           }).ToList();
            return retval;
        }

        /// <summary>
        /// 투자성향
        /// </summary>
        /// <returns></returns>
        public List<MemberInfoCode> GetInvestmentPropensityList()
        {
            List<MemberInfoCode> retval = (from tblA in db89_wowbill.tblCodeInvestmentPropensity
                                           join tblB in db89_wowbill.tblCodeInvestmentPropensityDetail
                                           on tblA.investmentPropensityId equals tblB.investmentPropensityId into _c
                                           from c in _c.DefaultIfEmpty()
                                           where tblA.apply == true
                                           orderby (c != null ? c.sort : 0) ascending, tblA.investmentPropensityId ascending
                                           select new MemberInfoCode()
                                           {
                                               Id = tblA.investmentPropensityId,
                                               Descript = tblA.descript
                                           }).ToList();
            return retval;
        }

        /// <summary>
        /// 직업
        /// </summary>
        /// <returns></returns>
        public List<MemberInfoCode> GetJobList()
        {
            List<MemberInfoCode> retval = (from tbl in db89_wowbill.tblCodeJob
                                           where tbl.apply == true
                                           orderby tbl.jobId ascending
                                           select new MemberInfoCode()
                                           {
                                               Id = tbl.jobId,
                                               Descript = tbl.descript
                                           }).ToList();
            return retval;
        }

        /// <summary>
        /// 주요증권거래처
        /// </summary>
        /// <returns></returns>
        public List<MemberInfoCode> GetStockCompanyList()
        {
            List<MemberInfoCode> retval = (from tblA in db89_wowbill.tblCodeStockCompany
                                           join tblB in db89_wowbill.tblCodeStockCompanyDetail
                                           on tblA.stockCompanyId equals tblB.stockCompanyId into _c
                                           from c in _c.DefaultIfEmpty()
                                           where tblA.apply == true
                                           orderby (c != null ? c.sort : 0) ascending, tblA.stockCompanyId ascending
                                           select new MemberInfoCode()
                                           {
                                               Id = tblA.stockCompanyId,
                                               Descript = tblA.descript
                                           }).ToList();
            return retval;
        }

        /// <summary>
        /// 관심분야
        /// </summary>
        /// <returns></returns>
        public List<MemberInfoCode> GetInterestList()
        {
            List<MemberInfoCode> retval = (from tblA in db89_wowbill.tblCodeInterest
                                           join tblB in db89_wowbill.tblCodeInterestDetail
                                           on tblA.interestId equals tblB.interestId into _c
                                           from c in _c.DefaultIfEmpty()
                                           where tblA.apply == true
                                           orderby (c != null ? c.sort : 0) ascending, tblA.interestId ascending
                                           select new MemberInfoCode()
                                           {
                                               Id = tblA.interestId,
                                               Descript = tblA.descript
                                           }).ToList();
            return retval;

        }

        /// <summary>
        /// 투자규모
        /// </summary>
        /// <returns></returns>
        public List<MemberInfoCode> GetInvestmentScaleList()
        {
            List<MemberInfoCode> retval = (from tblA in db89_wowbill.tblCodeInvestmentScale
                                           join tblB in db89_wowbill.tblCodeInvestmentScaleDetail
                                           on tblA.investmentScaleId equals tblB.investmentScaleId into _c
                                           from c in _c.DefaultIfEmpty()
                                           where tblA.apply == true
                                           orderby (c != null ? c.sort : 0) ascending, tblA.investmentScaleId ascending
                                           select new MemberInfoCode()
                                           {
                                               Id = tblA.investmentScaleId,
                                               Descript = tblA.descript
                                           }).ToList();
            return retval;
        }

        /// <summary>
        /// 가입경로
        /// </summary>
        /// <returns></returns>
        public List<MemberInfoCode> GetRegistRouteList()
        {
            List<MemberInfoCode> retval = (from tbl in db89_wowbill.tblCodeRegistRoute
                                           where tbl.apply == true
                                           orderby tbl.registRouteId ascending
                                           select new MemberInfoCode()
                                           {
                                               Id = tbl.registRouteId,
                                               Descript = tbl.descript
                                           }).ToList();
            return retval;
        }

        /// <summary>
        /// 이메일 인증 메일 완료 처리
        /// </summary>
        /// <param name="userNumber"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public EmailAuthCompleteResult EmailAuthComplete(int userNumber, string email)
        {
            DateTime now = DateTime.Now;
            EmailAuthCompleteResult result = new EmailAuthCompleteResult();
            result.UserMatched = false;
            result.EmailMatched = false;
            result.EmailAuthCompleted = false;

            try
            {
                tblUser userInfo = (from tbl in db89_wowbill.tblUser where tbl.userNumber == userNumber select tbl).SingleOrDefault();
                if (userInfo != null)
                {
                    result.UserMatched = true;
                    result.EmailMatched = userInfo.email == email;

                    tblMemberEmailAuth memberEmailAuth = (from tbl in db89_wowbill.tblMemberEmailAuth where tbl.userNumber == userNumber && tbl.expireDt >= now select tbl).SingleOrDefault();
                    if (memberEmailAuth != null)
                    {
                        db89_wowbill.Database.ExecuteSqlCommand("exec usp_MemberEmailAuth " + userNumber + ", 'REG'");
                        result.EmailAuthCompleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ExceptionMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// DI 값으로 UserNumber 받아오기 (미승인, 탈퇴회원 제외)
        /// </summary>
        /// <param name="dupInfo"></param>
        /// <returns></returns>
        public int? GetDupInfoUserNumber(string dupInfo)
        {
            List<tblUser_IPIN> ipinUserList = (from ipin in db89_wowbill.tblUser_IPIN.AsNoTracking()
                                               join user in db89_wowbill.tblUser.AsNoTracking() on ipin.usernumber equals user.userNumber
                                               where ipin.DupInfo.Equals(dupInfo) && user.apply.Equals(true)
                                               select ipin).ToList();
            if (ipinUserList.Count > 0)
            {
                return ipinUserList[0].usernumber;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// DI 값으로 UserNumber 받아오기 (미승인, 탈퇴회원 제외)
        /// </summary>
        /// <param name="dupInfo"></param>
        /// <returns></returns>
        public int? GetDupInfoUserNumber(string dupInfo, string userId)
        {
            List<tblUser_IPIN> ipinUserList = (from ipin in db89_wowbill.tblUser_IPIN.AsNoTracking()
                                               join user in db89_wowbill.tblUser.AsNoTracking() on ipin.usernumber equals user.userNumber
                                               where ipin.DupInfo.Equals(dupInfo) && user.userId.Equals(userId) && user.apply.Equals(true)
                                               select ipin).ToList();
            if (ipinUserList.Count > 0)
            {
                return ipinUserList[0].usernumber;
            }
            else
            {
                return null;
            }
        }

        public bool CheckUserId(string userId)
        {
            bool withSpecialCharacter = false;
            char[] characterArray = userId.ToCharArray();
            foreach (char character in characterArray)
            {
                Regex englishRegex = new Regex(@"[a-zA-Z]");
                bool isEnglish = englishRegex.IsMatch(character.ToString());

                Regex numberRegex = new Regex(@"[0-9]");
                bool isNumber = numberRegex.IsMatch(character.ToString());

                if (isEnglish == false && isNumber == false)
                {
                    withSpecialCharacter = true;
                    break;
                }
            }
            return withSpecialCharacter;
        }

        public bool CheckNickName(string nickName)
        {
            bool withSpecialCharacter = false;
            char[] characterArray = nickName.ToCharArray();
            foreach (char character in characterArray)
            {
                bool isUnicodeLanguage = char.GetUnicodeCategory(character) == System.Globalization.UnicodeCategory.OtherLetter;

                Regex englishRegex = new Regex(@"[a-zA-Z]");
                bool isEnglish = englishRegex.IsMatch(character.ToString());

                Regex numberRegex = new Regex(@"[0-9]");
                bool isNumber = numberRegex.IsMatch(character.ToString());

                if (isUnicodeLanguage == false && isEnglish == false && isNumber == false)
                {
                    withSpecialCharacter = true;
                    break;
                }
            }
            return withSpecialCharacter;
        }

        public string ToStringNullToEmpty(string str)
        {
            return string.IsNullOrEmpty(str) == false ? str.Trim() : "";
        }
    }

    public class SetPasswordResult
    {
        public bool Success { get; set; }
        public string ReturnMessage { get; set; }
        public string TempPassword { get; set; }
        public string MobileNo { get; set; }
    }
}
