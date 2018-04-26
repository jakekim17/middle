using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Fx;
using Wow.Tv.Middle.Model.Db51.ems50;

namespace Wow.Tv.Middle.Biz.Component
{
    public class EmailBiz : BaseBiz
    {
        public void Send(EmailSendParameter sendInfo)
        {
            EMS_MAILQUEUE mailInfo = null;
            try
            {
                string emailCode = ((int)sendInfo.EmailCode).ToString();
                if (emailCode.Length == 1)
                {
                    emailCode = "0" + emailCode;
                }
                string targetDate = null;
                if (sendInfo.TargetDate != null)
                {
                    targetDate = sendInfo.TargetDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                string registDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string executeSql = "EXEC NUP_MAILQUEUE_INSERT" +
                    " @MAIL_CODE={0},@TO_EMAIL={1},@TO_NAME={2},@FROM_EMAIL={3},@FROM_NAME={4},@SUBJECT={5},@TARGET_FLAG={6},@TARGET_DATE={7},@REG_DATE={8}" +
                    ",@MAP1={9},@MAP2={10},@MAP3={11},@MAP4={12},@MAP5={13},@MAP_CONTENT={14}";

                db51_ems50.Database.ExecuteSqlCommand(executeSql, emailCode, sendInfo.ToEmail, sendInfo.ToName, sendInfo.FromEmail, sendInfo.FromName,
                    sendInfo.Subject, "N", targetDate, registDate, null, null, null, null, null, sendInfo.Contents);
            }
            catch (Exception ex)
            {
                if (mailInfo != null)
                {
                    WowLog.Write("[EmailBiz.Send] " +
                        "MAIL_CODE: " + mailInfo.MAIL_CODE +
                        ", TO_NAME: " + mailInfo.TO_NAME +
                        ", TO_EMAIL: " + mailInfo.TO_EMAIL +
                        ", FROM_NAME: " + mailInfo.FROM_NAME +
                        ", FROM_EMAIL: " + mailInfo.FROM_EMAIL +
                        ", SUBJECT: " + mailInfo.SUBJECT +
                        ", MAP_CONTENT: " + mailInfo.MAP_CONTENT +
                        ", TARGET_FLAG: " + mailInfo.TARGET_FLAG +
                        ", TARGET_DATE" + (mailInfo.TARGET_DATE.HasValue == true ? mailInfo.TARGET_DATE.ToString() : "") +
                        ", REG_DATE" + (mailInfo.REG_DATE.HasValue == true ? mailInfo.REG_DATE.ToString() : "") +
                        "\r\nException: " + ex.ToString());
                }
                else
                {
                    WowLog.Write("[EmailBiz.Send] Exception: " + ex.ToString());
                }
            }
        }
    }

    public class EmailSendParameter
    {
        public EmailCodeModel EmailCode { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Contents { get; set; }
        public bool TargetFlag { get; set; } = false;
        public DateTime? TargetDate { get; set; }
    }

    public enum EmailCodeModel
    {
        /// <summary>
        /// 회원가입 완료 안내 (개인/외국인)
        /// </summary>
        RegisteredAlarmGeneral = 51,

        /// <summary>
        /// 회원가입 완료 안내 (법인)
        /// </summary>
        RegisteredAlarmCompany = 52,

        /// <summary>
        /// 법인회원승인거부
        /// </summary>
        CompanyRegistReject = 53,

        /// <summary>
        /// 이메일 인증
        /// </summary>
        EmailCerificate = 54,

        /// <summary>
        /// 아이디찾기
        /// </summary>
        FindIdAlarm = 55,

        /// <summary>
        /// 비밀번호 찾기
        /// </summary>
        FindPasswordAlarm = 56,

        /// <summary>
        /// 회원정보 수정 완료 안내
        /// </summary>
        ModifiedAlarm = 57,

        /// <summary>
        /// 회원 탈퇴 완료 안내
        /// </summary>
        SecessionAlarm = 58,

        /// <summary>
        /// 회원등급 업그레이드
        /// </summary>
        MemberLevelUpGradeAlarm = 59,

        /// <summary>
        /// IR클럽 가입 완료 안내
        /// </summary>
        IRClubRegisteredAlarm = 60,

        /// <summary>
        /// 기자에게 메일 보내기
        /// </summary>
        SendReporterEmail = 61,

        /// <summary>
        /// TV 프로그램 배송 안내
        /// </summary>
        TvProgramDeliveryAlarm = 62,

        /// <summary>
        /// CD 발송 완료 안내
        /// </summary>
        SendCDCompletedAlarm = 63,

        /// <summary>
        /// 사업문의시 메일 보내기
        /// </summary>
        SendBusinessAdmin = 64
    }
}
