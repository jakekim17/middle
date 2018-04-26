using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Fx;
using Wow.Tv.Middle.Biz.Member;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Model.Db89.Sleep_wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;
using Wow.Tv.Middle.Model.Db89.wowbill.MemberAdminManage;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.Member;

namespace Wow.Tv.Middle.Biz.MemberManage
{
    public class MemberManageBiz : BaseBiz
    {
        private MemberCommonBiz commonBiz = null;
        public MemberManageBiz()
        {
            commonBiz = new MemberCommonBiz();
        }

        public ListModel<MemberManageListResult> MemberSearchList(MemberManageCondition condition)
        {
            ObjectResult<NUP_ADMIN_MEMBER_LIST_SELECT_Result> list = db89_wowbill.NUP_ADMIN_MEMBER_LIST_SELECT(
                condition.RegistDateFrom, condition.RegistDateTo, condition.LatestConnectDateFrom, condition.LatestConnectDateTo, 
                condition.SearchType, condition.SearchText, condition.CurrentIndex, condition.PageSize);
            List<NUP_ADMIN_MEMBER_LIST_SELECT_Result> userList = list.ToList();


            string userNumberaList = "";
            foreach (NUP_ADMIN_MEMBER_LIST_SELECT_Result item in userList)
            {
                userNumberaList += "," + item.USERNUMBER;
            }

            ObjectResult<NUP_ADMIN_MEMBER_SUB_SELECT_Result> subList = db89_WOWTV_BILL_DB.NUP_ADMIN_MEMBER_SUB_SELECT(userNumberaList);

            List<MemberManageListResult> joinList = (from user in userList
                                                   join sub in subList on user.USERNUMBER equals sub.USERNUMBER into _c
                                                   from c in _c.DefaultIfEmpty()
                                                   orderby user.ROW_NUM ascending
                                                   select new MemberManageListResult()
                                                   {
                                                       ROW_NUM = user.ROW_NUM,
                                                       USERID = user.USERID,
                                                       USERNAME = user.USERNAME,
                                                       NICKNAME = user.NICKNAME,
                                                       MOBILE_NO = user.MOBILE_NO,
                                                       TEL_NO = user.TEL_NO,
                                                       EMAIL = user.EMAIL,
                                                       WOW_CASH = c.WOW_CASH,
                                                       USER_SECTION = user.USER_SECTION,
                                                       COMPANY_NAME = user.COMPANY_NAME,
                                                       USER_CLASS = c.USER_CLASS,
                                                       USER_TYPE = user.USER_TYPE,
                                                       ALIVE_TYPE = user.ALIVE_TYPE,
                                                       USERNUMBER = user.USERNUMBER,
                                                       JOIN_TODAY_COUNT = user.JOIN_TODAY_COUNT,
                                                       SEARCHED_TOTAL_COUNT = user.SEARCHED_TOTAL_COUNT
                                                   }).ToList();

            ListModel<MemberManageListResult> retval = new ListModel<MemberManageListResult>();
            retval.ListData = joinList;

            return retval;
        }

        public MemberManageInfoResult MemberInfo(int userNumber)
        {
            MemberManageInfoResult retval = new MemberManageInfoResult();

            ObjectResult<NUP_ADMIN_MEMBER_INFO_SELECT_Result> memberInfo = db89_wowbill.NUP_ADMIN_MEMBER_INFO_SELECT(userNumber);
            retval.MemberInfo = memberInfo.FirstOrDefault();
            retval.LatestApprovalLog = db89_wowbill.tblUser_approvalLog.Where(a => a.usernumber == userNumber && a.approved == false).OrderByDescending(a => a.seq).Select(a => a.reason).FirstOrDefault();

            retval.MemberInterestList = retval.MemberInfo.INTEREST_AREA.Split(',').ToList();
            for (int i = retval.MemberInterestList.Count - 1; i >=0; i--)
            {
                if (retval.MemberInterestList[i].Trim() == "")
                {
                    retval.MemberInterestList.RemoveAt(i);
                }
            }

            ObjectResult<NUP_ADMIN_MEMBER_SUB_SELECT_Result> subList = db89_WOWTV_BILL_DB.NUP_ADMIN_MEMBER_SUB_SELECT(userNumber.ToString());
            NUP_ADMIN_MEMBER_SUB_SELECT_Result subInfo = subList.SingleOrDefault();
            retval.WOW_CASH = subInfo.WOW_CASH;
            retval.COUPON_COUNT = subInfo.COUPON_COUNT;
            retval.USER_CLASS = subInfo.USER_CLASS;

            ObjectResult<NUP_ADMIN_MEMBER_INFO_HISTORY_SELECT_Result> history = db89_wowbill.NUP_ADMIN_MEMBER_INFO_HISTORY_SELECT(userNumber);
            retval.MemberHistory = new ListModel<NUP_ADMIN_MEMBER_INFO_HISTORY_SELECT_Result>();
            retval.MemberHistory.ListData = history.ToList();
            retval.MemberHistory.TotalDataCount = retval.MemberHistory.ListData.Count;

            if (retval.MemberInfo.USERNAME == "휴면회원")
            {
                bak_tblUser backupUserInfo = db89_Sleep_wowbill.bak_tblUser.AsNoTracking().Where(a => a.userNumber == userNumber).SingleOrDefault();
                if (backupUserInfo != null)
                {
                    retval.DormancyMobileNo = backupUserInfo.Mobile;
                }
            }

            //string executeSql = "EXEC NUP_ADMIN_MEMBER_INFO_HISTORY_SELECT @USER_NUMBER = {0}";
            //List<NUP_ADMIN_MEMBER_INFO_HISTORY_SELECT_Result> history = db89_wowbill.Database.SqlQuery<NUP_ADMIN_MEMBER_INFO_HISTORY_SELECT_Result>(executeSql, userNumber).ToList();
            //retval.MemberHistory.ListData = history;
            //retval.MemberHistory.TotalDataCount = history.Count;



            // 연간수입
            retval.Salary = (from tblA in db89_wowbill.tblCodeSalary.AsNoTracking()
                             join tblB in db89_wowbill.tblCodeSalaryDetail.AsNoTracking() on tblA.salaryId equals tblB.salaryId into _c
                             from c in _c.DefaultIfEmpty()
                             where tblA.apply == true
                             orderby (c != null ? c.sort : 0) ascending, tblA.salaryId ascending
                             select new MemberManageCode()
                             {
                                 Id = tblA.salaryId,
                                 Descript = tblA.descript
                             }).ToList();

            // 투자선호대상
            retval.InvestmentPreferenceObject = (from tblA in db89_wowbill.tblCodeInvestmentPreferenceObject.AsNoTracking()
                                                 join tblB in db89_wowbill.tblCodeInvestmentPreferenceObjectDetail.AsNoTracking()
                                                 on tblA.investmentPreferenceObjectId equals tblB.investmentPreferenceObjectId into _c
                                                 from c in _c.DefaultIfEmpty()
                                                 where tblA.apply == true
                                                 orderby (c != null ? c.sort : 0) ascending, tblA.investmentPreferenceObjectId ascending
                                                 select new MemberManageCode()
                                                 {
                                                     Id = tblA.investmentPreferenceObjectId,
                                                     Descript = tblA.descript
                                                 }).ToList();

            // 기존정보 습득처
            retval.InfoAcquirement = (from tblA in db89_wowbill.tblCodeInfoAcquirement.AsNoTracking()
                                      join tblB in db89_wowbill.tblCodeInfoAcquirementDetail.AsNoTracking()
                                      on tblA.infoAcquirementId equals tblB.infoAcquirementId into _c
                                      from c in _c.DefaultIfEmpty()
                                      where tblA.apply == true
                                      orderby (c != null ? c.sort : 0) ascending, tblA.infoAcquirementId ascending
                                      select new MemberManageCode()
                                      {
                                          Id = tblA.infoAcquirementId,
                                          Descript = tblA.descript
                                      }).ToList();

            // 투자기간
            retval.InvestmentPeriod = (from tblA in db89_wowbill.tblCodeInvestmentPeriod.AsNoTracking()
                                       join tblB in db89_wowbill.tblCodeInvestmentPeriodDetail.AsNoTracking()
                                       on tblA.investmentPeriodId equals tblB.investmentPeriodId into _c
                                       from c in _c.DefaultIfEmpty()
                                       where tblA.apply == true
                                       orderby (c != null ? c.sort : 0) ascending, tblA.investmentPeriodId ascending
                                       select new MemberManageCode()
                                       {
                                           Id = tblA.investmentPeriodId,
                                           Descript = tblA.descript
                                       }).ToList();

            // 투자성향
            retval.InvestmentPropensity = (from tblA in db89_wowbill.tblCodeInvestmentPropensity.AsNoTracking()
                                           join tblB in db89_wowbill.tblCodeInvestmentPropensityDetail.AsNoTracking()
                                           on tblA.investmentPropensityId equals tblB.investmentPropensityId into _c
                                           from c in _c.DefaultIfEmpty()
                                           where tblA.apply == true
                                           orderby (c != null ? c.sort : 0) ascending, tblA.investmentPropensityId ascending
                                           select new MemberManageCode()
                                           {
                                               Id = tblA.investmentPropensityId,
                                               Descript = tblA.descript
                                           }).ToList();

            // 주요증권거래처
            retval.StockCompany = (from tblA in db89_wowbill.tblCodeStockCompany.AsNoTracking()
                                   join tblB in db89_wowbill.tblCodeStockCompanyDetail.AsNoTracking()
                                   on tblA.stockCompanyId equals tblB.stockCompanyId into _c
                                   from c in _c.DefaultIfEmpty()
                                   where tblA.apply == true
                                   orderby (c != null ? c.sort : 0) ascending, tblA.stockCompanyId ascending
                                   select new MemberManageCode()
                                   {
                                       Id = tblA.stockCompanyId,
                                       Descript = tblA.descript
                                   }).ToList();

            // 투자규모
            retval.InvestmentScale = (from tblA in db89_wowbill.tblCodeInvestmentScale.AsNoTracking()
                                      join tblB in db89_wowbill.tblCodeInvestmentScaleDetail.AsNoTracking()
                                      on tblA.investmentScaleId equals tblB.investmentScaleId into _c
                                      from c in _c.DefaultIfEmpty()
                                      where tblA.apply == true
                                      orderby (c != null ? c.sort : 0) ascending, tblA.investmentScaleId ascending
                                      select new MemberManageCode()
                                      {
                                          Id = tblA.investmentScaleId,
                                          Descript = tblA.descript
                                      }).ToList();

            // 관심분야
            retval.Interest = (from tblA in db89_wowbill.tblCodeInterest.AsNoTracking()
                               join tblB in db89_wowbill.tblCodeInterestDetail.AsNoTracking()
                               on tblA.interestId equals tblB.interestId into _c
                               from c in _c.DefaultIfEmpty()
                               where tblA.apply == true
                               orderby (c != null ? c.sort : 0) ascending, tblA.interestId ascending
                               select new MemberManageCode()
                               {
                                   Id = tblA.interestId,
                                   Descript = tblA.descript
                               }).ToList();

            //if (string.IsNullOrEmpty(retval.MemberInfo.INTEREST_AREA.Trim()) == false)
            //{
            //    List<string> interestList = retval.MemberInfo.INTEREST_AREA.Split(',').ToList();

            //    int newId = 0;
            //    int insertIndex = 0;
            //    for (int i = 0; i < interestList.Count; i++)
            //    {
            //        string item = interestList[i].Trim();

            //        if (string.IsNullOrEmpty(item) == true)
            //        {
            //            continue;
            //        }

            //        if (retval.Interest.Where(a => a.Descript == item).Count() == 0)
            //        {
            //            newId--;
            //            MemberManageCode addItem = new MemberManageCode();
            //            addItem.Id = newId;
            //            addItem.Descript = item;

            //            retval.Interest.Insert(insertIndex, addItem);
            //            insertIndex++;
            //        }
            //    }
            //}


            //retval.Interest = new List<MemberManageCode>();
            //retval.Interest.Add(new MemberManageCode() { Id = 1, Descript = "주식" });
            //retval.Interest.Add(new MemberManageCode() { Id = 2, Descript = "부동산" });
            //retval.Interest.Add(new MemberManageCode() { Id = 3, Descript = "창업" });
            //retval.Interest.Add(new MemberManageCode() { Id = 4, Descript = "취업" });
            //retval.Interest.Add(new MemberManageCode() { Id = 5, Descript = "의학" });

            return retval;
        }

        public SetPasswordResult PasswordInitialize(int userNumber, string adminId)
        {
            return commonBiz.PasswordInitialize(userNumber, adminId);
        }

        public UserInfoModifyResult MemberInfoSave(UserInfoSaveRequest request)
        {
            if (request.TelNo1 == null) request.TelNo1 = "";
            if (request.TelNo2 == null) request.TelNo2 = "";
            if (request.TelNo3 == null) request.TelNo3 = "";
            if (request.FaxNo1 == null) request.FaxNo1 = "";
            if (request.FaxNo2 == null) request.FaxNo2 = "";
            if (request.FaxNo3 == null) request.FaxNo3 = "";
            if (request.MobileNo1 == null) request.MobileNo1 = "";
            if (request.MobileNo2 == null) request.MobileNo2 = "";
            if (request.MobileNo3 == null) request.MobileNo3 = "";
            if (request.BusinessItem == null) request.BusinessItem = "";
            if (request.BusinessNumber == null) request.BusinessNumber = "";

            bool isSendEmail = request.IsSendEmail == "Y" ? true : false;
            bool isSendEmailAd = request.IsSendEmailAd == "Y" ? true : false;
            bool isSendSms = request.IsSendSms == "Y" ? true : false;
            bool isSendSmsAd = request.IsSendSmsAd == "Y" ? true : false;
            string telNo = request.TelNo1 + "-" + request.TelNo2 + "-" + request.TelNo3;
            string faxNo = request.FaxNo1 + "-" + request.FaxNo2 + "-" + request.FaxNo3;
            string mobileNo = request.MobileNo1 + "-" + request.MobileNo2 + "-" + request.MobileNo3;

            string executeSql =
                "DECLARE @RETURN_NUMBER INT\r\n" +
                "DECLARE @RETURN_MESSAGE VARCHAR(4000)\r\n" +
                "EXEC dbo.NUP_ADMIN_MEMBER_INFO_UPDATE" +
                "  @USER_NUMBER = {0}, @IS_SEND_EMAIL = {1}, @IS_SEND_EMAIL_AD = {2}, @IS_SEND_SMS = {3}, @IS_SEND_SMS_AD = {4}" +
                ", @TEL_NO = {5}, @FAX_NO = {6}, @MOBILE_NO = {7}" +
                ", @ZIPCODE = {8}, @ADDRESS = {9}, @SALARY_ID = {10}, @INVESTMENT_PREFERENCE_OBJECT_ID = {11}, @INFO_ACQUIREMENT_ID = {12}" +
                ", @INVESTMENT_PERIOD_ID = {13}, @INVESTMENT_PROPENSITY_ID = {14}, @STOCK_COMPANY_ID = {15}, @INVESTMENT_SCALE_ID = {16}" +
                ", @INTEREST_AREA = {17}, @COMPANY_NAME = {18}, @BUSINESS_CONDITION = {19}, @BUSINESS_ITEM = {20}, @BUSINESS_NUMBER = {21}" +
                ", @ADMIN_ID = {22}" +
                ", @RETURN_NUMBER = @RETURN_NUMBER OUTPUT" +
                ", @RETURN_MESSAGE = @RETURN_MESSAGE OUTPUT\r\n" +
                "SELECT @RETURN_NUMBER AS ReturnNumber, @RETURN_MESSAGE AS ReturnMessage";

            UserInfoModifyResult retval = db89_wowbill.Database.SqlQuery<UserInfoModifyResult>(executeSql,
                request.UserNumber, isSendEmail, isSendEmailAd, isSendSms, isSendSmsAd, telNo, faxNo, mobileNo, request.ZipCode, request.Address,
                request.Salary, request.InvestmentPreferenceObject, request.InfoAcquirement, request.InvestmentPeriod, request.InvestmentPropensity,
                request.StockCompany, request.InvestmentScale, request.InterestList, request.CompanyName, request.BusinessCondition, request.BusinessItem,
                request.BusinessNumber, request.AdminId).SingleOrDefault();

            retval.IsSuccess = retval.ReturnNumber == 0;

            //ObjectParameter returnNumber = new ObjectParameter("RETURN_NUMBER", typeof(int));
            //ObjectParameter returnMessage = new ObjectParameter("RETURN_MESSAGE", typeof(string));

            //db89_wowbill.NUP_ADMIN_MEMBER_INFO_UPDATE(request.UserNumber, isSendEmail, isSendSms, telNo, faxNo, mobileNo,
            //    request.ZipCode, request.Address, request.Salary, request.InvestmentPreferenceObject, request.InfoAcquirement, request.InvestmentPeriod,
            //    request.InvestmentPropensity, request.StockCompany, request.InvestmentScale, interestList,
            //    request.CompanyName, request.BusinessCondition, request.BusinessItem, request.BusinessNumber, request.AdminId, returnNumber, returnMessage);

            //retval.IsSuccess = returnNumber.Value.ToString() == "0" ? true : false;
            //retval.ReturnMessage = returnMessage.Value.ToString();

            return retval;
        }

        //public UserInfoModifyResult MemberInfoDelete(int userNumber)
        //{
        //    UserInfoModifyResult retval = new UserInfoModifyResult();

        //    ObjectParameter returnNumber = new ObjectParameter("RETURN_NUMBER", typeof(int));
        //    ObjectParameter returnMessage = new ObjectParameter("RETURN_MESSAGE", typeof(string));

        //    db89_wowbill.NUP_ADMIN_MEMBER_INFO_DELETE(userNumber, returnNumber, returnMessage);

        //    retval.IsSuccess = returnNumber.Value.ToString() == "0" ? true : false;
        //    retval.ReturnMessage = returnMessage.Value.ToString();

        //    return retval;
        //}

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
                //db89_wowbill.Database.ExecuteSqlCommand("EXEC sp_InsertUserHistoryByUserNumber {0}, {1}, {2}", userNumber, "삭제", "user");
				db89_wowbill.Database.ExecuteSqlCommand("EXEC sp_InsertUserHistoryByUserNumber {0}, {1}, {2}", userNumber, "삭제", "admin");

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

        public List<tblPost> GetAddress(string searchKeyword)
        {
            List<tblPost> result = (from tbl in db89_wowbill.tblPost.AsNoTracking()
                                    where tbl.sido.Contains(searchKeyword) || tbl.gugun.Contains(searchKeyword) || tbl.dong.Contains(searchKeyword)
                                    orderby tbl.sido ascending, tbl.gugun ascending, tbl.dong ascending
                                    select tbl).ToList();
            return result;
        }

        public MemberInfoStatisticsResult MemberInfoStatistics()
        {
            MemberInfoStatisticsResult retval = new MemberInfoStatisticsResult();
            using (var connection = db89_wowbill.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "EXEC dbo.NUP_ADMIN_MEMBER_STATISTICS_SELECT";

                using (var reader = command.ExecuteReader())
                {
                    retval.Statistics = ((IObjectContextAdapter)db89_wowbill).ObjectContext.Translate<MemberInfoStatistics>(reader).SingleOrDefault();
                    reader.NextResult();
                    retval.AgeList = ((IObjectContextAdapter)db89_wowbill).ObjectContext.Translate<MemberInfoAge>(reader).ToList();
                }
            }
            return retval;
        }

        public void UserApproval(int userNumber, bool approved, string reason, string bOQv5BillHost)
        {
            tblUser userInfo = db89_wowbill.tblUser.Where(a => a.userNumber == userNumber).SingleOrDefault();
            if (userInfo == null)
            {
                return;
            }

            bool sendCoupon = userInfo.apply == false && approved == true;
            userInfo.apply = approved;
            db89_wowbill.SaveChanges();

            tblUser_approvalLog log = new tblUser_approvalLog();
            log.usernumber = userNumber;
            log.approved = approved;
            log.reason = reason;
            log.registDt = DateTime.Now;
            db89_wowbill.tblUser_approvalLog.Add(log);
            db89_wowbill.SaveChanges();

            if (sendCoupon == true)
            {
                try
                {
                    BOQv7BillLib.BillClass couponBill2 = new BOQv7BillLib.BillClass();
                    couponBill2.TxCmd = "issueonecoupon";
                    couponBill2.HOST = bOQv5BillHost;
                    couponBill2.CodePage = 0;
                    couponBill2.SetField("couponid", "917"); // 김종철 패키지
                    couponBill2.SetField("durationtype", "4"); // 유효기간 유형 (4:일, 5:월)	// K100204
                    couponBill2.SetField("duration", "31"); // 유효기간 값(일수 or 월수)	// K100204
                    couponBill2.SetField("reason", "신규회원 쿠폰");
                    couponBill2.SetField("adminid", "system"); // 지급관리자ID
                    couponBill2.SetField("userid", userInfo.userId); // 고객아이디
                    int couponBill2Result = couponBill2.StartAction();
                    if (couponBill2Result != 0)
                    {
                        WowLog.Write("법인회원승인 > 김종철 패키지 쿠폰 발행 실패(UserNumber: " + userInfo.userNumber.ToString() + ")\r\nCode: " + couponBill2Result + ", Message: " + couponBill2.ErrMsg);
                    }
                }
                catch (Exception ex)
                {
                    WowLog.Write("법인회원승인 > 김종철 패키지 쿠폰 발행 실패(UserNumber: "+ userInfo.userNumber.ToString() + "): " + ex.Message);
                }

                try
                {
                    BOQv7BillLib.BillClass couponBill3 = new BOQv7BillLib.BillClass();
                    couponBill3.TxCmd = "issueonecoupon";
                    couponBill3.HOST = bOQv5BillHost;
                    couponBill3.CodePage = 0;
                    couponBill3.SetField("couponid", "924"); // 매직양봉팀 스마트밴드 3일체험 쿠폰
                    couponBill3.SetField("durationtype", "4"); // 유효기간 유형 (4:일, 5:월)	// K100204
                    couponBill3.SetField("duration", "31"); // 유효기간 값(일수 or 월수)	// K100204
                    couponBill3.SetField("reason", "신규회원 쿠폰");
                    couponBill3.SetField("adminid", "system"); // 지급관리자ID
                    couponBill3.SetField("userid", userInfo.userId); // 고객아이디
                    int couponBill3Result = couponBill3.StartAction();
                    if (couponBill3Result != 0)
                    {
                        WowLog.Write("법인회원승인 > 김종철 패키지 쿠폰 발행 실패(UserNumber: " + userInfo.userNumber.ToString() + ")\r\nCode: " + couponBill3Result + ", Message: " + couponBill3.ErrMsg);
                    }
                }
                catch (Exception ex)
                {
                    WowLog.Write("법인회원승인 > 매직양봉팀 스마트밴드 3일체험 쿠폰 발행 실패(UserNumber: " + userInfo.userNumber.ToString() + "): " + ex.Message);
                }

                try
                {
                    // 와우캐시 10만원 무료지급
                    ObjectParameter cashReal = new ObjectParameter("po_intCashReal", typeof(int));
                    ObjectParameter cashBonus = new ObjectParameter("po_intCashBonus", typeof(int));
                    ObjectParameter cashNo = new ObjectParameter("po_intCashNo", typeof(long));
                    ObjectParameter cashErrorMessage = new ObjectParameter("po_strErrMsg", typeof(string));
                    ObjectParameter cashReturnCode = new ObjectParameter("po_intRetVal", typeof(int));

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

                    RegistUserReturn retval = db89_WOWTV_BILL_DB.Database.SqlQuery<RegistUserReturn>(executeSql, userInfo.userNumber, userInfo.userId, DBNull.Value, DBNull.Value, "event(m)-all", wowCash,
                        "회원가입 감사", "system(web)", DBNull.Value, DateTime.Now.AddDays(30).ToString("yyyyMMdd")).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    WowLog.Write("법인회원승인 > 와우캐시 지급 실패(UserNumber: " + userInfo.userNumber.ToString() + "): " + ex.Message);
                }
            }
        }

        public List<MemberInfoSimple> SimpleMemberList(List<int> userNumberList)
        {
            List<MemberInfoSimple> listData = db89_wowbill.tblUser.AsNoTracking().Where(a => userNumberList.Contains(a.userNumber))
                .Select(a => new MemberInfoSimple() { UserNumber = a.userNumber, UserId = a.userId, UserName = a.name }).ToList();
            //List<MemberInfoSimple> listData = (from a in userNumberList
            //                                   join b in db89_wowbill.tblUser.AsNoTracking() on
            //                                   a.UserNumber equals b.userNumber
            //                                   select new MemberInfoSimple()
            //                                    {
            //                                        UserNumber = a.UserNumber,
            //                                        UserId = b.userId,
            //                                        UserName = b.name
            //                                    }).ToList();
            return listData;
        }

        /// <summary>
        /// 휴면회원 해지
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public string RestoreDormancy(int userNumber)
        {
            string retval = "";
            string executeSql = "EXEC dbo.usp_MemberRestoreDormancy {0}";

            try
            {
                db89_wowbill.Database.ExecuteSqlCommand(executeSql, userNumber);
            }
            catch (Exception ex)
            {
                retval = ex.Message;
                WowLog.Write("[RestoreDormancy 오류] SQL: " + executeSql, ex);
            }
            return retval;
        }


		//DI 재가입불가 리스트
		public ListModel<TblMemberDIDenialList> MemberDIDenialList(IntegratedBoardCondition condition)
		{

			ListModel<TblMemberDIDenialList> resultData = new ListModel<TblMemberDIDenialList>();


			var list = db89_wowbill.TblMemberDIDenialList.AsQueryable();

			//list = list.Where(a => a.BCODE.Equals("T07020000"));
			list = list.OrderByDescending(a => a.RequestMailDate).ThenByDescending(a => a.RegDt);
			
			//전체 갯수 가져오기.
			resultData.TotalDataCount = list.Count();

			/*---- 페이징 ----*/
			if (condition.PageSize > -1)
			{
				if (condition.PageSize == 0)
				{
					condition.PageSize = 10;
				}

				list = list.OrderByDescending(a => a.seq).Skip(condition.CurrentIndex).Take(condition.PageSize);
			}
			/*---- 페이징 ----*/


			resultData.ListData = list.ToList();

			return resultData;
		}


		//DI 재가입불가 신규등록/수정
		public void MemberDIDenialWriteProc(TblMemberDIDenialList model)
		{

			TblMemberDIDenialList Single = db89_wowbill.TblMemberDIDenialList.SingleOrDefault(a => a.seq == model.seq);

			if (Single == null) //insert
			{
				
				model.DupInfo = model.DupInfo;
				model.AlertText = model.AlertText;
				model.RequestMailMember = model.RequestMailMember;
				model.RequestMailDate = model.RequestMailDate;
				model.RegDt = DateTime.Now;
				db89_wowbill.TblMemberDIDenialList.Add(model);
			}
			else //update
			{
				Single.AlertText = model.AlertText;
				Single.RequestMailMember = model.RequestMailMember;
				Single.RequestMailDate = model.RequestMailDate;
			}
			//db89_wowbill.Database.Log = s => Debug.WriteLine(s);

			db89_wowbill.SaveChanges();

		}

		//DI 재가입불가 뷰 
		public TblMemberDIDenialList MemberDIDenialView(int seq)
		{

			var resultData = new TblMemberDIDenialList();

			var dataSeq = db89_wowbill.TblMemberDIDenialList.AsNoTracking().Where(a => a.seq == seq).SingleOrDefault();
			if (dataSeq != null)
			{

				resultData = db89_wowbill.TblMemberDIDenialList.SingleOrDefault(a => a.seq == seq);

			}
			return resultData;
		}

		//삭제
		public void MemberDIDenialDelete(int seq)
		{
			var dataSeq = db89_wowbill.TblMemberDIDenialList.Where(a => a.seq == seq).SingleOrDefault();
			if (dataSeq != null) //seq가 null이 아니면 삭제함.
			{
				db89_wowbill.TblMemberDIDenialList.Remove(dataSeq);
				db89_wowbill.SaveChanges();
			}
		}

	}
}
