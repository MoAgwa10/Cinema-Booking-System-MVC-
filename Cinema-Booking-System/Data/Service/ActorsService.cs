using Cinema_Booking_System.Data.Base;
using Cinema_Booking_System.Data.Repo;
using Cinema_Booking_System.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Cinema_Booking_System.Data.Service
{
    public class ActorsService : EntityBaseRepository<Actor>,IActorsService
    {
        public ActorsService(AppDbContext context) : base(context) { }
    }
}
