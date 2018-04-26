using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Family;


namespace Wow.Tv.Middle.WcfService.Family
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IFamilyService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IFamilyService
    {
        [OperationContract]
        ListModel<NTB_FAMILY> SearchList(FamilyCondition condition);

        [OperationContract]
        NTB_FAMILY GetAt(int familySeq);

        [OperationContract]
        int Save(NTB_FAMILY model, LoginUser loginUser);

        [OperationContract]
        void Delete(int familySeq, LoginUser loginUser);

        [OperationContract]
        void DeleteList(List<int> seqList, LoginUser loginUser);

        [OperationContract]
        void UpDown(int familySeq, bool isUp, LoginUser loginUser);

        [OperationContract]
        System.IO.MemoryStream ExcelExport(FamilyCondition condition);
    }
}
