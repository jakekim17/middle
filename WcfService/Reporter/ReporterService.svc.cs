using Wow.Tv.Middle.Biz.Reporter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.Reporter;

namespace Wow.Tv.Middle.WcfService.Reporter
{
    public class ReporterService : IReporterService
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
            return new ReporterBiz().GetReporterList(searchId, searchName, searchInitial, page, pageSize, isRandom);
        }

        /// <summary>
        /// 기자 정보
        /// </summary>
        /// <returns>NUP_REPORTER_SELECT_Result</returns>
        public NUP_REPORTER_SELECT_Result GetReporterInfo(string reporterId)
        {
            return new ReporterBiz().GetReporterInfo(reporterId);
        }

        /// <summary>
        /// 기자에게 한마디
        /// </summary>
        /// <param name="condition">기자아이디/ 회원아이디</param>
        /// <returns></returns>
        public ListModel<ReportAword> GetAWordToReporter(AwordCondition condition, LoginUserInfo loginUser)
        {
            return new ReporterBiz().GetAWordToReporter(condition, loginUser);
        }

        /// <summary>
        /// 기자에게 한마디 저장
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        public void SaveAWordToReporter(NTB_REPORTER_AWORD model, LoginUserInfo loginUser)
        {
            new ReporterBiz().SaveAWordToReporter(model, loginUser);
        }

        /// <summary>
        /// 기자에게 한마디 삭제
        /// </summary>
        /// <param name="replyId"></param>
        public void DeleteAWordToReporter(int replyId)
        {
            new ReporterBiz().DeleteAWordToReporter(replyId);
        }

        /// <summary>
        /// 추천수 가져오기
        /// </summary>
        /// <param name="reporterID"></param>
        /// <returns></returns>
        public int GetRecommend(string reporterID)
        {
            return new ReporterBiz().GetRecommend(reporterID);
        }

        /// <summary>
        /// 추천수 저장
        /// </summary>
        /// <param name="reporterID"></param>
        public void SaveRecommend(string reporterID)
        {
            new ReporterBiz().SaveRecommend(reporterID);
        }

        /// <summary>
        /// 기자 정보 저장
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        public void SaveReporterInfo(NTB_REPORTER_PROFILE model, string txtImgURL, LoginUserInfo loginUser)
        {
            new ReporterBiz().SaveReporterInfo(model, txtImgURL, loginUser);
        }

        /// <summary>
        /// 구독하기 가져오기
        /// </summary>
        /// <param name="reporterID"></param>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        public NTB_ARTICLE_SUBSCRIPTION GetSubScription(string reporterID, LoginUserInfo loginUserInfo)
        {
            return new ReporterBiz().GetSubScription(reporterID, loginUserInfo);
        }

        /// <summary>
        /// 구독하기 저장
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        public bool SaveSubScription(NTB_ARTICLE_SUBSCRIPTION model, LoginUserInfo loginUserInfo)
        {
            return new ReporterBiz().SaveSubScription(model, loginUserInfo);
        }

        /// <summary>
        /// 구독하기 삭제
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUserInfo"></param>
        public void DeleteSubScription(string reporterID, LoginUserInfo loginUserInfo)
        {
             new ReporterBiz().DeleteSubScription(reporterID, loginUserInfo);
        }

        /// <summary>
        /// 구독하기 취소
        /// </summary>
        /// <param name="reporterId"></param>
        /// <param name="userEmail"></param>
        public void SubScriptionReject(string reporterId, string userEmail)
        {
            new ReporterBiz().SubScriptionReject(reporterId, userEmail);
        }

        public void SendEmail(SendEmail email)
        {
            new ReporterBiz().SendEmail(email);
        }
    }
}
