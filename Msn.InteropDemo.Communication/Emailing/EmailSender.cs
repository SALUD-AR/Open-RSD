using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Msn.InteropDemo.Communication.Emailing
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly EmailSenderConfiguration _configuration;

        public EmailSender(ILogger<EmailSender> logger, EmailSenderConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void SendEmail(EmailModel dto) => SendEmail(dto.From,
                                                           dto.To,
                                                           dto.Subject,
                                                           dto.Body,
                                                           dto.Cc,
                                                           dto.Bcc,
                                                           dto.IsBodyHml,
                                                           dto.Attachments);

        public void SendEmail(string from,
                              string to,
                              string subject,
                              string body,
                              string cc = null,
                              string bcc = null,
                              bool isBodyHtml = false,
                              string attachments = null)
        {
            if (string.IsNullOrWhiteSpace(from))
            {
                throw new System.ArgumentException("message", nameof(from));
            }

            if (string.IsNullOrWhiteSpace(to))
            {
                throw new System.ArgumentException("message", nameof(to));
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new System.ArgumentException("message", nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                throw new System.ArgumentException("message", nameof(body));
            }

            var client = new SmtpClient(_configuration.ServerName)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configuration.UserName, _configuration.Password),
                Port = _configuration.Port,
                EnableSsl = _configuration.EnableSSL
            };

            var message = new MailMessage()
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };

            var toCollection = to.Split(';');
            foreach (var item in toCollection)
            {
                message.To.Add(new MailAddress(item));
            }

            if (!string.IsNullOrWhiteSpace(cc))
            {
                var ccCollection = cc.Split(';');
                foreach (var item in ccCollection)
                {
                    message.CC.Add(new MailAddress(item));
                }
            }

            if (!string.IsNullOrWhiteSpace(bcc))
            {
                var bccCollection = bcc.Split(';');
                foreach (var item in bccCollection)
                {
                    message.Bcc.Add(new MailAddress(item));
                }
            }

            if (!string.IsNullOrWhiteSpace(attachments))
            {
                var attachmentCollection = attachments.Split(';');
                foreach (var item in attachmentCollection)
                {
                    message.Attachments.Add(new Attachment(item));
                }
            }

            client.Send(message);
            client.Dispose();
        }
    }
}
