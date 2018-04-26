using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.Finance;

namespace Wow.Tv.Middle.Biz.Finance
{
    public class TradingBiz : BaseBiz
    {
        public string GetStockData(TradingStockCondition condition)
        {
            var resultData = "";
            hpFinder.FinderClass hpFinder = new hpFinder.FinderClass
            {
                MaxDelay = 7
            };

            bool connStatus = hpFinder.Connect1("125.141.231.20", 30831);
            if (connStatus != true)
            {
                connStatus = hpFinder.Connect1("125.141.231.20", 30831);
            }

            var queryString = StockJoinString(condition);
            resultData = hpFinder.GetData1(queryString);
            hpFinder.close1();

            return resultData;
        }

        public string GetDayTradingData(TradingStockCondition condition)
        {
            var resultData = "";

            hpFinder.FinderClass hpFinder = new hpFinder.FinderClass
            {
                MaxDelay = 7
            };

            bool connStatus = hpFinder.Connect1("125.141.231.20", 30824);
            if (connStatus != true)
            {
                connStatus = hpFinder.Connect1("125.141.231.20", 30824);
            }

            condition.DateStr = "";
            condition.PatternStr = "16,24,32,36,42,47,53,58,63,67,70,72,74,76,77,78,78,76,73,71,66,60,56,32,9_60_60_60_60_60_60_60_60";
            condition.PatPeriod = "50_60_60_60_60_60_60_60_60";
            condition.ListedPriceRatio = 0;

            var queryString = StockJoinString(condition);
            resultData = hpFinder.GetData1(queryString);
            hpFinder.close1();

            return resultData;
        }

        public string StockJoinString(TradingStockCondition condition)
        {
            var str = "";
            str += condition.UserCode + "|";
            str += condition.UserName + "|";
            str += condition.DataID + "|";
            str += condition.DateStr + "|";
            str += condition.Mission + "|";
            str += condition.CondMenu + "|";
            str += condition.CondList + "|";
            str += condition.CodeOption + "|";
            str += condition.CodeList + "|";
            str += condition.VolumeRange1 + "|";
            str += condition.VolumeRange2 + "|";
            str += condition.VolumeCheck1 + "|";
            str += condition.VolumeCheck2 + "|";
            str += condition.AmountRange1 + "|";
            str += condition.AmountRange2 + "|";
            str += condition.AmountCheck1 + "|";
            str += condition.AmountCheck2 + "|";
            str += condition.PriceRange1 + "|";
            str += condition.PriceRange2 + "|";
            str += condition.PriceCheck1 + "|";
            str += condition.PriceCheck2 + "|";
            str += condition.CandleType + "|";
            str += condition.GwanLiCheck + "|";
            str += condition.WooSunCheck + "|";
            str += condition.PatternDate + "|";
            str += condition.PatternStr + "|";
            str += condition.PatternCandle + "|";
            str += condition.PatPeriod + "|";
            str += condition.DataClass + "|";
            str += condition.ProgramID + "|";
            str += condition.TradeAmountCheck + "|";
            str += condition.TradeAmountValue + "|";
            str += condition.ListedPriceCheck + "|";
            str += condition.ListedPriceRatio + "|";
            str += condition.StrConversion + "|";
            str += condition.CodeOnly + "|";
            str += "\r";

            return str;
        }

        public List<usp_GetBestSearchOnline_TypeA_Result> GetHotSearchList(string searchDate)
        {
            var result = db22_stock.usp_GetBestSearchOnline_TypeA(searchDate).ToList();
            return result;
        }
    }
}
