using System.Collections.Generic;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.History
{
    public class HistoryModel<T> : ListModel<T>
    {
        public List<NTB_CTGR> CTGRList { get; set; }
    }
}
