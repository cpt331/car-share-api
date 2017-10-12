﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace CarShareApi.Models
{
    public static class Mail
    {
        public static void SMTPMailer(string email)
        {
            // This address must be verified with Amazon SES.
            const String FROM = "shawn.burriss@gmail.com";
            const String FROMNAME = "Shawn Burriss";
            // if still in the sandbox, this address must be verified.
            const String TO = "shawn.burriss@gmail.com";
            const String SMTP_USERNAME = "AKIAIX7BCPEWPOAEY2OA";
            const String SMTP_PASSWORD = "An9t0AOtJpQCdMEbDRCZdCWWXnliKUAB2L5cRemj+xqx";
            const String HOST = "email-smtp.us-east-1.amazonaws.com";
            const int PORT = 587;

            // (Optional) the name of a configuration set to use for this message.
            // If you comment out this line, you also need to remove or comment out
            // the "X-SES-CONFIGURATION-SET" header on line 59, below.
            //const String CONFIGSET = "ConfigSet";

            // The subject line of the email
            const String SUBJECT =
                "Ewebah - Welcome to the App!";

            // The body of the email
            const String BODY =
                "<h1>Welcome to Ewebah</h1>" +
                "<p>Thank you for registering with  " +
                "<a href='ewebah.s3-website-us-east-1.amazonaws.com'>Ewebah</a>. Your account is set up!" +
                "Let's get driving.</p>";

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(new MailAddress(TO));
            message.Subject = SUBJECT;
            message.Body = BODY;
            // Comment or delete the next line if you are not using a configuration set
            //message.Headers.Add("X-SES-CONFIGURATION-SET", CONFIGSET);

            // Create and configure a new SmtpClient
            SmtpClient client =
                new SmtpClient(HOST, PORT);
            // Pass SMTP credentials
            client.Credentials =
                new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            // Enable SSL encryption
            client.EnableSsl = true;

            // Send the email. 
            try
            {
                Console.WriteLine("Attempting to send email...");
                client.Send(message);
                Console.WriteLine("Email sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("The email was not sent.");
                Console.WriteLine("Error message: " + ex.Message);
            }

            // Wait for a key press so that you can see the console output
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}