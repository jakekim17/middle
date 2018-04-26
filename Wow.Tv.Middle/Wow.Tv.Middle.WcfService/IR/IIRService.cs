using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Db49.wownet;

namespace Wow.Tv.Middle.WcfService.IR
{
    [ServiceContract]
    public interface IIRService
    {
        [OperationContract]
        List<TAB_STOCK_SITUATION> GetList();
    }
}
