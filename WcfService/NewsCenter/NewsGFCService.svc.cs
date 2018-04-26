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
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "NewsGFCService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 NewsGFCService.svc나 NewsGFCService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class NewsGFCService : INewsGFCService
    {
        public ListModel<NewsGFCList> NewsGFCList(string searchYear)
        {
            return new NewsGFCBiz().NewsGFCList(searchYear);
        }

		//GFC 기사 뷰
		/* List 그대로 view메소드에 노출
		public ListModel<NewsGFCList> NewsGFCView(string searchYear)
		{
			return new NewsGFCBiz().NewsGFCList(searchYear);
		}
		*/
		public usp_WOWTVNewsCenterView_Result NewsGFCView(string artid)
		{
			return new NewsGFCBiz().NewsGFCView(artid);
		}

		/*
		public NUP_NEWS_READ_SELECT_Result GetNewsReadInfo(string articleId, string prevArticleId, string mediaGubun)
		{
			return new NewsCenterBiz().GetNewsReadInfo(articleId, prevArticleId, mediaGubun);
		}
		*/



		public void sendEmailGFC(string email, string year, string language)
		{
			new NewsGFCBiz().sendEmailGFC(email, year, language);
		}


	}
}
