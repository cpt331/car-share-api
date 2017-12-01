//======================================
//
//Name: TestPrincipal.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

using System.Security.Claims;

namespace CarShareApi.Tests.Fakes
{
    public class TestPrincipal : ClaimsPrincipal
    {
        //this class tests the credentials access
        public TestPrincipal(params Claim[] claims) : base(
            new TestIdentity(claims))
        {
        }
    }

    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(params Claim[] claims) : base(claims)
        {
        }
    }
}