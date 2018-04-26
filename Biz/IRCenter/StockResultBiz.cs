using System;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.StockResult;

namespace Wow.Tv.Middle.Biz.IRCenter
{
    public class StockResultBiz : BaseBiz
    {
        public StockResultModel<TAB_STOCK_RESULT> GetList(StockResultCondition condition)
        {
            var resultData = new StockResultModel<TAB_STOCK_RESULT>();
            var list = db49_wownet.TAB_STOCK_RESULT.AsQueryable();

            var yearList = list.Select(a => a.SYEAR).Distinct();
            var roundsList = list.Select(a => a.DIVERGE);

            if (!String.IsNullOrEmpty(condition.Year))
            {
                list = list.Where(a => a.SYEAR.Equals(condition.Year));
                roundsList = list.Select(a => a.DIVERGE);
            }

            if (!String.IsNullOrEmpty(condition.Rounds))
            {
                list = list.Where(a => a.DIVERGE.Equals(condition.Rounds));
            }

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                list = list.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();
            resultData.YearList = yearList.ToList();
            resultData.RoundsList = roundsList.ToList();

            return resultData;
        }

        public StockResultDetail GetDetail(int seq)
        {
            var resultData = new StockResultDetail();   
            resultData.StockData = GetResData(seq);
            resultData.ConnectList = GetConList(seq).ToList();

            return resultData;
        }

        public int Save(StockResultDetail data, LoginUser loginUser)
        {
            var Rdata = GetResData(data.StockData.SEQ);
            if(Rdata != null)
            {
                Rdata.DIVERGE = data.StockData.DIVERGE;
                Rdata.SYEAR = data.StockData.SYEAR;
                Rdata.SMONTH = data.StockData.SMONTH;
                Rdata.SDAY = data.StockData.SDAY;
                Rdata.STIME1 = data.StockData.STIME1;
                Rdata.STIME2 = data.StockData.STIME2;
                Rdata.PLACE = data.StockData.PLACE;
                Rdata.VIEW_FLAG = data.StockData.VIEW_FLAG;
                Rdata.MOD_DATE = DateTime.Now;
                Rdata.MOD_ID = loginUser.UserName;
                
                if(data.ConnectList != null)
                {
                    foreach (var item in data.ConnectList)
                    {
                        var Cdata = GetConData(item.SEQ);
                        if (Cdata != null)
                        {
                            Cdata.CONTENT = item.CONTENT;
                            Cdata.STOCK = item.STOCK;
                        }
                        else
                        {
                            item.REG_DATE = DateTime.Now;
                            item.MSEQ = data.StockData.SEQ;
                            item.STOCK_FLAG = "";
                            db49_wownet.TAB_STOCK_RESULT_CONNECT.Add(item);
                        }
                    }
                }
                
                if(data.DeleteList != null)
                {
                    foreach (var item in data.DeleteList)
                    {
                        var Cdata = GetConData(item);
                        if (Cdata != null)
                        {
                            db49_wownet.TAB_STOCK_RESULT_CONNECT.Remove(Cdata);
                        }
                    }
                }
            }
            else
            {
                data.StockData.REG_DATE = DateTime.Now;
                data.StockData.REG_ID = loginUser.UserName;
                db49_wownet.TAB_STOCK_RESULT.Add(data.StockData);
                db49_wownet.SaveChanges();
                foreach (var item in data.ConnectList)
                {
                    item.MSEQ = data.StockData.SEQ;
                    item.REG_DATE = DateTime.Now;
                    item.STOCK_FLAG = "";
                    db49_wownet.TAB_STOCK_RESULT_CONNECT.Add(item);
                }
            }
            db49_wownet.SaveChanges();
            return data.StockData.SEQ;
        }

        public void Delete(int[] deleteList)
        {
            if(deleteList != null)
            {
                foreach(var item in deleteList)
                {
                    var Rdata = GetResData(item);
                    if(Rdata != null)
                    {
                        var Cdata = GetConList(item);
                        foreach(var Citem in Cdata)
                        {
                            db49_wownet.TAB_STOCK_RESULT_CONNECT.Remove(Citem);
                        }
                        db49_wownet.TAB_STOCK_RESULT.Remove(Rdata);
                        db49_wownet.SaveChanges();
                    }
                }
            }
        }

        public StockResultModel<JOIN_STOCK_CONNECT> GetJoinList(StockResultCondition condition)
        {
            var resultData = new StockResultModel<JOIN_STOCK_CONNECT>();
            var list = GetJoinData().Where(a => a.VIEW_FLAG.Equals("Y"));
            var conCountList = (from R in db49_wownet.TAB_STOCK_RESULT
                                join C in db49_wownet.TAB_STOCK_RESULT_CONNECT on R.SEQ equals C.MSEQ
                                where R.VIEW_FLAG == "Y"
                                group R by R.SEQ into grouped
                                select new JoinConGroup() { SEQ = (int)grouped.Key, ConnectCount = grouped.Count() });

            if (!String.IsNullOrEmpty(condition.Rounds))
            {
                list = list.Where(a => a.DIVERGE.Equals(condition.Rounds));
                int i = list.Select(a => a.SEQ).Distinct().First();
                conCountList = conCountList.Where(a => a.SEQ.Equals(i));
            }

            if (!String.IsNullOrEmpty(condition.Year))
            {
                list = list.Where(a => a.SYEAR.Equals(condition.Year));
                int i = list.Select(a => a.SEQ).Distinct().First();
                conCountList = conCountList.Where(a => a.SEQ.Equals(i));
            }
            resultData.ListData = list.OrderByDescending(a => a.SEQ).ToList();
            resultData.RoundsList = db49_wownet.TAB_STOCK_RESULT.Where(a => a.VIEW_FLAG.Equals("Y")).OrderByDescending(a => a.SEQ).Select(a => a.DIVERGE).ToList();

            var year = db49_wownet.TAB_STOCK_RESULT.Where(a => a.VIEW_FLAG.Equals("Y")).OrderByDescending(a => a.SYEAR);
            resultData.YearList = year.Select(a => a.SYEAR).ToList();
            //resultData.YearList = db49_wownet.TAB_STOCK_RESULT.Where(a => a.VIEW_FLAG.Equals("Y")).Select(a => a.SYEAR).ToList();

            resultData.ConCountList = conCountList.OrderByDescending(a => a.SEQ).ToList();
            return resultData;
        }
        

        public TAB_STOCK_RESULT GetResData(int seq)
        {
            return db49_wownet.TAB_STOCK_RESULT.SingleOrDefault(a => a.SEQ.Equals(seq));
        }

        public TAB_STOCK_RESULT_CONNECT GetConData(int seq)
        {
            return db49_wownet.TAB_STOCK_RESULT_CONNECT.SingleOrDefault(a => a.SEQ.Equals(seq));
        }

        public IQueryable<TAB_STOCK_RESULT_CONNECT> GetConList(int seq)
        {
            return db49_wownet.TAB_STOCK_RESULT_CONNECT.Where(a => a.MSEQ == seq);
        }

        public IQueryable<JOIN_STOCK_CONNECT> GetJoinData()
        {
            return db49_wownet.TAB_STOCK_RESULT
                .Join(
                db49_wownet.TAB_STOCK_RESULT_CONNECT
                , R => R.SEQ
                , C => C.MSEQ
                , (R, C) => new JOIN_STOCK_CONNECT()
                {
                    SEQ = R.SEQ,
                    DIVERGE = R.DIVERGE,
                    SYEAR = R.SYEAR,
                    SMONTH = R.SMONTH,
                    SDAY = R.SDAY,
                    STIME1 = R.STIME1,
                    STIME2 = R.STIME2,
                    PLACE = R.PLACE,
                    REG_DATE = R.REG_DATE,
                    VIEW_FLAG = R.VIEW_FLAG,
                    MSEQ = C.MSEQ,
                    CONTENT = C.CONTENT,
                    STOCK = C.STOCK,
                    STOCK_FLAG = C.STOCK_FLAG
                });
        }

        public int GetMaxYear()
        {
            return db49_wownet.TAB_STOCK_RESULT.ToList().Max(a => Int32.Parse(a.SYEAR));
        }
        
    }
}
