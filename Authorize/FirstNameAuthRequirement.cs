using Microsoft.AspNetCore.Authorization;

namespace IdentityManager.Authorize
{
    public class FirstNameAuthRequirement : IAuthorizationRequirement
    {
        public FirstNameAuthRequirement(string firstName)
        {
            FirstName = firstName;  
        }
        public string FirstName { get; set; }
    }
}
