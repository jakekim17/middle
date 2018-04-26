using System;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.TradingStar
{
    /// <summary>
    /// <para>  출연자/ 거래현황등록 검색조건 </para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-09-05</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-09-05</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-09-05 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class TradingStarCondition : BaseCondition
    {
        /// <summary>
        /// tradingCode 
        /// </summary>
        public string TradingCode { get; set; }
        /// <summary>
        /// 검색 타입
        /// </summary>
        public string SearchType { get; set; } = string.Empty;
        /// <summary>
        /// 검색 내용
        /// </summary>
        public string SearchText { get; set; } = string.Empty;
    }
}