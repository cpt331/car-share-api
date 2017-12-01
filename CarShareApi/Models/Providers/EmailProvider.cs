//======================================
//
//Name: EmailProvider.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using System.Net;
using System.Net.Mail;

namespace CarShareApi.Models.Providers
{
    //This provider allows an email to be sent through AWS SES service
    //The Welcomemailer creates the neccessary set up to pass SMTP credentials
    //before creating the neccesary method to process the message and content

    public interface IEmailProvider
    {
        void Send(string email, string subject, string title, 
            string body, string footer);
    }

    public class WelcomeMailer : IEmailProvider
    {
        public WelcomeMailer(string smtpUsername, string smtpPassword,
            string smtpServer, int smtpPort)
        {
            //Method to pass the SMTP credentials from a backend call
            //Which protects the SMTP credentials from visibility
            SmtpUsername = smtpUsername;
            SmtpPassword = smtpPassword;
            SmtpServer = smtpServer;
            SmtpPort = smtpPort;
        }

        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }

        public void Send(string email, string subject, string title, 
            string body, string footer)
        {
            //This method passes content to form the email body
            //Below are base inputs to form the email
            var from = "shawn.burriss@gmail.com";
            var fromName = "Ewebah Admin";
            string emailSubject = subject;
            var emailBody =
                $"<h1>{title}</h1>" +
                $"<p>{body}" +
                $"<p>{footer}";

            //This compiles the above inputs to create a message object
            var message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from, fromName),
                Subject = emailSubject,
                Body = emailBody
            };
            message.To.Add(new MailAddress(email));

            //Create a new smtp client and pass the protected crednetials
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
                //attempt to send message otherwise catch error
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