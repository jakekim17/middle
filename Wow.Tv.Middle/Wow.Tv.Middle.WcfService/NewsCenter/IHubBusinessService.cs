using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.HubBusiness;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IHubBusinessService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IHubBusinessService
    {
        [OperationContract]
        HubBusinessModel<NTB_HUB_BUSINESS> GetList();

        [OperationContract]
        int Save(NTB_HUB_BUSINESS model, LoginUser loginUser);

        [OperationContract]
        void Delete(int[] deleteList);
    }
}
