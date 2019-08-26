namespace Msn.InteropDemo.Communication.Emailing
{
    public interface IEmailSender
    {
        void SendEmail(EmailModel dto);

        void SendEmail(string from,
                       string to,
                       string subject,
                       string body,
                       string cc = null,
                       string bcc = null,
                       bool isBodyHtml = false,
                       string attachments = null);
    }
}
