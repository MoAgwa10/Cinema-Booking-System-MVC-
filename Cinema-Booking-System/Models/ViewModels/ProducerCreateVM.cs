using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.Models.ViewModels
{
    public class ProducerCreateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Biography is required")]
        [Display(Name = "Biography")]
        public string Bio { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile ImageFile { get; set; }

        // For displaying existing image during edit
        public string ProfilePicUrl { get; set; }
    }
}
