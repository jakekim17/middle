using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "INewsGFCService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface INewsGFCService
    {
        [OperationContract]
        ListModel<NewsGFCList> NewsGFCList(string searchYear);
		
		[OperationContract]
		//ListModel<NewsGFCList> NewsGFCView(string searchYear);
		usp_WOWTVNewsCenterView_Result NewsGFCView(string artid);



		[OperationContract]
		void sendEmailGFC(string email, string year, string language);
	}
}
