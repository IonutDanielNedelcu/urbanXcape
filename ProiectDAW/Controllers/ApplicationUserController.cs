using Microsoft.AspNetCore.Mvc;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext db;
        public ApplicationUserController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Show(string id)
        {
            // ApplicationUser? user = db.ApplicationUsers.Find(id);
            //return View(user);
            return View();
        }
    }
}
