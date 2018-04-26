using System;
using System.Collections.Generic;
using Wow.Tv.Middle.Biz.History;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.History;

namespace Wow.Tv.Middle.WcfService.History
{
    public class HistoryService : IHistoryService
    {
        public HistoryModel<HIS_CTGR> GetList(HistoryCondition codition)
        {
            return new HistoryBiz().GetList(codition);
        }

        public HistoryDetail GetDetail(int seq)
        {
            return new HistoryBiz().GetDetail(seq);
        }

        public void Delete(int[] seqList)
        {
            new HistoryBiz().Delete(seqList);
        }

        public int Save(NTB_HIS_MNG model, LoginUser loginUser)
        {
            return new HistoryBiz().Save(model, loginUser);
        }

        public List<DtlCTGRHistory> SearchHistory(string SearchYear)
        {
            return new HistoryBiz().SearchHistory(SearchYear);
        }
    }
}
