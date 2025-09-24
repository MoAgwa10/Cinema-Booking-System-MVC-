using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Controllers
{
    public class ProducerController : Controller
    {

        private readonly AppDbContext _context;
        public ProducerController(AppDbContext context)
        {
            _context = context;
        }
        public async Task< IActionResult> Index()
        {
            var  Prod =_context.Producers.ToListAsync();
            return  View();
        }
    }
}
