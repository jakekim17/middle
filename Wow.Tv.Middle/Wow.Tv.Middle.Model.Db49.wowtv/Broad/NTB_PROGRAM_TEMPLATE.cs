using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wow.Tv.Middle.Model.Db49.wowtv
{
    public partial class NTB_PROGRAM_TEMPLATE
    {
        public string ProgramName { get; set; }
        public NTB_ATTACH_FILE MainAttachFile { get; set; }
        public NTB_ATTACH_FILE SubAttachFile { get; set; }
        public NTB_ATTACH_FILE RectangleAttachFile { get; set; }
        public NTB_ATTACH_FILE ThumbnailAttachFile { get; set; }
    }
}
