using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Admin;

using ClosedXML.Excel;
using Wow.Tv.Middle.Biz.Component;
using Wow.Fx;
using System.Data.Entity.Core.Objects;
using Wow.Tv.Middle.Biz.Member;

namespace Wow.Tv.Middle.Biz.Admin
{
    public class AdminBiz : BaseBiz
    {
        #region 로그인 관련

        public TAB_CMS_ADMIN GetAt(int seq)
        {
            TAB_CMS_ADMIN result = db49_wowtv.TAB_CMS_ADMIN.SingleOrDefault(a => a.SEQ == seq);
            if (result != null)
            {
                var partCode = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(a => a.UP_COMMON_CODE == CommonCodeStatic.PART_CODE && a.CODE_VALUE1 == result.PART_CODE);
                if (partCode != null)
                {
                    result.PartCodeName = partCode.CODE_NAME;
                }
            }
            return result;
        }

        public TAB_CMS_ADMIN GetAtById(string id)
        {
            TAB_CMS_ADMIN result = db49_wowtv.TAB_CMS_ADMIN.SingleOrDefault(a => a.ADMIN_ID == id);
            if (result != null)
            {
                var partCode = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(a => a.UP_COMMON_CODE == CommonCodeStatic.PART_CODE && a.CODE_VALUE1 == result.PART_CODE);
                if (partCode != null)
                {
                    result.PartCodeName = partCode.CODE_NAME;
                }
            }
            return result;
        }

        public void MobileAuthNumberSet(string adminId, string callBackNumber)
        {
            string msg = "";
            string mobileAuthNumber = "";

            var admin = GetAtById(adminId);
            if (admin != null)
            {
                Random rnd = new Random();
                mobileAuthNumber = rnd.Next(100000, 999999).ToString();
                msg = "[Web발신]\r\n[" + mobileAuthNumber + "][한국경제TV] TV Admin 인증번호를 입력해주세요.";

                string mobileNum = admin.MOBILE1 + admin.MOBILE2 + admin.MOBILE3;
                new SmsBiz().SendSms(mobileNum, callBackNumber, msg, "wowsms-member", "휴대폰인증 TV Admin");

                admin.LAST_MOBILE_AUTH_NUM = mobileAuthNumber;
                db49_wowtv.SaveChanges();
            }
        }

        public TAB_CMS_ADMIN LoginCheck(string adminId, string pwd, string mobileAuthNumber, string ip)
        {
            TAB_CMS_ADMIN result = null;
            var admin = GetAtById(adminId);

            if (admin != null)
            {
                //result = admin;
#if DEBUG
                result = admin;
#else
                if (admin.LAST_MOBILE_AUTH_NUM == mobileAuthNumber)
                {
                    //Wow.Fx.WowLog.Write("DB->" + admin.PWD);
                    //Wow.Fx.WowLog.Write("Input->" + pwd);
                    //Wow.Fx.WowLog.Write("Encrypt->" + Wow.Fx.XdbCrypto.Hash(pwd));
                    if (admin.PWD == Wow.Fx.XdbCrypto.Hash(pwd))
                    {
                        result = admin;
                    }
                }
#endif

                // 로긴 실패시 실패횟수 증가
                if (result == null)
                {
                    int loginFailCount = 0;
                    if(admin.LOGIN_FAIL_COUNT != null)
                    {
                        loginFailCount = admin.LOGIN_FAIL_COUNT.Value;
                    }
                    loginFailCount++;
                    admin.LOGIN_FAIL_COUNT = loginFailCount;
                }
                else
                {
                    admin.PREV_LAST_LOGIN_DATE = admin.LAST_LOGIN_DATE;
                    admin.PREV_LAST_LOGIN_IP = admin.LAST_LOGIN_IP;

                    admin.LAST_LOGIN_DATE = DateTime.Now;
                    admin.LOGIN_FAIL_COUNT = 0;
                    admin.LAST_MOBILE_AUTH_NUM = "";
                    admin.LAST_LOGIN_IP = ip;
                }
                db49_wowtv.SaveChanges();
            }

            return result;
        }


        /// <summary>
        /// 해당 관리자의 권한목록 조회
        /// </summary>
        /// <param name="adminSeq">관리자 고유값</param>
        /// <returns></returns>
        public List<NTB_MENU_GROUP> GetAdminGroup(int adminSeq)
        {
            var adminInfo = GetAt(adminSeq);
            List<NTB_MENU_GROUP> list = db49_wowtv.NTB_MENU_GROUP.Where(a => a.GROUP_SEQ == adminInfo.GROUP_SEQ).ToList();

            foreach(var item in list)
            {
                item.Href = db49_wowtv.NTB_MENU.SingleOrDefault(a => a.MENU_SEQ == item.MENU_SEQ).Href;
                item.NTB_MENU = null;
                item.NTB_GROUP = null;
            }

            return list;
        }


        public int Save(TAB_CMS_ADMIN model)
        {
            TAB_CMS_ADMIN data = GetAt(model.SEQ);
            if (data == null)
            {
                TAB_CMS_ADMIN existAdmin = GetAtById(model.ADMIN_ID);
                if (existAdmin == null)
                {
                    model.DEL_YN = "N";
                    // TODO 암호화를 개발자 PC 에서 사용가능하게 될경우 아래 디버그 부분 삭제
#if DEBUG
                    model.PWD = model.PWD;
#else
                model.PWD = Wow.Fx.XdbCrypto.Hash(model.PWD);
#endif
                    model.MLEVEL = "Z";
                    model.LAST_LOGIN_DATE = DateTime.Now; // 원래 안넣어야 하지만 기존 스키마에 널 비허용으로 되어 있어서 넣어줌
                    model.APPROVAL_YN = "N";
                    model.REG_DATE = DateTime.Now;
                    model.MOD_DATE = DateTime.Now;
                    db49_wowtv.TAB_CMS_ADMIN.Add(model);
                }
                else
                {
                    throw new Exception("동일한 아이디를 가진 데이터가 존재합니다.");
                }
            }
            else
            {
                TAB_CMS_ADMIN existAdmin = GetAtById(model.ADMIN_ID);
                if (existAdmin != null)
                {
                    if (existAdmin.SEQ != model.SEQ)
                    {
                        throw new Exception("동일한 아이디를 가진 데이터가 존재합니다.");
                    }
                }

//#if DEBUG
//                data.PWD = model.PWD;
//#else
//                data.PWD = Wow.Fx.XdbCrypto.Hash(model.PWD);
//#endif
                data.ADMIN_ID = model.ADMIN_ID;
                data.NAME = model.NAME;
                data.PHONE1 = model.PHONE1;
                data.PHONE2 = model.PHONE2;
                data.PHONE3 = model.PHONE3;
                data.PARTNAME = model.PARTNAME;
                data.MOBILE1 = model.MOBILE1;
                data.MOBILE2 = model.MOBILE2;
                data.MOBILE3 = model.MOBILE3;
                data.WOWNET = model.WOWNET;
                data.APPROVAL_YN = model.APPROVAL_YN;
                data.GROUP_SEQ = model.GROUP_SEQ;
                data.PART_CODE = model.PART_CODE;
                data.JOB_CODE = model.JOB_CODE;
                data.IP_TYPE = model.IP_TYPE;
                data.IP = model.IP;
                data.IP6 = model.IP6;

                data.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();

            return model.SEQ;
        }


        public void PasswordChange(int adminSeq, string currentPassword, string changePassword)
        {
            var data = GetAt(adminSeq);

#if DEBUG
#else
            currentPassword = Wow.Fx.XdbCrypto.Hash(currentPassword);
            changePassword = Wow.Fx.XdbCrypto.Hash(changePassword);
#endif

            if (data.PWD == currentPassword)
            {
                data.PWD = changePassword;
                db49_wowtv.SaveChanges();

                // 비밀번호 변경 이력 저장
                DateTime now = DateTime.Now;
                TAB_CMS_ADMIN_PwdHistory history = db49_wowtv.TAB_CMS_ADMIN_PwdHistory.Where(a => a.admin_id == data.ADMIN_ID).SingleOrDefault();
                if (history == null)
                {
                    history = new TAB_CMS_ADMIN_PwdHistory();
                    history.admin_id = data.ADMIN_ID;
                    history.regDt = now;
                    history.udtDt = now;
                    history.alarmDt = now.AddMonths(3).ToString("yyyyMMdd");
                    db49_wowtv.TAB_CMS_ADMIN_PwdHistory.Add(history);
                }
                else
                {
                    history.udtDt = now;
                    history.alarmDt = now.AddMonths(3).ToString("yyyyMMdd");
                }
                db49_wowtv.SaveChanges();
            }
            else
            {
                throw new Exception("기존 비밀번호가 일치 하지 않습니다.");
            }
        }


        #endregion


        public ListModel<TAB_CMS_ADMIN> SearchList(AdminCondition condition)
        {
            db49_wowtv.Database.Log = sql => System.Diagnostics.Debug.Write(sql);

            ListModel<TAB_CMS_ADMIN> resultData = new ListModel<TAB_CMS_ADMIN>();

            var list = db49_wowtv.TAB_CMS_ADMIN.Where(a => a.DEL_YN == "N");

            if (condition.GroupSeq > 0)
            {
                list = list.Where(a => a.GROUP_SEQ == condition.GroupSeq);
            }

            if (String.IsNullOrEmpty(condition.PartCode) == false)
            {
                list = list.Where(a => a.PART_CODE == condition.PartCode);
            }


            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                if (condition.SearchType == "All")
                {
                    list = list.Where(a => a.NAME.Contains(condition.SearchText) == true || a.ADMIN_ID.Contains(condition.SearchText) == true);
                }
                else if (condition.SearchType == "Name")
                {
                    list = list.Where(a => a.NAME.Contains(condition.SearchText) == true);
                }
                else if (condition.SearchType == "Id")
                {
                    list = list.Where(a => a.ADMIN_ID.Contains(condition.SearchText) == true);
                }
            }


            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.REG_DATE);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            
            resultData.ListData = list.ToList();

            foreach(var item in resultData.ListData)
            {
                var groupInfo = db49_wowtv.NTB_GROUP.SingleOrDefault(a => a.GROUP_SEQ == item.GROUP_SEQ);
                if (groupInfo != null)
                {
                    item.GroupName = groupInfo.GROUP_NAME;
                }
                item.NTB_GROUP = null;

                var partCode = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(a => a.UP_COMMON_CODE == CommonCodeStatic.PART_CODE && a.CODE_VALUE1 == item.PART_CODE);
                if (partCode != null)
                {
                    item.PartCodeName = partCode.CODE_NAME;
                }

            }


            return resultData;
        }
        

        public void Delete(int seq, LoginUser loginUser)
        {
            var data = GetAt(seq);
            if (data != null)
            {
                //db49_wowtv.NTB_MENU.Remove(data);
                data.DEL_YN = "Y";
                db49_wowtv.SaveChanges();
            }
        }


        public void DeleteList(List<int> seqList, LoginUser loginUser)
        {
            foreach(int item in seqList)
            {
                Delete(item, loginUser);
            }
        }



        public System.IO.MemoryStream ExcelExport(AdminCondition condition)
        {
            var stream = new System.IO.MemoryStream();
            int rowIndex = 2;
            int columnIndex = 1;
            condition.PageSize = -1;
            var list = SearchList(condition);

            XLWorkbook workBook = new XLWorkbook();
            IXLWorksheet sheet = workBook.AddWorksheet("운영자 목록");

            sheet.Cell(1, 1).Value = "운영자 목록";
            sheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.Cell(1, 1).Style.Font.Bold = true;
            sheet.Cell(1, 1).Style.Font.FontSize = 15;


            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "번호";
            sheet.Column(columnIndex).Width = 15;
            sheet.Cell(rowIndex, columnIndex++).Value = "이름";
            sheet.Column(columnIndex).Width = 15;
            sheet.Cell(rowIndex, columnIndex++).Value = "아이디";
            sheet.Column(columnIndex).Width = 15;
            sheet.Cell(rowIndex, columnIndex++).Value = "그룹";
            sheet.Column(columnIndex).Width = 15;
            sheet.Cell(rowIndex, columnIndex++).Value = "부서";
            sheet.Column(columnIndex).Width = 15;
            sheet.Cell(rowIndex, columnIndex++).Value = "최종접속시간";
            sheet.Column(columnIndex).Width = 15;
            sheet.Cell(rowIndex, columnIndex++).Value = "접속IP";
            sheet.Column(columnIndex).Width = 10;
            sheet.Cell(rowIndex, columnIndex++).Value = "승인";
            sheet.Column(columnIndex).Width = 20;
            sheet.Cell(rowIndex, columnIndex++).Value = "작성일";

            columnIndex = 1;
            foreach (var item in list.ListData)
            {
                columnIndex = 1;
                rowIndex++;

                sheet.Cell(rowIndex, columnIndex++).SetValue<int>(item.SEQ);
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.NAME) == true ? "" : item.NAME));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.ADMIN_ID) == true ? "" : item.ADMIN_ID));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.GroupName) == true ? "" : item.GroupName));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.PartCodeName) == true ? "" : item.PartCodeName));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>(item.LAST_LOGIN_DATE.ToDate());
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.CheckIp) == true ? "" : item.CheckIp));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>((String.IsNullOrEmpty(item.APPROVAL_YN) == true ? "" : item.APPROVAL_YN));
                sheet.Cell(rowIndex, columnIndex++).SetValue<string>(item.REG_DATE.ToDateTime() + "\r\n" + item.MOD_DATE.ToDateTime());
            }

            if (columnIndex > 1)
            {
                columnIndex = columnIndex - 1;
            }

            sheet.Range(1, 1, 1, columnIndex).Merge();
            sheet.Range(2, 1, 2, columnIndex).Style.Fill.BackgroundColor = XLColor.FromHtml("#D8D8D8");
            sheet.Range(2, 1, 2, columnIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            sheet.Range(2, 1, rowIndex, columnIndex).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            sheet.Range(2, 1, rowIndex, columnIndex).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            workBook.SaveAs(stream);

            return stream;
        }


		//어드민 ID 비밀번호 초기화 : 고정값(리턴값 없음)
		public void usp_AdminCmsPwdInitialize(string adminId)
		{
			var adminIdAccount = db49_wowtv.TAB_CMS_ADMIN.AsNoTracking().Where(a => a.ADMIN_ID == adminId  ).SingleOrDefault();
			if (adminIdAccount != null)
			{
				string executeSql = "EXEC usp_AdminCmsPwdInitialize @ADMIN_ID={0} ";
				db49_wowtv.Database.ExecuteSqlCommand(executeSql, adminId);				
			}
		}


		//암복호화
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

		//난수발생
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

		//어드민 ID 비밀번호 초기화 : 난수값(리턴값이 있음)
		public SetPasswordResult AdminCmsPwdInitializeRanNum(string adminId)
		{
			SetPasswordResult retval = new SetPasswordResult();
			retval.Success = false;
			retval.ReturnMessage = "";
			
            string tempPassword = GetTempPassword(6);

            string encPassword = Encrypt("HASH", "SHA256", tempPassword);
			
			var adminIdAccount = db49_wowtv.TAB_CMS_ADMIN.Where(a => a.ADMIN_ID == adminId).SingleOrDefault();
			if (adminIdAccount != null)
			{
				if ((string.IsNullOrEmpty(adminIdAccount.MOBILE1) == true) || (string.IsNullOrEmpty(adminIdAccount.MOBILE2) == true) || (string.IsNullOrEmpty(adminIdAccount.MOBILE3) == true))
				{
					retval.ReturnMessage = "올바르지 않은 휴대폰번호 입니다.";
				}
				else
				{
					adminIdAccount.PWD = encPassword;
					db49_wowtv.SaveChanges();

					retval.TempPassword = tempPassword;
					retval.MobileNo = adminIdAccount.MOBILE1+ adminIdAccount.MOBILE2+ adminIdAccount.MOBILE3;
					retval.Success = true;
				}

			}
			else
			{
				retval.ReturnMessage = "사용자 정보가 없습니다.";
			}

			return retval;
		}


	}
}
