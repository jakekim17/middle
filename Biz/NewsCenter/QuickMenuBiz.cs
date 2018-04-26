using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;

namespace Wow.Tv.Middle.Biz.NewsCenter
{
    public class QuickMenuBiz : BaseBiz
    {

        /// <summary>
        /// 퀵 메뉴 마이피 기자 리스트
        /// </summary>
        /// <param name="userID">회원아이디</param>
        /// <param name="topN">개수</param>
        /// <returns>List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result></returns>
        public List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result> GetQuickMenuMypinReporter(string userID, int? topN)
        {
            List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result> resultData = new List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result>();

            resultData = db49_Article.NUP_QUICKMENU_MYPIN_REPORTER_SELECT(userID, topN).ToList();

            return resultData;

        }

        /// <summary>
        /// 퀵 메뉴 마이피 뉴스 리스트
        /// </summary>
        /// <param name="userID">회원아이디</param>
        /// <param name="topN">개수</param>
        /// <returns>List<NUP_QUICKMENU_MYPIN_NEWS_SELECT_Result></returns>
        public List<NUP_QUICKMENU_MYPIN_NEWS_SELECT_Result> GetQuickMenuMypinNews(string userID, int? topN)
        {
            List<NUP_QUICKMENU_MYPIN_NEWS_SELECT_Result> resultData = new List<NUP_QUICKMENU_MYPIN_NEWS_SELECT_Result>();

            resultData = db49_Article.NUP_QUICKMENU_MYPIN_NEWS_SELECT(userID, topN).ToList();

            return resultData;

        }

    }
}
