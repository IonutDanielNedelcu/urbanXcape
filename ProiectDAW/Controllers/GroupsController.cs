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

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View();
        }

        // details of a group
        [Authorize(Roles = "User,Admin")]
        public IActionResult Show(int id)
        {
            Group group = db.Groups
                .Include("Posts")
                .Include("UserGroups").Include("UserGroups.User")
                .Where(gr => gr.Id == id)
                .First();

            return View(group);
        }

        // add a new group
        public IActionResult New()
        {
            Group group = new Group();

            return View(group);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult New(Group group)
        {
            // Assuming you have a way to get the current user's ID
            string currentUserId = _userManager.GetUserId(User);
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
        public IActionResult Edit(int id)
        {
            Group group = db.Groups.Find(id);

            return View(group);
        }

        [HttpPost]
        public IActionResult Edit(int id, Group requestGroup)
        {
            Group group = db.Groups.Find(id);

            group.ModeratorId = requestGroup.ModeratorId;

            if (ModelState.IsValid)
            {
                group.Name = requestGroup.Name;
                group.Description = requestGroup.Description;
                
                TempData["message"] = "Group was edited!";
                db.SaveChanges();
                return RedirectToAction("Show", new { id = group.Id });
            }
            else
            {
                return View(requestGroup);
            }
        }

        // delete a group
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Group group = db.Groups.Find(id);

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
    }
}
