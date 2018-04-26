using System.Collections.Generic;
using System.ServiceModel;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IQuickMenuService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IQuickMenuService
    {

        /// <summary>
        /// 퀵 메뉴 마이피 기자 리스트
        /// </summary>
        /// <param name="userID">회원아이디</param>
        /// <param name="topN">개수</param>
        /// <returns>List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result></returns>
        [OperationContract]
        List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result> GetQuickMenuMypinReporter(string userID, int? topN);


        /// <summary>
        /// 퀵 메뉴 마이피 뉴스 리스트
        /// </summary>
        /// <param name="userID">회원아이디</param>
        /// <param name="topN">개수</param>
        /// <returns>List<NUP_QUICKMENU_MYPIN_NEWS_SELECT_Result></returns>
        [OperationContract]
        List<NUP_QUICKMENU_MYPIN_NEWS_SELECT_Result> GetQuickMenuMypinNews(string userID, int? topN);

    }
}
