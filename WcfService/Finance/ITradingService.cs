using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.Finance;

namespace Wow.Tv.Middle.WcfService.Finance
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "ITradingService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface ITradingService
    {
        [OperationContract]
        string GetStockData(TradingStockCondition condition);

        [OperationContract]
        string GetDayTradingData(TradingStockCondition condition);

        [OperationContract]
        List<usp_GetBestSearchOnline_TypeA_Result> GetHotSearchList(string searchDate);
    }
}
