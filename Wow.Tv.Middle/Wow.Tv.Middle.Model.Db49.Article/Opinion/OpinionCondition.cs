using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wow.Tv.Middle.Model.Common;

namespace Wow.Tv.Middle.Model.Db49.Article.Opinion
{
    public class OpinionCondition : BaseCondition
    {
        public string Class { set; get; }
        public string Text { set; get; }
        public int Seq { set; get; }
    }
}