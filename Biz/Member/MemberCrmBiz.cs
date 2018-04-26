using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db90.DNRS;

namespace Wow.Tv.Middle.Biz.Member
{
    public class MemberCrmBiz : BaseBiz
    {
        /// <summary>
        /// TV 다시보기 시청자 로그 기록
        /// </summary>
        public void TvReplayUserLog(int? userNumber, string userId, int tvReplayNum, string programId, string programName, DateTime? broadDate, string enterRoute)
        {
            string broadDateString = null;
            if (broadDate.HasValue == true)
            {
                broadDateString = broadDate.Value.ToString("yyyyMMddHHmm");
            }
            
            db89_wowbill.NUP_MEMBER_TV_VOD_CRM_LOG_INSERT(programId, tvReplayNum, broadDateString, programName, userNumber, userId, enterRoute);
        }
    }
}
