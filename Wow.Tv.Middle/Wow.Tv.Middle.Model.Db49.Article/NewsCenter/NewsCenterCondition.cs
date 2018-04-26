using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.Article.NewsCenter
{
    public class NewsCenterCondition : BaseCondition
    {
        /// <summary>
        /// WOWCODE
        /// </summary>
        public string SearchWowCode { get; set; }

        /// <summary>
        /// Class
        /// </summary>
        public string Class { get; set; }
        
        /// <summary>
        /// 출처(COMP CODE)
        /// </summary>
        public string SearchComp { get; set; }

        /// <summary>
        /// 구분코드(GUBUN CODE)
        /// </summary>
        public string SearchGubun { get; set; }

        /// <summary>
        /// 검색 색션(SEARCH SECTION)
        /// </summary>
        public string SearchSection { get; set; }

        /// <summary>
        /// 구분(DEPT CODE)
        /// </summary>
        public string SearchDept { get; set; }

        /// <summary>
        /// 검색어
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// 검색 시작일
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 검색 종료일
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 노출 여부
        /// </summary>
        public string SearchViewYN { get; set; }

        /// <summary>
        /// 진행 여부
        /// </summary>
        public string SearchOnOff { get; set; }

        /// <summary>
        /// 기사 아이디
        /// </summary>
        public string SearchArticleId { get; set; }

        /// <summary>
        /// 페이징(현재페이지)
        /// </summary>
        public int Page { get; set; } = 1;
        
    }
}
