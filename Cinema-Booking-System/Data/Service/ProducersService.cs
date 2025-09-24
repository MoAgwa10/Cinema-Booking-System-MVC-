using Cinema_Booking_System;
using Cinema_Booking_System.Data.Base;

public class ProducersService : EntityBaseRepository<Producer>, IProducersService
{
    public ProducersService(AppDbContext context) : base(context)
    {
    }
}