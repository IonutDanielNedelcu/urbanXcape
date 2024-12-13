using Microsoft.AspNetCore.Authorization;
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
                                .Where(p => p.GroupId == null)
                                .OrderByDescending(x => x.Date);

            ViewBag.Posts = posts;
            //afisez mesajul din delete
            if (TempData.ContainsKey("msg"))
            {
                ViewBag.Msg = TempData["msg"];
                ViewBag.MsgType = TempData["msgType"];
            }
            return View();
        }

        public IActionResult Show(int Id)
        {
            Post post = db.Posts.Include("Comments")
                                .Include("User")
                                .Include("Comments.User")    
                                .Where(post => post.Id == Id)
                                .First();
            ViewBag.Buttons = false;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Buttons = true;
            }

            return View(post);
        }
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Show([FromForm] Comment comment) //adugare comentarii
        {
            if (!User.Identity.IsAuthenticated)
            {
                string returnUrl = HttpContext.Request.Path; //calea catre pagina curenta
                string loginUrl = $"/Identity/Account/Login?returnUrl={Uri.EscapeDataString(returnUrl)}"; //calea catre pagina de login
                Console.WriteLine(returnUrl.ToString());
                return Redirect(loginUrl);
            }


            comment.Date = DateTime.Now;
            
            // preluam Id-ul utilizatorului care posteaza comentariul
            comment.UserId = _userManager.GetUserId(User);
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

        [Authorize(Roles = "User,Admin")]
        public IActionResult New()
        {
            if (!User.Identity.IsAuthenticated)
            {
                string returnUrl = HttpContext.Request.Path; //calea catre pagina curenta
                string loginUrl = $"/Identity/Account/Login?returnUrl={Uri.EscapeDataString(returnUrl)}"; //calea catre pagina de login
                Console.WriteLine(returnUrl.ToString());
                return Redirect(loginUrl);
            }

            Post post = new Post();

            return View(post);
        }

        [Authorize(Roles = "User,Admin")]
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

            ModelState.Remove(nameof(post.Image));   //elimina eroarea de validare a pozei

            if (string.IsNullOrEmpty(post.Description) && (Image == null || Image.Length == 0))
            {
                ModelState.AddModelError("", "Cel puțin un câmp trebuie să fie completat (Description sau Image).");
                return View(post);
            }

            post.Date = DateTime.Now;
            post.Likes = 0;
            post.UserId = _userManager.GetUserId(User);
            
            if(ModelState.IsValid)
            {
                
                db.Posts.Add(post);
                db.SaveChanges();

                TempData["msg"] = "Postarea a fost adaugata!";
                TempData["msgType"] = "alert-success"; //clasa de bootstrap pt mesaj de succes(verde)
                return RedirectToAction("Index");
            }
            else
            {
                // Dacă nu este autentificat, redirecționează la Login
                return View(post);
            }
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int Id) //doar userul care a postat poate edita
        {
            Post post = db.Posts.Find(Id);

            if (post.UserId == _userManager.GetUserId(User)) //verificam daca userul curent este cel care a postat
            {                                                //User.IsInRole("Admin") - verifica daca userul este admin
                return View(post);
            }
            else
            {
                TempData["msg"] = "Nu aveți dreptul să editați această postare!";
                TempData["msgType"] = "alert-danger"; //clasa de bootstrap pt mesaj de eroare(rosu)
                return RedirectToAction("Index");
            }
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
    

            ModelState.Remove(nameof(post.Image));
            //verificam daca cel putin un camp este completat (sau daca posarea are deja imagine sau descriere)
            if (string.IsNullOrEmpty(editedPost.Description) && string.IsNullOrEmpty(post.Description) && (Image == null || Image.Length == 0) && (post.Image == null || post.Image.Length == 0))
            {
                ModelState.AddModelError("", "Cel puțin un câmp trebuie să fie completat (Description sau Image).");
                return View(editedPost);
            }

            if (ModelState.IsValid)
            {
                if (post.UserId == _userManager.GetUserId(User)) //verificam daca userul curent este cel care a postat
                {                                                //User.IsInRole("Admin") - verifica daca userul este admin
                    post.Description = editedPost.Description;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msg"] = "Nu aveți dreptul să editați această postare!";
                    TempData["msgType"] = "alert-danger"; //clasa de bootstrap pt mesaj de eroare(rosu)
                    return RedirectToAction("Index");
                }

            }
            else
            {
                return View(editedPost); //editedPost ca a se pastreze datele introduse
            }
        }
        
        public IActionResult DeleteImage(int Id)
        {

            Post post = db.Posts.Find(Id);
            if (post.Image != null)
            {
                post.Image = null;
                db.SaveChanges();
                Console.WriteLine("Imaginea a fost stearsa!");
            }
            else
            {
                Console.WriteLine("Nu exista imagine de sters!");
            }
            return View("Edit", post);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Post post = db.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == Id);

            if (post.UserId == _userManager.GetUserId(User))
            {
                //stergem comentariile postarii
                db.Comments.RemoveRange(post.Comments);

                db.Posts.Remove(post);
                db.SaveChanges();

                TempData["msg"] = "Postarea a fost stearsa!";
                TempData["msgType"] = "alert-danger"; //clasa de bootstrap pt mesaj de eroare(rosu)
            }
            else
            {
                TempData["msg"] = "Nu aveți dreptul să ștergeți această postare!";
                TempData["msgType"] = "alert-danger"; //clasa de bootstrap pt mesaj de eroare(rosu)
            }
            return RedirectToAction("Index");
        }
    }
}
