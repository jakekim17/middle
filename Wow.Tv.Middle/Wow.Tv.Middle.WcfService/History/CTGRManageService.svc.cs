using System;
using System.Collections.Generic;
using Wow.Tv.Middle.Biz.History;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.History;

namespace Wow.Tv.Middle.WcfService.History
{
    public class CTGRManageService : ICTGRManageService
    {
        public CTGRModel<NTB_CTGR> GetList()
        {
            return new CTGRManageBiz().GetList();
        }

        public int Save(NTB_CTGR model, LoginUser loginUser)
        {
            return new CTGRManageBiz().Save(model, loginUser);
        }

        public void Delete(int seq)
        {
            new CTGRManageBiz().Delete(seq);
        }

        public string GetMaxYear()
        {
            return new CTGRManageBiz().GetMaxYear();
        }

        public List<NTB_CTGR> GetCTGRList()
        {
            return new CTGRManageBiz().GetCTGRList();
        }
    }
}
