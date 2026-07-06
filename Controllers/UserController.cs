using IdentityManager.Data;
using IdentityManager.Models;
using IdentityManager.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
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
                    user.Role = "none";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == user_Role.RoleId).Name;
                }
            }
            return View(userList);
        }

        public async Task<IActionResult> ManageRole(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            List<string> existingUserRoles = await _userManager.GetRolesAsync(user) as List<string>;
            var model = new RolesViewModel()
            {
                User = user
            };

            foreach (var role in _roleManager.Roles)
            {
                RoleSelection roleSelection = new()
                {
                    RoleName = role.Name,
                };

                if(existingUserRoles.Any(c => c == role.Name))
                {
                    roleSelection.IsSelected = true;
                }
                else
                {
                    roleSelection.IsSelected = false;
                }
                model.RolesList.Add(roleSelection);
            }
            return View(model);
        }
    }
}
