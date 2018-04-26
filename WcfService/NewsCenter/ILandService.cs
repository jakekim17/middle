using System.ServiceModel;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "ILandService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface ILandService
    {
        [OperationContract]
        NTB_POST GetMapAddress(string Sido, string Gugun);
    }
}
