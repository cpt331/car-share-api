using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarShareApi.ViewModels
{
    public class UserViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}