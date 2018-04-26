using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.History;

namespace Wow.Tv.Middle.Biz.History
{
    public class CTGRManageBiz : BaseBiz
    {
        public CTGRModel<NTB_CTGR> GetList()
        {
            var resultData = new CTGRModel<NTB_CTGR>();

            var list = db49_wowtv.NTB_CTGR.AsQueryable();

            resultData.TotalDataCount = list.Count();
            resultData.ListData = list.OrderByDescending(a=> a.CTGR_YR).ThenBy(a=> a.CTGR_RN).ToList();

            if(resultData.TotalDataCount == 0)
            {
                resultData.code = 100;
            }
            else
            {
                resultData.code = list.Max(a => a.CTGR_SEQ) + 1;
            }

            return resultData;
        }

        public int Save(NTB_CTGR model, LoginUser loginUser)
        {
            var data = GetData(model.CTGR_SEQ);

            if (data != null)
            {
                data.CTGR_YR = model.CTGR_YR;
                data.CTGR_RN = model.CTGR_RN;
                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;
            }
            else
            {
                model.REG_DATE = DateTime.Now;
                model.REG_ID = loginUser.LoginId;
                model.CTGR_DISP_YN = "Y";
                if(model.CTGR_RN == null)
                {
                    model.CTGR_RN = "1";
                }
                db49_wowtv.NTB_CTGR.Add(model);
            }
            db49_wowtv.SaveChanges();

            return model.CTGR_SEQ;
        }

        public void Delete(int seq)
        {
            var data = GetData(seq);

            if (data != null)
            {
                if (data.CTGR_DISP_YN == "Y")
                {
                    data.CTGR_DISP_YN = "N";
                }
                else
                {
                    data.CTGR_DISP_YN = "Y";
                }
                db49_wowtv.SaveChanges();
            }
        }

        public NTB_CTGR GetData(int seq)
        {
            return db49_wowtv.NTB_CTGR.SingleOrDefault(a => a.CTGR_SEQ.Equals(seq));
        }

        public string GetMaxYear()
        {
            //int maxYear = 0;
            string maxYear = "";
            var data = db49_wowtv.NTB_CTGR.Where(a => a.CTGR_DISP_YN.Equals("Y")).ToList();

            if (data.Count > 0)
            {
                //maxYear = data.DefaultIfEmpty().Max(a => Int32.Parse(a.CTGR_YR));
                maxYear = data.DefaultIfEmpty().OrderByDescending(a => a.CTGR_YR).ThenBy(a => a.CTGR_RN).FirstOrDefault().CTGR_YR;
            }

            return maxYear;
        }

        public List<NTB_CTGR> GetCTGRList()
        {
            return db49_wowtv.NTB_CTGR.Where(a => a.CTGR_DISP_YN.Equals("Y")).OrderByDescending(a => a.CTGR_YR).ThenBy(a => a.CTGR_RN).ToList();
        }
    }
}
