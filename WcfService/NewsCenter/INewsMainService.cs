using System;
using System.Collections.Generic;
using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "INewsMainService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface INewsMainService
    {
        /// <summary>
        /// 메인 뉴스 스탠드
        /// </summary>
        /// <returns>ListModel<NUP_NEWS_MAIN_NEWSSTAND_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_NEWSSTAND_SELECT_Result> GetNewsMainNewsstand();


        /// <summary>
        /// 메인 오피니언
        /// </summary>
        /// <returns>ListModel<NUP_NEWS_MAIN_OPINION_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_OPINION_SELECT_Result> GetNewsMainOpinionList();

        /// <summary>
        /// 메인 마켓 & 이슈
        /// </summary>
        /// <returns>ListModel<NUP_NEWS_MAIN_MARKET_ISSUE_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_MARKET_ISSUE_SELECT_Result> GetNewsMainMarketIssueList();

        /// <summary>
        /// 연예.스포츠 메인 리스트
        /// </summary>
        /// <param name="searchGubun">검색 조건</param>
        /// <param name="articleIdList">제외할 기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_ENT_SPO_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_ENT_SPO_SELECT_Result> GetNewsMainEntSpoList(string searchGubun, List<String> articleIdList);


        /// <summary>
        /// 메인 동영상뉴스 N개 리스트
        /// </summary>
        /// <param name="searchGubun">검색 조건</param>
        /// <param name="topN">리스트 개수</param>
        /// <param name="articleIdList">제외할 기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_VOD_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_VOD_SELECT_Result> GetNewsMainVodList(string searchGubun, int? topN, List<String> articleIdList);


        /// <summary>
        /// 뉴스 메인 & 컨텐츠 카드뉴스 N개 리스트
        /// </summary>
        /// <param name="searchGubun">검색 조건</param>
        /// <param name="topN">리스트 개수</param>
        /// <param name="articleIdList">제외할 기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_CARD_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_CARD_SELECT_Result> GetNewsMainCardList(string searchGubun, int? topN, List<String> articleIdList);


        /// <summary>
        /// 뉴스 메인 & 컨텐츠 셕션 N개 리스트
        /// </summary>
        /// <param name="searchGubun">검색 조건</param>
        /// <param name="topN">리스트 개수</param>
        /// <param name="articleIdList">제외할 기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_SECTION_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_SECTION_SELECT_Result> GetNewsMainSectionList(string searchGubun, int? topN, List<String> articleIdList);


        /// <summary>
        /// 뉴스 메인 리스트(관리자 LIST "Y" 체크된 리스트)
        /// </summary>
        /// <param name="articleIdList">기사 ID</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_LIST_Y_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_LIST_Y_SELECT_Result> GetNewsMainYList(List<String> articleIdList);


        /// <summary>
        /// 주요시세[특징주,종목,공시,와우넷]
        /// </summary>
        /// <param name="searchGubun">검색구분(특징주,종목,공시,와우넷)</param>
        /// <param name="topN">노출 개수</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_MARKET_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_MARKET_SELECT_Result> GetNewsMainMarketList(string searchGubun, int topN, List<String> articleIdList);

        /// <summary>
        /// 부동산 메인 SELECT
        /// </summary>
        /// <returns>ListModel<NUP_NEWS_MAIN_LAND_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_LAND_SELECT_Result> GetNewsMainLandList();

    }
}
