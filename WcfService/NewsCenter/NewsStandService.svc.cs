using Wow.Tv.Middle.Biz.NewsCenter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "NewsStandService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 NewsStandService.svc나 NewsStandService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class NewsStandService : INewsStandService
    {


        /// <summary>
        /// 뉴스 스탠드 해드라인 리스트 SP
        /// </summary>
        /// <returns>ListModel<usp_newsStandMetaXMLTopImg_Result></returns>
        public ListModel<usp_newsStandMetaXMLTopImg_Result> GetNewsStandHeadLine()
        {
            return new NewsStandBiz().GetNewsStandHeadLine();
        }


        /// <summary>
        /// 뉴스 스탠드 핫 키워드 SP
        /// </summary>
        /// <returns>ListModel<NUP_NEWSSTAND_KEWORD_SELECT_Result></returns>
        public ListModel<NUP_NEWSSTAND_KEWORD_SELECT_Result> GetNewsStandHotKeyword()
        {
            return new NewsStandBiz().GetNewsStandHotKeyword();
        }



    }
}
