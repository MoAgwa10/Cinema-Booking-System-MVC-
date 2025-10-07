using Cinema_Booking_System.Data.Repo;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.Models.ViewModels;
using Cinema_Booking_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema_Booking_System.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorsService _service;
        private readonly IImageService _imageService;

        public ActorController(IActorsService service, IImageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        // READ: All actors
        public async Task<IActionResult> Index()
        {
            var actors = await _service.GetAllAsync();
            return View(actors);
        }

        // CREATE: GET
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new ActorCreateVM());
        }

        // CREATE: POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ActorCreateVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                // Handle image upload using new ImageService
                string profilePicUrl = await _imageService.SaveImageAsync(viewModel.ImageFile, "actor");
                
                var actor = new Actor
                {
                    FullName = viewModel.FullName,
                    Bio = viewModel.Bio,
                    ProfilePicUrl = profilePicUrl
                };

                await _service.AddAsync(actor);
                TempData["Success"] = $"Actor '{actor.FullName}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating actor: {ex.Message}");
                return View(viewModel);
            }
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var actor = await _service.GetByIdAsync(id);
            if (actor == null) return View("NotFound");
            return View(actor);
        }

        // EDIT: GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var actor = await _service.GetByIdAsync(id);
            if (actor == null) return View("NotFound");
            
            var viewModel = new ActorCreateVM
            {
                Id = actor.id,
                FullName = actor.FullName,
                Bio = actor.Bio,
                ProfilePicUrl = actor.ProfilePicUrl
            };
            
            return View(viewModel);
        }

        // EDIT: POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, ActorCreateVM viewModel, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var actor = await _service.GetByIdAsync(id);
                if (actor == null) return View("NotFound");

                actor.FullName = viewModel.FullName;
                actor.Bio = viewModel.Bio;

                // Handle image upload if new file provided
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    actor.ProfilePicUrl = await _imageService.SaveImageAsync(ImageFile, "actor");
                }
                // Keep existing image if no new file uploaded

                await _service.UpdateAsync(id, actor);
                TempData["Success"] = $"Actor '{actor.FullName}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating actor: {ex.Message}");
                return View(viewModel);
            }
        }

        // DELETE: GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _service.GetByIdAsync(id);
            if (actor == null) return View("NotFound");
            return View(actor);
        }

        // DELETE: POST
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _service.GetByIdAsync(id);
            if (actor == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _service.DeleteAsync(id);
                TempData["Success"] = $"Actor '{actor.FullName}' deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = $"Cannot delete '{actor.FullName}' because they have movies associated with them. Please delete the movies first.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }
        }
    }
}
