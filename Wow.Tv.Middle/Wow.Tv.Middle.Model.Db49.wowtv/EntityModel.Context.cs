﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 템플릿에서 생성되었습니다.
//
//     이 파일을 수동으로 변경하면 응용 프로그램에서 예기치 않은 동작이 발생할 수 있습니다.
//     이 파일을 수동으로 변경하면 코드가 다시 생성될 때 변경 내용을 덮어씁니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wow.Tv.Middle.Model.Db49.wowtv
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Db49_wowtv : DbContext
    {
        public Db49_wowtv()
            : base("name=Db49_wowtv")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AAA> AAA { get; set; }
        public virtual DbSet<CCC> CCC { get; set; }
        public virtual DbSet<dtproperties> dtproperties { get; set; }
        public virtual DbSet<NTB_ACTION_LOG> NTB_ACTION_LOG { get; set; }
        public virtual DbSet<NTB_BOARD> NTB_BOARD { get; set; }
        public virtual DbSet<NTB_BOARD_COMMENT> NTB_BOARD_COMMENT { get; set; }
        public virtual DbSet<NTB_BOARD_CONTENT> NTB_BOARD_CONTENT { get; set; }
        public virtual DbSet<NTB_COMMON_CODE> NTB_COMMON_CODE { get; set; }
        public virtual DbSet<NTB_CTGR> NTB_CTGR { get; set; }
        public virtual DbSet<NTB_FAMILY> NTB_FAMILY { get; set; }
        public virtual DbSet<NTB_GROUP> NTB_GROUP { get; set; }
        public virtual DbSet<NTB_HIS_MNG> NTB_HIS_MNG { get; set; }
        public virtual DbSet<NTB_MENU> NTB_MENU { get; set; }
        public virtual DbSet<NTB_MENU_GROUP> NTB_MENU_GROUP { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TAB_ANCHOR> TAB_ANCHOR { get; set; }
        public virtual DbSet<TAB_BANNER> TAB_BANNER { get; set; }
        public virtual DbSet<TAB_BANNER_EVENT_LOG> TAB_BANNER_EVENT_LOG { get; set; }
        public virtual DbSet<TAB_BANNER_VIEWING_LOG> TAB_BANNER_VIEWING_LOG { get; set; }
        public virtual DbSet<TAB_BOARD> TAB_BOARD { get; set; }
        public virtual DbSet<TAB_BOARD_COMMENT> TAB_BOARD_COMMENT { get; set; }
        public virtual DbSet<TAB_BOARD_CONFIG> TAB_BOARD_CONFIG { get; set; }
        public virtual DbSet<TAB_BOARD_FILE> TAB_BOARD_FILE { get; set; }
        public virtual DbSet<TAB_BOARD_LOG> TAB_BOARD_LOG { get; set; }
        public virtual DbSet<TAB_BOARD_TOP> TAB_BOARD_TOP { get; set; }
        public virtual DbSet<TAB_BOARD_TOP_COMMENT> TAB_BOARD_TOP_COMMENT { get; set; }
        public virtual DbSet<TAB_BOARD_TOP_FILE> TAB_BOARD_TOP_FILE { get; set; }
        public virtual DbSet<TAB_BOARD_TOP_LOG> TAB_BOARD_TOP_LOG { get; set; }
        public virtual DbSet<TAB_CMS_ADMIN> TAB_CMS_ADMIN { get; set; }
        public virtual DbSet<TAB_CMS_MENU> TAB_CMS_MENU { get; set; }
        public virtual DbSet<TAB_CODE> TAB_CODE { get; set; }
        public virtual DbSet<TAB_COMMON_FILE> TAB_COMMON_FILE { get; set; }
        public virtual DbSet<TAB_FAQ> TAB_FAQ { get; set; }
        public virtual DbSet<TAB_IR_CLUB> TAB_IR_CLUB { get; set; }
        public virtual DbSet<TAB_JOB_COMMENT> TAB_JOB_COMMENT { get; set; }
        public virtual DbSet<TAB_JOB_FILE> TAB_JOB_FILE { get; set; }
        public virtual DbSet<TAB_JOBRESUME> TAB_JOBRESUME { get; set; }
        public virtual DbSet<TAB_JOBUCC> TAB_JOBUCC { get; set; }
        public virtual DbSet<TAB_NOTICE_FILE> TAB_NOTICE_FILE { get; set; }
        public virtual DbSet<TAB_PGM_ORDER> TAB_PGM_ORDER { get; set; }
        public virtual DbSet<TAB_POPUP> TAB_POPUP { get; set; }
        public virtual DbSet<TAB_POPUP_VIEWING_LOG> TAB_POPUP_VIEWING_LOG { get; set; }
        public virtual DbSet<TAB_PUBLIC> TAB_PUBLIC { get; set; }
        public virtual DbSet<TAB_PUBLIC_FILE> TAB_PUBLIC_FILE { get; set; }
        public virtual DbSet<TAB_PUBLIC_LOG> TAB_PUBLIC_LOG { get; set; }
        public virtual DbSet<TAB_QUEST_EMAIL> TAB_QUEST_EMAIL { get; set; }
        public virtual DbSet<TAB_QUEST_EMAIL_FILE> TAB_QUEST_EMAIL_FILE { get; set; }
        public virtual DbSet<TAB_TOUR> TAB_TOUR { get; set; }
        public virtual DbSet<TAB_TOUR_MEMBER> TAB_TOUR_MEMBER { get; set; }
        public virtual DbSet<TAB_VOTEAPT> TAB_VOTEAPT { get; set; }
        public virtual DbSet<TAB_WOWEVENT> TAB_WOWEVENT { get; set; }
        public virtual DbSet<TAB_WOWEVENT_SUB> TAB_WOWEVENT_SUB { get; set; }
        public virtual DbSet<TblArtVietnam> TblArtVietnam { get; set; }
        public virtual DbSet<tblBaseballRankUpdateDate> tblBaseballRankUpdateDate { get; set; }
        public virtual DbSet<TBLBASEBALLSCH> TBLBASEBALLSCH { get; set; }
        public virtual DbSet<tblBaseBallustream> tblBaseBallustream { get; set; }
        public virtual DbSet<TBLBC> TBLBC { get; set; }
        public virtual DbSet<TBLBCBOARD> TBLBCBOARD { get; set; }
        public virtual DbSet<TBLBCCODE2> TBLBCCODE2 { get; set; }
        public virtual DbSet<tblBoard_tmpUrl> tblBoard_tmpUrl { get; set; }
        public virtual DbSet<tblDaeBakChunKuk> tblDaeBakChunKuk { get; set; }
        public virtual DbSet<tblDaeBakChunKukGibeob> tblDaeBakChunKukGibeob { get; set; }
        public virtual DbSet<tblLoginConfirm> tblLoginConfirm { get; set; }
        public virtual DbSet<tblmedia> tblmedia { get; set; }
        public virtual DbSet<tblmediacnt> tblmediacnt { get; set; }
        public virtual DbSet<tblMobileAppDown> tblMobileAppDown { get; set; }
        public virtual DbSet<tblProjectEmergencyOpenYN> tblProjectEmergencyOpenYN { get; set; }
        public virtual DbSet<tblStockWindow> tblStockWindow { get; set; }
        public virtual DbSet<tblStockWindowApplyUser> tblStockWindowApplyUser { get; set; }
        public virtual DbSet<tblTextAndLinkBox> tblTextAndLinkBox { get; set; }
        public virtual DbSet<tblTextAndLinkCategory> tblTextAndLinkCategory { get; set; }
        public virtual DbSet<tblTradingStarRanking> tblTradingStarRanking { get; set; }
        public virtual DbSet<tblTradingStarTrade> tblTradingStarTrade { get; set; }
        public virtual DbSet<tblTradingStarUser> tblTradingStarUser { get; set; }
        public virtual DbSet<tblwownetinvest> tblwownetinvest { get; set; }
        public virtual DbSet<sf1_TAB_BOARD> sf1_TAB_BOARD { get; set; }
        public virtual DbSet<sf1_TAB_PUBLIC> sf1_TAB_PUBLIC { get; set; }
        public virtual DbSet<TAB_AREA_CHANNEL> TAB_AREA_CHANNEL { get; set; }
        public virtual DbSet<TAB_BANNER_VIEWING_LOG_Temp> TAB_BANNER_VIEWING_LOG_Temp { get; set; }
        public virtual DbSet<TAB_IR_CLUB_MANAGER> TAB_IR_CLUB_MANAGER { get; set; }
        public virtual DbSet<TAB_MAIN_EVENT> TAB_MAIN_EVENT { get; set; }
        public virtual DbSet<TAB_MAIN_EVENT_BACKUP> TAB_MAIN_EVENT_BACKUP { get; set; }
        public virtual DbSet<TAB_MAIN_NEW> TAB_MAIN_NEW { get; set; }
        public virtual DbSet<TAB_MANAGER_REG> TAB_MANAGER_REG { get; set; }
        public virtual DbSet<TAB_PGM_ORDER_PROCESS> TAB_PGM_ORDER_PROCESS { get; set; }
        public virtual DbSet<TAB_POPUP_EVENT_LOG> TAB_POPUP_EVENT_LOG { get; set; }
        public virtual DbSet<tblmedia_Code> tblmedia_Code { get; set; }
        public virtual DbSet<tblmedia_platform_code> tblmedia_platform_code { get; set; }
        public virtual DbSet<NTB_BROAD_LIVE> NTB_BROAD_LIVE { get; set; }
        public virtual DbSet<NTB_ATTACH_FILE> NTB_ATTACH_FILE { get; set; }
        public virtual DbSet<NTB_PROGRAM_TEMPLATE> NTB_PROGRAM_TEMPLATE { get; set; }
        public virtual DbSet<NTB_PROGRAM_BANNER> NTB_PROGRAM_BANNER { get; set; }
        public virtual DbSet<NTB_PROGRAM_INTRO> NTB_PROGRAM_INTRO { get; set; }
        public virtual DbSet<tblTradingStarCategory> tblTradingStarCategories { get; set; }
        public virtual DbSet<NTB_PROGRAM_ADMIN> NTB_PROGRAM_ADMIN { get; set; }
        public virtual DbSet<NTB_PROGRAM_PARTNER> NTB_PROGRAM_PARTNER { get; set; }
        public virtual DbSet<NTB_PROGRAM_TEMPLATE_GROUP> NTB_PROGRAM_TEMPLATE_GROUP { get; set; }
        public virtual DbSet<NTB_PROGRAM_GROUP> NTB_PROGRAM_GROUP { get; set; }
        public virtual DbSet<NTB_STOCKHOLDER_BOARD> NTB_STOCKHOLDER_BOARD { get; set; }
        public virtual DbSet<NTB_MYPIN_ARTICLE> NTB_MYPIN_ARTICLE { get; set; }
        public virtual DbSet<NTB_MYPIN_PROGRAM> NTB_MYPIN_PROGRAM { get; set; }
        public virtual DbSet<NTB_MAIN_MANAGE> NTB_MAIN_MANAGE { get; set; }
        public virtual DbSet<NTB_MYPIN_PARTNER> NTB_MYPIN_PARTNER { get; set; }
        public virtual DbSet<NTB_MYPIN_REPORTER> NTB_MYPIN_REPORTER { get; set; }
        public virtual DbSet<NTB_ACCESS_LOG> NTB_ACCESS_LOG { get; set; }
        public virtual DbSet<TAB_CMS_ADMIN_PwdHistory> TAB_CMS_ADMIN_PwdHistory { get; set; }
        public virtual DbSet<TAB_NOTICE> TAB_NOTICE { get; set; }
    
        public virtual int usp_tradingStarEarningRateRENEW(Nullable<int> seq, string tradingcode)
        {
            var seqParameter = seq.HasValue ?
                new ObjectParameter("seq", seq) :
                new ObjectParameter("seq", typeof(int));
    
            var tradingcodeParameter = tradingcode != null ?
                new ObjectParameter("tradingcode", tradingcode) :
                new ObjectParameter("tradingcode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Db49_wowtv.usp_tradingStarEarningRateRENEW", seqParameter, tradingcodeParameter);
        }
    }
}