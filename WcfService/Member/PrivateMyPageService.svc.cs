using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.MyPage;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.MyPage;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.MyInfo;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage;

namespace Wow.Tv.Middle.WcfService.Member
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IMyPageService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IPrivateMyPageService
    {
        [OperationContract]
        ListModel<UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result> GetCashList(LoginUserInfo loginUserInfo, CashCondition condition);

        [OperationContract]
        BillBalance GetWowTvBalance(LoginUserInfo loginUser);

        [OperationContract]
        double GetBillBalance(LoginUserInfo loginUser);
        
        [OperationContract]
        ListModel<UP_PORTAL_REFUND_LST_Result> GetRefundList(LoginUserInfo loginUserInfo, CashCondition condition);

        [OperationContract]
        int GetCouponCount(LoginUserInfo loginUserInfo, int useState);

        [OperationContract]
        ListModel<UP_PORTAL_COUPON_LST_Result> GetCouponList(LoginUserInfo loginUserInfo, BaseCondition condition,int useState);

        [OperationContract]
        CouponResult CouponChceck(string couponNo, string chkmyinfo);

        [OperationContract]
        string RegisterCoupon(CouponResult couponResult, LoginUserInfo loginUserInfo);

        [OperationContract]
        ListModel<MyOrderDelivery> GetOrderList(LoginUserInfo loginUserInfo, MyOrderCondition condition);

        [OperationContract]
        WOWSP_GET_BALANCE_Result GetMyCashAt(LoginUserInfo loginUserInfo);

        [OperationContract]
        List<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMainServiceList(LoginUserInfo loginUserInfo);

        [OperationContract]
        ListModel<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMyServiceList(LoginUserInfo loginUserInfo,MyServiceCondition condition);

        [OperationContract]
        string CancelRollback(int seqNo, LoginUserInfo loginUserInfo);

        [OperationContract]
        ListModel<UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result> GetVodDelivery(LoginUserInfo loginUserInfo,MyOrderCondition condition);

        [OperationContract]
        ListModel<MyOrderDelivery> GetGiftDelivery(LoginUserInfo loginUserInfo, MyOrderCondition condition);
        
        [OperationContract]
        MyClassResult GetMyClass(LoginUserInfo loginUserInfo);

        [OperationContract]
        USP_GetMYCurrentCash_Result GetMyCashInfo(LoginUserInfo loginUser);

        [OperationContract]
        List<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetUsedServiceList(LoginUserInfo loginUserInfo);

        [OperationContract]
        string GetCPNAME(decimal priceid);
    }
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "PrivateMyPageService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 PrivateMyPageService.svc나 PrivateMyPageService.svc.cs를 선택하고 디버깅을 시작하십시오.

    /// <summary>
    /// 89서버 사용시 쓰는 서비스
    /// </summary>
    public class PrivateMyPageService : IPrivateMyPageService
    {
        public ListModel<UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result> GetCashList(LoginUserInfo loginUserInfo, CashCondition condition)
        {
            return new MyPageBiz().GetCashList(loginUserInfo, condition);
        }

        public BillBalance GetWowTvBalance(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetWowTvBalance(loginUser, System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"]);
        }

        public double GetBillBalance(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetBillBalance(loginUser, "event-m", System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"]);
        }

        public ListModel<UP_PORTAL_REFUND_LST_Result> GetRefundList(LoginUserInfo loginUserInfo, CashCondition condition)
        {
            return new MyPageBiz().GetRefundList(loginUserInfo, condition);
        }

        public int GetCouponCount(LoginUserInfo loginUserInfo, int useState)
        {
            return new MyPageBiz().GetCouponCount(loginUserInfo, useState);
        }

        public ListModel<UP_PORTAL_COUPON_LST_Result> GetCouponList(LoginUserInfo loginUserInfo, BaseCondition condition, int useState)
        {
            return new MyPageBiz().GetCouponList(loginUserInfo, condition, useState);
        }

        public CouponResult CouponChceck(string couponNo, string chkmyinfo)
        {
            return new MyPageBiz().CouponChceck(couponNo, chkmyinfo);
        }

        public string RegisterCoupon(CouponResult couponResult, LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().RegisterCoupon(couponResult, loginUserInfo, System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"]);
        }

        public ListModel<MyOrderDelivery> GetOrderList(LoginUserInfo loginUserInfo, MyOrderCondition condition)
        {
            return new MyPageBiz().GetOrderList(loginUserInfo, condition);
        }

        public WOWSP_GET_BALANCE_Result GetMyCashAt(LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().GetMyCashAt(loginUserInfo);
        }

        public List<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMainServiceList(LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().GetMainServiceList(loginUserInfo);
        }

        public ListModel<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMyServiceList(LoginUserInfo loginUserInfo, MyServiceCondition condition)
        {
            return new MyPageBiz().GetMyServiceList(loginUserInfo, condition);
        }

        public string CancelRollback(int seqNo, LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().CancelRollback(seqNo, loginUserInfo, System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"]);
        }

        public ListModel<UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result> GetVodDelivery(LoginUserInfo loginUserInfo, MyOrderCondition condition)
        {
            return new MyPageBiz().GetVodDelivery(loginUserInfo, condition);
        }

        public ListModel<MyOrderDelivery> GetGiftDelivery(LoginUserInfo loginUserInfo, MyOrderCondition condition)
        {
            return new MyPageBiz().GetGiftDelivery(loginUserInfo, condition);
        }
        

        public MyClassResult GetMyClass(LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().GetMyClass(loginUserInfo);
        }

        public USP_GetMYCurrentCash_Result GetMyCashInfo(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetMyCashInfo(loginUser);
        }

        public List<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetUsedServiceList(LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().GetUsedServiceList(loginUserInfo);
        }

        public string GetCPNAME(decimal priceid)
        {
            return new MyPageBiz().GetCPNAME(priceid);
        }
    }
}
