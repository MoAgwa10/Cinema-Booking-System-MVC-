using System.ComponentModel.DataAnnotations;
using Cinema_Booking_System.Models;


namespace Cinema_Booking_System.Models
{
    public class Producer : IEntityBase
    {
        [Key]
        public int id { get; set; }
        
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }
        
        [Display(Name = "Profile Picture")]
        public string ProfilePicUrl { get; set; }

        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]
        public string Bio { get; set; }



        public List<Movie> Movies { get; set; }
    }
}
