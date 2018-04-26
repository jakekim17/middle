using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Group;


namespace Wow.Tv.Middle.WcfService.Group
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IGroupService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IGroupService
    {
        [OperationContract]
        ListModel<NTB_GROUP> SearchList(GroupCondition condition);

        [OperationContract]
        NTB_GROUP GetAt(int groupSeq);

        [OperationContract]
        int Save(NTB_GROUP model, LoginUser loginUser);

        [OperationContract]
        void Delete(int groupSeq, LoginUser loginUser);

        [OperationContract]
        void Copy(int groupSeq, LoginUser loginUser);


        [OperationContract]
        List<CCC> B(int seq);

        [OperationContract]
        CCC BB(int seq);

        [OperationContract]
        AAA BBB(int seq);

        [OperationContract]
        My BBBB(int seq);
    }
}
