using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.TextAndLink;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    [ServiceContract]
    public interface ITextAndLinkService
    {
        [OperationContract]
        TxtNLinkModel<JOIN_TXTNLINK_CODE> GetList();

        [OperationContract]
        int Save(NTB_TEXT_LINK model, LoginUser loginUser);

        [OperationContract]
        void Delete(int[] deleteList);
    }
}
