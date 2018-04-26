using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.MyProgram;

namespace Wow.Tv.Middle.Biz.MyProgram
{
    public class ProgramIntroBiz : BaseBiz
    {

        public NTB_PROGRAM_INTRO GetAt(string programCode)
        {
            NTB_PROGRAM_INTRO model = db49_wowtv.NTB_PROGRAM_INTRO.SingleOrDefault(a => a.PRG_CD == programCode);

            if (model != null)
            {
                AttachFile.AttachFileBiz attachFileBiz = new AttachFile.AttachFileBiz();
                var attachFile = attachFileBiz.GetAt("NTB_PROGRAM_INTRO", model.PROGRAM_INTRO_SEQ.ToString());
                model.AttachFile = attachFile;
            }

            return model;
        }


        public int Save(NTB_PROGRAM_INTRO model, LoginUser loginUser)
        {
            var prev = GetAt(model.PRG_CD);

            if (prev == null)
            {
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;

                db49_wowtv.NTB_PROGRAM_INTRO.Add(model);
            }
            else
            {
                prev.TITLE = model.TITLE;
                prev.REMARK = model.REMARK;

                prev.MOD_ID = loginUser.LoginId;
                prev.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();


            if (model.AttachFile != null)
            {
                model.AttachFile.TABLE_CODE = "NTB_PROGRAM_INTRO";
                model.AttachFile.TABLE_KEY = model.PROGRAM_INTRO_SEQ.ToString();

                new AttachFile.AttachFileBiz().Create(model.AttachFile);
            }


            return model.PROGRAM_INTRO_SEQ;
        }

    }
}
