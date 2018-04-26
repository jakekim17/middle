using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.TextAndLink;

namespace Wow.Tv.Middle.Biz.NewsCenter
{
    public class TextAndLinkBiz : BaseBiz
    {
        public TxtNLinkModel<JOIN_TXTNLINK_CODE> GetList()
        {
            var resultData = new TxtNLinkModel<JOIN_TXTNLINK_CODE>();
            var list = db49_Article.NTB_TEXT_LINK.AsQueryable().ToList();

            resultData.CodeList = db49_wowtv.NTB_COMMON_CODE.Where(a => a.UP_COMMON_CODE.Equals(CommonCodeStatic.TEXT_LINK_CODE)).ToList();

            var joinList = from l in list
                           join c in db49_wowtv.NTB_COMMON_CODE.Where(a => a.UP_COMMON_CODE.Equals(CommonCodeStatic.TEXT_LINK_CODE))
                           on l.CODE equals c.CODE_VALUE1
                           select new JOIN_TXTNLINK_CODE()
                           {
                               SEQ = l.SEQ,
                               CODE = l.CODE,
                               KEYWORD = l.KEYWORD,
                               LINK = l.LINK,
                               ARTICLE_ID = l.ARTICLE_ID,
                               MOD_DATE = l.MOD_DATE,
                               REG_DATE = l.REG_DATE,
                               CODE_NAME = c.CODE_NAME,
                               CODE_VALUE1 = c.CODE_VALUE1
                           };

            var JoinCodeName = joinList.Select(a => a.CODE_NAME);

            var joinCommonCode = (from a in ((from x in db49_wowtv.NTB_COMMON_CODE
                                              where x.UP_COMMON_CODE == CommonCodeStatic.TEXT_LINK_CODE
                                              select new { CODE_NAME = x.CODE_NAME }))
                                  join b in JoinCodeName on a.CODE_NAME equals b into jointable
                                  from j in jointable.DefaultIfEmpty()
                                  group a by a.CODE_NAME into GR
                                  select new
                                  {
                                      CODE_NAME = GR.Key,
                                      CNT = JoinCodeName.Where(a => a.Equals(GR.Key)).Count()
                                  }).OrderByDescending(a => a.CODE_NAME);

            var groupCodeCount = new Dictionary<string, int>();

            foreach (var item in joinCommonCode)
            {
                groupCodeCount.Add(item.CODE_NAME, item.CNT);
            }

            resultData.GroupCodeCount = groupCodeCount;
            resultData.ListData = joinList.OrderBy(a => a.CODE_NAME).ToList();

            return resultData;
        }

        public int Save(NTB_TEXT_LINK model, LoginUser loginUser)
        {
            var data = GetData(model.SEQ);
            if (data != null)
            {
                data.LINK = model.LINK;
                data.KEYWORD = model.KEYWORD;
                data.ARTICLE_ID = model.ARTICLE_ID;
                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;
            }
            else
            {
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;
                db49_Article.NTB_TEXT_LINK.Add(model);
            }
            db49_Article.SaveChanges();

            if (model.CODE.Equals("KEYWORD"))
            { 
                db49_Article.usp_tblArticleListStandKeyword_Insert();
            }

            return model.SEQ;
        }

        public void Delete(int[] deleteList)
        {
            if (deleteList != null)
            {
                var data = new NTB_TEXT_LINK();
                foreach (var index in deleteList)
                {
                    data = GetData(index);
                    if (data != null)
                    {
                        db49_Article.NTB_TEXT_LINK.Remove(data);
                    }
                }
                db49_Article.SaveChanges();
            }
        }

        public NTB_TEXT_LINK GetData(int seq)
        {
            return db49_Article.NTB_TEXT_LINK.SingleOrDefault(a => a.SEQ.Equals(seq));
        }

    }
}
