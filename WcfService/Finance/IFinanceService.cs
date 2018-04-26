using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
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
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IFinanceService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IFinanceService
    {
        [OperationContract]
        ListModel<usp_web_getDomesticIndex_Result> GetSiseChartDataList(ChartCondition condition);

        [OperationContract]
        ListModel<usp_getSiseTimeKosdaqChartData_new_Result> GetSiseTimeKosdaqChartDataList();

        [OperationContract]
        ListModel<usp_getSiseTimeFutChartData_new_Result> GetSiseTimeFutChartDataList();

        [OperationContract]
        ListModel<usp_getSiseTimeKpi200ChartData_new_Result> GetSiseTimeKpi200ChartDataList();

        [OperationContract]
        ListModel<usp_getSiseTimeKospi_new_Result> GetSiseTimeKospiList(FinanceCondition condition);

        [OperationContract]
        ListModel<usp_getSiseDayKospi_new_Result> GetSiseDayKospiList(FinanceCondition condition);

        [OperationContract]
        ListModel<usp_getSiseDayKosdaq_new_Result> GetSiseDayKosdaqList(FinanceCondition condition);

        [OperationContract]
        ListModel<usp_getSiseTimeKosdaq_new_Result> GetSiseTimeKosdaqList(FinanceCondition condition);

        [OperationContract]
        ListModel<usp_getSiseTimeKpi200_new_Result> GetSiseTimeKpi200List(FinanceCondition condition);

        [OperationContract]
        ListModel<usp_getSiseDayKpi200_new_Result> GetSiseDayKpi200List(FinanceCondition condition);

        [OperationContract]
        ListModel<usp_getSiseDayFut_new_Result> GetSiseDayFutList(FinanceCondition condition);

        [OperationContract]
        ListModel<usp_getSiseTimeFut_new_Result> GetSiseTimeFutList(FinanceCondition condition);

        [OperationContract]
        ListModel<usp_wownet_kpi200_stock_group_new_Result> GetWowNetKPI200StockGroupList(FinanceCondition condition);

        [OperationContract]
        StockInfoModel<usp_GetSiseCurrentKospi_Result, usp_getSiseMainmergecode_Result> GetSiseKospiInfo();

        [OperationContract]
        StockInfoModel<usp_GetSiseCurrentKosdaq_Result, usp_getSiseMainmergecode_Result> GetSiseKosdaqInfo();

        [OperationContract]
        StockInfoModel<usp_GetSiseCurrent_Result, usp_getSiseMainmergecode_Result> GetSiseFutInfo();

        [OperationContract]
        StockInfoModel<usp_GetSiseCurrentKPI_Result, usp_getSiseMainmergecode_Result> GetSiseKPIInfo();

        [OperationContract]
        ListModel<usp_wownet_ETF_stock_group_Result> GetWowNetETFStockGroupList();

        [OperationContract]
        ListModel<usp_GetIndustryState_Result> GetIndustryStateList(IndustryCondition condition);

        [OperationContract]
        IndustryInfoModel GetIndustryInfo(IndustryCondition condition);

        [OperationContract]
        ListModel<MarketResearchModel> GetIndustryMarketResearchListTop3(IndustryCondition condition);

        [OperationContract]
        ListModel<usp_web_getDomesticIndustryStock_Result> GetIndustryStockList(IndustryCondition condition);

        [OperationContract]
        ListModel<usp_GetThemaJisu_Result> GetThemaJisuList();

        [OperationContract]
        ListModel<usp_GetThemaOnline_Result> GetThemaOnlineList(ThemaCondition condition);

        [OperationContract]
        ListModel<usp_web_getSiseUpper_Result> GetSiseUpperList(UpperCondition condition);

        [OperationContract]
        ListModel<usp_web_getSiseLower_Result> GetSiseLowerList(LowerCondition condition);

        [OperationContract]
        ListModel<usp_web_GetSiseStockPlus_Result> GetSiseStockPlusList();

        [OperationContract]
        ListModel<usp_web_GetSiseStockPlusK_Result> GetSiseStockPlusKList();

        [OperationContract]
        ListModel<usp_web_getSiseMarketSum_Result1> GetSiseMarketSumList(MarketCondition condition);

        [OperationContract]
        ListModel<usp_web_getSiseForeignHold_Result> GetSiseForeignHoldList(MarketCondition condition);

        [OperationContract]
        usp_wownet_usa_index_online_Result GetUsaIndexOnline(MarketCondition condition);

        [OperationContract]
        ListModel<usp_wownet_usa_index_hour_Result> GetUsaIndexHourList(MarketCondition condition);

        [OperationContract]
        ListModel<usp_wownet_usa_index_history_Result> GetUsaIndexHistoryList(MarketCondition condition);

        [OperationContract]
        ListModel<usp_web_usa_stock_group_Result> GetUsaStockGroupList(MarketCondition condition);

        [OperationContract]
        usp_wownet_Asia_Index_online_Result GetAsiaIndexOnline(MarketCondition condition);

        [OperationContract]
        ListModel<usp_wownet_Asia_Index_hour_Result> GetAsiaIndexHourList(MarketCondition condition);

        [OperationContract]
        ListModel<usp_wownet_Asia_Index_history_Result> GetAsiaIndexHistoryList(MarketCondition condition);

        [OperationContract]
        ListModel<usp_web_korea_adr_Result> GetKoreaAdrList();

        [OperationContract]
        usp_wownet_Europe_Index_online_Result GetEuropeIndexOnline(MarketCondition condition);

        [OperationContract]
        ListModel<usp_wownet_Europe_Index_history_Result> GetEuropeIndexHistoryList(MarketCondition condition);

        [OperationContract]
        ListModel<usp_GetInvestOnline_Result> GetInvestOnlineList();

        [OperationContract]
        ListModel<usp_GetInvestTotal_Result> GetInvestTotalList(InvestCondition condition);

        [OperationContract]
        ListModel<usp_web_usa_adr_Result> GetUSAAdrTotalList();

        [OperationContract]
        ListModel<USP_WOWNET_USA_DETAIL_Result> GetUSAAdrDetailList(UsaIndustryCondition condition);

        [OperationContract]
        ListModel<usp_web_usa_adr_dawo_semiconductor_Result> GetUSAADRDawoSemiconductorList();

        [OperationContract]
        ListModel<usp_web_usa_adr_dawo_media_Result> GetUSAADRDawoMediaList();

        [OperationContract]
        ListModel<usp_web_usa_adr_dawo_insurance_Result> GetUSAADRDawoInsuranceList();

        [OperationContract]
        ListModel<usp_web_usa_adr_dawo_retail_Result> GetUSAADRDawoRetailList();

        [OperationContract]
        ListModel<usp_web_usa_adr_dawo_energy_Result> GetUSAADRDawoEnergyList();

        [OperationContract]
        ListModel<usp_web_usa_adr_dawo_pharmacy_Result> GetUSAADRDawoPharmacyList();

        [OperationContract]
        ListModel<usp_web_usa_adr_nasdaq_internet_Result> GetUSAADRNasdaqInternetList();

        [OperationContract]
        ListModel<usp_web_usa_adr_nasdaq_network_Result> GetUSAADRNasdaqNetworkList();

        [OperationContract]
        ListModel<usp_web_usa_adr_nasdaq_comunications_Result> GetUSAADRNasdaqComunicationsList();

        [OperationContract]
        ListModel<usp_web_usa_adr_nasdaq_computer_Result> GetUSAADRNasdaqComputerList();

        [OperationContract]
        ListModel<usp_web_usa_adr_nasdaq_semiconductor_Result> GetUSAADRNasdaqSemiconductorList();

        [OperationContract]
        ListModel<usp_web_usa_adr_nasdaq_bio_Result> GetUSAADRNasdaqBioList();

        [OperationContract]
        ListModel<usp_web_usa_adr_sandp500_internet_Result> GetUSAADRSANDP500InternetList();

        [OperationContract]
        ListModel<usp_wownet_Marketindex_BankExchange_Result> GetMarketIndexBankExchangeList();

        [OperationContract]
        ListModel<usp_wownet_Marketindex_worldExchange_Result> GetMarketIndexWorldExchangeList();

        [OperationContract]
        usp_GetStockPrice_Result GetStockPrice(FinanceCondition condition);

        [OperationContract]
        string GetHpFinderChart(HpCondition condition);

        [OperationContract]
        string GetRankData(RankingCondition condition);

        [OperationContract]
        string GetSabuScore(HpCondition condition);

        [OperationContract]
        ListModel<usp_web_getStockInvestorList_Result> GetStockInvestorList(StockInvestorCondition condition);

        [OperationContract]
        ListModel<usp_web_getStockInvestorSum_Result> GetStockInvestorSumList(StockInvestorCondition condition);

        [OperationContract]
        ListModel<usp_web_getStockPart_Result> GetStockPartList(StockInvestorCondition condition);

        [OperationContract]
        ListModel<usp_web_getStockThema_Result> GetStockThemaList(StockInvestorCondition condition);

        [OperationContract]
        ListModel<usp_web_getRelativeStock_Result> GetRelativeStockList(StockInvestorCondition condition);

        [OperationContract]
        ListModel<ArticleStock> GetRecentNewsTop4List(ArticleStockCondition condition);

        //[OperationContract]
        //NSP_BANNER_RANDOM_Result GetADBannner(ADBannnerCondition condition);

        [OperationContract]
        ListModel<usp_web_getNoticeTop6List_Result> GetNoticeTop6List(NoticeStockCondition condition);

        [OperationContract]
        ListModel<usp_web_getStockDiscussionTop9List_Result> GetDiscussionTop9List(DiscurssionCondition condiition);

        [OperationContract]
        ListModel<usp_web_getStockConsultTop9List_Result> GetConsultTop9List(ConsultCondition condition);

        [OperationContract]
        ListModel<usp_web_select_stock_cafe_list_Result> GetStockCafeTop3List(StockCafeCondition condition);

        [OperationContract]
        ListModel<usp_web_vodTop6List_Result> GetVodTop6List(VodCondition condition);

        [OperationContract]
        ListModel<usp_web_listSubTopvod6_Result> GetVodInvestTop6List(VodInvestCondition condition);

        [OperationContract]
        ListModel<usp_web_getUSAIndex_Result> GetUSAChartDataList(ChartCondition condition);

        [OperationContract]
        ListModel<usp_web_getAsiaIndex_Result> GetAsiaChartDataList(ChartCondition condition);

        [OperationContract]
        usp_LatestStockPrice_Result GetLatestStockPrice(CurrentAnalysisCondition condition);

        [OperationContract]
        usp_web_stock_hoga_Result GetStockHoga(CurrentAnalysisCondition condition);

        [OperationContract]
        ListModel<usp_web_getStockDealing5_Result> GetStockDealing5List(CurrentAnalysisCondition condition);

        [OperationContract]
        ListModel<usp_getDayStockPriceBand_Result> GetDayStockPriceBandList(CurrentAnalysisCondition condition);

        [OperationContract]
        ListModel<usp_SelectStockRecentlyTime_Result> GetSelectStockRecentlyTimeList(CurrentAnalysisCondition condition);

        [OperationContract]
        ListModel<usp_SelectStockRecentlyDay_Result> GetSelectStockRecentlyDayList(CurrentAnalysisCondition condition);

        [OperationContract]
        ListModel<usp_web_getNowPriceTime_Result> GetStockNowPriceTimeList(CurrentAnalysisCondition condition);

        [OperationContract]
        ListModel<usp_getDailyStockPricePop_Result> GetStockNowPriceDayList(CurrentAnalysisCondition condition);

        [OperationContract]
        ListModel<usp_web_getTradeTrend_Result> GetTradeTrendList(CurrentAnalysisCondition condition);

        [OperationContract]
        ListModel<ArticleStock> GetStockNewsList(ArticleStockCondition condition);

        [OperationContract]
        ListModel<usp_web_getStockGongsi_Result> GetStockGongsiList(NoticeStockCondition condition);

        [OperationContract]
        ListModel<tblMyFavoriteJongMok> GetMyFavoriteJongMokList(MyFavoriteStockCondition condition);

        [OperationContract]
        ListModel<usp_web_getStockGongsiView_Result> GetStockGongsiViewList(NoticeStockCondition condition);

        [OperationContract]
        bool GetCheckStockHoliday(HolidayCondition condition);

        [OperationContract]
        ListModel<usp_web_vodList_Result> GetVodList(VodCondition condition);

        [OperationContract]
        ListModel<usp_web_getInvestVodList_Result> GetInvestVodList(VodInvestCondition condition);

        [OperationContract]
        ListModel<usp_web_getExchangeTimeList_Result> GetExchangeTimeList(ExchangeCondition condition);

        [OperationContract]
        ListModel<usp_getSiseMainmergecode_Result> GetSiseMainmergecode();

        /// <summary>
        /// 가상화폐
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<usp_web_getVirtualMoney_Result> GetVirtualMoney();

        [OperationContract]
        ListModel<NUP_NEWS_SECTION_SELECT_Result> GetTodayStockNews();

        [OperationContract]
        List<usp_web_clfSelectOnlineIndex_Result> GetClfSelectOnlineIndex();

        [OperationContract]
        List<usp_web_clfSelectOnlineItem_Result> GetClfSelectOnlineItem();

        [OperationContract]
        List<usp_web_clfSelectHoliday_Result> GetClfSelectHoliday(SiseHolyCondition condition);

        [OperationContract]
        List<TAB_STRATEGY_APPLICATION> GetTodayInvests();

        [OperationContract]
        List<t_part> GetKospiIndustryPartList();

        [OperationContract]
        List<t_kosdaq_part> GetKosdaqIndustryPartList();

        [OperationContract]
        List<FNC_GetThemaJisuTop_Result> GetThemaJisuTopList();

        [OperationContract]
        ListModel<NUP_NEWS_SECTION_SELECT_Result> GetWorldStockNews();

        [OperationContract]
        ListModel<usp_web_getBestProHintStocking_Result> GetBestProHintStockingList();

        [OperationContract]
        ListModel<usp_web_getWorldProHintPartner_Result> GetWorldProHindtPartnerList();

        [OperationContract]
        ListModel<usp_web_getCurrentSearchStockList_Result> GetCurrentSearchStockList();

        //[OperationContract]
        //ListModel<NUP_NEWS_SECTION_SELECT_Result> GetCharacterStockNews();
    }
}
