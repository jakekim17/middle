using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.broadcast.MyProgram;

namespace Wow.Tv.Middle.Biz.MyProgram
{
    public class ExpertBiz : BaseBiz
    {
        public Pro_wowList GetAt(int payNo)
        {
            var model = db49_broadcast.Pro_wowList.SingleOrDefault(a => a.Pay_no == payNo);
            
            return model;
        }

        public ListModel<Pro_wowList> SearchList(ExpertCondition condition)
        {
            ListModel<Pro_wowList> resultData = new ListModel<Pro_wowList>();

            var list = db49_broadcast.Pro_wowList.AsQueryable();

            list = list.Where(a => a.State == "1");
            
            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                list = list.Where(a => a.Wowtv_id.Contains(condition.SearchText) == true || a.NickName.Contains(condition.SearchText) == true);
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.FullName);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();

            return resultData;
        }

    }
}
