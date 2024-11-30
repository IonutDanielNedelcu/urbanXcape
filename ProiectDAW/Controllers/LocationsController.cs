using Microsoft.AspNetCore.Mvc;

namespace ProiectDAW.Controllers
{
    public class LocationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Show()
        {
            return View();
        }
    }
}
