using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.Models
{
    public class Actors
    {
        [Key]
        int id {get;set;}
        public string FullName {get;set;}

        public string Bio {get;set;}

        public string ProfilePicUrl {get;set;}
    }
}
