using System.Security.Claims;

namespace CarShareApi.Models
{
    public class UserPrincipal : ClaimsPrincipal
    {
        public UserPrincipal(ClaimsPrincipal principal) : base(principal)
        {
        }

        public int? Id
        {
            get
            {
                var idString = FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (string.IsNullOrWhiteSpace(idString))
                    return null;
                return int.Parse(idString);
            }
        }

        public string Name => FindFirst(ClaimTypes.Name)?.Value;

        public string Email => FindFirst(ClaimTypes.Email)?.Value;

        public string Role => FindFirst(ClaimTypes.Role)?.Value;
    }
}