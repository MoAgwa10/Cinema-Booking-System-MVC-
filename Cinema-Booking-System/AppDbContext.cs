using Cinema_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

            
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define composite primary key
            modelBuilder.Entity<Actor_Movies>().HasKey(am => new
            {
                am.ActorId,
                am.MovieId
            });

            // Relationships
            modelBuilder.Entity<Actor_Movies>()
     .HasOne(m => m.Movie)
     .WithMany(am => am.Actor_Movies)
     .HasForeignKey(m => m.MovieId);

            modelBuilder.Entity<Actor_Movies>()
                .HasOne(m => m.Actor)
                .WithMany(am => am.Actor_Movies)
                .HasForeignKey(m => m.ActorId);

            modelBuilder.Entity<Movie_Cinema>().HasKey(mc => new { mc.MovieId, mc.CinemaId });

            modelBuilder.Entity<Movie_Cinema>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.Movies_Cinemas)
                .HasForeignKey(mc => mc.MovieId);

            modelBuilder.Entity<Movie_Cinema>()
                .HasOne(mc => mc.Cinema)
                .WithMany(c => c.Movies_Cinemas)
                .HasForeignKey(mc => mc.CinemaId);

        }
        

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Actor_Movies> Actors_Movies { get; set; }
        public DbSet<Movie_Cinema> Movies_Cinemas { get; set; }

        //Orders related tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }


}

