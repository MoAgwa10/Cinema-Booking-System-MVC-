using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Controllers
{
    public class MoviesController : Controller
    {

        private readonly AppDbContext _context;
        public MoviesController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var allMovies = await _context.Movies
     .Include(m => m.Movies_Cinemas)
         .ThenInclude(mc => mc.Cinema)
     .ToListAsync();
            return View(allMovies);
        }

    }
}
