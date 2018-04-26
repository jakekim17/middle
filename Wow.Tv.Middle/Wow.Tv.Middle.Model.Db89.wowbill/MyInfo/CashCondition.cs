using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db89.wowbill.MyInfo
{ 
    /// <summary>
    /// <para>  나의캐시 검색조건 </para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-10-31</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-10-31</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-10-31 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class CashCondition : BaseCondition
    {
        public DateTime START_DATE { get; set; }

        public DateTime END_DATE { get; set; }

        public string LoginId { get; set; }

        public decimal nBalance { get; set; }

       public  decimal EventBalance { get; set; }
        /// <summary>
        /// 검색 내용
        /// </summary>
        public string SearchText { get; set; } = string.Empty;
    }
}
