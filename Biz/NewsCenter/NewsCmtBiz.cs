using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.NewsCenter;
using Wow.Tv.Middle.Model.Db49.Article.NewsCmt;
using Wow.Tv.Middle.Model.Db49.wowtv;

namespace Wow.Tv.Middle.Biz.NewsCenter
{
    public class NewsCmtBiz : BaseBiz
    {
        public NewsCmdCodeModel<NewsCmtManageModel> GetList(NewsCmtCondition condition)
        {
            var resultData = new NewsCmdCodeModel<NewsCmtManageModel>();
            var list = GetData().Where(a => a.DEL_YN.Equals("N"));
            
            var CodeList = db49_wowtv.NTB_COMMON_CODE.Where(a => a.UP_COMMON_CODE.Equals(CommonCodeStatic.NEWS_WOW_CODE)).ToList(); //ex)부동산/연예/사회
            var CommonCodeList = CodeList.Select(a => a.COMMON_CODE);
            

            //검색어 있는 경우
            if (!String.IsNullOrEmpty(condition.NewsGubun))
            {
                var DetailCodeList = db49_wowtv.NTB_COMMON_CODE.AsQueryable().Select(a=> a.CODE_VALUE1);

                if (!condition.NewsGubun.Equals("all")) //분류 전체 아닌경우 
                {
                    DetailCodeList = db49_wowtv.NTB_COMMON_CODE.Where(a => a.UP_COMMON_CODE.Equals(condition.NewsGubun)).Select(a => a.CODE_VALUE1);

                    var SearchedCodeList = DetailCodeList.ToList();
                    var articleList = db49_Article.ArticleClass.Where(a => SearchedCodeList.Contains(a.WOWCode)).Select(a => a.ArticleID);

                    list = list.Where(a => articleList.Contains(a.ARTICLE_ID));
                }

                if (!String.IsNullOrEmpty(condition.SearchText))
                {
                    switch (condition.SearchType)
                    {
                        case "comment":
                            list = list.Where(a => a.COMMENT.Contains(condition.SearchText));
                            break;
                        case "regId":
                            list = list.Where(a => a.REG_ID.Contains(condition.SearchText));
                            break;
                        case "all":
                            list = list.Where(a => a.COMMENT.Contains(condition.SearchText)
                                                || a.REG_ID.Contains(condition.SearchText)
                                                || a.Title.Contains(condition.SearchText));
                            break;

                    }
                }
                
            }
            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }


                if (!String.IsNullOrEmpty(condition.Sort))
                {
                    if (condition.Sort.Equals("D"))//공개순정렬
                    {
                        list = list.OrderByDescending(a => a.OPEN_YN).ThenByDescending(a => a.REG_DATE);
                    }
                    else//비공개순정렬
                    {
                        list = list.OrderBy(a => a.OPEN_YN).ThenByDescending(a => a.REG_DATE);
                    }
                }
                else
                {
                    list = list.OrderByDescending(a => a.REG_DATE);
                }
                    
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();
            resultData.CodeList = CodeList;

            return resultData;
        }

        public void Delete(int[] deleteList)
        {
            foreach (var index in deleteList)
            {
                var data = db49_Article.NTB_ARTICLE_COMMENT.SingleOrDefault((a => a.COMMENT_SEQ.Equals(index)));

                if (data != null)
                {
                    if (data.DEL_YN.Equals("Y"))
                    {
                        data.DEL_YN = "N";
                    }
                    else
                    {
                        data.DEL_YN = "Y";
                    }
                }
            }
            db49_Article.SaveChanges();

        }


        public void Update(String seq)
        {
            int convertSeq = Int32.Parse(seq);
            var data = db49_Article.NTB_ARTICLE_COMMENT.SingleOrDefault((a => a.COMMENT_SEQ.Equals(convertSeq)));

            if (data != null)
            {
                if (data.OPEN_YN.Equals("Y"))
                {
                    data.OPEN_YN = "N";
                }
                else
                {
                    data.OPEN_YN = "Y";
                }
            }
            
            db49_Article.SaveChanges();

        }

        public IQueryable<NewsCmtManageModel> GetData()
        {
            return (from list in db49_Article.tblArticleList
             join cmt in db49_Article.NTB_ARTICLE_COMMENT on list.ArtID equals cmt.ARTICLE_ID
             select new NewsCmtManageModel
             {
                 COMMENT_SEQ = cmt.COMMENT_SEQ,
                 ARTICLE_ID = cmt.ARTICLE_ID,
                 COMMENT = cmt.COMMENT,
                 DEL_YN = cmt.DEL_YN,
                 REG_DATE = cmt.REG_DATE,
                 REG_ID = cmt.REG_ID,
                 OPEN_YN = cmt.OPEN_YN,
                 Title = list.Title
             });
        }

        public ListModel<NTB_ARTICLE_COMMENT> GetCommentList(CommentCondition condition)
        {
            var resultData = new ListModel<NTB_ARTICLE_COMMENT>();
            var list = db49_Article.NTB_ARTICLE_COMMENT.Where(a => a.ARTICLE_ID.Equals(condition.ArticleId) && a.DEL_YN.Equals("N"));

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                list = list.OrderByDescending(a => a.COMMENT_SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();

            return resultData;
        }

        public void SaveComment(NTB_ARTICLE_COMMENT model, LoginUserInfo loginUserInfo)
        {
            var data = db49_Article.NTB_ARTICLE_COMMENT.SingleOrDefault(a => a.COMMENT_SEQ.Equals(model.COMMENT_SEQ));
            if(data != null)
            {
                data.COMMENT = model.COMMENT;
                data.MOD_DATE = DateTime.Now;
            }
            else
            {
                if(loginUserInfo.SnsId != null)
                {
                    model.REG_ID = loginUserInfo.SnsId.ToString();
                    model.USER_NAME = loginUserInfo.SnsName;
                    model.LOGIN_TYPE = "H";
                }
                else
                {
                    model.REG_ID = loginUserInfo.UserId;
                    model.USER_NAME = loginUserInfo.NickName;
                }
                
                if(loginUserInfo.FacebookInfo.Id != null)
                {
                    model.LOGIN_TYPE = "F";
                }

                if(loginUserInfo.KakaoInfo.Id != null)
                {
                    model.LOGIN_TYPE = "K";
                }

                if(loginUserInfo.NaverInfo.Id != null)
                {
                    model.LOGIN_TYPE = "N";
                }
                model.DEL_YN = "N";
                model.OPEN_YN = "Y";
                model.REG_DATE = DateTime.Now;
                db49_Article.NTB_ARTICLE_COMMENT.Add(model);
            }
            db49_Article.SaveChanges();
        }

        public void DeleteComment(int deleteId)
        {
            var data = db49_Article.NTB_ARTICLE_COMMENT.SingleOrDefault(a => a.COMMENT_SEQ.Equals(deleteId));
            if(data != null)
            {
                data.DEL_YN = "Y";
                db49_Article.SaveChanges();
            }
        }
    }
}
