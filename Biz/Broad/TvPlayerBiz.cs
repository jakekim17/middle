using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Fx;
using Wow.Tv.Middle.Biz.CommonCode;
using Wow.Tv.Middle.Biz.Member;
using Wow.Tv.Middle.Model.Db49.editVOD;
using Wow.Tv.Middle.Model.Db49.wowtv;
using Wow.Tv.Middle.Model.Db90.DNRS;

namespace Wow.Tv.Middle.Biz.Broad
{
    public class TvPlayerBiz : BaseBiz
    {
        /// <summary>
        /// 라이브 TV 정보
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public LiveTvInfoModel LiveTvNowInfo(/*int userNumber*/)
        {
            LiveTvInfoModel retval = new LiveTvInfoModel();
            MemberInfoBiz memberInfoBiz = new MemberInfoBiz();

            DateTime now = DateTime.Now;
            string nowDateString = now.ToString("yyyy-MM-dd");
            string nowTimeString = now.ToString("HH:mm");

            List<T_RUNDOWN> list = db90_DNRS.T_RUNDOWN.Where(a => a.RUN_DATE == nowDateString && a.RUN_START.CompareTo(nowTimeString) <= 0 && a.RUN_END.CompareTo(nowTimeString) >= 0)
                .OrderByDescending(a => a.SEQ).ToList();
            T_RUNDOWN info = null;

            if (list.Count > 0)
            {
                info = list[0];
                retval.ProgramId = info.PRG_CD;
                retval.ProgramName = info.PRG_NM;
                retval.StartTime = info.RUN_START;
                retval.EndTime = info.RUN_END;

                retval.PartnerNameList = db49_wowtv.NTB_PROGRAM_PARTNER.Where(a => a.PRG_CD == info.PRG_CD).Select(a => a.PARTNER_NAME).ToList();

                NTB_NEWS_PROGRAM ntbNewsProgram = db90_DNRS.NTB_NEWS_PROGRAM.Where(a => a.PRG_CD == info.PRG_CD).SingleOrDefault();
                if (ntbNewsProgram != null)
                {
                    retval.PlanBroad = ntbNewsProgram.PLAN_BROAD;
                }

                T_NEWS_PRG newsProgram = db90_DNRS.T_NEWS_PRG.Where(a => a.PRG_CD == info.PRG_CD).SingleOrDefault();
                if (newsProgram != null)
                {
                    if (newsProgram.IsMonday == true)
                    {
                        retval.DayOfTheWeekList.Add("월");
                    }
                    if (newsProgram.IsTuesday == true)
                    {
                        retval.DayOfTheWeekList.Add("화");
                    }
                    if (newsProgram.IsWednesday == true)
                    {
                        retval.DayOfTheWeekList.Add("수");
                    }
                    if (newsProgram.IsThursday == true)
                    {
                        retval.DayOfTheWeekList.Add("목");
                    }
                    if (newsProgram.IsFriday == true)
                    {
                        retval.DayOfTheWeekList.Add("금");
                    }
                    if (newsProgram.IsSaturday == true)
                    {
                        retval.DayOfTheWeekList.Add("토");
                    }
                    if (newsProgram.IsSunday == true)
                    {
                        retval.DayOfTheWeekList.Add("일");
                    }
                }
            }

            return retval;
        }

        /// <summary>
        /// TV 다시보기 정보
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public TvReplayInfoModel TvReplayInfo(int num)
        {
            TvReplayInfoModel retval = new TvReplayInfoModel();

            tv_program playInfo = db90_DNRS.tv_program.Where(a => a.Num == num).SingleOrDefault();
            if (playInfo != null)
            {
                string programId = playInfo.Dep;

                // playInfo 정보
                if (string.IsNullOrEmpty(playInfo.FilePath) == false)
                {
                    retval.Url = playInfo.FilePath.Replace("rtmp://cdnvod.wowtv.co.kr/wowtvvod/_definst_/mp4:", "http://cdnvod.wowtv.co.kr:8080/");
                }

                retval.BroadDate = null;
                if (string.IsNullOrEmpty(playInfo.broad_date) == false && playInfo.broad_date.Length >= 12)
                {
                    retval.BroadDate = new DateTime(
                        int.Parse(playInfo.broad_date.Substring(0, 4)), int.Parse(playInfo.broad_date.Substring(4, 2)), int.Parse(playInfo.broad_date.Substring(6, 2)),
                        int.Parse(playInfo.broad_date.Substring(8, 2)), int.Parse(playInfo.broad_date.Substring(10, 2)), 0
                    );
                }
                retval.PartnerNameList = db49_wowtv.NTB_PROGRAM_PARTNER.Where(a => a.PRG_CD == programId).Select(a => a.PARTNER_NAME).ToList();

                // scheduleInfo 정보
                IMG_SCHEDULE scheduleInfo = db90_DNRS.IMG_SCHEDULE.Where(a => a.prog_id == programId).SingleOrDefault();
                if (scheduleInfo != null)
                {
                    retval.ProgramId = scheduleInfo.prog_id;
                    retval.ProgramName = scheduleInfo.prog_name;
                }

                // 첫 방송 무료 체크
                bool freeFirstProgram = false;
                NTB_NEWS_PROGRAM programInfo = db90_DNRS.NTB_NEWS_PROGRAM.Where(a => a.PRG_CD == programId).SingleOrDefault();
                if (programInfo != null)
                {
                    if (!string.IsNullOrEmpty(programInfo.BROAD_SECTION_CODE))
                    {
                        //retval.ProgramGubunCode = new CommonCode.CommonCodeBiz().GetAt("034000000", programInfo.BROAD_SECTION_CODE).COMMON_CODE;
                        retval.ProgramGubunCode = db49_wowtv.NTB_COMMON_CODE.Where(a => a.UP_COMMON_CODE == "034000000" && a.CODE_VALUE1 == programInfo.BROAD_SECTION_CODE).OrderBy(a => a.COMMON_CODE).FirstOrDefault().COMMON_CODE;
                    }

                    if (programInfo.FIRST_FREE_YN == "Y")
                    {
                        freeFirstProgram = true;
                    }
                    else
                    {
                        if (scheduleInfo != null)
                        {
                            NTB_NEWS_PROGRAM parentProgramInfo = db90_DNRS.NTB_NEWS_PROGRAM.Where(a => a.PRG_CD == scheduleInfo.parentid).SingleOrDefault();
                            if (parentProgramInfo != null)
                            {
                                if (parentProgramInfo.FIRST_FREE_YN == "Y")
                                {
                                    freeFirstProgram = true;
                                }
                            }
                        }
                    }
                }

                if (freeFirstProgram == true)
                {
                    int? firstPlayNum = db90_DNRS.tv_program.Where(a => a.Dep == programId).OrderBy(a => a.broad_date).Select(a => a.Num).FirstOrDefault();
                    if (firstPlayNum == playInfo.Num)
                    {
                        retval.Point = 0;
                    }
                    else
                    {
                        retval.Point = scheduleInfo.point;
                    }
                }
                else
                {
                    if (scheduleInfo != null)
                    {
                        retval.Point = scheduleInfo.point;
                    }
                }
            }

            return retval;
        }

        /// <summary>
        /// VOD 정보
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public VodInfoModel VodInfo(int num)
        {
            VodInfoModel retval = new VodInfoModel();

            tblVODList vodInfo = db49_editVOD.tblVODList.Where(a => a.vodNum == num).SingleOrDefault();
            if (vodInfo != null)
            {
                retval.Subject = vodInfo.Subject;
                retval.InsertDate = vodInfo.insDate;

                tblVODEdit vodEdit = db49_editVOD.tblVODEdit.Where(a => a.vodNum == num).SingleOrDefault();
                if (vodEdit != null)
                {
                    if (string.IsNullOrEmpty(vodEdit.EditFolder) == false && string.IsNullOrEmpty(vodEdit.EditFile) == false)
                    {
                        if (vodEdit.EditFile.IndexOf(".mp4") > -1)
                        {
                            retval.Url = "http://cdnvod.wowtv.co.kr:8080/EditVOD/" + vodEdit.EditFolder?.Trim() + "/" + vodEdit.EditFile?.Trim();
                        }
                        else
                        {
                            retval.Url = "mms://vod1.wowtv.co.kr/EditVOD/" + vodEdit.EditFolder?.Trim() + "/" + vodEdit.EditFile?.Trim();
                        }
                    }
                }
            }

            return retval;
        }
    }

    /// <summary>
    /// 실시간TV 정보
    /// </summary>
    public class LiveTvInfoModel
    {
        public LiveTvInfoModel()
        {
            PartnerNameList = new List<string>();
            DayOfTheWeekList = new List<string>();
        }

        /// <summary>
        /// 프로그램 아이디
        /// </summary>
        public string ProgramId { get; set; }

        /// <summary>
        /// 프로그램 명
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// 시작시간
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 종료시간
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 기획방송 (NONE: 일반, FIRST: 첫방송, OPEN: 개편방송)
        /// </summary>
        public string PlanBroad { get; set; }

        /// <summary>
        /// 파트너 리스트
        /// </summary>
        public List<string> PartnerNameList { get; set; }

        /// <summary>
        /// 방송요일 리스트
        /// </summary>
        public List<string> DayOfTheWeekList { get; set; }
    }

    /// <summary>
    /// TV 다시보기
    /// </summary>
    public class TvReplayInfoModel
    {
        /// <summary>
        /// 프로그램 아이디
        /// </summary>
        public string ProgramId { get; set; }

        /// <summary>
        /// 프로그램 명
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// 영상 경로
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 방송일시
        /// </summary>
        public DateTime? BroadDate { get; set; }

        /// <summary>
        /// 파트너 리스트
        /// </summary>
        public List<string> PartnerNameList { get; set; }

        /// <summary>
        /// 구매 포인트
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// 프로그램 구분 코드
        /// </summary>
        public string ProgramGubunCode { get; set; }    

    }

    /// <summary>
    /// 증권 영상
    /// </summary>
    public class VodInfoModel
    {
        /// <summary>
        /// 제목
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 영상 경로
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 방송일시
        /// </summary>
        public DateTime? InsertDate { get; set; }
    }
}
