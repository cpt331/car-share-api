//======================================
//
//Name: LogonRequest.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.ComponentModel.DataAnnotations;

namespace CarShareApi.Models.ViewModels
{
    public class LogonRequest
    {
        //Logon request provides the necessary Fields for a user to be able to 
        //login using an email and password.
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
