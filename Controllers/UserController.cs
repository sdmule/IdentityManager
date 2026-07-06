using IdentityManager.Data;
using IdentityManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userList = _db.ApplicationUser.ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in userList)
            {
                var user_Role = userRole.FirstOrDefault(u => u.UserId == user.Id);
                if (user_Role == null)
                {
                    user.RoleId = "none";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == user_Role.RoleId).Name;
                }
            }
            return View(userList);
        }
    }
}
