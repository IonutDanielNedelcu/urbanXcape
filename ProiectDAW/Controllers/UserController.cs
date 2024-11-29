using Microsoft.AspNetCore.Mvc;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;
        public UserController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Show(string id)
        {
            User? user = db.Users.Find(id);
            return View(user);
        }
    }
}
