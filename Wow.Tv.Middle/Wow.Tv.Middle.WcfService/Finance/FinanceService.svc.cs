using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Finance;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.Finance;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.Finance;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.editVOD;
using Wow.Tv.Middle.Model.Db49.editVOD.Stock;
using Wow.Tv.Middle.Model.Db49.wowcafe;
using Wow.Tv.Middle.Model.Db49.wowcafe.Finance;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.Finance;

namespace Wow.Tv.Middle.WcfService.Finance
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "FinanceService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 FinanceService.svc나 FinanceService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class FinanceService : IFinanceService
    {
        public void DoWork()
        {
        }

        public StockInfoModel<usp_GetSiseCurrentKospi_Result, usp_getSiseMainmergecode_Result> GetSiseKospiInfo()
        {
            return new FinanceBiz().GetSiseKospiInfo();
        }

        public StockInfoModel<usp_GetSiseCurrentKosdaq_Result, usp_getSiseMainmergecode_Result> GetSiseKosdaqInfo()
        {
            return new FinanceBiz().GetSiseKosdaqInfo();
        }

        public StockInfoModel<usp_GetSiseCurrent_Result, usp_getSiseMainmergecode_Result> GetSiseFutInfo()
        {
            return new FinanceBiz().GetSiseFutInfo();
        }

        public StockInfoModel<usp_GetSiseCurrentKPI_Result, usp_getSiseMainmergecode_Result> GetSiseKPIInfo()
        {
            return new FinanceBiz().GetSiseKPIInfo();
        }

        public ListModel<usp_web_getDomesticIndex_Result> GetSiseChartDataList(ChartCondition condition)
        {
            return new FinanceBiz().GetSiseChartDataList(condition);
        }

        public ListModel<usp_getSiseTimeKosdaqChartData_new_Result> GetSiseTimeKosdaqChartDataList()
        {
            return new FinanceBiz().GetSiseTimeKosdaqChartDataList();
        }

        public ListModel<usp_getSiseTimeFutChartData_new_Result> GetSiseTimeFutChartDataList()
        {
            return new FinanceBiz().GetSiseTimeFutChartDataList();
        }

        public ListModel<usp_getSiseTimeKpi200ChartData_new_Result> GetSiseTimeKpi200ChartDataList()
        {
            return new FinanceBiz().GetSiseTimeKpi200ChartDataList();
        }

        public ListModel<usp_getSiseTimeFut_new_Result> GetSiseTimeFutList(FinanceCondition condition)
        {
            return new FinanceBiz().GetSiseTimeFutList(condition);
        }

        public ListModel<usp_getSiseTimeKospi_new_Result> GetSiseTimeKospiList(FinanceCondition condition)
        {
            return new FinanceBiz().GetSiseTimeKospiList(condition);
        }

        public ListModel<usp_getSiseTimeKosdaq_new_Result> GetSiseTimeKosdaqList(FinanceCondition condition)
        {
            return new FinanceBiz().GetSiseTimeKosdaqList(condition);
        }

        public ListModel<usp_getSiseTimeKpi200_new_Result> GetSiseTimeKpi200List(FinanceCondition condition)
        {
            return new FinanceBiz().GetSiseTimeKpi200List(condition);
        }

        public ListModel<usp_getSiseDayKospi_new_Result> GetSiseDayKospiList(FinanceCondition condition)
        {
            return new FinanceBiz().GetSiseDayKospiList(condition);
        }

        public ListModel<usp_getSiseDayKosdaq_new_Result> GetSiseDayKosdaqList(FinanceCondition condition)
        {
            return new FinanceBiz().GetSiseDayKosdaqList(condition);
        }

        public ListModel<usp_getSiseDayFut_new_Result> GetSiseDayFutList(FinanceCondition condition)
        {
            return new FinanceBiz().GetSiseDayFutList(condition);
        }

        public ListModel<usp_getSiseDayKpi200_new_Result> GetSiseDayKpi200List(FinanceCondition condition)
        {
            return new FinanceBiz().GetSiseDayKpi200List(condition);
        }

        public ListModel<usp_wownet_ETF_stock_group_Result> GetWowNetETFStockGroupList()
        {
            return new FinanceBiz().GetWowNetETFStockGroupList();
        }

        public ListModel<usp_wownet_kpi200_stock_group_new_Result> GetWowNetKPI200StockGroupList(FinanceCondition condition)
        {
            return new FinanceBiz().GetWowNetKPI200StockGroupList(condition);
        }

        public ListModel<usp_GetIndustryState_Result> GetIndustryStateList(IndustryCondition condition)
        {
            return new FinanceBiz().GetIndustryStateList(condition);
        }

        public IndustryInfoModel GetIndustryInfo(IndustryCondition condition)
        {
            return new FinanceBiz().GetIndustryInfo(condition);
        }

        public ListModel<MarketResearchModel> GetIndustryMarketResearchListTop3(IndustryCondition condition)
        {
            return new FinanceBiz().GetIndustryMarketResearchListTop3(condition);
        }

        public ListModel<usp_web_getDomesticIndustryStock_Result> GetIndustryStockList(IndustryCondition condition)
        {
            return new FinanceBiz().GetIndustryStockList(condition);
        }

        public ListModel<usp_GetThemaJisu_Result> GetThemaJisuList()
        {
            return new FinanceBiz().GetThemaJisuList();
        }

        public ListModel<usp_GetThemaOnline_Result> GetThemaOnlineList(ThemaCondition condition)
        {
            return new FinanceBiz().GetThemaOnlineList(condition);
        }

        public ListModel<usp_web_getSiseUpper_Result> GetSiseUpperList(UpperCondition condition)
        {
            return new FinanceBiz().GetSiseUpperList(condition);
        }

        public ListModel<usp_web_getSiseLower_Result> GetSiseLowerList(LowerCondition condition)
        {
            return new FinanceBiz().GetSiseLowerList(condition);
        }

        public ListModel<usp_web_GetSiseStockPlus_Result> GetSiseStockPlusList()
        {
            return new FinanceBiz().GetSiseStockPlusList();
        }

        public ListModel<usp_web_GetSiseStockPlusK_Result> GetSiseStockPlusKList()
        {
            return new FinanceBiz().GetSiseStockPlusKList();
        }

        public ListModel<usp_web_getSiseMarketSum_Result1> GetSiseMarketSumList(MarketCondition condition)
        {
            return new FinanceBiz().GetSiseMarketSumList(condition);
        }

        public ListModel<usp_web_getSiseForeignHold_Result> GetSiseForeignHoldList(MarketCondition condition)
        {
            return new FinanceBiz().GetSiseForeignHoldList(condition);
        }

        public usp_wownet_usa_index_online_Result GetUsaIndexOnline(MarketCondition condition)
        {
            return new FinanceBiz().GetUsaIndexOnline(condition);
        }

        public ListModel<usp_wownet_usa_index_hour_Result> GetUsaIndexHourList(MarketCondition condition)
        {
            return new FinanceBiz().GetUsaIndexHourList(condition);
        }

        public ListModel<usp_wownet_usa_index_history_Result> GetUsaIndexHistoryList(MarketCondition condition)
        {
            return new FinanceBiz().GetUsaIndexHistoryList(condition);
        }

        public ListModel<usp_web_usa_stock_group_Result> GetUsaStockGroupList(MarketCondition condition)
        {
            return new FinanceBiz().GetUsaStockGroupList(condition);
        }

        public usp_wownet_Asia_Index_online_Result GetAsiaIndexOnline(MarketCondition condition)
        {
            return new FinanceBiz().GetAsiaIndexOnline(condition);
        }

        public ListModel<usp_wownet_Asia_Index_hour_Result> GetAsiaIndexHourList(MarketCondition condition)
        {
            return new FinanceBiz().GetAsiaIndexHourList(condition);
        }

        public ListModel<usp_wownet_Asia_Index_history_Result> GetAsiaIndexHistoryList(MarketCondition condition)
        {
            return new FinanceBiz().GetAsiaIndexHistoryList(condition);
        }

        public ListModel<usp_web_korea_adr_Result> GetKoreaAdrList()
        {
            return new FinanceBiz().GetKoreaAdrList();
        }

        public usp_wownet_Europe_Index_online_Result GetEuropeIndexOnline(MarketCondition condition)
        {
            return new FinanceBiz().GetEuropeIndexOnline(condition);
        }

        public ListModel<usp_wownet_Europe_Index_history_Result> GetEuropeIndexHistoryList(MarketCondition condition)
        {
            return new FinanceBiz().GetEuropeIndexHistoryList(condition);
        }

        public ListModel<usp_GetInvestOnline_Result> GetInvestOnlineList()
        {
            return new FinanceBiz().GetInvestOnlineList();
        }

        public ListModel<usp_GetInvestTotal_Result> GetInvestTotalList(InvestCondition condition)
        {
            return new FinanceBiz().GetInvestTotalList(condition);
        }

        public ListModel<usp_web_usa_adr_Result> GetUSAAdrTotalList()
        {
            return new FinanceBiz().GetUSAAdrTotalList();
        }

        public ListModel<USP_WOWNET_USA_DETAIL_Result> GetUSAAdrDetailList(UsaIndustryCondition condition)
        {
            return new FinanceBiz().GetUSAAdrDetailList(condition);
        }

        public ListModel<usp_web_usa_adr_dawo_semiconductor_Result> GetUSAADRDawoSemiconductorList()
        {
            return new FinanceBiz().GetUSAADRDawoSemiconductorList();
        }

        public ListModel<usp_web_usa_adr_dawo_media_Result> GetUSAADRDawoMediaList()
        {
            return new FinanceBiz().GetUSAADRDawoMediaList();
        }

        public ListModel<usp_web_usa_adr_dawo_insurance_Result> GetUSAADRDawoInsuranceList()
        {
            return new FinanceBiz().GetUSAADRDawoInsuranceList();
        }

        public ListModel<usp_web_usa_adr_dawo_retail_Result> GetUSAADRDawoRetailList()
        {
            return new FinanceBiz().GetUSAADRDawoRetailList();
        }

        public ListModel<usp_web_usa_adr_dawo_energy_Result> GetUSAADRDawoEnergyList()
        {
            return new FinanceBiz().GetUSAADRDawoEnergyList();
        }

        public ListModel<usp_web_usa_adr_dawo_pharmacy_Result> GetUSAADRDawoPharmacyList()
        {
            return new FinanceBiz().GetUSAADRDawoPharmacyList();
        }

        public ListModel<usp_web_usa_adr_nasdaq_internet_Result> GetUSAADRNasdaqInternetList()
        {
            return new FinanceBiz().GetUSAADRNasdaqInternetList();
        }

        public ListModel<usp_web_usa_adr_nasdaq_network_Result> GetUSAADRNasdaqNetworkList()
        {
            return new FinanceBiz().GetUSAADRNasdaqNetworkList();
        }

        public ListModel<usp_web_usa_adr_nasdaq_comunications_Result> GetUSAADRNasdaqComunicationsList()
        {
            return new FinanceBiz().GetUSAADRNasdaqComunicationsList();
        }

        public ListModel<usp_web_usa_adr_nasdaq_computer_Result> GetUSAADRNasdaqComputerList()
        {
            return new FinanceBiz().GetUSAADRNasdaqComputerList();
        }

        public ListModel<usp_web_usa_adr_nasdaq_semiconductor_Result> GetUSAADRNasdaqSemiconductorList()
        {
            return new FinanceBiz().GetUSAADRNasdaqSemiconductorList();
        }

        public ListModel<usp_web_usa_adr_nasdaq_bio_Result> GetUSAADRNasdaqBioList()
        {
            return new FinanceBiz().GetUSAADRNasdaqBioList();
        }

        public ListModel<usp_web_usa_adr_sandp500_internet_Result> GetUSAADRSANDP500InternetList()
        {
            return new FinanceBiz().GetUSAADRSANDP500InternetList();
        }

        public ListModel<usp_wownet_Marketindex_BankExchange_Result> GetMarketIndexBankExchangeList()
        {
            return new FinanceBiz().GetMarketIndexBankExchangeList();
        }

        public ListModel<usp_wownet_Marketindex_worldExchange_Result> GetMarketIndexWorldExchangeList()
        {
            return new FinanceBiz().GetMarketIndexWorldExchangeList();
        }

        public usp_GetStockPrice_Result GetStockPrice(FinanceCondition condition)
        {
            return new FinanceBiz().GetStockPrice(condition);
        }

        public string GetHpFinderChart(HpCondition condition)
        {
            return new FinanceBiz().GetHpFinderChart(condition);
        }

        public string GetRankData(RankingCondition condition)
        {
            return new FinanceBiz().GetRankData(condition);
        }

        public string GetSabuScore(HpCondition condition)
        {
            return new FinanceBiz().GetSabuScore(condition);
        }

        public ListModel<usp_web_getStockInvestorList_Result> GetStockInvestorList(StockInvestorCondition condition)
        {
            return new FinanceBiz().GetStockInvestorList(condition);
        }

        public ListModel<usp_web_getStockInvestorSum_Result> GetStockInvestorSumList(StockInvestorCondition condition)
        {
            return new FinanceBiz().GetStockInvestorSumList(condition);
        }

        public ListModel<usp_web_getStockPart_Result> GetStockPartList(StockInvestorCondition condition)
        {
            return new FinanceBiz().GetStockPartList(condition);
        }

        public ListModel<usp_web_getStockThema_Result> GetStockThemaList(StockInvestorCondition condition)
        {
            return new FinanceBiz().GetStockThemaList(condition);
        }

        public ListModel<usp_web_getRelativeStock_Result> GetRelativeStockList(StockInvestorCondition condition)
        {
            return new FinanceBiz().GetRelativeStockList(condition);
        }

        public ListModel<ArticleStock> GetRecentNewsTop4List(ArticleStockCondition condition)
        {
            return new FinanceBiz().GetRecentNewsTop4List(condition);
        }

        public ListModel<usp_web_getNoticeTop6List_Result> GetNoticeTop6List(NoticeStockCondition condition)
        {
            return new FinanceBiz().GetNoticeTop6List(condition);
        }

        public ListModel<usp_web_getStockDiscussionTop9List_Result> GetDiscussionTop9List(DiscurssionCondition condition)
        {
            return new FinanceBiz().GetDiscussionTop9List(condition);
        }

        public ListModel<usp_web_getStockConsultTop9List_Result> GetConsultTop9List(ConsultCondition condition)
        {
            return new FinanceBiz().GetConsultTop9List(condition);
        }

        public ListModel<usp_web_select_stock_cafe_list_Result> GetStockCafeTop3List(StockCafeCondition condition)
        {
            return new FinanceBiz().GetStockCafeList(condition);
        }

        public ListModel<usp_web_vodTop6List_Result> GetVodTop6List(VodCondition condition)
        {
            return new FinanceBiz().GetVodTop6List(condition);
        }

        public ListModel<usp_web_listSubTopvod6_Result> GetVodInvestTop6List(VodInvestCondition condition)
        {
            return new FinanceBiz().GetVodInvestTop6List(condition);
        }

        public ListModel<usp_web_getUSAIndex_Result> GetUSAChartDataList(ChartCondition condition)
        {
            return new FinanceBiz().GetUSAChartDataList(condition);
        }

        public usp_LatestStockPrice_Result GetLatestStockPrice(CurrentAnalysisCondition condition)
        {
            return new FinanceBiz().GetLatestStockPrice(condition);
        }

        public usp_web_stock_hoga_Result GetStockHoga(CurrentAnalysisCondition condition)
        {
            return new FinanceBiz().GetStockHoga(condition);
        }

        public ListModel<usp_web_getStockDealing5_Result> GetStockDealing5List(CurrentAnalysisCondition condition)
        {
            return new FinanceBiz().GetStockDealing5List(condition);
        }

        public ListModel<usp_getDayStockPriceBand_Result> GetDayStockPriceBandList(CurrentAnalysisCondition condition)
        {
            return new FinanceBiz().GetDayStockPriceBandList(condition);
        }

        public ListModel<usp_SelectStockRecentlyTime_Result> GetSelectStockRecentlyTimeList(CurrentAnalysisCondition condition)
        {
            return new FinanceBiz().GetSelectStockRecentlyTimeList(condition);
        }

        public ListModel<usp_SelectStockRecentlyDay_Result> GetSelectStockRecentlyDayList(CurrentAnalysisCondition condition)
        {
            return new FinanceBiz().GetSelectStockRecentlyDayList(condition);
        }

        public ListModel<usp_web_getNowPriceTime_Result> GetStockNowPriceTimeList(CurrentAnalysisCondition condition)
        {
            return new FinanceBiz().GetStockNowPriceTimeList(condition);
        }

        public ListModel<usp_getDailyStockPricePop_Result> GetStockNowPriceDayList(CurrentAnalysisCondition condition)
        {
            return new FinanceBiz().GetStockNowPriceDayList(condition);
        }

        public ListModel<usp_web_getTradeTrend_Result> GetTradeTrendList(CurrentAnalysisCondition condition)
        {
            return new FinanceBiz().GetTradeTrendList(condition);
        }

        public ListModel<ArticleStock> GetStockNewsList(ArticleStockCondition condition)
        {
            return new FinanceBiz().GetStockNewsList(condition);
        }

        public ListModel<usp_web_getStockGongsi_Result> GetStockGongsiList(NoticeStockCondition condition)
        {
            return new FinanceBiz().GetStockGongsiList(condition);
        }

        public ListModel<tblMyFavoriteJongMok> GetMyFavoriteJongMokList(MyFavoriteStockCondition condition)
        {
            return new FinanceBiz().GetMyFavoriteJongMokList(condition);
        }

        public ListModel<usp_web_getStockGongsiView_Result> GetStockGongsiViewList(NoticeStockCondition condition)
        {
            return new FinanceBiz().GetStockGongsiView(condition);
        }

        public bool GetCheckStockHoliday(HolidayCondition condition)
        {
            return new FinanceBiz().GetCheckStockHoliday(condition);
        }

        public ListModel<usp_web_vodList_Result> GetVodList(VodCondition condition)
        {
            return new FinanceBiz().GetVodList(condition);
        }

        public ListModel<usp_web_getInvestVodList_Result> GetInvestVodList(VodInvestCondition condition)
        {
            return new FinanceBiz().GetInvestVodList(condition);
        }

        public ListModel<usp_web_getExchangeTimeList_Result> GetExchangeTimeList(ExchangeCondition condition)
        {
            return new FinanceBiz().GetExchangeTimeList(condition);
        }

        public ListModel<usp_getSiseMainmergecode_Result> GetSiseMainmergecode()
        {
            return new FinanceBiz().GetSiseMainmergecode();
        }

        /// <summary>
        /// 가상화폐
        /// </summary>
        /// <returns></returns>
        public List<usp_web_getVirtualMoney_Result> GetVirtualMoney()
        {
            return new FinanceBiz().GetVirtualMoney();
        }

        public ListModel<NUP_NEWS_SECTION_SELECT_Result> GetTodayStockNews()
        {
            return new FinanceBiz().GetTodayStockNews();
        }

      public List<usp_web_clfSelectOnlineIndex_Result> GetClfSelectOnlineIndex()
      {
          return new FinanceBiz().GetClfSelectOnlineIndex();
      }

      public List<usp_web_clfSelectOnlineItem_Result> GetClfSelectOnlineItem()
      {
          return new FinanceBiz().GetClfSelectOnlineItem();
      }

      public List<usp_web_clfSelectHoliday_Result> GetClfSelectHoliday(SiseHolyCondition condition)
      {
          return new FinanceBiz().GetClfSelectHoliday(condition);
      }
                
        public List<TAB_STRATEGY_APPLICATION> GetTodayInvests()
        {
            return new FinanceBiz().GetTodayInvests();
        }

        public List<t_part> GetKospiIndustryPartList()
        {
            return new FinanceBiz().GetKospiIndustryPartList();
        }

        public List<t_kosdaq_part> GetKosdaqIndustryPartList()
        {
            return new FinanceBiz().GetKosdaqIndustryPartList();
        }

        public List<FNC_GetThemaJisuTop_Result> GetThemaJisuTopList()
        {
            return new FinanceBiz().GetThemaJisuTopList();
        }

        public ListModel<NUP_NEWS_SECTION_SELECT_Result> GetWorldStockNews()
        {
            return new FinanceBiz().GetWorldStockNews();
        }

        public ListModel<usp_web_getBestProHintStocking_Result> GetBestProHintStockingList()
        {
            return new FinanceBiz().GetBestProHintStockingList();
        }

        public ListModel<usp_web_getWorldProHintPartner_Result> GetWorldProHindtPartnerList()
        {
            return new FinanceBiz().GetWorldProHindtPartnerList();
        }

        public ListModel<usp_web_getCurrentSearchStockList_Result> GetCurrentSearchStockList()
        {
            return new FinanceBiz().GetCurrentSearchStockList();
        }

        public ListModel<usp_web_getAsiaIndex_Result> GetAsiaChartDataList(ChartCondition condition)
        {
            return new FinanceBiz().GetAsiaChartDataList(condition);
        }
        

        //public ListModel<NUP_NEWS_SECTION_SELECT_Result> GetCharacterStockNews()
        //{
        //    return new FinanceBiz().GetCharacterStockNews();
        //}

        //public NSP_BANNER_RANDOM_Result GetADBannner(ADBannnerCondition condition)
        //{
        //    return new FinanceBiz().GetADBanner(condition);
        //}
    }
}
