using System;
using CarShareApi.Models.Providers;

namespace CarShareApi.Tests.Fakes
{
    public class FakeEmailProvider : IEmailProvider
    {
        public void Send(string email, string firstName, string otpRecord)
        {
            //do nothing
            Console.WriteLine($"email: {email}");
            Console.WriteLine($"firstName: {firstName}");
            Console.WriteLine($"otpRecord: {otpRecord}");
        }

        public void Send(string email, string subject, 
            string title, string body, string footer)
        {
            //do nothing
            Console.WriteLine($"email: {email}");
            Console.WriteLine($"subject: {subject}");
            Console.WriteLine($"title: {title}");
            Console.WriteLine($"body: {body}");
            Console.WriteLine($"footer: {footer}");
        }
    }
}