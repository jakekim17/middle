using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Model.Db49.wowtv.AttachFile;

using Wow.Tv.Middle.Biz.AttachFile;

namespace Wow.Tv.Middle.Biz.Board
{
    public class BoardContentBiz : BaseBiz
    {
        const string TABLE_CODE = "NTB_BOARD_CONTENT";

        public ListModel<NTB_BOARD_CONTENT> SearchList(BoardContentCondition condition)
        {
            ListModel<NTB_BOARD_CONTENT> resultData = new ListModel<NTB_BOARD_CONTENT>();

            var list = db49_wowtv.NTB_BOARD_CONTENT.Where(a => a.DEL_YN == "N");

            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                if (condition.SearchType == "Title")
                {
                    list = list.Where(a => a.TITLE.Contains(condition.SearchText) == true);
                }
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.SORD_ORDER);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();


            return resultData;
        }



        public NTB_BOARD_CONTENT GetAt(int boardContentSeq)
        {
            return db49_wowtv.NTB_BOARD_CONTENT.SingleOrDefault(a => a.BOARD_CONTENT_SEQ == boardContentSeq);
        }




        public void Save(NTB_BOARD_CONTENT model, LoginUser loginUser)
        {
            NTB_BOARD_CONTENT data = GetAt(model.BOARD_CONTENT_SEQ);
            if (data == null)
            {
                if (model.DEPTH == 0)
                {
                    model.SORD_ORDER = (db49_wowtv.NTB_BOARD_CONTENT.Count() + 1) * 1000;
                }
                else
                {
                    if (model.TOP_BOARD_CONTENT_SEQ == 0)
                    {
                        throw new Exception("하위게시글은 최상위 게시글 번호가 있어야 합니다.");
                    }
                    if (model.UP_BOARD_CONTENT_SEQ == 0)
                    {
                        throw new Exception("하위게시글은 상위 게시글 번호가 있어야 합니다.");
                    }

                    // 내 차상위에 대한 정보 조회
                    var up = db49_wowtv.NTB_BOARD_CONTENT.SingleOrDefault(a => a.BOARD_CONTENT_SEQ == model.UP_BOARD_CONTENT_SEQ);

                    // 순번밀려야 하는목록 조회 (최상위가 같고 차상위의 하위들)
                    var list = db49_wowtv.NTB_BOARD_CONTENT.Where(a => a.TOP_BOARD_CONTENT_SEQ == model.TOP_BOARD_CONTENT_SEQ);
                    list = list.Where(a => a.SORD_ORDER < up.SORD_ORDER);
                    foreach(var item in list)
                    {
                        item.SORD_ORDER = item.SORD_ORDER - 1;
                    }
                    model.SORD_ORDER = up.SORD_ORDER - 1;
                }
                model.DEL_YN = "N";
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;
                db49_wowtv.NTB_BOARD_CONTENT.Add(model);
            }
            else
            {
                //data.BOARD_SEQ = model.BOARD_SEQ;
                //data.TOP_BOARD_CONTENT_SEQ = model.TOP_BOARD_CONTENT_SEQ;
                //data.SORD_ORDER = model.SORD_ORDER;
                //data.DEPTH = model.DEPTH;
                data.TITLE = model.TITLE;
                data.CONTENT = model.CONTENT;
                data.NOTICE_YN = model.NOTICE_YN;
                data.KEYWORD = model.KEYWORD;
                data.PASSWORD = model.PASSWORD;

                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();


            // 첨부파일처리
            AttachFileBiz attachFileBiz = new AttachFileBiz();
            foreach(var item in model.AttachFileList)
            {
                item.TABLE_CODE = TABLE_CODE;
                item.TABLE_KEY = model.BOARD_CONTENT_SEQ.ToString();

                attachFileBiz.Create(item);
            }
        }


        public void Delete(int boardContentSeq, LoginUser loginUser)
        {
            NTB_BOARD_CONTENT data = GetAt(boardContentSeq);


            // 첨부파일처리
            AttachFileBiz attachFileBiz = new AttachFileBiz();
            AttachFileCondition attachFileCondition = new AttachFileCondition();
            attachFileCondition.PageSize = -1;
            attachFileCondition.TableCode = TABLE_CODE;
            attachFileCondition.TableKey = data.BOARD_CONTENT_SEQ.ToString();
            var attachFileList = attachFileBiz.SearchList(attachFileCondition);
            foreach (var item in attachFileList.ListData)
            {
                attachFileBiz.Delete(item.ATTACH_FILE_SEQ);
            }


            if (data != null)
            {
                data.DEL_YN = "Y";
                db49_wowtv.SaveChanges();
            }
        }





        public void AttachFileDelete(int attachFileSeq)
        {
            new AttachFileBiz().Delete(attachFileSeq);

        }









        /// <summary>
        /// 어드민 홈에서 사용하는 시청자의견 목록 조회
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<NTB_BOARD_CONTENT> GetBroadBoard(BoardContentCondition condition)
        {
            ListModel<NTB_BOARD_CONTENT> model = new ListModel<NTB_BOARD_CONTENT>();

            var list = db49_wowtv.NTB_BOARD_CONTENT.AsQueryable();
            list = list.Where(a =>
                db49_wowtv.NTB_MENU.Where(b => b.MENU_NAME == "시청자참여" && b.FIX_YN == "Y" && b.CONTENT_SEQ == a.BOARD_SEQ).Count() > 0);

            list = list.OrderByDescending(a => a.REG_DATE);

            list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            model.ListData = list.ToList();

            foreach(var item in model.ListData)
            {
                //var menu = db49_wowtv.NTB_MENU.Where(a => a.CONTENT_SEQ == item.BOARD_SEQ).OrderByDescending(a => a.MENU_SEQ).FirstOrDefault();
				var menu = db49_wowtv.NTB_MENU.Where(a => a.CONTENT_SEQ == item.BOARD_SEQ && a.PRG_CD == item.COMMON_CODE).OrderByDescending(a => a.MENU_SEQ).FirstOrDefault();
				if (menu != null)
                {
                    item.MenuSeq = menu.MENU_SEQ;
                    item.ProgramCode = menu.PRG_CD;
                    var newsProgram = db90_DNRS.T_NEWS_PRG.SingleOrDefault(a => a.PRG_CD == item.ProgramCode);
                    if (newsProgram != null)
                    {
                        item.ProgramName = newsProgram.PRG_NM;
                    }
                }
            }

            return model;
        }


    }


}
