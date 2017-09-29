using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.Models
{
    public class Helpers
    {
        public static string GetIssuerUri()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                return "http://localhost:63130";
            }

            return "http://careshareapi-env.hdwwh7zgb3.us-east-1.elasticbeanstalk.com";
            
        }
    }
}