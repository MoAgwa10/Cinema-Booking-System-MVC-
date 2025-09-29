using Cinema_Booking_System.Data.Repo;
using Cinema_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Controllers
{
    public class ActorController : Controller
    {

        private readonly IActorsService _service;

        public ActorController(IActorsService service)
        {
            _service = service;
        }

        // READ: All actors
        public async Task<IActionResult> Index()
        {
            var actors = await _service.GetAllAsync();
            return View(actors);
        }

        // CREATE: GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE: POST
        [HttpPost]
   
        public async Task<IActionResult> Create([Bind("FullName,Bio,ProfilePicUrl")] Actor actor)
        {
            if (!ModelState.IsValid) return View(actor);
            
            await _service.AddAsync(actor);
            return RedirectToAction(nameof(Index));
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var actor = await _service.GetByIdAsync(id);
            if (actor == null) return View("NotFound");
            return View(actor);
        }

        // EDIT: GET
        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _service.GetByIdAsync(id);
            if (actor == null) return View("NotFound");
            return View(actor);
        }

     
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Bio,ProfilePicUrl")] Actor actor)
        {
            if (!ModelState.IsValid) return View(actor);

            await _service.UpdateAsync(id, actor);
            return RedirectToAction(nameof(Index));
        }

      
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _service.GetByIdAsync(id);
            if (actor == null) return View("NotFound");
            return View(actor);
        }

        // DELETE: POST
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _service.GetByIdAsync(id);
            if (actor == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
