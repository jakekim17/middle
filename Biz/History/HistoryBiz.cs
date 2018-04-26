using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.History;

namespace Wow.Tv.Middle.Biz.History
{
    public class HistoryBiz : BaseBiz
    {
        public HistoryModel<HIS_CTGR> GetList(HistoryCondition condition)
        {
            var resultData = new HistoryModel<HIS_CTGR>();
            var list = GetJoinData();  

            if (condition.CTGYR > 0)
            {
                list = list.Where(a => a.CTGR_SEQ.Equals(condition.CTGYR));
            }

            if (!String.IsNullOrEmpty(condition.DispYN))
            {
                list = list.Where(a => a.HIS_DISP_YN.Equals(condition.DispYN));
            }

            if (!String.IsNullOrEmpty(condition.SearchText))
            {
                switch (condition.SearchType)
                {
                    case "all":
                        list = list.Where(a => a.HIS_CONT.Contains(condition.SearchText) || a.REG_ID.Equals(condition.SearchText));
                        break;
                    case "regId":
                        list = list.Where(a => a.REG_ID.Contains(condition.SearchText));
                        break;
                    case "content":
                        list.Where(a => a.HIS_CONT.Contains(condition.SearchText));
                        break;
                }
            }

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                list = list.OrderByDescending(a => a.HIS_SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            resultData.CTGRList = GetCTGRList(condition.Orderby).ToList();
            
            return resultData;
        }

        public HistoryDetail GetDetail(int seq)
        {
            var resultData = new HistoryDetail();
            var data = GetJoinData().SingleOrDefault(a => a.HIS_SEQ.Equals(seq));

            if(data != null)
            {
                resultData.HISData = data;
            }
            resultData.CTGRList = GetCTGRList("ASC");

            return resultData;
        }

        public int Save(NTB_HIS_MNG model, LoginUser loginUser)
        {
            var data = GetData(model.HIS_SEQ);

            if(data != null)
            {
                data.CTGR_SEQ = model.CTGR_SEQ;
                data.HIS_DATE = model.HIS_DATE;
                data.HIS_CONT = model.HIS_CONT;
                data.HIS_DISP_YN = model.HIS_DISP_YN;
                data.MOD_ID = loginUser.UserName;
                data.MOD_DATE = DateTime.Now;
            }
            else
            {
                model.REG_ID = loginUser.UserName;
                model.REG_DATE = DateTime.Now;
                db49_wowtv.NTB_HIS_MNG.Add(model);
            }
            db49_wowtv.SaveChanges();

            return model.HIS_SEQ;
        }

        public void Delete(int[] seqList)
        {
            foreach (var index in seqList)
            {
                var data = GetData(index);

                if (data != null)
                {
                    db49_wowtv.NTB_HIS_MNG.Remove(data);
                    db49_wowtv.SaveChanges();
                }
            }    
        }

        public NTB_HIS_MNG GetData(int seq)
        {
            return db49_wowtv.NTB_HIS_MNG.SingleOrDefault(a => a.HIS_SEQ.Equals(seq));
        }

        public List<NTB_CTGR> GetCTGRList(string order)
        {
            if(order == "DESC")
            {
                return db49_wowtv.NTB_CTGR.Where(a => a.CTGR_DISP_YN.Equals("Y")).OrderByDescending(a => a.CTGR_YR).ThenBy(a => a.CTGR_RN).ToList();
            }
            else
            {
                return db49_wowtv.NTB_CTGR.Where(a => a.CTGR_DISP_YN.Equals("Y")).OrderBy(a => a.CTGR_YR).ThenBy(a => a.CTGR_RN).ToList();
            }
            
        }

        public IQueryable<HIS_CTGR> GetJoinData()
        {
            return db49_wowtv
                    .NTB_HIS_MNG
                    .Join(db49_wowtv.NTB_CTGR,
                     his => his.CTGR_SEQ,
                     ctg => ctg.CTGR_SEQ,
                     (his, ctg) => new HIS_CTGR()
                     {
                         HIS_SEQ = his.HIS_SEQ,
                         CTGR_SEQ = his.CTGR_SEQ,
                         CTGR_YR = ctg.CTGR_YR,
                         HIS_DATE = his.HIS_DATE,
                         HIS_CONT = his.HIS_CONT,
                         HIS_DISP_YN = his.HIS_DISP_YN,
                         REG_ID = his.REG_ID,
                         REG_DATE = (DateTime)his.REG_DATE,
                         MOD_DATE = his.MOD_DATE
                     });
        }

        public List<DtlCTGRHistory> SearchHistory(string SearchYear) {
            var resultData = new List<DtlCTGRHistory>();
            var list = new DtlCTGRHistory();

            var resultCTGR = db49_wowtv.NTB_CTGR.Where(a => a.CTGR_YR.Equals(SearchYear) && a.CTGR_DISP_YN.Equals("Y"));

            if(resultCTGR != null)
            {
                foreach (var item in resultCTGR)
                {
                    list = new DtlCTGRHistory
                    {
                        CTGR_YR = item.CTGR_YR
                    };

                    var historyData = db49_wowtv.NTB_HIS_MNG.Where(a => a.CTGR_SEQ.Equals(item.CTGR_SEQ) && a.HIS_DISP_YN.Equals("Y")).ToList();

                    if (historyData != null)
                    {
                        foreach (var hisItem in historyData)
                        {
                            var index = historyData.IndexOf(hisItem);
                            if (hisItem.HIS_DATE.Contains('.'))
                            {
                                var splitData = hisItem.HIS_DATE.Split('.');
                                if (splitData[1].Contains('~'))
                                {
                                    historyData[index].HIS_DAY = Int32.Parse(splitData[1].Split('~')[0]);
                                }
                                else
                                {
                                    historyData[index].HIS_DAY = Int32.Parse(splitData[1]);
                                }
                                historyData[index].HIS_MONTH = Int32.Parse(splitData[0]);
                            }
                            else
                            {
                                historyData[index].HIS_MONTH = Int32.Parse(hisItem.HIS_DATE);
                            }
                        }
                        list.HistoryList = historyData.OrderByDescending(a => a.HIS_MONTH).ThenByDescending(a => a.HIS_DAY).ToList();
                    }
                    resultData.Add(list);
                }
            } 
            return resultData;
        }
    }
}
