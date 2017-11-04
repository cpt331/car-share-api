using System;
using System.Net;
using System.Net.Mail;
using CarShareApi.Models.Repositories.Data;

namespace CarShareApi.Models.Providers
{
    public interface IEmailProvider
    {
        void Send(string email, string firstName, string otpRecord,
            Template emailTemplate);
    }

    public class WelcomeMailer : IEmailProvider
    {
        private string email;
        private string firstName;
        private string otpRecord;

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

        public void Send(string email, string firstName, string otpRecord,
            Template emailTemplate)
        {
            Console.WriteLine("Attempting to run email service");
            Console.WriteLine($"Subject : {emailTemplate.Subject}");
            Console.WriteLine($"Body : {emailTemplate.Body}");
            Console.WriteLine($"Footer : {emailTemplate.Footer}");

            var from = "shawn.burriss@gmail.com";
            var fromName = "Ewebah Admin";
            var subject = emailTemplate.Subject;
            var body =
                $"<h1>{emailTemplate.Title}, " + $"{firstName}!</h1>" +
                $"<p>{emailTemplate.Body}  " +
                "<p>Your activation key to finalise your account is: " +
                $"<p><b>{otpRecord}</b>" +
                $"<p>{emailTemplate.Footer}";

            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from, fromName),
                Subject = subject,
                Body = body
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