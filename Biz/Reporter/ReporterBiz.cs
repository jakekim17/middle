using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Biz.Component;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.Reporter;

namespace Wow.Tv.Middle.Biz.Reporter
{
    public class ReporterBiz : BaseBiz
    {

        /// <summary>
        /// 기자 리스트
        /// </summary>
        /// <param name="searchId">기자 아이디[보도정보 시스템]</param>
        /// <param name="searchName">기자명</param>
        /// <param name="searchInitial">기자명 초성</param>
        /// <returns>ListModel<NUP_REPORTER_SELECT_Result></returns>
        public ListModel<NUP_REPORTER_SELECT_Result> GetReporterList(string searchId, string searchName, string searchInitial, int? page, int? pageSize, string isRandom)
        {
            //            int Page = condition.Page;
            //int PageSize = condition.PageSize;
            //, int? Page, PageSize

            ListModel<NUP_REPORTER_SELECT_Result> resultData = new ListModel<NUP_REPORTER_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_REPORTER_SELECT(searchId, searchName, searchInitial, page, pageSize, isRandom).ToList();

            return resultData;
        }

        /// <summary>
        /// 기자 정보
        /// </summary>
        /// <param name="reporterhId">기자 아이디[보도정보 시스템]</param>
        /// <returns>NUP_REPORTER_SELECT_Result</returns>
        public NUP_REPORTER_SELECT_Result GetReporterInfo(string reporterId)
        {
            NUP_REPORTER_SELECT_Result model = db49_Article.NUP_REPORTER_SELECT(reporterId, null, null, 1, 1, null).SingleOrDefault(p => p.REPORTER_ID.Equals(reporterId));

            return model;
        }

        /// <summary>
        /// 기자 프로필 정보 저장
        /// </summary>
        public void SaveReporterInfo(NTB_REPORTER_PROFILE model, string txtImgURL, LoginUserInfo loginUser)
        {
            var data = db49_Article.NTB_REPORTER_PROFILE.SingleOrDefault(a => a.REPORTER_ID.Equals(model.REPORTER_ID));

            if (model.BLOG_VIEW_YN == null) model.BLOG_VIEW_YN = "N";
            if (model.FACEBOOK_VIEW_YN == null) model.FACEBOOK_VIEW_YN = "N";
            if (model.TWITTER_VIEW_YN == null) model.TWITTER_VIEW_YN = "N";

            if(data != null)
            {
                data.EMAIL = model.EMAIL;
                data.BLOG_URL = model.BLOG_URL;
                data.BLOG_VIEW_YN = model.BLOG_VIEW_YN;
                data.TWITTER_URL = model.TWITTER_URL;
                data.TWITTER_VIEW_YN = model.TWITTER_VIEW_YN;
                data.FACEBOOK_URL = model.FACEBOOK_URL;
                data.FACEBOOK_VIEW_YN = model.FACEBOOK_VIEW_YN;
                data.INTRODUCTION = model.INTRODUCTION;
                data.MOD_DATE = DateTime.Now;
                data.MOD_ID = loginUser.UserId;
                data.USER_ID = loginUser.UserId;

                var splitData = data.IMAGE_URL.Split('/');
                if(txtImgURL != splitData[splitData.Length - 1])
                {
                    data.IMAGE_URL = model.IMAGE_URL;
                }
            }
            else
            {
                model.REG_DATE = DateTime.Now;
                model.REG_ID = loginUser.UserId;
                model.USER_ID = loginUser.UserId;
                db49_Article.NTB_REPORTER_PROFILE.Add(model);
            }
            db49_Article.SaveChanges();
        }

        /// <summary>
        /// 기자에게 한마디 가져오기
        /// </summary>
        /// <param name="condition">기자명 / 회원아이디</param>
        /// <returns>ListModel<NTB_REPORTER_AWORD></returns> 
        public ListModel<ReportAword> GetAWordToReporter(AwordCondition condition, LoginUserInfo loginUser)
        {
            var resultData = new ListModel<ReportAword>();
            var resultListData = new List<ReportAword>();
            var list = db49_Article.NTB_REPORTER_AWORD.Where(a => a.REPORTER_ID.Equals(condition.ReporterId) && a.MSEQ.Equals(null));
            var reporterInfo = GetReporterInfo(condition.ReporterId);

            
            if(loginUser.SnsId != null)
            {
                var str = loginUser.SnsId.ToString();
                list = list.Where(a => a.USER_ID.Equals(str));
            }
            else
            {
                if (reporterInfo.USER_ID != loginUser.UserId)
                {
                    list = list.Where(a => a.USER_ID.Equals(loginUser.UserId));
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

            var subreplyList = db49_Article.NTB_REPORTER_AWORD.Where(a => a.MSEQ > 0).ToList();
            foreach (var item in list)
            {
                var reportAword = new ReportAword
                {
                    SubReplyList = subreplyList.Where(a => a.MSEQ.Equals(item.SEQ)).OrderByDescending(a => a.REG_DATE).ToList(),
                    ReplyData = item
                };

                resultListData.Add(reportAword);
            }

            resultData.ListData = resultListData;
            return resultData;
        }

        /// <summary>
        /// 기자에게 한마디 저장
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        public void SaveAWordToReporter(NTB_REPORTER_AWORD model, LoginUserInfo loginUser)
        {
            var data = db49_Article.NTB_REPORTER_AWORD.SingleOrDefault(a => a.SEQ.Equals(model.SEQ));
            if (data != null)
            {
                data.CONTENT = model.CONTENT;
                data.MOD_DATE = DateTime.Now;
                data.LOGIN_TYPE = "";
            }
            else
            {
                if (loginUser.SnsId != null)
                {
                    model.USER_ID = loginUser.SnsId.ToString();
                    model.NAME = loginUser.SnsName;
                }
                else
                {
                    model.USER_ID = loginUser.UserId;
                    model.NAME = loginUser.NickName;
                    model.LOGIN_TYPE = "H";
                }

                if (loginUser.FacebookInfo.Id != null)
                {
                    model.LOGIN_TYPE = "F";
                }

                if (loginUser.KakaoInfo.Id != null)
                {
                    model.LOGIN_TYPE = "K";
                }

                if (loginUser.NaverInfo.Id != null)
                {
                    model.LOGIN_TYPE = "N";
                }
                model.REG_DATE = DateTime.Now;
                db49_Article.NTB_REPORTER_AWORD.Add(model);
            }
            db49_Article.SaveChanges();
        }

        /// <summary>
        /// 기자에게 한마디 삭제
        /// </summary>
        /// <param name="replyId"></param>
        public void DeleteAWordToReporter(int replyId)
        {
            var data = db49_Article.NTB_REPORTER_AWORD.SingleOrDefault(a => a.SEQ.Equals(replyId));
            if (data != null)
            {
                var subreplyList = db49_Article.NTB_REPORTER_AWORD.Where(a => a.MSEQ > 0).ToList();
                var subData = subreplyList.Where(a => a.MSEQ.Equals(data.SEQ));
                if (subData != null)
                {
                    foreach (var item in subData)
                    {
                        db49_Article.NTB_REPORTER_AWORD.Remove(item);
                    }
                }
                db49_Article.NTB_REPORTER_AWORD.Remove(data);
                db49_Article.SaveChanges();
            }
        }

        /// <summary>
        /// 추천 가져오기
        /// </summary>
        /// <param name="reporterID"></param>
        /// <returns></returns>
        public int GetRecommend(string reporterID)
        {
            var recommend = 0;
            var data = db49_Article.NTB_REPORTER_RECOMMEND.Where(a => a.REPORTER_ID.Equals(reporterID));
            foreach (var item in data)
            {
                recommend += item.RECOMMEND_COUNT;
            }
            return recommend;
        }

        /// <summary>
        /// 추천 저장
        /// </summary>
        /// <param name="reporterID"></param>
        public void SaveRecommend(string reporterID)
        {
            DateTime today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            var data = db49_Article.NTB_REPORTER_RECOMMEND.Where(a => a.REPORTER_ID.Equals(reporterID) && a.RECOMMEND_DATE.Equals(today)).FirstOrDefault();
            if (data != null)
            {
                data.RECOMMEND_COUNT = data.RECOMMEND_COUNT + 1;
            }
            else
            {
                var model = new NTB_REPORTER_RECOMMEND
                {
                    RECOMMEND_COUNT = 1,
                    RECOMMEND_DATE = DateTime.Now,
                    REPORTER_ID = reporterID
                };
                db49_Article.NTB_REPORTER_RECOMMEND.Add(model);
            }
            db49_Article.SaveChanges();
        }

        /// <summary>
        /// 기자 구독하기 정보 가져오기 (로그인한경우)
        /// </summary>
        /// <param name="reporterID"></param>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        public NTB_ARTICLE_SUBSCRIPTION GetSubScription(string reporterID, LoginUserInfo loginUserInfo)
        {
            return db49_Article.NTB_ARTICLE_SUBSCRIPTION.SingleOrDefault(a => a.REPORTER_ID.Equals(reporterID) && a.USER_ID.Equals(loginUserInfo.UserId));
        }

        /// <summary>
        /// 구독하기 저장
        /// </summary>
        /// <param name="model"></param>
        /// <param name=""></param>
        public bool SaveSubScription(NTB_ARTICLE_SUBSCRIPTION model, LoginUserInfo loginUserInfo)
        {
            var data = CheckSubScription(model);
            var isSave = false;
            if(data == null)
            {
                model.REG_DATE = DateTime.Now;
                if(loginUserInfo != null)
                {
                    model.REG_ID = loginUserInfo.UserId;
                    model.USER_ID = loginUserInfo.UserId;
                }
                db49_Article.NTB_ARTICLE_SUBSCRIPTION.Add(model);
                db49_Article.SaveChanges();
                isSave = true;
            }

            return isSave;
        }

        /// <summary>
        /// 구독하기 체크
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUserInfo"></param> (비로그인시)
        public NTB_ARTICLE_SUBSCRIPTION CheckSubScription(NTB_ARTICLE_SUBSCRIPTION model)
        {
            return db49_Article.NTB_ARTICLE_SUBSCRIPTION.SingleOrDefault(a => a.REPORTER_ID.Equals(model.REPORTER_ID) && a.EMAIL.Equals(model.EMAIL));
        }

        /// <summary>
        /// 구독하기 취소
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUserInfo"></param>
        public void DeleteSubScription(string reporterID, LoginUserInfo loginUserInfo)
        {
            var data = GetSubScription(reporterID, loginUserInfo);
            if(data != null)
            {
                db49_Article.NTB_ARTICLE_SUBSCRIPTION.Remove(data);
                db49_Article.SaveChanges();
            }
        }

        /// <summary>
        /// 구독하기 취소
        /// </summary>
        /// <param name="reporterId"></param>
        /// <param name="userEmail"></param>
        public void SubScriptionReject(string reporterId, string userEmail)
        {
            var data = db49_Article.NTB_ARTICLE_SUBSCRIPTION.SingleOrDefault(a => a.REPORTER_ID.Equals(reporterId) && a.EMAIL.Equals(userEmail));

            if (data != null)
            {
                db49_Article.NTB_ARTICLE_SUBSCRIPTION.Remove(data);
                db49_Article.SaveChanges();
            }
        }
        
        /// <summary>
        /// 이메일 보내기
        /// </summary>
        /// <param name="email"></param>
        public void SendEmail(SendEmail email)
        {
            new EmailBiz().Send(new EmailSendParameter()
            {
                EmailCode = EmailCodeModel.SendReporterEmail,
                FromName = email.UserName,
                FromEmail = email.UserEmail,
                ToName = email.ReporterName,
                ToEmail = email.ReporterEmail,
                Subject = email.Title,
                Contents = email.Contents
            });
        }
    }
}
