using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.ViewModels.Users
{
    public class PasswordResetResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string[] Errors { get; set; }
    }
}