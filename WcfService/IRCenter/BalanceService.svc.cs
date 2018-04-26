using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.IRCenter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db51.contents;
using Wow.Tv.Middle.Model.Db51.contents.Balance;

namespace Wow.Tv.Middle.WcfService.IRCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "BalanceService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 BalanceService.svc나 BalanceService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class BalanceService : IBalanceService
    {
        public BalanceModel<NTB_FINANCE_STATUS> GetList(BalanceCondition condition)
        {
            return new BalanceBiz().GetList(condition);
        }

        public void Delete(int[] deleteList)
        {
            new BalanceBiz().Delete(deleteList);
        }

        public int Save(NTB_FINANCE_STATUS model, LoginUser loginUser)
        {
            return new BalanceBiz().Save(model, loginUser);
        }

        public NTB_FINANCE_STATUS GetData(int no)
        {
            return new BalanceBiz().GetData(no);
        }

        public BalanceModel<NTB_FINANCE_STATUS> GetFrontData(String year)
        {
            return new BalanceBiz().GetFrontData(year);
        }
    }
}
