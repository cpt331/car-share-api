using System;
using System.Net;
using System.Net.Mail;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Providers
{
    public interface IEmailProvider
    {
        void Send(string email, string subject, string title, string body, string footer);
    }

    public class WelcomeMailer : IEmailProvider
    {
        public WelcomeMailer(string smtpUsername, string smtpPassword,
            string smtpServer, int smtpPort)
        {
            SmtpUsername = smtpUsername;
            SmtpPassword = smtpPassword;
            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
        }

        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }

        public void Send(string email, string subject, string title, string body, string footer)
        {
            var from = "shawn.burriss@gmail.com";
            var fromName = "Ewebah Admin";
            string emailSubject = subject;
            var emailBody =
                $"<h1>{title}</h1>" +
                $"<p>{body}" +
                $"<p>{footer}";


            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from, fromName),
                Subject = emailSubject,
                Body = emailBody
            };
            message.To.Add(new MailAddress(email));

            var client =
                new SmtpClient(SmtpServer, SmtpPort)
                {
                    // Pass SMTP credentials and enable SSL encryption
                    Credentials =
                        new NetworkCredential(SmtpUsername, SmtpPassword),
                    EnableSsl = true
                };
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
            }
        }
    }
}