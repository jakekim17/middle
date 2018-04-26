using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.editVOD;
using Wow.Tv.Middle.Model.Db49.wowtv;

namespace Wow.Tv.Middle.Biz.Broad
{
    public class ProgramCmsAuthBiz : BaseBiz
    {
        public void MainFileMigration()
        {
            var list = db49_editVOD.TAB_PGM_CMS_AUTH.OrderBy(a => a.PGM_ID).AsQueryable().ToList();

            AttachFile.AttachFileBiz attachFileBiz = new AttachFile.AttachFileBiz();
            NTB_ATTACH_FILE attachFile = new NTB_ATTACH_FILE();

            foreach (var item in list)
            {
                try
                {
                    attachFile = new NTB_ATTACH_FILE();
                    attachFile.USER_UPLOAD_FILE_NAME = System.IO.Path.GetFileName(item.MAIN_BG_IMG);
                    attachFile.EXTENSION = System.IO.Path.GetExtension(attachFile.USER_UPLOAD_FILE_NAME);
                    attachFile.FILE_SIZE = 1;
                    attachFile.REAL_FILE_PATH = item.MAIN_BG_IMG;
                    attachFile.REAL_WEB_PATH = item.MAIN_BG_IMG;
                    attachFile.TABLE_CODE = "T_NEWS_PRG-MAIN";
                    attachFile.TABLE_KEY = item.PGM_ID;

                    attachFileBiz.Create(attachFile);
                }
                catch(Exception ex)
                {
                }
            }
            
        }


        //public TAB_PGM_CMS_AUTH GetAt(string programCode)
        //{
        //    TAB_PGM_CMS_AUTH model = db49_editVOD.TAB_PGM_CMS_AUTH.SingleOrDefault(a => a.PGM_ID == programCode);

        //    return model;
        //}


        //public void Save(TAB_PGM_CMS_AUTH model)
        //{
        //    var prev = GetAt(model.PGM_ID);

        //    if(prev == null)
        //    {
        //        TAB_PGM_CMS_AUTH add = new TAB_PGM_CMS_AUTH();
        //        add.PGM_ID = model.PGM_ID;
        //        add.INTRO = model.INTRO;
        //        add.MAIN_BG_IMG = model.MAIN_BG_IMG;
        //        add.SUB_BG_IMG = model.SUB_BG_IMG;
        //        add.PGM_NAME = model.PGM_NAME;
        //        add.PD_ID = "";
        //        add.DEL_YN = "N";
        //        add.DISP_YN = "";
        //        add.REG_ID = "";
        //        add.REG_DATE = DateTime.Now;

        //        db49_editVOD.TAB_PGM_CMS_AUTH.Add(add);
        //    }
        //    else
        //    {
        //        prev.INTRO = model.INTRO;
        //        if (String.IsNullOrEmpty(model.MAIN_BG_IMG) == false)
        //        {
        //            prev.MAIN_BG_IMG = model.MAIN_BG_IMG;
        //        }
        //        if (String.IsNullOrEmpty(model.SUB_BG_IMG) == false)
        //        {
        //            prev.SUB_BG_IMG = model.SUB_BG_IMG;
        //        }
        //    }

        //    db49_editVOD.SaveChanges();
        //}
    }
}
