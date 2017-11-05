using System.Diagnostics;

namespace CarShareApi.Models
{
    public class Helpers
    {
        public static string GetIssuerUri()
        {
            //provides resources for debugging and help
            if (Debugger.IsAttached)
                return "http://localhost:63130";

            return
                "https://ewebahapi.azurewebsites.net/";
        }
    }
}