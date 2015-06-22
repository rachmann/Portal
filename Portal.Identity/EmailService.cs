using System.Net.Mail;
using System.Web.Mail;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Identity
{

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var emailType = "gmail";

            emailType = ConfigurationManager.AppSettings["MailType"];
            var fromEmail = ConfigurationManager.AppSettings["MailFromEmailAdmin"];
            var emailCc = ConfigurationManager.AppSettings["MailCCEmail"];

            emailType = emailType.ToLower();
            switch (emailType)
            {
                case "mailgun":
                    //using (var mailer = new MailGunClient(ConfigurationManager.AppSettings["MailGunApiKey"], ConfigurationManager.AppSettings["MailGunURL"]))
                    //{
                    //    var msg = new MailGunMessage {From = fromEmail, Subject = message.Subject, Text = message.Body};

                    //    msg.AddTo(message.Destination);
                    //    if (!string.IsNullOrWhiteSpace(emailCc))
                    //    {
                    //        msg.AddCc(emailCc);
                    //    }

                    //    mailer.SendMessage(msg);
                    //}
                    break;
                default:
                    try
                    {
                        using (var smtpServer = new SmtpClient("smtp.gmail.com"))
                        {
                            using (var mail = new System.Net.Mail.MailMessage())
                            {
                                var pwd = ConfigurationManager.AppSettings["MailGMailPassword"];
                                var email = ConfigurationManager.AppSettings["MailFromEmailAdmin"];
                                mail.From = new MailAddress(email);
                                mail.Subject = message.Subject;
                                mail.Body = message.Body;

                                mail.To.Add(message.Destination);

                                smtpServer.Port = 587;
                                smtpServer.Credentials = new System.Net.NetworkCredential(email, pwd);
                                smtpServer.EnableSsl = true;

                                smtpServer.Send(mail);
                            }
                        }
                    }
                    catch  // (Exception ex)
                    {
                       // TODO: add logging
                    }
                    break;
            }

            return Task.FromResult(0);
        }
    }
}
