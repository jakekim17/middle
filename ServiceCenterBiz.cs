using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv.ServiceCenter;
using Wow.Tv.Middle.Model.Db49.wowtv;

namespace Wow.Tv.Middle.Biz.Admin
{

    /// <summary>
    /// <para>  고객센터 Biz</para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-08-03</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-08-08</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-08-08 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class ServiceCenterBiz : BaseBiz
    {
        #region 공지사항
        /// <summary>
        /// 상위에 표시할 List
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public List<TAB_BOARD_TOP> GetTopList(string codes)
        {
            var topList = db49_wowtv.TAB_BOARD_TOP.Where(x => x.VIEW_FLAG == "Y" && x.CODES.Contains(codes)).ToList();

            return topList;
        }

        /// <summary>
        /// 검색 된 List
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<TAB_NOTICE> NolticeSearchList(NoticeCondition condition)
        {
            ListModel<TAB_NOTICE> resultData = new ListModel<TAB_NOTICE>();


            var list = db49_wowtv.TAB_NOTICE.AsQueryable();

            if (!string.IsNullOrEmpty(condition.Bcode))
            {
                list = list.Where(a => a.BCODE.Contains(condition.Bcode));
            }

            //게시여부
            if (!string.IsNullOrEmpty(condition.ViewFlag) && !condition.ViewFlag.Equals("ALL"))
            {
                list = list.Where(a => a.VIEW_FLAG.Contains(condition.ViewFlag));
            }

            ////삭제여부
            //if (!string.IsNullOrEmpty(condition.DelFlag) && !condition.DelFlag.Equals("ALL"))
            //{
            //    list = list.Where(a => a.DEL_YN.Contains(condition.DelFlag));
            //}
            //else
            //{
            //    list = list.Where(a => a.DEL_YN == "N");
            //}
            //검색
            if (!string.IsNullOrEmpty(condition.SearchName))
            {
                if (condition.SearchFlag.Equals("TITLE"))
                {
                    list = list.Where(a => a.TITLE.Contains(condition.SearchName));
                }
                else if (condition.SearchFlag.Equals("CONTENT"))
                {
                    list = list.Where(a => a.CONTENT.Contains(condition.SearchName));
                }
                else
                {
                    list = list.Where(a => a.TITLE.Contains(condition.SearchName) || a.CONTENT.Contains(condition.SearchName));
                }

            }

            if (!string.IsNullOrEmpty(condition.TopFlag) && !condition.TopFlag.Equals("ALL"))
            {
                ///TODO : 아직 TOP에 대한 정의 필요..8월 16일날 확인 예정
                //list = list.Where(a => a.TOP_FLAG.Contains(condition.TOP_FLAG) == true);
            }


            resultData.TotalDataCount = list.Count();
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            return resultData;
        }

        /// <summary>
        /// 선택 된 게시물 가져온다.
        /// </summary>
        /// <param name="boardSeq"></param>
        /// <returns></returns>
        public TAB_NOTICE GetNoticeSingle(int boardSeq)
        {
            return db49_wowtv.TAB_NOTICE.SingleOrDefault(a => a.SEQ == boardSeq);
        }

        ///// <summary>
        ///// /// 선택 된 Log 게시물 가져온다.
        ///// </summary>
        ///// <param name="db"></param>
        ///// <param name="seq"></param>
        ///// <returns></returns>
        //private TAB_NOTICE_LOG_49_NET GetNoticeLogSingle(int seq)
        //{
        //    using (Db49_wownet db = new Db49_wownet())
        //    {
        //        return db.TAB_NOTICE_LOG_49_NET.SingleOrDefault(a => a.SEQ == seq);
        //    }
        //}

        /// <summary>
        /// 게시물을 저장한다.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        public void NoticeSave(TAB_NOTICE model, LoginUser loginUser)
        {
          
                TAB_NOTICE noticeSingle = GetNoticeSingle(model.SEQ); 
                if (noticeSingle == null)
                {
                    model.REG_DATE = DateTime.Now;
                    model.USER_ID = loginUser.LoginId;
                    model.USER_NAME = loginUser.UserName;
                    db49_wowtv.TAB_NOTICE.Add(model);
                    //Context가 유지가 되어야지 업데이트가 됨.
                    //db49_wowtv.Entry(model).State = EntityState.Added;
                }
                else
                {
                    //TODO : 상단공지는 각 사이트별 최대 5개까지 지정한다.(미구현)

                    noticeSingle.VIEW_FLAG = model.VIEW_FLAG;
                    noticeSingle.CONTENT = model.CONTENT;
                    noticeSingle.TITLE = model.TITLE;
                //noticeSingle.VIEW_FLAG = model.VIEW_FLAG;
                    noticeSingle.USER_ID = loginUser.LoginId;
                    noticeSingle.USER_NAME = loginUser.UserName;

                    //Context가 유지가 되어야지 업데이트가 됨.
                    //db49_wowtv.Entry(noticeSingle).State = EntityState.Modified;
                }
            //db49_wowtv.Database.Log = s => Debug.WriteLine(s);

            db49_wowtv.SaveChanges();
        }

        /// <summary>
        /// 게시물을 삭제한다.
        /// </summary>
        /// <param name="seq"></param>
        public void NoticeDelete(int seq, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                TAB_NOTICE noticeSingle = GetNoticeSingle(seq);
                // TAB_NOTICE_LOG_49_NET noticeLogSingle = GetNoticeLogSingle(seq);

                if (noticeSingle != null)
                {
                    try
                    {
                        //  db.TAB_NOTICE_LOG_49_NET.Remove(noticeLogSingle);
                        db49_wowtv.TAB_NOTICE.Remove(noticeSingle);
                        //noticeSingle.USER_ID = loginUser.LoginId;
                        //noticeSingle.USER_NAME = loginUser.UserName;
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
        }
        #endregion

        #region FAQ
        /// <summary>
        /// 검색 된 List
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<TAB_FAQ> FAQSearchList(FAQCondition condition)
        {
            ListModel<TAB_FAQ> resultData = new ListModel<TAB_FAQ>();


            var list = db49_wowtv.TAB_FAQ.AsQueryable();

            if (!string.IsNullOrEmpty(condition.Bcode))
            {
                list = list.Where(a => a.BCODE.Contains(condition.Bcode));
            }

            //게시여부
            if (!string.IsNullOrEmpty(condition.VIEW_YN) && !condition.VIEW_YN.Equals("ALL"))
            {
                list = list.Where(a => a.VIEW_YN.Contains(condition.VIEW_YN));
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
            //검색
            if (!string.IsNullOrEmpty(condition.SearchName))
            {
                if (condition.SearchFlag.Equals("TITLE"))
                {
                    list = list.Where(a => a.TITLE.Contains(condition.SearchName));
                }
                else if (condition.SearchFlag.Equals("CONTENT"))
                {
                    list = list.Where(a => a.CONTENT.Contains(condition.SearchName));
                }
                else
                {
                    list = list.Where(a => a.TITLE.Contains(condition.SearchName) || a.CONTENT.Contains(condition.SearchName));
                }

            }

            resultData.TotalDataCount = list.Count();
            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            return resultData;
        }

        /// <summary>
        /// 선택 된 게시물 가져온다.
        /// </summary>
        /// <param name="boardSeq"></param>
        /// <returns></returns>
        public TAB_FAQ GetFAQSingle(int boardSeq)
        {
            return db49_wowtv.TAB_FAQ.SingleOrDefault(a => a.SEQ == boardSeq);
        }

        ///// <summary>
        ///// /// 선택 된 Log 게시물 가져온다.
        ///// </summary>
        ///// <param name="db"></param>
        ///// <param name="seq"></param>
        ///// <returns></returns>
        //private TAB_NOTICE_LOG_49_NET GetNoticeLogSingle(int seq)
        //{
        //    using (Db49_wownet db = new Db49_wownet())
        //    {
        //        return db.TAB_NOTICE_LOG_49_NET.SingleOrDefault(a => a.SEQ == seq);
        //    }
        //}

        /// <summary>`
        /// 게시물을 저장한다.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        public void FAQSave(TAB_FAQ model, LoginUser loginUser)
        {

            TAB_FAQ faqSingle = GetFAQSingle(model.SEQ);
            if (faqSingle == null)
            {
                model.REG_DATE = DateTime.Now;
                model.USER_ID = loginUser.LoginId;
                model.USER_NAME = loginUser.UserName;
                model.DEL_YN = "N";
                db49_wowtv.TAB_FAQ.Add(model);
            }
            else
            {
                faqSingle.VIEW_YN = model.VIEW_YN;
                faqSingle.CONTENT = model.CONTENT;
                faqSingle.TITLE = model.TITLE;
                faqSingle.DEL_YN = model.DEL_YN;
                faqSingle.USER_ID = loginUser.LoginId;
                faqSingle.USER_NAME = loginUser.UserName;
            }
            //db49_wowtv.Database.Log = s => Debug.WriteLine(s);

            db49_wowtv.SaveChanges();
        }

        /// <summary>
        /// 게시물을 삭제한다.
        /// </summary>
        /// <param name="seq"></param>
        public void FAQDelete(int seq, LoginUser loginUser)
        {
            using (var dbContextTransaction = db49_wowtv.Database.BeginTransaction())
            {
                TAB_FAQ faqSingle = GetFAQSingle(seq);

                if (faqSingle != null)
                {
                    try
                    {
                        //Del_YN 으로 처리 Y는 삭제 N은 정상
                        faqSingle.DEL_YN = "Y";
                        faqSingle.USER_ID = loginUser.LoginId;
                        faqSingle.USER_NAME = loginUser.UserName;
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
        }
        #endregion

    }
}
