using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public class TradingStockCondition
    {
        public string UserCode { get; set; } = "0";
        public string UserName { get; set; } = "WOWUSER";
        public string DataID { get; set; } = "";                 
        public string DateStr { get; set; } = DateTime.Now.ToString("yyyyMMdd");
        public string Mission { get; set; } = "TECH";                           // 조건검색:TECH, 순위검색:SORT, 패턴검색:PATTEN,MAJORSEARCH,BULLSIGNAL,XBOX1,XBOX2,DIAG,DIAGALL,DIAGNEXT,5MINREPORT
        public string CondMenu { get; set; } = "";
        public string CondList { get; set; } = ",,,,,,,,";

        public int CodeOption { get; set; } = 0;                                //Null이면 아래의 CodeList 목록, 0=전종목, 1=거래소전종목, 2=코스닥전종목
        public string CodeList { get; set; } = "";                              // 검색대상종목 011200_042100_009540_003450_005380_010520_011170 형식으로전달
        public int VolumeRange1 { get; set; } = 10;                             //거래량조건1 (단위:천주)  이상
        public int VolumeRange2 { get; set; } = 0;                              //거래량조건2 (단위:천주)  이하
        public int VolumeCheck1 { get; set; } = 0;                              //거래량조건1 적용 여부 (1:적용, 0:무시)
        public int VolumeCheck2 { get; set; } = 0;                              //거래량조건2 적용 여부 (1:적용, 0:무시)
        public int AmountRange1 { get; set; } = 100;                            //거래대금조건1 (단위:백만원)  이상
        public int AmountRange2 { get; set; } = 1000;                           //거래대금조건2(단위:백만원)  이하
        public int AmountCheck1 { get; set; } = 0;                              //거래대금조건1 적용 여부 (1:적용, 0:무시)
        public int AmountCheck2 { get; set; } = 0;                              //거래대금조건2 적용 여부 (1:적용, 0:무시)
        public int PriceRange1 { get; set; } = 0;                               //거래대금조건2 적용 여부 (1:적용, 0:무시)
        public int PriceRange2 { get; set; } = 0;                               //현재가조건1 (단위:원)  이상
        public int PriceCheck1 { get; set; } = 0;                               //현재가조건1 적용 여부 (1:적용, 0:무시)
        public int PriceCheck2 { get; set; } = 0;                               //현재가조건2 적용 여부 (1:적용, 0:무시)
        public string CandleType { get; set; } = "0";                           //5분봉:0, 10분봉:1, 15분봉:2, 20분봉:3, 30분봉:4, 60분봉:5  (분봉에서만 적용함)
        public int GwanLiCheck { get; set; } = 0;                               //관리종목 제외여부 (1:제외, 0:포함)
        public int WooSunCheck { get; set; } = 0;                               //우선주 제외여부 (1:제외, 0:포함)
        public int PatternDate { get; set; } = 3;                               //1=당일.2=전일.3=연속분봉 (분봉에서만 적용함)
        public string PatternStr { get; set; } = "0_0_0_0_0_0_0_0_0";
        public int PatternCandle { get; set; } = 3;
        public string PatPeriod { get; set; } = "60_60_60_60_60_60_60_60_60";   //패턴의 기간
        public string DataClass { get; set; } = "";
        public string ProgramID { get; set; } = "WOWNET";                       //증권사구분코드
        public int TradeAmountCheck { get; set; } = 0;
        public int TradeAmountValue { get; set; } = 0;
        public int ListedPriceCheck { get; set; } = 0;                          //액면가 제외조건 적용여부 (1:적용, 0:무시)
        public int ListedPriceRatio { get; set; } = 100;                        //액면가 제외조건 (단위:%, 100이면 액면가의 100% 이하 제외)
        public int StrConversion { get; set; } = 2;                             //0=디폴트 1=Codepage1  2=Codepage2 (수신소켓 장애발생시 선택옵션)
        public int CodeOnly { get; set; } = 0;                                  //0=디폴트, 1=서버에 종목코드만 전송요구(Mission이Tech일때만 해당)	
    }
}
