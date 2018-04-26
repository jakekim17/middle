using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow
{
    public class CommonCodeStatic
    {
        public const string ACTION_CODE = "001000000";
        public const string CONTENT_TYPE_CODE = "002000000";
        public const string CHANNEL_CODE = "003000000";
        public const string PART_CODE = "004000000";
        public const string JOB_CODE = "007000000";
        public const string FAMILY_SITE_CODE = "008000000";
        public const string BOARD_TYPE_CODE = "011000000";
        public const string BROAD_TYPE_CODE = "014000000"; // 영어철자 잘봐야 함 보드가 아니고 브로드임
        public const string BROAD_CATEGORY_CODE = "015000000";
        public const string BROAD_SECTION_CODE = "034000000";  // BROAD_CATEGORY_CODE 와 유사하지만 신규로 만듬
        public const string TEXT_LINK_CODE = "021000000";

        /// <summary>
        /// (검색용) 어드민메뉴이거나 같이쓰는메뉴 
        /// </summary>
        public const string MENU_BROAD_ADMIN_OR_DUAL_CHANNEL_CODE = "BroadProgramAdminOrDual"; // (검색용) 어드민메뉴이거나 같이쓰는메뉴 
        /// <summary>
        /// (검색용) 프런트메뉴이거나 같이쓰는메뉴 
        /// </summary>
        public const string MENU_BROAD_FRONT_OR_DUAL_CHANNEL_CODE = "BroadProgramFrontOrDual"; // (검색용) 프런트메뉴이거나 같이쓰는메뉴 
        /// <summary>
        /// 어드민과 프런트에서 같이 쓰는 메뉴
        /// </summary>
        public const string MENU_BROAD_DUAL_CHANNEL_CODE = "BroadProgramAdminOrFront"; // 어드민과 프런트에서 같이 쓰는 메뉴
        /// <summary>
        /// 어드민에서 쓰는 메뉴
        /// </summary>
        public const string MENU_BROAD_ADMIN_CHANNEL_CODE = "BroadProgramAdmin"; // 어드민에서 쓰는 메뉴
        /// <summary>
        /// 프런트에서 쓰는 메뉴
        /// </summary>
        public const string MENU_BROAD_FRONT_CHANNEL_CODE = "BroadProgramFront"; // 프런트에서 쓰는 메뉴


        /// <summary>
        /// 뉴스 출처
        /// </summary>
        public const string NEWS_COMP_CODE  = "005000000";

        /// <summary>
        /// 뉴스 구분코드
        /// </summary>
        public const string NEWS_GUBUN_CODE = "006000000";

        /// <summary>
        /// 뉴스 DEPT CODE
        /// </summary>
        public const string NEWS_DEPT_CODE = "018000000";

        /// <summary>
        /// 뉴스 WOWCODE 분류
        /// </summary>
        public const string NEWS_WOW_CODE = "022000000";

        /// <summary>
        /// 뉴스(부동산) WOWCODE 분류
        /// </summary>
        public const string NEWS_LAND_CODE = "022001000";

        /// <summary>
        /// 뉴스(연예) WOWCODE 분류
        /// </summary>
        public const string NEWS_ENTERTAIN_CODE = "022012000";

        /// <summary>
        /// 뉴스(스포츠) WOWCODE 분류
        /// </summary>
        public const string NEWS_SPORT_CODE = "022013000";

        /// <summary>
        /// 뉴스 오피니언  분류
        /// </summary>
        public const string NEWS_OPINION_CODE = "026000000";

        /// <summary>
        /// 뉴스 기자 구분
        /// </summary>
        public const string NEWS_REPORTER_CODE = "024000000";

        /// <summary>
        /// 뉴스 관리자 기자 리스트 부서 코드
        /// </summary>
        public const string NEWS_REPORTER_DEPT_CODE = "029000000";


        /// <summary>
        /// 통합게시판 공지사항 공통코드
        /// </summary>
        public const string INTEGRATED_BOARD_NOTICE_CODE = "016001000";
        /// <summary>
        /// 통합게시판 FAQ 공통코드
        /// </summary>
        public const string INTEGRATED_BOARD_FAQ_CODE = "016002000";
        /// <summary>
        /// 통합게시판 공고게시판 공통코드
        /// </summary>
        public const string INTEGRATED_BOARD_OFFICIAL_CODE = "016003000";
        /// <summary>
        /// 통합게시판 1:1 게시판 공통코드
        /// </summary>
        public const string INTEGRATED_BOARD_INQUIRY_CODE = "027000000";
        /// <summary>
        /// 매수 수수료 공통코드
        /// </summary>
        public const string TRADINGSTAR_BUY_COMMISSION_CODE = "019001000";

        /// <summary>
        /// 매도 수수료 공통코드
        /// </summary>
        public const string TRADINGSTAR_SELL_COMMISSION_CODE = "019002000";

        /// <summary>
        /// 매도 거래세 공통코드
        /// </summary>
        public const string TRADINGSTAR_SELL_TEX_CODE = "019003000";

        /// <summary>
        /// 구글 트래픽 메뉴별 공통코드
        /// </summary>
        public const string GOOGLE_TRAFFIC_MENU_CODE = "025001000";

        /// <summary>
        /// 구글 트래픽 기사 제공사별 공통코드
        /// </summary>
        public const string GOOGLE_TRAFFIC_ART_CODE = "025003000";

        /// <summary>
        /// MAIN화면 QNA 통계 코드
        /// </summary>
        public const string MAIN_QNA_SEQ_CODE = "020001000";

        /// <summary>
        /// MAIN화면 IR 통계 코드
        /// </summary>
        public const string MAIN_IR_SEQ_CODE = "020002000";

        /// <summary>
        /// MAIN화면 고객제보 통계 코드
        /// </summary>
        public const string MAIN_REPORT_SEQ_CODE = "020003000";

        /// <summary>
        /// MAIN화면 고객제보  코드
        /// </summary>
        public const string MAIN_INQUIRY_CODE = "032000000";
        

        /// <summary>
        /// 뉴스 메인(뉴스스탠드 관리 개수)
        /// </summary>
        public const int NEWS_STAND_MANUAL_COUNT = 13;
        /// <summary>
        /// 뉴스 메인(관련뉴스 관리 개수)
        /// </summary>
        public const int NEWS_RELATION_MANUAL_COUNT = 10;
        /// <summary>
        /// 뉴스(카드뉴스 관리 개수)
        /// </summary>
        public const int NEWS_CARD_MANUAL_COUNT = 12;
        /// <summary>
        /// 뉴스(영상뉴스 관리 개수)
        /// </summary>
        public const int NEWS_VOD_MANUAL_COUNT = 5;
        /// <summary>
        /// 뉴스(부동산 관리 개수)
        /// </summary>
        public const int NEWS_LAND_MANUAL_COUNT = 6;
        /// <summary>
        /// 뉴스(연예.스포츠 관리 개수)
        /// </summary>
        public const int NEWS_ENTERTAIN_MANUAL_COUNT = 5;

        /// <summary>
        /// 이벤트관리(이벤트, 강연회) 사이트 구분코드
        /// </summary>
        public const String VIEW_SITE_CODE = "023000000";



        /// <summary>
        /// 통합게시판 1:1 게시판 메뉴코드
        /// </summary>
        public const string INTEGRATED_BOARD_INQUIRY_MENU_CODE = "030000000";

        /// <summary>
        /// 통계 프리랜서 목록 코드
        /// </summary>
        public const string FREELANCER_CODE = "045000000";

        /// <summary>
        /// 통합게시판 IR 1:1 게시판 공통코드
        /// </summary>
        public const string INTEGRATED_BOARD_IR_INQUIRY_CODE = "044000000";


        public const string CUSTOMER_INQUIRY_RECUITE_CODE = "042001000";
        public const string CUSTOMER_INQUIRY_AD_CODE = "043001000";
        public const string CUSTOMER_INQUIRY_BUSINESS_CODE = "044000000";

        public const string EMAIL_CODE = "036000000";
    }
}
