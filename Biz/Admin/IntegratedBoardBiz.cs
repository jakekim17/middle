using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Wow.Fx;
using Wow.Tv.Middle.Biz.AttachFile;
using Wow.Tv.Middle.Biz.Component;
using Wow.Tv.Middle.Biz.Menu;
using Wow.Tv.Middle.Biz.MyPage;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Model.Db49.wowtv.CommonCode;
using Wow.Tv.Middle.Model.Db90.DNRS;

namespace Wow.Tv.Middle.Biz.Admin
{

    /// <summary>
    /// <para>  통합게시판 Biz</para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-08-22</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-09-21</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-08-22 생성</para>
    /// <para>  2017-09-21 정재민 : 일감 적용 (http://172.19.0.21/redmine/issues/82)</para>
    /// <para>  2017-09-25 정재민 : 통합마이페이지_v0.5_170914 개인정보 동의 여부가 없어서 null 체크 추가</para>
    /// </summary>
    /// <remarks></remarks>
    public class IntegratedBoardBiz : BaseBiz
    {
        /// <summary>
        /// 통합 게시판 정보를 가져온다
        /// </summary>
        /// <param name="menuSeq">메뉴 seq</param>
        /// <returns>NTB_BOARD</returns>
        public NTB_BOARD GetBoard(int menuSeq)
        {
            var board = db49_wowtv.NTB_BOARD.SingleOrDefault(n => n.BOARD_SEQ == menuSeq);

            return board;
        }

        /// <summary>
        /// 검색 된 List
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IntegratedBoard<NTB_BOARD_CONTENT> IntegratedSearchList(IntegratedBoardCondition condition)
        {
            IntegratedBoard<NTB_BOARD_CONTENT> resultData = new IntegratedBoard<NTB_BOARD_CONTENT>();

            var list = db49_wowtv.NTB_BOARD_CONTENT.AsQueryable();

            var boardInfo = (from ntbMenu in db49_wowtv.NTB_MENU.AsNoTracking()
                             join ntbBoard in db49_wowtv.NTB_BOARD.AsNoTracking() on ntbMenu.CONTENT_SEQ equals ntbBoard.BOARD_SEQ
                             where ntbMenu.MENU_SEQ == condition.CurrentMenuSeq
                             select ntbBoard).SingleOrDefault();

            if (boardInfo != null)
            {
                resultData.BoardInfo = boardInfo.Clone() as NTB_BOARD;
                if (resultData.BoardInfo != null)
                {
                    list = list.Where(a => a.BOARD_SEQ.Equals(resultData.BoardInfo.BOARD_SEQ));
                    if (resultData.BoardInfo.REPLY_YN == "Y")
                    {
                        list = list.Where(a => a.UP_BOARD_CONTENT_SEQ.Equals(0));//답변 아닌것만 가져온다.
                    }
                }

                //삭제여부
                if (!string.IsNullOrEmpty(condition.DEL_YN) && !condition.DEL_YN.Equals("ALL"))
                {
                    list = list.Where(a => a.DEL_YN.Contains(condition.DEL_YN));
                }
                else
                {
                    list = list.Where(a => a.DEL_YN == "N");
                }

                //게시여부
                if (!string.IsNullOrEmpty(condition.VIEW_YN) && !condition.VIEW_YN.Equals("ALL"))
                {
                    list = list.Where(a => a.VIEW_YN.Contains(condition.VIEW_YN));
                }

                if (!string.IsNullOrEmpty(condition.UP_COMMON_CODE) && !condition.UP_COMMON_CODE.Equals("ALL"))
                {
                    list = list.Where(a => a.UP_COMMON_CODE.Contains(condition.UP_COMMON_CODE));
                }

                if (!string.IsNullOrEmpty(condition.COMMON_CODE) && !condition.COMMON_CODE.Equals("ALL"))
                {
                    list = list.Where(a => a.COMMON_CODE.Contains(condition.COMMON_CODE));
                }

                //기간 검색
                if (condition.START_DATE.Year > 1900 && condition.END_DATE.Year > 1900)
                {
                    list = list.Where(a => a.START_DATE >= condition.START_DATE && a.START_DATE <= condition.END_DATE || a.END_DATE >= condition.START_DATE && a.END_DATE <= condition.END_DATE);
                }

                if (!string.IsNullOrEmpty(condition.LoginId))
                {
                    list = list.Where(a => a.USER_ID.Contains(condition.LoginId));
                }

                if (!string.IsNullOrEmpty(condition.NoticeYn))
                {
                    list = list.Where(a => a.NOTICE_YN == condition.NoticeYn);
                }


                if (!string.IsNullOrEmpty(condition.COMMON_CODE_2) && !condition.COMMON_CODE_2.Equals("ALL"))
                {
                    list = list.Where(a => a.COMMON_CODE.Contains(condition.COMMON_CODE_2));
                }
                else if (!string.IsNullOrEmpty(condition.COMMON_CODE_1) && !condition.COMMON_CODE_1.Equals("ALL"))
                {
                    CommonCodeCondition commonCodeCondition = new CommonCodeCondition();
                    commonCodeCondition.UpCommonCode = condition.COMMON_CODE_1;
                    commonCodeCondition.PageSize = -1;
                    var businessList = new CommonCode.CommonCodeBiz().SearchList(commonCodeCondition).ListData.Select(a => a.COMMON_CODE).ToList();
                    businessList.Add(condition.COMMON_CODE_1);

                    list = list.Where(a => businessList.Contains(a.UP_COMMON_CODE));
                }




                //검색
                if (!string.IsNullOrEmpty(condition.SearchText))
                {
                    if (condition.SearchType.Equals("TITLE"))
                    {
                        list = list.Where(a => a.TITLE.Contains(condition.SearchText));
                    }
                    else if (condition.SearchType.Equals("CONTENT"))
                    {
                        list = list.Where(a => a.CONTENT.Contains(condition.SearchText));
                    }
                    else if (condition.SearchType.Equals("WRITE"))
                    {
                        list = list.Where(a => a.USER_NAME.Contains(condition.SearchText));
                    }
                    else
                    {
                        list = list.Where(a => a.TITLE.Contains(condition.SearchText) || a.CONTENT.Contains(condition.SearchText) || (a.USER_NAME != null && a.USER_NAME.Contains(condition.SearchText)));
                    }

                }

                resultData.TotalDataCount = list.Count();

                if (condition.PageSize > -1)
                {
                    if (condition.PageSize == 0)
                    {
                        condition.PageSize = 20;
                    }

                    if (boardInfo.TOP_NOTICE_YN.Equals("Y"))
                    {
                        list = list.OrderByDescending(a => a.NOTICE_YN).ThenByDescending(a => a.REG_DATE).Skip(condition.CurrentIndex).Take(condition.PageSize);
                    }
                    else
                    {
                        list = list.OrderByDescending(a => a.REG_DATE).Skip(condition.CurrentIndex).Take(condition.PageSize);
                    }

                }


                //데이터 조회
                resultData.ListData = list.ToList();

                IEnumerable<NTB_BOARD_CONTENT> ntbBoardContents = resultData.ListData;

                //답변 여부
                if (!string.IsNullOrWhiteSpace(condition.REPLY_YN) && !condition.REPLY_YN.Equals("ALL"))
                {
                    ntbBoardContents = ntbBoardContents.Where(a => a.REPLY_YN.Equals(condition.REPLY_YN));
                }
                foreach (var item in ntbBoardContents)
                {
                    if (!string.IsNullOrWhiteSpace(item.COMMON_CODE))
                    {
                        var commonCode = db49_wowtv.NTB_COMMON_CODE.AsNoTracking().SingleOrDefault(x => x.COMMON_CODE.Equals(item.COMMON_CODE.ToString()));
                        if (commonCode != null) item.CommonName = commonCode.CODE_NAME;

                    }
                    if (string.IsNullOrWhiteSpace(item.UP_COMMON_CODE)) continue;
                    {
                        var upCommonCode = db49_wowtv.NTB_COMMON_CODE.AsNoTracking().SingleOrDefault(x => x.COMMON_CODE.Equals(item.UP_COMMON_CODE.ToString()));
                        if (upCommonCode != null) item.UpCommonName = upCommonCode.CODE_NAME;
                    }
                }

                //ntbBoardContents = (from ntbBoard in ntbBoardContents
                //                    join users in db89_wowbill.tblUser.AsNoTracking() on ntbBoard.MOD_ID equals users.userId into u
                //                    from user in u.DefaultIfEmpty()
                //                    select new NTB_BOARD_CONTENT
                //                    {
                //                        COMMON_CODE = ntbBoard.COMMON_CODE,
                //                        UP_COMMON_CODE = ntbBoard.UP_COMMON_CODE,
                //                        CommonName = ntbBoard.CommonName,
                //                        UpCommonName = ntbBoard.UpCommonName,
                //                        USER_ID = ntbBoard.USER_ID,
                //                        USER_NAME = user == null ? ntbBoard.USER_NAME : user.NickName,
                //                        DEL_YN = ntbBoard.DEL_YN,
                //                        START_DATE = ntbBoard.START_DATE,
                //                        END_DATE = ntbBoard.END_DATE,
                //                        BOARD_SEQ = ntbBoard.BOARD_SEQ,
                //                        MenuSeq = ntbBoard.MenuSeq,
                //                        UP_BOARD_CONTENT_SEQ = ntbBoard.UP_BOARD_CONTENT_SEQ,
                //                        VIEW_YN = ntbBoard.VIEW_YN,
                //                        READ_CNT = ntbBoard.READ_CNT,
                //                        REPLY_YN = ntbBoard.REPLY_YN,
                //                        TITLE = ntbBoard.TITLE,
                //                        CONTENT = ntbBoard.CONTENT,
                //                        NOTICE_YN = ntbBoard.NOTICE_YN,
                //                        BOARD_CONTENT_SEQ = ntbBoard.BOARD_CONTENT_SEQ,
                //                        IsReply = ntbBoard.IsReply,
                //                        REG_DATE = ntbBoard.REG_DATE,
                //                        MOD_DATE = ntbBoard.MOD_DATE,
                //                        BLIND_YN = ntbBoard.BLIND_YN,
                //                        AttachFileList = ntbBoard.AttachFileList,
                //                        ReplyList = ntbBoard.ReplyList
                //                    }).ToList();


                if (resultData.BoardInfo?.FILE_COUNT > 0) //첨부파일 목록
                {
                    var attachFiles = db49_wowtv.NTB_ATTACH_FILE.AsNoTracking().Where(x => x.TABLE_CODE.Equals("NTB_BOARD_CONTENT") && x.DEL_YN.Equals("N"));

                    foreach (var item in ntbBoardContents)
                    {
                        var attachFile = attachFiles.Where(x => x.TABLE_KEY.Equals(item.BOARD_CONTENT_SEQ.ToString())).ToList();
                        item.AttachFileList = attachFile;
                    }
                }

                if (resultData.BoardInfo?.COMMENT_YN == "Y") //댓글 목록
                {

                    foreach (var item in ntbBoardContents)
                    {
                        var comment = db49_wowtv.NTB_BOARD_COMMENT.AsNoTracking().Where(x => x.BOARD_CONTENT_SEQ.Equals(item.BOARD_CONTENT_SEQ) && x.DEL_YN.Equals("N")).ToList();
                        item.CommentList = comment;
                    }
                }

                if (resultData.BoardInfo?.REPLY_YN == "Y") //답변여부
                {
                    var replyList = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().Where(x => x.BOARD_SEQ == boardInfo.BOARD_SEQ && x.UP_BOARD_CONTENT_SEQ > 0 && x.DEL_YN.Equals("N")).ToList().Clone();

                    foreach (var item in ntbBoardContents)
                    {
                        var reply = replyList.Where(x => x.UP_BOARD_CONTENT_SEQ.Equals(item.BOARD_CONTENT_SEQ)).ToList();
                        item.ReplyList = reply;
                    }
                }

                //해당 게시판 타입이 시청자 의견 타입이면..프로그램명으로
                if (boardInfo.BOARD_TYPE_CODE.Equals("FeedBack"))
                {
                    var newProgramList = db90_DNRS.T_NEWS_PRG.AsNoTracking().Where(x => x.DELFLAG.Equals("0")).ToList();

                    foreach (var item in ntbBoardContents)
                    {
                        if (!string.IsNullOrWhiteSpace(item.COMMON_CODE))
                        {
                            var programName = newProgramList.FirstOrDefault(x => x.PRG_CD.Equals(item.COMMON_CODE));
                            item.CommonName = programName.PRG_NM;
                        }
                    }
                }
            }
            //순환 참조 제거
            foreach (var item in resultData.ListData)
            {
                item.NTB_BOARD = null;
                item.NTB_BOARD_COMMENT = null;
            }
            return resultData;
        }

        /// <summary>
        /// 선택 된 게시물 상세 정보를 가져온다.
        /// </summary>
        /// <param name="boardContentSeq">seq</param>
        /// <returns>NTB_BOARD_CONTENT</returns>
        public NTB_BOARD_CONTENT GetBoardDetail(int boardContentSeq)
        {
            var ntbBoardContent = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().SingleOrDefault(a => a.BOARD_CONTENT_SEQ == boardContentSeq);

            if (!(ntbBoardContent?.Clone() is NTB_BOARD_CONTENT boardItem)) return null;



            var boardInfo = db49_wowtv.NTB_BOARD.AsNoTracking().SingleOrDefault(a => a.BOARD_SEQ == boardItem.BOARD_SEQ);

            if (boardInfo?.FILE_COUNT > 0)
            {
                var attachFiles = db49_wowtv.NTB_ATTACH_FILE.AsNoTracking().Where(x => x.TABLE_CODE.Equals("NTB_BOARD_CONTENT") && x.TABLE_KEY.Equals(boardContentSeq.ToString()) && x.DEL_YN.Equals("N")).ToList();
                boardItem.AttachFileList = attachFiles;
                boardItem.IsFile = true;
            }


            if (boardInfo?.COMMENT_YN == "Y") //댓글 목록
            {

                var comment = db49_wowtv.NTB_BOARD_COMMENT.AsNoTracking().Where(x => x.BOARD_CONTENT_SEQ.Equals(boardItem.BOARD_CONTENT_SEQ) && x.DEL_YN.Equals("N")).ToList();
                if (comment != null)
                {
                    boardItem.CommentList = comment;
                }
            }



            if (boardInfo?.REPLY_YN != "Y") return boardItem;

            var reply = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().Where(x => x.BOARD_SEQ == boardInfo.BOARD_SEQ && x.UP_BOARD_CONTENT_SEQ == boardItem.BOARD_CONTENT_SEQ && x.DEL_YN.Equals("N")).ToList().Clone();
            boardItem.IsReply = true;
            boardItem.ReplyList = new List<NTB_BOARD_CONTENT>(reply);


            if(boardItem != null)
            {
                var CodeList = db49_wowtv.NTB_COMMON_CODE.Where(a => a.COMMON_CODE.Equals(boardItem.COMMON_CODE)).SingleOrDefault();
                if(CodeList != null)
                {

                    boardItem.CommonName = CodeList.CODE_NAME;
                }
            }


            return boardItem;
        }

        /// <summary>
        /// 답변 상세 정보를 가져온다.
        /// </summary>
        /// <param name="upBoardContentSeq">seq</param>
        /// <returns>NTB_BOARD_CONTENT</returns>
        public NTB_BOARD_CONTENT GetReplyBoardDetail(int upBoardContentSeq)
        {
            var ntbBoardContent = db49_wowtv.NTB_BOARD_CONTENT.SingleOrDefault(a => a.UP_BOARD_CONTENT_SEQ == upBoardContentSeq);
            if (ntbBoardContent == null) return null;

            if (!(ntbBoardContent.Clone() is NTB_BOARD_CONTENT boardItem)) return null;

            var boardInfo = db49_wowtv.NTB_BOARD.SingleOrDefault(a => a.BOARD_SEQ == boardItem.BOARD_SEQ);

            if (boardInfo?.FILE_COUNT > 0)
            {
                var attachFiles = db49_wowtv.NTB_ATTACH_FILE.Where(x => x.TABLE_CODE.Equals("NTB_BOARD_CONTENT") && x.TABLE_KEY.Equals(upBoardContentSeq.ToString()) && x.DEL_YN.Equals("N")).ToList();
                boardItem.AttachFileList = attachFiles;
                boardItem.IsFile = true;
            }

            if (boardInfo?.REPLY_YN != "Y") return boardItem;

            var reply = db49_wowtv.NTB_BOARD_CONTENT.Where(x => x.BOARD_SEQ == boardInfo.BOARD_SEQ && x.UP_BOARD_CONTENT_SEQ == boardItem.BOARD_CONTENT_SEQ && x.DEL_YN.Equals("N")).ToList().Clone();
            boardItem.IsReply = true;
            boardItem.ReplyList = new List<NTB_BOARD_CONTENT>(reply);

            return boardItem;
        }

        public NTB_BOARD GetBoardInfo(int currentMenuSeq)
        {
            var boardInfo = (from ntbMenu in db49_wowtv.NTB_MENU
                             join ntbBoard in db49_wowtv.NTB_BOARD on ntbMenu.CONTENT_SEQ equals ntbBoard.BOARD_SEQ
                             where ntbMenu.MENU_SEQ == currentMenuSeq
                             select ntbBoard).SingleOrDefault();

            return boardInfo;
        }

        /// <summary>
        /// 선택 된 게시물 상세 정보, 게시판 정보를 가져온다.
        /// </summary>
        /// <param name="currentMenuSeq"></param>
        /// <param name="boardContentSeq">seq</param>
        /// <returns>NTB_BOARD_CONTENT</returns>
        public IntegratedBoard<NTB_BOARD_CONTENT> GetBoardInfoAndDetail(int currentMenuSeq, int boardContentSeq)
        {

            var attachFile = db49_wowtv.NTB_ATTACH_FILE.AsNoTracking().Where(x => x.TABLE_CODE.Equals("NTB_BOARD_CONTENT") && x.TABLE_KEY.Equals(boardContentSeq.ToString()) && x.DEL_YN.Equals("N")).ToList();

            NTB_BOARD_CONTENT boardItem = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().SingleOrDefault(a => a.BOARD_CONTENT_SEQ == boardContentSeq);

            IntegratedBoard<NTB_BOARD_CONTENT> resultData = new IntegratedBoard<NTB_BOARD_CONTENT> { BoardInfo = GetBoardInfo(currentMenuSeq) };

            if (boardItem == null) return resultData;

            boardItem.AttachFileList = attachFile;

            return resultData;
        }

        /// <summary>
        /// 게시물을 저장한다.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        public int BoardSave(NTB_BOARD_CONTENT model, LoginUser loginUser)
        {

            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    NTB_BOARD_CONTENT boardSingle = GetBoardDetail(model.BOARD_CONTENT_SEQ);
                    if (boardSingle == null)
                    {
                        TopFiveUpdate(model.BOARD_SEQ, model.COMMON_CODE);
                        model.DEL_YN = "N";
                        model.REPLY_YN = "N";
                        model.REG_DATE = DateTime.Now;
                        if (loginUser == null)
                        {
                            //일단 비회원은 이름으로 등록한다.
                            //퍼블에서 model.AGREE_YN.Equals("Y")  없어짐
                            if (model.AGREE_YN == null)
                            {
                                model.REG_ID = model.USER_NAME;
                                model.MOD_ID = model.USER_NAME;
                            }
                            else
                            {
                                model.REG_ID = model.AGREE_YN.Equals("Y") ? model.USER_NAME : model.USER_ID;
                                model.MOD_ID = model.AGREE_YN.Equals("Y") ? model.USER_NAME : model.USER_ID;
                            }
                        }
                        else
                        {
                            model.REG_ID = loginUser.LoginId;
                            model.USER_ID = loginUser.LoginId;
                            model.USER_NAME = loginUser.UserName;
                            model.MOD_ID = loginUser.LoginId;
                        }

                        model.MOD_DATE = DateTime.Now;
                        db49_wowtv.NTB_BOARD_CONTENT.Add(model);
                        db49_wowtv.SaveChanges();
                        if (model.NOTICE_YN == "Y")
                        {
                            //상단공지는 각 사이트별 최대 5개까지 지정한다.
                            TopFiveUpdate(model.BOARD_SEQ, model.COMMON_CODE);
                        }

                        if(model.UP_COMMON_CODE != null)
                        {
                            var upCommonCode = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(a => a.COMMON_CODE.Equals(model.UP_COMMON_CODE)).UP_COMMON_CODE;
                            if (model.BOARD_SEQ == 25 && upCommonCode == "044000000")
                            {
                                EmailBusinessInqury(model.COMMON_CODE);
                            }
                        }
                    }
                    else
                    {
                        if (model.UP_BOARD_CONTENT_SEQ > 0 && model.DEPTH > 0) //답변 처리 
                        {
                            ReplySave(model, loginUser);

                            boardSingle.REPLY_YN = "Y";
                        }
                        else
                        {
                            boardSingle.VIEW_YN = model.VIEW_YN;
                            boardSingle.NOTICE_YN = model.NOTICE_YN;
                            if (model.START_DATE != null && model.START_DATE.Value.Year > 1900)
                                boardSingle.START_DATE = model.START_DATE;
                            if (model.END_DATE != null && model.END_DATE.Value.Year > 1900)
                                boardSingle.END_DATE = model.END_DATE;
                            if (model.CONTENT != null && model.TITLE != null) //1:1문의에서 답글 없을 때 처리 안되도록
                            {
                                boardSingle.CONTENT = model.CONTENT;
                                boardSingle.TITLE = model.TITLE;
                            }
                            boardSingle.USER_NAME = loginUser.UserName;
                            boardSingle.PAGE_LINK = model.PAGE_LINK;
                        }

                        boardSingle.DEL_YN = model.DEL_YN;
                        boardSingle.COMMON_CODE = model.COMMON_CODE;
                        boardSingle.UP_COMMON_CODE = model.UP_COMMON_CODE;
                        boardSingle.MOD_ID = loginUser.LoginId;
                        boardSingle.MOD_DATE = DateTime.Now;
                        db49_wowtv.NTB_BOARD_CONTENT.AddOrUpdate(boardSingle);
                        db49_wowtv.SaveChanges();

                        if (boardSingle.NOTICE_YN == "Y")
                        {
                            //상단공지는 각 사이트별 최대 5개까지 지정한다.
                            TopFiveUpdate(boardSingle.BOARD_SEQ, model.COMMON_CODE);
                        }
                    }
                    dbContextTransaction.Commit();

                    return model.BOARD_CONTENT_SEQ;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }




        /// <summary>
        /// 답변 저장
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        private void ReplySave(NTB_BOARD_CONTENT model, LoginUser loginUser)
        {
            NTB_BOARD_CONTENT replyData = GetReplyBoardDetail(model.UP_BOARD_CONTENT_SEQ);
            if (replyData != null && model.BOARD_CONTENT_SEQ == model.UP_BOARD_CONTENT_SEQ) //같으면 수정 
            {

                //replyData.COMMON_CODE = model.COMMON_CODE;
                replyData.CONTENT = model.CONTENT;
                replyData.TITLE = model.TITLE;
                replyData.DEL_YN = model.DEL_YN;
                replyData.MOD_ID = loginUser.LoginId;
                replyData.USER_NAME = loginUser.UserName;
                replyData.MOD_DATE = DateTime.Now;
                db49_wowtv.NTB_BOARD_CONTENT.AddOrUpdate(replyData);
            }
            else
            {
                model.REPLY_YN = "N";
                model.DEL_YN = "N";
                model.REG_DATE = DateTime.Now;
                model.REG_ID = loginUser.LoginId;
                model.USER_ID = loginUser.LoginId;
                model.USER_NAME = loginUser.UserName;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;
                model.READ_CNT = 0;
                model.SORD_ORDER = 0;
                model.CONTENT_ID = 0;
                //model.NTB_BOARD = null;
                //model.NTB_BOARD_COMMENT = null;
                //boardSingle.NTB_BOARD = null;
                //boardSingle.NTB_BOARD_COMMENT = null;
                db49_wowtv.NTB_BOARD_CONTENT.Add(model);
            }
            db49_wowtv.SaveChanges();
        }
        /// <summary>
        /// 상위 Top5 공지
        /// </summary>
        /// <param name="boardSeq"></param>
        /// <param name="commonCode"></param>
        private void TopFiveUpdate(int boardSeq, string commonCode)
        {

            var topFiveList = db49_wowtv.NTB_BOARD_CONTENT.Where(x => x.BOARD_SEQ == boardSeq && x.COMMON_CODE.Equals(commonCode) && x.NOTICE_YN.Equals("Y")).OrderByDescending(a => a.REG_DATE).ToList().Clone();


            if (topFiveList.Count <= 5) return;
            {
                for (int i = 5; i < topFiveList.Count; i++)
                {
                    int seq = topFiveList[i].BOARD_CONTENT_SEQ;
                    NTB_BOARD_CONTENT boardContent = db49_wowtv.NTB_BOARD_CONTENT.SingleOrDefault(a => a.BOARD_CONTENT_SEQ == seq);
                    if (boardContent != null)
                    {
                        boardContent.NOTICE_YN = "N";
                    }
                    db49_wowtv.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 게시물을 삭제한다.
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="loginUser">로그인유저정보</param>
        public void
            BoardDelete(int seq, LoginUser loginUser)
        {
            NTB_BOARD_CONTENT boardSingle = GetBoardDetail(seq);

            if (boardSingle == null) return;

            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    if (boardSingle.REPLY_YN != null && boardSingle.REPLY_YN.Equals("Y")) //답변 삭제
                    {
                        NTB_BOARD_CONTENT replyInfo = GetReplyBoardDetail(boardSingle.BOARD_CONTENT_SEQ);

                        if (replyInfo.ReplyList.Count == 0)
                        {
                            replyInfo.DEL_YN = "Y";
                            replyInfo.USER_ID = loginUser.LoginId;
                            replyInfo.MOD_ID = loginUser.LoginId;
                            replyInfo.MOD_DATE = DateTime.Now;
                            db49_wowtv.NTB_BOARD_CONTENT.AddOrUpdate(replyInfo);
                            db49_wowtv.SaveChanges();
                        }
                        else
                        {
                            foreach (var replyItem in replyInfo.ReplyList)
                            {
                                replyItem.DEL_YN = "Y";
                                replyItem.USER_ID = loginUser.LoginId;
                                replyItem.MOD_ID = loginUser.LoginId;
                                replyItem.MOD_DATE = DateTime.Now;
                                db49_wowtv.NTB_BOARD_CONTENT.AddOrUpdate(replyItem);
                                db49_wowtv.SaveChanges();
                            }
                        }

                    }
                    else
                    {
                        //코멘트 삭제
                        var commentList = db49_wowtv.NTB_BOARD_COMMENT.Where(x => x.BOARD_CONTENT_SEQ.Equals(boardSingle.BOARD_CONTENT_SEQ) && x.DEL_YN.Equals("N")).ToList();

                        foreach (var item in commentList)
                        {
                            item.DEL_YN = "Y";
                            item.MOD_DATE = DateTime.Now;
                            item.MOD_ID = loginUser.LoginId;
                            db49_wowtv.NTB_BOARD_COMMENT.AddOrUpdate(item);
                            db49_wowtv.SaveChanges();
                        }


                        //Del_YN 으로 처리 Y는 삭제 N은 정상
                        boardSingle.DEL_YN = "Y";
                        boardSingle.USER_ID = loginUser.LoginId;
                        boardSingle.MOD_ID = loginUser.LoginId;
                        boardSingle.MOD_DATE = DateTime.Now;
                        db49_wowtv.NTB_BOARD_CONTENT.AddOrUpdate(boardSingle);
                        db49_wowtv.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

        }
        /// <summary>
        /// 파일 저장
        /// </summary>
        /// <param name="model"></param>
        /// <param name="boardContentSeq"></param>
        /// <param name="isDeleteFileAll"></param>
        public void FileSave(NTB_ATTACH_FILE model, int boardContentSeq, bool isDeleteFileAll = false)
        {
            model.TABLE_CODE = "NTB_BOARD_CONTENT";
            model.TABLE_KEY = boardContentSeq.ToString();
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    if (isDeleteFileAll)
                        new AttachFileBiz().DeleteAll(model.TABLE_CODE, model.TABLE_KEY);

                    new AttachFileBiz().Create(model);
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void DeleteFile(int attachFile)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    new AttachFileBiz().Delete(attachFile);
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 조회수를 1씩 증가한다.
        /// </summary>
        /// <param name="boardContentSeq">게시판 번호</param>
        public void ReadCountIncrease(int boardContentSeq)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                NTB_BOARD_CONTENT boardSingle = GetBoardDetail(boardContentSeq);

                if (boardSingle == null) return;
                try
                {
                    int readCount = boardSingle.READ_CNT + 1;
                    db49_wowtv.Database.ExecuteSqlCommand($"UPDATE NTB_BOARD_CONTENT SET READ_CNT = {readCount} WHERE BOARD_CONTENT_SEQ = {boardContentSeq} ");
                    //"UPDATE dbo.Blogs SET Name = 'Another Name' WHERE BlogId = 1")};
                    //boardSingle.READ_CNT = boardSingle.READ_CNT + 1;
                    //db49_wowtv.Entry(boardSingle).State = EntityState.Modified;
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 메인화면 QNA 통계 정보를 가져온다.
        /// </summary>
        /// <returns></returns>
        public List<MainStatsModel> GetQnaStats()
        {
            List<MainStatsModel> resultData = new List<MainStatsModel>();

            var menuSeqCode = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(x => x.COMMON_CODE.Contains(CommonCodeStatic.MAIN_QNA_SEQ_CODE))?.CODE_VALUE1;

            if (menuSeqCode == null) return resultData;

            int menuSeq;
            if (!int.TryParse(menuSeqCode, out menuSeq)) return resultData;

            var boardInfo = (from ntbMenu in db49_wowtv.NTB_MENU
                             join ntbBoard in db49_wowtv.NTB_BOARD on ntbMenu.CONTENT_SEQ equals ntbBoard.BOARD_SEQ
                             where ntbMenu.MENU_SEQ == menuSeq
                             select ntbBoard).SingleOrDefault();

            if (boardInfo == null) return resultData;

            var qnaCommonList = db49_wowtv.NTB_COMMON_CODE.Where(x => x.UP_COMMON_CODE.Contains(CommonCodeStatic.INTEGRATED_BOARD_INQUIRY_CODE)).ToList();

            var qnaList = (from content in db49_wowtv.NTB_BOARD_CONTENT
                           join common in db49_wowtv.NTB_COMMON_CODE on content.UP_COMMON_CODE equals common.COMMON_CODE
                           where common.UP_COMMON_CODE == CommonCodeStatic.INTEGRATED_BOARD_INQUIRY_CODE
                                 && content.BOARD_SEQ == boardInfo.BOARD_SEQ
                                 && content.DEPTH == 0
                                 && content.DEL_YN.Contains("N")
                           select new MainStatsModel
                           {
                               GroupCode = common.COMMON_CODE,
                               GroupName = common.CODE_NAME
                           }).AsQueryable().ToList();

            resultData = (from common in qnaCommonList
                          select new MainStatsModel
                          {
                              GroupCode = common.COMMON_CODE,
                              GroupName = common.CODE_NAME,
                              GroupCount = qnaList.Count(x => x.GroupCode.Contains(common.COMMON_CODE))
                          }).ToList();


            return resultData;
        }

        /// <summary>
        /// IR 사이트 문의 통계 정보를 가져온다.
        /// </summary>
        /// <returns></returns>
        public List<MainStatsModel> GetIRStats()
        {
            List<MainStatsModel> resultData = new List<MainStatsModel>();

            var menuSeqCode = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(x => x.COMMON_CODE.Contains(CommonCodeStatic.MAIN_IR_SEQ_CODE))?.CODE_VALUE1;

            if (menuSeqCode == null) return resultData;

            int menuSeq;
            if (!int.TryParse(menuSeqCode, out menuSeq)) return resultData;

            var boardInfo = (from ntbMenu in db49_wowtv.NTB_MENU
                             join ntbBoard in db49_wowtv.NTB_BOARD on ntbMenu.CONTENT_SEQ equals ntbBoard.BOARD_SEQ
                             where ntbMenu.MENU_SEQ == menuSeq
                             select ntbBoard).SingleOrDefault();

            if (boardInfo == null) return resultData;

            var qnaCommonList = db49_wowtv.NTB_COMMON_CODE.Where(x => x.UP_COMMON_CODE.Contains(CommonCodeStatic.INTEGRATED_BOARD_IR_INQUIRY_CODE)).ToList();
            qnaCommonList.Add(db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(x => x.COMMON_CODE.Equals("042001000")));
            qnaCommonList.Add(db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(x => x.COMMON_CODE.Equals("043001000")));

            var qnaList = (from content in db49_wowtv.NTB_BOARD_CONTENT
                           join common in db49_wowtv.NTB_COMMON_CODE on content.UP_COMMON_CODE equals common.COMMON_CODE
                           where /*common.UP_COMMON_CODE == CommonCodeStatic.INTEGRATED_BOARD_IR_INQUIRY_CODE
                                 &&*/ content.BOARD_SEQ == boardInfo.BOARD_SEQ
                                 && content.DEPTH == 0
                                 && content.DEL_YN.Contains("N")
                           select new MainStatsModel
                           {
                               GroupCode = common.COMMON_CODE,
                               GroupName = common.CODE_NAME
                           }).AsQueryable().ToList();

            resultData = (from common in qnaCommonList
                          select new MainStatsModel
                          {
                              GroupCode = common.COMMON_CODE,
                              GroupName = common.CODE_NAME,
                              GroupCount = qnaList.Count(x => x.GroupCode.Contains(common.COMMON_CODE))
                          }).ToList();


            return resultData;
        }

        /// <summary>
        /// 고객제보 통계 정보를 가져온다.
        /// </summary>
        /// <returns></returns>
        public List<MainStatsModel> GetReportStats()
        {
            List<MainStatsModel> resultData = new List<MainStatsModel>();

            var menuSeqCode = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(x => x.COMMON_CODE.Contains(CommonCodeStatic.MAIN_REPORT_SEQ_CODE))?.CODE_VALUE1;

            if (menuSeqCode == null) return resultData;

            int menuSeq;
            if (!int.TryParse(menuSeqCode, out menuSeq)) return resultData;

            var boardInfo = (from ntbMenu in db49_wowtv.NTB_MENU
                             join ntbBoard in db49_wowtv.NTB_BOARD on ntbMenu.CONTENT_SEQ equals ntbBoard.BOARD_SEQ
                             where ntbMenu.MENU_SEQ == menuSeq
                             select ntbBoard).SingleOrDefault();

            if (boardInfo == null) return resultData;

            var qnaCommonList = db49_wowtv.NTB_COMMON_CODE.Where(x => x.UP_COMMON_CODE.Contains(CommonCodeStatic.MAIN_INQUIRY_CODE)).ToList();

            var qnaList = (from content in db49_wowtv.NTB_BOARD_CONTENT
                           join common in db49_wowtv.NTB_COMMON_CODE on content.UP_COMMON_CODE equals common.COMMON_CODE
                           where common.UP_COMMON_CODE == CommonCodeStatic.MAIN_INQUIRY_CODE
                                 && content.BOARD_SEQ == boardInfo.BOARD_SEQ
                                 && content.DEPTH == 0
                                 && content.DEL_YN.Contains("N")
                           select new MainStatsModel
                           {
                               GroupCode = common.COMMON_CODE,
                               GroupName = common.CODE_NAME
                           }).AsQueryable().ToList();

            resultData = (from common in qnaCommonList
                          select new MainStatsModel
                          {
                              GroupCode = common.COMMON_CODE,
                              GroupName = common.CODE_NAME,
                              GroupCount = qnaList.Count(x => x.GroupCode.Contains(common.COMMON_CODE))
                          }).ToList();


            return resultData;
        }

        public List<T_NEWS_PRG> GetProgramList(string ingYn)
        {
            try
            {

                List<T_NEWS_PRG> newProgramList;
                if (!string.IsNullOrWhiteSpace(ingYn)) //여긴 고객센터
                {
                    newProgramList = db90_DNRS.T_NEWS_PRG.AsNoTracking().Where(x =>
                            x.DELFLAG.Equals("0"))
                        .ToList();

                    foreach (var item in newProgramList)
                    {
                        item.ProgramList = db90_DNRS.TAB_PROGRAM_LIST.FirstOrDefault(a => a.PGM_ID == item.PRG_CD) ?? new TAB_PROGRAM_LIST();
                    }
                    newProgramList = newProgramList.Where(x => x.ProgramList.ING_YN != null && x.ProgramList.ING_YN.Equals(ingYn)).ToList();
                }
                else //마이페이지 속도 튜닝
                {
                    newProgramList = db90_DNRS.T_NEWS_PRG.AsNoTracking().Where(x => x.DELFLAG.Equals("0")).ToList();
                }

                return newProgramList;
            }
            catch (Exception e)
            {
                WowLog.Write(e.Message);

                throw;
            }


        }


        #region 댓글 CRUD
        public void CommentSave(NTB_BOARD_COMMENT model, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    model.DEL_YN = "N";
                    model.BLIND_YN = "N";
                    model.MOD_ID = loginUser.LoginId;
                    model.MOD_DATE = DateTime.Now;
                    model.REG_ID = loginUser.LoginId;
                    model.REG_DATE = DateTime.Now;
                    model.NICKNM = loginUser.UserName;
                    db49_wowtv.NTB_BOARD_COMMENT.Add(model);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public List<NTB_BOARD_COMMENT> GetCommentList(int seq)
        {
            var commentList = db49_wowtv.NTB_BOARD_COMMENT.AsNoTracking().Where(x => x.BOARD_CONTENT_SEQ.Equals(seq) && x.DEL_YN.Equals("N")).OrderByDescending(x=>x.REG_DATE).ToList();

            return commentList;
        }

        public ListModel<NTB_BOARD_COMMENT> GetCommentPaging(IntegratedBoardCondition condition)
        {
            var list = db49_wowtv.NTB_BOARD_COMMENT.AsNoTracking().AsQueryable();

            ListModel<NTB_BOARD_COMMENT> resultData = new ListModel<NTB_BOARD_COMMENT>();

            //기간 검색
            list = list.Where(x => x.BOARD_CONTENT_SEQ.Equals(condition.BOARD_CONTENT_SEQ) && x.DEL_YN.Equals("N"));


            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }

                list = list.OrderByDescending(x => x.REG_DATE).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }


            resultData.ListData = list.ToList();


       
            if (resultData.ListData.Count <= 0) return resultData;

            return resultData;
        }


        public void CommentDelete(int seq, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    var commentSingle = SingleBoardComment(seq);
                    if (commentSingle == null) return;

                    commentSingle.DEL_YN = "Y";
                    commentSingle.MOD_ID = loginUser.LoginId;
                    commentSingle.MOD_DATE = DateTime.Now;
                    db49_wowtv.NTB_BOARD_COMMENT.AddOrUpdate(commentSingle);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void CommentUpdate(int seq, string content, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    var commentSingle = SingleBoardComment(seq);
                    if (commentSingle == null) return;

                    commentSingle.CONTENT = content;
                    commentSingle.DEL_YN = "N";
                    commentSingle.MOD_ID = loginUser.LoginId;
                    commentSingle.MOD_DATE = DateTime.Now;
                    db49_wowtv.NTB_BOARD_COMMENT.AddOrUpdate(commentSingle);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }
        public NTB_BOARD_COMMENT SingleBoardComment(int seq)
        {
            return db49_wowtv.NTB_BOARD_COMMENT.FirstOrDefault(x => x.COMMENT_SEQ.Equals(seq));
        }
        #endregion


        public NTB_BOARD_CONTENT SingleBoardContent(int seq)
        {
            return db49_wowtv.NTB_BOARD_CONTENT.FirstOrDefault(x => x.BOARD_CONTENT_SEQ.Equals(seq));
        }

        public void UpdateContentBlind(int seq,string blind, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    var contentSingle = SingleBoardContent(seq);
                    if (contentSingle == null) return;

                    contentSingle.BLIND_YN = blind;
                    contentSingle.MOD_ID = loginUser.LoginId;
                    contentSingle.MOD_DATE = DateTime.Now;
                    db49_wowtv.NTB_BOARD_CONTENT.AddOrUpdate(contentSingle);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdateReplyBlind(int seq, string blind, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    var commentSingle = SingleBoardComment(seq);
                    if (commentSingle == null) return;

                    commentSingle.BLIND_YN = blind;
                    commentSingle.MOD_ID = loginUser.LoginId;
                    commentSingle.MOD_DATE = DateTime.Now;
                    db49_wowtv.NTB_BOARD_COMMENT.AddOrUpdate(commentSingle);
                    db49_wowtv.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }


        #region 시청자의견

        public ListModel<NTB_BOARD_CONTENT> GetFeedBackList(IntegratedBoardCondition condition)
        {

            //var feedBackSeqs = db49_wowtv.NTB_BOARD.AsNoTracking().AsQueryable().Where(x => x.BOARD_TYPE_CODE.Equals("FeedBack") && x.DEL_YN.Equals("N")).Select(x=> x.BOARD_SEQ).ToList();

            //var menuList = (from ntbMenu in db49_wowtv.NTB_MENU.AsNoTracking()
            //    join ntbBoard in db49_wowtv.NTB_BOARD.AsNoTracking() on ntbMenu.CONTENT_SEQ equals ntbBoard.BOARD_SEQ
            //    where ntbBoard.BOARD_TYPE_CODE.Equals("FeedBack")
            //       && ntbMenu.CHANNEL_CODE.Equals("BroadProgramAdminOrFront")
            //       && ntbMenu.FIX_YN.Equals("Y")
            //    select ntbMenu).ToList();

            //var list = (from menu in menuList
            //            join ntbBoardContent in db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking() on menu.CONTENT_SEQ equals ntbBoardContent.BOARD_SEQ
            //            where ntbBoardContent.DEL_YN.Contains("N")
            //              && (condition.LoginId == "" ||  ntbBoardContent.USER_ID.Equals(condition.LoginId))
            //            //ntbBoard.BOARD_TYPE_CODE.Equals("FeedBack")
            //            //      && ntbMenu.CHANNEL_CODE.Equals("BroadProgramAdminOrFront")
            //            //      && ntbMenu.FIX_YN.Equals("Y")
            //            select ntbBoardContent).AsQueryable();

            //var list = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().AsQueryable();

            var menuList = db49_wowtv.NTB_MENU.AsQueryable();
            menuList = menuList.Where(a => a.CHANNEL_CODE == "BroadProgramAdminOrFront");
            menuList = menuList.Where(a => a.FIX_YN == "Y");
            menuList = menuList.Where(a => a.ACTIVE_YN == "Y");
            menuList = menuList.Where(a => a.CONTENT_TYPE_CODE == "Board");
            menuList = menuList.Where(a => a.CONTENT_SEQ > 0);
            menuList = menuList.Where(a => db49_wowtv.NTB_BOARD.Any(b => b.BOARD_SEQ == a.CONTENT_SEQ && b.BOARD_TYPE_CODE == "FeedBack"));
            List<int> menuSeqList = menuList.Select(a => a.CONTENT_SEQ.Value).ToList();

            var list = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().Where(a => a.DEL_YN == "N" && menuSeqList.Contains(a.BOARD_SEQ) );


            if (!string.IsNullOrEmpty(condition.COMMON_CODE))
            {
                list = list.Where(a => a.COMMON_CODE == condition.COMMON_CODE);
            }
            if (!string.IsNullOrEmpty(condition.LoginId))
            {
                list = list.Where(a => a.MOD_ID.Equals(condition.LoginId));
            }

            if (!string.IsNullOrEmpty(condition.SearchText))
            {
                if (condition.SearchType.Equals("TITLE"))
                {
                    list = list.Where(a => a.TITLE.Contains(condition.SearchText));
                }
                else if (condition.SearchType.Equals("CONTENT"))
                {
                    list = list.Where(a => a.CONTENT.Contains(condition.SearchText));
                }
                else
                {
                    list = list.Where(a => a.TITLE.Contains(condition.SearchText) || a.CONTENT.Contains(condition.SearchText));
                }

            }

            ListModel<NTB_BOARD_CONTENT> resultData = new ListModel<NTB_BOARD_CONTENT>();
            
            //기간 검색
            //list = list.Where(x => x.BOARD_CONTENT_SEQ && x.DEL_YN.Equals("N"));


            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }

                list = list.OrderByDescending(x => x.REG_DATE).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }


            resultData.ListData = list.ToList();

            

            foreach (var item in resultData.ListData)
            {
                if (!string.IsNullOrWhiteSpace(item.COMMON_CODE))
                {
                    var newProgramList = db90_DNRS.T_NEWS_PRG.AsNoTracking().Where(a => a.PRG_CD.Equals(item.COMMON_CODE) && a.DELFLAG.Equals("0"));
                    var programName = newProgramList.FirstOrDefault();
                    item.CommonName = (programName == null ? "" : programName.PRG_NM);
                }

                var comment = db49_wowtv.NTB_BOARD_COMMENT.AsNoTracking().Where(x => x.BOARD_CONTENT_SEQ.Equals(item.BOARD_CONTENT_SEQ) && x.DEL_YN.Equals("N")).ToList();
                item.CommentList = comment;
            }

            return resultData;
        }

        public NTB_MENU GetHelpPageToFeedBackSeq(string menuName)
        {
            return db49_wowtv.NTB_MENU.AsNoTracking().SingleOrDefault(x => x.CHANNEL_CODE.Equals("HELP") && x.MENU_NAME.Contains(menuName) && x.UP_MENU_SEQ != 0 && x.DEPTH == 2 && x.ACTIVE_YN.Equals("Y") && x.CONTENT_SEQ != 0 
            && x.DEL_YN.Equals("N"));
        }

        #endregion

        #region 고객센터-시청자의견

        public NTB_MENU GetFeedBackProgamCodeToBoardSeq(string programCode)
        {

            var boardInfo = (from ntbMenu in db49_wowtv.NTB_MENU.AsNoTracking()
                            join ntbBoard in db49_wowtv.NTB_BOARD.AsNoTracking() on ntbMenu.CONTENT_SEQ equals ntbBoard.BOARD_SEQ
                            where ntbBoard.BOARD_TYPE_CODE.Equals("FeedBack")
                                  && ntbMenu.CHANNEL_CODE.Equals("BroadProgramAdminOrFront")
                                  && ntbMenu.CONTENT_TYPE_CODE.Equals("Board")
                               && ntbMenu.FIX_YN.Equals("Y")
                               && ntbMenu.PRG_CD.Equals(programCode)
                                  && ntbMenu.CONTENT_SEQ > 0
                             select ntbMenu).SingleOrDefault();
            
            return boardInfo;
        }

        /// <summary>
        /// 고객센터 시청자 의견 게시물을 저장한다.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        public int BoardFeedBackSave(NTB_BOARD_CONTENT model, LoginUser loginUser)
        {

            ////
            ////고객센터는 따로 프로그램 코드로 Board 번호를 가져온다
            //NTB_MENU menuInfo = integratedBoard.GetFeedBackProgamCodeToBoardSeq(board.COMMON_CODE);
            //board.BOARD_SEQ = menuInfo.CONTENT_SEQ.Value;



            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                try
                {
                    NTB_BOARD_CONTENT boardSingle = GetBoardDetail(model.BOARD_CONTENT_SEQ);
                    if (boardSingle == null) //신규등록
                    {
                        
                        model.DEL_YN = "N";
                        model.BLIND_YN = "N";
                        model.REPLY_YN = "N";
                        model.REG_DATE = DateTime.Now;
                        model.REG_ID = loginUser.LoginId;
                        model.USER_ID = loginUser.LoginId;
                        model.USER_NAME = loginUser.UserName;
                        model.MOD_ID = loginUser.LoginId;
                        model.MOD_DATE = DateTime.Now;
                        db49_wowtv.NTB_BOARD_CONTENT.Add(model);
                        db49_wowtv.SaveChanges();

                    }
                    else
                    {
                        //코멘트 업데이트
                        //이전 게시판 번호와 현재 게시판번호가 변경 되었을 경우
                        if (boardSingle.BOARD_SEQ != model.BOARD_SEQ) 
                        {
                            CommentUpdateAll(boardSingle.BOARD_SEQ, model.BOARD_SEQ, loginUser);
                        }
                        
                        boardSingle.BOARD_SEQ = model.BOARD_SEQ;
                        //boardSingle.VIEW_YN = model.VIEW_YN;
                        boardSingle.NOTICE_YN = model.NOTICE_YN;
                        if (model.START_DATE != null && model.START_DATE.Value.Year > 1900)
                            boardSingle.START_DATE = model.START_DATE;
                        if (model.END_DATE != null && model.END_DATE.Value.Year > 1900)
                            boardSingle.END_DATE = model.END_DATE;
                        if (model.CONTENT != null && model.TITLE != null) //1:1문의에서 답글 없을 때 처리 안되도록
                        {
                            boardSingle.CONTENT = model.CONTENT;
                            boardSingle.TITLE = model.TITLE;
                        }
                        boardSingle.USER_NAME = loginUser.UserName;
                        boardSingle.DEL_YN = model.DEL_YN;
                        boardSingle.COMMON_CODE = model.COMMON_CODE;
                        boardSingle.UP_COMMON_CODE = model.UP_COMMON_CODE;
                        boardSingle.PAGE_LINK = model.PAGE_LINK;
                        boardSingle.MOD_ID = loginUser.LoginId;
                        boardSingle.MOD_DATE = DateTime.Now;
                        db49_wowtv.NTB_BOARD_CONTENT.AddOrUpdate(boardSingle);
                        db49_wowtv.SaveChanges();
                    }
                    dbContextTransaction.Commit();

                    return model.BOARD_CONTENT_SEQ;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        private void CommentUpdateAll(int beforeBoardContentSeq, int afterBoardContentSeq, LoginUser loginUser)
        {

            var commentList = db49_wowtv.NTB_BOARD_COMMENT.Where(x => x.BOARD_CONTENT_SEQ.Equals(beforeBoardContentSeq)).ToList();
            foreach (var item in commentList)
            {
                item.BOARD_CONTENT_SEQ = afterBoardContentSeq;
                item.MOD_ID = loginUser.LoginId;
                item.MOD_DATE = DateTime.Now;
                db49_wowtv.NTB_BOARD_COMMENT.AddOrUpdate(item);
                db49_wowtv.SaveChanges();

            }
        }

        /// <summary>
        /// 내 프로그램에서 시청자 의견 게시판이 생성 된 정보만 가져온다.
        /// </summary>
        /// <param name="ingYn"></param>
        /// <returns></returns>
        public List<TAB_PROGRAM_LIST> GetCreateBoardProgramList(string ingYn)
        {

            var createBoardList = (from ntbMenu in db49_wowtv.NTB_MENU.AsNoTracking()
                join ntbBoard in db49_wowtv.NTB_BOARD.AsNoTracking() on ntbMenu.CONTENT_SEQ equals ntbBoard.BOARD_SEQ
                where ntbBoard.BOARD_TYPE_CODE.Equals("FeedBack")
                      && ntbMenu.CHANNEL_CODE.Equals("BroadProgramAdminOrFront")
                      && ntbMenu.CONTENT_TYPE_CODE.Equals("Board")
                      && ntbMenu.FIX_YN.Equals("Y")
                      && ntbMenu.CONTENT_SEQ >0
                      && ntbMenu.ACTIVE_YN.Equals("Y")
                      && ntbBoard.DEL_YN.Equals("N")
                                   select ntbMenu).ToList();

            

            List<TAB_PROGRAM_LIST> programList = new List<TAB_PROGRAM_LIST>();

            foreach (var item in createBoardList)
            {
                var program = string.IsNullOrWhiteSpace(ingYn)
                    ? db90_DNRS.TAB_PROGRAM_LIST.FirstOrDefault(a => a.PGM_ID == item.PRG_CD)
                    : db90_DNRS.TAB_PROGRAM_LIST.FirstOrDefault(a => a.PGM_ID == item.PRG_CD && a.ING_YN.Equals(ingYn));
                if (program!=null)
                    programList.Add(program);
            }

            return programList;
        }
        #endregion


        #region 메인 화면

        /// <summary>
        /// 메인화면 나의 1:1문의
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IntegratedBoard<NTB_BOARD_CONTENT> MyMainIntegratedSearchCount(IntegratedBoardCondition condition)
        {
            IntegratedBoard<NTB_BOARD_CONTENT> resultData = new IntegratedBoard<NTB_BOARD_CONTENT>();

            var list = db49_wowtv.NTB_BOARD_CONTENT.AsQueryable();

            var boardInfo = (from ntbMenu in db49_wowtv.NTB_MENU.AsNoTracking()
                             join ntbBoard in db49_wowtv.NTB_BOARD.AsNoTracking() on ntbMenu.CONTENT_SEQ equals ntbBoard.BOARD_SEQ
                             where ntbMenu.MENU_SEQ == condition.CurrentMenuSeq
                             select ntbBoard).SingleOrDefault();

            if (boardInfo != null)
            {
                resultData.BoardInfo = boardInfo.Clone() as NTB_BOARD;
                if (resultData.BoardInfo != null)
                {
                    list = list.Where(a => a.BOARD_SEQ.Equals(resultData.BoardInfo.BOARD_SEQ));
                    if (resultData.BoardInfo.REPLY_YN == "Y")
                    {
                        list = list.Where(a => a.UP_BOARD_CONTENT_SEQ.Equals(0));//답변 아닌것만 가져온다.
                    }
                }

                //삭제여부
                if (!string.IsNullOrEmpty(condition.DEL_YN) && !condition.DEL_YN.Equals("ALL"))
                {
                    list = list.Where(a => a.DEL_YN.Contains(condition.DEL_YN));
                }
                else
                {
                    list = list.Where(a => a.DEL_YN == "N");
                }

                //게시여부
                if (!string.IsNullOrEmpty(condition.VIEW_YN) && !condition.VIEW_YN.Equals("ALL"))
                {
                    list = list.Where(a => a.VIEW_YN.Contains(condition.VIEW_YN));
                }

                if (!string.IsNullOrEmpty(condition.UP_COMMON_CODE) && !condition.UP_COMMON_CODE.Equals("ALL"))
                {
                    list = list.Where(a => a.UP_COMMON_CODE.Contains(condition.UP_COMMON_CODE));
                }

                if (!string.IsNullOrEmpty(condition.COMMON_CODE) && !condition.COMMON_CODE.Equals("ALL"))
                {
                    list = list.Where(a => a.COMMON_CODE.Contains(condition.COMMON_CODE));
                }

                list = list.Where(a => a.USER_ID.Contains(condition.LoginId));

                //데이터 조회
                resultData.ListData = list.ToList();

                IEnumerable<NTB_BOARD_CONTENT> ntbBoardContents = resultData.ListData;

                //답변 여부
                if (!string.IsNullOrWhiteSpace(condition.REPLY_YN) && !condition.REPLY_YN.Equals("ALL"))
                {
                    ntbBoardContents = ntbBoardContents.Where(a => a.REPLY_YN.Equals(condition.REPLY_YN));
                }
                
                resultData.TotalDataCount = ntbBoardContents.Count();

                if (resultData.BoardInfo?.REPLY_YN == "Y") //답변여부
                {
                    var replyList = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().Where(x => x.BOARD_SEQ == boardInfo.BOARD_SEQ && x.UP_BOARD_CONTENT_SEQ > 0 && x.DEL_YN.Equals("N")).ToList().Clone();

                    foreach (var item in ntbBoardContents)
                    {
                        var reply = replyList.Where(x => x.UP_BOARD_CONTENT_SEQ.Equals(item.BOARD_CONTENT_SEQ)).ToList();
                        item.ReplyList = reply;
                    }
                }
            }
            //순환 참조 제거
            foreach (var item in resultData.ListData)
            {
                item.NTB_BOARD = null;
                item.NTB_BOARD_COMMENT = null;
            }
            return resultData;
        }

        public List<NTB_BOARD_CONTENT> GetFAQList(int menuSeq)
        {
            IntegratedBoard<NTB_BOARD_CONTENT> resultData = new IntegratedBoard<NTB_BOARD_CONTENT>();

            var list = db49_wowtv.NTB_BOARD_CONTENT.AsQueryable();

            var boardInfo = (from ntbMenu in db49_wowtv.NTB_MENU.AsNoTracking()
                             join ntbBoard in db49_wowtv.NTB_BOARD.AsNoTracking() on ntbMenu.CONTENT_SEQ equals ntbBoard.BOARD_SEQ
                             where ntbMenu.MENU_SEQ == menuSeq
                             select ntbBoard).SingleOrDefault();

            if (boardInfo != null)
            {
                //마이페이지-FAQ목록 (MyPageFAQ) 공통코드로 관리
                //var faqList = db49_wowtv.NTB_COMMON_CODE.Where(x => x.UP_COMMON_CODE.Equals("035000000")).ToList();
                var faqList = (from ntbBoard in db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking()
                    join commonCode in db49_wowtv.NTB_COMMON_CODE.AsNoTracking() on
                        ntbBoard.BOARD_CONTENT_SEQ.ToString() equals commonCode.CODE_NAME
                    where commonCode.UP_COMMON_CODE.Equals("035000000")
                    select ntbBoard);
                //삭제여부
                faqList = faqList.Where(a => a.DEL_YN == "N");
                faqList = faqList.Where(a => a.VIEW_YN.Contains("Y"));
                
                //데이터 조회
                resultData.ListData = faqList.ToList();

            }
            //순환 참조 제거
            foreach (var item in resultData.ListData)
            {
                item.NTB_BOARD = null;
                item.NTB_BOARD_COMMENT = null;
            }
            return resultData.ListData;

        }

        public NTB_BOARD_CONTENT GetMainNoticeContent()
        {
            MenuBiz menuBiz = new MenuBiz();
            var menuInfo = menuBiz.GetAtByName("HELP", "공지사항");


            if (menuInfo == null)
            {
                return null;
            }

            var firstNotice = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().AsQueryable();
            var dateLast = DateTime.Now.AddDays(-2);

            firstNotice = firstNotice.Where(x => x.BOARD_SEQ.Equals(menuInfo.CONTENT_SEQ.Value) && x.DEL_YN.Equals("N"));
            firstNotice = firstNotice.Where(x => x.VIEW_YN.Equals("Y"));
           // singleNotice = singleNotice.Where(x => x.BLIND_YN.Equals("N"));
            firstNotice = firstNotice.Where(x => x.REG_DATE >= dateLast);
            firstNotice = firstNotice.OrderByDescending(x => x.REG_DATE);

            return firstNotice.FirstOrDefault(); 
        }

        #endregion


        #region 고객센터 메인에 Top4 공지사항 가져오기
         public List<NTB_BOARD_CONTENT> GetHelpMainNotice()
        {
            MenuBiz menuBiz = new MenuBiz();
            var menuInfo = menuBiz.GetAtByName("HELP", "공지사항");


            if (menuInfo == null)
            {
                return null;
            }

            var top4Notice = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().AsQueryable();

            top4Notice = top4Notice.Where(x => x.BOARD_SEQ.Equals(menuInfo.CONTENT_SEQ.Value) && x.DEL_YN.Equals("N"));
            //top4Notice = top4Notice.Where(x => x.NOTICE_YN.Equals("Y"));
            top4Notice = top4Notice.Where(x => x.VIEW_YN.Equals("Y"));
            top4Notice = top4Notice.OrderByDescending(x => x.REG_DATE);
            top4Notice = top4Notice.Take(4);

            var list = top4Notice.ToList();


            for (var i = 0;i < list.Count() ;i++)
            {
                var top4NoticeCode = list[i].COMMON_CODE;
                var commonCode = db49_wowtv.NTB_COMMON_CODE.AsNoTracking().SingleOrDefault(x => x.COMMON_CODE.Equals(top4NoticeCode));
                if (commonCode != null) list[i].CommonName = commonCode.CODE_NAME;
            }

            return list.ToList();
        }
        #endregion

        public List<NTB_BOARD_CONTENT> GetQuickNoticeList()
        {
            MenuBiz menuBiz = new MenuBiz();
            var menuInfo = menuBiz.GetAtByName("HELP", "공지사항");


            if (menuInfo == null)
            {
                return null;
            }

            var top4Notice = db49_wowtv.NTB_BOARD_CONTENT.AsNoTracking().AsQueryable();

            top4Notice = top4Notice.Where(x => x.BOARD_SEQ.Equals(menuInfo.CONTENT_SEQ.Value) && x.DEL_YN.Equals("N"));
            //top4Notice = top4Notice.Where(x => x.NOTICE_YN.Equals("Y"));
            top4Notice = top4Notice.Where(x => x.VIEW_YN.Equals("Y"));
            top4Notice = top4Notice.OrderByDescending(x => x.REG_DATE);
            top4Notice = top4Notice.Take(1);

            var list = top4Notice.ToList();


            for (var i = 0; i < list.Count(); i++)
            {
                var top4NoticeCode = list[i].COMMON_CODE;
                var commonCode = db49_wowtv.NTB_COMMON_CODE.AsNoTracking().SingleOrDefault(x => x.COMMON_CODE.Equals(top4NoticeCode));
                if (commonCode != null) list[i].CommonName = commonCode.CODE_NAME;
            }

            return list.ToList();
        }

        public void EmailBusinessInqury(string commonCode)
        {
            //var arrayAdminEmail = ConfigurationManager.AppSettings["BusinessAdminEmail"].Split(',');
            var codeName = db49_wowtv.NTB_COMMON_CODE.SingleOrDefault(x => x.COMMON_CODE.Equals(commonCode)).CODE_NAME;
            var arrayAdminEmail = db49_wowtv.NTB_COMMON_CODE.Where(x => 
                                        x.UP_COMMON_CODE.Equals(db49_wowtv.NTB_COMMON_CODE.FirstOrDefault(a => a.CODE_NAME.Equals(commonCode)).COMMON_CODE) 
                                        && x.DEL_YN.Equals("N") && x.ACTIVE_YN.Equals("Y"))
                                    .Select(x => x.CODE_NAME)
                                    .ToArray();

            var emailContents = "\"한국경제TV 회사소개 > 사업문의\"를 통해<br>\"" + codeName + "\" 관련한 고객/고객사의 문의가 접수되었습니다.<br><br>";
            emailContents += "메일 받으신 관련 담당자분께서는 한국경제TV 웹관리자 사이트로 접속하시어 내용을 확인하시기 바랍니다.<br><br>";
            emailContents += "O 바로 확인하기 : (웹관리자>회사소개-사이트>게시물 관리>고객문의) <br>";
            emailContents += "ㄴ <a href=\"http://admin.wowtv.co.kr/IntegratedBoard/Inquiry/Index?menuSeq=131\" target=\"_blank\">";
            emailContents += "http://admin.wowtv.co.kr/IntegratedBoard/Inquiry/Index?menuSeq=131</a><br><br>";
            emailContents += "O 웹에 게시된 사업내용 보기 <br>";
            emailContents += "ㄴ <a href=\"http://company.wowtv.co.kr/BusinessGuide/Business/Index?menuSeq=153\" target=\"_blank\">";
            emailContents += "http://company.wowtv.co.kr/BusinessGuide/Business/Index?menuSeq=153</a><br><br>";
            emailContents += "※ 본 메일은 관련 담당자분에게 자동 발송되는 메일입니다.<br>";
            emailContents += "※ 기타 이메일 및 사업내용 업데이트 문의는 -한국경제TV 사이트 운영자에게 해주시기 바랍니다.<br><br>";
            emailContents += "감사합니다.";
            

            if (arrayAdminEmail.Count() > 0)
            {
                foreach (var item in arrayAdminEmail)
                {
                    new EmailBiz().Send(new EmailSendParameter()
                    {
                        //EmailCode = EmailCodeModel.SendBusinessAdmin,
                        EmailCode = EmailCodeModel.SendCDCompletedAlarm,
                        FromName = "한국경제TV",
                        FromEmail = "webmaster@wowtv.co.kr",
                        ToName = "",
                        ToEmail = item,
                        Subject = "[안내] 한국경제TV -온라인을 통해 고객문의가 접수되었습니다.",
                        Contents = emailContents
                    });
                }
            }
        }
    }
}