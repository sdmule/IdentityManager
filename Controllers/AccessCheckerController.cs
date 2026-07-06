using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize(Roles = $"{SD.Admin},{SD.User}")]//Here it is OR condition
        //Account with role of user or admin can access this action.
        public IActionResult UserOrAdminRoleAccess()
        {
            return View();
        }

        [Authorize(Roles = SD.Admin)]
        //Account with role of admin can access this action.
        public IActionResult AdminRoleAccess()
        {
            return View();
        }

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
