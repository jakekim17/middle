using System.Collections.Generic;
using Wow.Tv.Middle.Biz.IRCenter;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.IRClub;

namespace Wow.Tv.Middle.WcfService.IRCenter
{
    public class IRClubService : IIRClubService
    {
        public ListModel<TAB_IR_CLUB_2010> GetList(IRClubCondition condition)
        {
            return new IRClubBiz().GetList(condition);
        }

        public int Save(TAB_IR_CLUB_2010 model)
        {
            return new IRClubBiz().Save(model);
        }

        public void Delete(int[] deleteList)
        {
            new IRClubBiz().Delete(deleteList);
        }

        public TAB_IR_CLUB_2010 GetData(int seq)
        {
            return new IRClubBiz().GetData(seq);
        }

        public List<view_AllStockCode> GetStockCode(string searchText)
        {
            return new IRClubBiz().GetStockCode(searchText);
        }
    }
}
