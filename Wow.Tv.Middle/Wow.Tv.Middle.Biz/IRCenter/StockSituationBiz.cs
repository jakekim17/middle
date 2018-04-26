using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.StockSituation;

namespace Wow.Tv.Middle.Biz.IRCenter
{
    public class StockSituationBiz : BaseBiz
    {
        public StockModel<TAB_STOCK_SITUATION> GetList()
        {
            var resultData = new StockModel<TAB_STOCK_SITUATION>();

            resultData.ListData = db49_wownet.TAB_STOCK_SITUATION
                                 .OrderBy(a => a.DISP_ORDER)
                                 .ToList();

            if(resultData.ListData.Count > 0)
            {
                resultData.totalStockCnt = (int)db49_wownet.TAB_STOCK_SITUATION.Sum(a => a.STOCK_CNT);
            }
            return resultData;
        }

        public int Save(TAB_STOCK_SITUATION model)
        {
            var data = GetData(model.SEQ);
            if(data != null)
            {
                data.STOCK_FLAG = model.STOCK_FLAG;
                data.STOCK = model.STOCK;
                data.NAME = model.NAME;
                data.STOCK_CNT = model.STOCK_CNT;
            }
            else
            {
                var order = db49_wownet.TAB_STOCK_SITUATION.Max(a => a.DISP_ORDER);
                if( order <= 0)
                {
                    order = 1;
                }
                model.DISP_ORDER = (byte)(order + 1);
                model.REG_DATE = DateTime.Now;
                db49_wownet.TAB_STOCK_SITUATION.Add(model);
            }
            db49_wownet.SaveChanges();

            return model.SEQ;
        }

        public void Delete(int seq)
        {
            var data = GetData(seq);
            if(data != null)
            {
                db49_wownet.TAB_STOCK_SITUATION.Remove(data);
                db49_wownet.SaveChanges();
            }
        }

        public void UpdateOrder(int seq, bool isUp)
        {
            TAB_STOCK_SITUATION updateStock = null;

            var data = GetData(seq);
            if(data != null)
            {
                if (isUp == true)
                {
                    updateStock = db49_wownet.TAB_STOCK_SITUATION.Where(a => a.DISP_ORDER <= data.DISP_ORDER && a.SEQ != data.SEQ)
                                .OrderByDescending(a => a.DISP_ORDER).ThenByDescending(a => a.REG_DATE).FirstOrDefault();
                }
                else
                {
                    updateStock = db49_wownet.TAB_STOCK_SITUATION.Where(a => a.DISP_ORDER >= data.DISP_ORDER && a.SEQ != data.SEQ).OrderBy(a => a.DISP_ORDER).FirstOrDefault();
                }

                if (updateStock != null)
                {
                    var order = updateStock.DISP_ORDER;
                    updateStock.DISP_ORDER = data.DISP_ORDER;
                    data.DISP_ORDER = order;

                    db49_wownet.SaveChanges();
                }
            }
        }

        public TAB_STOCK_SITUATION GetData(int seq)
        {
            return db49_wownet.TAB_STOCK_SITUATION.SingleOrDefault(a => a.SEQ.Equals(seq));
        }
    }
}
