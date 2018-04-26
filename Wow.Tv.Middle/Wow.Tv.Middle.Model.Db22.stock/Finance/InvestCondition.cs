using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db22.stock.Finance
{
    public class InvestCondition : BaseCondition
    {
        /// <summary>
        /// 100일 이하 int형 값
        /// </summary>
        public int fromDate { get; set; }
    }
}
