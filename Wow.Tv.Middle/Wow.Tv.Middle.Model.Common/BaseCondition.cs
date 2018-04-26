﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Common
{
    [Serializable]
    public class BaseCondition
    {
        public int CurrentIndex { get; set; } = 0;
        public int PageSize { get; set; } = 20;

        public int CurrentMenuSeq { get; set; }

        public BaseCondition()
        {
            PageSize = 20;
        }
    }
}