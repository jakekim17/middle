using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.NewsCenter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "TravelService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 TravelService.svc나 TravelService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class TravelService : ITravelService
    {

        /// <summary>
        /// 여행 정보 리스트
        /// </summary>
        /// <param name="condition">검색 조건</param>
        /// <returns></returns>
        public ListModel<ArticleClass_Hanatour> TravelList(NewsCenterCondition condition)
        {
            return new TravelBiz().TravelList(condition);
        }

        /// <summary>
        /// 여행 기사 내용
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns>ArticleClass_Hanatour</returns>
        public ArticleClass_Hanatour GetTravelReadInfo(string articleId)
        {
            return new TravelBiz().GetTravelReadInfo(articleId);
        }

        /// <summary>
        /// 기사의 좋아요(여행) COUNT
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns></returns>
        public NTB_ARTICLE_LIKEIT_HANATOUR GetArticleLikeitHanatour(string articleId)
        {
            return new TravelBiz().GetArticleLikeitHanatour(articleId);
        }

        /// <summary>
        /// 기사의 좋아요(여행) Insert & Update
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <param name="likeitGubun">좋아요 구분</param>
        public void SetArticleLikeitHanatour(string articleId, string likeitGubun)
        {
            new TravelBiz().SetArticleLikeitHanatour(articleId, likeitGubun);
        }

        /// <summary>
        /// 이전, 다음 기사
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <param name="condition">검색 조건</param>
        /// <returns>ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result></returns>
        public ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result> GetTravelPrevNext(string articleId, NewsCenterCondition condition)
        {
            return new TravelBiz().GetTravelPrevNext(articleId, condition);
        }

    }
}
