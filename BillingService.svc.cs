using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Wow.Tv.Middle.WcfService.Member
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IBillingService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IBillingService
    {
        [OperationContract]
        void DoWork();
    }
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "BillingService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 BillingService.svc나 BillingService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class BillingService : IBillingService
    {
        public void DoWork()
        {
        }
    }
}
