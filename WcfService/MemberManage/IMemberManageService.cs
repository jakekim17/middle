using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Member;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Member;
using Wow.Tv.Middle.Model.Db89.wowbill.MemberAdminManage;
using Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage;

namespace Wow.Tv.Middle.WcfService.MemberManage
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IMemberManageService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IMemberManageService
    {
        [OperationContract]
        ListModel<MemberManageListResult> MemberSearchList(MemberManageCondition condition);

        [OperationContract]
        MemberManageInfoResult MemberInfo(int userNumber);

        [OperationContract]
        UserInfoModifyResult MemberInfoSave(UserInfoSaveRequest request);

        //[OperationContract]
        //UserInfoModifyResult MemberInfoDelete(int userNumber);

        [OperationContract]
        MemberSecessionResult MemberSecession(int userNumber, string secessionReasonKey, string secessionReasonDescription);

        [OperationContract]
        List<tblPost> GetAddress(string searchKeyword);

        [OperationContract]
        MemberInfoStatisticsResult MemberInfoStatistics();

        [OperationContract]
        SetPasswordResult PasswordInitialize(int userNumber, string adminId);

        [OperationContract]
        void SendSms(string receiveNo, string callbackNo, string message);

        [OperationContract]
        void UserApproval(int userNumber, bool approved, string reason);

        [OperationContract]
        List<MemberInfoSimple> SimpleMemberList(List<int> userNumberList);

        [OperationContract]
        string RestoreDormancy(int userNumber);

		//DI 재가입불가 리스트
		[OperationContract]
		ListModel<TblMemberDIDenialList> MemberDIDenialList(IntegratedBoardCondition condition);

		//DI 재가입불가 신규등록/수정
		[OperationContract]
		void MemberDIDenialWriteProc(TblMemberDIDenialList model);

		//DI 재가입불가 뷰 
		[OperationContract]
		TblMemberDIDenialList MemberDIDenialView(int seq);

		//삭제
		[OperationContract]
		void MemberDIDenialDelete(int seq);



	}
}
