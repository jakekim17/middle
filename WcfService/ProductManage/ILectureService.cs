using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wow.Tv.Middle.Model.Common;
using Wow.Tv.Middle.Model.Db49.wownet;
using Wow.Tv.Middle.Model.Db49.wownet.Lecture;

namespace Wow.Tv.Middle.WcfService.ProductManage
{
    [ServiceContract]
    public interface ILectureService
    {
        [OperationContract]
        ListModel<JOIN_LECTURES__CODE> GetList(LectureCondition condition);

        [OperationContract]
        LectureScheduleDtl GetDetail(int seq);

        [OperationContract]
        int Save(LectureScheduleDtl data, LoginUser loginUser);

        [OperationContract]
        void Delete(int[] deleteList);

        [OperationContract]
        List<JOIN_LECTURES_PARTNER> GetCalendarDate(DateTime searchDate);

        [OperationContract]
        DtlSchedule SearchSchedule(int seq);

        [OperationContract]
        Dictionary<string, int> GetDateCount(string FirstDate, string LastDate);

        [OperationContract]
        bool ApplyLecture(TAB_ACADEMY_APPL model, LoginUserInfo loginUserInfo);

        [OperationContract]
        ListModel<JOIN_LECTURES__CODE> GetQuickLectures(LectureCondition condition);

        [OperationContract]
        ListModel<JOIN_LECTURES_SCHEDULE> GetLatestLecture();
    }
}
