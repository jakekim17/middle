using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Member;

namespace Wow.Tv.Middle.WcfService.Member
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IMemberUtilService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IMemberUtilService
    {
        [OperationContract]
        void SmsTempPassword(string mobileNo, string tempPassword);

        [OperationContract]
        string SmsDormancyAuth(string mobileNo);

        [OperationContract]
        void EmailAuth(int userNumber, string email, DateTime expireDate);

        [OperationContract]
        void EmailRegistGeneral(string userName, string nickName, string email, int bounsCash);

        [OperationContract]
        void EmailRegistCompany(string userName, string nickName, string email, int bounsCash);

        [OperationContract]
        void EmailUserEdit(string userId, string nickName, List<string> changedItemList, string email);

        [OperationContract]
        void EmailCompanyRegistRejectAlarm(string nickName, string email, string userId);

        [OperationContract]
        void EmailFindIdAlarm(string nickName, string email, string userId);

        [OperationContract]
        void EmailFindPasswordAlarm(string nickName, string email, string tempPassword);

        [OperationContract]
        void EmailUserSecession(string nickName, string email);

        [OperationContract]
        void EmailTemplateTest();
    }
}
