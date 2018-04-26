using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db89.wowbill.MemberManage
{
    public class MemberMonitoringCondition : BaseCondition
    {
        public DateTime? RegistDateFrom { get; set; }
        public DateTime? RegistDateTo { get; set; }
        public DateTime? LatestConnectDateFrom { get; set; }
        public DateTime? LatestConnectDateTo { get; set; }

        public string SearchType { get; set; }
        public string SearchText { get; set; }

        public int UserNumber { get; set; }
        public string ParameterMessage { get; set; }
    }
}
