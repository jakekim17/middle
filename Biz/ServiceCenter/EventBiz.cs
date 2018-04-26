using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.wownet.ServiceCenter;

namespace Wow.Tv.Middle.Biz.ServiceCenter
{
    public class EventBiz : BaseBiz
    {
        //static DateTime today = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        static DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        //static DateTime today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

        //관리자 이벤트 리스트
        public EventModel<EventContent> GetList(EventCondition condition)
        {
            var resultData = new EventModel<EventContent>();
            var CodeJoinList = EventData();
            

            if (!String.IsNullOrEmpty(condition.CodeType))
            {
                CodeJoinList = CodeJoinList.Where(a => a.ViewSite.Equals(condition.CodeType));
            }

            if (!String.IsNullOrEmpty(condition.EventType))
            {
                if (condition.EventType.Equals("0"))
                {//진행전
                    CodeJoinList = CodeJoinList.Where(a => a.StartDate > today).ToList();
                }
                if (condition.EventType.Equals("1"))
                {//진행중
                    CodeJoinList = CodeJoinList.Where(a => a.StartDate <= today && a.EndDate >= today).ToList();
                }
                if (condition.EventType.Equals("2"))
                {//마감
                    CodeJoinList = CodeJoinList.Where(a => a.EndDate < today && a.WinViewFlag == null).ToList();
                }
                if (condition.EventType.Equals("3"))
                {//당첨자발표
                    CodeJoinList = CodeJoinList.Where(a => a.WinViewFlag != null 
                                                        && (a.WinViewFlag.Equals("Y") || a.WinViewFlag.Equals("N"))
                                                        && a.EndDate < today).ToList();
                }

            }

            //검색어 있는 경우
            if (!String.IsNullOrEmpty(condition.SearchText))
            {
                if (condition.SearchType.Equals("title"))
                {
                    CodeJoinList = CodeJoinList.Where(a => a.Title.Contains(condition.SearchText));
                }
                else if (condition.SearchType.Equals("partner"))
                {
                    CodeJoinList = CodeJoinList.Where(a => a.NickName.Contains(condition.SearchText));
                }
            }

            resultData.TotalDataCount = CodeJoinList.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                CodeJoinList = CodeJoinList.OrderByDescending(a => a.Seq).Skip(condition.CurrentIndex).Take(condition.PageSize);
            }

            resultData.EventList = CodeJoinList.ToList();
            resultData.CodeList = db49_wowtv.NTB_COMMON_CODE.Where(a => a.UP_COMMON_CODE.Equals(CommonCodeStatic.VIEW_SITE_CODE)).ToList();//사이트구분
            return resultData;
        }

        //관리자 이벤트 상세
        public EventModel<EventContent> GetData(int seq)
        {
            var resultData = new EventModel<EventContent>();
            var data = db49_wownet.tblEvents.SingleOrDefault(a => a.seq.Equals(seq));

            var ProImage = ""; //파트너이미지
            if (seq != 0 && data.proId != null && !data.proId.Equals("0000"))//파트너이벤트이면서
            {
                if(String.IsNullOrEmpty(data.bannerImage))//등록한 이미지가 없을때 
                {
                    ProImage = db49_broadcast.USP_GetBroadcast1ByProId(data.proId).Select(a => a.NEWPHOTO_SMALL2).FirstOrDefault();
                }
            }


            resultData.EventData = new tblEvent();
            resultData.CodeList = db49_wowtv.NTB_COMMON_CODE.Where(a => a.UP_COMMON_CODE.Equals(CommonCodeStatic.VIEW_SITE_CODE)).ToList();//사이트구분
            resultData.EventData = db49_wownet.tblEvents.SingleOrDefault(a => a.seq.Equals(seq));
            resultData.ProImage = ProImage;

            return resultData;
        }

        //파트너
        public ListModel<Pro_wowList> GetBroadList()
        {
            var resultData = new ListModel<Pro_wowList>();
            var list = db49_broadcast.Pro_wowList.AsQueryable();//항목리스트

            list = list.Where(a => a.State.Equals("1") &&
                                !a.Pro_id.Equals("") &&
                                !a.Pro_id.Equals("P000") &&
                                (a.Pro_id.Contains("P") || a.Pro_id.Contains("F") || a.Pro_id.Equals("HI626")));

            resultData.ListData = list.ToList();

            return resultData;
        }

        //관리자 파트너 저장
        public int Save(tblEvent model, LoginUser loginUser)
        {
            var data = GetData(model.seq).EventData;

            if (String.IsNullOrEmpty(model.WIN_CONTENT))
            {
                model.WIN_VIEW_FLAG = null;
                model.WIN_CONTENT = null;
            }

            

            if (data != null)//데이터 존재한다면 업데이트 
            {
                data.VIEW_SITE = model.VIEW_SITE;       //사이트구분
                data.proId = model.proId;               //이벤트구분
                data.title = model.title;               //제목
                data.viewFlag = model.viewFlag;         //게시여부
                data.content = model.content;           //내용
                data.linkUrl = model.linkUrl;
                data.startDate = model.startDate;       //시작일
                data.endDate = model.endDate;           //종료일  
                data.WINNER_DATE = model.WINNER_DATE;
                data.WIN_VIEW_FLAG = model.WIN_VIEW_FLAG;
                data.WIN_CONTENT = model.WIN_CONTENT;
                data.bannerImage = model.bannerImage;


            }
            else
            {//인서트
                model.seq = db49_wownet.tblEvents.OrderByDescending(a => a.seq).First().seq + 1;
                model.adminId = loginUser.LoginId;
                model.regdate = DateTime.Now;
                model.linkTarget = "B";
                model.readCount = 0;
                model.DEL_YN = "N";
                db49_wownet.tblEvents.Add(model);

            }
            db49_wownet.SaveChanges();

            return model.seq;
        }

        //관리자 파트너 삭제 
        public void Delete(int[] deleteList)
        {
            foreach (var index in deleteList)
            {
                var data = db49_wownet.tblEvents.SingleOrDefault((a => a.seq.Equals(index)));

                if (data != null)
                {
                    data.DEL_YN = "Y";
                    db49_wownet.SaveChanges();
                }

            }
        }

        //이벤트 프론트 리스트
        public ListModel<EventContent> GetFrontList(EventCondition condition)
        {
            var resultData = new ListModel<EventContent>();
            var CodeJoinList = EventData().Where(a => a.ViewFlag.Equals("Y"));

            if (condition.EventType.Equals("1"))
            {//진행중이벤트
                //string tmpstr = today.ToString("yyyy-MM-dd HH-mm-ss");
                CodeJoinList = CodeJoinList.Where(a => a.StartDate <= today && a.EndDate >= today);
            }
            if (condition.EventType.Equals("2"))
            {//지난이벤트
                CodeJoinList = CodeJoinList.Where(a => a.EndDate < today && a.WinViewFlag == null).ToList();
            }
            if (condition.EventType.Equals("3"))
            {//당첨자발표
                CodeJoinList = CodeJoinList.Where(a => a.WinViewFlag != null
                                                    && (a.WinViewFlag.Equals("Y") || a.WinViewFlag.Equals("N"))
                                                    && a.EndDate < today).ToList();
                //CodeJoinList = CodeJoinList.Where(a => a.WinViewFlag != null &&  a.WinViewFlag.Equals("Y") && a.WinnerDate < DateTime.Now).ToList();
            }

            if (!String.IsNullOrEmpty(condition.SearchText))
            {
                CodeJoinList = CodeJoinList.Where(a => a.Title.Contains(condition.SearchText) || a.NickName.Contains(condition.SearchText));
            }

            resultData.TotalDataCount = CodeJoinList.Count();

            if (condition.PageSize > -1)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 12;
                }
                //최신순, 마감임박순
                if (!String.IsNullOrEmpty(condition.SearchType))
                {
                    //최신순
                    if (condition.SearchType.Equals("recent"))
                    {
                        CodeJoinList = CodeJoinList.OrderByDescending(a => a.RegDate);
                    }

                    //마감임박순
                    if (condition.SearchType.Equals("deadline"))
                    {
                        CodeJoinList = CodeJoinList.OrderBy(a => a.EndDate);
                    }
                }
                else
                {
                    CodeJoinList = CodeJoinList.OrderByDescending(a => a.Seq);
                }
                CodeJoinList = CodeJoinList.Skip(condition.CurrentIndex).Take(condition.PageSize);
            }
            else
            {
                //최신순, 마감임박순
                if (!String.IsNullOrEmpty(condition.SearchType))
                {
                    //최신순
                    if (condition.SearchType.Equals("recent"))
                    {
                        CodeJoinList = CodeJoinList.OrderByDescending(a => a.RegDate);
                    }

                    //마감임박순
                    if (condition.SearchType.Equals("deadline"))
                    {
                        CodeJoinList = CodeJoinList.OrderBy(a => a.EndDate);
                    }
                }
                else
                {
                    CodeJoinList = CodeJoinList.OrderByDescending(a => a.Seq);
                }
            }
            
            resultData.ListData = CodeJoinList.ToList();
            return resultData;
        }

        //이벤트 프론트 상세
        public EventModel<EventContent> GetFrontDetail(EventCondition condition) {
            var resultData = new EventModel<EventContent>();
            var CodeJoinData = EventData().Where(a => a.Seq.Equals(condition.Seq));

            if (condition.EventType.Equals("1"))
            {//진행중
                CodeJoinData = CodeJoinData.Where(a => a.StartDate <= today && a.EndDate >= today);
            }
            if (condition.EventType.Equals("2"))
            {//지난
                CodeJoinData = CodeJoinData.Where(a => a.EndDate < today && a.WinViewFlag == null);
            }
            if (condition.EventType.Equals("3"))
            {//당첨자
                CodeJoinData = CodeJoinData.Where(a => a.WinViewFlag != null
                                                    && (a.WinViewFlag.Equals("Y") || a.WinViewFlag.Equals("N"))
                                                    && a.EndDate < today);
            }

            var proId = CodeJoinData.Select(a => a.ProId).FirstOrDefault();
            var broadData = (from a in db49_broadcast.USP_GetBroadcast1ByProId(proId)
                             select new BroadModel() {
                                 NickName = a.NICKNAME,
                                 ProId = a.PRO_ID,
                                 BPlayType = a.BPLAYTYPE,
                                 BMemType = a.BMEMTYPE,
                                 State = a.STATE,
                                 BRoom = a.BROOM,
                                 PriceId = a.PRODUCT_ID_1,
                                 Photo = a.NEWPHOTO_SMALL2
                             }).SingleOrDefault();

            var wowtvId = CodeJoinData.Select(a => a.WowtvId).FirstOrDefault().ToString();
            var cafeInfo = db49_wownet.usp_Select_MasterOpenCafeInfo(wowtvId).SingleOrDefault();
            if(cafeInfo != null)
            {
                broadData.CafeDomain = cafeInfo.CafeDomain;
            }
            else
            {
                broadData.CafeDomain = "";
            }
            
            resultData.FrontData = CodeJoinData.SingleOrDefault();
            resultData.BroadData = broadData == null ? new BroadModel() : broadData;

            return resultData;
        }

        //조회수
        public void ReadCountIncrease(EventCondition condition)
        {
            EventModel<EventContent> data = GetFrontDetail(condition);
            var tblData = db49_wownet.tblEvents.SingleOrDefault(a => a.seq.Equals(data.FrontData.Seq));

            if (tblData != null)
            {
                tblData.readCount = tblData.readCount + 1;
            }
            db49_wownet.SaveChanges();
        }
        
        //이벤트 조인
        public IEnumerable<EventContent> EventData()
        {
            var list = db49_wownet.tblEvents.Where(a => a.DEL_YN.Equals("N") || a.DEL_YN.Equals(null)).AsQueryable().ToList();

            var joinList = (from a in list
                            join b in db49_broadcast.Pro_wowList.Where(a => a.State == "1")
                            on a.proId equals b.Pro_id into eventGroup
                            from ab in eventGroup.DefaultIfEmpty()
                            select new EventContent
                            {
                                Seq = a.seq,
                                ViewSite = a.VIEW_SITE == null ? "" : a.VIEW_SITE,
                                Title = a.title,
                                NickName = ab == null ? "" : ab.NickName,
                                StartDate = a.startDate,
                                EndDate = a.endDate,
                                RegDate = a.regdate,
                                ReadCount = a.readCount,
                                ViewFlag = a.viewFlag,
                                WinViewFlag = a.WIN_VIEW_FLAG,
                                WinnerDate = a.WINNER_DATE,
                                WinContent = a.WIN_CONTENT,
                                LinkUrl = a.linkUrl,
                                Content = a.content,
                                ProId = a.proId,
                                ProductId = ab == null ? "" : ab.Product_ID_1,
                                WowtvId = ab == null ? "" : ab.Wowtv_id,
                                NEWphoto_small2 = ab == null ? "" : ab.NEWphoto_small2,
                                BannerImage = a.bannerImage == null ? "" : a.bannerImage
                            }).ToList();

            //if(joinList != null)
            //{
            //    foreach(var item in joinList)
            //    {
            //        if(String.IsNullOrEmpty(item.ViewSite))
            //        {
            //            joinList[joinList.IndexOf(item)].ViewSite = "N";
            //        }
            //    }
            //}


            var CodeJoinList = (from a in joinList
                                join b in db49_wowtv.NTB_COMMON_CODE
                                on a.ViewSite equals b.CODE_VALUE1 into jointable
                                from ab in jointable.DefaultIfEmpty()
                                select new EventContent
                                {
                                    Seq = a.Seq,
                                    ViewSite = a.ViewSite,
                                    EventFlag = a.EventFlag,
                                    Title = a.Title,
                                    NickName = a.NickName,
                                    StartDate = a.StartDate,
                                    EndDate = a.EndDate,
                                    RegDate = a.RegDate,
                                    ReadCount = a.ReadCount,
                                    ViewFlag = a.ViewFlag,
                                    BannerImage = a.BannerImage,
                                    NEWphoto_small2 = a.NEWphoto_small2,
                                    LinkUrl = a.LinkUrl,
                                    WinViewFlag = ab == null ? "" : a.WinViewFlag,
                                    WinnerDate = a.WinnerDate,
                                    WinContent = a.WinContent,
                                    CodeName = ab == null ? "-" : ab.CODE_NAME,
                                    Content = a.Content,
                                    ProId = a.ProId,
                                    WowtvId = a.WowtvId,
                                    ProductId = a.ProductId,
                                    UpCommonCode = ab == null ? CommonCodeStatic.VIEW_SITE_CODE : ab.UP_COMMON_CODE
                                }).Where(a => a.UpCommonCode != null && a.UpCommonCode.Equals(CommonCodeStatic.VIEW_SITE_CODE)).ToList();

            return CodeJoinList;
        }
        
        //이벤트 (메인) 조인
        public IEnumerable<EventContent> EventFrontData()
        {
            var list = db49_wownet.tblEvents.Where(a => a.DEL_YN.Equals("N") || a.DEL_YN.Equals(null)).AsQueryable().ToList();

            var joinList = (from a in list
                            join b in db49_broadcast.Pro_wowList
                            on a.proId equals b.Pro_id into eventGroup
                            from ab in eventGroup.DefaultIfEmpty()
                            select new EventContent
                            {
                                Seq = a.seq,
                                ViewSite = a.VIEW_SITE == null ? "" : a.VIEW_SITE,
                                Title = a.title,
                                StartDate = a.startDate,
                                EndDate = a.endDate,
                                RegDate = a.regdate,
                                ReadCount = a.readCount,
                                ViewFlag = a.viewFlag,
                                WinViewFlag = a.WIN_VIEW_FLAG,
                                WinnerDate = a.WINNER_DATE,
                                LinkUrl = a.linkUrl,
                                ProId = a.proId,
                                ProductId = ab == null ? "" : ab.Product_ID_1,
                                WowtvId = ab == null ? "" : ab.Wowtv_id,
                            }).ToList();
            

            return joinList;
        }

        //고객센터 메인 이벤트 리스트
        public ListModel<EventContent> GetMainEventList()
        {
            var resultData = new ListModel<EventContent>();
            var CodeJoinList = EventFrontData().Where(a => a.ViewFlag.Equals("Y"));

            CodeJoinList = CodeJoinList.Where(a => (a.StartDate <= today && a.EndDate >= today) && a.WinViewFlag == null);
            CodeJoinList = CodeJoinList.OrderByDescending(a => a.RegDate);
            CodeJoinList = CodeJoinList.Take(2);

            resultData.ListData = CodeJoinList.ToList();

            return resultData;
        }

        //프론트 퀵메뉴
        public ListModel<EventContent> GetQuickEventList()
        {
            var resultData = new ListModel<EventContent>();
            var CodeJoinList = EventData().Where(a => a.ViewFlag.Equals("Y"));

            CodeJoinList = CodeJoinList.Where(a => a.WinViewFlag == null);
            CodeJoinList = CodeJoinList.OrderByDescending(a => a.RegDate);
            CodeJoinList = CodeJoinList.Take(2);

            resultData.ListData = CodeJoinList.ToList();
            return resultData;
        }

        // GetQuickEventList --> 프로시저로 전환
        //public EventModel<EventContent> GetEventData()
        public List<usp_tblEvent_select_Result> GetEventData()
        {
            List<usp_tblEvent_select_Result> resultData = new List<usp_tblEvent_select_Result>();

            resultData = db49_wownet.usp_tblEvent_select().ToList();
            return resultData;
        }

        public ListModel<Pro_wowList> GetPartenrRandom()
        {
            var resultData = new ListModel<Pro_wowList>();
            var list = db49_broadcast.Pro_wowList.AsQueryable();//항목리스트

            list = list.Where(a => a.State.Equals("1") &&
                                !a.Pro_id.Equals("") &&
                                !a.Pro_id.Equals("P000") &&
                                (a.Pro_id.Contains("P") || a.Pro_id.Contains("F") || a.Pro_id.Equals("HI626")));

            list = list.OrderBy(x => Guid.NewGuid()).Take(4);
            resultData.ListData = list.ToList();

            return resultData;
        }


/*
         public List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result> GetQuickMenuMypinReporter(string userID, int? topN)
        {
            List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result> resultData = new List<NUP_QUICKMENU_MYPIN_REPORTER_SELECT_Result>();

            resultData = db49_Article.NUP_QUICKMENU_MYPIN_REPORTER_SELECT(userID, topN).ToList();

            return resultData;
 
 */
        //public String GetPartnerCafeDomain()
        //{
        //    var list = db49_broadcast.Pro_wowList.AsQueryable();//항목리스트
        //    var cafeMasterInfo = db49_wownet.usp_Select_MasterOpenCafeInfo(partnerinfo?.Wowtv_id).SingleOrDefault();
        //}
    }
}
