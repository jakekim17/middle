using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wownet;

namespace Wow.Tv.Middle.Biz.Pilot
{
    public class Test : BaseBiz
    {
        public TAB_BOARD_AA GetAt()
        {
            return db49_wownet.TAB_BOARD.SingleOrDefault(a => a.SEQ == 3);
        }
    }
}
