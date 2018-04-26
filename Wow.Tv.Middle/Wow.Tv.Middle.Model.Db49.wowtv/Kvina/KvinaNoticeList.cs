using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv.Kvina
{
	public class KvinaNoticeList
	{
        public int SEQ { get; set; }
        public string BCODE { get; set; }
        public string TITLE { get; set; }
        public string CONTENT { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public Nullable<int> REF { get; set; }
        public Nullable<int> REF_STEP { get; set; }
        public Nullable<int> REF_LEVEL { get; set; }
        public Nullable<int> READ_CNT { get; set; }
        public Nullable<int> MEMO_CNT { get; set; }
        public Nullable<int> RECOMMEND_CNT { get; set; }
        public Nullable<int> REVERSE_CNT { get; set; }
        public Nullable<int> ASTROMANCY_CNT { get; set; }
        public Nullable<int> ASTROMANCY_NUM { get; set; }
        public string VIEW_FLAG { get; set; }
        public string EMOTICON { get; set; }
        public System.DateTime REG_DATE { get; set; }
        public string GUBUN { get; set; }
        public string SOURCE { get; set; }
        public string EMAIL { get; set; }
        public Nullable<int> PUBLICITY_CNT { get; set; }
        public Nullable<int> TOTAL_NUM { get; set; }
        public Nullable<bool> TOP_YN { get; set; }
        public string SITE_NAME { get; set; }
        public string DEL_YN { get; set; }

		public int ROWCNT { get; set; }
	}





}
