using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Component;
using Wow.Tv.Middle.Biz.Member;
using Wow.Tv.Middle.Biz.MemberManage;
using Wow.Tv.Middle.Biz.RegCateManage;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;
using Wow.Tv.Middle.Model.Db89.wowbill.MemberAdminManage;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.WcfService.MemberManage
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "MemberManageService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 MemberManageService.svc나 MemberManageService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class MemberManageService : IMemberManageService
    {
        public ListModel<MemberManageListResult> MemberSearchList(MemberManageCondition condition)
        {
            return new MemberManageBiz().MemberSearchList(condition);
        }

        public void SendSms(string receiveNo, string callbackNo, string message)
        {
            new SmsBiz().SendSms(receiveNo, callbackNo, message, "wowsms-member", "회원메시지 TV Admin", true);
        }

        public MemberManageInfoResult MemberInfo(int userNumber)
        {
            return new MemberManageBiz().MemberInfo(userNumber);
        }

        public UserInfoModifyResult MemberInfoSave(UserInfoSaveRequest request)
        {
            return new MemberManageBiz().MemberInfoSave(request);
        }

        //public UserInfoModifyResult MemberInfoDelete(int userNumber)
        //{
        //    return new MemberManageBiz().MemberInfoDelete(userNumber);
        //}

        public MemberSecessionResult MemberSecession(int userNumber, string secessionReasonKey, string secessionReasonDescription)
        {
            return new MemberManageBiz().MemberSecession(userNumber, secessionReasonKey, secessionReasonDescription);
        }

        public List<tblPost> GetAddress(string searchKeyword)
        {
            return new MemberManageBiz().GetAddress(searchKeyword);
        }

        public MemberInfoStatisticsResult MemberInfoStatistics()
        {
            return new MemberManageBiz().MemberInfoStatistics();
        }

        public SetPasswordResult PasswordInitialize(int userNumber, string adminId)
        {
            return new MemberManageBiz().PasswordInitialize(userNumber, adminId);
        }

        public void UserApproval(int userNumber, bool approved, string reason)
        {
            string bOQv5BillHost = System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"];
            new MemberManageBiz().UserApproval(userNumber, approved, reason, bOQv5BillHost);
        }

        public List<MemberInfoSimple> SimpleMemberList(List<int> userNumberList)
        {
            return new MemberManageBiz().SimpleMemberList(userNumberList);
        }

        public string RestoreDormancy(int userNumber)
        {
            return new MemberManageBiz().RestoreDormancy(userNumber);
        }

		//DI 재가입불가 리스트
		public ListModel<TblMemberDIDenialList> MemberDIDenialList(IntegratedBoardCondition condition)
		{
			return new MemberManageBiz().MemberDIDenialList(condition);
		}

		//DI 재가입불가 신규등록/수정
		public void MemberDIDenialWriteProc(TblMemberDIDenialList model)
		{
			new MemberManageBiz().MemberDIDenialWriteProc(model);
		}
		//DI 재가입불가 뷰 
		public TblMemberDIDenialList MemberDIDenialView(int seq)
		{
			return new MemberManageBiz().MemberDIDenialView(seq);
		}

		//삭제
		public void MemberDIDenialDelete(int seq)
		{
			new MemberManageBiz().MemberDIDenialDelete(seq);
		}


	}
}
