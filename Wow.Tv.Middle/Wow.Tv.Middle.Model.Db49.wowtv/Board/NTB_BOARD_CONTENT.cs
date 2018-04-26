using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv
{

    public partial class NTB_BOARD_CONTENT  :ICloneable
    {
        /// <summary>
        /// 첨부 파일 리스트
        /// </summary>
        public List<NTB_ATTACH_FILE> AttachFileList { get; set; }
        /// <summary>
        /// 답변 리스트
        /// </summary>
        public List<NTB_BOARD_CONTENT> ReplyList { get; set; }
        /// <summary>
        /// 댓글 리스트
        /// </summary>
        public List<NTB_BOARD_COMMENT> CommentList { get; set; }

        /// <summary>
        /// 답변 상태 활성화 여부
        /// </summary>
        [DefaultValue(false)]
        public bool IsReply { get; set; }


        /// <summary>
        /// 파일 첨부 활성화 여부
        /// </summary>
        [DefaultValue(false)]
        public bool IsFile { get; set; }

        /// <summary>
        /// 코멘트 활성화 여부
        /// </summary>
        [DefaultValue(false)]
        public bool IsComment { get; set; }

        /// <summary>
        /// 1차 코드명
        /// </summary>
        [DefaultValue("")]
        public string UpCommonName { get; set; }

        /// <summary>
        /// 2차 코드명
        /// </summary>
        [DefaultValue("")]
        public string CommonName { get; set; }

        /// <summary>
        /// 연락처
        /// </summary>
        [DefaultValue("")]
        public string Telno { get; set; }

        [DefaultValue("")]
        public string MOD_NICKNAME {get; set; }

        /// <summary>
        /// 복사 
        /// 엔티티 컬럼 추가시 적용 바람
        /// </summary>
        /// <returns>NTB_BOARD_CONTENT</returns>
        public object Clone()
        {
            return new NTB_BOARD_CONTENT
            {
                BOARD_CONTENT_SEQ =  this.BOARD_CONTENT_SEQ,
                BOARD_SEQ =  this.BOARD_SEQ,
                AttachFileList =  this.AttachFileList,
                COMMON_CODE =  this.COMMON_CODE,
                CONTENT = this.CONTENT,
                CONTENT_ID = this.CONTENT_ID,
                DEL_YN =  this.DEL_YN,
                DEPTH =  this.DEPTH,
                EMAIL =  this.EMAIL,
                EMAIL_YN = this.EMAIL_YN,
                END_DATE = this.END_DATE,
                IsComment = this.IsComment,
                IsFile = this.IsFile,
                IsReply = this.IsReply,
                KEYWORD = this.KEYWORD,
                MOD_DATE = this.MOD_DATE,
                MOD_ID = this.MOD_ID,
                NOTICE_YN = this.NOTICE_YN,
                PASSWORD = this.PASSWORD,
                READ_CNT = this.READ_CNT,
                REG_DATE = this.REG_DATE,
                REG_ID = this.REG_ID,
                ReplyList = this.ReplyList,
                SORD_ORDER = this.SORD_ORDER,
                TITLE = this.TITLE,
                START_DATE = this.START_DATE,
                UP_BOARD_CONTENT_SEQ = this.UP_BOARD_CONTENT_SEQ,
                TOP_BOARD_CONTENT_SEQ = this.TOP_BOARD_CONTENT_SEQ,
                USER_ID = this.USER_ID,
                USER_NAME = this.USER_NAME,
                USER_PHONE = this.USER_PHONE,
                VIEW_YN = this.VIEW_YN,
                REPLY_YN = this.REPLY_YN,
                AGREE_YN = AGREE_YN,
                UP_COMMON_CODE = UP_COMMON_CODE,
                PAGE_LINK = PAGE_LINK,
                UpCommonName = UpCommonName,
                CommonName = CommonName,
                BLIND_YN = BLIND_YN,
                Telno = Telno,
                REG_IP = REG_IP,
                MOD_NICKNAME = MOD_NICKNAME
            };
        }





        public int MenuSeq { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
    }
}
