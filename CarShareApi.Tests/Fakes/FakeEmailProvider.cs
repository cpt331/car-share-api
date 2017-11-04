using CarShareApi.Models.Providers;

namespace CarShareApi.Tests.Fakes
{
    public class FakeEmailProvider : IEmailProvider
    {
        public void Send(string email, string firstName, string otpRecord)
        {
            //do nothing
        }
    }
}