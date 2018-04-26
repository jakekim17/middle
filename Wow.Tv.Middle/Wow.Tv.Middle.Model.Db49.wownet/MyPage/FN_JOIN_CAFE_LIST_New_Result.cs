using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Db49.wowcafe;

namespace Wow.Tv.Middle.Model.Db49.wownet
{
    public partial class FN_JOIN_CAFE_LIST_New_Result
    {
        public List<usp_Select_TopNewBoard_sakal_Result> TopnewBoardSakalList { get; set; }
        public List<usp_Select_TopNewBoard_Result> TopNewBoardList { get; set; }
    }
}
