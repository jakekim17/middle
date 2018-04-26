using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.NewsCenter;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "QuickMenuService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 QuickMenuService.svc나 QuickMenuService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class QuickMenuService : IQuickMenuService
    {

        /// <summary>
        /// 퀵 메뉴 마이피 기자 리스트
        /// </summary>
        /// <param name="userID">회원아이디</param>
        /// <param name="topN">개수</param>
        /// <returns>List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result></returns>
        public List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result> GetQuickMenuMypinReporter(string userID, int? topN)
        {
            return new QuickMenuBiz().GetQuickMenuMypinReporter(userID, topN);
        }

        /// <summary>
        /// 퀵 메뉴 마이피 뉴스 리스트
        /// </summary>
        /// <param name="userID">회원아이디</param>
        /// <param name="topN">개수</param>
        /// <returns>List<NUP_QUICKMENU_MYPIN_NEWS_SELECT_Result></returns>
        public List<NUP_QUICKMENU_MYPIN_NEWS_SELECT_Result> GetQuickMenuMypinNews(string userID, int? topN)
        {
            return new QuickMenuBiz().GetQuickMenuMypinNews(userID, topN);
        }

    }
}
