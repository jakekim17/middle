﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wow.Tv.Middle.WcfService.PrivateMyPageService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PrivateMyPageService.IPrivateMyPageService")]
    public interface IPrivateMyPageService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetCashList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetCashListResponse")]
        Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result> GetCashList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.CashCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetCashList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetCashListResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result>> GetCashListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.CashCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetWowTvBalance", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetWowTvBalanceResponse")]
        Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.BillBalance GetWowTvBalance(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetWowTvBalance", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetWowTvBalanceResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.BillBalance> GetWowTvBalanceAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetBillBalance", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetBillBalanceResponse")]
        double GetBillBalance(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetBillBalance", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetBillBalanceResponse")]
        System.Threading.Tasks.Task<double> GetBillBalanceAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetRefundList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetRefundListResponse")]
        Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_REFUND_LST_Result> GetRefundList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.CashCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetRefundList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetRefundListResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_REFUND_LST_Result>> GetRefundListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.CashCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetCouponCount", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetCouponCountResponse")]
        int GetCouponCount(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, int useState);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetCouponCount", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetCouponCountResponse")]
        System.Threading.Tasks.Task<int> GetCouponCountAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, int useState);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetCouponList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetCouponListResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyServiceCondition))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.CashCondition))]
        Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_COUPON_LST_Result> GetCouponList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Common.BaseCondition condition, int useState);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetCouponList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetCouponListResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_COUPON_LST_Result>> GetCouponListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Common.BaseCondition condition, int useState);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/CouponChceck", ReplyAction="http://tempuri.org/IPrivateMyPageService/CouponChceckResponse")]
        Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.CouponResult CouponChceck(string couponNo, string chkmyinfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/CouponChceck", ReplyAction="http://tempuri.org/IPrivateMyPageService/CouponChceckResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.CouponResult> CouponChceckAsync(string couponNo, string chkmyinfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/RegisterCoupon", ReplyAction="http://tempuri.org/IPrivateMyPageService/RegisterCouponResponse")]
        string RegisterCoupon(Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.CouponResult couponResult, Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/RegisterCoupon", ReplyAction="http://tempuri.org/IPrivateMyPageService/RegisterCouponResponse")]
        System.Threading.Tasks.Task<string> RegisterCouponAsync(Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.CouponResult couponResult, Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetOrderList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetOrderListResponse")]
        Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderDelivery> GetOrderList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetOrderList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetOrderListResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderDelivery>> GetOrderListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetMyCashAt", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetMyCashAtResponse")]
        Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.WOWSP_GET_BALANCE_Result GetMyCashAt(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetMyCashAt", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetMyCashAtResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.WOWSP_GET_BALANCE_Result> GetMyCashAtAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetMainServiceList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetMainServiceListResponse")]
        Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result[] GetMainServiceList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetMainServiceList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetMainServiceListResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result[]> GetMainServiceListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetMyServiceList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetMyServiceListResponse")]
        Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMyServiceList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyServiceCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetMyServiceList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetMyServiceListResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result>> GetMyServiceListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyServiceCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/CancelRollback", ReplyAction="http://tempuri.org/IPrivateMyPageService/CancelRollbackResponse")]
        string CancelRollback(int seqNo, Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/CancelRollback", ReplyAction="http://tempuri.org/IPrivateMyPageService/CancelRollbackResponse")]
        System.Threading.Tasks.Task<string> CancelRollbackAsync(int seqNo, Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetVodDelivery", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetVodDeliveryResponse")]
        Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result> GetVodDelivery(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetVodDelivery", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetVodDeliveryResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result>> GetVodDeliveryAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetGiftDelivery", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetGiftDeliveryResponse")]
        Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderDelivery> GetGiftDelivery(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetGiftDelivery", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetGiftDeliveryResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderDelivery>> GetGiftDeliveryAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetMyClass", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetMyClassResponse")]
        Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyClassResult GetMyClass(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetMyClass", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetMyClassResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyClassResult> GetMyClassAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetMyCashInfo", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetMyCashInfoResponse")]
        Wow.Tv.Middle.Model.Db89.wowbill.USP_GetMYCurrentCash_Result GetMyCashInfo(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetMyCashInfo", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetMyCashInfoResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.wowbill.USP_GetMYCurrentCash_Result> GetMyCashInfoAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetUsedServiceList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetUsedServiceListResponse")]
        Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result[] GetUsedServiceList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetUsedServiceList", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetUsedServiceListResponse")]
        System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result[]> GetUsedServiceListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetCPNAME", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetCPNAMEResponse")]
        string GetCPNAME(decimal priceid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPrivateMyPageService/GetCPNAME", ReplyAction="http://tempuri.org/IPrivateMyPageService/GetCPNAMEResponse")]
        System.Threading.Tasks.Task<string> GetCPNAMEAsync(decimal priceid);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPrivateMyPageServiceChannel : Wow.Tv.Middle.WcfService.PrivateMyPageService.IPrivateMyPageService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PrivateMyPageServiceClient : System.ServiceModel.ClientBase<Wow.Tv.Middle.WcfService.PrivateMyPageService.IPrivateMyPageService>, Wow.Tv.Middle.WcfService.PrivateMyPageService.IPrivateMyPageService {
        
        public PrivateMyPageServiceClient() {
        }
        
        public PrivateMyPageServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PrivateMyPageServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PrivateMyPageServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PrivateMyPageServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result> GetCashList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.CashCondition condition) {
            return base.Channel.GetCashList(loginUserInfo, condition);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result>> GetCashListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.CashCondition condition) {
            return base.Channel.GetCashListAsync(loginUserInfo, condition);
        }
        
        public Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.BillBalance GetWowTvBalance(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser) {
            return base.Channel.GetWowTvBalance(loginUser);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.BillBalance> GetWowTvBalanceAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser) {
            return base.Channel.GetWowTvBalanceAsync(loginUser);
        }
        
        public double GetBillBalance(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser) {
            return base.Channel.GetBillBalance(loginUser);
        }
        
        public System.Threading.Tasks.Task<double> GetBillBalanceAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser) {
            return base.Channel.GetBillBalanceAsync(loginUser);
        }
        
        public Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_REFUND_LST_Result> GetRefundList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.CashCondition condition) {
            return base.Channel.GetRefundList(loginUserInfo, condition);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_REFUND_LST_Result>> GetRefundListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.wowbill.MyInfo.CashCondition condition) {
            return base.Channel.GetRefundListAsync(loginUserInfo, condition);
        }
        
        public int GetCouponCount(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, int useState) {
            return base.Channel.GetCouponCount(loginUserInfo, useState);
        }
        
        public System.Threading.Tasks.Task<int> GetCouponCountAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, int useState) {
            return base.Channel.GetCouponCountAsync(loginUserInfo, useState);
        }
        
        public Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_COUPON_LST_Result> GetCouponList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Common.BaseCondition condition, int useState) {
            return base.Channel.GetCouponList(loginUserInfo, condition, useState);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_COUPON_LST_Result>> GetCouponListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Common.BaseCondition condition, int useState) {
            return base.Channel.GetCouponListAsync(loginUserInfo, condition, useState);
        }
        
        public Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.CouponResult CouponChceck(string couponNo, string chkmyinfo) {
            return base.Channel.CouponChceck(couponNo, chkmyinfo);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.CouponResult> CouponChceckAsync(string couponNo, string chkmyinfo) {
            return base.Channel.CouponChceckAsync(couponNo, chkmyinfo);
        }
        
        public string RegisterCoupon(Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.CouponResult couponResult, Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.RegisterCoupon(couponResult, loginUserInfo);
        }
        
        public System.Threading.Tasks.Task<string> RegisterCouponAsync(Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.CouponResult couponResult, Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.RegisterCouponAsync(couponResult, loginUserInfo);
        }
        
        public Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderDelivery> GetOrderList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition) {
            return base.Channel.GetOrderList(loginUserInfo, condition);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderDelivery>> GetOrderListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition) {
            return base.Channel.GetOrderListAsync(loginUserInfo, condition);
        }
        
        public Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.WOWSP_GET_BALANCE_Result GetMyCashAt(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.GetMyCashAt(loginUserInfo);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.WOWSP_GET_BALANCE_Result> GetMyCashAtAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.GetMyCashAtAsync(loginUserInfo);
        }
        
        public Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result[] GetMainServiceList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.GetMainServiceList(loginUserInfo);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result[]> GetMainServiceListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.GetMainServiceListAsync(loginUserInfo);
        }
        
        public Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMyServiceList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyServiceCondition condition) {
            return base.Channel.GetMyServiceList(loginUserInfo, condition);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result>> GetMyServiceListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyServiceCondition condition) {
            return base.Channel.GetMyServiceListAsync(loginUserInfo, condition);
        }
        
        public string CancelRollback(int seqNo, Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.CancelRollback(seqNo, loginUserInfo);
        }
        
        public System.Threading.Tasks.Task<string> CancelRollbackAsync(int seqNo, Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.CancelRollbackAsync(seqNo, loginUserInfo);
        }
        
        public Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result> GetVodDelivery(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition) {
            return base.Channel.GetVodDelivery(loginUserInfo, condition);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result>> GetVodDeliveryAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition) {
            return base.Channel.GetVodDeliveryAsync(loginUserInfo, condition);
        }
        
        public Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderDelivery> GetGiftDelivery(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition) {
            return base.Channel.GetGiftDelivery(loginUserInfo, condition);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Common.ListModel<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderDelivery>> GetGiftDeliveryAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo, Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyOrderCondition condition) {
            return base.Channel.GetGiftDeliveryAsync(loginUserInfo, condition);
        }
        
        public Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyClassResult GetMyClass(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.GetMyClass(loginUserInfo);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage.MyClassResult> GetMyClassAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.GetMyClassAsync(loginUserInfo);
        }
        
        public Wow.Tv.Middle.Model.Db89.wowbill.USP_GetMYCurrentCash_Result GetMyCashInfo(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser) {
            return base.Channel.GetMyCashInfo(loginUser);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.wowbill.USP_GetMYCurrentCash_Result> GetMyCashInfoAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUser) {
            return base.Channel.GetMyCashInfoAsync(loginUser);
        }
        
        public Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result[] GetUsedServiceList(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.GetUsedServiceList(loginUserInfo);
        }
        
        public System.Threading.Tasks.Task<Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result[]> GetUsedServiceListAsync(Wow.Tv.Middle.Model.Common.LoginUserInfo loginUserInfo) {
            return base.Channel.GetUsedServiceListAsync(loginUserInfo);
        }
        
        public string GetCPNAME(decimal priceid) {
            return base.Channel.GetCPNAME(priceid);
        }
        
        public System.Threading.Tasks.Task<string> GetCPNAMEAsync(decimal priceid) {
            return base.Channel.GetCPNAMEAsync(priceid);
        }
    }
}
