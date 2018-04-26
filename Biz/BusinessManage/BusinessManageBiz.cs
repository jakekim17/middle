using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using Wow.Tv.Middle.Biz.Component;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db49.wowtv.BusinessManage;

namespace Wow.Tv.Middle.Biz.BusinessManage
{
    public class BusinessManageBiz :BaseBiz
    {
        readonly int BOARD_SEQ = 14;

        public ListModel<BOARD_CONT_MENU> SearchList(BusinessCondition condition)
        {
            var resultData = new ListModel<BOARD_CONT_MENU>();
            var list = GetJoinData();

            if (!String.IsNullOrEmpty(condition.displayYn))
            {
                list = list.Where(a => a.VIEW_YN.Equals(condition.displayYn)).ToList();
            }

            if (!String.IsNullOrEmpty(condition.searchText))
            {
                switch (condition.searchType)
                {
                    case "all":
                        list = list.Where(a => a.TITLE.Contains(condition.searchText) || a.REG_ID.Contains(condition.searchText)).ToList();
                        break;
                    case "regId":
                        list = list.Where(a => a.REG_ID.Contains(condition.searchText)).ToList();
                        break;
                    case "title":
                        list = list.Where(a => a.TITLE.Contains(condition.searchText)).ToList();
                        break;
                }
            }

            resultData.TotalDataCount = list.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                list = list.OrderByDescending(a => a.BOARD_CONTENT_SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }

            resultData.ListData = list.ToList();
            return resultData;

        }

        public BOARD_CONT_MENU GetDetail(int seq)
        {
            BOARD_CONT_MENU data = new BOARD_CONT_MENU();

            data = GetJoinData().FirstOrDefault(a => a.BOARD_CONTENT_SEQ.Equals(seq));

            if (data == null)
            {
                data = new BOARD_CONT_MENU();

                int businessCount = db49_wowtv.NTB_BOARD_CONTENT.Where(a=> a.CONTENT_ID.ToString() != null &&  a.CONTENT_ID > 0).Count();
                int menuConSeq = (int)(db49_wowtv.NTB_MENU.Max(a => a.CONTENT_SEQ) + 1);

                int busConSeq = (businessCount ==  0 ? 100000 : db49_wowtv.NTB_BOARD_CONTENT.Max(a => a.CONTENT_ID) + 1);

                if (menuConSeq >= busConSeq)
                {
                    data.CONTENT_ID = menuConSeq;
                }
                else
                {
                    data.CONTENT_ID = busConSeq;
                }
            }

            return data;
        }

        public int Save(NTB_BOARD_CONTENT model, LoginUser loginUser)
        {
            var data = GetData(model.BOARD_CONTENT_SEQ);
            if(data != null)
            {
                data.TITLE = model.TITLE;
                data.CONTENT = model.CONTENT;
                data.VIEW_YN = model.VIEW_YN;
                data.MOD_DATE = DateTime.Now;
                data.MOD_ID = loginUser.UserName;
                data.EMAIL = model.EMAIL;
            }
            else
            {
                model.BOARD_SEQ = BOARD_SEQ;
                model.REG_DATE = DateTime.Now;
                model.REG_ID = loginUser.UserName;
                model.DEL_YN = "N";
                db49_wowtv.NTB_BOARD_CONTENT.Add(model);
            }
            db49_wowtv.SaveChanges();

            return model.BOARD_CONTENT_SEQ;
        }

        public void Delete(int[] seqList)
        {
            foreach(var index in seqList)
            {
                var data = GetData(index);
                if (data != null)
                {
                    db49_wowtv.NTB_BOARD_CONTENT.Remove(data);
                    db49_wowtv.SaveChanges();
                }
            } 
        }
       
        public NTB_BOARD_CONTENT GetData(int seq)
        {
            return db49_wowtv.NTB_BOARD_CONTENT.SingleOrDefault(a => a.BOARD_CONTENT_SEQ.Equals(seq));
        }

        public List<BOARD_CONT_MENU> GetJoinData()
        {
            var resultData = new List<BOARD_CONT_MENU>();
            var contentList = db49_wowtv.NTB_BOARD_CONTENT.Where(a => a.BOARD_SEQ.Equals(BOARD_SEQ) && !a.DEL_YN.Equals("Y")).ToList();
            
            foreach(var item in contentList)
            {
                var joinData = new BOARD_CONT_MENU
                {
                    BOARD_CONTENT_SEQ = item.BOARD_CONTENT_SEQ,
                    BOARD_SEQ = item.BOARD_SEQ,
                    CONTENT = item.CONTENT,
                    CONTENT_ID = item.CONTENT_ID,
                    EMAIL = item.EMAIL,
                    MOD_ID = item.MOD_ID,
                    EMAIL_YN = item.EMAIL_YN,
                    READ_CNT = item.READ_CNT,
                    REG_DATE = item.REG_DATE,
                    TITLE = item.TITLE,
                    REG_ID = item.REG_ID,
                    MOD_DATE = item.MOD_DATE,
                    VIEW_YN = item.VIEW_YN
                };

                var menu = db49_wowtv.NTB_MENU.FirstOrDefault(a => a.CONTENT_SEQ == item.BOARD_CONTENT_SEQ);

                if (menu != null)
                {
                    joinData.MENU_NAME_DEPTH_1 = menu.MENU_NAME_DEPTH_1;
                    joinData.MENU_NAME_DEPTH_2 = menu.MENU_NAME_DEPTH_2;
                    joinData.MENU_NAME_DEPTH_3 = menu.MENU_NAME_DEPTH_3;
                }

                resultData.Add(joinData);
            }

            return resultData;
        }

        public NTB_BOARD_CONTENT SearchData(int menuSeq)
        {
            var result =  ( from b in db49_wowtv.NTB_BOARD_CONTENT
                            join m in db49_wowtv.NTB_MENU on b.BOARD_CONTENT_SEQ equals m.CONTENT_SEQ
                            where m.MENU_SEQ == menuSeq
                            select b).SingleOrDefault();
            if(result != null)
            {
                result.READ_CNT = result.READ_CNT + 1;
                db49_wowtv.SaveChanges();
            }

            return result;
        }

        public bool SendMobileSMS(string AppType ,string mobileNum)
        {
            string msg = "";
            //string msg = "한국경제TV 앱 다운로드 ";
            string callBackNumber = "02-6676-0000";
            string etc = "앱 다운로드 링크 " + AppType;

            bool isSend = false;

            if (!IsSMSSendCheck(mobileNum, etc))
            {
                if (AppType == "IOS")
                {
                    msg += "https://itunes.apple.com/kr/app/nyuhanguggyeongjetv/id705749014?mt=8";
                }
                else if (AppType == "ADR")
                {
                    msg += "https://play.google.com/store/apps/details?id=kr.co.wowtv.wowtv";
                }
                else if (AppType == "IOSNET")
                {
                    msg += "https://itunes.apple.com/kr/app/daebagne/id442902802?mt=8";
                }
                else if (AppType == "ADRNET")
                {
                    msg += "https://play.google.com/store/apps/details?id=co.kr.wownet.daebak";
                }
                else if (AppType == "IOSSTOCK")
                {
                    msg += "https://itunes.apple.com/kr/app/%EC%A3%BC%EC%8B%9D%EC%B0%BD-%EC%A3%BC%EC%8B%9D-%EC%A6%9D%EA%B6%8C1%EC%9C%84-%EC%95%B1-%EC%A6%9D%EA%B6%8C%EC%8B%9C%EC%84%B8-%EC%A3%BC%EC%8B%9D%EB%B9%84%ED%83%80%EB%AF%BC/id577658269?mt=8";
                }
                else if (AppType == "ADRSTOCK")
                {
                    msg += "https://play.google.com/store/apps/details?id=kr.co.wowtv.StockTalk";
                }
                else if (AppType == "IOSBAND")
                {
                    msg += "https://itunes.apple.com/kr/app/%EC%99%80%EC%9A%B0%EB%B0%B4%EB%93%9C/id1079686651?mt=8";
                }
                else if (AppType == "ADRBAND")
                {
                    msg += "https://play.google.com/store/apps/details?id=kr.co.futurewiz.android.wowband";
                }

                new SmsBiz().SendSms(mobileNum, callBackNumber, msg, "wowsms-admin", etc);

                isSend = true;
            }

            return isSend;
        }

        public bool IsSMSSendCheck(string mobileNum , string etc1)
        {
            bool isSMS = false;

            var tranResult = db51_ARSsms.SC_TRAN.Where(a => a.TR_PHONE.Equals(mobileNum) && a.TR_ETC1.Equals(etc1)).Count();
            var msgResult = Db51_WOWMMS.mms_msg.Where(a => a.PHONE.Equals(mobileNum) && a.ETC1.Equals(etc1)).Count();

            if(tranResult > 5 || msgResult > 5)
            {
                isSMS = true;
            }

            return isSMS;
        }
    }
}
