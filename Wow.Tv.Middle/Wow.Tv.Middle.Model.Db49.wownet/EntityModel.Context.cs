﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 템플릿에서 생성되었습니다.
//
//     이 파일을 수동으로 변경하면 응용 프로그램에서 예기치 않은 동작이 발생할 수 있습니다.
//     이 파일을 수동으로 변경하면 코드가 다시 생성될 때 변경 내용을 덮어씁니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wow.Tv.Middle.Model.Db49.wownet
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Db49_wownet : DbContext
    {
        public Db49_wownet()
            : base("name=Db49_wownet")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TAB_BOARD_AA> TAB_BOARD { get; set; }
        public virtual DbSet<TAB_STOCK_SITUATION> TAB_STOCK_SITUATION { get; set; }
        public virtual DbSet<TAB_BOARD_CONFIG_49_NET> TAB_BOARD_CONFIG_49_NET { get; set; }
        public virtual DbSet<TAB_BOARD_TOP_49_NET> TAB_BOARD_TOP_49_NET { get; set; }
        public virtual DbSet<TAB_NOTICE_49_NET> TAB_NOTICE_49_NET { get; set; }
        public virtual DbSet<TAB_NOTICE_LOG_49_NET> TAB_NOTICE_LOG_49_NET { get; set; }
        public virtual DbSet<TAB_IR_CLUB_2010> TAB_IR_CLUB_2010 { get; set; }
        public virtual DbSet<TAB_STOCK_RESULT> TAB_STOCK_RESULT { get; set; }
        public virtual DbSet<TAB_STOCK_RESULT_CONNECT> TAB_STOCK_RESULT_CONNECT { get; set; }
        public virtual DbSet<TAB_LECTURES> TAB_LECTURES { get; set; }
        public virtual DbSet<TAB_LECTURES_SCHEDULE> TAB_LECTURES_SCHEDULE { get; set; }
        public virtual DbSet<NTB_LECTURES_LECTURER> NTB_LECTURES_LECTURER { get; set; }
        public virtual DbSet<tblEvent> tblEvents { get; set; }
        public virtual DbSet<TAB_SCRAP_CATEGORY> TAB_SCRAP_CATEGORY { get; set; }
        public virtual DbSet<TAB_SCRAP> TAB_SCRAP { get; set; }
        public virtual DbSet<TAB_SCRAP_CONTENT> TAB_SCRAP_CONTENT { get; set; }
        public virtual DbSet<TAB_SCRAP_MENU> TAB_SCRAP_MENU { get; set; }
        public virtual DbSet<TAB_CONSULTATION_APPLICATION> TAB_CONSULTATION_APPLICATION { get; set; }
        public virtual DbSet<TAB_ACADEMY_APPL> TAB_ACADEMY_APPL { get; set; }
        public virtual DbSet<TBLCMSMNU_GIFT_USER> TBLCMSMNU_GIFT_USER { get; set; }
        public virtual DbSet<TAB_STRATEGY_APPLICATION> TAB_STRATEGY_APPLICATION { get; set; }
        public virtual DbSet<TAB_CODE_NET> TAB_CODE { get; set; }
    
        public virtual ObjectResult<NUP_TAB_NOTICE_PAGE_Result> NUP_TAB_NOTICE_PAGE(Nullable<int> i_PAGENUM, Nullable<int> i_PAGESIZE, string i_SEARCHCONDITION, string i_ORDERBY, string i_ORDERCOLUMN, ObjectParameter o_TOTALCOUNT)
        {
            var i_PAGENUMParameter = i_PAGENUM.HasValue ?
                new ObjectParameter("I_PAGENUM", i_PAGENUM) :
                new ObjectParameter("I_PAGENUM", typeof(int));
    
            var i_PAGESIZEParameter = i_PAGESIZE.HasValue ?
                new ObjectParameter("I_PAGESIZE", i_PAGESIZE) :
                new ObjectParameter("I_PAGESIZE", typeof(int));
    
            var i_SEARCHCONDITIONParameter = i_SEARCHCONDITION != null ?
                new ObjectParameter("I_SEARCHCONDITION", i_SEARCHCONDITION) :
                new ObjectParameter("I_SEARCHCONDITION", typeof(string));
    
            var i_ORDERBYParameter = i_ORDERBY != null ?
                new ObjectParameter("I_ORDERBY", i_ORDERBY) :
                new ObjectParameter("I_ORDERBY", typeof(string));
    
            var i_ORDERCOLUMNParameter = i_ORDERCOLUMN != null ?
                new ObjectParameter("I_ORDERCOLUMN", i_ORDERCOLUMN) :
                new ObjectParameter("I_ORDERCOLUMN", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NUP_TAB_NOTICE_PAGE_Result>("Db49_wownet.NUP_TAB_NOTICE_PAGE", i_PAGENUMParameter, i_PAGESIZEParameter, i_SEARCHCONDITIONParameter, i_ORDERBYParameter, i_ORDERCOLUMNParameter, o_TOTALCOUNT);
        }
    
        public virtual ObjectResult<USP_GetTabStrategetApplication_Result> USP_GetTabStrategetApplication(Nullable<int> rowCount)
        {
            var rowCountParameter = rowCount.HasValue ?
                new ObjectParameter("rowCount", rowCount) :
                new ObjectParameter("rowCount", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetTabStrategetApplication_Result>("Db49_wownet.USP_GetTabStrategetApplication", rowCountParameter);
        }
    
        public virtual ObjectResult<NSP_BANNER_RANDOM_Result> NSP_BANNER_RANDOM(Nullable<int> tYPE, Nullable<int> aREA)
        {
            var tYPEParameter = tYPE.HasValue ?
                new ObjectParameter("TYPE", tYPE) :
                new ObjectParameter("TYPE", typeof(int));
    
            var aREAParameter = aREA.HasValue ?
                new ObjectParameter("AREA", aREA) :
                new ObjectParameter("AREA", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NSP_BANNER_RANDOM_Result>("Db49_wownet.NSP_BANNER_RANDOM", tYPEParameter, aREAParameter);
        }
    
        public virtual ObjectResult<usp_web_getStockDiscussionTop9List_Result> usp_web_getStockDiscussionTop9List(string bCode, string arjCode)
        {
            var bCodeParameter = bCode != null ?
                new ObjectParameter("BCode", bCode) :
                new ObjectParameter("BCode", typeof(string));
    
            var arjCodeParameter = arjCode != null ?
                new ObjectParameter("ArjCode", arjCode) :
                new ObjectParameter("ArjCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_web_getStockDiscussionTop9List_Result>("Db49_wownet.usp_web_getStockDiscussionTop9List", bCodeParameter, arjCodeParameter);
        }
    
        public virtual ObjectResult<usp_web_getStockConsultTop9List_Result> usp_web_getStockConsultTop9List(string bCode, string cName)
        {
            var bCodeParameter = bCode != null ?
                new ObjectParameter("BCode", bCode) :
                new ObjectParameter("BCode", typeof(string));
    
            var cNameParameter = cName != null ?
                new ObjectParameter("CName", cName) :
                new ObjectParameter("CName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_web_getStockConsultTop9List_Result>("Db49_wownet.usp_web_getStockConsultTop9List", bCodeParameter, cNameParameter);
        }
    
        [DbFunction("Db49_wownet", "FN_JOIN_CAFE_LIST_New")]
        public virtual IQueryable<FN_JOIN_CAFE_LIST_New_Result> FN_JOIN_CAFE_LIST_New(string uSERID)
        {
            var uSERIDParameter = uSERID != null ?
                new ObjectParameter("USERID", uSERID) :
                new ObjectParameter("USERID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<FN_JOIN_CAFE_LIST_New_Result>("[Db49_wownet].[FN_JOIN_CAFE_LIST_New](@USERID)", uSERIDParameter);
        }
    
        public virtual ObjectResult<usp_Select_MasterOpenCafeInfo_Result> usp_Select_MasterOpenCafeInfo(string userID)
        {
            var userIDParameter = userID != null ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Select_MasterOpenCafeInfo_Result>("Db49_wownet.usp_Select_MasterOpenCafeInfo", userIDParameter);
        }
    
        public virtual ObjectResult<NUP_MY_LECTURES_SELECT_Result> NUP_MY_LECTURES_SELECT(string uSER_ID, string sTART_DATE, string eND_DATE)
        {
            var uSER_IDParameter = uSER_ID != null ?
                new ObjectParameter("USER_ID", uSER_ID) :
                new ObjectParameter("USER_ID", typeof(string));
    
            var sTART_DATEParameter = sTART_DATE != null ?
                new ObjectParameter("START_DATE", sTART_DATE) :
                new ObjectParameter("START_DATE", typeof(string));
    
            var eND_DATEParameter = eND_DATE != null ?
                new ObjectParameter("END_DATE", eND_DATE) :
                new ObjectParameter("END_DATE", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NUP_MY_LECTURES_SELECT_Result>("Db49_wownet.NUP_MY_LECTURES_SELECT", uSER_IDParameter, sTART_DATEParameter, eND_DATEParameter);
        }
    
        public virtual ObjectResult<NUP_MAIN_COMMON_LECTURE_SELECT_Result> NUP_MAIN_COMMON_LECTURE_SELECT()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<NUP_MAIN_COMMON_LECTURE_SELECT_Result>("Db49_wownet.NUP_MAIN_COMMON_LECTURE_SELECT");
        }
    
        public virtual ObjectResult<usp_tblEvent_select_Result> usp_tblEvent_select()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_tblEvent_select_Result>("Db49_wownet.usp_tblEvent_select");
        }
    }
}