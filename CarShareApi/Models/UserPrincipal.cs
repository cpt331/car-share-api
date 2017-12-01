//======================================
//
//Name: UserPrincipal.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: 
//
//======================================

using System.Security.Claims;

namespace CarShareApi.Models
{
    public class UserPrincipal : ClaimsPrincipal
    {
        //this module allows parsing user account details to be used
        //to identify the users main detals as well as their role

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