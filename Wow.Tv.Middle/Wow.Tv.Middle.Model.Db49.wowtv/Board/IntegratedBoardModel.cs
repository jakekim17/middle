using System.Collections.Generic;
using Wow.Tv.Middle.Model.Db49.wowtv;

namespace Wow.Tv.Middle.Model.Common
{
    public class IntegratedBoard<T> : ListModel<T> where T : class
    {
        public NTB_BOARD BoardInfo { get; set; }
        public List<NTB_COMMON_CODE> CommonCodes { get; set; }
    }
}