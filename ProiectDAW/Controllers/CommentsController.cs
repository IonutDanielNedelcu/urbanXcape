using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int Id)
        {
            Comment comm = db.Comments.Find(Id);
            ViewBag.Comment = comm;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int Id, Comment editedComm)
        {
            Comment comm = db.Comments.Find(Id);

            if (comm.UserId == _userManager.GetUserId(User))
            {
                if (ModelState.IsValid)
                {
                    comm.Text = editedComm.Text;
                    db.SaveChanges();
                    return Redirect("/Posts/Show/" + comm.PostId);
                }
                else
                {
                    TempData["msg"] = "Nu aveți dreptul să editați acest mesaj!";
                    TempData["msgType"] = "alert-danger"; //clasa de bootstrap pt mesaj de eroare(rosu)
                    return Redirect("/Posts/Show/" + comm.PostId);
                }
            }
            else
            {
                TempData["msg"] = "Nu aveți dreptul să editați această postare!";
                TempData["msgType"] = "alert-danger"; //clasa de bootstrap pt mesaj de eroare(rosu)
                return Redirect("/Posts/Show/" + comm.PostId);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Delete(int id)
        {   
            Comment comm = db.Comments.Find(id);

            if (comm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Comments.Remove(comm);
                db.SaveChanges();
                return Redirect("/Posts/Show/" + comm.PostId);
            }
            else
            {
                TempData["msg"] = "Nu aveți dreptul să ștergeți acest mesaj!";
                TempData["msgType"] = "alert-danger"; //clasa de bootstrap pt mesaj de eroare(rosu)
                return Redirect("/Posts/Show/" + comm.PostId);
            }
            
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public IActionResult Like(int id)
        {
            Comment comm = db.Comments.Find(id);
            CommentLike commLike = db.CommentLikes.FirstOrDefault(p => p.CommentId == id && p.UserId == _userManager.GetUserId(User));
            if (commLike != null)
            {
                comm.Likes--;
                db.CommentLikes.Remove(commLike);
                db.SaveChanges();
                TempData[id.ToString()] = "0";
            }
            else
            {
                db.CommentLikes.Add(new CommentLike
                {
                    CommentId = id,
                    UserId = _userManager.GetUserId(User)
                });
                comm.Likes++;
                db.SaveChanges();
                TempData[id.ToString()] = "2";
            }
            return Redirect("/Posts/Show/" + comm.PostId);
        }

    }
}
