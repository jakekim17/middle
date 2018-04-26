using Wow.Tv.Middle.Biz.NewsCenter;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;


namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "LandService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 LandService.svc나 LandService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class LandService : ILandService
    {
        public void DoWork()
        {
        }

        public NTB_POST GetMapAddress(string Sido, string Gugun)
        {
            return new LandBiz().GetMapAddress(Sido, Gugun);
        }
    }
}
