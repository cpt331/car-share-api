using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.Models
{
    public static class Encryption
    {
        //encrypt password 
        //https://stackoverflow.com/a/212526
        public static string EncryptString(string str)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(str);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            String hash = System.Text.Encoding.ASCII.GetString(data);
            return hash;
        }
    }
}