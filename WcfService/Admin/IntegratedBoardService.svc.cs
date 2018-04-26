using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Admin;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db90.DNRS;

namespace Wow.Tv.Middle.WcfService.Admin
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IIntegratedBoardService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IIntegratedBoardService
    {
        [OperationContract]
        IntegratedBoard<NTB_BOARD_CONTENT> IntegratedSearchList(IntegratedBoardCondition condition);

        [OperationContract]
        NTB_BOARD_CONTENT GetBoardDetail(int boardContentSeq);

        [OperationContract]
        int BoardSave(NTB_BOARD_CONTENT board, LoginUser loginUser);

        [OperationContract]
        void BoardDelete(int seq, LoginUser loginUser);

        [OperationContract]
        void FileSave(NTB_ATTACH_FILE model, int boardContentSeq,bool isDeleteFileAll);

        [OperationContract]
        IntegratedBoard<NTB_BOARD_CONTENT> GetBoardInfoAndDetail(int currentMenuSeq, int boardContentSeq);

        [OperationContract]
        NTB_BOARD GetBoardInfo(int currentMenuSeq);

        [OperationContract]
        void ReadCountIncrease(int boardContentSeq);

        [OperationContract]
        List<MainStatsModel> GetQnaStats();

        [OperationContract]
        List<MainStatsModel> GetIRStats();

        [OperationContract]
        List<MainStatsModel> GetReportStats();

        [OperationContract]
        void DeleteFile(int attachFile);

        [OperationContract]
        ListModel<NTB_BOARD_CONTENT> GetBroadBoard(BoardContentCondition condition);

        [OperationContract]
        List<T_NEWS_PRG> GetProgramList(string ingYn);

        [OperationContract]
        void CommentSave(NTB_BOARD_COMMENT comment, LoginUser loginUser);

        [OperationContract]
        List<NTB_BOARD_COMMENT> GetCommentList(int seq);

        [OperationContract]
        void CommentDelete(int seq, LoginUser loginUser);

        [OperationContract]
        void CommentUpdate(int seq, string content, LoginUser loginUser);

        [OperationContract]
        void UpdateContentBlind(int seq, string blind, LoginUser loginUser);

        [OperationContract]
        void UpdateReplyBlind(int seq, string blind, LoginUser loginUser);
        

        [OperationContract]
        ListModel<NTB_BOARD_COMMENT> GetCommentPaging(IntegratedBoardCondition condition);

        [OperationContract]
        ListModel<NTB_BOARD_CONTENT> GetFeedBackList(IntegratedBoardCondition condition);

        [OperationContract]
        NTB_MENU GetHelpPageToFeedBackSeq(string menuName);

        [OperationContract]
        NTB_MENU GetFeedBackProgamCodeToBoardSeq(string programCode);

        [OperationContract]
        int BoardFeedBackSave(NTB_BOARD_CONTENT model, LoginUser loginUser);

        [OperationContract]
        List<TAB_PROGRAM_LIST> GetCreateBoardProgramList(string ingYn);

        [OperationContract]
        IntegratedBoard<NTB_BOARD_CONTENT> MyMainIntegratedSearchCount(IntegratedBoardCondition condition);

        [OperationContract]
        NTB_BOARD_CONTENT GetMainNoticeContent();

        [OperationContract]
        tblUser GetMemberInfo(string userId);

        [OperationContract]
        List<NTB_BOARD_CONTENT> GetHelpMainNotice();

        [OperationContract]
        NTB_BOARD_CONTENT GetAdminSiteBoardDetail(int boardContentSeq);

        [OperationContract]
        List<NTB_BOARD_CONTENT> GetQuickNoticeList();
    }

    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "IntegratedBoardService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 IntegratedBoardService.svc나 IntegratedBoardService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class IntegratedBoardService : IIntegratedBoardService
    {
        public IntegratedBoard<NTB_BOARD_CONTENT> IntegratedSearchList(IntegratedBoardCondition condition)
        {
            var resultData = new IntegratedBoardBiz().IntegratedSearchList(condition);


            foreach (var item in resultData.ListData)
            {
                var userInfo = new PriMiddleService.MemberInfoServiceClient().GetMemberInfo(item.MOD_ID);
                if (userInfo != null)
                {
                    item.MOD_NICKNAME = userInfo.NickName;

                    if (item.BOARD_SEQ == 30 || item.BOARD_SEQ == 24)
                    {
                        item.USER_NAME = userInfo.name;
                    }
                    else
                    {
                        item.USER_NAME = userInfo?.NickName;
                    }
                }

            }
            return resultData;
        }

        public NTB_BOARD_CONTENT GetAdminSiteBoardDetail(int boardContentSeq)
        {
            var resultData = new IntegratedBoardBiz().GetBoardDetail(boardContentSeq);
            return resultData;
        }


        public NTB_BOARD_CONTENT GetBoardDetail(int boardContentSeq)
        {
            var resultData = new IntegratedBoardBiz().GetBoardDetail(boardContentSeq);

            var userInfo = new PriMiddleService.MemberInfoServiceClient().GetMemberInfo(resultData.USER_ID);


            if (userInfo != null)
            {
                if(resultData.BOARD_SEQ == 30 || resultData.BOARD_SEQ == 24)
                {
                    resultData.USER_NAME = userInfo.name;
                }
                else
                {
                    resultData.USER_NAME = userInfo?.NickName;
                }
            }
            else
            {
                resultData.USER_NAME = resultData.USER_NAME.Substring(0, 1) + "**";
            }

            return resultData;
        }


        public IntegratedBoard<NTB_BOARD_CONTENT> GetBoardInfoAndDetail(int currentMenuSeq, int boardContentSeq)
        {
            return new IntegratedBoardBiz().GetBoardInfoAndDetail(currentMenuSeq, boardContentSeq);
        }

        public NTB_BOARD GetBoardInfo(int currentMenuSeq)
        {
            return new IntegratedBoardBiz().GetBoardInfo(currentMenuSeq);
        }

        public int BoardSave(NTB_BOARD_CONTENT board, LoginUser loginUser)
        {
            return new IntegratedBoardBiz().BoardSave(board, loginUser);
        }

        public void BoardDelete(int seq, LoginUser loginUser)
        {
            new IntegratedBoardBiz().BoardDelete(seq, loginUser);
        }

        public void FileSave(NTB_ATTACH_FILE model, int boardContentSeq, bool isDeleteFileAll = false)
        {
            new IntegratedBoardBiz().FileSave(model, boardContentSeq, isDeleteFileAll);
        }

        public void ReadCountIncrease(int boardContentSeq)
        {
            new IntegratedBoardBiz().ReadCountIncrease(boardContentSeq);
        }

        #region 관리자 Main 1:1,IR,고객제보 통계 조회

        public List<MainStatsModel> GetQnaStats()
        {

            return new IntegratedBoardBiz().GetQnaStats();
        }

        public List<MainStatsModel> GetIRStats()
        {

            return new IntegratedBoardBiz().GetIRStats();
        }

        public List<MainStatsModel> GetReportStats()
        {

            return new IntegratedBoardBiz().GetReportStats();
        }

        #endregion

        public void DeleteFile(int attachFile)
        {
            new IntegratedBoardBiz().DeleteFile(attachFile);
        }

        public ListModel<NTB_BOARD_CONTENT> GetBroadBoard(BoardContentCondition condition)
        {
            return new Wow.Tv.Middle.Biz.Board.BoardContentBiz().GetBroadBoard(condition);
        }

        public List<T_NEWS_PRG> GetProgramList(string ingYn)
        {
            return new IntegratedBoardBiz().GetProgramList(ingYn);
        }

        public void CommentSave(NTB_BOARD_COMMENT comment, LoginUser loginUser)
        {
            new IntegratedBoardBiz().CommentSave(comment, loginUser);
        }

        public List<NTB_BOARD_COMMENT> GetCommentList(int seq)
        {
            var resultData = new IntegratedBoardBiz().GetCommentList(seq);

            foreach (var item in resultData)
            {
                var userInfo = new PriMiddleService.MemberInfoServiceClient().GetMemberInfo(item.REG_ID);
                if (userInfo != null)
                {
                    item.NickName = userInfo.NickName;
                }
                
            }
            return resultData;
        }

        public void CommentDelete(int seq, LoginUser loginUser)
        {
            new IntegratedBoardBiz().CommentDelete(seq, loginUser);
        }

        public void CommentUpdate(int seq, string content, LoginUser loginUser)
        {
            new IntegratedBoardBiz().CommentUpdate(seq, content, loginUser);
        }

        public ListModel<NTB_BOARD_COMMENT> GetCommentPaging(IntegratedBoardCondition condition)
        {
            var resultData = new IntegratedBoardBiz().GetCommentPaging(condition);

            foreach (var ntbBoardComment in resultData.ListData)
            {
                var userInfo = new PriMiddleService.MemberInfoServiceClient().GetMemberInfo(ntbBoardComment.REG_ID);
                if (userInfo != null)
                {
                    ntbBoardComment.NickName = userInfo.NickName;
                }
                  
            }

            return new IntegratedBoardBiz().GetCommentPaging(condition);
        }

        public void UpdateContentBlind(int seq, string blind, LoginUser loginUser)
        {
            new IntegratedBoardBiz().UpdateContentBlind(seq, blind, loginUser);
        }

        public void UpdateReplyBlind(int seq, string blind, LoginUser loginUser)
        {
            new IntegratedBoardBiz().UpdateReplyBlind(seq, blind, loginUser);
        }

        #region 시청자 의견
        public ListModel<NTB_BOARD_CONTENT> GetFeedBackList(IntegratedBoardCondition condition)
        {
            return new IntegratedBoardBiz().GetFeedBackList(condition);
        }

        public NTB_MENU GetHelpPageToFeedBackSeq(string menuName)
        {
            return new IntegratedBoardBiz().GetHelpPageToFeedBackSeq(menuName);
        }

        public NTB_MENU GetFeedBackProgamCodeToBoardSeq(string programCode)
        {
            return new IntegratedBoardBiz().GetFeedBackProgamCodeToBoardSeq(programCode);
        }

        public int BoardFeedBackSave(NTB_BOARD_CONTENT model, LoginUser loginUser)
        {
            return new IntegratedBoardBiz().BoardFeedBackSave(model, loginUser);
        }

        public List<TAB_PROGRAM_LIST> GetCreateBoardProgramList(string ingYn)
        {
            return new IntegratedBoardBiz().GetCreateBoardProgramList(ingYn);
        }
        #endregion

        #region 마이페이지 메인

        public IntegratedBoard<NTB_BOARD_CONTENT> MyMainIntegratedSearchCount(IntegratedBoardCondition condition)
        {
            return  new IntegratedBoardBiz().MyMainIntegratedSearchCount(condition);
        }

        public NTB_BOARD_CONTENT GetMainNoticeContent()
        {
            return  new IntegratedBoardBiz().GetMainNoticeContent();
        }
        #endregion

        public tblUser GetMemberInfo(string userId)
        {
            return new PriMiddleService.MemberInfoServiceClient().GetMemberInfo(userId);
        }

        #region 고객센터 메인에 Top4 공지사항 가져오기

        public List<NTB_BOARD_CONTENT> GetHelpMainNotice()
        {
            return new IntegratedBoardBiz().GetHelpMainNotice();
        }
        #endregion

        public List<NTB_BOARD_CONTENT> GetQuickNoticeList()
        {
            return new IntegratedBoardBiz().GetQuickNoticeList();
        }
    }
}
