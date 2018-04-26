using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.IR;
using Wow.Tv.Middle.Model.Db49.wownet;

namespace Wow.Tv.Middle.WcfService.IR
{
    public class IRService : IIRService
    {
        public List<TAB_STOCK_SITUATION> GetList()
        {
            return new IRBiz().GetList();
        }
    }
}
