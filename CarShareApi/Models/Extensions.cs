//======================================
//
//Name: Extensions.cs
//Version: 1.0
//Developer: Steven Innes
//Contributor: 
//
//======================================

using System;
using System.Text;

namespace CarShareApi.Models
{
    public static class Extensions
    {
        public static string Decode(this string s)
        {
            //method converts encoded information in base64
            var data = Convert.FromBase64String(s);
            return Encoding.UTF8.GetString(data);
        }
    }
}