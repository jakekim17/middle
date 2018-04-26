

using System;
using ADODB;
using CDO;

namespace Wow.Fx
{
    public class CDOSend
    {
        public static void MailSend()
        {
            //Set myMail = CreateObject("CDO.Message")
            //myMail.Subject = "Sending email with CDO"
            //myMail.From = "mymail@mydomain.com"
            //myMail.To = "someone@somedomain.com"
            //myMail.Bcc = "someoneelse@somedomain.com"
            //myMail.Cc = "someoneelse2@somedomain.com"
            //myMail.TextBody = "This is a message."
            //myMail.Send
            //set myMail = nothing

            //'1 (로컬 SMTP) / 2 (외부 SMTP)
            //objConfig.Fields.Item("http://schemas.microsoft.com/cdo/configuration/sendusing") = 1

            //'SMTP Port

            //objConfig.Fields.Item("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 25

            //'호스트설정

            //objConfig.Fields.Item("http://schemas.microsoft.com/cdo/configuration/smtpserver") = "localhost"

            //'연결시간

            //objConfig.Fields.Item("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout") = 30

            //objConfig.Fields.Update
            CDO.Message mail = new CDO.Message();

            IConfiguration iConfg = mail.Configuration;

            ADODB.Fields oFields;
            oFields = iConfg.Fields;

            // Set configuration.
            ADODB.Field oField = oFields["http://schemas.microsoft.com/cdo/configuration/sendusing"];
            oField.Value = CDO.CdoSendUsing.cdoSendUsingPickup;

            oField = oFields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"];
            oField.Value = 25;

            oField = oFields["http://schemas.microsoft.com/cdo/configuration/smtpserver"];
            oField.Value = "localhost";

            oField = oFields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"];
            oField.Value = @"C:\inetpub\mailroot\Pickup";

            oField = oFields["http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout"];
            oField.Value = 30;

            oFields.Update();

            mail.Configuration = (CDO.Configuration)iConfg;
            mail.Subject = "테스트 CDO";
            mail.From = "ori007@naver.com";
            mail.To = "youlove0131@paran.com";
            //mail.HTMLBody = "CDOㅔ ";
            mail.TextBody = "CDO 메일 테스트 합니다.";
            mail.Send();
            mail = null;

        }
    }
}