using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;

namespace Wow.Tv.Middle.WcfService.Board
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IBoardService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IBoardService
    {
        [OperationContract]
        ListModel<NTB_BOARD> SearchList(BoardCondition condition);

        [OperationContract]
        NTB_BOARD GetAt(int boardSeq);

        [OperationContract]
        int Save(NTB_BOARD model, LoginUser loginUser);

        [OperationContract]
        void Delete(int boardSeq, LoginUser loginUser);

        [OperationContract]
        void DeleteList(List<int> seqList, LoginUser loginUser);

        [OperationContract]
        System.IO.MemoryStream ExcelExport(BoardCondition condition);
    }
}
