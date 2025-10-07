using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.Models.ViewModels
{
    public class CinemaCreateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Cinema name is required")]
        [Display(Name = "Cinema Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Cinema Logo")]
        public IFormFile? ImageFile { get; set; }

        // For displaying existing image during edit
        public string? LogoUrl { get; set; }
    }
}
