using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using ProiectDAW.Data;
using ProiectDAW.Models;
using SQLitePCL;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Web;

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
                                .Include("PostLikes")
                                .Include("Location")
                                .OrderByDescending(x => x.Date);

            

            ViewBag.Posts = posts;

            //verificam daca userul curent a dat like la postari
            foreach (var post in posts)
            {
                PostLike postlike = post.PostLikes.FirstOrDefault(p => p.UserId == _userManager.GetUserId(User));
                if (postlike != null)
                {
                    TempData[post.Id.ToString()] = "2";
                }
            }

            //afisez mesajul din delete
            if (TempData.ContainsKey("msg"))
            {
                ViewBag.Msg = TempData["msg"];
                ViewBag.MsgType = TempData["msgType"];
            }

            TempData["ReturnUrl"] = "/Posts/Index"; //calea de intoarcere din Like

            return View();
        }

        public IActionResult Show(int Id)
        {
            Post post = db.Posts.Include("Comments")
                                .Include("User")
                                .Include("Comments.User")
                                .Include("Comments.CommentLikes")
                                .Include("PostLikes")
                                .Include("Location")
                                .Where(post => post.Id == Id)
                                .First();
            ViewBag.Buttons = false;
            //if (User.Identity.IsAuthenticated)
            //{
            //    ViewBag.Buttons = true;
            //}

            ViewBag.UserId = _userManager.GetUserId(User);

            if (post.UserId == _userManager.GetUserId(User)) //verificam daca userul curent este cel care a postat
            {
                ViewBag.Buttons = true;                                    //User.IsInRole("Admin") - verifica daca userul este admin
                return View(post);
            }
            else if (User.IsInRole("Admin")) //verificam daca userul curent este admin
            {
                ViewBag.Buttons1 = true;
                return View(post);
            }

            //verificam daca userul curent a dat like la postare
            PostLike postlike = post.PostLikes.FirstOrDefault(p => p.UserId == _userManager.GetUserId(User));
            if (postlike != null)
            {
                TempData[post.Id.ToString()] = "2";
            }
            //verificam daca userul curent a dat like la comentarii
            foreach (var comment in post.Comments)
            {
                CommentLike commentLike = comment.CommentLikes.FirstOrDefault(p => p.UserId == _userManager.GetUserId(User));
                if (commentLike != null)
                {
                    TempData[comment.Id.ToString()] = "2";
                }
            }

            TempData["ReturnUrl"] = $"/Posts/Show/{Id}"; //calea de intoarcere din Like
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

            return View();
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> New([FromForm] Post post, [FromForm] IFormFile Image, [FromForm] string? locationData)
        {
            // legam locatia cu postarea
            if (!string.IsNullOrEmpty(locationData))
            {
                
                var location = JsonConvert.DeserializeObject<Address>(locationData);
                Location location1 = db.Locations.FirstOrDefault(l => l.Address == location.address);
                //verificam dafac exista deja in baza de date 
                if (location1 != null)
                {
                    post.LocationId = location1.Id;
                }
                else
                {
                    db.Locations.Add(new Location
                    {
                        Address = JsonConvert.DeserializeObject<Address>(locationData).address,
                        Latitude = JsonConvert.DeserializeObject<Address>(locationData).latitude,
                        Longitude = JsonConvert.DeserializeObject<Address>(locationData).longitude
                    });
                    db.SaveChanges();
                    post.LocationId = db.Locations.FirstOrDefault(l => l.Address == location.address).Id;
                }
            }

            
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
            if(string.IsNullOrEmpty(locationData))
            {
                ModelState.AddModelError("", "Introduceti locatia!");
                return View(post);
            }

            //video
            if (!string.IsNullOrEmpty(post.linkVideo))
            {
                post.linkVideo = ConvertLinkToEmbed(post.linkVideo);
            }

            post.Date = DateTime.Now;
            post.Likes = 0;
            post.UserId = _userManager.GetUserId(User);

  
            if (ModelState.IsValid)
            {
                
                db.Posts.Add(post);
                db.SaveChanges();

                TempData["msg"] = "Postarea a fost adaugata!";
                TempData["msgType"] = "alert-success"; //clasa de bootstrap pt mesaj de succes(verde)
                //return Ok(new { message = "Postarea a fost adăugată cu succes!" });
                //return RedirectToAction("Index");
                //return RedirectToAction("Index");
                Console.WriteLine("\n corect \n");

                return Json(new { redirectUrl = Url.Action("Index", "Posts") });
            }
            else
            {
                // Dacă nu este autentificat, redirecționează la Login
                
                return BadRequest();
                //return View(post);
            }
            
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int Id) //doar userul care a postat poate edita
        {
            Post post = db.Posts.Include("Location")
                                .Where(p => p.Id == Id)
                                .First();

            if (post.UserId == _userManager.GetUserId(User)) //verificam daca userul curent este cel care a postat
            {   
                ViewBag.Buttons = true;                                    //User.IsInRole("Admin") - verifica daca userul este admin
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
            //verififcam daca cel putin un camp este completat (sau daca posarea are deja imagine sau descriere)
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

        public IActionResult DeleteLocation(int Id)
        {

            Post post = db.Posts.Find(Id);
            if (post.LocationId != null)
            {
                post.LocationId = null;
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
            Post post = db.Posts.Include(p => p.Comments)
                                .Include(p => p.PostLikes)
                                .FirstOrDefault(p => p.Id == Id);



            if (post.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                //stergem comentariile postarii
                db.Comments.RemoveRange(post.Comments);
                // Delete likes of the post
                db.PostLikes.RemoveRange(post.PostLikes);

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


        

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Like(int id)
        {
            
            Post post = db.Posts.Find(id);
            PostLike postLike = db.PostLikes.FirstOrDefault(p => p.PostId == id && p.UserId == _userManager.GetUserId(User));
            if (postLike != null)
            {
                post.Likes--;
                db.PostLikes.Remove(postLike);
                db.SaveChanges();
                TempData[id.ToString()] = "0";
            }
            else
            {
                db.PostLikes.Add(new PostLike
                {
                    PostId = id,
                    UserId = _userManager.GetUserId(User)
                });
                post.Likes++;
                db.SaveChanges();
                TempData[id.ToString()] = "2";
            }

            
            return Redirect(TempData["ReturnUrl"].ToString());
        }

        private string ConvertLinkToEmbed(string link)
        {
            if (link.Contains("youtube.com") || link.Contains("youtu.be"))
            {
                // Extrage ID-ul videoclipului YouTube
                var videoId = link.Contains("youtu.be")
                    ? link.Split('/').Last()
                    : HttpUtility.ParseQueryString(new Uri(link).Query).Get("v");

                // Returnează codul iframe
                return $"<iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/{videoId}\" frameborder=\"0\" allowfullscreen></iframe>";
            }

            return string.Empty; // Returnează un string gol dacă nu este un link suportat
        }

    }
}
