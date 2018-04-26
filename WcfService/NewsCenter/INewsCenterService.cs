using System;
using System.Collections.Generic;
using System.ServiceModel;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.broadcast.NewsCenter;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "INewsCenterService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface INewsCenterService
    {

        /// <summary>
        /// 뉴스 상세 내용
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <param name="prevArticleId">이전 기사 아이디</param>
        /// <param name="mediaGubun">pc,mobile 구분</param>
        /// <returns>ListModel<NUP_NEWS_READ_SELECT_Result></returns>
        [OperationContract]
        NUP_NEWS_READ_SELECT_Result GetNewsReadInfo(string articleId, string prevArticleId, string mediaGubun);


        /// <summary>
        /// 뉴스 상세 관련 기사 리스트
        /// </summary>
        /// <param name="articleIdList">기사 아이디</param>
        /// <param name="articleIdList">제외할 기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_READ_RELATION_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_READ_RELATION_SELECT_Result> GetNewsReadRelationList(string articleId, List<String> articleIdList);


        /// <summary>
        /// 기사의 좋아요 COUNT
        /// </summary>
        /// <param name="articleId">기사아이디</param>
        /// <returns></returns>
        [OperationContract]
        NTB_ARTICLE_LIKEIT GetArticleLikeit(string articleId);

        /// <summary>
        /// 기사의 좋아요 Insert & Update
        /// </summary>
        /// <param name="articleId">기사아이디</param>
        /// <param name="likeitGubun">좋아요 구분</param>
        [OperationContract]
        void SetArticleLikeit(string articleId, string likeitGubun);

        /// <summary>
        /// 이전, 다음 기사
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <param name="condition">검색 조건</param>
        /// <returns>ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_PREV_NEXT_SELECT_Result> GetNewsPrevNext(string articleId, NewsCenterCondition condition);

        /// <summary>
        /// 이기사와 많이본 기사
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns>ListModel<NUP_NEWS_THIS_MANY_SEE_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_THIS_MANY_SEE_SELECT_Result> GetNewsThisManySee(string articleId);

        /// <summary>
        /// 색션별 많이 본기사
        /// </summary>
        /// <param name="searchSection">섹션구분</param>
        /// <param name="searchWowCode">와우코드</param>
        /// <returns>ListModel<NUP_NEWS_SECTION_MANY_SEE_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_SECTION_MANY_SEE_SELECT_Result> GetNewsSectionManySee(string searchSection, string searchWowCode);

        /// <summary>
        /// 기사의 종목 코드
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns>ListModel<ArticleStock></returns>
        [OperationContract]
        ListModel<ArticleStock> GetArticleStockList(string articleId);
        
        /// <summary>
        /// 뉴스 상세 내용 종목정보
        /// </summary>
        /// <param name="articleId">기사 아이디</param>
        /// <returns>ListModel<usp_GetStockPrice_Result></returns>
        [OperationContract]
        ListModel<usp_GetStockPrice_Result> GetStockInfo(string articleId);

        /// <summary>
        /// 뉴스 상세 파트너 아닌 기자 정보
        /// </summary>
        /// <param name="reporterId">기자 아이디</param>
        /// <returns>ListModel<NUP_REPORTER_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_REPORTER_SELECT_Result> GetReporterInfo(NewsCenterCondition condition);

        /// <summary>
        /// 뉴스 상세 파트너 기자 정보
        /// </summary>
        /// <param name="reporterId">기자 아이디</param>
        /// <returns>ListModel<NUP_REPORTER_SELECT_Result></returns>
        [OperationContract]
        PartnerInfo GetReportPartnerInfo(NewsCenterCondition condition, string tag);

        /// <summary>
        /// 색션별 뉴스 리스트
        /// </summary>
        /// <param name="condition">검색 조건</param>
        /// <returns>ListModel<NUP_NEWS_SECTION_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_SECTION_SELECT_Result> GetNewsSectionList(NewsCenterCondition condition);        

        /// <summary>
        /// 기사 카운터 조회
        /// </summary>
        /// <param name="startDate">시작일</param>
        /// <param name="endDate">종료일</param>
        /// <param name="title">제목</param>
        /// <param name="articleID">기사 ID</param>
        /// <param name="userName">기자명</param>
        /// <returns>기사수</returns>
        [OperationContract]
        int GetArticleCount(string startDate, string endDate, string title, string articleID, string userName);


        /// <summary>
        /// 기사 카운터 조회
        /// </summary>
        /// <param name="startDate">시작일</param>
        /// <param name="endDate">종료일</param>
        /// <returns>기사수</returns>
        [OperationContract]
        int GetArticleCountDate(string startDate, string endDate);


        /// <summary>
        /// 뉴스 스탠드 최종 기사 송출 시간
        /// </summary>
        /// <returns>업데이트 정보</returns>
        [OperationContract]
        tblNewsStandArticleManage GetNewsStandUpdateTime();

        /// <summary>
        /// 뉴스 스탠드 최종 기사 송출 시간 수정
        /// </summary>
        /// <returns>결과값</returns>
        [OperationContract]
        bool SetNewsStandUpdateTime();

        /// <summary>
        /// 뉴스 스탠드 리스트
        /// </summary>
        /// <returns>ListModel<NUP_NEWSSTAND_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWSSTAND_SELECT_Result> GetNewsStandList();


        /// <summary>
        /// 뉴스 메인 리스트 설정
        /// </summary>
        /// <param name="updateInfo">수정할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>결과값</returns>
        [OperationContract]
        bool SetNewsMainListUpdate(ListModel<NUP_NEWS_MAIN_MAGE_SELECT_Result> updateInfo, LoginUser loginUser);


        /// <summary>
        /// 뉴스 스탠드 수정 정보
        /// </summary>
        /// <returns>tblNewsStand</returns>
        [OperationContract]
        tblNewsStand GetNewsStandLastUpdateInfo();


        /// <summary>
        /// 뉴스 스탠드 가제목 수정
        /// </summary>
        /// <param name="updateInfo">수정할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>결과값</returns>
        [OperationContract]
        bool SetNewsStandTmpTitleUpdate(ListModel<tblNewsStand> updateInfo, LoginUser loginUser);


        /// <summary>
        /// 뉴스 메인 관리 리스트
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns>ListModel<NUP_NEWS_MAIN_MAGE_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_MAIN_MAGE_SELECT_Result> GetNewsMainMageList(NewsCenterCondition condition);


        /// <summary>
        /// HTML 재변환
        /// </summary>
        /// <param name="articleId">기사 ID</param>
        /// <param name="compCode">Comp Code</param>
        /// <returns>isSuccess</returns>
        [OperationContract]
        bool SetArticleHtmlConvert(string articleId, string compCode);


        /// <summary>
        /// 기사 내용
        /// </summary>
        /// <param name="ArtID">기사 ID</param>
        /// <returns>기사내용</returns>
        [OperationContract]
        tblArticleList GetArtcleInfo(string ArtID);

        /// <summary>
        /// 기사 관련 이미지 정보
        /// </summary>
        /// <param name="ArtID">기사 ID</param>
        /// <returns>기사내용</returns>
        [OperationContract]
        tblRelationImage GetRelationImageInfo(string ArtID);

        /// <summary>
        /// 기사 관련 이미지 등록
        /// </summary>
        /// <param name="relationImageInfo">이미지 정보</param>
        /// <returns>처리결과</returns>
        [OperationContract]
        bool SetRelationImageSave(tblRelationImage relationImageInfo);

        /// <summary>
        /// 기사 관련 이미지 삭제
        /// </summary>
        /// <param name="relationImageInfo">이미지 정보</param>
        /// <returns>처리결과</returns>
        [OperationContract]
        bool SetRelationImageDelete(tblRelationImage relationImageInfo);

        /// <summary>
        /// 기사 작성자 정보
        /// </summary>
        /// <param name="ArtID">기사 ID</param>
        /// <returns>기사 작성자 정보</returns>
        [OperationContract]
        tblArticleCreation GetArtcleCreationInfo(string ArtID);


        /// <summary>
        /// 카드&VOD 뉴스 리스트
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns>ListModel<NUP_NEWS_CARD_VOD_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_CARD_VOD_SELECT_Result> GetNewsCardVodList(NewsCenterCondition condition);


        /// <summary>
        /// 부동산&연예.스포츠 뉴스 리스트
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns>ListModel<NUP_NEWS_LAND_ENTSPO_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_NEWS_LAND_ENTSPO_SELECT_Result> GetNewsLandEntSpoList(NewsCenterCondition condition);


        /// <summary>
        /// 뉴스 관련 뉴스 설정 정보 리스트
        /// </summary>
        /// <returns>ListModel<NTB_ARTICLE_RELATION_CONFIG></returns>
        [OperationContract]
        ListModel<NTB_ARTICLE_RELATION_CONFIG> GetNewsRelationConfigList();

        /// <summary>
        /// 관련 뉴스 설정
        /// </summary>
        /// <param name="saveInfo">설정할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>결과값</returns>
        [OperationContract]
        bool SetNewsRelationConfigSave(NTB_ARTICLE_RELATION_CONFIG saveInfo, LoginUser loginUser);

        /// <summary>
        /// 관련 뉴스 설정 수정
        /// </summary>
        /// <param name="saveInfo">수정할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>결과값</returns>
        [OperationContract]
        bool SetNewsRelationConfigUpdate(NTB_ARTICLE_RELATION_CONFIG saveInfo, LoginUser loginUser);


        /// <summary>
        /// 뉴스 노출설정 정보
        /// </summary>
        /// <param name="ShowCode">설정 색션코드</param>
        /// <returns>NTB_ARTICLE_SHOW_CONFIG</returns>
        [OperationContract]
        NTB_ARTICLE_SHOW_CONFIG GetNewsShowConfig(string ShowCode);


        /// <summary>
        /// 뉴스 노출설정(자동/수동) UPDATE
        /// </summary>
        /// <param name="updateInfo">수정할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>처리결과</returns>
        [OperationContract]
        bool SetNewsShowConfig(NTB_ARTICLE_SHOW_CONFIG updateInfo, LoginUser loginUser);


        /// <summary>
        /// 뉴스 활성설정(ACTIVE_YN) UPDATE
        /// </summary>
        /// <param name="updateInfo">수정할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>처리결과</returns>
        [OperationContract]
        bool SetNewsShowActiveConfig(NTB_ARTICLE_SHOW_CONFIG updateInfo, LoginUser loginUser);


        /// <summary>
        /// 뉴스(기사) 노출순서 설정
        /// </summary>
        /// <param name="saveInfo">저장할 정보</param>
        /// <param name="loginUser">로그인 정보</param>
        /// <returns>결과값</returns>
        [OperationContract]
        bool SetNewsShowNumSave(ListModel<NTB_ARTICLE_SHOW_NUM> saveInfo, LoginUser loginUser);


        /// <summary>
        /// 오피니언 리스트
        /// </summary>
        /// <param name="condition">검색 조건</param>
        /// <returns>ListModel<tblPlanArticle></returns>
        [OperationContract]
        ListModel<tblPlanArticle> GetOpinionList(NewsCenterCondition condition);

        /// <summary>
        /// 오피니언 정보
        /// </summary>
        /// <param name="SEQ">일렬번호</param>
        /// <returns>tblPlanArticle</returns>
        [OperationContract]
        tblPlanArticle GetOpinionInfo(int SEQ);

        /// <summary>
        /// 오피니언 등록,수정
        /// </summary>
        /// <param name="tblPlanArticleInfo">오피니언 정보</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        [OperationContract]
        bool SetOpinionInfo(tblPlanArticle tblPlanArticleInfo, LoginUser loginUser);

        /// <summary>
        /// 오피니언 삭제
        /// </summary>
        /// <param name="deleteList">SEQ List</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        [OperationContract]
        bool SetOpinionDelete(int[] deleteList, LoginUser loginUser);

        /// <summary>
        /// 기사 뷰페이지 관리 리스트
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        ListModel<NTB_ARTICLE_VIEWPAGE_MANAGE> GetNewsViewPageManageList();

        /// <summary>
        /// 기사 뷰페이지 관리 등록,수정
        /// </summary>
        /// <param name="viewPageManageInfo">설정 정보</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        [OperationContract]
        bool SetNewsViewPageManage(NTB_ARTICLE_VIEWPAGE_MANAGE viewPageManageInfo, LoginUser loginUser);

        /// <summary>
        /// 기자의 최신 기사 리스트
        /// </summary>
        /// <param name="reporterId">reporterId</param>
        /// <returns>ListModel<NUP_REPORTER_RECENTLY_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_REPORTER_RECENTLY_SELECT_Result> GetReporterRecentlySelect(string reporterId);

        /// <summary>
        /// 기자 페이지 관리 USER ID 등록 체크
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>결과</returns>
        [OperationContract]
        bool IsReporterManageUserIdDuplicated(string userId);

        /// <summary>
        /// 기자 페이지 관리 리스트
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns>ListModel<NUP_REPORTER_MANAGE_SELECT_Result></returns>
        [OperationContract]
        ListModel<NUP_REPORTER_MANAGE_SELECT_Result> GetReporterMageList(NewsCenterCondition condition);

        /// <summary>
        /// 기자 페이지 관리 등록,수정
        /// </summary>
        /// <param name="reporterManageInfo">설정 정보</param>
        /// <param name="loginUser">로그인 사용자 정보</param>
        /// <returns>처리결과</returns>
        [OperationContract]
        bool SetReporterManage(NTB_REPORTER_MANAGE reporterManageInfo, LoginUser loginUser);


        /// <summary>
        /// 하나투어 여행 기사 리스트
        /// </summary>
        /// <param name="condition">검색조건</param>
        /// <returns></returns>
        [OperationContract]
        ListModel<ArticleClass_Hanatour> GetHanatourArticleList(NewsCenterCondition condition);


        /// <summary>
        /// 하나투어 여행 기사
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        [OperationContract]
        ArticleClass_Hanatour GetHanatourArticleInfo(string articleID);

    }
}
