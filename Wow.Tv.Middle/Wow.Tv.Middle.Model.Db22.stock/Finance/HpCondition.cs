using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public class HpCondition
    {
        public string Trid { get; set; }
        public string Code { get; set; }

        /*차트만 사용*/
        public string Width { get; set; } = "970";
        public string Height { get; set; } = "400";

    }
}
