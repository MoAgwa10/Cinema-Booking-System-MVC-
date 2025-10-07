using Cinema_Booking_System.Data.Repo;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.Models.ViewModels;
using Cinema_Booking_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema_Booking_System.Controllers
{
    public class ProducerController : Controller
    {
        private readonly IProducersService _service;
        private readonly IImageService _imageService;

        public ProducerController(IProducersService service, IImageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        // READ: All producers
        public async Task<IActionResult> Index()
        {
            var producers = await _service.GetAllAsync();
            return View(producers);
        }

        // CREATE: GET
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new ProducerCreateVM());
        }

        // CREATE: POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProducerCreateVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var producer = new Producer
            {
                FullName = viewModel.FullName,
                Bio = viewModel.Bio
            };

            // Handle image upload
            producer.ProfilePicUrl = await _imageService.SaveImageAsync(viewModel.ImageFile, "producer");
            
            await _service.AddAsync(producer);
            TempData["Success"] = $"Producer '{producer.FullName}' created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var producer = await _service.GetByIdAsync(id);
            if (producer == null) return View("NotFound");
            return View(producer);
        }

        // EDIT: GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var producer = await _service.GetByIdAsync(id);
            if (producer == null) return View("NotFound");
            
            var viewModel = new ProducerCreateVM
            {
                Id = producer.id,
                FullName = producer.FullName,
                Bio = producer.Bio,
                ProfilePicUrl = producer.ProfilePicUrl
            };
            
            return View(viewModel);
        }

        // EDIT: POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, ProducerCreateVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var producer = await _service.GetByIdAsync(id);
            if (producer == null) return View("NotFound");

            producer.FullName = viewModel.FullName;
            producer.Bio = viewModel.Bio;

            // Handle image upload
            if (viewModel.ImageFile != null)
            {
                producer.ProfilePicUrl = await _imageService.SaveImageAsync(viewModel.ImageFile, "producer");
            }

            await _service.UpdateAsync(id, producer);
            TempData["Success"] = $"Producer '{producer.FullName}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // DELETE: GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var producer = await _service.GetByIdAsync(id);
            if (producer == null) return View("NotFound");
            return View(producer);
        }

        // DELETE: POST
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producer = await _service.GetByIdAsync(id);
            if (producer == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _service.DeleteAsync(id);
                TempData["Success"] = $"Producer '{producer.FullName}' deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = $"Cannot delete '{producer.FullName}' because they have movies associated with them. Please delete the movies first.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }
        }
    }
}
