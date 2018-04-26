using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Pilot;

namespace Wow.Tv.Middle.Biz.Pilot
{
    public class PilotBiz : BaseBiz
    {
        public ListModel<TAB_BOARD> SearchList(PilotCondition condition)
        {
            ListModel<TAB_BOARD> resultData = new ListModel<TAB_BOARD>();
            
            var list = db49_wowtv.TAB_BOARD.AsQueryable();

            if(String.IsNullOrEmpty(condition.Title) == false)
            {
                list = list.Where(a => a.TITLE.Contains(condition.Title) == true);
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderBy(a => a.SEQ);

            if (condition.PageSize > -1)
            {
                if(condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();

            return resultData;
        }


        public TAB_BOARD GetAt(int seq)
        {
            return db49_wowtv.TAB_BOARD.SingleOrDefault(a => a.SEQ == seq);
        }


        public void Save(TAB_BOARD model)
        {
            TAB_BOARD data = GetAt(model.SEQ);
            if(data == null)
            {
                model.BCODE = "T02020300";
                model.VIEW_FLAG = "N";
                model.REG_DATE = DateTime.Now;
                db49_wowtv.TAB_BOARD.Add(model);
            }
            else
            {
                data.TITLE = model.TITLE;
                data.CONTENT = model.CONTENT;
            }
            db49_wowtv.SaveChanges();
        }


        public void Delete(int seq)
        {
            TAB_BOARD data = GetAt(seq);
            if (data != null)
            {
                db49_wowtv.TAB_BOARD.Remove(data);
                db49_wowtv.SaveChanges();
            }
        }




        public string TTT(PilotCondition condition)
        {
            var list = db49_wowtv.TAB_BOARD.AsQueryable();
            
            list = list.OrderBy(a => a.SEQ).Skip(condition.CurrentIndex * condition.PageSize).Take(1);

            return list.FirstOrDefault().TITLE;
        }
    }
}
