//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 템플릿에서 생성되었습니다.
//
//     이 파일을 수동으로 변경하면 응용 프로그램에서 예기치 않은 동작이 발생할 수 있습니다.
//     이 파일을 수동으로 변경하면 코드가 다시 생성될 때 변경 내용을 덮어씁니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Wow.Tv.Middle.Model.Db89.wowbill
{
    using System;
    using System.Collections.Generic;
    
    public partial class NTB_USER_LOGIN_LOG
    {
        public long LOG_SEQ { get; set; }
        public Nullable<int> USER_NUMBER { get; set; }
        public string USER_ID { get; set; }
        public string WEB_TYPE { get; set; }
        public string WEB_FROM { get; set; }
        public string WEB_SERVER_NAME { get; set; }
        public string LOGIN_SITE { get; set; }
        public Nullable<System.DateTime> LOGIN_DT { get; set; }
        public string CLIENT_IP { get; set; }
        public string REQUEST_URL { get; set; }
    }
}