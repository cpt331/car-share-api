﻿//======================================
//
//Name: Encryption.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: 
//
//======================================

using System.Security.Cryptography;
using System.Text;

namespace CarShareApi.Models
{
    public static class Encryption
    {
        //encrypt password 
        //https://stackoverflow.com/a/212526
        public static string EncryptString(string str)
        {
            var data = Encoding.ASCII.GetBytes(str);
            data = new SHA256Managed()
                .ComputeHash(data);
            var hash = Encoding.ASCII.GetString(data);
            return hash;
        }
    }
}