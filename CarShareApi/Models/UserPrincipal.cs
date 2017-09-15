using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace CarShareApi.Models
{
    public class UserPrincipal : ClaimsPrincipal
    {
        public UserPrincipal(ClaimsPrincipal principal): base(principal)
        {
        }

        public int? Id
        {
            get
            {
                var idString = this.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (string.IsNullOrWhiteSpace(idString))
                {
                    return null;
                }
                return int.Parse(idString);
            }
        }

        public string Name => this.FindFirst(ClaimTypes.Name)?.Value;

        public string Email => this.FindFirst(ClaimTypes.Email)?.Value;

        public string Role => this.FindFirst(ClaimTypes.Role)?.Value;
    }
}