using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cinema_Booking_System.Data;

public class Movie:IEntityBase
{
    [Key]
    public int id {get;set;}
    public string Name {get;set;}
    public double Price {get;set;}

    public string Description {get;set;}
    public string ImageUrl {get;set;}
    public DateTime Startdate {get;set;}
    public DateTime Enddate {get;set;}  

    public MovieCategory movieCategory {get;set;}

    [ForeignKey("ProducerId")]
    public int ProducerId {get;set;}
    public Producer Producer {get;set;}


    public List<Movie_Cinema> Movies_Cinemas { get; set; }
    public List<Actor_Movies> Actor_Movies { get; set; }
    public List<OrderItem> orderItems { get; set; }

}


