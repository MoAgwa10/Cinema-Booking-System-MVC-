using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.Models
{
    public class Cinema : IEntityBase
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Cinema Name")]
        [Required(ErrorMessage = "Cinema name is required")]
        public string FullName { get; set; }


        [Display(Name = "Cinema Logo")]
        [Required(ErrorMessage = "Cinema logo is required")]
        public string LogoUrl { get; set; }


        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public List<Movie_Cinema> Movies_Cinemas { get; set; }
    }
}