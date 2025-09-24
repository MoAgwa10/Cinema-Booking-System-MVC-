using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Controllers
{
    public class CinemasController : Controller
    {

        private readonly AppDbContext _context;
        public CinemasController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ActionResult> Index()
        {
            var Cinema =await _context.Cinemas.ToListAsync();
            return View(Cinema);
        }
    }
}
