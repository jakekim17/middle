using System;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.StockholderBoard;

namespace Wow.Tv.Middle.Biz.PostManage
{
    public class StockholderBoardBiz : BaseBiz
    {
        public ListModel<StockholderBoard> GetList(StockholderBoardCondition condition)
        {
            var resultData = new ListModel<StockholderBoard>();
            var list = db49_wowtv.NTB_STOCKHOLDER_BOARD.Where(a => a.UP_STOCKHOLDER_SEQ == 0)
                    .Select(a => new StockholderBoard
                    {
                        STOCKHOLDER_SEQ = a.STOCKHOLDER_SEQ,
                        UP_STOCKHOLDER_SEQ = a.UP_STOCKHOLDER_SEQ,
                        TITLE = a.TITLE,
                        CONTENTS = a.CONTENTS,
                        READ_CNT = a.READ_CNT,
                        BLIND_YN = a.BLIND_YN,
                        EMAIL = a.EMAIL,
                        REG_ID = a.REG_ID,
                        REG_DATE = a.REG_DATE,
                        MOD_ID = a.MOD_ID,
                        MOD_DATE = a.MOD_DATE,
                        USER_NAME = a.USER_NAME
                    });

            if (!String.IsNullOrEmpty(condition.BlindYn))
            {
                list = list.Where(a => a.BLIND_YN.Equals(condition.BlindYn));
            }

            if (!String.IsNullOrEmpty(condition.SearchText))
            {
                switch (condition.SearchType)
                {
                    case "ALL":
                        list = list.Where(a => a.TITLE.Contains(condition.SearchText) || a.USER_NAME.Contains(condition.SearchText));
                        break;
                    case "TITLE":
                        list = list.Where(a => a.TITLE.Contains(condition.SearchText));
                        break;
                    case "USER_NAME":
                        list = list.Where(a => a.USER_NAME.Contains(condition.SearchText));
                        break;
                }

            }
            resultData.TotalDataCount = list.Count();

            if(condition.PageSize > -1)
            {
                if(condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                list = list.OrderByDescending(a => a.STOCKHOLDER_SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            resultData.ListData = list.ToList();
            var replyList = db49_wowtv.NTB_STOCKHOLDER_BOARD.Where(a => a.UP_STOCKHOLDER_SEQ > 0).ToList();

            foreach (var item in resultData.ListData)
            {
                var index = resultData.ListData.IndexOf(item);
                //var reply = replyList.Where(a => a.UP_STOCKHOLDER_SEQ.Equals(item.STOCKHOLDER_SEQ)).ToList();
                //if (reply != null)
                //{

                //    resultData.ListData[index].ReplyList = reply.ToList();
                //}

                var replyData = replyList.SingleOrDefault(a => a.UP_STOCKHOLDER_SEQ.Equals(item.STOCKHOLDER_SEQ));
                resultData.ListData[index].ReplyData = replyData;
            }

            //resultData.ListData = list.ToList();
            return resultData;
        }

        public StockholderBoard GetDetail(int seq)
        {
            var data = GetData(seq);
            var resultData = new StockholderBoard();
            if(data != null)
            {
                resultData.STOCKHOLDER_SEQ = data.STOCKHOLDER_SEQ;
                resultData.UP_STOCKHOLDER_SEQ = data.UP_STOCKHOLDER_SEQ;
                resultData.TITLE = data.TITLE;
                resultData.CONTENTS = data.CONTENTS;
                resultData.READ_CNT = data.READ_CNT;
                resultData.BLIND_YN = data.BLIND_YN;
                resultData.EMAIL = data.EMAIL;
                resultData.REG_ID = data.REG_ID;
                resultData.REG_DATE = data.REG_DATE;
                resultData.MOD_ID = data.MOD_ID;
                resultData.MOD_DATE = data.MOD_DATE;
                resultData.USER_NAME = data.USER_NAME;

                var replyList = db49_wowtv.NTB_STOCKHOLDER_BOARD.Where(a => a.UP_STOCKHOLDER_SEQ > 0).ToList();

                //var reply = replyList.Where(a => a.UP_STOCKHOLDER_SEQ.Equals(data.STOCKHOLDER_SEQ)).ToList();
                //resultData.ReplyList = reply;

                var replyData = replyList.SingleOrDefault(a => a.UP_STOCKHOLDER_SEQ.Equals(data.STOCKHOLDER_SEQ));
                resultData.ReplyData = replyData;
            }
            return resultData;
        }

        //public int Save(StockholderBoard model, LoginUser loginUser)
        //{
        //    var data = GetData(model.STOCKHOLDER_SEQ);
        //    if(data != null)
        //    {
        //        data.MOD_DATE = DateTime.Now;
        //        data.MOD_ID = loginUser.LoginId;
        //        data.TITLE = model.TITLE;
        //        data.CONTENTS = model.CONTENTS;
        //        data.EMAIL = model.EMAIL;

        //        if(model.ReplyData != null)
        //        {
        //            var subData = GetSubData(model.STOCKHOLDER_SEQ);
        //            if(subData != null)
        //            {
        //                subData.MOD_DATE = DateTime.Now;
        //                subData.MOD_ID = loginUser.LoginId;
        //                subData.TITLE = model.TITLE;
        //                subData.CONTENTS = model.CONTENTS;
        //                subData.EMAIL = model.EMAIL;
        //            }
        //            else
        //            {
        //                var subStock = new NTB_SOTCKHOLDER_BOARD();
        //                subStock.UP_STOCKHOLDER_SEQ = data.UP_STOCKHOLDER_SEQ;
        //                subStock.TITLE = model.ReplyData.TITLE;
        //                subStock.CONTENTS = model.ReplyData.CONTENTS;
        //                subStock.READ_CNT = model.ReplyData.READ_CNT;
        //                subStock.BLIND_YN = model.ReplyData.BLIND_YN;
        //                subStock.EMAIL = model.ReplyData.EMAIL;
        //                subStock.REG_ID = loginUser.LoginId;
        //                subStock.REG_DATE = DateTime.Now;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var nStock = new NTB_SOTCKHOLDER_BOARD();
        //        nStock.TITLE = model.TITLE;
        //        nStock.CONTENTS = model.CONTENTS;
        //        nStock.READ_CNT = model.READ_CNT;
        //        nStock.BLIND_YN = model.BLIND_YN;
        //        nStock.EMAIL = model.EMAIL;
        //        nStock.REG_ID = loginUser.LoginId;
        //        nStock.REG_DATE = DateTime.Now;

        //        db49_wowtv.NTB_SOTCKHOLDER_BOARD.Add(nStock);
        //        db49_wowtv.SaveChanges();

        //        if(model.ReplyData != null)
        //        {
        //            var subStock = new NTB_SOTCKHOLDER_BOARD();
        //            subStock.UP_STOCKHOLDER_SEQ = nStock.UP_STOCKHOLDER_SEQ;
        //            subStock.TITLE = model.ReplyData.TITLE;
        //            subStock.CONTENTS = model.ReplyData.CONTENTS;
        //            subStock.READ_CNT = model.ReplyData.READ_CNT;
        //            subStock.BLIND_YN = model.ReplyData.BLIND_YN;
        //            subStock.EMAIL = model.ReplyData.EMAIL;
        //            subStock.REG_ID = loginUser.LoginId;
        //            subStock.REG_DATE = DateTime.Now;
        //        }
        //    }
        //    db49_wowtv.SaveChanges();

        //    return model.STOCKHOLDER_SEQ;
        //}

        public void BoardSave(NTB_STOCKHOLDER_BOARD model)
        {
            var data = GetData(model.STOCKHOLDER_SEQ);
            if (data != null)
            {
                data.MOD_DATE = DateTime.Now;
                data.MOD_ID = model.USER_NAME;
                data.TITLE = model.TITLE;
                data.CONTENTS = model.CONTENTS;
                data.EMAIL = model.EMAIL;
            }
            else
            {
                model.REG_ID = model.USER_NAME;
                model.REG_DATE = DateTime.Now;
                model.BLIND_YN = "N";
                db49_wowtv.NTB_STOCKHOLDER_BOARD.Add(model);
            }
            db49_wowtv.SaveChanges();
        }

        public int ReplySave(NTB_STOCKHOLDER_BOARD model, LoginUser loginUser)
        {
            var data = db49_wowtv.NTB_STOCKHOLDER_BOARD.Where(a => a.UP_STOCKHOLDER_SEQ > 0 && a.UP_STOCKHOLDER_SEQ.Equals(model.UP_STOCKHOLDER_SEQ)).ToList();
            if (data != null && data.Count() > 0)
            {
                foreach(var item in data)
                {
                    if(item.STOCKHOLDER_SEQ == model.STOCKHOLDER_SEQ)
                    {
                        item.MOD_DATE = DateTime.Now;
                        item.MOD_ID = loginUser.LoginId;
                        item.TITLE = model.TITLE;
                        item.CONTENTS = model.CONTENTS;
                        item.EMAIL = model.EMAIL;
                        item.VIEW_YN = model.VIEW_YN;
                    }
                }              
            }
            else
            {
                model.USER_NAME = "관리자";
                model.REG_ID = loginUser.LoginId;
                model.REG_DATE = DateTime.Now;
                db49_wowtv.NTB_STOCKHOLDER_BOARD.Add(model);
            }
            db49_wowtv.SaveChanges();

            return model.STOCKHOLDER_SEQ;
        }

        public void Delete(int[] deleteList)
        {
            if(deleteList != null)
            {
                foreach(var item in deleteList)
                {
                    var data = GetData(item);
                    if(data != null)
                    {
                        var subData = GetSubList(data.STOCKHOLDER_SEQ);
                        if(subData != null)
                        {
                            foreach(var subItme in subData)
                            {
                                db49_wowtv.NTB_STOCKHOLDER_BOARD.Remove(subItme);
                            }
                        }
                        db49_wowtv.NTB_STOCKHOLDER_BOARD.Remove(data);
                    }
                    db49_wowtv.SaveChanges();
                }
            }
        }

        public void UpdateBlind(int seq, string blindYn, LoginUser loginUser)
        {
            var data = GetData(seq);
            if(data != null)
            {
                data.BLIND_YN = blindYn;
                data.MOD_ID = loginUser.LoginId;
                data.MOD_DATE = DateTime.Now;
                db49_wowtv.SaveChanges();
            }
        }

        public void UpdateReadCnt(int seq)
        {
            var data = GetData(seq);
            if(data != null)
            {
                data.READ_CNT = data.READ_CNT + 1;
                db49_wowtv.SaveChanges();
            }
        }

        public NTB_STOCKHOLDER_BOARD GetData(int seq)
        {
            return db49_wowtv.NTB_STOCKHOLDER_BOARD.SingleOrDefault(a => a.STOCKHOLDER_SEQ.Equals(seq));
        }

        public IQueryable<NTB_STOCKHOLDER_BOARD> GetSubList(int seq)
        {
            return db49_wowtv.NTB_STOCKHOLDER_BOARD.Where(a => a.UP_STOCKHOLDER_SEQ.Equals(seq));
        }

        public NTB_STOCKHOLDER_BOARD GetSubData(int seq)
        {
            return db49_wowtv.NTB_STOCKHOLDER_BOARD.SingleOrDefault(a => a.UP_STOCKHOLDER_SEQ.Equals(seq));
        }

        public int GetMaxBoardNum()
        {
            return db49_wowtv.NTB_STOCKHOLDER_BOARD.Max(a => a.STOCKHOLDER_SEQ);
        }

        public int GetMinBoardNum()
        {
            return db49_wowtv.NTB_STOCKHOLDER_BOARD.Min(a => a.STOCKHOLDER_SEQ);
        }
    }
}
