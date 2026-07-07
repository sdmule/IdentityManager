using IdentityManager.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityManager.Authorize
{
    public class AdminWithOver1000DaysHandler : AuthorizationHandler<AdminWithMoreThan1000DaysRequirement>
    {
        private readonly INumberOfDaysForAccount _numberOfDaysForAccount;
        public AdminWithOver1000DaysHandler(INumberOfDaysForAccount numberOfDaysForAccount)
        {
            _numberOfDaysForAccount = numberOfDaysForAccount;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminWithMoreThan1000DaysRequirement requirement)
        {
            if (!context.User.IsInRole(SD.Admin))
            {
                return Task.CompletedTask;
            }

            //The condition is not true means this is an Admin account

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var numberOfDays = _numberOfDaysForAccount.Get(userId);

            if(numberOfDays >= requirement.Days)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
