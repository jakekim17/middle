using System;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wowtv.Board
{

    /// <summary>
    /// <para>  통합게시판 검색조건 </para>
    /// <para>- 작  성  자 : ABC솔루션 정재민</para>
    /// <para>- 최초작성일 : 2017-08-22</para>
    /// <para>- 최종수정자 : ABC솔루션 정재민</para>
    /// <para>- 최종수정일 : 2017-09-21</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-08-22 생성</para>
    /// <para>  2017-09-21 정재민 : UP_COMMON_CODE 추가 (http://172.19.0.21/redmine/issues/82)</para>
    /// <para>  2017-10-19 정재민 : 검색 아이디 추가</para>
    /// </summary>
    /// <remarks></remarks>
    public class IntegratedBoardCondition : BaseCondition
    {
        /// <summary>
        /// board seq 
        /// </summary>
        public int BOARD_SEQ { get;  set; }

        /// <summary>
        /// board content seq
        /// </summary>
        public int BOARD_CONTENT_SEQ { get; set; }

        /// <summary>
        /// 1차 공통코드
        /// </summary>
        public string UP_COMMON_CODE { get; set; } = string.Empty;
        /// <summary>
        /// 2차 공통코드
        /// </summary>
        public string COMMON_CODE { get;  set; } = string.Empty;

        /// <summary>
        /// 게시여부
        /// </summary>
        public string VIEW_YN { get; set; } = string.Empty;

        /// <summary>
        /// 삭제여부
        /// </summary>
        public string DEL_YN { get; set; } = string.Empty;

        /// <summary>
        /// 답변 상태
        /// </summary>
        public string REPLY_YN { get; set; } = string.Empty;

        public DateTime START_DATE { get;  set; }

        public DateTime END_DATE { get;  set; }
        

        /// <summary>
        /// 검색 타입
        /// </summary>
        public string SearchType { get; set; } = string.Empty;
        /// <summary>
        /// 검색 내용
        /// </summary>
        public string SearchText { get; set; } = string.Empty;

        /// <summary>
        /// 검색할 아이디
        /// </summary>
        public string LoginId { get; set; } = string.Empty;

        /// <summary>
        /// 프로그램 구분
        /// </summary>
        public string IngYn { get; set; } = string.Empty;



        public string NoticeYn { get; set; }

        public string IRInquiryYN { get; set; }


        public string COMMON_CODE_1 { get; set; }
        public string COMMON_CODE_2 { get; set; }
        public string COMMON_CODE_3 { get; set; }
    }
}