using System;

namespace Wow.Tv.Middle.Model.Db49.wowtv.BusinessManage
{
    public class BOARD_CONT_MENU
    {
        public int BOARD_CONTENT_SEQ { get; set; }
        public int BOARD_SEQ { get; set; }
        public string TITLE { get; set; }
        public string CONTENT { get; set; }
        public string REG_ID { get; set; }
        public DateTime REG_DATE { get; set; }
        public string MOD_ID { get; set; }
        public Nullable<DateTime> MOD_DATE { get; set; }
        public string DEL_YN { get; set; }
        public int CONTENT_ID { get; set; }
        public string EMAIL_YN { get; set; }
        public string EMAIL { get; set; }
        public int READ_CNT { get; set; }
        public string MENU_NAME_DEPTH_1 { get; set; }
        public string MENU_NAME_DEPTH_2 { get; set; }
        public string MENU_NAME_DEPTH_3 { get; set; }
        public string CONTENT_TYPE_CODE { get; set; }
        public string VIEW_YN { get; set; }
    }
}
