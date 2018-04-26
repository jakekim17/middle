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
    public class BannerBiz : BaseBiz
    {
        public NTB_PROGRAM_BANNER GetAt(int programBannerSeq)
        {
            NTB_PROGRAM_BANNER model = db49_wowtv.NTB_PROGRAM_BANNER.SingleOrDefault(a => a.PROGRAM_BANNER_SEQ == programBannerSeq);

            if (model != null)
            {
                AttachFile.AttachFileBiz attachFileBiz = new AttachFile.AttachFileBiz();
                var attachFile = attachFileBiz.GetAt("NTB_PROGRAM_BANNER", model.PROGRAM_BANNER_SEQ.ToString());
                model.AttachFile = attachFile;
            }

            return model;
        }

        public ListModel<NTB_PROGRAM_BANNER> SearchList(BannerCondition condition)
        {
            DateTime nowDate = DateTime.Now.Date;
            ListModel<NTB_PROGRAM_BANNER> resultData = new ListModel<NTB_PROGRAM_BANNER>();

            var list = db49_wowtv.NTB_PROGRAM_BANNER.Where(a => a.DEL_YN == "N");
            list = list.Where(a => a.PRG_CD == condition.ProgramCode);

            if (condition.StartDate != null)
            {
                list = list.Where(a => a.START_DATE >= condition.StartDate.Value);
            }

            if (condition.EndDate != null)
            {
                list = list.Where(a => a.END_DATE <= condition.EndDate.Value);
            }

            if (String.IsNullOrEmpty(condition.PublishYn) == false)
            {
                list = list.Where(a => a.PUBLISH_YN == condition.PublishYn);

                //DateTime nowDate = DateTime.Now;
                //if (condition.PublishYn == "Y")
                //{
                //    list = list.Where(a => a.START_DATE <= nowDate && a.END_DATE >= nowDate);
                //}
                //else
                //{
                //    list = list.Where(a => a.START_DATE > nowDate || a.END_DATE < nowDate);
                //}
            }

            if (String.IsNullOrEmpty(condition.BannerName) == false)
            {
                list = list.Where(a => a.BANNER_NAME.Contains(condition.BannerName) == true);
            }


            if (condition.IsNow != null)
            {
                list = list.Where(a => a.START_DATE <= nowDate && a.END_DATE >= nowDate);
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.REG_DATE);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = list.ToList();

            AttachFile.AttachFileBiz attachFileBiz = new AttachFile.AttachFileBiz();
            foreach (var item in resultData.ListData)
            {
                var attachFile = attachFileBiz.GetAt("NTB_PROGRAM_BANNER", item.PROGRAM_BANNER_SEQ.ToString());
                item.AttachFile = attachFile;

                if (item.AttachFile == null)
                {
                    item.AttachFile = new NTB_ATTACH_FILE();
                }
            }

            return resultData;
        }

        public int Save(NTB_PROGRAM_BANNER model, LoginUser loginUser)
        {
            var prev = GetAt(model.PROGRAM_BANNER_SEQ);

            if (prev == null)
            {
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;

                model.DEL_YN = "N";

                db49_wowtv.NTB_PROGRAM_BANNER.Add(model);
            }
            else
            {
                prev.BANNER_NAME = model.BANNER_NAME;
                prev.START_DATE = model.START_DATE;
                prev.END_DATE = model.END_DATE;
                prev.LINK_TYPE = model.LINK_TYPE;
                prev.URL = model.URL;
                prev.REMARK = model.REMARK;
                prev.PUBLISH_YN = model.PUBLISH_YN;

                prev.MOD_ID = loginUser.LoginId;
                prev.MOD_DATE = DateTime.Now;
            }
            db49_wowtv.SaveChanges();


            if (model.AttachFile != null)
            {
                model.AttachFile.TABLE_CODE = "NTB_PROGRAM_BANNER";
                model.AttachFile.TABLE_KEY = model.PROGRAM_BANNER_SEQ.ToString();

                new AttachFile.AttachFileBiz().Create(model.AttachFile);
            }


            return model.PROGRAM_BANNER_SEQ;
        }

        public void Delete(int programBannerSeq)
        {
            var prev = GetAt(programBannerSeq);

            prev.DEL_YN = "Y";
            //db49_wowtv.NTB_PROGRAM_BANNER.Remove(prev);

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
