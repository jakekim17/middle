using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.Article;
using Wow.Tv.Middle.Model.Db49.Article.HubBusiness;

namespace Wow.Tv.Middle.Biz.NewsCenter
{
    public class HubBusinessBiz : BaseBiz
    {
        public HubBusinessModel<NTB_HUB_BUSINESS> GetList()
        {
            var resultData = new HubBusinessModel<NTB_HUB_BUSINESS>();

            var list = db49_Article.NTB_HUB_BUSINESS.Where(a => a.DEL_YN.Equals("N")).OrderByDescending(a => a.SEQ).AsQueryable();

            resultData.ListData = list.ToList();

            return resultData;
        }
        

        public int Save(NTB_HUB_BUSINESS model, LoginUser loginUser)
        {
            var data = GetData(model.SEQ).HubData;

            if(data != null)
            {//업데이트
                if(model.HUB_IMAGE != null)
                {
                    data.HUB_IMAGE = model.HUB_IMAGE;
                }
                data.HUB_TITLE = model.HUB_TITLE;
                data.HUB_URL = model.HUB_URL;
                data.MOD_DATE = DateTime.Now;
                data.OPEN_YN = model.OPEN_YN;
            }
            else
            {//인서트

                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.DEL_YN = "N";
                model.OPEN_YN = "N";
                db49_Article.NTB_HUB_BUSINESS.Add(model);
            }

            try
            {

                db49_Article.SaveChanges();

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Wow.Fx.WowLog.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }

            return model.SEQ;
        }

        public void Delete(int[] deleteList)
        {

            foreach (var index in deleteList)
            {
                var data = GetData(index).HubData;

                if (data != null)
                {
                    data.DEL_YN = "Y";
                }
                db49_Article.SaveChanges();

            }

        }
        
        public HubBusinessModel<NTB_HUB_BUSINESS> GetData(int seq)
        {
            var resultData = new HubBusinessModel<NTB_HUB_BUSINESS>();

            resultData.HubData = db49_Article.NTB_HUB_BUSINESS.Where(a => a.SEQ.Equals(seq) && a.DEL_YN.Equals("N")).FirstOrDefault();

            return resultData;
        }

    }
}
