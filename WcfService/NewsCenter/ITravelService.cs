using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "ITravelService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface ITravelService
    {
        
        /// <summary>
        /// 여행 정보 리스트
        /// </summary>
        /// <param name="condition">검색 조건</param>
        /// <returns></returns>
        [OperationContract]
        ListModel<ArticleClass_Hanatour> TravelList(NewsCenterCondition condition);

        /// <summary>
        /// 여행 기사 내용
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns>ArticleClass_Hanatour</returns>
        [OperationContract]
        ArticleClass_Hanatour GetTravelReadInfo(string articleId);

        /// <summary>
        /// 기사의 좋아요(여행) COUNT
        /// </summary>
        /// <param name="articleId">기사아이디</param>
        /// <returns></returns>
        [OperationContract]
        NTB_ARTICLE_LIKEIT_HANATOUR GetArticleLikeitHanatour(string articleId);

        /// <summary>
        /// 기사의 좋아요(여행) Insert & Update
        /// </summary>
        /// <param name="articleId">기사아이디</param>
        /// <param name="likeitGubun">좋아요 구분</param>
        [OperationContract]
        void SetArticleLikeitHanatour(string articleId, string likeitGubun);

        /// <summary>
        /// 이전, 다음 기사
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <param name="condition">검색 조건</param>
        /// <returns>ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result> GetTravelPrevNext(string articleId, NewsCenterCondition condition);

    }
}
