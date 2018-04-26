using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.PostManage;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.StockholderBoard;

namespace Wow.Tv.Middle.WcfService.PostManage
{
    public class StockholderBoardService : IStockholderBoardService
    {  
        public ListModel<StockholderBoard> GetList(StockholderBoardCondition condition)
        {
            return new StockholderBoardBiz().GetList(condition);
        }

        public StockholderBoard GetDetail(int seq)
        {
            return new StockholderBoardBiz().GetDetail(seq);
        }

        public void BoardSave(NTB_STOCKHOLDER_BOARD model)
        {
            new StockholderBoardBiz().BoardSave(model);
        }

        public int ReplySave(NTB_STOCKHOLDER_BOARD model, LoginUser loginUser)
        {
            return new StockholderBoardBiz().ReplySave(model, loginUser);
        }

        public void Delete(int[] deleteList)
        {
            new StockholderBoardBiz().Delete(deleteList);
        }

        public void UpdateBlind(int seq, string blindYn, LoginUser loginUser)
        {
            new StockholderBoardBiz().UpdateBlind(seq, blindYn, loginUser);
        }

        public void UpdateReadCnt(int seq)
        {
            new StockholderBoardBiz().UpdateReadCnt(seq);
        }

        public int GetMaxBoardNum()
        {
            return new StockholderBoardBiz().GetMaxBoardNum();
        }
        public int GetMinBoardNum()
        {
            return new StockholderBoardBiz().GetMinBoardNum();
        }
    }
}
