using System.Collections.Generic;
using System.ServiceModel;
using Wow.Tv.Middle.Biz.Admin;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.ServiceCenter;

namespace Wow.Tv.Middle.WcfService.Admin
{
    [ServiceContract]
    public interface IServiceCenter
    {
        [OperationContract]
        ListModel<TAB_NOTICE> NolticeSearchList(NoticeCondition condition);


        [OperationContract]
        TAB_NOTICE GetNoticeSingle(int seq);

        [OperationContract]
        void NoticeSave(TAB_NOTICE model, LoginUser loginUser);

        [OperationContract]
        void NoticeDelete(int seq, LoginUser loginUser);

        [OperationContract]
        ListModel<TAB_FAQ> FAQSearchList(FAQCondition condition);


        [OperationContract]
        TAB_FAQ GetFAQSingle(int seq);

        [OperationContract]
        void FAQSave(TAB_FAQ model, LoginUser loginUser);

        [OperationContract]
        void FAQDelete(int seq, LoginUser loginUser);
    }

    public class ServiceCenter : IServiceCenter
    {
        #region 공지사항
        public void NoticeDelete(int seq, LoginUser loginUser)
        {
            new ServiceCenterBiz().NoticeDelete(seq, loginUser);
        }

        public TAB_NOTICE GetNoticeSingle(int seq)
        {
            return new ServiceCenterBiz().GetNoticeSingle(seq);
        }

        public List<TAB_BOARD_TOP> GetTopList(string codes)
        {
            return new ServiceCenterBiz().GetTopList(codes);
        }

        public void NoticeSave(TAB_NOTICE model, LoginUser loginUser)
        {
            new ServiceCenterBiz().NoticeSave(model, loginUser);
        }

        public ListModel<TAB_NOTICE> NolticeSearchList(NoticeCondition condition)
        {
            return new ServiceCenterBiz().NolticeSearchList(condition);
        }
        #endregion

        #region FAQ
        public void FAQDelete(int seq, LoginUser loginUser)
        {
            new ServiceCenterBiz().FAQDelete(seq, loginUser);
        }

        public TAB_FAQ GetFAQSingle(int seq)
        {
            return new ServiceCenterBiz().GetFAQSingle(seq);
        }
        

        public void  FAQSave(TAB_FAQ model, LoginUser loginUser)
        {
            new ServiceCenterBiz().FAQSave(model, loginUser);
        }

        public ListModel<TAB_FAQ> FAQSearchList(FAQCondition condition)
        {
            return new ServiceCenterBiz().FAQSearchList(condition);
        }

#endregion
    }
}
