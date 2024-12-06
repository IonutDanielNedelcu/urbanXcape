using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ProiectDAW.Data;
using ProiectDAW.Models;
using SQLitePCL;
using System.Reflection.Metadata;
using System.Security.Principal;

namespace ProiectDAW.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env; //pt poza
        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = env;
        }
        public IActionResult Index()
        {
            var posts = db.Posts.Include("User")
                                .Include("Comments")
                                .OrderByDescending(x => x.Date);

            ViewBag.Posts = posts;
            //afisez mesajul din delete
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Msg = TempData["message"];
            }
            return View();
        }

        public IActionResult Show(int Id)
        {
            Post post = db.Posts.Include("Comments")
                                .Where(post => post.Id == Id)
                                .First();
            return View(post);
        }
        [HttpPost]
        public IActionResult Show([FromForm] Comment comment) //adugare comentarii
        {
            comment.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Posts/Show/" + comment.PostId);
            }
            else
            {
                Post post = db.Posts.Include("Comments")
                                    .Where(post => post.Id == comment.PostId)
                                    .First();
                Console.WriteLine("Eroare la adaugarea comentariului"); 
                //return Redirect("/Articles/Show/" + comm.ArticleId);
                return View(post);
            }
        }

        public IActionResult New()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login"); // Controllerul și metoda Login
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> New(Post post, IFormFile Image)
        {
            if (Image != null && Image.Length > 0)
            {
                // Verificăm extensia
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov" };
                var fileExtension = Path.GetExtension(Image.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    Console.WriteLine("Invalid file extension");
                    ModelState.AddModelError("Image", "Fișierul trebuie să fie o imagine (jpg, jpeg, png, gif) sau un video (mp4, mov).");
                    return View(post);
                }

                // Cale stocare
                var storagePath = Path.Combine(_env.WebRootPath, "images", Image.FileName);
                var databaseFileName = "/images/" + Image.FileName;

                // Salvare fișier
                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }
                ModelState.Remove(nameof(post.Image));
                post.Image = databaseFileName;
            }

            try
            {
                post.Date = DateTime.Now;
                post.Likes = 0;
                post.UserId = _userManager.GetUserId(User);
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la adăugarea postării: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                // Dacă nu este autentificat, redirecționează la Login
                
                return View(post);
            }
        }

        public IActionResult Edit(int Id)
        {
            Post post = db.Posts.Find(Id);
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int Id, Post editedPost, IFormFile Image)
        {
            Post post = db.Posts.Find(Id);

            if (Image != null && Image.Length > 0)
            {
                // Verificăm extensia
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov" };
                var fileExtension = Path.GetExtension(Image.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    Console.WriteLine("Invalid file extension");
                    ModelState.AddModelError("Image", "Fișierul trebuie să fie o imagine (jpg, jpeg, png, gif) sau un video (mp4, mov).");
                    return View(post);
                }

                // Cale stocare
                var storagePath = Path.Combine(_env.WebRootPath, "images", Image.FileName);
                var databaseFileName = "/images/" + Image.FileName;

                // Salvare fișier
                using (var fileStream = new FileStream(storagePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }
                ModelState.Remove(nameof(post.Image));
                post.Image = databaseFileName;
            }

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
            Post post = db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == Id);
            
            //stergem comentariile postarii
            db.Comments.RemoveRange(post.Comments);

            db.Posts.Remove(post);
            db.SaveChanges();

            TempData["message"] = "Postarea a fost stearsa!";
            return RedirectToAction("Index");
        }
    }
}
