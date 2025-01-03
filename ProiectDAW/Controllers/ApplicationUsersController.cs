using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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

        [Authorize(Roles = "User,Admin")]
        public IActionResult Index()
        {
            var applicationUsers = db.ApplicationUsers
                .Include(u => u.Followers)
                .Where(u => u.Id != _userManager.GetUserId(User))
                .AsQueryable();

            var search = "";

            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();

                List<string> usersIds = db.ApplicationUsers
                    .Where(u => (u.FirstName.Contains(search) || u.LastName.Contains(search)
                    || u.UserName.Contains(search)) && (u.Id != _userManager.GetUserId(User)))
                    .Select(u => u.Id)
                    .ToList();

                applicationUsers = db.ApplicationUsers
                    .Where(u => usersIds.Contains(u.Id))
                    .Include(u => u.Followers)
                    .OrderBy(user => user.UserName);
            }

            //verificam daca userul curent a dat like la postari
            foreach (var appUser in applicationUsers)
            {
                FollowRequest? followRequest = appUser.Followers.FirstOrDefault(u => u.FollowerId == _userManager.GetUserId(User));
                if (followRequest != null)
                {
                    if (followRequest.Accepted == false)
                        TempData[appUser.Id.ToString()] = "1";
                    else if(followRequest.Accepted == true)
                        TempData[appUser.Id.ToString()] = "2";
                }
            }


            ViewBag.IsAdmin = _userManager.IsInRoleAsync(_userManager.FindByIdAsync(_userManager.GetUserId(User)).Result, "Admin").Result;
            ViewBag.SearchString = search;
            ViewBag.ApplicationUsers = applicationUsers
                .Include(u => u.Followers)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.FirstName,
                    u.LastName,
                    u.Description,
                    u.ProfilePic,
                    FollowersCount = u.Followers.Where(f => f.Accepted == true).Count()
                }).ToList();

            return View();
        }

        [Authorize(Roles = "User,Admin")]
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
                ViewBag.Requests = db.FollowRequests
                    .Include(f => f.Follower)
                    .Where(f => f.FollowedId == applicationUser.Id && f.Accepted == false)
                    .ToList();
                ViewBag.IsCurrentUser = true;
            }
            else
            {
                ViewBag.IsCurrentUser = false;
                ViewBag.IsFollowing = applicationUser.Followers.Any(f => f.FollowerId == _userManager.GetUserId(User) && f.Accepted == true);
            }
            return View(applicationUser);
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Follow(string id)
        {
            ApplicationUser? applicationUser = db.ApplicationUsers.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            bool acceptable = (!applicationUser.Privacy);


            FollowRequest followRequest = new FollowRequest
            {
                FollowerId = _userManager.GetUserId(User),
                FollowedId = id,
                Date = DateTime.Now,
                Accepted = acceptable
            };
            db.FollowRequests.Add(followRequest);
            db.SaveChanges();
            return Redirect("/ApplicationUsers/Index/");
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Unfollow(string id)
        {
            ApplicationUser? applicationUser = db.ApplicationUsers.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            FollowRequest followRequest = db.FollowRequests.FirstOrDefault(f => f.FollowerId == _userManager.GetUserId(User) && f.FollowedId == id);
            db.FollowRequests.Remove(followRequest);
            db.SaveChanges();
            return Redirect("/ApplicationUsers/Index/");
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Accept(string id)
        {
            ApplicationUser? applicationUser = db.ApplicationUsers.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            FollowRequest followRequest = db.FollowRequests.FirstOrDefault(f => f.FollowerId == id && f.FollowedId == _userManager.GetUserId(User));
            followRequest.Accepted = true;
            db.SaveChanges();
            return Redirect("/ApplicationUsers/Index/");
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Reject(string id)
        {
            ApplicationUser? applicationUser = db.ApplicationUsers.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            FollowRequest followRequest = db.FollowRequests.FirstOrDefault(f => f.FollowerId == id && f.FollowedId == _userManager.GetUserId(User));
            db.FollowRequests.Remove(followRequest);
            db.SaveChanges();
            return Redirect("/ApplicationUsers/Index/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            ApplicationUser? applicationUser = db.ApplicationUsers.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            db.ApplicationUsers.Remove(applicationUser);
            db.SaveChanges();
            return Redirect("/ApplicationUsers/Index/");
        }
    }

}
