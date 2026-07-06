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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole roleObj) //upsert means update or insert
        {
            if (await _roleManager.RoleExistsAsync(roleObj.Name))
            {
                //Error message
            }
            if (string.IsNullOrEmpty(roleObj.NormalizedName))
            {
                // Create new role
                await _roleManager.CreateAsync(new IdentityRole() { Name = roleObj.Name });
                TempData[SD.Success] = "Role created successfully";
            }
            else
            {
                // Update existing role
                var objFromDb = _db.Roles.FirstOrDefault(r => r.Id == roleObj.Id);
                objFromDb.Name = roleObj.Name;
                objFromDb.NormalizedName = roleObj.Name.ToUpper();
                var result = await _roleManager.UpdateAsync(objFromDb);
                //var result = _db.Roles.Update(objFromDb); //This is an alternative way to update the role, but using RoleManager is preferred for Identity roles.
                TempData[SD.Success] = "Role updated successfully";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string roleId)
        {
            var objFromDb = _db.Roles.FirstOrDefault(r => r.Id == roleId);
            if (objFromDb != null)
            {
                var userRolesForThisRole = _db.UserRoles.Where(u => u.RoleId == roleId).Count();
                if(userRolesForThisRole > 0)
                {
                    TempData[SD.Error] = "Role cannot be deleted as it is assigned to one or more users.";
                    return RedirectToAction(nameof(Index));
                }

                var result = await _roleManager.DeleteAsync(objFromDb);
                TempData[SD.Success] = "Role deleted successfully";
            }
            TempData[SD.Error] = "Role not found.";
            return RedirectToAction(nameof(Index));
        }
    }
}
