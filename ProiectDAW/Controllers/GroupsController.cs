using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectDAW.Data;
using ProiectDAW.Models;
using System.Security.Claims;

namespace ProiectDAW.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GroupsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // list of all groups available
        [Authorize(Roles = "User,Admin")]
        public IActionResult Index()
        {
            var groups = db.Groups;
            ViewBag.Groups = groups;

            //if (TempData.ContainsKey("message"))
            //{
            //    ViewBag.Message = TempData["message"];
            //}

            return View();
        }

        // details of a group
        [Authorize(Roles = "User,Admin")]
        public IActionResult Show(int id)
        {
            Group group = db.Groups
                .Include(g => g.Posts)
                .Include(g => g.Moderator)
                .Where(gr => gr.Id == id)
                .First();

            SetAccessRights();

            return View(group);
        }

        // add a new group
        public IActionResult New()
        {
            Group group = new Group();

            return View(group);
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult New(Group group)
        {
            // Assuming you have a way to get the current user's ID
            string? currentUserId = _userManager.GetUserId(User);
            group.ModeratorId = currentUserId;

            db.Groups.Add(group);
            db.SaveChanges();

            UserGroup userGroup = new UserGroup
            {
                UserId = currentUserId,
                GroupId = group.Id
            };

            db.UserGroups.Add(userGroup);
            db.SaveChanges();

            TempData["message"] = "Group was added!";

            return RedirectToAction("Show", new { id = group.Id });
        }


        // edit a group
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Edit(int id)
        {
            Group group = db.Groups
                .Where(gr => gr.Id == id)
                .First();

            // SetAccessRights();

            if (group.ModeratorId == _userManager.GetUserId(User))
            {
                return View(group);
            }
            else
            {
                TempData["message"] = "You are not the moderator of this group!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Show", new { id = group.Id });
            }
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Edit(int id, Group requestGroup)
        {
            Group group = db.Groups.Find(id);

            if (ModelState.IsValid)
            {
                if (group.ModeratorId == _userManager.GetUserId(User))
                {
                    group.Name = requestGroup.Name;
                    group.Description = requestGroup.Description;

                    TempData["message"] = "Group was edited!";
                    db.SaveChanges();
                    
                    return RedirectToAction("Show", new { id = group.Id });
                }
                else
                {
                    TempData["message"] = "You are not the moderator of this group!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Show", new { id = group.Id });
                }
            }
            else
            {
                return View(requestGroup);
            }
        }

        // delete a group
        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Delete(int id)
        {
            Group group = db.Groups
                .Where(gr => gr.Id == id)
                .First();

            if ((group.ModeratorId == _userManager.GetUserId(User)) || User.IsInRole("Admin"))
            {
                // delete all user groups connections to this group
                db.UserGroups.RemoveRange(db.UserGroups.Where(ug => ug.GroupId == id));
                // delete all requests to join this group
                db.GroupRequests.RemoveRange(db.GroupRequests.Where(gr => gr.GroupId == id));
                // delete all posts in this group
                db.Posts.RemoveRange(db.Posts.Where(p => p.GroupId == id));

                db.Groups.Remove(group);
                db.SaveChanges();
                TempData["message"] = "Group was deleted!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "You are not the moderator of this group or an administrator!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Show", new { id = group.Id });
            }


        }

        private void SetAccessRights()
        {
            ViewBag.ShowButtons = false;
            if (User.IsInRole("Admin") || User.IsInRole("Editor"))
            {
                ViewBag.ShowButtons = true;
            }

            ViewBag.CurrentUser = _userManager.GetUserId(User);
            ViewBag.IsAdmin = User.IsInRole("Admin");
        }

        /// METHODS FOR OTHER ENTITIES  


        // added by Dani
        [Authorize(Roles = "User,Admin")]
        public IActionResult NewGroupPost(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                string returnUrl = HttpContext.Request.Path; //calea catre pagina curenta
                string loginUrl = $"/Identity/Account/Login?returnUrl={Uri.EscapeDataString(returnUrl)}"; //calea catre pagina de login
                Console.WriteLine(returnUrl.ToString());
                return Redirect(loginUrl);
            }

            Post post = new Post();
            post.GroupId = id;

            TempData["GroupId"] = id;

            return View("/Views/Posts/New.cshtml", post);
        }

    }
}
