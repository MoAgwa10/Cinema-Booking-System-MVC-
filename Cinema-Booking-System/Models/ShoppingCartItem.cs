using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cinema_Booking_System.Models;

public class ShoppingCartItem
{
    [Key]
    public int id { get; set; }

    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public Movie Movie { get; set; }
    
    public int Amount { get; set; }
    public string ShoppingCartId { get; set; }
}