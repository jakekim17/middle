using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db89.wowbill.RegiCategoryManage
{
    /// <summary>
    /// 연간수입 정보
    /// </summary>
    public class YearIncome
    {
        public byte? SalaryId { get; set; }
        public string Descript { get; set; }
        public DateTime RegistDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AdminId { get; set; }
        public bool Apply { get; set; }
        public byte Sort { get; set; }
        public string SaveType { get; set; }
    }

    /// <summary>
    /// 연간수입 변경 결과
    /// </summary>
    public class YearIncomeModifyResult
    {
        public byte? SalaryId { get; set; }
        public string Descript { get; set; }
        public bool IsSuccess { get; set; }
        public bool UserChagned { get; set; }
        public string ReturnMessage { get; set; }
    }

    /// <summary>
    /// 투자선호대상 정보
    /// </summary>
    public class IvstFavorObj
    {
        public byte? InvestmentPreferenceObjectId { get; set; }
        public string Descript { get; set; }
        public DateTime RegistDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AdminId { get; set; }
        public bool Apply { get; set; }
        public byte Sort { get; set; }
        public string SaveType { get; set; }
    }

    /// <summary>
    /// 투자선호대상 변경 결과
    /// </summary>
    public class IvstFavorObjModifyResult
    {
        public byte? InvestmentPreferenceObjectId { get; set; }
        public string Descript { get; set; }
        public bool IsSuccess { get; set; }
        public bool UserChagned { get; set; }
        public string ReturnMessage { get; set; }
    }

    /// <summary>
    /// 기존정보 습득처 정보
    /// </summary>
    public class OrgnInfoAcquirer
    {
        public byte? InfoAcquirementId { get; set; }
        public string Descript { get; set; }
        public DateTime RegistDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AdminId { get; set; }
        public bool Apply { get; set; }
        public byte Sort { get; set; }
        public string SaveType { get; set; }
    }

    /// <summary>
    /// 기존정보 습득처 변경 결과
    /// </summary>
    public class OrgnInfoAcquirerModifyResult
    {
        public byte? InfoAcquirementId { get; set; }
        public string Descript { get; set; }
        public bool IsSuccess { get; set; }
        public bool UserChagned { get; set; }
        public string ReturnMessage { get; set; }
    }

    /// <summary>
    /// 투자기간 정보
    /// </summary>
    public class IvstProd
    {
        public byte? InvestmentPeriodId { get; set; }
        public string Descript { get; set; }
        public DateTime RegistDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AdminId { get; set; }
        public bool Apply { get; set; }
        public byte Sort { get; set; }
        public string SaveType { get; set; }
    }

    /// <summary>
    /// 투자기간 변경 결과
    /// </summary>
    public class IvstProdModifyResult
    {
        public byte? InvestmentPeriodId { get; set; }
        public string Descript { get; set; }
        public bool IsSuccess { get; set; }
        public bool UserChagned { get; set; }
        public string ReturnMessage { get; set; }
    }

    /// <summary>
    /// 투자성향 정보
    /// </summary>
    public class InvstTendency
    {
        public byte? InvestmentPropensityId { get; set; }
        public string Descript { get; set; }
        public DateTime RegistDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AdminId { get; set; }
        public bool Apply { get; set; }
        public byte Sort { get; set; }
        public string SaveType { get; set; }
    }

    /// <summary>
    /// 투자성향 변경 결과
    /// </summary>
    public class InvstTendencyModifyResult
    {
        public byte? InvestmentPropensityId { get; set; }
        public string Descript { get; set; }
        public bool IsSuccess { get; set; }
        public bool UserChagned { get; set; }
        public string ReturnMessage { get; set; }
    }

    /// <summary>
    /// 주요증권거래처 정보
    /// </summary>
    public class MainStockTrader
    {
        public byte? StockCompanyId { get; set; }
        public string Descript { get; set; }
        public DateTime RegistDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AdminId { get; set; }
        public bool Apply { get; set; }
        public byte Sort { get; set; }
        public string SaveType { get; set; }
    }

    /// <summary>
    /// 주요증권거래처 변경 결과
    /// </summary>
    public class MainStockTraderModifyResult
    {
        public byte? StockCompanyId { get; set; }
        public string Descript { get; set; }
        public bool IsSuccess { get; set; }
        public bool UserChagned { get; set; }
        public string ReturnMessage { get; set; }
    }

    /// <summary>
    /// 투자규모 정보
    /// </summary>
    public class IvstScale
    {
        public byte? InvestmentScaleId { get; set; }
        public string Descript { get; set; }
        public DateTime RegistDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AdminId { get; set; }
        public bool Apply { get; set; }
        public byte Sort { get; set; }
        public string SaveType { get; set; }
    }

    /// <summary>
    /// 투자규모 변경 결과
    /// </summary>
    public class IvstScaleModifyResult
    {
        public byte? InvestmentScaleId { get; set; }
        public string Descript { get; set; }
        public bool IsSuccess { get; set; }
        public bool UserChagned { get; set; }
        public string ReturnMessage { get; set; }
    }

    /// <summary>
    /// 관심분야 정보
    /// </summary>
    public class FavorField
    {
        public byte? InterestId { get; set; }
        public string Descript { get; set; }
        public DateTime RegistDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AdminId { get; set; }
        public bool Apply { get; set; }
        public byte Sort { get; set; }
        public string SaveType { get; set; }
    }

    /// <summary>
    /// 관심분야 변경 결과
    /// </summary>
    public class FavorFieldModifyResult
    {
        public byte? InterestId { get; set; }
        public string Descript { get; set; }
        public bool IsSuccess { get; set; }
        public bool UserChagned { get; set; }
        public string ReturnMessage { get; set; }
    }
}
