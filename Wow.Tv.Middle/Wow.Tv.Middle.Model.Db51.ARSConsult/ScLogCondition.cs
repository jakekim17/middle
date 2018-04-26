using System;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db51.ARSConsult
{
    public class ScLogCondition : BaseCondition
    {
        public string SearchType { get;  set; }

        public DateTime? SearchDate { get;  set; }
        public string Phone { get; set; }
        public string Msg { get; set; }
    }
}