using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Member;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Admin;

namespace Wow.Tv.Middle.WcfService.Admin
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IAdminService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IAdminService
    {
        [OperationContract]
        void SmsSend(string adminId);


        [OperationContract]
        TAB_CMS_ADMIN GetAt(int seq);


        [OperationContract]
        TAB_CMS_ADMIN GetAtById(string adminId);



        [OperationContract]
        TAB_CMS_ADMIN LoginCheck(string adminId, string pwd, string mobileAuthNumber, string ip);


        [OperationContract]
        List<NTB_MENU_GROUP> GetAdminGroup(int adminSeq);

        [OperationContract]
        int Save(TAB_CMS_ADMIN model);

        [OperationContract]
        void PasswordChange(int adminSeq, string currentPassword, string changePassword);



        [OperationContract]
        ListModel<TAB_CMS_ADMIN> SearchList(AdminCondition condition);

        [OperationContract]
        void Delete(int seq, LoginUser loginUser);

        [OperationContract]
        void DeleteList(List<int> seqList, LoginUser loginUser);

        [OperationContract]
        System.IO.MemoryStream ExcelExport(AdminCondition condition);

		//어드민 ID 비밀번호 초기화 : 고정값(리턴값 없음)
		[OperationContract]
		void usp_AdminCmsPwdInitialize(string adminid);

		//어드민 ID 비밀번호 초기화 : 난수값(리턴값이 있음)
		[OperationContract]
		SetPasswordResult AdminCmsPwdInitializeRanNum(string adminId);



	}
}
