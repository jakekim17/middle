using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db49.wownet;

namespace Wow.Tv.Middle.Biz.IR
{
    public class IRBiz : BaseBiz
    {
        public List<TAB_STOCK_SITUATION> GetList()
        {
            return db49_wownet.TAB_STOCK_SITUATION.ToList();
        }
    }
}
