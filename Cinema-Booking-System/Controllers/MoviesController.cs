using Cinema_Booking_System.Data.Repo;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.Models.ViewModels;
using Cinema_Booking_System.ViewModel;
using Cinema_Booking_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _service;
        private readonly IImageService _imageService;

        public MoviesController(IMoviesService service, IImageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            var allMovies = await _service.GetAllMoviesWithRelationsAsync();
            return View(allMovies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var movieDetail = await _service.GetMovieByIdAsync(id);
            if (movieDetail == null) return View("NotFound");
            return View(movieDetail);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
            
            // Ensure lists are never null
            var producers = movieDropdownsData.Producers ?? new List<Producer>();
            var actors = movieDropdownsData.Actors ?? new List<Actor>();
            var cinemas = movieDropdownsData.Cinemas ?? new List<Cinema>();
            
            if (!producers.Any() || !actors.Any() || !cinemas.Any())
            {
                TempData["Error"] = "Please add producers, actors, and cinemas first before creating movies.";
            }
            
            var viewModel = new MovieCreateVM
            {
                Producers = producers,
                Actors = actors,
                Cinemas = cinemas,
                Startdate = DateTime.Now,
                Enddate = DateTime.Now.AddDays(30)
            };
            
            return View(viewModel);
        }

        // POST: Movies/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(MovieCreateVM movieVM)
        {
            // Debug: Log received data
            Console.WriteLine($"[DEBUG] Movie Name: {movieVM.Name}");
            Console.WriteLine($"[DEBUG] Movie Description: {movieVM.Description}");
            Console.WriteLine($"[DEBUG] Movie Price: {movieVM.Price}");
            
            // Simple validation
            if (movieVM.SelectedActorIds == null || !movieVM.SelectedActorIds.Any())
                ModelState.AddModelError("SelectedActorIds", "Please select at least one actor");
            
            if (movieVM.SelectedCinemaIds == null || !movieVM.SelectedCinemaIds.Any())
                ModelState.AddModelError("SelectedCinemaIds", "Please select at least one cinema");

            if (!ModelState.IsValid)
            {
                // Reload dropdown data when validation fails
                var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
                movieVM.Producers = movieDropdownsData.Producers ?? new List<Producer>();
                movieVM.Actors = movieDropdownsData.Actors ?? new List<Actor>();
                movieVM.Cinemas = movieDropdownsData.Cinemas ?? new List<Cinema>();
                
                return View(movieVM);
            }

            // Handle image upload
            string imageUrl = await _imageService.SaveImageAsync(movieVM.ImageFile, "movie");

            // Create new movie
            var newMovie = new Movie
            {
                Name = movieVM.Name,
                Description = movieVM.Description,
                Price = movieVM.Price,
                ImageUrl = imageUrl,
                movieCategory = movieVM.movieCategory,
                Startdate = movieVM.Startdate,
                Enddate = movieVM.Enddate,
                ProducerId = movieVM.ProducerId
            };

            await _service.AddNewMovieAsync(newMovie);

            // Add actors and cinemas
            if (movieVM.SelectedActorIds != null && movieVM.SelectedActorIds.Any())
            {
                await _service.AddActorsToMovieAsync(newMovie.id, movieVM.SelectedActorIds);
            }

            if (movieVM.SelectedCinemaIds != null && movieVM.SelectedCinemaIds.Any())
            {
                await _service.AddCinemasToMovieAsync(newMovie.id, movieVM.SelectedCinemaIds);
            }

            TempData["Success"] = $"Movie '{newMovie.Name}' created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Edit/1
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            if (movieDetails == null) return View("NotFound");

            var movieDropdownsData = await _service.GetNewMovieDropdownsValues();

            var viewModel = new MovieEditVM
            {
                Id = movieDetails.id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                Startdate = movieDetails.Startdate,
                Enddate = movieDetails.Enddate,
                CurrentImageUrl = movieDetails.ImageUrl,
                movieCategory = movieDetails.movieCategory,
                ProducerId = movieDetails.ProducerId,
                SelectedActorIds = movieDetails.Actor_Movies.Select(n => n.ActorId).ToList(),
                SelectedCinemaIds = movieDetails.Movies_Cinemas.Select(n => n.CinemaId).ToList(),
                Producers = movieDropdownsData.Producers ?? new List<Producer>(),
                Actors = movieDropdownsData.Actors ?? new List<Actor>(),
                Cinemas = movieDropdownsData.Cinemas ?? new List<Cinema>()
            };

            return View(viewModel);
        }

        // POST: Movies/Edit/1
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, MovieEditVM movieVM)
        {
            if (id != movieVM.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
                movieVM.Producers = movieDropdownsData.Producers ?? new List<Producer>();
                movieVM.Actors = movieDropdownsData.Actors ?? new List<Actor>();
                movieVM.Cinemas = movieDropdownsData.Cinemas ?? new List<Cinema>();
                return View(movieVM);
            }

            string imageUrl = movieVM.CurrentImageUrl; // Keep current image by default

            // Upload new image if provided
            if (movieVM.ImageFile != null)
            {
                // Delete old image if it exists
                if (!string.IsNullOrEmpty(movieVM.CurrentImageUrl) && movieVM.CurrentImageUrl != "/images/default-movie.png")
                {
                    // Old image will be replaced by new one
                }

                imageUrl = await _imageService.SaveImageAsync(movieVM.ImageFile, "movie");
            }

            // Convert to NewMovieVM for service
            var newMovieVM = new NewMovieVM
            {
                id = movieVM.Id,
                Name = movieVM.Name,
                Price = movieVM.Price,
                Description = movieVM.Description,
                StartDate = movieVM.Startdate,
                EndDate = movieVM.Enddate,
                ImageURL = imageUrl,
                MovieCategory = movieVM.movieCategory,
                ProducerId = movieVM.ProducerId,
                ActorIds = movieVM.SelectedActorIds,
                CinemaIds = movieVM.SelectedCinemaIds
            };

            await _service.UpdateMovieAsync(newMovieVM);
            TempData["Success"] = "Movie updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Delete/1
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            if (movieDetails == null) return View("NotFound");
            return View(movieDetails);
        }

        // POST: Movies/Delete/1
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            if (movieDetails == null) return View("NotFound");

            // Check if movie has order history
            if (movieDetails.orderItems != null && movieDetails.orderItems.Any())
            {
                TempData["Error"] = $"Cannot delete '{movieDetails.Name}' because it has order history. You can mark it as inactive instead.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _service.DeleteAsync(id);
                TempData["Success"] = $"Movie '{movieDetails.Name}' deleted successfully!";
            }
            catch (Exception ex)
            {
                // Handle any other database constraint issues
                TempData["Error"] = $"Cannot delete '{movieDetails.Name}' because it has related data. Please remove all related records first or mark it as inactive.";
                Console.WriteLine($"[ERROR] Movie deletion failed: {ex.Message}");
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
