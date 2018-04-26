using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Biz.Admin;
using Wow.Tv.Middle.Biz.MyPage;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.MyPage;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.MyPage;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Board;
using Wow.Tv.Middle.Model.Db49.wowtv.MyPage;
using Wow.Tv.Middle.Model.Db51.ARSConsult;
using Wow.Tv.Middle.Model.Db51.contents;
using Wow.Tv.Middle.Model.Db51.WOWMMS;
using Wow.Tv.Middle.Model.Db89.wowbill;
using Wow.Tv.Middle.Model.Db89.wowbill.MyInfo;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB;
using Wow.Tv.Middle.Model.Db89.WOWTV_BILL_DB.MyPage;
using Wow.Tv.Middle.Model.Db90.DNRS;

namespace Wow.Tv.Middle.WcfService.MyPage
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IMyActive"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IMyActiveService
    {
        [OperationContract]
        ListModel<NUP_SC_LOG_SELECT_Result> GetSmsSearchList(ScLogCondition condition);

        [OperationContract]
        ListModel<NUP_MMS_LOG_SELECT_Result> GetLmsSearchList(ScLogCondition condition);

        [OperationContract]
        IList<TAB_SCRAP_CATEGORY> GetScrapCategory(LoginUserInfo loginUser);

        [OperationContract]
        void ScrapCategoryDelete(int seq);

        [OperationContract]
        void UpdateScrapCategory(int seq, string folder);


        [OperationContract]
        IList<TAB_SCRAP_MENU> GetScrapMenu(LoginUserInfo loginUser);

        [OperationContract]
        void ScrapMenuDelete(int seq, LoginUserInfo loginUser);

        [OperationContract]
        void UpdateScrapMenu(int seq, string folderName);

        [OperationContract]
        void SaveScrapMenu(string folderName, LoginUserInfo loginUser);

        [OperationContract]
        ListModel<JOIN_TAB_SCRAP_CONTENT> GetNewsPin(MyPinCondition condition, LoginUserInfo loginUser);

        [OperationContract]
        void NewsPinDelete(int seq, LoginUserInfo loginUser);


        [OperationContract]
        ListModel<NUP_MYPIN_REPORTER_SELECT_Result> GetReporterPin(MyPinCondition condition, LoginUserInfo loginUser);

        [OperationContract]
        void ReporterPinDelete(int seq, LoginUserInfo loginUser);

        [OperationContract]
        ListModel<NTB_MYPIN_PROGRAM> GetProgramPin(MyPinCondition condition, LoginUserInfo loginUser);

        [OperationContract]
        void ProgramPinDelete(int seq, LoginUserInfo loginUser);

        [OperationContract]
        ListModel<NTB_MYPIN_PARTNER> GetPartnerPin(MyPinCondition condition, LoginUserInfo loginUser);

        [OperationContract]
        void PartnerPinDelete(int seq, LoginUserInfo loginUser);

        [OperationContract]
        IList<T_RUNDOWN> GetRecommendProgram();

        [OperationContract]
        IList<USP_GetRecommendPro3_Result> GetRecommendPartner();

        [OperationContract]
        IList<USP_GetRecommendPro3_Result> GetStockRecommendPartner();

        [OperationContract]
        ListModel<TAB_CONSULTATION_APPLICATION> GetStockConsultList(MyWriteCollectCondition condition);

        [OperationContract]
        ListModel<TAB_BOARD_AA> GetStockDebateList(MyWriteCollectCondition condition);

        [OperationContract]
        ListModel<TAB_BOARD_AA> GetDealList(MyWriteCollectCondition condition);

        [OperationContract]
        ListModel<TAB_BOARD_AA> GetStockInfoList(MyWriteCollectCondition condition);

        [OperationContract]
        void SavePartner(NTB_MYPIN_PARTNER mypin, LoginUserInfo loginUser);

        [OperationContract]
        void SaveProgram(NTB_MYPIN_PROGRAM mypin, LoginUserInfo loginUser);

        [OperationContract]
        void SaveReporter(NTB_MYPIN_REPORTER mypin, LoginUserInfo loginUser);

        [OperationContract]
        void SaveNews(TAB_SCRAP_CONTENT mypin, LoginUserInfo loginUser);

        [OperationContract]
        USP_GetMYCurrentCash_Result GetMyCashInfo(LoginUserInfo loginUser);


        [OperationContract]
        ListModel<UP_PORTAL_COUPON_LST_Result> GetCouponList(LoginUserInfo loginUserInfo, BaseCondition condition, int useState);

        [OperationContract]
        ListModel<UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result> GetCashList(LoginUserInfo loginUserInfo, CashCondition condition);

        [OperationContract]
        ListModel<UP_PORTAL_REFUND_LST_Result> GetRefundList(LoginUserInfo loginUserInfo, CashCondition condition);

        [OperationContract]
        CouponResult CouponChceck(string couponNo, string chkmyinfo);

        [OperationContract]
        string RegisterCoupon(CouponResult couponResult, LoginUserInfo loginUserInfo);

        [OperationContract]
        ListModel<MyOrderDelivery> GetOrderList(LoginUserInfo loginUserInfo, MyOrderCondition condition);

        [OperationContract]
        int GetCouponCount(LoginUserInfo loginUserInfo, int useState);

        [OperationContract]
        IntegratedBoard<NTB_BOARD_CONTENT> MyMainIntegratedSearchCount(IntegratedBoardCondition condition);

        [OperationContract]
        Pro_wowList GetMyPartnerAt(LoginUserInfo loginUserInfo);

        [OperationContract]
        WOWSP_GET_BALANCE_Result GetMyCashAt(LoginUserInfo loginUserInfo);

        [OperationContract]
        ListModel<JOIN_TAB_SCRAP_CONTENT> GetMainNewsPin(LoginUserInfo loginUser);

        [OperationContract]
        ListModel<NTB_MYPIN_PARTNER> GetMainPartnerPin(LoginUserInfo loginUser);

        [OperationContract]
        ListModel<NTB_MYPIN_PROGRAM> GetMainProgramPin(LoginUserInfo loginUser);

        [OperationContract]
        ListModel<NUP_MYPIN_REPORTER_SELECT_Result> GetMainReporterPin(LoginUserInfo loginUser);

        [OperationContract]
        List<NTB_BOARD_CONTENT> GetFAQList(int menuSeq);

        [OperationContract]
        List<FN_JOIN_CAFE_LIST_New_Result> GetJoinCafeList(LoginUserInfo loginUserInfo);

        [OperationContract]
        TAB_SCRAP_CONTENT SingleNewsPin(string articleId, LoginUserInfo loginUser);

        [OperationContract]
        List<usp_getInvestmentPartners_Result> GetInvestmentPartnerList(LoginUserInfo loginUserInfo);

        [OperationContract]
        ListModel<NUP_MY_LECTURES_SELECT_Result> GetLecturesList(MyLecturesCondition condition);

        [OperationContract]
        void SaveMyFavoriteJongMok(LoginUserInfo loginUserInfo, string stockcode);

        [OperationContract]
        void RemoveMyFavoriteJongMok(LoginUserInfo loginUserInfo, string stockcode);

        [OperationContract]
        ListModel<tblMyFavoriteJongMok> GetMyFavoriteJongMokList(LoginUserInfo loginUserInfo,MyJongMokCondition condition);

        [OperationContract]
        double GetMyChangePercent(LoginUserInfo loginUserInfo);

        [OperationContract]
        List<tblStockBatch> GetStockList(LoginUserInfo loginUserInfo, string searchText, string searchValue);

        [OperationContract]
        List<tblMyFavoriteJongMok> GetPopupMyFavoriteJongMokList(LoginUserInfo loginUserInfo);

        [OperationContract]
        List<TAB_MY_STOCK> GetMyStock(LoginUserInfo loginUserInfo);

        [OperationContract]
        void RemoveMyStock(LoginUserInfo loginUserInfo, int seq);

        [OperationContract]
        void SaveMyStock(LoginUserInfo loginUserInfo, TAB_MY_STOCK tabMyStock);

        [OperationContract]
        void SaveMyFavoriteJongMokRange(LoginUserInfo loginUserInfo, IList<string> items);

        [OperationContract]
        List<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMainServiceList(LoginUserInfo loginUserInfo);

        [OperationContract]
        ListModel<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMyServiceList(LoginUserInfo loginUserInfo,MyServiceCondition condition);

        [OperationContract]
        string CancelRollback(int seqNo, LoginUserInfo loginUserInfo);

        [OperationContract]
        bool IsMyFavoriteJongMok(LoginUserInfo loginUserInfo, string stockcode);

        [OperationContract]
        ListModel<MyOrderDelivery> GetGiftDelivery(LoginUserInfo loginUserInfo, MyOrderCondition condition);

        [OperationContract]
        ListModel<UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result> GetVodDelivery(LoginUserInfo loginUserInfo,MyOrderCondition condition);

        [OperationContract]
        ListModel<TAB_BOARD_AA> GetReport119(LoginUserInfo loginUserInfo, MyPageCondition condition);

        [OperationContract]
        TAB_BOARD_AA GetReport119Detail(int seq);

        [OperationContract]
        string TestGetBillBalance(string usernumber, string gamecode);

        [OperationContract]
        MyClassResult GetMyClass(LoginUserInfo loginUserInfo);

        [OperationContract]
        BillBalance GetWowTvBalance(LoginUserInfo loginUser);

        [OperationContract]
        double GetBillBalance(LoginUserInfo loginUser);

        [OperationContract]
        List<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetUsedServiceList(LoginUserInfo loginUserInfo);

        [OperationContract]
        tblUser GetMemberInfo(string userId);

        [OperationContract]
        string GetCPNAME(decimal priceid);

        [OperationContract]
        ListModel<NTB_MYPIN_PROGRAM> GetQuickProgramPin(LoginUserInfo loginUser);

        [OperationContract]
        ListModel<NTB_MYPIN_PARTNER> GetQuickPartnerPin(LoginUserInfo loginUser);

        [OperationContract]
        string GetPartnerCafeDomain(string wowtvId);


    }
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "MyActive"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 MyActive.svc나 MyActive.svc.cs를 선택하고 디버깅을 시작하십시오.
    public class MyActiveService : IMyActiveService
    {
        public ListModel<NUP_SC_LOG_SELECT_Result> GetSmsSearchList(ScLogCondition condition)
        {
            return new MyPageBiz().GetSmsSearchList(condition);
        }

        public ListModel<NUP_MMS_LOG_SELECT_Result> GetLmsSearchList(ScLogCondition condition)
        {
            return new MyPageBiz().GetLmsSearchList(condition);
        }

        public IList<TAB_SCRAP_CATEGORY> GetScrapCategory(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetScrapCategory(loginUser);
        }

        public void ScrapCategoryDelete(int seq)
        {
            new MyPageBiz().ScrapCategoryDelete(seq);
        }

        public void UpdateScrapCategory(int seq, string folder)
        {
            new MyPageBiz().UpdateScrapCategory(seq, folder);
        }

        public IList<TAB_SCRAP_MENU> GetScrapMenu(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetScrapMenu(loginUser);
        }

        public void ScrapMenuDelete(int seq, LoginUserInfo loginUser)
        {
            new MyPageBiz().ScrapMenuDelete(seq, loginUser);
        }

        public void UpdateScrapMenu(int seq, string folderName)
        {
            new MyPageBiz().UpdateScrapMenu(seq, folderName);
        }

        public void SaveScrapMenu(string folderName, LoginUserInfo loginUser)
        {
            new MyPageBiz().SaveScrapMenu(folderName, loginUser);
        }

        public ListModel<JOIN_TAB_SCRAP_CONTENT> GetNewsPin(MyPinCondition condition, LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetNewsPin(condition, loginUser);
        }

        public void NewsPinDelete(int seq, LoginUserInfo loginUser)
        {
            new MyPageBiz().NewsPinDelete(seq, loginUser);
        }

        public ListModel<NUP_MYPIN_REPORTER_SELECT_Result> GetReporterPin(MyPinCondition condition, LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetReporterPin(condition, loginUser);
        }

        public void ReporterPinDelete(int seq, LoginUserInfo loginUser)
        {
            new MyPageBiz().ReporterPinDelete(seq, loginUser);
        }


        public ListModel<NTB_MYPIN_PROGRAM> GetProgramPin(MyPinCondition condition, LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetProgramPin(condition, loginUser);
        }

        public void ProgramPinDelete(int seq, LoginUserInfo loginUser)
        {
            new MyPageBiz().ProgramPinDelete(seq, loginUser);
        }


        public ListModel<NTB_MYPIN_PARTNER> GetPartnerPin(MyPinCondition condition, LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetPartnerPin(condition, loginUser);
        }

        public void PartnerPinDelete(int seq, LoginUserInfo loginUser)
        {
            new MyPageBiz().PartnerPinDelete(seq, loginUser);
        }

        public IList<T_RUNDOWN> GetRecommendProgram()
        {
            return new MyPageBiz().GetRecommendProgram();
        }
        public IList<USP_GetRecommendPro3_Result> GetRecommendPartner()
        {
            return new MyPageBiz().GetRecommendPartner();
        }
        public IList<USP_GetRecommendPro3_Result> GetStockRecommendPartner()
        {
            return new MyPageBiz().GetRecommendPartner(true);
        }

        public ListModel<TAB_CONSULTATION_APPLICATION> GetStockConsultList(MyWriteCollectCondition condition)
        {
            return new MyPageBiz().GetStockConsultList(condition);
        }

        public ListModel<TAB_BOARD_AA> GetStockDebateList(MyWriteCollectCondition condition)
        {
            return new MyPageBiz().GetStockDebateList(condition);
        }

        public ListModel<TAB_BOARD_AA> GetDealList(MyWriteCollectCondition condition)
        {
            return new MyPageBiz().GetDealList(condition);
        }

        public ListModel<TAB_BOARD_AA> GetStockInfoList(MyWriteCollectCondition condition)
        {
            return new MyPageBiz().GetStockInfoList(condition);
        }

        #region 마이핀 
        public void SavePartner(NTB_MYPIN_PARTNER mypin, LoginUserInfo loginUser)
        {
            new MyPageBiz().SavePartner(mypin, loginUser);
        }

        public void SaveProgram(NTB_MYPIN_PROGRAM mypin, LoginUserInfo loginUser)
        {
            new MyPageBiz().SaveProgram(mypin, loginUser);
        }

        public void SaveReporter(NTB_MYPIN_REPORTER mypin, LoginUserInfo loginUser)
        {
            new MyPageBiz().SaveReporter(mypin, loginUser);
        }

        public void SaveNews(TAB_SCRAP_CONTENT mypin, LoginUserInfo loginUser)
        {
            new MyPageBiz().SaveNews(mypin, loginUser);
        }
        #endregion

        #region 나의캐시

        public USP_GetMYCurrentCash_Result GetMyCashInfo(LoginUserInfo loginUser)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetMyCashInfo(loginUser);
            //return null;
            //return db89_wowbill.USP_GetMYCurrentCash(userInfo.userNumber.ToString()).SingleOrDefault();
            //return new MyPageBiz().GetMyCashInfo(loginUser);
        }

        /// <summary>
        ///  PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public double GetBillBalance(LoginUserInfo loginUser)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetBillBalance(loginUser);
        }

        /// <summary>
        ///  PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public BillBalance GetWowTvBalance(LoginUserInfo loginUser)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetWowTvBalance(loginUser);
        }

        /// <summary>
        ///  PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<UP_PORTAL_MYPAGE_PAYNCHARGE_UR_LST_Result> GetCashList(LoginUserInfo loginUserInfo, CashCondition condition)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetCashList(loginUserInfo, condition);
        }

        /// <summary>
        ///  PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<UP_PORTAL_REFUND_LST_Result> GetRefundList(LoginUserInfo loginUserInfo, CashCondition condition)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetRefundList(loginUserInfo, condition);
        }

        #endregion

        #region 나의쿠폰

        /// <summary>
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="condition"></param>
        /// <param name="useState"></param>
        /// <returns></returns>
        public ListModel<UP_PORTAL_COUPON_LST_Result> GetCouponList(LoginUserInfo loginUserInfo, BaseCondition condition, int useState)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetCouponList(loginUserInfo, condition, useState);
            //return new MyPageBiz().GetCouponList(loginUserInfo, condition, useState);
        }
        /// <summary>UP_PORTAL_COUPON_LST_Result
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="couponNo"></param>
        /// <returns></returns>
        public CouponResult CouponChceck(string couponNo, string chkmyinfo)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().CouponChceck(couponNo, chkmyinfo);
            //return new MyPageBiz().CouponChceck(couponNo);
        }

        /// <summary>
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="couponResult"></param>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        public string RegisterCoupon(CouponResult couponResult, LoginUserInfo loginUserInfo)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().RegisterCoupon(couponResult, loginUserInfo);
            //return new MyPageBiz().RegisterCoupon(couponResult, loginUserInfo, System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"]);
        }

        /// <summary>
        /// 회원정보 가져오기
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public tblUser GetMemberInfo(string userId)
        {
            return new PriMiddleService.MemberInfoServiceClient().GetMemberInfo(userId);
        }

        public string GetCPNAME(decimal priceid)
        {
           return new PrivateMyPageService.PrivateMyPageServiceClient().GetCPNAME(priceid);
            //return new MyPageBiz().GetCPNAME(5210);
        }

        #endregion

        #region 나의 서비스,주문,배송

        /// <summary>
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<MyOrderDelivery> GetOrderList(LoginUserInfo loginUserInfo, MyOrderCondition condition)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetOrderList(loginUserInfo, condition);
            //return new MyPageBiz().GetOrderList(loginUserInfo, condition);
        }
        #endregion

        #region  나의 가입 카페

        public List<FN_JOIN_CAFE_LIST_New_Result> GetJoinCafeList(LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().GetJoinCafeList(loginUserInfo);
        }
        #endregion

        #region 마이페이지 메인 화면

        /// <summary>
        /// 나의 보유 쿠폰 PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="useState"></param>
        /// <returns></returns>
        public int GetCouponCount(LoginUserInfo loginUserInfo, int useState)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetCouponCount(loginUserInfo, useState);
            //return new MyPageBiz().GetCouponCount(loginUserInfo,useState);
        }

        /// <summary>
        /// 나의 1:1문의 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IntegratedBoard<NTB_BOARD_CONTENT> MyMainIntegratedSearchCount(IntegratedBoardCondition condition)
        {
            return new IntegratedBoardBiz().MyMainIntegratedSearchCount(condition);
        }

        /// <summary>
        /// 나의 투자 파트너
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        public Pro_wowList GetMyPartnerAt(LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().GetMyPartnerAt(loginUserInfo);
        }

        /// <summary>
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        public WOWSP_GET_BALANCE_Result GetMyCashAt(LoginUserInfo loginUserInfo)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetMyCashAt(loginUserInfo);
            //return new MyPageBiz().GetMyCashAt(loginUserInfo);
        }

        public ListModel<JOIN_TAB_SCRAP_CONTENT> GetMainNewsPin(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetMainNewsPin(  loginUser);
        }

        public ListModel<NTB_MYPIN_PARTNER> GetMainPartnerPin(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetMainPartnerPin( loginUser);
        }

        public ListModel<NTB_MYPIN_PROGRAM> GetMainProgramPin(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetMainProgramPin( loginUser);
        }

        public ListModel<NUP_MYPIN_REPORTER_SELECT_Result> GetMainReporterPin(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetMainReporterPin( loginUser);
        }

        public List<NTB_BOARD_CONTENT> GetFAQList(int menuSeq)
        {
            return new IntegratedBoardBiz().GetFAQList(menuSeq);
        }


        #endregion

        #region 프론트 웹에서 사용

        public TAB_SCRAP_CONTENT SingleNewsPin(string articleId, LoginUserInfo loginUser)
        {
            return new MyPageBiz().SingleNewsPin(articleId,loginUser);
        }
        #endregion

        #region  나의 투자 파트너

        public List<usp_getInvestmentPartners_Result> GetInvestmentPartnerList(LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().GetInvestmentPartnerList(loginUserInfo);
        }

        #endregion

        #region 나의 강연회 참여현황

        public ListModel<NUP_MY_LECTURES_SELECT_Result> GetLecturesList(MyLecturesCondition condition)
        {
            return new MyPageBiz().GetLecturesList(condition);
        }
        #endregion

        #region 나의 관심종목 보유종목
        /// <summary>
        /// 관심 종목 등록
        /// </summary>
        /// <param name="loginUserInfo">로그인정보</param>
        /// <param name="stockcode">종목코드</param>
        public void SaveMyFavoriteJongMok(LoginUserInfo loginUserInfo, string stockcode)
        {
            new MyPageBiz().SaveMyFavoriteJongMok(loginUserInfo, stockcode);
        }


        public void RemoveMyFavoriteJongMok(LoginUserInfo loginUserInfo, string stockcode)
        {
            new MyPageBiz().RemoveMyFavoriteJongMok(loginUserInfo, stockcode);
        }

        public ListModel<tblMyFavoriteJongMok> GetMyFavoriteJongMokList(LoginUserInfo loginUserInfo,MyJongMokCondition condition)
        {
            return new MyPageBiz().GetMyFavoriteJongMokList(loginUserInfo,condition);
        }

        public double GetMyChangePercent(LoginUserInfo loginUserInfo)
        {
            return new  MyPageBiz().GetMyChangePercent(loginUserInfo);
        }

        public List<tblStockBatch> GetStockList(LoginUserInfo loginUserInfo,string searchText, string searchValue)
        {
            return new MyPageBiz().GetStockList(loginUserInfo,searchText, searchValue);
        }

        public List<tblMyFavoriteJongMok> GetPopupMyFavoriteJongMokList(LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().GetPopupMyFavoriteJongMokList(loginUserInfo);
        }

        public List<TAB_MY_STOCK> GetMyStock(LoginUserInfo loginUserInfo)
        {
            return new MyPageBiz().GetMyStock(loginUserInfo);
        }

        public void RemoveMyStock(LoginUserInfo loginUserInfo, int seq)
        {
            new MyPageBiz().RemoveMyStock(loginUserInfo, seq);
        }

        public void SaveMyStock(LoginUserInfo loginUserInfo, TAB_MY_STOCK tabMyStock)
        {
            new MyPageBiz().SaveMyStock(loginUserInfo, tabMyStock);
        }

        public void SaveMyFavoriteJongMokRange(LoginUserInfo loginUserInfo, IList<string> items)
        {
            new MyPageBiz().SaveMyFavoriteJongMokRange(loginUserInfo, items);
        }

        public bool IsMyFavoriteJongMok(LoginUserInfo loginUserInfo, string stockcode)
        {
            return new MyPageBiz().IsMyFavoriteJongMok(loginUserInfo, stockcode);
        }
        #endregion

        #region 서비스 이용내역
        /// <summary>
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        public List<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMainServiceList(LoginUserInfo loginUserInfo)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetMainServiceList(loginUserInfo).ToList();
            //return new MyPageBiz().GetMainServiceList(loginUserInfo);
        }
        /// <summary>
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetMyServiceList(LoginUserInfo loginUserInfo,MyServiceCondition condition)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetMyServiceList(loginUserInfo, condition);
            //return new MyPageBiz().GetMyServiceList(loginUserInfo, condition);
        }

        /// <summary>
        /// 해지신청 후 접수취소
        /// </summary>
        /// <param name="seqNo"></param>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        public string CancelRollback(int seqNo, LoginUserInfo loginUserInfo)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().CancelRollback(seqNo, loginUserInfo);
            //return new MyPageBiz().CancelRollback(seqNo, loginUserInfo, System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"]);
            //return new MyPageBiz().RegisterCoupon(couponResult, loginUserInfo, System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"]);
        }

        #endregion

        #region 주문 배송내역
        /// <summary>
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<MyOrderDelivery> GetGiftDelivery(LoginUserInfo loginUserInfo, MyOrderCondition condition)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetGiftDelivery(loginUserInfo, condition);
            //return new MyPageBiz().GetGiftDelivery(loginUserInfo,condition);
        }

        /// <summary>
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<UP_PORTAL_MYPAGE_DELIVERY_UR_LST_Result> GetVodDelivery(LoginUserInfo loginUserInfo, MyOrderCondition condition)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetVodDelivery(loginUserInfo, condition);
            //return new MyPageBiz().GetVodDelivery(loginUserInfo, condition);
        }
        #endregion

        #region 불공정거래신고
        /// <summary>
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ListModel<TAB_BOARD_AA> GetReport119(LoginUserInfo loginUserInfo, MyPageCondition condition)
        {
            //return new PrivateMyPageService.PrivateMyPageServiceClient().GetReport119(loginUserInfo, condition);
            return new MyPageBiz().GetReport119(loginUserInfo, condition);
        }

        public TAB_BOARD_AA GetReport119Detail(int seq)
        {
            return new MyPageBiz().GetReport119Detail(seq);
        }
        #endregion

        #region 테스트로 확인하는 빌링 메서드
        public string TestGetBillBalance(string usernumber, string gamecode)
        {
            return new MyPageBiz().TestGetBillBalance(usernumber, "event-m", System.Configuration.ConfigurationManager.AppSettings["BOQv5BillHost"]);
        }
        #endregion

        #region 나의 회원등급
        /// <summary>
        /// PrivateMyPageService 이용
        /// </summary>
        /// <param name="loginUserInfo"></param>
        /// <returns></returns>
        public MyClassResult GetMyClass(LoginUserInfo loginUserInfo)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetMyClass(loginUserInfo);
            //return new MyPageBiz().GetMyClass(loginUserInfo);
        }


        public List<UP_PORTAL_MYPAGE_SERVICE_UR_LST_Result> GetUsedServiceList(LoginUserInfo loginUserInfo)
        {
            return new PrivateMyPageService.PrivateMyPageServiceClient().GetUsedServiceList(loginUserInfo).ToList();
            //return new MyPageBiz().GetUsedServiceList(loginUserInfo);
        }

        #endregion

        #region 메인 퀵메뉴 마이핀
        public ListModel<NTB_MYPIN_PROGRAM> GetQuickProgramPin(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetQuickProgramPin(loginUser);
        }

        public ListModel<NTB_MYPIN_PARTNER> GetQuickPartnerPin(LoginUserInfo loginUser)
        {
            return new MyPageBiz().GetQuickPartnerPin(loginUser);
        }

        public string GetPartnerCafeDomain(string wowtvId)
        {
            return new MyPageBiz().GetPartnerCafeDomain(wowtvId);
        }
        #endregion




    }
}
