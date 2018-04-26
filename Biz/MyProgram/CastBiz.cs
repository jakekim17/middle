using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.broadcast.MyProgram;
using Wow.Tv.Middle.Model.Db49.wowtv;

namespace Wow.Tv.Middle.Biz.MyProgram
{
    public class ProgramCastModel
    {
        public NTB_PROGRAM_CAST ProgramCast { get; set; }
        public NTB_ATTACH_FILE AttachFile { get; set; }
        public string ImageUrl { get; set; }

        public Pro_wowList ProWowList { get; set; }
    }

    public class CastBiz : BaseBiz
    {

        public ProgramCastModel GetAt(int programCastSeq)
        {
            ProgramCastModel model = new ProgramCastModel();
            model.ProgramCast = db49_broadcast.NTB_PROGRAM_CAST.SingleOrDefault(a => a.PROGRAM_CAST_SEQ == programCastSeq);

            if (model.ProgramCast != null)
            {
                AttachFile.AttachFileBiz attachFileBiz = new AttachFile.AttachFileBiz();
                var attachFile = attachFileBiz.GetAt("NTB_PROGRAM_CAST", model.ProgramCast.PROGRAM_CAST_SEQ.ToString());
                model.AttachFile = attachFile;
            }

            return model;
        }

        public ListModel<ProgramCastModel> SearchList(CastCondition condition)
        {
            ListModel<ProgramCastModel> resultData = new ListModel<ProgramCastModel>();

            var list = db49_broadcast.NTB_PROGRAM_CAST.AsQueryable();

            list = list.Where(a => a.DEL_YN == "N");
            list = list.Where(a => a.PRG_CD == condition.ProgramCode);

            if (condition.CastType != null)
            {
                list = list.Where(a => a.CAST_TYPE == condition.CastType);
            }

            if (condition.PublishYn != null)
            {
                list = list.Where(a => a.PUBLISH_YN == condition.PublishYn);
            }

            if (String.IsNullOrEmpty(condition.SearchText) == false)
            {
                switch(condition.SearchType)
                {
                    case "All":
                        list = list.Where(a => a.CAST_NAME.Contains(condition.SearchText) == true || (a.Pro_wowList != null && a.Pro_wowList.Wowtv_id.Contains(condition.SearchText) == true));
                        break;
                    case "Name":
                        list = list.Where(a => a.CAST_NAME.Contains(condition.SearchText) == true);
                        break;
                    case "Id":
                        list = list.Where(a => a.Pro_wowList != null && a.Pro_wowList.Wowtv_id.Contains(condition.SearchText) == true);
                        break;
                }
            }
            
            resultData.TotalDataCount = list.Count();

            list = list.OrderByDescending(a => a.PROGRAM_CAST_SEQ);

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 20;
                }
                list = list.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.ListData = new List<ProgramCastModel>();
            foreach (var item in list)
            {
                ProgramCastModel programCastModel = new ProgramCastModel();
                programCastModel.ProgramCast = item;
                programCastModel.ProWowList = db49_broadcast.Pro_wowList.SingleOrDefault(a => a.Pay_no == item.pay_no);
                item.Pro_wowList = null;

                if (item.CAST_TYPE == "Make")
                {
                    AttachFile.AttachFileBiz attachFileBiz = new AttachFile.AttachFileBiz();
                    var attachFile = attachFileBiz.GetAt("NTB_PROGRAM_CAST", item.PROGRAM_CAST_SEQ.ToString());
                    if (attachFile != null)
                    {
                        programCastModel.ImageUrl = attachFile.REAL_WEB_PATH;
                    }
                }
                else
                {
                    if (programCastModel.ProWowList != null)
                    {
                        programCastModel.ImageUrl = "http://image.wownet.co.kr/" + programCastModel.ProWowList.Photo_Service;
                    }
                }

                resultData.ListData.Add(programCastModel);
            }
            
            return resultData;
        }

        public int Save(NTB_PROGRAM_CAST model, NTB_ATTACH_FILE attachFile , LoginUser loginUser)
        {
            var prev = GetAt(model.PROGRAM_CAST_SEQ);

            if (prev == null || prev.ProgramCast == null)
            {
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;

                model.REG_NAME = loginUser.UserName;

                model.DEL_YN = "N";

                db49_broadcast.NTB_PROGRAM_CAST.Add(model);
            }
            else
            {
                prev.ProgramCast.pay_no = model.pay_no;
                prev.ProgramCast.CAST_TYPE = model.CAST_TYPE;
                prev.ProgramCast.PUBLISH_YN = model.PUBLISH_YN;
                prev.ProgramCast.MONDAY_YN = model.MONDAY_YN;
                prev.ProgramCast.TUESDAY_YN = model.TUESDAY_YN;
                prev.ProgramCast.WEDNESDAY_YN = model.WEDNESDAY_YN;
                prev.ProgramCast.THURSDAY_YN = model.THURSDAY_YN;
                prev.ProgramCast.FRIDAY_YN = model.FRIDAY_YN;
                prev.ProgramCast.STURDAY_YN = model.STURDAY_YN;
                prev.ProgramCast.SUNDAY_YN = model.SUNDAY_YN;
                prev.ProgramCast.REMARK = model.REMARK;
                prev.ProgramCast.CAST_NAME = model.CAST_NAME;

                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;

                model.REG_NAME = loginUser.UserName;
            }
            db49_broadcast.SaveChanges();



            if (attachFile != null)
            {
                attachFile.TABLE_CODE = "NTB_PROGRAM_CAST";
                attachFile.TABLE_KEY = model.PROGRAM_CAST_SEQ.ToString();

                new AttachFile.AttachFileBiz().Create(attachFile);
            }



            return model.PROGRAM_CAST_SEQ;
        }

        public void Delete(int programCastSeq)
        {
            var prev = GetAt(programCastSeq);

            prev.ProgramCast.DEL_YN = "Y";
            //db49_broadcast.NTB_PROGRAM_CAST.Remove(prev);

            db49_broadcast.SaveChanges();
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
