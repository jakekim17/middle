using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Biz.Board;

namespace Wow.Tv.Middle.WcfService.Board
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "BoardService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 BoardService.svc나 BoardService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class BoardService : IBoardService
    {
        public ListModel<NTB_BOARD> SearchList(BoardCondition condition)
        {
            return new BoardBiz().SearchList(condition);
        }
        public NTB_BOARD GetAt(int boardSeq)
        {
            return new BoardBiz().GetAt(boardSeq);
        }

        public int Save(NTB_BOARD model, LoginUser loginUser)
        {
            return new BoardBiz().Save(model, loginUser);
        }

        public void Delete(int boardSeq, LoginUser loginUser)
        {
            new BoardBiz().Delete(boardSeq, loginUser);
        }

        public void DeleteList(List<int> seqList, LoginUser loginUser)
        {
            new BoardBiz().DeleteList(seqList, loginUser);
        }

        public System.IO.MemoryStream ExcelExport(BoardCondition condition)
        {
            return new BoardBiz().ExcelExport(condition);
        }
    }
}
