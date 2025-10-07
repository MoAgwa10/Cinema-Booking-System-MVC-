using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.Models.ViewModels
{
    public class ActorCreateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Biography is required")]
        [Display(Name = "Biography")]
        public string Bio { get; set; } = string.Empty;

        [Display(Name = "Profile Picture")]
        public IFormFile? ImageFile { get; set; }

        // For displaying existing image during edit
        public string? ProfilePicUrl { get; set; }
    }
}
