using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.editVOD;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db90.DNRS;
using Wow.Tv.Middle.Model.Db90.DNRS.NewsProgram;
using Wow.Tv.Middle.Model.Db123.WOW4989;
using Wow.Tv.Middle.Model.Db16.wowfa;

namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "INewsProgramService"을 변경할 수 있습니다.
    [ServiceContract]
    public interface INewsProgramService
    {
        [OperationContract]
        ListModel<TAB_CODE> GetCategoryList(int pseq);

        [OperationContract]
        T_NEWS_PRG GetAt(string programCode);

        [OperationContract]
        IMG_SCHEDULE GetAtImgSchedule(string programCode);

        [OperationContract]
        NTB_ATTACH_FILE GetMainAttachFile(string programCode);

        [OperationContract]
        NTB_ATTACH_FILE GetSubAttachFile(string programCode);

        [OperationContract]
        NTB_ATTACH_FILE GetRectangleAttachFile(string programCode);

        [OperationContract]
        NTB_ATTACH_FILE GetThumbnailAttachFile(string programCode);

        [OperationContract]
        ListModel<T_NEWS_PRG> SearchList(NewsProgramCondition condition);

        [OperationContract]
        ListModel<IMG_SCHEDULE> SearchListImgSchedule(NewsProgramCondition condition);

        [OperationContract]
        void SaveSchedule(T_NEWS_PRG model);

        [OperationContract]
        void Save(T_NEWS_PRG model, NTB_ATTACH_FILE mainAttachFile, NTB_ATTACH_FILE subAttachFile
            , NTB_ATTACH_FILE rectangleAttachFile, NTB_ATTACH_FILE thumbnailAttachFile);

        [OperationContract]
        void SaveFromMyProgram(T_NEWS_PRG model, NTB_ATTACH_FILE mainAttachFile, NTB_ATTACH_FILE subAttachFile
            , NTB_ATTACH_FILE rectangleAttachFile, NTB_ATTACH_FILE thumbnailAttachFile);

        [OperationContract]
        void ExcelFileSave(NTB_ATTACH_FILE model);

        [OperationContract]
        NTB_ATTACH_FILE GetExcelFile();

        [OperationContract]
        void Delete(string programCode);

        [OperationContract]
        void DeleteImgSchedule(string programCode);




        #region Partner


        [OperationContract]
        List<Pro_wowList> GetWowListPartnerList(string programCode);

        [OperationContract]
        List<NTB_PROGRAM_PARTNER> GetProgramPartnerList(string programCode);



        [OperationContract]
        void AddProgramPartner(string programCode, int payNo, LoginUser loginUser);


        [OperationContract]
        void DeleteProgramPartner(string programCode, int payNo);

        [OperationContract]
        void DeleteProgramPartnerList(string programCode);

        #endregion



        #region Admin

        [OperationContract]
        List<NTB_PROGRAM_ADMIN> GetProgramAdminList(string programCode);



        [OperationContract]
        void AddProgramAdmin(string programCode, int adminSeq, LoginUser loginUser);


        [OperationContract]
        void DeleteProgramAdmin(string programCode, int adminSeq);

        [OperationContract]
        void DeleteProgramAdminList(string programCode);


        [OperationContract]
        List<string> GetAdminProgram(int adminSeq);

        #endregion




        [OperationContract]
        List<T_RUNDOWN> SearchListRunDown(string date);



        [OperationContract]
        T_RUNDOWN GetNowRunDown();







        #region Front Main 용

        [OperationContract]
        List<tblStockClass> GetListStockVod();


        [OperationContract]
        List<NUP_BROAD_MAIN_ARTICLE_SELECT_Result> GetListMarket();



        [OperationContract]
        void ProgramOrder(TAB_PGM_ORDER model, NTB_ATTACH_FILE attachFile);



        [OperationContract]
        List<NTB_BOARD_CONTENT> GetNotice1();
        [OperationContract]
        List<TAB_LECTURES_SCHEDULE> GetNotice2();
        [OperationContract]
        List<NTB_BOARD_CONTENT> GetNotice3();


        [OperationContract]
        bool IsHoliDayCheck();



        #region 와우넷/와우파/와우스타

        [OperationContract]
        List<BestStockPro> GetWowNetData();

        [OperationContract]
        List<USP_GetRecommendPro3_Result> GetWowNetData2();

        [OperationContract]
        List<WOW_M_BANNER> GetWowFaData();
        [OperationContract]
        List<NUP_MAIN_COMMON_LECTURE_SELECT_Result> GetWowFaDataLecture();

        [OperationContract]
        List<wowstar_video_Result> GetWowStarData();
        [OperationContract]
        wowstar_p2p_wowtv_Result GetWowStarDataStock();

        [OperationContract]
        Pro_wow_junmunga_broadcast GetJunMunGaData(string proId);

        #endregion



        [OperationContract]
        List<DumyModel> TickerList();




        //[OperationContract]
        //List<Model.Db89.WOWTV_BILL_DB.TItemPriceMst> PartnerEvent(string proId);



        [OperationContract]
        List<int> GetPartnerEventItemPrice(string proId);


        #endregion





        #region 연합형에 보이는 전문가 목록 가져오기

        [OperationContract]
        List<USP_GetBroadcast1ByWellList_Result> GetProWowWellList();


        [OperationContract]
        List<Pro_wowListStockKing> GetStockKing3();


        #endregion



        #region 온에어 에서 보여지는 목록

        [OperationContract]
        List<USP_GetTabStrategetApplication_Result> TodayStrategy();

        [OperationContract]
        List<usp_GetlivePro_Result> GetOnAirPartnerList();

        #endregion


        #region 코너가져오기

        [OperationContract]
        ListModel<usp_GetCornerVODList_WEB_Result> GetCornerVod(string scCode, int currentIndex, int pageSize);

        [OperationContract]
        ListModel<NUP_GetCornerVODList_WEB_Result> GetCornerVodAll(int currentIndex, int pageSize);


        [OperationContract]
        List<NTB_MENU> GetMenuByProgramCode(string programCode);


        [OperationContract]
        NTB_MENU GetMenuByCornerSeq(string programCode, string scCode);


        #endregion


        #region 수익률 가져오기

        [OperationContract]
        NTB_BOARD_CONTENT GetTrade(int boardContentSeq);

        #endregion



        #region 메인의 우측스크롤 관련

        [OperationContract]
        ListModel<T_NEWS_PRG> SearchListRandom();

        #endregion

        [OperationContract]
        void Migration();

        [OperationContract]
        List<T_NEWS_PRG> GetMigrationProgramList();

        [OperationContract]
        List<tblSubConerList> MigrationProgramCornerList(string programCode, LoginUser loginUser);



        [OperationContract]
        List<NUP_GetAllProgramList_Result> GetAllProgramList(string programNameStart, string programNameEnd, string year, int startIndex, int endIndex);

        [OperationContract]
        List<NUP_GetAllProgramEtcList_Result> GetAllProgramEtcList(string year, int startIndex, int endIndex);

    }
}
