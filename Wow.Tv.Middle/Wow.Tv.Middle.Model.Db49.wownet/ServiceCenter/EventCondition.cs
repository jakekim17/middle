using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wownet.ServiceCenter
{
    public class EventCondition : BaseCondition
    {
        public int Seq { get; set; }
        public String Title { get; set; }
        public String AdminId { get; set; }
        public DateTime Regdate { get; set; }
        public int Readcount { get; set; }
        public char ViewFlag { get; set; }
        public String SearchType { get; set; }
        public String SearchText { get; set; }
        public String CodeType { get; set; }
        public String EventType { get; set; }
        public String EventGubun { get; set; }
        public String BannerImage { get; set; }
    }
}
