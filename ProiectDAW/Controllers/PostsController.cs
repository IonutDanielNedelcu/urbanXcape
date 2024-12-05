using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var posts = db.Posts.Include("User")
                                .Include("Comments")
                                .OrderByDescending(x => x.Date);

            ViewBag.Posts = posts;
            return View();
        }

        public IActionResult Show(int Id=3)
        {
            Post post = db.Posts.Find(Id);
            return View(post);
        }

        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(Post post)
        {
            try
            {
                post.Date = DateTime.Now;
                post.Likes = 0;
                post.UserId = _userManager.GetUserId(User);
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        public IActionResult Edit(int Id)
        {
            Post post = db.Posts.Find(Id);
            return View(post);
        }
        [HttpPost]
        public IActionResult Edit(int Id, Post editedPost)
        {
            Post post = db.Posts.Find(Id);

            try
            {
                post.Description = editedPost.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", post.Id);
            }
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Post post = db.Posts.Find(Id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
