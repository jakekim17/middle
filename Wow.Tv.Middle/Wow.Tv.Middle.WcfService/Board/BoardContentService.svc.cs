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
using Wow.Tv.Middle.Biz.AttachFile;

namespace Wow.Tv.Middle.WcfService.Board
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "BoardContentService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 BoardContentService.svc나 BoardContentService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class BoardContentService : IBoardContentService
    {
        public ListModel<NTB_BOARD_CONTENT> SearchList(BoardContentCondition condition)
        {
            return new BoardContentBiz().SearchList(condition);
        }


        public NTB_BOARD_CONTENT GetAt(int boardContentSeq)
        {
            return new BoardContentBiz().GetAt(boardContentSeq);
        }

        public void Save(NTB_BOARD_CONTENT model, LoginUser loginUser)
        {
            new BoardContentBiz().Save(model, loginUser);
        }

        public void Delete(int boardContentSeq, LoginUser loginUser)
        {
            new BoardContentBiz().Delete(boardContentSeq, loginUser);
        }



        
        public void CommentSave(NTB_BOARD_COMMENT model, LoginUser loginUser)
        {
            new BoardCommentBiz().Save(model, loginUser);
        }

        public void CommentDelete(int commentSeq, LoginUser loginUser)
        {
            new BoardCommentBiz().Delete(commentSeq, loginUser);
        }


        public void AttachFileDelete(int attachFileSeq)
        {
            new AttachFileBiz().Delete(attachFileSeq);
        }
    }
}
