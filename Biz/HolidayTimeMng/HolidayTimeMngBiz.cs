using System;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db22.stock;
using Wow.Tv.Middle.Model.Db22.stock.Admin;

namespace Wow.Tv.Middle.Biz.HolidayTimeMng
{
    public class HolidayTimeMngBiz : BaseBiz
    {
        public ListModel<NTB_SISE_TIME> GetList(HolidayMngCondition condition)
        {
            var resultData = new ListModel<NTB_SISE_TIME>();
            var list = db22_stock.NTB_SISE_TIME.Where(a => a.DEL_YN != "Y").AsQueryable();

            if (!String.IsNullOrEmpty(condition.Gubun))
            {
                list = list.Where(a => a.GUBUN.Equals(condition.Gubun));
            }

            if (!String.IsNullOrEmpty(condition.StartDate))
            {
                condition.StartDate = condition.StartDate.Replace("-", "");
                condition.EndDate = condition.EndDate.Replace("-", "");
                list = list.Where(a => ( a.MARKET_DT.CompareTo(condition.StartDate) > 0 || a.MARKET_DT.CompareTo(condition.StartDate) == 0)
                                    && ( a.MARKET_DT.CompareTo(condition.EndDate) < 0 || a.MARKET_DT.CompareTo(condition.EndDate) == 0 ));
            }

            if (!String.IsNullOrEmpty(condition.Holiday))
            {
                list = list.Where(a => a.HOLIDAY_CD.Equals(condition.Holiday));
            }
            resultData.TotalDataCount = list.Count();

            if(condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                list = list.OrderByDescending(a=> a.MARKET_SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            return resultData;
        }

        public bool DateConfirm(NTB_SISE_TIME model)
        {
            var isDate = false;
            model.MARKET_DT = model.MARKET_DT.Replace("-", "");
            var data = GetTimeData(model.MARKET_SEQ);

            int result = db22_stock.NTB_SISE_TIME.Where(a => a.MARKET_DT.Equals(model.MARKET_DT) && a.GUBUN.Equals(model.GUBUN) && a.DEL_YN != "Y").Count();

            if (data != null && model.MARKET_DT == data.MARKET_DT)
            {
                result = 0;       
            }

            if(result > 0 )
            {
                isDate = true;
            }

            return isDate;
        }

        public int SaveTime(NTB_SISE_TIME model, LoginUser loginUser)
        {
            var data = GetTimeData(model.MARKET_SEQ);
            model.MARKET_DT = model.MARKET_DT.Replace("-", "");
            if (data != null)
            {
                data.MARKET_DT = model.MARKET_DT;
                if (model.GUBUN == "S")
                {
                    data.MARKET_STA_H = model.MARKET_STA_H;
                    data.MARKET_STA_M = model.MARKET_STA_M;
                    data.MARKET_END_H = model.MARKET_END_H;
                    data.MARKET_END_M = model.MARKET_END_M;
                }
                data.MARKET_HOLY_DESC = model.MARKET_HOLY_DESC;
                data.HOLIDAY_CD = model.HOLIDAY_CD;
                data.MOD_DATE = DateTime.Now;
                data.MOD_ID = loginUser.UserName;
            }
            else
            {
                model.REG_DATE = DateTime.Now;
                model.REG_ID = loginUser.UserName;
                model.DEL_YN = "N";
                if (model.GUBUN == "T")
                {
                    model.MARKET_STA_H = "00";
                    model.MARKET_STA_M = "00";
                    model.MARKET_END_H = "00";
                    model.MARKET_END_M = "00";
                }

                db22_stock.NTB_SISE_TIME.Add(model);
            }

            db22_stock.Database.Log = sql => Wow.Fx.WowLog.Write(sql);
            db22_stock.SaveChanges();
            return model.MARKET_SEQ;
        }

        public int SaveMaster(NTB_SISE_MASTER model, LoginUser loginUser)
        {
            var list = db22_stock.NTB_SISE_MASTER.AsQueryable();
            if(list != null && list.Count() > 0)
            {
                var data = list.FirstOrDefault();
                data.TRAD_STA_H = model.TRAD_STA_H;
                data.TRAD_STA_M = model.TRAD_STA_M;
                data.TRAD_END_H = model.TRAD_END_H;
                data.TRAD_END_M = model.TRAD_END_M;
                data.MOD_DATE = DateTime.Now;
                data.MOD_ID = loginUser.UserName;
            }
            else
            {
                model.REG_DATE = DateTime.Now;
                model.REG_ID = loginUser.UserName;
                model.DEL_YN = "N";
                model.MOD_DATE = DateTime.Now;
                model.MOD_ID = loginUser.UserName;

                db22_stock.NTB_SISE_MASTER.Add(model);
            }
            db22_stock.SaveChanges();
            return model.SISE_SEQ;
        }

        public void Delete(int seq, LoginUser loginUser)
        {
            var data = GetTimeData(seq);
            if (data != null)
            {
                data.DEL_YN = "Y";
                data.MOD_DATE = DateTime.Now;
                data.MOD_ID = loginUser.UserName;
                db22_stock.SaveChanges();
            }
        }

        public NTB_SISE_TIME GetTimeData(int seq)
        {
            return db22_stock.NTB_SISE_TIME.SingleOrDefault(a => a.MARKET_SEQ.Equals(seq));
        }

        public NTB_SISE_MASTER GetMasterData()
        {
            return db22_stock.NTB_SISE_MASTER.FirstOrDefault();
        }
    }
}
