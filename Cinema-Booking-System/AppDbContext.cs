using Cinema_Booking_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

            
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base method to configure Identity tables
            base.OnModelCreating(modelBuilder);
            
            // Define composite primary key
            modelBuilder.Entity<Actor_Movies>().HasKey(am => new
            {
                am.ActorId,
                am.MovieId
            });

            // Relationships with Cascade Delete
            modelBuilder.Entity<Actor_Movies>()
                .HasOne(m => m.Movie)
                .WithMany(am => am.Actor_Movies)
                .HasForeignKey(m => m.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Actor_Movies>()
                .HasOne(m => m.Actor)
                .WithMany(am => am.Actor_Movies)
                .HasForeignKey(m => m.ActorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movie_Cinema>().HasKey(mc => new { mc.MovieId, mc.CinemaId });

            modelBuilder.Entity<Movie_Cinema>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.Movies_Cinemas)
                .HasForeignKey(mc => mc.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movie_Cinema>()
                .HasOne(mc => mc.Cinema)
                .WithMany(c => c.Movies_Cinemas)
                .HasForeignKey(mc => mc.CinemaId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem - Movie relationship
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Movie)
                .WithMany(m => m.orderItems)
                .HasForeignKey(oi => oi.MovieId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict to prevent accidental order history deletion

            // Producer - Movie relationship (One-to-Many)
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Producer)
                .WithMany(p => p.Movies)
                .HasForeignKey(m => m.ProducerId)
                .OnDelete(DeleteBehavior.Restrict); // Don't delete producer if movies exist

            // ShoppingCartItem - Movie relationship
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Movie)
                .WithMany()
                .HasForeignKey(sci => sci.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            // PaymentTransaction - Order relationship
            modelBuilder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Order)
                .WithMany()
                .HasForeignKey(pt => pt.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

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
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    }


}

