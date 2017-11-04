using System;
using System.Net;
using System.Net.Mail;

namespace CarShareApi.Models.Providers
{
    public interface IEmailProvider
    {
        void Send(string email, string firstName, string otpRecord);
    }

    public class WelcomeMailer : IEmailProvider
    {
        private string email;
        private string firstName;
        private string otpRecord;

        public WelcomeMailer(string smtpUsername, string smtpPassword, string smtpServer, int smtpPort)
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

        public void Send(string email, string firstName, string otpRecord)
        {
            this.email = email;
            this.firstName = firstName;
            this.otpRecord = otpRecord;

            var from = "shawn.burriss@gmail.com";
            var fromName = "Ewebah Admin";
            var subject = "Ewebah - Welcome to the App!";
            var body =
                $"<h1>Welcome to Ewebah, {firstName}!</h1>" +
                "<p>Thank you for registering with  " +
                "<a href='ewebah.s3-website-us-east-1.amazonaws.com'>Ewebah</a>. Your account is set up! " +
                "<p>Your activation key to finalise your account is: " +
                $"<p><b>{otpRecord}</b>" +
                "<p>Are you ready to ride with us?";

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
                    Credentials = new NetworkCredential(SmtpUsername, SmtpPassword),
                    EnableSsl = true
                };
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("The email was not sent.");
                //Console.WriteLine("Error message: " + ex.Message);
            }
        }
    }
}