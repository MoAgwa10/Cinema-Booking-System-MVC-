using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Controllers
{
    public class ActorController : Controller
    {

        private readonly AppDbContext _context;
        public ActorController(AppDbContext context)
        {
            _context = context;
        }


        public async Task <ActionResult> Index()
        {
            var actor = await _context.Actors.ToListAsync();
            return View(actor);
        }
    }
}
