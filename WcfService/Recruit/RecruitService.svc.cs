using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Recruit;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.Recruit;

namespace Wow.Tv.Middle.WcfService.Recruit
{
    public class RecruitService : IRecruitService
    {
        public NUP_RECRUIT_SELECT_Result GetApplicantInfo(RecruitCondition condition)
        {
            return new RecruitBiz().GetApplicantInfo(condition);
        }

        public void IncreaseViewCnt(int seq)
        {
            new RecruitBiz().IncreaseViewCnt(seq);
        }

        public void SaveRecruit(tblRecruit model)
        {
            new RecruitBiz().SaveRecruit(model);
        }

        public List<NUP_RECRUIT_SELECT_Result> SearchList(RecruitCondition condition)
        {
            return new RecruitBiz().SearchList(condition);
        }
    }
}
