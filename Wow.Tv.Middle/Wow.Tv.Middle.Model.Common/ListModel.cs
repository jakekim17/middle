using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Common
{
    public class ListModel<T>
    {
        public int TotalDataCount { get; set; }
        public List<T> ListData { get; set; }
        public List<T> ListData2 { get; set; }

        public int AddInfoInt1 { get; set; }
        public int AddInfoInt2 { get; set; }
        public int AddInfoInt3 { get; set; }

        public string AddInfoString1 { get; set; }
        public string AddInfoString2 { get; set; }
        public string AddInfoString3 { get; set; }
    }
}
