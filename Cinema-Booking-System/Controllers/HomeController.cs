using System.Diagnostics;
using Cinema_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get featured movies (current and upcoming movies)
            var featuredMovies = await _context.Movies
                .Include(m => m.Movies_Cinemas)
                    .ThenInclude(mc => mc.Cinema)
                .Where(m => m.Enddate >= DateTime.Now) // Only show current and future movies
                .OrderBy(m => m.Startdate)
                .Take(6) // Limit to 6 movies for the home page
                .ToListAsync();

            return View(featuredMovies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
