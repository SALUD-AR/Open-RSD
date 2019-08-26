using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.Communication.Emailing;

namespace Msn.InteropDemo.Web.Emailing
{
    public class EmailGenerator: IEmailGenerator
    {
        private EmailTemplatesWebPath _emailTemplatesWebPath;
        private readonly ILogger<EmailGenerator> _logger;
        private readonly IHostingEnvironment _environment;

        public EmailGenerator(IHostingEnvironment environment,
                              ILogger<EmailGenerator> logger,
                              EmailTemplatesWebPath emailTemplatesWebPath)
        {
            _emailTemplatesWebPath = emailTemplatesWebPath;
            _logger = logger;
            _environment = environment;
        }

        public EmailModel GenerateResetPasswordEmail(string resetPasswordCode,
                                                     string firstName,
                                                     string lastName,
                                                     string fromEmailAddess,
                                                     string toEmailAdrress)
        {
            try
            {

                var templateXmlPath = Path.Combine(_environment.ContentRootPath, _emailTemplatesWebPath.ResetPasswordTemplatePath);
                var doc = new XmlDocument();
                doc.Load(templateXmlPath);

                var el = doc.DocumentElement;
                var nodeSubject = el.ChildNodes[0].ChildNodes[0];
                var nodeBody = el.ChildNodes[1].ChildNodes[0];
                var nodebcc = el.SelectSingleNode("/Email/bcc");

                var msg = new Communication.Emailing.EmailModel
                {
                    Subject = nodeSubject.Value,
                    From = fromEmailAddess,
                    To = toEmailAdrress,
                    IsBodyHml = true
                };


                if (nodebcc != null && nodebcc.ChildNodes[0] != null && nodebcc.ChildNodes[0].Value != null)
                {
                    msg.Bcc = nodebcc.ChildNodes[0].Value;
                }

                var b = new StringBuilder();
                b.Append("<html> <head> </head> <body> <p>");
                var str = nodeBody.Value;

                str = str.Replace("$firstname$", firstName);
                str = str.Replace("$lastname$", lastName);
                str = str.Replace("$UrlAndCode$", _emailTemplatesWebPath.ResetPasswordUrl + "?code=" + resetPasswordCode);

                b.Append(str);
                b.Append("</p> </body></html>");

                msg.Body = b.ToString();

                return msg;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando Email Body.");
                throw;
            }
        }
    }
}
