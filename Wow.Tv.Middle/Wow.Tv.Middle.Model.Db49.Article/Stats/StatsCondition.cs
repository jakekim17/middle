using System;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.Article.Stats
{
    /// <summary>
    /// <para>  통계 검색조건 </para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-09-11</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-09-11</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-09-11 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class StatsCondition : BaseCondition
    {
        /// <summary>
        /// DeptCD 
        /// </summary>
        public string DeptCD { get; set; }
        /// <summary>
        /// FreelancerID 
        /// </summary>
        public string FreelancerID { get; set; }
        /// <summary>
        /// CompCode 
        /// </summary>
        public string CompCode { get; set; }

        /// <summary>
        /// 검색 시작일
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 검색 종료일
        /// </summary>
        public DateTime EndDate { get; set; }

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