using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.wownet
{
    public class MyWriteCollectCondition : BaseCondition
    {
        /// <summary>
        /// 검색할 아이디
        /// </summary>
        public string LoginId { get; set; } = string.Empty;
    }
}
