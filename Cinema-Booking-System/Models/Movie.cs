using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cinema_Booking_System.Data;

namespace Cinema_Booking_System.Models
{
    public class Movie : IEntityBase
    {
        [Key]
        public int id { get; set; }
        
        [Required(ErrorMessage = "Movie name is required")]
        [Display(Name = "Movie Name")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000.00, ErrorMessage = "Price must be between $0.01 and $1000.00")]
        [Display(Name = "Price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
        
        [Display(Name = "Movie Poster")]
        public string? ImageUrl { get; set; }
        
        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        public DateTime Startdate { get; set; }
        
        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        public DateTime Enddate { get; set; }

        [Required(ErrorMessage = "Movie category is required")]
        [Display(Name = "Category")]
        public MovieCategory movieCategory { get; set; }

        [ForeignKey("ProducerId")]
        [Required(ErrorMessage = "Producer is required")]
        public int ProducerId { get; set; }
        public Producer? Producer { get; set; }

        public List<Movie_Cinema> Movies_Cinemas { get; set; } = new List<Movie_Cinema>();
        public List<Actor_Movies> Actor_Movies { get; set; } = new List<Actor_Movies>();
        public List<OrderItem> orderItems { get; set; } = new List<OrderItem>();
    }
}
