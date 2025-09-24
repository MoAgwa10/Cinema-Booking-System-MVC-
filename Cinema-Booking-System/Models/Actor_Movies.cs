using System.ComponentModel.DataAnnotations.Schema;
using Cinema_Booking_System.Models;

public class Actor_Movies
{
    
    public int ActorId {get;set;}
    public Actor Actor {get;set;}

   
    public int MovieId {get;set;}
    public Movie Movie {get;set;}   
}
