using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db51.contents;
using Wow.Tv.Middle.Model.Db51.contents.Balance;

namespace Wow.Tv.Middle.WcfService.IRCenter
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IBalanceService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IBalanceService
    {
        [OperationContract]
        BalanceModel<NTB_FINANCE_STATUS> GetList(BalanceCondition condition);

        [OperationContract]
        void Delete(int[] deleteList);

        [OperationContract]
        int Save(NTB_FINANCE_STATUS model, LoginUser loginUser);

        [OperationContract]
        NTB_FINANCE_STATUS GetData(int no);

        [OperationContract]
        BalanceModel<NTB_FINANCE_STATUS> GetFrontData(String year);
    }
}
