using System;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage
{
    /// <summary>
    /// <para>  나의 서비스 이용내역 검색조건 </para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-11-03</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-11-03</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-11-03 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class MyServiceCondition : BaseCondition
    {

        public string START_DATE { get; set; }

        public string END_DATE { get; set; }

        public byte? ServiceStatus { get; set; }

        public byte? ServiceType { get; set; }
        /// <summary>
        /// 검색 내용
        /// </summary>
        public string SearchText { get; set; } = string.Empty;
    }
}