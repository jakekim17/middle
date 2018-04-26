using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db22.stock.Admin
{
    public class HolidayMngCondition : BaseCondition
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Holiday { get; set; }
        public string Gubun { get; set; }
    }
}
