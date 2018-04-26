using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db89.wowbill.Recruit
{
    public class RecruitCondition: BaseCondition
    {
        /// <summary>
        /// 시퀀스 번호
        /// </summary>
        public int SearchSeq { get; set; }

        /// <summary>
        /// 이름
        /// </summary>
        public string SearchName { get; set; }

        /// <summary>
        /// 주민번호
        /// </summary>
        public string SearchSsno { get; set; }

        /// <summary>
        /// 비밀번호
        /// </summary>
        public string SearchPassword { get; set; }

        /// <summary>
        /// 현재 페이지
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 합격 여부
        /// </summary>
       public string Passed { get; set; }
    }
}
