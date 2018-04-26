using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db89.wowbill
{
    public partial class tblUser
    {
        public bool IsBlackList { get; set; }
        public string AlertText { get; set; }
        public bool IsHumanUser { get; set; }
        public string ShareUserId { get; set; }
        public string SharePassword { get; set; }
        public bool RequiredPasswordChange { get; set; }
        public bool Rejected { get; set; }
        public string RejectedReason { get; set; }
    }
}
