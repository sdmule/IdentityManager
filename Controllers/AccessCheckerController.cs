using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IdentityManager.Controllers
{
    //This is dummy controller to check if the user has access to the IdentityManager area.
    //If the user does not have access, they will be redirected to the login page.
    [Authorize]
    public class AccessCheckerController : Controller
    {
        //Anyone can access this action, even if they are not logged in.
        [AllowAnonymous]
        public IActionResult AllAccess()
        {
            return View();
        }

        //Anyone that is logged in can access this action.
        public IActionResult AuthorizedAccess()
        {
            return View();
        }

        [Authorize(Policy = "AdminAndUser")]
        //Account with role of user and admin(both roles must be there to user) can access this action.
        public IActionResult UserANDAdminRoleAccess()
        {
            return View();
        }

        [Authorize(Roles = $"{SD.Admin},{SD.User}")]//Here it is OR condition
        //Account with role of user or admin can access this action.
        public IActionResult UserOrAdminRoleAccess()
        {
            return View();
        }


        /*Policy-Based Authorization is an authorization mechanism introduced in ASP.NET Core that allows developers to define 
        reusable authorization rules(policies).
        A policy can contain one or more requirements such as roles, claims, or custom business logic.
        Controllers or actions can then enforce these rules using the [Authorize(Policy = "...")] attribute, 
        making authorization centralized, reusable, and easier to maintain.*/
        //In almost all ASP.NET Core applications, authorization policies are configured during application startup, which is typically in Program.cs.
        [Authorize(Policy = "Admin")]
        //Account with role of admin can access this action.
        public IActionResult AdminRoleAccess()
        {
            return View();
        }

        [Authorize(Policy = "AdminRole_CreateClaim")]
        //Account with admin role and create claim can access this action.
        public IActionResult Admin_CreateAccess()
        {
            return View();
        }

        //Account with admin role and (create & Edit & Delete) claim can access this action.
        public IActionResult Admin_Create_Edit_DeleteAccess()
        {
            return View();
        }
    }
}
