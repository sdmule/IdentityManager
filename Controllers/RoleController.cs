using IdentityManager.Data;
using IdentityManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)  
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _db.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Upsert(string roleId) //upsert means update or insert
        {
            if (string.IsNullOrEmpty(roleId))
            {
                // Create new role
                return View();
            }
            else
            {
                // Update existing role
                var objFromDb = _db.Roles.FirstOrDefault(r => r.Id == roleId);
                return View(objFromDb);
            }
        }
    }
}
