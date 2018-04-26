using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Finance;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.Finance;

namespace Wow.Tv.Middle.WcfService.Finance
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "TradingService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 TradingService.svc나 TradingService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class TradingService : ITradingService
    {
        public string GetDayTradingData(TradingStockCondition condition)
        {
            return new TradingBiz().GetDayTradingData(condition);
        }

        public List<usp_GetBestSearchOnline_TypeA_Result> GetHotSearchList(string searchDate)
        {
            return new TradingBiz().GetHotSearchList(searchDate);
        }

        public string GetStockData(TradingStockCondition condition)
        {
            return new TradingBiz().GetStockData(condition);
        }
    }
}
