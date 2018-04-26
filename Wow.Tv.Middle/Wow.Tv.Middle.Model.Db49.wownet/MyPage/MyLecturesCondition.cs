using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wownet.MyPage
{
    /// <summary>
    /// <para>  나의 강연회 참여 현황 검색조건 </para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-11-08</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-11-08</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-11-08 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class MyLecturesCondition : BaseCondition
    {
        /// <summary>
        /// tradingCode 
        /// </summary>
        public string START_DATE { get; set; }

        /// <summary>
        /// 검색 타입
        /// </summary>
        public string END_DATE { get; set; }

        /// <summary>
        /// 검색 내용
        /// </summary>
        public string loginId { get; set; }
    }
}
