using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Biz.Admin;
using Wow.Tv.Middle.Biz.Member;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Admin;

namespace Wow.Tv.Middle.WcfService.Admin
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "AdminService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 AdminService.svc나 AdminService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class AdminService : IAdminService
    {
        public void SmsSend(string adminId)
        {
            string callBackNumber = System.Configuration.ConfigurationManager.AppSettings["SmsCallBackNumber"];

            new AdminBiz().MobileAuthNumberSet(adminId, callBackNumber);
        }



        public TAB_CMS_ADMIN GetAt(int seq)
        {
            return new AdminBiz().GetAt(seq);
        }


        public TAB_CMS_ADMIN GetAtById(string adminId)
        {
            return new AdminBiz().GetAtById(adminId);
        }

        public TAB_CMS_ADMIN LoginCheck(string adminId, string pwd, string mobileAuthNumber, string ip)
        {
            return new AdminBiz().LoginCheck(adminId, pwd, mobileAuthNumber, ip);
        }



        public List<NTB_MENU_GROUP> GetAdminGroup(int adminSeq)
        {
            return new AdminBiz().GetAdminGroup(adminSeq);
        }

        public int Save(TAB_CMS_ADMIN model)
        {
            return new AdminBiz().Save(model);
        }

        public void PasswordChange(int adminSeq, string currentPassword, string changePassword)
        {
            new AdminBiz().PasswordChange(adminSeq, currentPassword, changePassword);
        }

        public ListModel<TAB_CMS_ADMIN> SearchList(AdminCondition condition)
        {
            return new AdminBiz().SearchList(condition);
        }

        public void Delete(int seq, LoginUser loginUser)
        {
            new AdminBiz().Delete(seq, loginUser);
        }

        public void DeleteList(List<int> seqList, LoginUser loginUser)
        {
            new AdminBiz().DeleteList(seqList, loginUser);
        }

        public MemoryStream ExcelExport(AdminCondition condition)
        {
            return new AdminBiz().ExcelExport(condition);
        }

		//어드민 ID 비밀번호 초기화 : 고정값(리턴값 없음)
		public void usp_AdminCmsPwdInitialize(string adminid)
		{
			new AdminBiz().usp_AdminCmsPwdInitialize(adminid);
		}

		//어드민 ID 비밀번호 초기화 : 난수값(리턴값이 있음)
		public SetPasswordResult AdminCmsPwdInitializeRanNum(string adminId)
		{
			return new AdminBiz().AdminCmsPwdInitializeRanNum(adminId);
		}


	}
}
