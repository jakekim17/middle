using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv
{
    public partial class NTB_BOARD :ICloneable
    {
        public string BoardTypeCodeName { get; set; }

        public object Clone()
        {
            return new NTB_BOARD
            {
                
                BOARD_SEQ = BOARD_SEQ,
                ACTIVE_YN =  ACTIVE_YN,
                ATTACH_FILE_YN = ATTACH_FILE_YN,
                BLIND_YN = BLIND_YN,
                BOARD_NAME = BOARD_NAME,
                BOARD_TYPE_CODE = BOARD_TYPE_CODE,
                BOTTOM_CONTENT = BOTTOM_CONTENT,
                BoardTypeCodeName = BoardTypeCodeName,
                COMMENT_YN = COMMENT_YN,
                FILE_COUNT = FILE_COUNT,
                KEYWORD_YN = KEYWORD_YN,
                MOD_DATE = MOD_DATE,
                MOD_ID = MOD_ID,
                NOTICE_COUNT = NOTICE_COUNT,
                DEL_YN = DEL_YN,
                EMAIL_YN = EMAIL_YN,
                PASSWORD_YN = PASSWORD_YN,
                REG_DATE = REG_DATE,
                REG_ID = REG_ID,
                REPLY_YN = REPLY_YN,
                SCRAP_YN = SCRAP_YN,
                TOP_CONTENT = TOP_CONTENT,
                TOP_NOTICE_YN = TOP_NOTICE_YN

            };
        }

    }
}
