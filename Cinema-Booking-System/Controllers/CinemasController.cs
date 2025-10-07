using Cinema_Booking_System.Data.Repo;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.Models.ViewModels;
using Cinema_Booking_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Cinema_Booking_System.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ICinemasService _service;
        private readonly IImageService _imageService;

        public CinemasController(ICinemasService service, IImageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        // GET: Cinemas
        public async Task<IActionResult> Index()
        {
            var allCinemas = await _service.GetAllAsync();
            return View(allCinemas);
        }

        // GET: Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new CinemaCreateVM());
        }

        // POST: Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CinemaCreateVM viewModel)
        {
            // Debug: Check ModelState
            if (!ModelState.IsValid)
            {
                // Debug: Log validation errors
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Field: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                TempData["Error"] = "Please check the form for errors.";
                return View(viewModel);
            }

            try
            {
                // Handle image upload using new ImageService
                string logoUrl = await _imageService.SaveImageAsync(viewModel.ImageFile, "cinema");
                
                var cinema = new Cinema
                {
                    FullName = viewModel.FullName,
                    Description = viewModel.Description,
                    LogoUrl = logoUrl
                };
                
                await _service.AddAsync(cinema);
                TempData["Success"] = $"Cinema '{cinema.FullName}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating cinema: {ex.Message}");
                return View(viewModel);
            }
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var cinema = await _service.GetByIdAsync(id);
            if (cinema == null) return View("NotFound");
            return View(cinema);
        }

        // EDIT: GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var cinema = await _service.GetByIdAsync(id);
            if (cinema == null) return View("NotFound");
            return View(cinema);
        }

        // EDIT: POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Cinema cinema, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(cinema);
            }

            try
            {
                // Get existing cinema to preserve current image if no new one uploaded
                var existingCinema = await _service.GetByIdAsync(id);
                if (existingCinema == null) return View("NotFound");

                // Handle image upload if new file provided
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    cinema.LogoUrl = await _imageService.SaveImageAsync(ImageFile, "cinema");
                }
                else
                {
                    // Keep existing image
                    cinema.LogoUrl = existingCinema.LogoUrl;
                }

                await _service.UpdateAsync(id, cinema);
                TempData["Success"] = $"Cinema '{cinema.FullName}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating cinema: {ex.Message}");
                return View(cinema);
            }
        }

        // DELETE: GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var cinema = await _service.GetByIdAsync(id);
            if (cinema == null) return View("NotFound");
            return View(cinema);
        }

        // DELETE: POST
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinema = await _service.GetByIdAsync(id);
            if (cinema == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await _service.DeleteAsync(id);
            TempData["Success"] = $"Cinema '{cinema.FullName}' deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
