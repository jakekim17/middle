//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 템플릿에서 생성되었습니다.
//
//     이 파일을 수동으로 변경하면 응용 프로그램에서 예기치 않은 동작이 발생할 수 있습니다.
//     이 파일을 수동으로 변경하면 코드가 다시 생성될 때 변경 내용을 덮어씁니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wow.Tv.Middle.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TAB_CMS_ADMIN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAB_CMS_ADMIN()
        {
            this.TAB_CMS_MENU = new HashSet<TAB_CMS_MENU>();
        }
    
        public string ADMIN_ID { get; set; }
        public string NAME { get; set; }
        public string PWD { get; set; }
        public string EMAIL { get; set; }
        public Nullable<System.DateTime> REG_DATE { get; set; }
        public Nullable<System.DateTime> MOD_DATE { get; set; }
        public string SSN { get; set; }
        public string IP { get; set; }
        public string PHONE1 { get; set; }
        public string PHONE2 { get; set; }
        public string PHONE3 { get; set; }
        public string MLEVEL { get; set; }
        public string PARTNAME { get; set; }
        public string PART { get; set; }
        public string MCLASS { get; set; }
        public string MOBILE1 { get; set; }
        public string MOBILE2 { get; set; }
        public string MOBILE3 { get; set; }
        public string WOWNET { get; set; }
        public string MISSLEE { get; set; }
        public string DEL_YN { get; set; }
        public System.DateTime LAST_LOGIN_DATE { get; set; }
        public int SEQ { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAB_CMS_MENU> TAB_CMS_MENU { get; set; }
    }
}