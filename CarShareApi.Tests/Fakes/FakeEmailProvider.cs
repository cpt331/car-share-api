//======================================
//
//Name: FakeEmailProvider.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System;
using CarShareApi.Models.Providers;

namespace CarShareApi.Tests.Fakes
{
    public class FakeEmailProvider : IEmailProvider
    {
        //email testing by implementing the smtp mailer
        public void Send(string email, string firstName, string otpRecord)
        {
            //write records to console for testing
            //this would normally be the email content
            Console.WriteLine($"email: {email}");
            Console.WriteLine($"firstName: {firstName}");
            Console.WriteLine($"otpRecord: {otpRecord}");
        }

        public void Send(string email, string subject, 
            string title, string body, string footer)
        {
            //write records to console for testing
            //this would normally be the email header content
            Console.WriteLine($"email: {email}");
            Console.WriteLine($"subject: {subject}");
            Console.WriteLine($"title: {title}");
            Console.WriteLine($"body: {body}");
            Console.WriteLine($"footer: {footer}");
        }
    }
}