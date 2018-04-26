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

using Wow.Tv.Middle.Biz.Broad;

namespace Wow.Tv.Middle.WcfService.Broad
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드, svc 및 config 파일에서 클래스 이름 "NewsProgramService"을 변경할 수 있습니다.
    // 참고: 이 서비스를 테스트하기 위해 WCF 테스트 클라이언트를 시작하려면 솔루션 탐색기에서 NewsProgramService.svc나 NewsProgramService.svc.cs를 선택하고 디버깅을 시작하십시오.
    public partial class BroadService : INewsProgramService
    {
        public ListModel<TAB_CODE> GetCategoryList(int pseq)
        {
            return new NewsProgreamBiz().GetCategoryList(pseq);
        }

        public T_NEWS_PRG GetAt(string programCode)
        {
            return new NewsProgreamBiz().GetAt(programCode);
        }
        public IMG_SCHEDULE GetAtImgSchedule(string programCode)
        {
            return new NewsProgreamBiz().GetAtImgSchedule(programCode);
        }
        public NTB_ATTACH_FILE GetMainAttachFile(string programCode)
        {
            return new NewsProgreamBiz().GetMainAttachFile(programCode);
        }
        public NTB_ATTACH_FILE GetSubAttachFile(string programCode)
        {
            return new NewsProgreamBiz().GetSubAttachFile(programCode);
        }
        public NTB_ATTACH_FILE GetRectangleAttachFile(string programCode)
        {
            return new NewsProgreamBiz().GetRectangleAttachFile(programCode);
        }
        public NTB_ATTACH_FILE GetThumbnailAttachFile(string programCode)
        {
            return new NewsProgreamBiz().GetThumbnailAttachFile(programCode);
        }

        public ListModel<T_NEWS_PRG> SearchList(NewsProgramCondition condition)
        {
            return new NewsProgreamBiz().SearchList(condition);
        }

        public ListModel<IMG_SCHEDULE> SearchListImgSchedule(NewsProgramCondition condition)
        {
            return new NewsProgreamBiz().SearchListImgSchedule(condition);
        }


        public void SaveSchedule(T_NEWS_PRG model)
        {
            new NewsProgreamBiz().SaveSchedule(model);
        }

        public void Save(T_NEWS_PRG model, NTB_ATTACH_FILE mainAttachFile, NTB_ATTACH_FILE subAttachFile
            , NTB_ATTACH_FILE rectangleAttachFile, NTB_ATTACH_FILE thumbnailAttachFile)
        {
            new NewsProgreamBiz().Save(model, mainAttachFile, subAttachFile, rectangleAttachFile, thumbnailAttachFile);
        }


        public void SaveFromMyProgram(T_NEWS_PRG model, NTB_ATTACH_FILE mainAttachFile, NTB_ATTACH_FILE subAttachFile
            , NTB_ATTACH_FILE rectangleAttachFile, NTB_ATTACH_FILE thumbnailAttachFile)
        {
            new NewsProgreamBiz().SaveFromMyProgram(model, mainAttachFile, subAttachFile, rectangleAttachFile, thumbnailAttachFile);
        }


        public void ExcelFileSave(NTB_ATTACH_FILE model)
        {
            new NewsProgreamBiz().ExcelFileSave(model);
        }

        public NTB_ATTACH_FILE GetExcelFile()
        {
            return new NewsProgreamBiz().GetExcelFile();
        }

        public void Delete(string programCode)
        {
            new NewsProgreamBiz().Delete(programCode);
        }

        public void DeleteImgSchedule(string programCode)
        {
            new NewsProgreamBiz().DeleteImgSchedule(programCode);
        }



        #region Partner

        public List<Pro_wowList> GetWowListPartnerList(string programCode)
        {
            return new NewsProgreamBiz().GetWowListPartnerList(programCode);
        }

        public List<NTB_PROGRAM_PARTNER> GetProgramPartnerList(string programCode)
        {
            return new NewsProgreamBiz().GetPartnerList(programCode);
        }

        public void AddProgramPartner(string programCode, int payNo, LoginUser loginUser)
        {
            new NewsProgreamBiz().AddPartner(programCode, payNo, loginUser);
        }

        public void DeleteProgramPartner(string programCode, int payNo)
        {
            new NewsProgreamBiz().DeletePartner(programCode, payNo);
        }

        public void DeleteProgramPartnerList(string programCode)
        {
            new NewsProgreamBiz().DeletePartnerList(programCode);
        }

        #endregion



        #region Admin

        public List<NTB_PROGRAM_ADMIN> GetProgramAdminList(string programCode)
        {
            return new NewsProgreamBiz().GetAdminList(programCode);
        }

        public void AddProgramAdmin(string programCode, int adminSeq, LoginUser loginUser)
        {
            new NewsProgreamBiz().AddAdmin(programCode, adminSeq, loginUser);
        }

        public void DeleteProgramAdmin(string programCode, int adminSeq)
        {
            new NewsProgreamBiz().DeleteAdmin(programCode, adminSeq);
        }

        public void DeleteProgramAdminList(string programCode)
        {
            new NewsProgreamBiz().DeleteAdminList(programCode);
        }


        public List<string> GetAdminProgram(int adminSeq)
        {
            return new NewsProgreamBiz().GetAdminProgram(adminSeq);
        }

        #endregion





        public List<T_RUNDOWN> SearchListRunDown(string date)
        {
            return new NewsProgreamBiz().SearchListRunDown(date);
        }



        public T_RUNDOWN GetNowRunDown()
        {
            return new NewsProgreamBiz().GetNowRunDown();
        }




        #region Front Main 용

        public List<tblStockClass> GetListStockVod()
        {
            return new NewsProgreamBiz().GetListStockVod();
        }

        public List<NUP_BROAD_MAIN_ARTICLE_SELECT_Result> GetListMarket()
        {
            return new NewsProgreamBiz().GetListMarket();
        }



        public void ProgramOrder(TAB_PGM_ORDER model, NTB_ATTACH_FILE attachFile)
        {
            new NewsProgreamBiz().ProgramOrder(model, attachFile);
        }


        public List<NTB_BOARD_CONTENT> GetNotice1()
        {
            return new NewsProgreamBiz().GetNotice1();
        }
        public List<TAB_LECTURES_SCHEDULE> GetNotice2()
        {
            return new NewsProgreamBiz().GetNotice2();
        }
        public List<NTB_BOARD_CONTENT> GetNotice3()
        {
            return new NewsProgreamBiz().GetNotice3();
        }





        public List<BestStockPro> GetWowNetData()
        {
            return new NewsProgreamBiz().GetWowNetData();
        }

        public List<USP_GetRecommendPro3_Result> GetWowNetData2()
        {
            return new NewsProgreamBiz().GetWowNetData2();
        }

        public List<WOW_M_BANNER> GetWowFaData()
        {
            return new NewsProgreamBiz().GetWowFaData();
        }
        public List<NUP_MAIN_COMMON_LECTURE_SELECT_Result> GetWowFaDataLecture()
        {
            return new NewsProgreamBiz().GetWowFaDataLecture();
        }

        public List<wowstar_video_Result> GetWowStarData()
        {
            return new NewsProgreamBiz().GetWowStarData();
        }
        public wowstar_p2p_wowtv_Result GetWowStarDataStock()
        {
            return new NewsProgreamBiz().GetWowStarDataStock();
        }

        public Pro_wow_junmunga_broadcast GetJunMunGaData(string proId)
        {
            return new NewsProgreamBiz().GetJunMunGaData(proId);
        }



        public bool IsHoliDayCheck()
        {
            return new NewsProgreamBiz().IsHoliDayCheck();
        }



        public List<DumyModel> TickerList()
        {
            return new NewsProgreamBiz().TickerList();
        }


        //public List<Model.Db89.WOWTV_BILL_DB.TItemPriceMst> PartnerEvent(string proId)
        //{
        //    return new NewsProgreamBiz().PartnerEvent(proId);
        //}


        public List<int> GetPartnerEventItemPrice(string proId)
        {
            return new NewsProgreamBiz().GetPartnerEventItemPrice(proId);
        }


        #endregion




        #region 연합형에 보이는 전문가 목록 가져오기

        public List<USP_GetBroadcast1ByWellList_Result> GetProWowWellList()
        {
            return new NewsProgreamBiz().GetProWowWellList();
        }

        public List<Pro_wowListStockKing> GetStockKing3()
        {
            return new NewsProgreamBiz().GetStockKing3();
        }

        #endregion




        #region 온에어 에서 보여지는 목록


        public List<USP_GetTabStrategetApplication_Result> TodayStrategy()
        {
            return new NewsProgreamBiz().TodayStrategy();
        }



        public List<usp_GetlivePro_Result> GetOnAirPartnerList()
        {
            return new NewsProgreamBiz().GetOnAirPartnerList();
        }


        #endregion


        #region 코너가져오기


        public ListModel<usp_GetCornerVODList_WEB_Result> GetCornerVod(string scCode, int currentIndex, int pageSize)
        {
            return new NewsProgreamBiz().GetCornerVod(scCode, currentIndex, pageSize);
        }

        public ListModel<NUP_GetCornerVODList_WEB_Result> GetCornerVodAll(int currentIndex, int pageSize)
        {
            return new NewsProgreamBiz().GetCornerVodAll(currentIndex, pageSize);
        }


        public List<NTB_MENU> GetMenuByProgramCode(string programCode)
        {
            return new NewsProgreamBiz().GetMenuByProgramCode(programCode);
        }

        public NTB_MENU GetMenuByCornerSeq(string programCode, string scCode)
        {
            return new NewsProgreamBiz().GetMenuByCornerSeq(programCode, scCode);
        }

        #endregion


        #region 수익률 가져오기

        public NTB_BOARD_CONTENT GetTrade(int boardContentSeq)
        {
            return new NewsProgreamBiz().GetTrade(boardContentSeq);
        }

        #endregion




        #region 메인의 우측스크롤 관련


        public ListModel<T_NEWS_PRG> SearchListRandom()
        {
            return new NewsProgreamBiz().SearchListRandom();
        }

        #endregion


        public void Migration()
        {
            new NewsProgreamBiz().Migration();
        }


        public List<T_NEWS_PRG> GetMigrationProgramList()
        {
            return new NewsProgreamBiz().GetMigrationProgramList();
        }
        
        public List<tblSubConerList> MigrationProgramCornerList(string programCode, LoginUser loginUser)
        {
            return new NewsProgreamBiz().MigrationProgramCornerList(programCode, loginUser);
        }


        public List<NUP_GetAllProgramList_Result> GetAllProgramList(string programNameStart, string programNameEnd, string year, int startIndex, int endIndex)
        {
            return new NewsProgreamBiz().GetAllProgramList(programNameStart, programNameEnd, year, startIndex, endIndex);
        }

        public List<NUP_GetAllProgramEtcList_Result> GetAllProgramEtcList(string year, int startIndex, int endIndex)
        {
            return new NewsProgreamBiz().GetAllProgramEtcList(year, startIndex, endIndex);
        }
    }
}
