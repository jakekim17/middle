using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.Biz.NewsCenter
{
    /// <summary>
    /// <para>  뉴스 스탠드 Biz</para>
    /// <para>- 작  성  자 : ABC솔루션 오규환</para>
    /// <para>- 최초작성일 : 2017-09-05</para>
    /// <para>- 최종수정자 : ABC솔루션 오규환</para>
    /// <para>- 최종수정일 : 2017-09-05</para>
    /// <para>- 주요변경로그</para>
    /// <para>  2017-09-05 생성</para>
    /// </summary>
    /// <remarks></remarks>
    public class NewsStandBiz : BaseBiz
    {
        /// <summary>
        /// 뉴스 스탠드 해드라인 리스트 SP
        /// </summary>
        /// <returns>ListModel<usp_newsStandMetaXMLTopImg_Result></returns>
        public ListModel<usp_newsStandMetaXMLTopImg_Result> GetNewsStandHeadLine()
        {
            ListModel<usp_newsStandMetaXMLTopImg_Result> resultData = new ListModel<usp_newsStandMetaXMLTopImg_Result>();

            resultData.ListData = db49_Article.usp_newsStandMetaXMLTopImg().ToList();

            return resultData;
        }

        /// <summary>
        /// 뉴스 스탠드 핫 키워드 SP
        /// </summary>
        /// <returns>ListModel<NUP_NEWSSTAND_KEWORD_SELECT_Result></returns>
        public ListModel<NUP_NEWSSTAND_KEWORD_SELECT_Result> GetNewsStandHotKeyword()
        {
            ListModel<NUP_NEWSSTAND_KEWORD_SELECT_Result> resultData = new ListModel<NUP_NEWSSTAND_KEWORD_SELECT_Result>();

            resultData.ListData = db49_Article.NUP_NEWSSTAND_KEWORD_SELECT().ToList();

            return resultData;
        }


    }
}
