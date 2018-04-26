using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.Broad;

namespace Wow.Tv.Middle.Biz.Broad
{
    public class ProgramGroupBiz : BaseBiz
    {
        public NTB_PROGRAM_GROUP GetAt(int programGroupSeq)
        {
            var model = db49_wowtv.NTB_PROGRAM_GROUP.SingleOrDefault(a => a.PROGRAM_GROUP_SEQ == programGroupSeq);

            if (model != null)
            {
                model.MainAttachFile = new AttachFile.AttachFileBiz().GetAt("NTB_PROGRAM_GROUP", model.PROGRAM_GROUP_SEQ.ToString());
            }

            return model;
        }


        public NTB_PROGRAM_GROUP GetAtByMainCode(string masterProgramCode)
        {
            return db49_wowtv.NTB_PROGRAM_GROUP.FirstOrDefault(a => a.DEL_YN == "N" && a.MASTER_PRG_CD == masterProgramCode);
        }




        public ListModel<NTB_PROGRAM_GROUP> SearchList(BroadGroupCondition condition)
        {
            ListModel<NTB_PROGRAM_GROUP> resultData = new ListModel<NTB_PROGRAM_GROUP>();

            var list = db49_wowtv.NTB_PROGRAM_GROUP.Where(a => a.DEL_YN == "N");

            if(String.IsNullOrEmpty(condition.GroupName) == false)
            {
                list = list.Where(a => a.GROUP_NAME.Contains(condition.GroupName) == true);
            }

            if(condition.programCodeList != null)
            {
                List<int> programTemplateSeqList = db49_wowtv.NTB_PROGRAM_TEMPLATE.Where(a => condition.programCodeList.Contains(a.PRG_CD)).Select(a => a.PROGRAM_TEMPLATE_SEQ).ToList();
                List<int> programGroupSeqList = db49_wowtv.NTB_PROGRAM_TEMPLATE_GROUP.Where(a => programTemplateSeqList.Contains(a.PROGRAM_TEMPLATE_SEQ)).Select(a => a.PROGRAM_GROUP_SEQ).ToList();

                list = list.Where(a => programGroupSeqList.Contains(a.PROGRAM_GROUP_SEQ));
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderBy(a => a.GROUP_NAME);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();

            return resultData;
        }


        public int Save(NTB_PROGRAM_GROUP model, LoginUser loginUser)
        {
            var prev = GetAt(model.PROGRAM_GROUP_SEQ);

            if (prev == null)
            {
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;

                model.DEL_YN = "N";

                db49_wowtv.NTB_PROGRAM_GROUP.Add(model);
            }
            else
            {
                prev.MASTER_PRG_CD = model.MASTER_PRG_CD;
                prev.GROUP_NAME = model.GROUP_NAME;
                prev.TYPE_CODE = model.TYPE_CODE;

                prev.MOD_ID = loginUser.LoginId;
                prev.MOD_DATE = DateTime.Now;
            }

            
            db49_wowtv.SaveChanges();


            if (model.MainAttachFile != null)
            {
                model.MainAttachFile.TABLE_CODE = "NTB_PROGRAM_GROUP";
                model.MainAttachFile.TABLE_KEY = model.PROGRAM_GROUP_SEQ.ToString();

                new AttachFile.AttachFileBiz().Create(model.MainAttachFile);
            }

            return model.PROGRAM_GROUP_SEQ;
        }


        public void Delete(int programGroupSeq)
        {
            var prev = GetAt(programGroupSeq);

            prev.DEL_YN = "Y";
            
            db49_wowtv.SaveChanges();
        }


        public void DeleteList(List<int> seqList)
        {
            foreach (int item in seqList)
            {
                Delete(item);
            }
        }
    }
}
