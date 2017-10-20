using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CarShareApi.Models
{
    public static class Extensions
    {
        public static string Decode(this string s)
        {
            var data = Convert.FromBase64String(s);
            return Encoding.UTF8.GetString(data);
        }
    }
}