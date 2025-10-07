using System.ComponentModel.DataAnnotations;
using Cinema_Booking_System.Data;
using Microsoft.AspNetCore.Http;

namespace Cinema_Booking_System.Models.ViewModels
{
    public class MovieEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Movie name is required")]
        [StringLength(100, ErrorMessage = "Movie name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000.00, ErrorMessage = "Price must be between $0.01 and $1000.00")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime Startdate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime Enddate { get; set; }

        [Required(ErrorMessage = "Movie category is required")]
        public MovieCategory movieCategory { get; set; }

        [Required(ErrorMessage = "Producer is required")]
        public int ProducerId { get; set; }

        // Current image URL
        public string? CurrentImageUrl { get; set; }

        // Optional new image file
        [Display(Name = "Movie Poster")]
        public IFormFile? ImageFile { get; set; }

        public List<int> SelectedActorIds { get; set; } = new List<int>();
        public List<int> SelectedCinemaIds { get; set; } = new List<int>();

        // For dropdowns
        public IEnumerable<Producer> Producers { get; set; } = new List<Producer>();
        public IEnumerable<Actor> Actors { get; set; } = new List<Actor>();
        public IEnumerable<Cinema> Cinemas { get; set; } = new List<Cinema>();
    }
}
