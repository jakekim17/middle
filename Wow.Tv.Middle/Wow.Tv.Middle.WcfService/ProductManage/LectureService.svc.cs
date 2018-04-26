using System;
using System.Collections.Generic;
using Wow.Tv.Middle.Biz.ProductManage;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.Lecture;

namespace Wow.Tv.Middle.WcfService.ProductManage
{
    public class LectureService : ILectureService
    {
        public ListModel<JOIN_LECTURES__CODE> GetList(LectureCondition condition)
        {
            return new LectureBiz().GetList(condition);
        }

        public LectureScheduleDtl GetDetail(int seq)
        {
            return new LectureBiz().GetDetail(seq);
        }

        public int Save(LectureScheduleDtl data, LoginUser loginUser)
        {
            return new LectureBiz().Save(data, loginUser);
        }

        public void Delete(int[] deleteList)
        {
            new LectureBiz().Delete(deleteList);
        }

        public List<JOIN_LECTURES_PARTNER> GetCalendarDate(DateTime searchDate)
        {
            return new LectureBiz().GetCalendarDate(searchDate);
        }

        public DtlSchedule SearchSchedule(int seq)
        {
            return new LectureBiz().SearchSchedule(seq);
        }

        public Dictionary<string, int> GetDateCount(string FirstDate, string LastDate)
        {
            return new LectureBiz().GetDateCount(FirstDate, LastDate);
        }

        public bool ApplyLecture(TAB_ACADEMY_APPL model, LoginUserInfo loginUserInfo)
        {
            return new LectureBiz().ApplyLecture(model, loginUserInfo);
        }

        public ListModel<JOIN_LECTURES__CODE> GetQuickLectures(LectureCondition condition)
        {
            return new LectureBiz().GetQuickLectures(condition);
        }

        public ListModel<JOIN_LECTURES_SCHEDULE> GetLatestLecture()
        {
            return new LectureBiz().GetLatestLecture();
        }
    }
}
