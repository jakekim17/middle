using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wow.Tv.Middle.Biz.Component;
using Wow.Tv.Middle.Model.Db51.ARSsms;

namespace Wow.Tv.Middle.Biz.Member
{
    public class MemberUtilBiz : BaseBiz
    {
        /// <summary>
        /// SMS > 비밀번호 초기화
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="tempPassword"></param>
        /// <returns></returns>
        public void SmsTempPassword(string mobileNo, string tempPassword)
        {
            string mobileNumber = mobileNo?.Replace("-", "");
            SC_TRAN data = new SC_TRAN();
            data.TR_SENDDATE = DateTime.Now;
            data.TR_SENDSTAT = "0";
            data.TR_RSLTSTAT = "00";
            data.TR_PHONE = mobileNumber;
            data.TR_CALLBACK = "0266760000";
            data.TR_MSG = "회원님의 임시 비밀번호는 " + tempPassword + " 입니다.";
            data.TR_ID = "wowsms-member";
            data.TR_ETC1 = "비밀번호찾기(TV)";
            db51_ARSsms.SC_TRAN.Add(data);
            db51_ARSsms.SaveChanges();
        }

        /// <summary>
        /// SMS 인증
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <returns></returns>
        public string SmsDormancyAuth(string mobileNo)
        {
            mobileNo = mobileNo.Replace("-", "");
            Random rnd = new Random();
            int number = rnd.Next(0, 999999);
            string authNumber = string.Format("{0:D6}", number);

            SC_TRAN data = new SC_TRAN();
            data.TR_SENDDATE = DateTime.Now;
            data.TR_SENDSTAT = "0";
            data.TR_RSLTSTAT = "00";
            data.TR_PHONE = mobileNo;
            data.TR_CALLBACK = "0266760000";
            data.TR_MSG = "[한국경제TV] 인증번호는 [" + authNumber + "] 입니다.";
            data.TR_ID = "wowsms-member";
            data.TR_ETC1 = "휴대폰인증 휴면회원";
            db51_ARSsms.SC_TRAN.Add(data);
            db51_ARSsms.SaveChanges();

            return authNumber;
        }

        /// <summary>
        /// 이메일 > 비밀번호 초기화
        /// </summary>
        /// <param name="parameter"></param>
        public void EmailAuth(MemberEmailModel parameter)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mailing\\Member\\EmailAuth.html");
            string encodedEmail = System.Net.WebUtility.UrlEncode(parameter.Email);
            StreamReader reader = new StreamReader(templatePath);
            string mapContent = reader.ReadToEnd();
            mapContent = mapContent.Replace("@@EmailAuthCompleteUrl@@", parameter.WowTvMemberUrl + "/Join/EmailAuthComplete?usernumber=" + parameter.UserNumber.ToString() + "&email=" + encodedEmail);
            mapContent = mapContent.Replace("@@ExpireDate@@", parameter.ExpireDate.ToString());
            _EmailTemplateReplace(ref mapContent, parameter);

            new EmailBiz().Send(new EmailSendParameter()
            {
                EmailCode = EmailCodeModel.EmailCerificate,
                FromName = "한국경제TV",
                FromEmail = "webmaster@wowtv.co.kr",
                ToName = "이메일인증시도회원",
                ToEmail = parameter.Email,
                Subject = "한국경제TV 이메일 인증입니다.",
                Contents = mapContent
            });
        }

        /// <summary>
        /// 메일 > 회원가입완료 (일반/외국인 사용자)
        /// </summary>
        /// <param name="parameter"></param>
        public void EmailRegistGeneral(MemberEmailModel parameter)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mailing\\Member\\RegisteredAlarmGeneral.html");
            StreamReader reader = new StreamReader(templatePath);
            string mapContent = reader.ReadToEnd();
            mapContent = mapContent.Replace("@@MyPageUrl@@", parameter.WowTvMyPageUrl);
            mapContent = mapContent.Replace("@@UserName@@", parameter.UserName);
            mapContent = mapContent.Replace("@@NickName@@", parameter.NickName);
            mapContent = mapContent.Replace("@@Email@@", parameter.Email);
            mapContent = mapContent.Replace("@@BonusCash@@", string.Format("{0:#,0}", parameter.BonusCash));
            _EmailTemplateReplace(ref mapContent, parameter);

            new EmailBiz().Send(new EmailSendParameter()
            {
                EmailCode = EmailCodeModel.RegisteredAlarmGeneral,
                FromName = "한국경제TV관리자",
                FromEmail = "webmaster@wownet.co.kr",
                ToName = parameter.NickName,
                ToEmail = parameter.Email,
                Subject = "한국경제TV 회원이 되신것을 진심으로 축하드립니다.",
                Contents = mapContent
            });
        }

        /// <summary>
        /// 메일 > 회원가입완료 (법인 사용자)
        /// </summary>
        /// <param name="parameter"></param>
        public void EmailRegistCompany(MemberEmailModel parameter)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mailing\\Member\\RegisteredAlarmCompany.html");
            StreamReader reader = new StreamReader(templatePath);
            string mapContent = reader.ReadToEnd();
            mapContent = mapContent.Replace("@@MyPageUrl@@", parameter.WowTvMyPageUrl);
            mapContent = mapContent.Replace("@@UserName@@", parameter.UserName);
            mapContent = mapContent.Replace("@@NickName@@", parameter.NickName);
            mapContent = mapContent.Replace("@@Email@@", parameter.Email);
            mapContent = mapContent.Replace("@@BonusCash@@", string.Format("{0:#,0}", parameter.BonusCash));
            _EmailTemplateReplace(ref mapContent, parameter);

            new EmailBiz().Send(new EmailSendParameter()
            {
                EmailCode = EmailCodeModel.RegisteredAlarmCompany,
                FromName = "한국경제TV관리자",
                FromEmail = "webmaster@wownet.co.kr",
                ToName = parameter.NickName,
                ToEmail = parameter.Email,
                Subject = "한국경제TV 회원이 되신것을 진심으로 축하드립니다.",
                Contents = mapContent
            });
        }

        /// <summary>
        /// 메일 > 회원정보 수정 완료
        /// </summary>
        /// <param name="parameter"></param>
        public void EmailUserEdit(MemberEmailModel parameter)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mailing\\Member\\EditedAlarm.html");
            StreamReader reader = new StreamReader(templatePath);
            string mapContent = reader.ReadToEnd();
            mapContent = mapContent.Replace("@@UserId@@", parameter.UserId);
            mapContent = mapContent.Replace("@@NickName@@", parameter.NickName);
            int changedItemNo = 0;
            if (parameter.ChangedItemList != null)
            {
                foreach (string item in parameter.ChangedItemList)
                {
                    changedItemNo++;
                    mapContent = mapContent.Replace("@@ChangedItem" + changedItemNo + "@@", item);
                }
            }
            for (int i = changedItemNo + 1; i <= 7; i++)
            {
                mapContent = mapContent.Replace("@@ChangedItem" + i + "@@", "");
            }
            _EmailTemplateReplace(ref mapContent, parameter);

            new EmailBiz().Send(new EmailSendParameter()
            {
                EmailCode = EmailCodeModel.ModifiedAlarm,
                FromName = "한국경제TV관리자",
                FromEmail = "webmaster@wownet.co.kr",
                ToName = parameter.NickName,
                ToEmail = parameter.Email,
                Subject = "한국경제TV 통합회원정보 수정이 완료 되었습니다.",
                Contents = mapContent
            });
        }

        /// <summary>
        /// 메일 > 법인회원 승인거부 안내
        /// </summary>
        /// <param name="parameter"></param>
        public void EmailCompanyRegistRejectAlarm(MemberEmailModel parameter)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mailing\\Member\\CompanyRegistRejectAlarm.html");
            StreamReader reader = new StreamReader(templatePath);
            string mapContent = reader.ReadToEnd();
            mapContent = mapContent.Replace("@@NickName@@", parameter.NickName);
            mapContent = mapContent.Replace("@@UserId@@", parameter.UserId);
            _EmailTemplateReplace(ref mapContent, parameter);

            new EmailBiz().Send(new EmailSendParameter()
            {
                EmailCode = EmailCodeModel.SecessionAlarm,
                FromName = "한국경제TV관리자",
                FromEmail = "webmaster@wownet.co.kr",
                ToName = parameter.NickName,
                ToEmail = parameter.Email,
                Subject = "한국경제TV 법인회원 승인거부 안내",
                Contents = mapContent
            });
        }

        /// <summary>
        /// 메일 > 회원 아이디 찾기 안내
        /// </summary>
        /// <param name="parameter"></param>
        public void EmailFindIdAlarm(MemberEmailModel parameter)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mailing\\Member\\FindIdAlarm.html");
            StreamReader reader = new StreamReader(templatePath);
            string mapContent = reader.ReadToEnd();
            mapContent = mapContent.Replace("@@NickName@@", parameter.NickName);
            mapContent = mapContent.Replace("@@UserId@@", parameter.UserId);
            _EmailTemplateReplace(ref mapContent, parameter);

            new EmailBiz().Send(new EmailSendParameter()
            {
                EmailCode = EmailCodeModel.SecessionAlarm,
                FromName = "한국경제TV관리자",
                FromEmail = "webmaster@wownet.co.kr",
                ToName = parameter.NickName,
                ToEmail = parameter.Email,
                Subject = "한국경제TV 아이디 조회 알림",
                Contents = mapContent
            });
        }

        /// <summary>
        /// 메일 > 회원 임시 비밀번호 안내
        /// </summary>
        /// <param name="parameter"></param>
        public void EmailFindPasswordAlarm(MemberEmailModel parameter)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mailing\\Member\\FindPasswordAlarm.html");
            StreamReader reader = new StreamReader(templatePath);
            string mapContent = reader.ReadToEnd();
            mapContent = mapContent.Replace("@@NickName@@", parameter.NickName);
            mapContent = mapContent.Replace("@@TempPassword@@", parameter.TempPassword);
            _EmailTemplateReplace(ref mapContent, parameter);

            new EmailBiz().Send(new EmailSendParameter()
            {
                EmailCode = EmailCodeModel.SecessionAlarm,
                FromName = "한국경제TV관리자",
                FromEmail = "webmaster@wownet.co.kr",
                ToName = parameter.NickName,
                ToEmail = parameter.Email,
                Subject = "한국경제TV 임시 비밀번호 입니다.",
                Contents = mapContent
            });
        }

        /// <summary>
        /// 메일 > 회원 탈퇴 완료
        /// </summary>
        /// <param name="parameter"></param>
        public void EmailUserSecession(MemberEmailModel parameter)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mailing\\Member\\SecessionAlarm.html");
            StreamReader reader = new StreamReader(templatePath);
            string mapContent = reader.ReadToEnd();
            mapContent = mapContent.Replace("@@NickName@@", parameter.NickName);
            _EmailTemplateReplace(ref mapContent, parameter);

            new EmailBiz().Send(new EmailSendParameter()
            {
                EmailCode = EmailCodeModel.SecessionAlarm,
                FromName = "한국경제TV관리자",
                FromEmail = "webmaster@wownet.co.kr",
                ToName = parameter.NickName,
                ToEmail = parameter.Email,
                Subject = "한국경제TV 통합회원 탈퇴가 완료 되었습니다.",
                Contents = mapContent
            });
        }

        private void _EmailTemplateReplace(ref string mapContent, MemberEmailModel parameter)
        {
            mapContent = mapContent.Replace("@@WowTvMemberStyle@@", parameter.WowTvMemberStyle);
            mapContent = mapContent.Replace("@@WowTvFrontUrl@@", parameter.WowTvFrontUrl);
            mapContent = mapContent.Replace("@@WowNetCafeUrl@@", parameter.WowNetCafeUrl);
            mapContent = mapContent.Replace("@@WowFaFrontUrl@@", parameter.WowFaFrontUrl);
            mapContent = mapContent.Replace("@@WowStarFrontUrl@@", parameter.WowStarFrontUrl);
            mapContent = mapContent.Replace("@@WowTvHelpInquiryUrl@@", parameter.WowTvHelpInquiryUrl);
            mapContent = mapContent.Replace("@@WowTvMemberInfoEdit@@", parameter.WowTvMemberInfoEdit);
            mapContent = mapContent.Replace("@@WowTvMemberUrl@@", parameter.WowTvMemberUrl);
        }
    }

    public class MemberEmailModel
    {
        public int UserNumber { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string TempPassword { get; set; }
        public int BonusCash { get; set; }
        public DateTime ExpireDate { get; set; }
        public List<string> ChangedItemList { get; set; }
        public string WowTvFrontUrl { get; set; }
        public string WowNetFrontUrl { get; set; }
        public string WowNetCafeUrl { get; set; }
        public string WowFaFrontUrl { get; set; }
        public string WowStarFrontUrl { get; set; }
        public string WowTvHelpInquiryUrl { get; set; }
        public string WowTvMemberInfoEdit { get; set; }
        public string WowTvMemberStyle { get; set; }
        public string WowTvMemberUrl { get; set; }
        public string WowTvMyPageUrl { get; set; }
    }
}