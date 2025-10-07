using Cinema_Booking_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Cinema_Booking_System.Data
{
    public class AppDbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            // Ensure database is created
            context.Database.EnsureCreated();

            // Create Admin Role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create User Role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Create Admin User
            var adminEmail = configuration["AdminUser:Email"];
            var adminPassword = configuration["AdminUser:Password"];
            
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Cinemas
            if (!context.Cinemas.Any())
            {
                context.Cinemas.AddRange(new[]
                {
                    new Cinema { FullName = "AMC Theaters", LogoUrl = "https://images.unsplash.com/photo-1489599735734-79b4169c4388?w=400&h=300&fit=crop", Description = "Premium movie theater with state-of-the-art sound and projection systems" },
                    new Cinema { FullName = "Regal Cinemas", LogoUrl = "https://images.unsplash.com/photo-1517604931442-7e0c8ed2963c?w=400&h=300&fit=crop", Description = "Luxury cinema experience with comfortable seating and premium amenities" },
                    new Cinema { FullName = "Cinemark", LogoUrl = "https://images.unsplash.com/photo-1536440136628-849c177e76a1?w=400&h=300&fit=crop", Description = "Modern multiplex cinema with the latest blockbuster movies" }
                });
                context.SaveChanges();
            }

            // Actors
            if (!context.Actors.Any())
            {
                context.Actors.AddRange(new[]
                {
                    new Actor { FullName = "Leonardo DiCaprio", Bio = "Academy Award-winning actor known for his roles in Titanic, Inception, and The Revenant", ProfilePicUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=300&h=400&fit=crop" },
                    new Actor { FullName = "Morgan Freeman", Bio = "Legendary actor with distinctive voice, known for The Shawshank Redemption and many other classics", ProfilePicUrl = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=300&h=400&fit=crop" },
                    new Actor { FullName = "Christian Bale", Bio = "Versatile actor known for his transformative roles in The Dark Knight trilogy and many dramatic films", ProfilePicUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=300&h=400&fit=crop" }
                });
                context.SaveChanges();
            }

            // Producers
            if (!context.Producers.Any())
            {
                context.Producers.AddRange(new[]
                {
                    new Producer { FullName = "Christopher Nolan", Bio = "Acclaimed filmmaker known for complex narratives and stunning visuals in films like Inception, Interstellar, and The Dark Knight", ProfilePicUrl = "https://images.unsplash.com/photo-1560250097-0b93528c311a?w=300&h=400&fit=crop" },
                    new Producer { FullName = "James Cameron", Bio = "Visionary director and producer behind epic films like Titanic, Avatar, and Terminator series", ProfilePicUrl = "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?w=300&h=400&fit=crop" }
                });
                context.SaveChanges();
            }

            // Movies
            if (!context.Movies.Any())
            {
                context.Movies.AddRange(new[]
                {
                    new Movie
                    {
                        Name = "Inception",
                        Description = "A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                        Price = 12.99,
                        ImageUrl = "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?w=400&h=600&fit=crop",
                        Startdate = System.DateTime.Now.AddDays(-10),
                        Enddate = System.DateTime.Now.AddDays(10),
                        movieCategory = MovieCategory.SciFi,
                        ProducerId = context.Producers.First().id
                    },
                    new Movie
                    {
                        Name = "The Shawshank Redemption",
                        Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                        Price = 10.99,
                        ImageUrl = "https://images.unsplash.com/photo-1489599735734-79b4169c4388?w=400&h=600&fit=crop",
                        Startdate = System.DateTime.Now,
                        Enddate = System.DateTime.Now.AddDays(14),
                        movieCategory = MovieCategory.Drama,
                        ProducerId = context.Producers.Skip(1).First().id
                    },
                    new Movie
                    {
                        Name = "Interstellar",
                        Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                        Price = 14.99,
                        ImageUrl = "https://images.unsplash.com/photo-1446776653964-20c1d3a81b06?w=400&h=600&fit=crop",
                        Startdate = System.DateTime.Now.AddDays(-5),
                        Enddate = System.DateTime.Now.AddDays(15),
                        movieCategory = MovieCategory.SciFi,
                        ProducerId = context.Producers.First().id
                    },
                    new Movie
                    {
                        Name = "The Dark Knight",
                        Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests.",
                        Price = 13.99,
                        ImageUrl = "https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=400&h=600&fit=crop",
                        Startdate = System.DateTime.Now.AddDays(-2),
                        Enddate = System.DateTime.Now.AddDays(20),
                        movieCategory = MovieCategory.Action,
                        ProducerId = context.Producers.First().id
                    },
                    new Movie
                    {
                        Name = "Titanic",
                        Description = "A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the luxurious, ill-fated R.M.S. Titanic.",
                        Price = 11.99,
                        ImageUrl = "https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=400&h=600&fit=crop",
                        Startdate = System.DateTime.Now.AddDays(1),
                        Enddate = System.DateTime.Now.AddDays(30),
                        movieCategory = MovieCategory.Romance,
                        ProducerId = context.Producers.Skip(1).First().id
                    },
                    new Movie
                    {
                        Name = "Avengers: Endgame",
                        Description = "After the devastating events of Infinity War, the Avengers assemble once more to reverse Thanos' actions and restore balance to the universe.",
                        Price = 15.99,
                        ImageUrl = "https://images.unsplash.com/photo-1635805737707-575885ab0820?w=400&h=600&fit=crop",
                        Startdate = System.DateTime.Now.AddDays(-7),
                        Enddate = System.DateTime.Now.AddDays(7),
                        movieCategory = MovieCategory.Adventure,
                        ProducerId = context.Producers.First().id
                    }
                });
                context.SaveChanges();
            }

                // Actor_Movies
                if (!context.Actors_Movies.Any())
                {
                    var movie1 = context.Movies.First();
                    var movie2 = context.Movies.Skip(1).First();
                    var actor1 = context.Actors.First();
                    var actor2 = context.Actors.Skip(1).First();
                    var actor3 = context.Actors.Skip(2).First();

                    context.Actors_Movies.AddRange(new[]
                    {
                        new Actor_Movies { ActorId = actor1.id, MovieId = movie1.id },
                        new Actor_Movies { ActorId = actor2.id, MovieId = movie1.id },
                        new Actor_Movies { ActorId = actor2.id, MovieId = movie2.id },
                        new Actor_Movies { ActorId = actor3.id, MovieId = movie2.id }
                    });
                    context.SaveChanges();
                }

                // Movie_Cinema
                if (!context.Movies_Cinemas.Any())
                {
                    var movie1 = context.Movies.First();           // Life
                    var movie2 = context.Movies.Skip(1).First();   // Shawshank
                    var movie3 = context.Movies.Skip(2).First();   // Inception
                    var movie4 = context.Movies.Skip(3).First();   // Interstellar
                    var cinema1 = context.Cinemas.First();
                    var cinema2 = context.Cinemas.Skip(1).First();
                    var cinema3 = context.Cinemas.Skip(2).First();

                    context.Movies_Cinemas.AddRange(new[]
                    {
                        new Movie_Cinema { MovieId = movie1.id, CinemaId = cinema1.id },
                        new Movie_Cinema { MovieId = movie2.id, CinemaId = cinema2.id },
                        new Movie_Cinema { MovieId = movie3.id, CinemaId = cinema1.id }, // نفس السينما مع Movie1
                        new Movie_Cinema { MovieId = movie4.id, CinemaId = cinema3.id }
                    });
                    context.SaveChanges();
                }
            }
        }
    }

