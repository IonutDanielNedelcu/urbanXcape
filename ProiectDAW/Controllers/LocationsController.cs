using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProiectDAW.Data;
using ProiectDAW.Models;

namespace ProiectDAW.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("save-location")]
        public async Task<IActionResult> SaveLocation([FromBody] Address location)
        {
            
            

            if (location == null || string.IsNullOrEmpty(location.address))
            {
                return BadRequest("Datele locației sunt invalide.");
            }

            // Creează un obiect de locație pentru salvare
            var newLocation = new Location
            {
                Address = location.address,
                Latitude = location.latitude,
                Longitude = location.longitude
            };

          

            _context.Locations.Add(newLocation);
            
            await _context.SaveChangesAsync();

            int Id = location.postId;
            
            Post post = _context.Posts.Find(Id);
            if (post != null)
            {
                post.LocationId = newLocation.Id;
            }
            await _context.SaveChangesAsync();

            return Json(new { redirectUrl = Url.Action("Edit", "Posts") });
        }

        [HttpGet("get-location/{id}")]
        public async Task<IActionResult> GetLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound("Locația nu a fost găsită.");
            }
            return Ok(location);
        }

        

        [HttpGet("show")]
        public IActionResult Show()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
