using System;
using System.Net.Mail;

namespace DatabaseBackUpService
{
    public class EmailSender
    {
        public void sendEmail(string to, string cc, string bcc, bool isBodyHtml, string mailSubject, string mailBody)
        {
            try
            {
                MailMessage message = new MailMessage();
                if (string.IsNullOrEmpty(to))
                {
                    throw new ArgumentNullException("Mail to address is null");
                }
                else
                {
                    message.To.Add(to);
                }
                if (!string.IsNullOrEmpty(cc))
                {
                    message.CC.Add(cc);
                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    message.Bcc.Add(bcc);
                }
                message.From = new MailAddress("inbound@snkmdr.com", "UrSqlBackup");
                message.IsBodyHtml = isBodyHtml;
                message.Subject = mailSubject;
                message.Body = mailBody;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient("mail.snkmdr.com");
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("inbound@snkmdr.com", "Snkmdr123@@");
                client.Port = 27;
                client.EnableSsl = false;

                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Email Send - {0}", ex.InnerException);
            }
        }
    }
}
