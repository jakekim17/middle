using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.Finance;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.Finance;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.Finance;
using Wow.Tv.Middle.Model.Db49.wowcafe;
using Wow.Tv.Middle.Model.Db49.wowcafe.Finance;
using Wow.Tv.Middle.Model.Db49.editVOD;
using Wow.Tv.Middle.Model.Db49.editVOD.Stock;
using Wow.Tv.Middle.Model.Db22.stock.MyPage;
using System.Data.Objects.SqlClient;
using Wow.Tv.Middle.Model.Db49.broadcast;

namespace Wow.Tv.Middle.Biz.Finance
{
    public class FinanceBiz : BaseBiz
    {
        /*hpFinder 예측차트*/
        public string GetHpFinderChart(HpCondition condition)
        {
            string result = "";
            string strQuery = "wowsearch.asp?trid=" + condition.Trid + "&code=" + condition.Code + "&width=" + condition.Width + "&height=" + condition.Height + "&toplogo=1";
            hpFinder2.FinderClass hpFinder = new hpFinder2.FinderClass();
            hpFinder.MaxDelay = 3;
            bool connStatus = hpFinder.Connect1("125.141.231.11", Convert.ToInt32(GetRandomsPort()));
            if(connStatus != true)
            {
                connStatus = hpFinder.Connect1("125.141.231.11", Convert.ToInt32(GetRandomsPort()));
            }
            
            result = hpFinder.GetData1(strQuery);
            
            hpFinder.close1();
            return result;
        }

        /*hpFinder 사부점수*/
        public string GetSabuScore(HpCondition condition)
        {
            string result = "";
            string strQuery = "wowsearch.asp?trid=" + condition.Trid + "&code=" + condition.Code + "";
            hpFinder2.FinderClass hpFinderObj = new hpFinder2.FinderClass();
            hpFinderObj.MaxDelay = 3;

            bool connStatus = hpFinderObj.Connect1("125.141.231.11", Convert.ToInt32(GetRandomsPort()));
            if (connStatus != true)
            {
                connStatus = hpFinderObj.Connect1("125.141.231.11", Convert.ToInt32(GetRandomsPort()));
            }
            result = hpFinderObj.GetData1(strQuery);
            hpFinderObj.close1();
            return result;
        }

        /*hpFinder 예측차트 랜덤포트 생성*/
        private string GetRandomsPort()
        {
            Random random = new Random();
            int num = random.Next(0, 10);
            string resultFort = "808" + num.ToString();
            return resultFort;
        }

        /*hpFinder 랭크데이터*/
        public string GetRankData(RankingCondition condition)
        {
            string result = "";
            hpFinder.FinderClass hpFinder = new hpFinder.FinderClass();
            hpFinder.MaxDelay = 3;
            bool connStatus = hpFinder.Connect1("125.141.231.11", 8090);
            if (connStatus != true)
            {
                connStatus = hpFinder.Connect1("125.141.231.11", 8090);
            }
            string strQuery = "wowsearch.asp?trid="+ condition.TrId+ "&Time="+ condition.RnkTime+ "&Sect=" + condition.Sect + "&Count="+condition.Count;
            result = hpFinder.GetData1(strQuery);

            hpFinder.close1();
            return result;
        }

        /*국내증시 > 코스피 정보*/
        public StockInfoModel<usp_GetSiseCurrentKospi_Result, usp_getSiseMainmergecode_Result> GetSiseKospiInfo()
        {
            StockInfoModel<usp_GetSiseCurrentKospi_Result, usp_getSiseMainmergecode_Result> stockInfoModel = new StockInfoModel<usp_GetSiseCurrentKospi_Result, usp_getSiseMainmergecode_Result>();
            stockInfoModel.StockInfo = db22_stock.usp_GetSiseCurrentKospi().FirstOrDefault();
            stockInfoModel.ThemaInfo = db22_stock.usp_getSiseMainmergecode().Where(m => m.Market == "코스피").FirstOrDefault();
            return stockInfoModel;
        }

        /*국내증시 > 코스닥 정보*/
        public StockInfoModel<usp_GetSiseCurrentKosdaq_Result, usp_getSiseMainmergecode_Result> GetSiseKosdaqInfo()
        {
            StockInfoModel<usp_GetSiseCurrentKosdaq_Result, usp_getSiseMainmergecode_Result> stockInfoModel = new StockInfoModel<usp_GetSiseCurrentKosdaq_Result, usp_getSiseMainmergecode_Result>();
            stockInfoModel.StockInfo = db22_stock.usp_GetSiseCurrentKosdaq().FirstOrDefault();
            stockInfoModel.ThemaInfo = db22_stock.usp_getSiseMainmergecode().Where(m => m.Market == "코스닥").FirstOrDefault();
            return stockInfoModel;
        }

        /*국내증시 > 선물 정보*/
        public StockInfoModel<usp_GetSiseCurrent_Result, usp_getSiseMainmergecode_Result> GetSiseFutInfo()
        {
            StockInfoModel<usp_GetSiseCurrent_Result, usp_getSiseMainmergecode_Result> stockInfoModel = new StockInfoModel<usp_GetSiseCurrent_Result, usp_getSiseMainmergecode_Result> {
                StockInfo = db22_stock.usp_GetSiseCurrent().FirstOrDefault(),
                ThemaInfo = db22_stock.usp_getSiseMainmergecode().Where(m => m.Market == "선물").FirstOrDefault()
            };
            return stockInfoModel;
        }

        /*국내증시 > 코스피200 정보*/
        public StockInfoModel<usp_GetSiseCurrentKPI_Result, usp_getSiseMainmergecode_Result> GetSiseKPIInfo()
        {
            StockInfoModel<usp_GetSiseCurrentKPI_Result, usp_getSiseMainmergecode_Result> stockInfoModel = new StockInfoModel<usp_GetSiseCurrentKPI_Result, usp_getSiseMainmergecode_Result>
            {
                StockInfo = db22_stock.usp_GetSiseCurrentKPI().FirstOrDefault()
            };
            return stockInfoModel;
        }

        /*국내증시 > 메인 > 주체별 동향*/
        public ListModel<usp_getSiseMainmergecode_Result> GetSiseMainmergecode()
        {
            ListModel<usp_getSiseMainmergecode_Result> resultData = new ListModel<usp_getSiseMainmergecode_Result> { ListData = db22_stock.usp_getSiseMainmergecode().ToList() };
            return resultData;
        }

        /*국내증시 > 차트데이터*/
        public ListModel<usp_web_getDomesticIndex_Result> GetSiseChartDataList(ChartCondition condition)
        {
            return new ListModel<usp_web_getDomesticIndex_Result> { ListData = db22_stock.usp_web_getDomesticIndex(condition.Class_1, condition.Class_2).ToList() };
        }

        /*해외증시 > 미국 > 차트데이터*/
        public ListModel<usp_web_getUSAIndex_Result> GetUSAChartDataList(ChartCondition condition)
        {
            return new ListModel<usp_web_getUSAIndex_Result> { ListData = db22_stock.usp_web_getUSAIndex(condition.Class_1, condition.Class_2).ToList() };
        }

        /*해외증시 > 아시아 > 차트데이터*/
        public ListModel<usp_web_getAsiaIndex_Result> GetAsiaChartDataList(ChartCondition condition)
        {
            return new ListModel<usp_web_getAsiaIndex_Result> { ListData = db22_stock.usp_web_getAsiaIndex(condition.Class_1, condition.Class_2).ToList() };
        }

        /* 차트 데이터 변경 후 아래삭제 예정 */
        public ListModel<usp_getSiseTimeKosdaqChartData_new_Result> GetSiseTimeKosdaqChartDataList()
        {
            return new ListModel<usp_getSiseTimeKosdaqChartData_new_Result> { ListData = db22_stock.usp_getSiseTimeKosdaqChartData_new().ToList() };
        }

        public ListModel<usp_getSiseTimeKpi200ChartData_new_Result> GetSiseTimeKpi200ChartDataList()
        {
            return new ListModel<usp_getSiseTimeKpi200ChartData_new_Result> { ListData = db22_stock.usp_getSiseTimeKpi200ChartData_new().ToList() };
        }

        public ListModel<usp_getSiseTimeFutChartData_new_Result> GetSiseTimeFutChartDataList()
        {
            return new ListModel<usp_getSiseTimeFutChartData_new_Result> { ListData = db22_stock.usp_getSiseTimeFutChartData_new().ToList() };
        }
        /* 여기까지 */

        /* 국내증시 > 시간대별 코스피 데이터*/
        public ListModel<usp_getSiseTimeKospi_new_Result> GetSiseTimeKospiList(FinanceCondition condition)
        {
            //ListModel<usp_getSiseTimeKospi_new_Result> resultData = new ListModel<usp_getSiseTimeKospi_new_Result> { ListData = db22_stock.usp_getSiseTimeKospi_new().ToList() };
            ListModel<usp_getSiseTimeKospi_new_Result> resultData = new ListModel<usp_getSiseTimeKospi_new_Result>();

            var list = db22_stock.usp_getSiseTimeKospi_new().ToList();

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        /* 종목상세 > 단일종목 시간대별 데이터*/
        public ListModel<usp_web_getNowPriceTime_Result> GetStockNowPriceTimeList(CurrentAnalysisCondition condition)
        {
            ListModel<usp_web_getNowPriceTime_Result> resultData = new ListModel<usp_web_getNowPriceTime_Result>();

            var list = db22_stock.usp_web_getNowPriceTime(condition.StockCode).ToList();

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        /*종목상세 > 단일종목 일자별 데이터*/
        public ListModel<usp_getDailyStockPricePop_Result> GetStockNowPriceDayList(CurrentAnalysisCondition condition)
        {
            ListModel<usp_getDailyStockPricePop_Result> resultData = new ListModel<usp_getDailyStockPricePop_Result>();

            var list = db22_stock.usp_getDailyStockPricePop(condition.StockCode).ToList();

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        /*국내증시 > 코스닥 시간대별 데이터*/
        public ListModel<usp_getSiseTimeKosdaq_new_Result> GetSiseTimeKosdaqList(FinanceCondition condition)
        {
            ListModel<usp_getSiseTimeKosdaq_new_Result> resultData = new ListModel<usp_getSiseTimeKosdaq_new_Result>();

            var list = db22_stock.usp_getSiseTimeKosdaq_new().ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        /*국내증시 > 선물 시간대별 데이터*/
        public ListModel<usp_getSiseTimeFut_new_Result> GetSiseTimeFutList(FinanceCondition condition)
        {
            ListModel<usp_getSiseTimeFut_new_Result> resultData = new ListModel<usp_getSiseTimeFut_new_Result>();

            var list = db22_stock.usp_getSiseTimeFut_new().ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        /*국내증시 > 코스피200 시간대별 데이터*/
        public ListModel<usp_getSiseTimeKpi200_new_Result> GetSiseTimeKpi200List(FinanceCondition condition)
        {
            ListModel<usp_getSiseTimeKpi200_new_Result> resultData = new ListModel<usp_getSiseTimeKpi200_new_Result>();

            var list = db22_stock.usp_getSiseTimeKpi200_new().ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        /*국내증시 > 코스피 일별 데이터*/
        public ListModel<usp_getSiseDayKospi_new_Result> GetSiseDayKospiList(FinanceCondition condition)
        {
            ListModel<usp_getSiseDayKospi_new_Result> resultData = new ListModel<usp_getSiseDayKospi_new_Result>();

            var list = db22_stock.usp_getSiseDayKospi_new().ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        /*국내증시 > 코스닥 일별 데이터*/
        public ListModel<usp_getSiseDayKosdaq_new_Result> GetSiseDayKosdaqList(FinanceCondition condition)
        {
            ListModel<usp_getSiseDayKosdaq_new_Result> resultData = new ListModel<usp_getSiseDayKosdaq_new_Result>();

            var list = db22_stock.usp_getSiseDayKosdaq_new().ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }
            resultData.ListData = list;

            return resultData;
        }

        /*국내증시 > 선물 일별 데이터*/
        public ListModel<usp_getSiseDayFut_new_Result> GetSiseDayFutList(FinanceCondition condition)
        {
            ListModel<usp_getSiseDayFut_new_Result> resultData = new ListModel<usp_getSiseDayFut_new_Result>();

            var list = db22_stock.usp_getSiseDayFut_new().ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        /*국내증시 > 코스피200 일별 데이터*/
        public ListModel<usp_getSiseDayKpi200_new_Result> GetSiseDayKpi200List(FinanceCondition condition)
        {
            ListModel<usp_getSiseDayKpi200_new_Result> resultData = new ListModel<usp_getSiseDayKpi200_new_Result>();

            var list = db22_stock.usp_getSiseDayKpi200_new().ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        public ListModel<usp_wownet_kpi200_stock_group_new_Result> GetWowNetKPI200StockGroupList(FinanceCondition condition)
        {
            ListModel<usp_wownet_kpi200_stock_group_new_Result> resultData = new ListModel<usp_wownet_kpi200_stock_group_new_Result>();

            var list = db22_stock.usp_wownet_kpi200_stock_group_new().ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        /// <summary>
        /// ETF Group List
        /// </summary>
        /// <returns></returns>
        public ListModel<usp_wownet_ETF_stock_group_Result> GetWowNetETFStockGroupList()
        {
            return new ListModel<usp_wownet_ETF_stock_group_Result> { ListData = db22_stock.usp_wownet_ETF_stock_group().ToList() };
        }

        public ListModel<usp_GetIndustryState_Result> GetIndustryStateList(IndustryCondition condition)
        {
            return new ListModel<usp_GetIndustryState_Result> { ListData = db22_stock.usp_GetIndustryState(condition.Market, condition.OrderBy, condition.TargetDT).ToList() };
        }

        public usp_web_getDomesticIndustryIndex_Result GetIndustryIndex(IndustryCondition condition)
        {
            IndustryCondition new_condition = IndustryConditionChange(condition);

            return db22_stock.usp_web_getDomesticIndustryIndex(condition.Market, new_condition.PartCode).FirstOrDefault();
        }

        public IndustryCondition IndustryConditionChange(IndustryCondition condition)
        {
            
           IndustryCondition new_condition = new IndustryCondition { PartCode = condition.PartCode };

            switch (new_condition.PartCode.Length)
            {
                case 1:
                    new_condition.PartCode = "00" + new_condition.PartCode;
                    break;
                case 2:
                    new_condition.PartCode = "0" + new_condition.PartCode;
                    break;
                default:
                    break;
            }
           
            return new_condition;
        }

        public ListModel<usp_GetIndustryDetail_Result> GetIndustryDetailList(IndustryCondition condition)
        {
            return new ListModel<usp_GetIndustryDetail_Result> { ListData = db22_stock.usp_GetIndustryDetail(condition.Market, condition.PartCode).ToList() };
        }

        public IndustryInfoModel GetIndustryInfo(IndustryCondition condition)
        {
            return new IndustryInfoModel
            {
                //StockInfo = db22_stock.usp_GetIndustryIndex(condition.Market, condition.PartCode).FirstOrDefault(),
                //IndustryStateList = new ListModel<usp_GetIndustryState_Result> { ListData = db22_stock.usp_GetIndustryState(condition.Market, condition.OrderBy, condition.TargetDT).ToList() },
                //DetailList = new ListModel<usp_GetIndustryDetail_Result> { ListData = db22_stock.usp_GetIndustryDetail(condition.Market, condition.PartCode).ToList() }
                StockInfo = GetIndustryIndex(condition),
                KospiIndustryPart = GetKospiIndustryPartList(),
                KosdaqIndustryPart = GetKosdaqIndustryPartList(),
                DetailList = GetIndustryDetailList(condition),
                MarketSearchTop3List = GetIndustryMarketResearchListTop3(condition)
            };
        }

        public List<t_part> GetKospiIndustryPartList()
        {
            List<t_part> list = new List<t_part>();
            list = db22_stock.t_part.Where(m =>  m.f_part_code.CompareTo("05") >= 0).Where(m => m.f_part_code.CompareTo("27") <= 0).OrderBy(m => m.f_part_code).ToList();
            return list;
        }

        public List<t_kosdaq_part> GetKosdaqIndustryPartList()
        {
            List<t_kosdaq_part> list = new List<t_kosdaq_part>();
            list = db22_stock.t_kosdaq_part.Where(m => m.web_flag == true).OrderBy(m => m.k_part_code).ToList();
            return list;
        }

        public ListModel<MarketResearchModel> GetIndustryMarketResearchListTop3(IndustryCondition condition)
        {
            var resultKosdaqList = (
                                from tblMarketResearch in db22_stock.TAB_MARKET_RESEARCH
                                join tblTKosdaqPart in db22_stock.t_kosdaq_part on tblMarketResearch.BUSINESS equals tblTKosdaqPart.k_part_code
                                where
                                tblMarketResearch.VIEW_FLAG == "Y"
                                && tblMarketResearch.BUSINESS == condition.PartCode
                                && tblMarketResearch.BCODE == "N01060200"
                                && tblMarketResearch.GUBUN == "2"
                                orderby tblMarketResearch.REG_DATE descending
                                select new MarketResearchModel
                                {
                                    SEQ = tblMarketResearch.SEQ,
                                    BCODE = tblMarketResearch.BCODE,
                                    TITLE = tblMarketResearch.TITLE,
                                    REF = tblMarketResearch.REF,
                                    REF_LEVEL = tblMarketResearch.REF_LEVEL,
                                    REF_STEP = tblMarketResearch.REF_STEP,
                                    REG_DATE = tblMarketResearch.REG_DATE,
                                    MarketName = tblTKosdaqPart.k_part_name
                                }
                             ).Take(3).ToList();
            var resultKospiList = (
                                from tblMarketResearch in db22_stock.TAB_MARKET_RESEARCH
                                join tblTPart in db22_stock.t_part on tblMarketResearch.BUSINESS equals tblTPart.f_part_code
                                where
                                tblMarketResearch.VIEW_FLAG == "Y"
                                && tblMarketResearch.BUSINESS == condition.PartCode
                                && tblMarketResearch.BCODE == "N01060200"
                                && tblMarketResearch.GUBUN == "1"
                                orderby tblMarketResearch.REG_DATE descending
                                select new MarketResearchModel
                                {
                                    SEQ = tblMarketResearch.SEQ,
                                    BCODE = tblMarketResearch.BCODE,
                                    TITLE = tblMarketResearch.TITLE,
                                    REF = tblMarketResearch.REF,
                                    REF_LEVEL = tblMarketResearch.REF_LEVEL,
                                    REF_STEP = tblMarketResearch.REF_STEP,
                                    REG_DATE = tblMarketResearch.REG_DATE,
                                    MarketName = tblTPart.F_part_name
                                }
                             ).Take(3).ToList();
                             //).Take(3).ToList();

            return new ListModel<MarketResearchModel> { ListData = resultKospiList.Union(resultKosdaqList).ToList() };
        } 

        public ListModel<usp_web_getDomesticIndustryStock_Result> GetIndustryStockList(IndustryCondition condition)
        {
            ListModel<usp_web_getDomesticIndustryStock_Result> resultData = new ListModel<usp_web_getDomesticIndustryStock_Result>();

            IndustryCondition new_condition = IndustryConditionChange(condition);

            var list = db22_stock.usp_web_getDomesticIndustryStock(condition.Market, new_condition.PartCode).ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 10;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        public ListModel<usp_GetThemaJisu_Result> GetThemaJisuList()
        {
            return new ListModel<usp_GetThemaJisu_Result> { ListData = db22_stock.usp_GetThemaJisu().ToList() };
        }

        public ListModel<usp_GetThemaOnline_Result> GetThemaOnlineList(ThemaCondition condition)
        {
            return new ListModel<usp_GetThemaOnline_Result> { ListData = db22_stock.usp_GetThemaOnline(condition.PartNum).ToList() };
        }

        public ListModel<usp_web_getSiseUpper_Result> GetSiseUpperList(UpperCondition condition)
        {
            return new ListModel<usp_web_getSiseUpper_Result> { ListData = db22_stock.usp_web_getSiseUpper(condition.Market).ToList() };
        }

        public ListModel<usp_web_getSiseLower_Result> GetSiseLowerList(LowerCondition condition)
        {
            return new ListModel<usp_web_getSiseLower_Result> { ListData = db22_stock.usp_web_getSiseLower(condition.Market).ToList() };
        }

        public ListModel<usp_web_GetSiseStockPlus_Result> GetSiseStockPlusList()
        {
            return new ListModel<usp_web_GetSiseStockPlus_Result> { ListData = db22_stock.usp_web_GetSiseStockPlus().ToList() };
        }

        public ListModel<usp_web_GetSiseStockPlusK_Result> GetSiseStockPlusKList()
        {
            return new ListModel<usp_web_GetSiseStockPlusK_Result> { ListData = db22_stock.usp_web_GetSiseStockPlusK().ToList() };
        }

        public ListModel<usp_web_getSiseMarketSum_Result1> GetSiseMarketSumList(MarketCondition condition)
        {
            return new ListModel<usp_web_getSiseMarketSum_Result1> { ListData = db22_stock.usp_web_getSiseMarketSum(condition.Market).ToList() };
        }

        public ListModel<usp_web_getSiseForeignHold_Result> GetSiseForeignHoldList(MarketCondition condition)
        {
            return new ListModel<usp_web_getSiseForeignHold_Result> { ListData = db22_stock.usp_web_getSiseForeignHold(condition.Market).ToList() };
        }

        /*해외증시*/
        public usp_wownet_usa_index_online_Result GetUsaIndexOnline(MarketCondition condition)
        {
            return db22_stock.usp_wownet_usa_index_online(condition.Market).FirstOrDefault();
        }

        public ListModel<usp_wownet_usa_index_hour_Result> GetUsaIndexHourList(MarketCondition condition)
        {
            ListModel<usp_wownet_usa_index_hour_Result> resultData = new ListModel<usp_wownet_usa_index_hour_Result>();
            var list = db22_stock.usp_wownet_usa_index_hour(condition.Market).ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;
            return resultData;
        }

        public ListModel<usp_wownet_usa_index_history_Result> GetUsaIndexHistoryList(MarketCondition condition)
        {
            ListModel<usp_wownet_usa_index_history_Result> resultData = new ListModel<usp_wownet_usa_index_history_Result>();
            var list = db22_stock.usp_wownet_usa_index_history(condition.Market).ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;
            return resultData;
        }

        public ListModel<usp_web_usa_stock_group_Result> GetUsaStockGroupList(MarketCondition condition)
        {
            ListModel<usp_web_usa_stock_group_Result> resultData = new ListModel<usp_web_usa_stock_group_Result>();
            var list = db22_stock.usp_web_usa_stock_group(condition.Market).ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;
            return resultData;
        }

        public usp_wownet_Asia_Index_online_Result GetAsiaIndexOnline(MarketCondition condition)
        {
            return db22_stock.usp_wownet_Asia_Index_online(condition.Market).FirstOrDefault();
        }

        public ListModel<usp_wownet_Asia_Index_hour_Result> GetAsiaIndexHourList(MarketCondition condition)
        {
            ListModel<usp_wownet_Asia_Index_hour_Result> resultData = new ListModel<usp_wownet_Asia_Index_hour_Result>();
            var list = db22_stock.usp_wownet_Asia_Index_hour(condition.Market).ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;
            return resultData;
        }

        public ListModel<usp_wownet_Asia_Index_history_Result> GetAsiaIndexHistoryList(MarketCondition condition)
        {
            ListModel<usp_wownet_Asia_Index_history_Result> resultData = new ListModel<usp_wownet_Asia_Index_history_Result>();
            var list = db22_stock.usp_wownet_Asia_Index_history(condition.Market).ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;
            return resultData;
        }

        public ListModel<usp_web_korea_adr_Result> GetKoreaAdrList()
        {
            return new ListModel<usp_web_korea_adr_Result> { ListData = db22_stock.usp_web_korea_adr().ToList() };
        }

        /*유럽*/
        public usp_wownet_Europe_Index_online_Result GetEuropeIndexOnline(MarketCondition condition)
        {
            return db22_stock.usp_wownet_Europe_Index_online(condition.Market).FirstOrDefault();
        }

        public ListModel<usp_wownet_Europe_Index_history_Result> GetEuropeIndexHistoryList(MarketCondition condition)
        {
            ListModel<usp_wownet_Europe_Index_history_Result> resultData = new ListModel<usp_wownet_Europe_Index_history_Result>();
            var list = db22_stock.usp_wownet_Europe_Index_history(condition.Market).ToList();
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize > -1)
                {
                    condition.PageSize = 7;
                }

                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;
            return resultData;
        }

        public ListModel<usp_GetInvestOnline_Result> GetInvestOnlineList()
        {
            return new ListModel<usp_GetInvestOnline_Result> { ListData = db22_stock.usp_GetInvestOnline().ToList() };
        }

        public ListModel<usp_GetInvestTotal_Result> GetInvestTotalList(InvestCondition condition)
        {
            return new ListModel<usp_GetInvestTotal_Result> { ListData = db22_stock.usp_GetInvestTotal(condition.fromDate).ToList() };
        }

        public ListModel<usp_web_usa_adr_Result> GetUSAAdrTotalList()
        {
            return new ListModel<usp_web_usa_adr_Result> { ListData = db22_stock.usp_web_usa_adr().ToList() };
        }

        public ListModel<USP_WOWNET_USA_DETAIL_Result> GetUSAAdrDetailList(UsaIndustryCondition condition)
        {
            return new ListModel<USP_WOWNET_USA_DETAIL_Result> { ListData = db22_stock.USP_WOWNET_USA_DETAIL(condition.PartName).ToList() };
        }

        public ListModel<usp_web_usa_adr_dawo_semiconductor_Result> GetUSAADRDawoSemiconductorList()
        {
            return new ListModel<usp_web_usa_adr_dawo_semiconductor_Result> { ListData = db22_stock.usp_web_usa_adr_dawo_semiconductor().ToList() };
        }

        public ListModel<usp_web_usa_adr_dawo_media_Result> GetUSAADRDawoMediaList()
        {
            return new ListModel<usp_web_usa_adr_dawo_media_Result> { ListData = db22_stock.usp_web_usa_adr_dawo_media().ToList() };
        }

        public ListModel<usp_web_usa_adr_dawo_insurance_Result> GetUSAADRDawoInsuranceList()
        {
            return new ListModel<usp_web_usa_adr_dawo_insurance_Result> { ListData = db22_stock.usp_web_usa_adr_dawo_insurance().ToList() };
        }

        public ListModel<usp_web_usa_adr_dawo_retail_Result> GetUSAADRDawoRetailList()
        {
            return new ListModel<usp_web_usa_adr_dawo_retail_Result> { ListData = db22_stock.usp_web_usa_adr_dawo_retail().ToList() };
        }

        public ListModel<usp_web_usa_adr_dawo_energy_Result> GetUSAADRDawoEnergyList()
        {
            return new ListModel<usp_web_usa_adr_dawo_energy_Result> { ListData = db22_stock.usp_web_usa_adr_dawo_energy().ToList() };
        }

        public ListModel<usp_web_usa_adr_dawo_pharmacy_Result> GetUSAADRDawoPharmacyList()
        {
            return new ListModel<usp_web_usa_adr_dawo_pharmacy_Result> { ListData = db22_stock.usp_web_usa_adr_dawo_pharmacy().ToList() };
        }

        public ListModel<usp_web_usa_adr_nasdaq_internet_Result> GetUSAADRNasdaqInternetList()
        {
            return new ListModel<usp_web_usa_adr_nasdaq_internet_Result> { ListData = db22_stock.usp_web_usa_adr_nasdaq_internet().ToList() };
        }

        public ListModel<usp_web_usa_adr_nasdaq_network_Result> GetUSAADRNasdaqNetworkList()
        {
            return new ListModel<usp_web_usa_adr_nasdaq_network_Result> { ListData = db22_stock.usp_web_usa_adr_nasdaq_network().ToList() };
        }

        public ListModel<usp_web_usa_adr_nasdaq_comunications_Result> GetUSAADRNasdaqComunicationsList()
        {
            return new ListModel<usp_web_usa_adr_nasdaq_comunications_Result> { ListData = db22_stock.usp_web_usa_adr_nasdaq_comunications().ToList() };
        }

        public ListModel<usp_web_usa_adr_nasdaq_computer_Result> GetUSAADRNasdaqComputerList()
        {
            return new ListModel<usp_web_usa_adr_nasdaq_computer_Result> { ListData = db22_stock.usp_web_usa_adr_nasdaq_computer().ToList() };
        }

        public ListModel<usp_web_usa_adr_nasdaq_semiconductor_Result> GetUSAADRNasdaqSemiconductorList()
        {
            return new ListModel<usp_web_usa_adr_nasdaq_semiconductor_Result> { ListData = db22_stock.usp_web_usa_adr_nasdaq_semiconductor().ToList() };
        }

        public ListModel<usp_web_usa_adr_nasdaq_bio_Result> GetUSAADRNasdaqBioList()
        {
            return new ListModel<usp_web_usa_adr_nasdaq_bio_Result> { ListData = db22_stock.usp_web_usa_adr_nasdaq_bio().ToList() };
        }

        public ListModel<usp_web_usa_adr_sandp500_internet_Result> GetUSAADRSANDP500InternetList()
        {
            return new ListModel<usp_web_usa_adr_sandp500_internet_Result> { ListData = db22_stock.usp_web_usa_adr_sandp500_internet().ToList() };
        }

        public ListModel<usp_wownet_Marketindex_BankExchange_Result> GetMarketIndexBankExchangeList()
        {
            return new ListModel<usp_wownet_Marketindex_BankExchange_Result> { ListData = db22_stock.usp_wownet_Marketindex_BankExchange().ToList() };
        }

        public ListModel<usp_web_getExchangeTimeList_Result> GetExchangeTimeList(ExchangeCondition condition)
        {
            return new ListModel<usp_web_getExchangeTimeList_Result> { ListData = db22_stock.usp_web_getExchangeTimeList(condition.Code).ToList() };
        }

        public ListModel<usp_wownet_Marketindex_worldExchange_Result> GetMarketIndexWorldExchangeList()
        {
            return new ListModel<usp_wownet_Marketindex_worldExchange_Result> { ListData = db22_stock.usp_wownet_Marketindex_worldExchange().ToList() };
        }

        public usp_GetStockPrice_Result GetStockPrice(FinanceCondition condition)
        {
            usp_GetStockPrice_Result stockPriceObj = new usp_GetStockPrice_Result();

            condition.SearchKey = condition.SearchStr;
            
            //SearchText : (ShortStockCode Or Stock KorName)
            if (!String.IsNullOrEmpty(condition.SearchKey))
            {
                try
                {
                    stockPriceObj = db22_stock.usp_GetStockPrice(condition.SearchKey).FirstOrDefault();
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
                {
                    stockPriceObj = new usp_GetStockPrice_Result { mkt_halt = "N", arj_code = condition.SearchKey, stock_nickname = condition.SearchKey, stock_wanname = condition.SearchKey, data_day = DateTime.Now.ToString() };
                }

                return stockPriceObj;
            }

            return stockPriceObj;
        }

        public ListModel<usp_web_getStockInvestorList_Result> GetStockInvestorList(StockInvestorCondition condition)
        {
            return new ListModel<usp_web_getStockInvestorList_Result> { ListData = db22_stock.usp_web_getStockInvestorList(condition.Type, condition.ShortCode).Take(5).ToList() };
        }

        public ListModel<usp_web_getStockInvestorSum_Result> GetStockInvestorSumList(StockInvestorCondition condition)
        {
            return new ListModel<usp_web_getStockInvestorSum_Result> { ListData = db22_stock.usp_web_getStockInvestorSum(condition.StockCode).Take(5).ToList() };
        }

        public ListModel<usp_web_getStockPart_Result> GetStockPartList(StockInvestorCondition condition)
        {
            ListModel<usp_web_getStockPart_Result> stockPartList;

            if (condition.K_gbn == "1")
            {
                stockPartList = new ListModel<usp_web_getStockPart_Result> { ListData = db22_stock.usp_web_getStockPart(condition.PartCode1).ToList() };
            }
            else
            {
                if(condition.PartCode2.Length == 2)
                {
                    condition.PartCode2 = "0" + condition.PartCode2;
                }

                stockPartList = new ListModel<usp_web_getStockPart_Result> { ListData = db22_stock.usp_web_getStockPart(condition.PartCode2).ToList() };
            }
            return stockPartList;
        }

        public ListModel<usp_web_getStockThema_Result> GetStockThemaList(StockInvestorCondition condition)
        {
            ListModel<usp_web_getStockThema_Result> stockThemaList;

            if (condition.K_gbn == "1")
            {
                stockThemaList = new ListModel<usp_web_getStockThema_Result> { ListData = db22_stock.usp_web_getStockThema(condition.PartCode1).ToList() };
            }
            else
            {
                if (condition.PartCode2.Length == 2)
                {
                    condition.PartCode2 = "0" + condition.PartCode2;
                }

                stockThemaList = new ListModel<usp_web_getStockThema_Result> { ListData = db22_stock.usp_web_getStockThema(condition.PartCode2).ToList() };
            }
            return stockThemaList;
            
        }

        public ListModel<usp_web_getRelativeStock_Result> GetRelativeStockList(StockInvestorCondition condition)
        {
            return new ListModel<usp_web_getRelativeStock_Result> { ListData = db22_stock.usp_web_getRelativeStock(condition.K_gbn, condition.PartCode1, condition.PartCode2).ToList() };
        }

        //public NSP_BANNER_RANDOM_Result GetADBanner(ADBannnerCondition condition)
        //{
        //    return db49_wownet.NSP_BANNER_RANDOM(condition.Type, condition.Area).FirstOrDefault();
        //}

        public ListModel<ArticleStock> GetRecentNewsTop4List(ArticleStockCondition condition)
        {
            ListModel<ArticleStock> recentNewsTop4List = new ListModel<ArticleStock>();

            if (!String.IsNullOrEmpty(condition.ArjCode))
            {
                recentNewsTop4List.ListData = db49_Article.ArticleStock.Where(a => a.StockCode.Equals(condition.ArjCode)).OrderByDescending(a => a.ArticleDate).Take(4).ToList();
            }

            return recentNewsTop4List;
        }

        public ListModel<ArticleStock> GetStockNewsList(ArticleStockCondition condition)
        {
            ListModel<ArticleStock> resultData = new ListModel<ArticleStock>();

            var list = db49_Article.ArticleStock.OrderByDescending(a => a.ArticleDate).Where(a => a.StockCode == condition.ArjCode);

            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                if(condition.SearchType == "title")
                {
                    list = list.Where(a => a.Title.Contains(condition.SearchText) == true);
                }
            }

            resultData.TotalDataCount = list.Count();

            if(condition.PageSize > -1)
            {
                if(condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();

            return resultData;
        }

        public ListModel<usp_web_getStockGongsi_Result> GetStockGongsiList(NoticeStockCondition condition)
        {
            ListModel<usp_web_getStockGongsi_Result> resultData = new ListModel<usp_web_getStockGongsi_Result>();

            var list = db22_stock.usp_web_getStockGongsi(condition.StockCode).ToList();

            resultData.TotalDataCount = list.Count();

            if(condition.PageSize > -1)
            {
                if(condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        public ListModel<usp_web_getStockGongsiView_Result> GetStockGongsiView(NoticeStockCondition condition)
        {
            ListModel<usp_web_getStockGongsiView_Result> resultData = new ListModel<usp_web_getStockGongsiView_Result>();

            var list = db22_stock.usp_web_getStockGongsiView(condition.FSeq, condition.FDataDay, condition.StockCode).ToList();

            resultData.ListData = list;

            return resultData;
        }

        public ListModel<usp_web_getNoticeTop6List_Result> GetNoticeTop6List(NoticeStockCondition condition)
        {
            ListModel<usp_web_getNoticeTop6List_Result> recentNoticeTop6List = new ListModel<usp_web_getNoticeTop6List_Result>();

            if (!String.IsNullOrEmpty(condition.StockCode))
            {
                recentNoticeTop6List.ListData = db22_stock.usp_web_getNoticeTop6List(condition.StockCode).ToList();
            }

            return recentNoticeTop6List;
        }

        public ListModel<usp_web_getStockDiscussionTop9List_Result> GetDiscussionTop9List(DiscurssionCondition condition)
        {
            ListModel<usp_web_getStockDiscussionTop9List_Result> discussionTop9List = new ListModel<usp_web_getStockDiscussionTop9List_Result>();
            
            if (!String.IsNullOrEmpty(condition.ArjCode))
            {
                if (String.IsNullOrEmpty(condition.BCode))
                {
                    condition.BCode = "N03040000";
                }

                discussionTop9List.ListData = db49_wownet.usp_web_getStockDiscussionTop9List(condition.BCode, condition.ArjCode).ToList();
            }

            return discussionTop9List;
        }

        public ListModel<usp_web_getStockConsultTop9List_Result> GetConsultTop9List(ConsultCondition condition)
        {
            ListModel<usp_web_getStockConsultTop9List_Result> consultTop9List = new ListModel<usp_web_getStockConsultTop9List_Result>();

            if (!String.IsNullOrEmpty(condition.CName))
            {
                if (String.IsNullOrEmpty(condition.BCode))
                {
                    condition.BCode = "N03010100";
                }

                consultTop9List.ListData = db49_wownet.usp_web_getStockConsultTop9List(condition.BCode, condition.CName).ToList();
            }

            return consultTop9List;
        }

        public ListModel<usp_web_select_stock_cafe_list_Result> GetStockCafeList(StockCafeCondition condition)
        {
            ListModel<usp_web_select_stock_cafe_list_Result> stockCafeTop3List = new ListModel<usp_web_select_stock_cafe_list_Result>();

            if (!String.IsNullOrEmpty(condition.ArjCode))
            {
                stockCafeTop3List.ListData = db49_wowcafe.usp_web_select_stock_cafe_list(condition.ArjCode).ToList();
            }

            return stockCafeTop3List;
        }

        public ListModel<usp_web_vodTop6List_Result> GetVodTop6List(VodCondition condition)
        {
            ListModel<usp_web_vodTop6List_Result> vodTop6List = new ListModel<usp_web_vodTop6List_Result>();
            if (!String.IsNullOrEmpty(condition.ArjCode))
            {
                vodTop6List.ListData = db49_editVOD.usp_web_vodTop6List(condition.ArjCode).ToList();
            }

            return vodTop6List;
        }

        public ListModel<usp_web_vodList_Result> GetVodList(VodCondition condition)
        {
            ListModel<usp_web_vodList_Result> resultData = new ListModel<usp_web_vodList_Result>();

            var list = db49_editVOD.usp_web_vodList(condition.ArjCode).ToList();

            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                if (condition.SearchType == "title")
                {
                    list = list.Where(a => a.subject.Contains(condition.SearchText) == true).ToList();
                }
            }

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        public ListModel<usp_web_listSubTopvod6_Result> GetVodInvestTop6List(VodInvestCondition condition)
        {
            ListModel<usp_web_listSubTopvod6_Result> vodInvestTop5List = new ListModel<usp_web_listSubTopvod6_Result>();
            if (!String.IsNullOrEmpty(condition.CtCode))
            {
                vodInvestTop5List.ListData = db49_editVOD.usp_web_listSubTopvod6(condition.CtCode).ToList();
            }

            return vodInvestTop5List;
        }

        /*종목상세 > 종목동영상 > 투자전략*/
        public ListModel<usp_web_getInvestVodList_Result> GetInvestVodList(VodInvestCondition condition)
        {
            ListModel<usp_web_getInvestVodList_Result> resultData = new ListModel<usp_web_getInvestVodList_Result>();

            var list = db49_editVOD.usp_web_getInvestVodList(condition.CtCode).ToList();

            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                if (condition.SearchType == "title")
                {
                    list = list.Where(a => a.subject.Contains(condition.SearchText) == true).ToList();
                }
            }

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list;

            return resultData;
        }

        /* 종목상세 > 현재가 > 주요시세 */
        public usp_LatestStockPrice_Result GetLatestStockPrice(CurrentAnalysisCondition condition)
        {
            usp_LatestStockPrice_Result latestStockPrice = new usp_LatestStockPrice_Result();
            if (!String.IsNullOrEmpty(condition.StockCode))
            {
                latestStockPrice = db22_stock.usp_LatestStockPrice(condition.StockCode).FirstOrDefault();
            }

            return latestStockPrice;
        }

       public usp_web_stock_hoga_Result GetStockHoga(CurrentAnalysisCondition condition)
        {
            usp_web_stock_hoga_Result hoga = new usp_web_stock_hoga_Result();
            if (!String.IsNullOrEmpty(condition.StockCode))
            {
                hoga = db22_stock.usp_web_stock_hoga(condition.StockCode).FirstOrDefault();
            }

            return hoga;
        }

        public ListModel<usp_web_getStockDealing5_Result> GetStockDealing5List(CurrentAnalysisCondition condition)
        {
            ListModel<usp_web_getStockDealing5_Result> stockDealing5List = new ListModel<usp_web_getStockDealing5_Result>();
            if (!String.IsNullOrEmpty(condition.StockCode))
            {
                stockDealing5List.ListData = db22_stock.usp_web_getStockDealing5(condition.StockCode).ToList();
            }

            return stockDealing5List;
        }

        public ListModel<usp_getDayStockPriceBand_Result> GetDayStockPriceBandList(CurrentAnalysisCondition condition)
        {
            ListModel<usp_getDayStockPriceBand_Result> dayStockPriceBandList = new ListModel<usp_getDayStockPriceBand_Result>();
            if (!String.IsNullOrEmpty(condition.StockCode))
            {
                dayStockPriceBandList.ListData = db22_stock.usp_getDayStockPriceBand(condition.StockCode).ToList();
            }

            return dayStockPriceBandList;
        }

        public ListModel<usp_SelectStockRecentlyTime_Result> GetSelectStockRecentlyTimeList(CurrentAnalysisCondition condition)
        {
            ListModel<usp_SelectStockRecentlyTime_Result> stockRecentlyTimeList = new ListModel<usp_SelectStockRecentlyTime_Result>();
            if (!String.IsNullOrEmpty(condition.StockCode))
            {
                stockRecentlyTimeList.ListData = db22_stock.usp_SelectStockRecentlyTime(condition.StockCode).ToList();
            }

            return stockRecentlyTimeList;
        }

        public ListModel<usp_SelectStockRecentlyDay_Result> GetSelectStockRecentlyDayList(CurrentAnalysisCondition condition)
        {
            ListModel<usp_SelectStockRecentlyDay_Result> stockRecentlyDayList = new ListModel<usp_SelectStockRecentlyDay_Result>();
            if (!String.IsNullOrEmpty(condition.StockCode))
            {
                stockRecentlyDayList.ListData = db22_stock.usp_SelectStockRecentlyDay(condition.StockCode).ToList();
            }

            return stockRecentlyDayList;
        }

        public ListModel<usp_web_getTradeTrend_Result> GetTradeTrendList(CurrentAnalysisCondition condition)
        {
            ListModel<usp_web_getTradeTrend_Result> tradeTrendList = new ListModel<usp_web_getTradeTrend_Result>();
            if (!String.IsNullOrEmpty(condition.StockCode))
            {
                tradeTrendList.ListData = db22_stock.usp_web_getTradeTrend(condition.StockCode).ToList();
            }

            return tradeTrendList;
        }

        public ListModel<tblMyFavoriteJongMok> GetMyFavoriteJongMokList(MyFavoriteStockCondition condition) 
        {
            ListModel<tblMyFavoriteJongMok> resultData = new ListModel<tblMyFavoriteJongMok>();

            var list = db22_stock.tblMyFavoriteJongMok.AsNoTracking().AsQueryable();

            list = list.Where(x => x.usernumber.Value.Equals(condition.loginUserInfo.UserNumber));

            resultData.TotalDataCount = list.Count();
            
            resultData.ListData = list.OrderByDescending(m => m.orderNo).Take(5).ToList();

            if(resultData.TotalDataCount > 0)
            {
                usp_GetStockPrice_Result stockPriceResult;
                foreach (var item in resultData.ListData)
                {
                    bool chk = (db22_stock.tblStockBatch.Where(m => m.ShortCode == "A" + item.stockcode || m.korName == item.stock_wanname).Count() >= 1 ? true : false);

                    if (chk)
                    {
                        stockPriceResult = db22_stock.usp_GetStockPrice(item.stockcode).FirstOrDefault();
                    }
                    else
                    {
                        stockPriceResult = new usp_GetStockPrice_Result { arj_code = item.arj_code, mkt_halt = "1", stock_wanname = "거래정지 종목" };
                    }

                    if (stockPriceResult != null)
                    {
                        item.chg_type = stockPriceResult.chg_type;
                        item.curr_price = stockPriceResult.curr_price;
                        item.net_chg = stockPriceResult.net_chg;
                        item.mkt_halt = stockPriceResult.mkt_halt;
                        item.stock_wanname = stockPriceResult.stock_wanname;
                        item.Groupid = stockPriceResult.Groupid;
                        item.stock_code = stockPriceResult.stock_code;
                    }
                }
            }
            
            return resultData;

        }

        public bool GetCheckStockHoliday(HolidayCondition condition)
        {
            //휴일이 false면 실시간, true는 실시간이 적용되지 않는다.
            //토요일, 일요일, 휴일로 지정된 날이 있으면 true를 return해서 실시간을 적용하지 않는다.
            bool chkResult = false;


            if (DayOfWeek.Sunday == DateTime.Now.DayOfWeek || DayOfWeek.Saturday == DateTime.Now.DayOfWeek)
            {
                chkResult = true;
            }
            else
            {
                ListModel<usp_web_getStockHoliday_Result> resultData = new ListModel<usp_web_getStockHoliday_Result>();
                if (!String.IsNullOrEmpty(condition.CheckDt))
                {
                    resultData.ListData = db22_stock.usp_web_getStockHoliday(condition.CheckDt).ToList();
                }

                if (resultData.ListData.Count > 0)
                {
                    //휴일이 존재하면 실시간을 멈춘다. 
                    chkResult = true;
                }
                else
                {
                    DateTime currentTime = DateTime.Now;
                    //DateTime currentTime = new DateTime(Int32.Parse(DateTime.Now.Year.ToString()), Int32.Parse(DateTime.Now.Month.ToString()), Int32.Parse(DateTime.Now.Day.ToString()), 09, 00, 0);
                    //DateTime currentTime = new DateTime(Int32.Parse(DateTime.Now.Year.ToString()), Int32.Parse(DateTime.Now.Month.ToString()), Int32.Parse(DateTime.Now.Day.ToString()), 10, 00, 0);
                    //DateTime currentTime = new DateTime(Int32.Parse(DateTime.Now.Year.ToString()), Int32.Parse(DateTime.Now.Month.ToString()), Int32.Parse(DateTime.Now.Day.ToString()), 13, 00, 0);
                    //DateTime currentTime = new DateTime(Int32.Parse(DateTime.Now.Year.ToString()), Int32.Parse(DateTime.Now.Month.ToString()), Int32.Parse(DateTime.Now.Day.ToString()), 18, 00, 0);
                    //TimeSpan t2 = currentTime.Subtract(DateTime.Now);
                    DateTime startTime = new DateTime(Int32.Parse(currentTime.Year.ToString()), Int32.Parse(currentTime.Month.ToString()), Int32.Parse(currentTime.Day.ToString()), 08, 30, 0);
                    DateTime endTime = new DateTime(Int32.Parse(currentTime.Year.ToString()), Int32.Parse(currentTime.Month.ToString()), Int32.Parse(currentTime.Day.ToString()), 16, 00, 0);
                    //TimeSpan t3 = endTime.Subtract(currentTime);

                    if (currentTime.CompareTo(startTime) == 1 && currentTime.CompareTo(endTime) == -1)
                    {
                        //실시간 
                        chkResult = false;
                    }
                    else
                    {
                        //실시간을 멈춘다
                        chkResult = true;
                        
                    }
                }
            }

            return chkResult; 
        }

        /// <summary>
        /// 가상화폐
        /// </summary>
        /// <returns></returns>
        //public List<tblVirtualMoney> GetVirtualMoney()
        //{
        //    List<tblVirtualMoney> resultList = db22_stock.tblVirtualMoney.Where(a => a.DeleteFlag.Equals("N")).OrderBy(a => a.etc).AsQueryable().ToList();
            
        //    return resultList;
        //}
        public List<usp_web_getVirtualMoney_Result> GetVirtualMoney()
        {
            List<usp_web_getVirtualMoney_Result> resultList = db22_stock.usp_web_getVirtualMoney().ToList();

            return resultList;
        }

        //금융 > 국내증시 > 메인 > 증시뉴스
        public ListModel<NUP_NEWS_SECTION_SELECT_Result> GetTodayStockNews()
        {
            //return new ListModel<NUP_NEWS_SECTION_SELECT_Result> { ListData = db49_Article.NUP_NEWS_SECTION_SELECT("STOCK", "", "", "", "", "", 1, 30).Take(5).ToList() };
            string startDay = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
            //string endDay = string.Format("{0}-{1}-{2}", DateTime.Now.Year, DateTime.Now.Month, Int32.Parse(DateTime.Now.Day.ToString()));
            string endDay = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return new ListModel<NUP_NEWS_SECTION_SELECT_Result> { ListData = db49_Article.NUP_NEWS_SECTION_SELECT("", "", "W001", "", startDay, endDay, 1, 5).ToList() };
            //return new ListModel<NUP_NEWS_SECTION_SELECT_Result>();
        }

        //금융 > 해외증시 > 메인 > 해외뉴스
        public ListModel<NUP_NEWS_SECTION_SELECT_Result> GetWorldStockNews()
        {
            string startDay = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
            //string endDay = string.Format("{0}-{1}-{2}", DateTime.Now.Year, DateTime.Now.Month, Int32.Parse(DateTime.Now.Day.ToString()) + 1);
            string endDay = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return new ListModel<NUP_NEWS_SECTION_SELECT_Result> { ListData = db49_Article.NUP_NEWS_SECTION_SELECT("", "", "W070", "", startDay, endDay, 1, 5).ToList() };
        }

        //금융 > 국내증시 > 메인 > 오늘의 투자전략
        public List<TAB_STRATEGY_APPLICATION> GetTodayInvests()
        {
            var list = db49_wownet.TAB_STRATEGY_APPLICATION.Where(m => m.VIEW_FLAG == "Y" && m.BCODE == "N01020100").OrderByDescending(m => m.REF).ThenBy(m => m.REF_STEP).Take(3).ToList();

            return list;
        }

        //금융 > 국내증시 > 메인 > 테마별 상위
        public List<FNC_GetThemaJisuTop_Result> GetThemaJisuTopList()
        {
            var list = db22_stock.FNC_GetThemaJisuTop((int)DateTime.Now.DayOfWeek, DateTime.Now.Hour, DateTime.Now.Minute).ToList();

            return list;
        }

        //금융 > 국내증시 > 메인 > 베스트파트너
        public ListModel<usp_web_getBestProHintStocking_Result> GetBestProHintStockingList()
        {
            return new ListModel<usp_web_getBestProHintStocking_Result> { ListData = db49_broadcast.usp_web_getBestProHintStocking().ToList() };
        }

        //금융 > 해외증시 > 메인 > 베스트파트너
        public ListModel<usp_web_getWorldProHintPartner_Result> GetWorldProHindtPartnerList()
        {
            return new ListModel<usp_web_getWorldProHintPartner_Result> { ListData = db49_broadcast.usp_web_getWorldProHintPartner().ToList() };
        }

        //금융 > 국내증시 > 메인 > 최근검색종목
        public ListModel<usp_web_getCurrentSearchStockList_Result> GetCurrentSearchStockList()
        {
            return new ListModel<usp_web_getCurrentSearchStockList_Result> { ListData = db22_stock.usp_web_getCurrentSearchStockList().ToList() };
        }


        #region//실시간 데이터 (시세서버에서 사용하는 데이터)
        public List<usp_web_clfSelectOnlineItem_Result> GetClfSelectOnlineItem()
        {
            List<usp_web_clfSelectOnlineItem_Result> list = db22_stock.usp_web_clfSelectOnlineItem().ToList();
            
            return list;
        }

        public List<usp_web_clfSelectOnlineIndex_Result> GetClfSelectOnlineIndex()
        {
            List<usp_web_clfSelectOnlineIndex_Result> list = db22_stock.usp_web_clfSelectOnlineIndex().ToList();
            return list;
        }

        public List<usp_web_clfSelectHoliday_Result> GetClfSelectHoliday(SiseHolyCondition condition)
        {
            List<usp_web_clfSelectHoliday_Result> list = new List<usp_web_clfSelectHoliday_Result>();

            if(!String.IsNullOrEmpty(condition.MARKET_DT) && !String.IsNullOrEmpty(condition.HOLY_YN) && !String.IsNullOrEmpty(condition.DEL_YN))
            {
                list = db22_stock.usp_web_clfSelectHoliday(condition.MARKET_DT, condition.HOLY_YN, condition.DEL_YN).ToList();
            }

            return list;
        }

       
        #endregion
        #region 람다예시 추후 삭제예정
        /// <summary>
        /// 금융 > 국내증시 > 코스피 
        /// 기존 as_is usp_GetSiseCurrentKospi, usp_GetSiseMainMergeCode
        /// </summary>
        /// <returns>현재 코스피 주요정보</returns>
        //public StockInfo GetAtSiseCurrentKospi()
        //{
        //    var resultObj = (from tbl_OnlineIndex in db22_stock.tblOnlineIndexes
        //                     join tbl_MarketSummaries in db22_stock.tblMarketSummaries on tbl_OnlineIndex.Market equals tbl_MarketSummaries.Market
        //                     where tbl_OnlineIndex.Market == "1" && tbl_OnlineIndex.Datatype == "D0" && tbl_OnlineIndex.IndustryCode == "001"
        //                     select new
        //                     {
        //                         Market = tbl_OnlineIndex.Market,
        //                         Price = tbl_OnlineIndex.TradePrice,
        //                         ChgType = tbl_OnlineIndex.ChgType,
        //                         ChgPrice = tbl_OnlineIndex.ChgPrice,
        //                         ChgType1 = tbl_MarketSummaries.UpLimit,
        //                         ChgType2 = tbl_MarketSummaries.Up,
        //                         ChgType3 = tbl_MarketSummaries.Middle,
        //                         ChgType4 = tbl_MarketSummaries.DownLimit,
        //                         ChgType5 = tbl_MarketSummaries.Down,
        //                         Volume = tbl_OnlineIndex.TotalVol,
        //                         VolPrice = tbl_OnlineIndex.TotalPrice
        //                     }).FirstOrDefault();
        //    stockInfo =
        //    new StockInfo
        //    {
        //        Market = resultObj.Market,
        //        CurPoint = Convert.ToInt32(resultObj.Price) * 100,
        //        Price = Convert.ToDecimal(resultObj.Price),
        //        ChgType = ChgTypeConvert(resultObj.ChgType),
        //        ChgPrice = resultObj.ChgPrice.ToString(),
        //        ChgRate = resultObj.ChgType + Math.Round(Convert.ToDouble(((resultObj.ChgPrice / (resultObj.Price - resultObj.ChgPrice)) * 100)), 2).ToString() + "%",
        //        ChgType1 = resultObj.ChgType1.ToString(),
        //        ChgType2 = resultObj.ChgType2.ToString(),
        //        ChgType3 = resultObj.ChgType3.ToString(),
        //        ChgType4 = resultObj.ChgType4.ToString(),
        //        ChgType5 = resultObj.ChgType5.ToString(),
        //        Volume = Convert.ToDecimal(resultObj.Volume),
        //        VolPrice = Convert.ToDecimal(resultObj.VolPrice),
        //        SiseMainMergeCode = db22_stock.usp_getSiseMainmergecode().Where(m => m.Market == MarketConvert(resultObj.Market)).FirstOrDefault()
        //    };

        //    return stockInfo;
        //}

        ///// <summary>
        ///// 금융 > 국내증시 > 코스피 
        ///// 기존 as_is usp_GetSiseCurrentKosdaq, usp_GetSiseMainMergeCode
        ///// </summary>
        ///// <returns>현재 코스닥 주요정보</returns>
        //public StockInfo GetAtSiseCurrentKosdaq()
        //{
        //    var resultObj = (from tbl_OnlineIndex in db22_stock.tblOnlineIndexes
        //                     join tbl_MarketSummaries in db22_stock.tblMarketSummaries on tbl_OnlineIndex.Market equals tbl_MarketSummaries.Market
        //                     where tbl_OnlineIndex.Market == "2" && tbl_OnlineIndex.Datatype == "E4" && tbl_OnlineIndex.IndustryCode == "001"
        //                     select new
        //                     {
        //                         Market = tbl_OnlineIndex.Market,
        //                         curpoint = (from view_index_all in db22_stock.v_index_all
        //                                    where view_index_all.code == "K02"
        //                                    select view_index_all.cur_price).FirstOrDefault(),
        //                         Price = tbl_OnlineIndex.TradePrice,
        //                         ChgType = tbl_OnlineIndex.ChgType,
        //                         ChgPrice = tbl_OnlineIndex.ChgPrice,
        //                         ChgType1 = tbl_MarketSummaries.UpLimit,
        //                         ChgType2 = tbl_MarketSummaries.Up,
        //                         ChgType3 = tbl_MarketSummaries.Middle,
        //                         ChgType4 = tbl_MarketSummaries.DownLimit,
        //                         ChgType5 = tbl_MarketSummaries.Down,
        //                         Volume = tbl_OnlineIndex.TotalVol,
        //                         VolPrice = tbl_OnlineIndex.TotalPrice
        //                     }).FirstOrDefault();

        //    stockInfo = new StockInfo
        //                {
        //                    Market = resultObj.Market,
        //                    CurPoint = resultObj.curpoint,
        //                    Price = Convert.ToDecimal(resultObj.Price),
        //                    ChgType = ChgTypeConvert(resultObj.ChgType),
        //                    ChgPrice = resultObj.ChgPrice.ToString(),
        //                    ChgRate = resultObj.ChgType + Math.Round(Convert.ToDouble(((resultObj.ChgPrice / (resultObj.Price - resultObj.ChgPrice)) * 100)), 2).ToString() + "%",
        //                    ChgType1 = resultObj.ChgType1.ToString(),
        //                    ChgType2 = resultObj.ChgType2.ToString(),
        //                    ChgType3 = resultObj.ChgType3.ToString(),
        //                    ChgType4 = resultObj.ChgType4.ToString(),
        //                    ChgType5 = resultObj.ChgType5.ToString(),
        //                    Volume = Convert.ToDecimal(resultObj.Volume),
        //                    VolPrice = Convert.ToDecimal(resultObj.VolPrice),
        //                    SiseMainMergeCode = db22_stock.usp_getSiseMainmergecode().Where(m => m.Market == MarketConvert(resultObj.Market)).FirstOrDefault()
        //                };

        //    return stockInfo;
        //}

        private int ChgTypeConvert(string chgType)
        {
            int rtnNum;
            switch (chgType)
            {
                case "+":
                    rtnNum = 2;
                    break;
                case "-":
                    rtnNum = 5;
                    break;
                default:
                    rtnNum = 3;
                    break;
            }

            return rtnNum;
        }

        private string MarketConvert(string marketCode)
        {
            string rtnStr;

            switch (marketCode)
            {
                case "1":
                    rtnStr = "코스피";
                    break;
                case "2":
                    rtnStr = "코스닥";
                    break;
                default:
                    rtnStr = "";
                    break;
            }

            return rtnStr;
        }

#endregion
    }
}
