using System;
using System.Collections.Generic;
using System.Linq;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.broadcast;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.Lecture;

namespace Wow.Tv.Middle.Biz.ProductManage
{
    public class LectureBiz : BaseBiz
    {
        public ListModel<JOIN_LECTURES__CODE> GetList(LectureCondition condition)
        {
            var resultData = new ListModel<JOIN_LECTURES__CODE>();
            var joinlist = GetJoin();

            if (!String.IsNullOrEmpty(condition.ViewSite))
            {
                joinlist = joinlist.Where(a => a.VIEW_SITE.Equals(condition.ViewSite));
            }

            if (!String.IsNullOrEmpty(condition.MainCtgr))
            {
                joinlist = joinlist.Where(a => a.MAIN_CTGR.Equals(condition.MainCtgr));
            }

            if (!String.IsNullOrEmpty(condition.TypeFlag))
            {
                joinlist = joinlist.Where(a => a.TYPE_FLAG.Equals(condition.TypeFlag));
            }

            if (!String.IsNullOrEmpty(condition.SearchText))
            {
                switch (condition.SearchType)
                {
                    case "ALL":
                        joinlist = joinlist.Where(a => a.TITLE.Contains(condition.SearchText) || a.LECTURER.Contains(condition.SearchText)
                                        || a.PLACE.Contains(condition.SearchText));
                        break;
                    case "TITLE":
                        joinlist = joinlist.Where(a => a.TITLE.Contains(condition.SearchText));
                        break;
                    case "PLACE":
                        joinlist = joinlist.Where(a => a.PLACE.Contains(condition.SearchText));
                        break;
                    case "LECTURER":
                        joinlist = joinlist.Where(a => a.LECTURER.Contains(condition.SearchText));
                        break;
                }
            }

            if(condition.CurrentSite != "Admin")
            {
                if (!String.IsNullOrEmpty(condition.LecturesDate))
                {
                    if (condition.LecturesDate != "ALL")
                    {
                        joinlist = joinlist.Where(a => a.LECTURES_DATE.Equals(condition.LecturesDate));
                    }
                }
                else
                {
                    var today = DateTime.Today.ToString("yyyy-MM-dd");
                    joinlist = joinlist.Where(a => String.IsNullOrEmpty(a.LECTURES_DATE) == false
                                               && a.LECTURES_DATE.Length >= 8
                                               && a.LECTURES_DATE.CompareTo(today) >= 0);
                }
            }

            var toList = joinlist.ToList();
            var CodeJoinList = new List<JOIN_LECTURES__CODE>();

            if (condition.CurrentSite == "Admin")
            {
                CodeJoinList = toList.GroupBy(a => new { a.SEQ, a.VIEW_SITE, a.MAIN_CTGR, a.TYPE_FLAG, a.TITLE, a.REG_DATE, a.VIEW_FLAG, a.WG_IMAGE_FILE })
                                    .Select(a => new JOIN_LECTURES__CODE()
                                    {
                                        SEQ = a.Key.SEQ,
                                        VIEW_SITE = a.Key.VIEW_SITE,
                                        MAIN_CTGR = a.Key.MAIN_CTGR,
                                        TYPE_FLAG = a.Key.TYPE_FLAG,
                                        TITLE = a.Key.TITLE,
                                        REG_DATE = a.Key.REG_DATE,
                                        VIEW_FLAG = a.Key.VIEW_FLAG,
                                        WG_IMAGE_FILE = a.Key.WG_IMAGE_FILE
                                    }
                                ).ToList();
            }
            else
            {
                CodeJoinList = toList.Select(a => new JOIN_LECTURES__CODE()
                                                    {
                                                        SEQ = a.SEQ,
                                                        MSEQ = a.MSEQ,
                                                        VIEW_SITE = a.VIEW_SITE,
                                                        MAIN_CTGR = a.MAIN_CTGR,
                                                        TYPE_FLAG = a.TYPE_FLAG,
                                                        TITLE = a.TITLE,
                                                        REG_DATE = a.REG_DATE,
                                                        VIEW_FLAG = a.VIEW_FLAG,
                                                        LECTURES_DATE = a.LECTURES_DATE,
                                                        LECTURES_TIME = a.LECTURES_TIME,
                                                        PLACE = a.PLACE,
                                                        WG_IMAGE_FILE = a.WG_IMAGE_FILE,
                                                        THUMNAIL_FILE = a.THUMNAIL_FILE
                                                    }
                                            ).Where(a => a.VIEW_FLAG.Equals("Y")).ToList();
            }

            resultData.TotalDataCount = CodeJoinList.Count();

            if (condition.PageSize > 0)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                if (condition.CurrentSite == "Admin")
                {
                    CodeJoinList = CodeJoinList.OrderByDescending(a => a.SEQ).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();

                    foreach (var item in CodeJoinList)
                    {
                        var scheduleData = db49_wownet.TAB_LECTURES_SCHEDULE.Where(a => a.MSEQ == item.SEQ).OrderBy(a => a.SEQ).FirstOrDefault();

                        if (scheduleData != null)
                        {
                            CodeJoinList[CodeJoinList.IndexOf(item)].LECTURES_DATE = scheduleData.LECTURES_DATE;
                        }
                    }

                }
                else
                { //최신순 LECTURE_DATE
                    if(!String.IsNullOrEmpty(condition.LecturesDate) && condition.LecturesDate == "ALL")
                    {
                        CodeJoinList = CodeJoinList.OrderByDescending(a => DateTime.Parse(a.LECTURES_DATE)).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
                    }
                    else
                    {
                        CodeJoinList = CodeJoinList.OrderBy(a => DateTime.Parse(a.LECTURES_DATE)).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
                    }
                    
                }
            }

            resultData.ListData = CodeJoinList.ToList();
            return resultData;
        }

        public LectureScheduleDtl GetDetail(int seq)
        {
            var resultData = new LectureScheduleDtl
            {
                LectureData = GetLecData(seq)
            };

            var SchList = GetSchList(seq).OrderBy(a=> a.LECTURES_DATE).ToList();
            var dtlSchList = new List<DtlSchedule>();

            if (SchList != null)
            {
                for (int i = 0; i < SchList.Count; i++)
                {
                    var dtlSchedule = new DtlSchedule
                    {
                        SEQ = SchList[i].SEQ,
                        MSEQ = SchList[i].MSEQ,
                        PLACE = SchList[i].PLACE,
                        LECTURES_DATE = SchList[i].LECTURES_DATE,
                        LECTURES_TIME = SchList[i].LECTURES_TIME,
                        ETC = SchList[i].ETC
                    };

                    dtlSchedule.LecturerList = (from s in db49_wownet.NTB_LECTURES_LECTURER
                                                where s.MSEQ == dtlSchedule.SEQ
                                                select new JOIN_LECTURER_PARTNER()
                                                {
                                                    SEQ = s.SEQ,
                                                    MSEQ = s.MSEQ,
                                                    LECTURER = s.LECTURER,
                                                    LECTURER_TYPE = s.LECTURER_TYPE,
                                                    PARTNER_NO = s.PARTNER_NO
                                                }).ToList();
                    dtlSchList.Add(dtlSchedule);
                }
            }
            resultData.ScheduleList = dtlSchList;
            return resultData;
        }

        public int Save(LectureScheduleDtl data, LoginUser loginUser)
        {
            var LecData = GetLecData(data.LectureData.SEQ);
            if (LecData != null)
            {
                LecData.TYPE_FLAG = data.LectureData.TYPE_FLAG;
                LecData.VIEW_SITE = data.LectureData.VIEW_SITE;
                LecData.VIEW_FLAG = data.LectureData.VIEW_FLAG;
                LecData.TITLE = data.LectureData.TITLE;
                LecData.CONTENTS = data.LectureData.CONTENTS;
                LecData.EXPENSE = data.LectureData.EXPENSE;
                LecData.MANAGER = data.LectureData.MANAGER;
                LecData.PHONE = data.LectureData.PHONE;
                LecData.MAIN_CTGR = data.LectureData.MAIN_CTGR;
                LecData.METHOD = data.LectureData.METHOD;
                
                if (data.LectureData.WG_IMAGE_FILE != null)
                {
                    LecData.WG_IMAGE_FILE = data.LectureData.WG_IMAGE_FILE;
                }

                if (data.LectureData.THUMNAIL_FILE != null)
                {
                    LecData.THUMNAIL_FILE = data.LectureData.THUMNAIL_FILE;
                }

                LecData.MAIN_CTGR = data.LectureData.MAIN_CTGR;

                if (data.ScheduleList != null)
                {
                    foreach (var item in data.ScheduleList)
                    {
                        var Sdata = GetSchData(item.SEQ);
                        if (Sdata != null)
                        {
                            Sdata.PLACE = item.PLACE;
                            Sdata.LECTURES_DATE = item.LECTURES_DATE;
                            Sdata.LECTURES_TIME = item.LECTURES_TIME;
                            Sdata.ETC = item.ETC;

                            if (item.LecturerList != null)
                            {
                                foreach (var Litem in item.LecturerList)
                                {
                                    var LecturerData = GetNTBLecData(Litem.SEQ);
                                    if (LecturerData != null)
                                    {
                                        LecturerData.LECTURER = Litem.LECTURER;
                                        LecturerData.LECTURER_TYPE = Litem.LECTURER_TYPE;
                                        LecturerData.PARTNER_NO = Litem.PARTNER_NO;
                                    }
                                    else
                                    {
                                        var NTBLecData = new NTB_LECTURES_LECTURER
                                        {
                                            MSEQ = item.SEQ,
                                            LECTURER = Litem.LECTURER,
                                            LECTURER_TYPE = Litem.LECTURER_TYPE,
                                            PARTNER_NO = Litem.PARTNER_NO,
                                            REG_DATE = DateTime.Now
                                        };
                                        db49_wownet.NTB_LECTURES_LECTURER.Add(NTBLecData);
                                    }

                                }
                            }
                        }
                        else
                        {
                            var sTable = new TAB_LECTURES_SCHEDULE
                            {
                                REG_DATE = DateTime.Now,
                                PLACE = item.PLACE,
                                LECTURES_DATE = item.LECTURES_DATE,
                                LECTURES_TIME = item.LECTURES_TIME,
                                MSEQ = data.LectureData.SEQ
                            };
                            db49_wownet.TAB_LECTURES_SCHEDULE.Add(sTable);
                            db49_wownet.SaveChanges();

                            if (item.LecturerList != null)
                            {
                                foreach (var Litem in item.LecturerList)
                                {
                                    var NTBLecData = new NTB_LECTURES_LECTURER
                                    {
                                        MSEQ = sTable.SEQ,
                                        LECTURER = Litem.LECTURER,
                                        LECTURER_TYPE = Litem.LECTURER_TYPE,
                                        PARTNER_NO = Litem.PARTNER_NO,
                                        REG_DATE = DateTime.Now
                                    };
                                    db49_wownet.NTB_LECTURES_LECTURER.Add(NTBLecData);
                                }
                            }
                        }
                    }
                    if (data.SchDelList != null)
                    {
                        foreach (var item in data.SchDelList)
                        {
                            var Sdata = GetSchData(item);
                            if (Sdata != null)
                            {
                                var Ldata = GetNTBLecList(item);
                                if (Ldata != null)
                                {
                                    foreach (var Litem in Ldata)
                                    {
                                        db49_wownet.NTB_LECTURES_LECTURER.Remove(Litem);
                                    }
                                    db49_wownet.SaveChanges();
                                }
                                db49_wownet.TAB_LECTURES_SCHEDULE.Remove(Sdata);
                                db49_wownet.SaveChanges();
                            }
                        }
                    }

                    if (data.LecDelList != null)
                    {
                        foreach (var item in data.LecDelList)
                        {
                            var Sdata = GetNTBLecData(item);
                            if (Sdata != null)
                            {
                                db49_wownet.NTB_LECTURES_LECTURER.Remove(Sdata);
                            }
                        }
                    }
                }
            }
            else
            {
                data.LectureData.REG_DATE = DateTime.Now;
                data.LectureData.ADMIN_ID = loginUser.LoginId;
                data.LectureData.SUMMARY = "";
                db49_wownet.TAB_LECTURES.Add(data.LectureData);
                db49_wownet.SaveChanges();

                if (data.ScheduleList != null)
                {
                    foreach (var item in data.ScheduleList)
                    {
                        var sItem = new TAB_LECTURES_SCHEDULE
                        {
                            REG_DATE = DateTime.Now,
                            PLACE = item.PLACE,
                            LECTURES_DATE = item.LECTURES_DATE,
                            LECTURES_TIME = item.LECTURES_TIME,
                            MSEQ = data.LectureData.SEQ,
                            ETC = item.ETC
                        };
                        db49_wownet.TAB_LECTURES_SCHEDULE.Add(sItem);
                        db49_wownet.SaveChanges();

                        if (item.LecturerList != null)
                        {
                            foreach (var Litem in item.LecturerList)
                            {
                                var NTBLecData = new NTB_LECTURES_LECTURER
                                {
                                    MSEQ = sItem.SEQ,
                                    LECTURER = Litem.LECTURER,
                                    LECTURER_TYPE = Litem.LECTURER_TYPE,
                                    PARTNER_NO = Litem.PARTNER_NO,
                                    REG_DATE = DateTime.Now
                                };

                                db49_wownet.NTB_LECTURES_LECTURER.Add(NTBLecData);
                            }
                        }
                    }
                }
            }
            db49_wownet.SaveChanges();
            return data.LectureData.SEQ;
        }

        public void Delete(int[] deleteList)
        {
            if (deleteList != null)
            {
                foreach (var item in deleteList)
                {
                    var LecData = GetLecData(item);
                    if (LecData != null)
                    {
                        var SchData = GetSchList(item);
                        if (SchData != null)
                        {
                            foreach (var SchItem in SchData)
                            {
                                var NtbLdata = GetNTBLecList(SchItem.SEQ);
                                if (NtbLdata != null)
                                {
                                    foreach (var Litem in NtbLdata)
                                    {
                                        db49_wownet.NTB_LECTURES_LECTURER.Remove(Litem);
                                    }
                                }
                                db49_wownet.TAB_LECTURES_SCHEDULE.Remove(SchItem);
                            }
                        }
                        db49_wownet.TAB_LECTURES.Remove(LecData);
                        db49_wownet.SaveChanges();
                    }
                }
            }
        }

        public DtlSchedule SearchSchedule(int seq)
        {
            var SchData = GetSchData(seq);
            var dtlSchedule = new DtlSchedule();

            if (SchData != null)
            {
                    dtlSchedule.SEQ = SchData.SEQ;
                    dtlSchedule.MSEQ = SchData.MSEQ;
                    dtlSchedule.PLACE = SchData.PLACE;
                    dtlSchedule.LECTURES_DATE = SchData.LECTURES_DATE;
                    dtlSchedule.LECTURES_TIME = SchData.LECTURES_TIME;
                    dtlSchedule.ETC = SchData.ETC;

                var lecturerList = db49_wownet.NTB_LECTURES_LECTURER.Where(a => a.MSEQ.Equals(dtlSchedule.SEQ)).ToList();

                dtlSchedule.LecturerList = (from l in lecturerList
                                            join p in db49_broadcast.Pro_wowList
                                            on l.PARTNER_NO equals p.Pay_no into jointable
                                            from j in jointable.DefaultIfEmpty()
                                            select new JOIN_LECTURER_PARTNER()
                                            {
                                                SEQ = l.SEQ,
                                                MSEQ = l.MSEQ,
                                                LECTURER = l.LECTURER,
                                                LECTURER_TYPE = l.LECTURER_TYPE,
                                                PARTNER_NO = l.PARTNER_NO,
                                                FullName = (j == null ? "" : j.FullName),
                                                NickName = (j == null ? "" : j.NickName),
                                                Pro_id = (j == null ? "" : j.Pro_id),
                                                Wowtv_id = (j == null ? "" : j.Wowtv_id)
                                            }).ToList();
                }

            //파트너 카페 도메인 정보
            foreach(var item in dtlSchedule.LecturerList)
            {
                if (item.Wowtv_id != "")
                {
                    var index = dtlSchedule.LecturerList.IndexOf(item);
                    var cafeInfo = db49_wownet.usp_Select_MasterOpenCafeInfo(item.Wowtv_id).SingleOrDefault();
                    if (cafeInfo != null)
                    {
                        dtlSchedule.LecturerList[index].CafeDomain = cafeInfo.CafeDomain;
                    }
                    
                }
            }
            
            return dtlSchedule;
        }

        public TAB_LECTURES GetLecData(int seq)
        {
            return db49_wownet.TAB_LECTURES.SingleOrDefault(a => a.SEQ.Equals(seq));
        }

        public TAB_LECTURES_SCHEDULE GetSchData(int seq)
        {
            return db49_wownet.TAB_LECTURES_SCHEDULE.SingleOrDefault(a => a.SEQ.Equals(seq));
        }

        public IQueryable<TAB_LECTURES_SCHEDULE> GetSchList(int seq)
        {
            return db49_wownet.TAB_LECTURES_SCHEDULE.Where(a => a.MSEQ.Equals(seq));
        }

        public NTB_LECTURES_LECTURER GetNTBLecData(int seq)
        {
            return db49_wownet.NTB_LECTURES_LECTURER.SingleOrDefault(a => a.SEQ.Equals(seq));
        }

        public IQueryable<NTB_LECTURES_LECTURER> GetNTBLecList(int seq)
        {
            return db49_wownet.NTB_LECTURES_LECTURER.Where(a => a.MSEQ.Equals(seq));
        }

        public IQueryable<JOIN_LECTURES_SCHEDULE> GetJoin()
        {
            return (from lec in db49_wownet.TAB_LECTURES
                    join sc in db49_wownet.TAB_LECTURES_SCHEDULE
                        on lec.SEQ equals sc.MSEQ
                    join ntb in db49_wownet.NTB_LECTURES_LECTURER
                        on sc.SEQ equals ntb.SEQ into jointable
                    from j in jointable.DefaultIfEmpty()
                    select new JOIN_LECTURES_SCHEDULE()
                    {
                        SEQ = lec.SEQ,
                        //CONTENTS = lec.CONTENTS,
                        TYPE_FLAG = lec.TYPE_FLAG,
                        VIEW_SITE = lec.VIEW_SITE,
                        VIEW_FLAG = lec.VIEW_FLAG,
                        TITLE = lec.TITLE,
                        REG_DATE = lec.REG_DATE,
                        MAIN_CTGR = lec.MAIN_CTGR,
                        MSEQ = sc.SEQ,
                        PLACE = sc.PLACE,
                        LECTURES_DATE = sc.LECTURES_DATE,
                        LECTURER = j.LECTURER,
                        LECTURER_TYPE = j.LECTURER_TYPE,
                        PARTNER_NO = j.PARTNER_NO,
                        WG_IMAGE_FILE = lec.WG_IMAGE_FILE,
                        THUMNAIL_FILE = lec.THUMNAIL_FILE
                    });
        }

        public Pro_wowList GetPartner(int partnerNo)
        {
            return db49_broadcast.Pro_wowList.SingleOrDefault(a => a.Pay_no.Equals(partnerNo));
        }

        public List<JOIN_LECTURES_PARTNER> GetCalendarDate(DateTime searchDate)
        {
            var joinlist = GetJoin().Where(a => a.VIEW_FLAG.Equals("Y"));
            var startDate = searchDate.AddDays(-10).ToString("yyyy-MM-dd");
            var endDate = searchDate.AddDays(40).ToString("yyyy-MM-dd");

            var AMonthList = joinlist.Where(a => String.IsNullOrEmpty(a.LECTURES_DATE) == false
                                           && a.LECTURES_DATE.Length >= 8
                                           && a.LECTURES_DATE.CompareTo(endDate) < 0
                                           && a.LECTURES_DATE.CompareTo(startDate) > 0).ToList();

            var result = (from m in AMonthList
                                        join p in db49_broadcast.Pro_wowList
                                            on m.PARTNER_NO equals p.Pay_no into jointable
                                        from j in jointable.DefaultIfEmpty()
                                        select new JOIN_LECTURES_PARTNER()
                                        {
                                            SEQ = m.SEQ,
                                            CSEQ = m.MSEQ,
                                            TITLE = m.TITLE,
                                            MAIN_CTGR = m.MAIN_CTGR,
                                            LECTURES_DATE = m.LECTURES_DATE,
                                            LECTURES_TIME = m.LECTURES_TIME,
                                            LECTURER = m.LECTURER,
                                            LECTURER_TYPE = m.LECTURER_TYPE,
                                            PARTNER_NO = m.PARTNER_NO,
                                            FullName = (j == null ? "" : j.FullName),
                                            NickName = (j == null ? "" : j.NickName)
                                        }).ToList();

            return result;
        }

        public Dictionary<string, int> GetDateCount(string FirstDate, string LastDate)
        {

                var joinToList = GetJoin().Where(a => a.VIEW_FLAG.Equals("Y")).ToList();
                var dateCount = joinToList.GroupBy(a => a.LECTURES_DATE)
                                    .Select(a => new { LECTURES_DATE = a.Key, CNT = a.Count() });
                dateCount = dateCount.Where(a => (DateTime.Parse(a.LECTURES_DATE) > DateTime.Parse(FirstDate) || DateTime.Parse(a.LECTURES_DATE) == DateTime.Parse(FirstDate))
                                                && (DateTime.Parse(a.LECTURES_DATE) < DateTime.Parse(LastDate) || DateTime.Parse(a.LECTURES_DATE) == DateTime.Parse(LastDate)))
                                                .OrderBy(a=> a.LECTURES_DATE);
                var dateDict = new Dictionary<string, int>();

                foreach (var item in dateCount)
                {
                    dateDict.Add(item.LECTURES_DATE, item.CNT);
                }

                return dateDict;
        }
        
        public bool ApplyLecture(TAB_ACADEMY_APPL model, LoginUserInfo loginUserInfo)
        {
            bool isApply = true;
            var data = new TAB_ACADEMY_APPL();

            if(loginUserInfo != null)
            {
                data = db49_wownet.TAB_ACADEMY_APPL.SingleOrDefault(a => a.LS_SEQ == model.LS_SEQ && a.USERID.Equals(model.USERID));
            }
            else
            {
                data = db49_wownet.TAB_ACADEMY_APPL.SingleOrDefault(a => a.LS_SEQ == model.LS_SEQ && a.MOBILE.Equals(model.MOBILE));
            }
            
            if(data != null)
            {
                isApply = false;
            }
            else{
                model.REGDATE = DateTime.Now;
                model.REG_FLAG = "Y";
                if (model.AGREE_AD == null)
                {
                    model.AGREE_AD = "N";
                }
                db49_wownet.TAB_ACADEMY_APPL.Add(model);
                db49_wownet.SaveChanges();
            }
            return isApply;
        }

        public ListModel<JOIN_LECTURES__CODE> GetQuickLectures(LectureCondition condition)
        {
            var resultData = new ListModel<JOIN_LECTURES__CODE>();
            var joinlist = GetJoin();

            var toList = joinlist.ToList();
            var CodeJoinList = new List<JOIN_LECTURES__CODE>();


                CodeJoinList = toList.Select(a => new JOIN_LECTURES__CODE()
                                                {
                                                    SEQ = a.SEQ,
                                                    MSEQ = a.MSEQ,
                                                    VIEW_SITE = a.VIEW_SITE,
                                                    MAIN_CTGR = a.MAIN_CTGR,
                                                    TYPE_FLAG = a.TYPE_FLAG,
                                                    TITLE = a.TITLE,
                                                    REG_DATE = a.REG_DATE,
                                                    VIEW_FLAG = a.VIEW_FLAG,
                                                    LECTURES_DATE = a.LECTURES_DATE,
                                                    LECTURES_TIME = a.LECTURES_TIME,
                                                    PLACE = a.PLACE,
                                                    WG_IMAGE_FILE = a.WG_IMAGE_FILE,
                                                }
                                            ).Where(a => a.VIEW_FLAG.Equals("Y")).ToList();


            resultData.TotalDataCount = CodeJoinList.Count();

            if (condition.PageSize > 0)
            {
                if (condition.PageSize == 0)
                {
                    condition.PageSize = 10;
                }
                //최신순 LECTURE_DATE
                CodeJoinList = CodeJoinList.Where(a => DateTime.Parse(a.LECTURES_DATE) >= DateTime.Now
                                        && DateTime.Parse(a.LECTURES_DATE) <= DateTime.Parse(a.LECTURES_DATE).AddDays(7)).ToList();
                CodeJoinList = CodeJoinList.OrderByDescending(a => DateTime.Parse(a.LECTURES_DATE)).Skip(condition.CurrentIndex).Take(condition.PageSize).ToList();
            }
            resultData.ListData = CodeJoinList.ToList();
            return resultData;
        }

        public ListModel<JOIN_LECTURES_SCHEDULE> GetLatestLecture()
        {
            var resultData = new ListModel<JOIN_LECTURES_SCHEDULE>();
            var today = DateTime.Now.ToString("yyyy-MM-dd");


            //var joinlist = GetJoin().Where(a => a.VIEW_FLAG.Equals("Y"));
            var joinlist = GetJoin().Where(a => a.VIEW_FLAG.Equals("Y") && a.MAIN_CTGR.Equals("S"));
            joinlist = joinlist.Where(a => String.IsNullOrEmpty(a.LECTURES_DATE) == false
                                           && a.LECTURES_DATE.Length >= 8
                                           && (a.LECTURES_DATE.CompareTo(today) > 0
                                                || a.LECTURES_DATE.CompareTo(today) == 0));

            resultData.ListData = joinlist.OrderBy(a => a.LECTURES_DATE).Take(3).ToList();
            return resultData;
        }
    }
}
