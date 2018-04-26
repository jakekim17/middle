using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.Reporter;

namespace Wow.Tv.Middle.WcfService.Reporter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IReporterService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IReporterService
    {
        /// <summary>
        /// 기자 리스트
        /// </summary>
        /// <param name="searchId">기자 아이디[보도정보 시스템]</param>
        /// <param name="searchName">기자명</param>
        /// <param name="searchInitial">기자명 초성</param>
        /// <returns>ListModel<NUP_REPORTER_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_REPORTER_SELECT_Result> GetReporterList(string searchId, string searchName, string searchInitial, int? page, int? pageSize, string isRandom);

        /// <summary>
        /// 기자 정보
        /// </summary>
        /// <returns>NUP_REPORTER_SELECT_Result</returns>
        [OperationContract]
        NUP_REPORTER_SELECT_Result GetReporterInfo(string reporterId);

        /// <summary>
        /// 기자에게 한마디
        /// </summary>
        /// <param name="condition">기자아이디/ 회원아이디</param>
        /// <returns>ListModel<NUP_NEWS_SECTION_SELECT_Result></returns>
        [OperationContract]
        ListModel<ReportAword> GetAWordToReporter(AwordCondition condition, LoginUserInfo loginUser);

        /// <summary>
        /// 기자에게 한마디 저장
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        [OperationContract]
        void SaveAWordToReporter(NTB_REPORTER_AWORD model, LoginUserInfo loginUser);

        /// <summary>
        /// 기자에게 한마디 삭제
        /// </summary>
        /// <param name="replyId"></param>
        [OperationContract]
        void DeleteAWordToReporter(int replyId);

        /// <summary>
        /// 추천수 가져오기
        /// </summary>
        /// <param name="reporterID"></param>
        /// <returns></returns>
        [OperationContract]
        int GetRecommend(string reporterID);

        /// <summary>
        /// 추천 저장
        /// </summary>
        /// <param name="reporterID"></param>
        [OperationContract]
        void SaveRecommend(string reporterID);

        /// <summary>
        /// 기자 정보 저장
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUser"></param>
        [OperationContract]
        void SaveReporterInfo(NTB_REPORTER_PROFILE model, string txtImgURL, LoginUserInfo loginUser);

        /// <summary>
        /// 구독하기 정보 가져오기
        /// </summary>
        /// <param name="reporterID"></param>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        [OperationContract]
        NTB_ARTICLE_SUBSCRIPTION GetSubScription(string reporterID, LoginUserInfo loginUserInfo);

        /// <summary>
        /// 구독하기 저장하기
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveSubScription(NTB_ARTICLE_SUBSCRIPTION model, LoginUserInfo loginUserInfo);

        /// <summary>
        /// 구독하기 삭제하기
        /// </summary>
        /// <param name="model"></param>
        /// <param name="loginUserInfo"></param>
        [OperationContract]
        void DeleteSubScription(string reporterID, LoginUserInfo loginUserInfo);

        /// <summary>
        /// 구독하기 취소
        /// </summary>
        /// <param name="reporterId"></param>
        /// <param name="userEmail"></param>
        [OperationContract]
        void SubScriptionReject(string reporterId, string userEmail);
        
        /// <summary>
        /// 이메일 보내기
        /// </summary>
        /// <param name="email"></param>
        [OperationContract]
        void SendEmail(SendEmail email);
    }
}
