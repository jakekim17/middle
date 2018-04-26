using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Admin;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.Admin;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.TradingStar;

namespace Wow.Tv.Middle.WcfService.Admin
{

    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "ITradingStarService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface ITradingStarService
    {
        [OperationContract]
        List<tblTradingStarCategory> CategoryList();

        [OperationContract]
        tblTradingStarCategory GetCategory(string tradingCode);

        [OperationContract]
        void CategorySave(tblTradingStarCategory tradingstarCategory, LoginUser loginUser);

        [OperationContract]
        ListModel<tblTradingStarUser> UserList(TradingStarCondition condition);

        [OperationContract]
        void UserDelete(int seq);

        [OperationContract]
        void UserSave(tblTradingStarUser tradingStarUser, LoginUser loginUser);

        [OperationContract]
        tblTradingStarUser GetUser(int seq);


        [OperationContract]
        IList<tblTradingStarTrade> TradeList(int regseq);

        [OperationContract]
        void TradeDelete(int seq);

        [OperationContract]
        void TradeSave(tblTradingStarTrade tradingStarUser, LoginUser loginUser);

        [OperationContract]
        tblTradingStarTrade GetTrade(int seq);

        [OperationContract]
        List<tblOnlineSise> CurrentStockPriceList(string korname);

        [OperationContract]
        void UpdateEarningRate(int seq, double earningRateSum);

        [OperationContract]
        void UpdateTotalHavaRateText(int regSeq, double totalHavaRateText);

        [OperationContract]
        ListModel<tblStockBatch> GetStockList(StockBatchCondition condition);

        [OperationContract]
        void UpdateStarMark(int seq, float starMark);

    }
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "TradingStarService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 TradingStarService.svc나 TradingStarService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class TradingStarService : ITradingStarService
    {
        #region 수익률 Middle
        public List<tblTradingStarCategory> CategoryList()
        {
            return new TradingStarBiz().CategoryList();
        }

        public tblTradingStarCategory GetCategory(string tradingCode)
        {
            return new TradingStarBiz().GetCategory(tradingCode);
        }

        public void CategorySave(tblTradingStarCategory tradingstarCategory, LoginUser loginUser)
        {
            new TradingStarBiz().CategorySave(tradingstarCategory, loginUser);
        }
        #endregion

        #region 출연자 Middle
        public ListModel<tblTradingStarUser> UserList(TradingStarCondition condition)
        {
            return new TradingStarBiz().UserList(condition);
        }

        public tblTradingStarUser GetUser(int seq)
        {
            return new TradingStarBiz().GetUser(seq);
        }

        public void UserSave(tblTradingStarUser tradingStarUser, LoginUser loginUser)
        {
            new TradingStarBiz().UserSave(tradingStarUser, loginUser);
        }

        public void UserDelete(int seq)
        {
            new TradingStarBiz().UserDelete(seq);
        }
        #endregion

        #region 거래현황 Middle
        public IList<tblTradingStarTrade> TradeList(int regseq)
        {
            return new TradingStarBiz().TradeList(regseq);
        }

        public tblTradingStarTrade GetTrade(int seq)
        {
            return new TradingStarBiz().GetTrade(seq);
        }

        public void TradeSave(tblTradingStarTrade tradingStarTrade, LoginUser loginUser)
        {
            new TradingStarBiz().TradeSave(tradingStarTrade, loginUser);
        }

        public void TradeDelete(int seq)
        {
            new TradingStarBiz().TradeDelete(seq);
        }

        public List<tblOnlineSise> CurrentStockPriceList(string korname)
        {

            return new TradingStarBiz().CurrentStockPriceList(korname);
        }

        public void UpdateEarningRate(int seq, double earningRateSum)
        {
            new TradingStarBiz().UpdateEarningRate(seq, earningRateSum);
        }

        public void UpdateTotalHavaRateText(int regSeq,double totalHavaRateText)
        {
            new TradingStarBiz().UpdateTotalHavaRateText(regSeq, totalHavaRateText);
        }

        public ListModel<tblStockBatch> GetStockList(StockBatchCondition condition)
        {
            return new TradingStarBiz().GetStockList(condition);
        }

        public void UpdateStarMark(int seq, float starMark)
        {
            new TradingStarBiz().UpdateStarMark(seq, starMark);

        }
        #endregion
    }
}
