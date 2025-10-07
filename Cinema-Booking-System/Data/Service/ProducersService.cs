using Cinema_Booking_System;
using Cinema_Booking_System.Data.Base;
using Cinema_Booking_System.Models;

public class ProducersService : EntityBaseRepository<Producer>, IProducersService
{
    public ProducersService(AppDbContext context) : base(context)
    {
    }
}