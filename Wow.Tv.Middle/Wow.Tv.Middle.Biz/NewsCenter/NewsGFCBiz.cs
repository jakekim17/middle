using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;

namespace Wow.Tv.Middle.Biz.NewsCenter
{
    public class NewsGFCBiz : BaseBiz
    {
        public ListModel<NewsGFCList> NewsGFCList(string searchYear)
        {
            if (string.IsNullOrEmpty(searchYear))
            {
                searchYear = "2017&GFC";
            }
            else
            {
                searchYear = searchYear + "&GFC";
            }

            string sqlQuery = $@"
                    SELECT top 100 fts.idx, fts.compcode , fts.articledate, fts.articleid , fts.title , list.contents as body, 
                     list.contents_img, list.writer, list.email, list.imgdir, list.imgfile, list.thumbnail_type1, list.thumbnail_type2, 'T' arttype  
                    FROM dbo.tblarticlelistfts fts 
                    JOIN dbo.tblarticlelist list
                    ON fts.articleid  = list.artid and fts.compcode = list.compcode 
                    WHERE  contains(fts.title, '{searchYear}') 
                    ORDER BY ARTICLEDATE DESC ";

            var resultData = new ListModel<NewsGFCList>();
            resultData.ListData = db49_Article.Database.SqlQuery<NewsGFCList>(sqlQuery).ToList();

            return resultData;
		}
		/* List 그대로 view메소드에 노출
		public ListModel<NewsGFCList> NewsGFCView(string searchYear)
		{
			if (string.IsNullOrEmpty(searchYear))
			{
				searchYear = "2017&GFC";
			}
			else
			{
				searchYear = searchYear + "&GFC";
			}

			string sqlQuery = $@"
                    SELECT top 100 fts.idx, fts.compcode , fts.articledate, fts.articleid , fts.title , list.contents as body, 
                     list.contents_img, list.writer, list.email, list.imgdir, list.imgfile, list.thumbnail_type1, list.thumbnail_type2, 'T' arttype  
                    FROM dbo.tblarticlelistfts fts 
                    JOIN dbo.tblarticlelist list
                    ON fts.articleid  = list.artid and fts.compcode = list.compcode 
                    WHERE  contains(fts.title, '{searchYear}') 
                    ORDER BY ARTICLEDATE DESC ";

			var resultData = new ListModel<NewsGFCList>();
			resultData.ListData = db49_Article.Database.SqlQuery<NewsGFCList>(sqlQuery).ToList();

			return resultData;
		}
		*/
		public usp_WOWTVNewsCenterView_Result NewsGFCView(string artid)
		{
			if (string.IsNullOrEmpty(artid))
			{
				artid = "A201711300402";
			}


			//string sqlQuery = $@"
			//                 exec  usp_WOWTVNewsCenterView '{artid}' ";

			//var resultData = new usp_WOWTVNewsCenterView_Result();
			//resultData = db49_Article.Database.SqlQuery<usp_WOWTVNewsCenterView_Result>(sqlQuery).FirstOrDefault();

			var resultData = db49_Article.usp_WOWTVNewsCenterView(artid).FirstOrDefault();

			return resultData;
		}

		/*
				public usp_WOWTVNewsCenterView_Result GetNewsGFCView(string articleId)
				{
					usp_WOWTVNewsCenterView_Result SingleRow = new usp_WOWTVNewsCenterView_Result();

					try
					{
						SingleRow = db49_Article.usp_WOWTVNewsCenterView_Result(articleId).SingleOrDefault();
					}
					catch (Exception ex)
					{
						Wow.Fx.WowLog.Write("GetNewsGFCView => " + ex.Message);

						if (ex.InnerException != null)
						{
							Wow.Fx.WowLog.Write("GetNewsGFCView Inner => " + ex.InnerException.Message);
						}
					}

					return SingleRow;
				}
				*/

		public void sendEmailGFC(string email, string year, string language)
		{
			if (email != "")
			{
				string executeSql = " exec usp_ExportSendEmail_GFC	{0}, {1}, {2} ";
				db51_ems50.Database.ExecuteSqlCommand(executeSql, email, year, language);
			}

		}

	}
}
