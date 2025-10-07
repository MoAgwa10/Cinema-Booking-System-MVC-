using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.Models
{
    public class Actor : IEntityBase
    {
        [Key]
        public int id { get; set; }
        
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]
        public string Bio { get; set; } = string.Empty;

        [Display(Name = "Profile Picture")]
        public string? ProfilePicUrl { get; set; }

        public List<Actor_Movies> Actor_Movies { get; set; } = new List<Actor_Movies>();
    }
}
