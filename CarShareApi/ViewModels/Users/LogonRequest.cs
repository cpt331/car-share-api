using System.ComponentModel.DataAnnotations;

namespace CarShareApi.Models.ViewModels
{
    public class LogonRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
