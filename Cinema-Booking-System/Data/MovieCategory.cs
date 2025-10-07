using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.Data
{
    public enum MovieCategory
    {
        [Display(Name = "Action")]
        Action,
        
        [Display(Name = "Adventure")]
        Adventure,
        
        [Display(Name = "Comedy")]
        Comedy,
        
        [Display(Name = "Drama")]
        Drama,
        
        [Display(Name = "Horror")]
        Horror,
        
        [Display(Name = "Thriller")]
        Thriller,
        
        [Display(Name = "Mystery")]
        Mystery,
        
        [Display(Name = "Romance")]
        Romance,
        
        [Display(Name = "Sci-Fi")]
        SciFi,
        
        [Display(Name = "Fantasy")]
        Fantasy,
        
        [Display(Name = "Animation")]
        Animation,
        
        [Display(Name = "Family")]
        Family,
        
        [Display(Name = "Documentary")]
        Documentary,
        
        [Display(Name = "Biography")]
        Biography,
        
        [Display(Name = "Crime")]
        Crime,
        
        [Display(Name = "War")]
        War,
        
        [Display(Name = "Western")]
        Western,
        
        [Display(Name = "Musical")]
        Musical,
        
        [Display(Name = "Sport")]
        Sport
    }
}
