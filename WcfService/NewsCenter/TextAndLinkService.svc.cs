using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.NewsCenter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.TextAndLink;

namespace Wow.Tv.Middle.WcfService.NewsCenter
{
    public class TextAndLinkService : ITextAndLinkService
    {
        public TxtNLinkModel<JOIN_TXTNLINK_CODE> GetList()
        {
            return new TextAndLinkBiz().GetList();
        }

        public int Save(NTB_TEXT_LINK model, LoginUser loginUser)
        {
            return new TextAndLinkBiz().Save(model, loginUser);
        }

        public void Delete(int[] deleteList)
        {
            new TextAndLinkBiz().Delete(deleteList);
        }
    }
}
