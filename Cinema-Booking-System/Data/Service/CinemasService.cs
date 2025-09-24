using Cinema_Booking_System;
using Cinema_Booking_System.Data.Base;

public class CinemasService : EntityBaseRepository<Cinema>, ICinemasService
{
    public CinemasService(AppDbContext context) : base(context)
    { }
}