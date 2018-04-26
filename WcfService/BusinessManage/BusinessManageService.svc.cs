using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.BusinessManage;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.BusinessManage;

namespace Wow.Tv.Middle.WcfService.BusinessManage
{
    public class BusinessManageService : IBusinessManageService
    {
        public void Delete(int[] seqList)
        {
            new BusinessManageBiz().Delete(seqList);
        }

        public BOARD_CONT_MENU GetDetail(int seq)
        {
            return new BusinessManageBiz().GetDetail(seq);
        }

        public int Save(NTB_BOARD_CONTENT model, LoginUser loginUser)
        {
            return new BusinessManageBiz().Save(model, loginUser);
        }

        public NTB_BOARD_CONTENT SearchData(int menuSeq)
        {
            return new BusinessManageBiz().SearchData(menuSeq);
        }

        public ListModel<BOARD_CONT_MENU> SearchList(BusinessCondition condition)
        {
            return new BusinessManageBiz().SearchList(condition);
        }

        public bool SendMobileSMS(string AppType, string mobileNum)
        {
            return new BusinessManageBiz().SendMobileSMS(AppType, mobileNum);
        }
    }
}
