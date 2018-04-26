using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.StockholderBoard;

namespace Wow.Tv.Middle.WcfService.PostManage
{
    [ServiceContract]
    public interface IStockholderBoardService
    {
        [OperationContract]
        ListModel<StockholderBoard> GetList(StockholderBoardCondition condition);

        [OperationContract]
        StockholderBoard GetDetail(int seq);

        [OperationContract]
        void BoardSave(NTB_STOCKHOLDER_BOARD model);

        [OperationContract]
        int ReplySave(NTB_STOCKHOLDER_BOARD model, LoginUser loginUser);

        [OperationContract]
        void Delete(int[] deleteList);

        [OperationContract]
        void UpdateBlind(int seq, string blindYn, LoginUser loginUser);

        [OperationContract]
        void UpdateReadCnt(int seq);

        [OperationContract]
        int GetMaxBoardNum();

        [OperationContract]
        int GetMinBoardNum();
    }
}
