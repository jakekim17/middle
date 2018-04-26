﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 템플릿에서 생성되었습니다.
//
//     이 파일을 수동으로 변경하면 응용 프로그램에서 예기치 않은 동작이 발생할 수 있습니다.
//     이 파일을 수동으로 변경하면 코드가 다시 생성될 때 변경 내용을 덮어씁니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wow.Tv.Middle.Model.Db49.wowcafe
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Db49_wowcafe : DbContext
    {
        public Db49_wowcafe()
            : base("name=Db49_wowcafe")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CafeMemberInfo> CafeMemberInfo { get; set; }
        public virtual DbSet<CafeInfo> CafeInfo { get; set; }
    
        public virtual int usp_RegistCafememberGold(string proID, string userID, ObjectParameter strResultCafe)
        {
            var proIDParameter = proID != null ?
                new ObjectParameter("proID", proID) :
                new ObjectParameter("proID", typeof(string));
    
            var userIDParameter = userID != null ?
                new ObjectParameter("userID", userID) :
                new ObjectParameter("userID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Db49_wowcafe.usp_RegistCafememberGold", proIDParameter, userIDParameter, strResultCafe);
        }
    
        public virtual ObjectResult<usp_select_stock_cafe_list_Result> usp_select_stock_cafe_list(string arjCode)
        {
            var arjCodeParameter = arjCode != null ?
                new ObjectParameter("ArjCode", arjCode) :
                new ObjectParameter("ArjCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_select_stock_cafe_list_Result>("Db49_wowcafe.usp_select_stock_cafe_list", arjCodeParameter);
        }
    
        public virtual ObjectResult<usp_Select_TopNewBoard_Result> usp_Select_TopNewBoard(Nullable<int> cafeCode, Nullable<int> pageSize, Nullable<int> myGradeLevel)
        {
            var cafeCodeParameter = cafeCode.HasValue ?
                new ObjectParameter("CafeCode", cafeCode) :
                new ObjectParameter("CafeCode", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var myGradeLevelParameter = myGradeLevel.HasValue ?
                new ObjectParameter("MyGradeLevel", myGradeLevel) :
                new ObjectParameter("MyGradeLevel", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Select_TopNewBoard_Result>("Db49_wowcafe.usp_Select_TopNewBoard", cafeCodeParameter, pageSizeParameter, myGradeLevelParameter);
        }
    
        public virtual ObjectResult<usp_Select_TopNewBoard_sakal_Result> usp_Select_TopNewBoard_sakal(Nullable<int> cafeCode, Nullable<int> pageSize, Nullable<int> myGradeLevel)
        {
            var cafeCodeParameter = cafeCode.HasValue ?
                new ObjectParameter("CafeCode", cafeCode) :
                new ObjectParameter("CafeCode", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var myGradeLevelParameter = myGradeLevel.HasValue ?
                new ObjectParameter("MyGradeLevel", myGradeLevel) :
                new ObjectParameter("MyGradeLevel", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Select_TopNewBoard_sakal_Result>("Db49_wowcafe.usp_Select_TopNewBoard_sakal", cafeCodeParameter, pageSizeParameter, myGradeLevelParameter);
        }
    
        public virtual ObjectResult<usp_web_select_stock_cafe_list_Result> usp_web_select_stock_cafe_list(string arjCode)
        {
            var arjCodeParameter = arjCode != null ?
                new ObjectParameter("ArjCode", arjCode) :
                new ObjectParameter("ArjCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_web_select_stock_cafe_list_Result>("Db49_wowcafe.usp_web_select_stock_cafe_list", arjCodeParameter);
        }
    }
}