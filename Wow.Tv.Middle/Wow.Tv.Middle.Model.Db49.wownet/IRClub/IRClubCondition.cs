using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wownet.IRClub
{
    public class IRClubCondition : BaseCondition
    {
        public string industryVolume { get; set; }
        public string regType { get; set; }
        public string searchType { get; set; }
        public string searchText { get; set; }
        public string viewFlag { get; set; }
    }
}
