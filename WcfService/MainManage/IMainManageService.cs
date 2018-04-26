using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.MainManage;

namespace Wow.Tv.Middle.WcfService.MainManage
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IMainManageService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IMainManageService
    {
        [OperationContract]
        ListModel<NTB_MAIN_MANAGE> SearchList(MainManageCondition condition);

        [OperationContract]
        NTB_MAIN_MANAGE GetAt(int mainManageSeq);

        [OperationContract]
        int Save(NTB_MAIN_MANAGE model, LoginUser loginUser);

        [OperationContract]
        void Delete(int mainManageSeq);

        [OperationContract]
        void UpDown(int mainManageSeq, bool isUp, LoginUser loginUser);

        [OperationContract]
        ListModel<NTB_MAIN_MANAGE> GetFrontList(string mainTypeCode);
    }
}
