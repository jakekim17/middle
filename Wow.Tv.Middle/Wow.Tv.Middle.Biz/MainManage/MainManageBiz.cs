using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.MainManage;

namespace Wow.Tv.Middle.Biz.MainManage
{
    public class MainManageBiz : BaseBiz
    {
        public ListModel<NTB_MAIN_MANAGE> SearchList(MainManageCondition condition)
        {
            ListModel<NTB_MAIN_MANAGE> resultData = new ListModel<NTB_MAIN_MANAGE>();

            var list = db49_wowtv.NTB_MAIN_MANAGE.Where(a => a.DEL_YN == "N");
            
            if (String.IsNullOrEmpty(condition.MainTypeCode) == false)
            {
                list = list.Where(a => a.MAIN_TYPE_CODE == condition.MainTypeCode);
            }

            resultData.TotalDataCount = list.Count();

            list = list.OrderBy(a => a.SORT_ORDER);

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


        public NTB_MAIN_MANAGE GetAt(int mainManageSeq)
        {
            var model = db49_wowtv.NTB_MAIN_MANAGE.SingleOrDefault(a => a.MAIN_MANAGE_SEQ == mainManageSeq);

            if (model != null)
            {
                var file = new AttachFile.AttachFileBiz().GetAt("NTB_MAIN_MANAGE", model.MAIN_MANAGE_SEQ.ToString());
                if (file == null)
                {
                    model.AttachFile = new NTB_ATTACH_FILE();
                }
                else
                {
                    model.AttachFile = file;
                }
            }

            return model;
        }



        public int Save(NTB_MAIN_MANAGE model, LoginUser loginUser)
        {
            if(model.MAIN_MANAGE_SEQ == 0)
            {
                if (model.ACTIVE_YN == "Y")
                {
                    var prevListCount = db49_wowtv.NTB_MAIN_MANAGE.Where(a => a.DEL_YN == "N" && a.ACTIVE_YN == "Y" && a.MAIN_TYPE_CODE == model.MAIN_TYPE_CODE).Count();
                    if (model.MAIN_TYPE_CODE == "MainTop")
                    {
                        if (prevListCount >= 20)
                        {
                            throw new Exception("20개 이상 활성상태를 등록하실수 없습니다.");
                        }
                    }
                    else
                    {
                        if (prevListCount >= 3)
                        {
                            throw new Exception("3개 이상 활성상태를 등록하실수 없습니다.");
                        }
                    }
                }

                if (model.PUBLISH_START != null)
                {
                    model.PUBLISH_START = new DateTime(model.PUBLISH_START.Value.Year, model.PUBLISH_START.Value.Month, model.PUBLISH_START.Value.Day, model.PublishStartHour, model.PublishStartMinute, 0);
                }
                if (model.PUBLISH_END != null)
                {
                    model.PUBLISH_END = new DateTime(model.PUBLISH_END.Value.Year, model.PUBLISH_END.Value.Month, model.PUBLISH_END.Value.Day, model.PublishEndHour, model.PublishEndMinute, 0);
                }

                int order = 0;
                var maxOrder = db49_wowtv.NTB_MAIN_MANAGE.Where(a => a.DEL_YN == "N" && a.MAIN_TYPE_CODE == model.MAIN_TYPE_CODE).OrderByDescending(a => a.SORT_ORDER).FirstOrDefault();
                if(maxOrder != null)
                {
                    order = maxOrder.SORT_ORDER;
                }
                order++;
                model.SORT_ORDER = order;

                model.DEL_YN = "N";
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                model.MOD_ID = loginUser.LoginId;
                model.MOD_DATE = DateTime.Now;

                db49_wowtv.NTB_MAIN_MANAGE.Add(model);
            }
            else
            {
                var prev = GetAt(model.MAIN_MANAGE_SEQ);

                if (prev.ACTIVE_YN == "N" && model.ACTIVE_YN == "Y")
                {
                    var prevListCount = db49_wowtv.NTB_MAIN_MANAGE.Where(a => a.DEL_YN == "N" && a.ACTIVE_YN == "Y" && a.MAIN_TYPE_CODE == model.MAIN_TYPE_CODE).Count();
                    if (model.MAIN_TYPE_CODE == "MainTop")
                    {
                        if (prevListCount >= 20)
                        {
                            throw new Exception("20개 이상 활성상태를 등록하실수 없습니다.");
                        }
                    }
                    else
                    {
                        if (prevListCount >= 3)
                        {
                            throw new Exception("3개 이상 활성상태를 등록하실수 없습니다.");
                        }
                    }
                }


                if (model.PUBLISH_START != null)
                {
                    model.PUBLISH_START = new DateTime(model.PUBLISH_START.Value.Year, model.PUBLISH_START.Value.Month, model.PUBLISH_START.Value.Day, model.PublishStartHour, model.PublishStartMinute, 0);
                }
                if (model.PUBLISH_END != null)
                {
                    model.PUBLISH_END = new DateTime(model.PUBLISH_END.Value.Year, model.PUBLISH_END.Value.Month, model.PUBLISH_END.Value.Day, model.PublishEndHour, model.PublishEndMinute, 0);
                }

                prev.MAIN_TYPE_CODE = model.MAIN_TYPE_CODE;
                prev.TITLE = model.TITLE;
                prev.TYPE_CODE = model.TYPE_CODE;
                prev.TEXT_INFO1 = model.TEXT_INFO1;
                prev.TEXT_INFO2 = model.TEXT_INFO2;
                prev.LINK_URL = model.LINK_URL;
                prev.LINK_TYPE = model.LINK_TYPE;
                prev.PUBLISH_START = model.PUBLISH_START;
                prev.PUBLISH_END = model.PUBLISH_END;
                prev.ACTIVE_YN = model.ACTIVE_YN;

                prev.MOD_ID = loginUser.LoginId;
                prev.MOD_DATE = DateTime.Now;
            }

            db49_wowtv.SaveChanges();
            
            if (model.AttachFile != null)
            {
                model.AttachFile.TABLE_CODE = "NTB_MAIN_MANAGE";
                model.AttachFile.TABLE_KEY = model.MAIN_MANAGE_SEQ.ToString();

                new AttachFile.AttachFileBiz().Create(model.AttachFile);
            }

            return model.MAIN_MANAGE_SEQ;
        }



        public void Delete(int mainManageSeq)
        {
            var prev = GetAt(mainManageSeq);
            prev.DEL_YN = "Y";
            db49_wowtv.SaveChanges();
        }


        
        public void UpDown(int mainManageSeq, bool isUp, LoginUser loginUser)
        {
            NTB_MAIN_MANAGE upDown = null;

            var prev = GetAt(mainManageSeq);

            if (isUp == true)
            {
                // 바로위 조회
                upDown = db49_wowtv.NTB_MAIN_MANAGE.Where(a => a.DEL_YN == "N" && a.MAIN_TYPE_CODE == prev.MAIN_TYPE_CODE && a.SORT_ORDER < prev.SORT_ORDER).OrderByDescending(a => a.SORT_ORDER).FirstOrDefault();
            }
            else
            {
                // 바로아래 조회
                upDown = db49_wowtv.NTB_MAIN_MANAGE.Where(a => a.DEL_YN == "N" && a.MAIN_TYPE_CODE == prev.MAIN_TYPE_CODE && a.SORT_ORDER > prev.SORT_ORDER).OrderBy(a => a.SORT_ORDER).FirstOrDefault();
            }

            if (upDown != null)
            {
                int upDownOrder = upDown.SORT_ORDER;
                upDown.SORT_ORDER = prev.SORT_ORDER;
                prev.SORT_ORDER = upDownOrder;


                upDown.MOD_ID = loginUser.LoginId;
                upDown.MOD_DATE = DateTime.Now;
                prev.MOD_ID = loginUser.LoginId;
                prev.MOD_DATE = DateTime.Now;

                db49_wowtv.SaveChanges();
            }
        }



        public ListModel<NTB_MAIN_MANAGE> GetFrontList(string mainTypeCode)
        {
            //Wow.Fx.WowLog.Write("1");

            DateTime nowDate = DateTime.Now;
            ListModel<NTB_MAIN_MANAGE> model = new ListModel<NTB_MAIN_MANAGE>();

            try
            {
                var list = db49_wowtv.NTB_MAIN_MANAGE.Where(a => a.DEL_YN == "N" && a.ACTIVE_YN == "Y");
                list = list.Where(a => a.MAIN_TYPE_CODE == mainTypeCode);
                list = list.Where(a => (a.PUBLISH_START == null && a.PUBLISH_END == null)
                                       || (a.PUBLISH_START == null && a.PUBLISH_END >= nowDate)
                                       || (a.PUBLISH_START <= nowDate && a.PUBLISH_END == null)
                                       || (a.PUBLISH_START <= nowDate && a.PUBLISH_END >= nowDate));

                list = list.OrderBy(a => a.SORT_ORDER);

                //Wow.Fx.WowLog.Write("2");

                if (mainTypeCode == "MainTop")
                {
                    list = list.Take(20);
                    model.ListData = list.ToList();
                    if (model.ListData.Count % 2 > 0)
                    {
                        model.ListData = model.ListData.Take(model.ListData.Count - 1).ToList();
                    }
                }
                else
                {
                    list = list.Take(3);
                    model.ListData = list.ToList();
                }


                //Wow.Fx.WowLog.Write("3");

                foreach (var item in model.ListData)
                {
                    //Wow.Fx.WowLog.Write("4");

                    var file = new AttachFile.AttachFileBiz().GetAt("NTB_MAIN_MANAGE", item.MAIN_MANAGE_SEQ.ToString());

                    //Wow.Fx.WowLog.Write("5");

                    if (file == null)
                    {
                        item.AttachFile = new NTB_ATTACH_FILE();
                        //Wow.Fx.WowLog.Write("6");
                    }
                    else
                    {
                        item.AttachFile = file;
                        //Wow.Fx.WowLog.Write("7");
                    }
                }
            }
            catch(Exception ex)
            {
                //Wow.Fx.WowLog.Write(ex.Message);

                if (ex.InnerException != null)
                {
                    //Wow.Fx.WowLog.Write("Inner : " + ex.Message);
                }
            }

            //Wow.Fx.WowLog.Write("8");

            return model;
        }


    }
}
