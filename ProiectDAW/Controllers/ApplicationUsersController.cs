using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var applicationUsers = db.ApplicationUsers;
            ViewBag.ApplicationUsers = applicationUsers;

            return View();
        }

        public IActionResult Show(string id)
        {
            ApplicationUser? applicationUser = db.ApplicationUsers
                .Include(u => u.Posts)
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .FirstOrDefault(u => u.Id == id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            if (applicationUser.Id == _userManager.GetUserId(User))
            {
                ViewBag.IsCurrentUser = true;
            }
            else
            {
                ViewBag.IsCurrentUser = false;
            }
            return View(applicationUser);
        }


    }
}
