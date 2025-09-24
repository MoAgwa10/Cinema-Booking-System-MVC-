using Cinema_Booking_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Cinema_Booking_System.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<AppDbContext>())
            {
                // Cinemas
                if (!context.Cinemas.Any())
                {
                    context.Cinemas.AddRange(new[]
                    {
                        new Cinema { FullName = "Cinema 1", LogoUrl = "http://dotnethow.net/images/cinemas/cinema-1.jpeg", Description = "This is the description of Cinema 1" },
                        new Cinema { FullName = "Cinema 2", LogoUrl = "http://dotnethow.net/images/cinemas/cinema-2.jpeg", Description = "This is the description of Cinema 2" },
                        new Cinema { FullName = "Cinema 3", LogoUrl = "http://dotnethow.net/images/cinemas/cinema-3.jpeg", Description = "This is the description of Cinema 3" }
                    });
                    context.SaveChanges();
                }

                // Actors
                if (!context.Actors.Any())
                {
                    context.Actors.AddRange(new[]
                    {
                        new Actor { FullName = "Actor 1", Bio = "Bio of Actor 1", ProfilePicUrl = "http://dotnethow.net/images/actors/actor-1.jpeg" },
                        new Actor { FullName = "Actor 2", Bio = "Bio of Actor 2", ProfilePicUrl = "http://dotnethow.net/images/actors/actor-2.jpeg" },
                        new Actor { FullName = "Actor 3", Bio = "Bio of Actor 3", ProfilePicUrl = "http://dotnethow.net/images/actors/actor-3.jpeg" }
                    });
                    context.SaveChanges();
                }

                // Producers
                if (!context.Producers.Any())
                {
                    context.Producers.AddRange(new[]
                    {
                        new Producer { FullName = "Producer 1", Bio = "Bio of Producer 1", ProfilePicUrl = "http://dotnethow.net/images/producers/producer-1.jpeg" },
                        new Producer { FullName = "Producer 2", Bio = "Bio of Producer 2", ProfilePicUrl = "http://dotnethow.net/images/producers/producer-2.jpeg" }
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
                            Name = "Life",
                            Description = "This is the Life movie description",
                            Price = 39.5,
                            ImageUrl = "http://dotnethow.net/images/movies/movie-3.jpeg",
                            Startdate = System.DateTime.Now.AddDays(-10),
                            Enddate = System.DateTime.Now.AddDays(10),
                            ProducerId = context.Producers.First().id
                        },
                        new Movie
                        {
                            Name = "The Shawshank Redemption",
                            Description = "This is the Shawshank Redemption description",
                            Price = 29.5,
                            ImageUrl = "http://dotnethow.net/images/movies/movie-1.jpeg",
                            Startdate = System.DateTime.Now,
                            Enddate = System.DateTime.Now.AddDays(3),
                            ProducerId = context.Producers.Skip(1).First().id
                        },
                        new Movie
                        {
                            Name = "Inception",
                            Description = "This is the Inception description",
                            Price = 49.0,
                            ImageUrl = "http://dotnethow.net/images/movies/movie-2.jpeg",
                            Startdate = System.DateTime.Now.AddDays(-5),
                            Enddate = System.DateTime.Now.AddDays(15),
                            ProducerId = context.Producers.First().id
                        },
                        new Movie
                        {
                            Name = "Interstellar",
                            Description = "This is the Interstellar description",
                            Price = 59.0,
                            ImageUrl = "http://dotnethow.net/images/movies/movie-4.jpeg",
                            Startdate = System.DateTime.Now.AddDays(-2),
                            Enddate = System.DateTime.Now.AddDays(20),
                            ProducerId = context.Producers.Skip(1).First().id
                        },
                        new Movie
                        {
                            Name = "The Dark Knight",
                            Description = "This is The Dark Knight description",
                            Price = 45.0,
                            ImageUrl = "http://dotnethow.net/images/movies/movie-5.jpeg",
                            Startdate = System.DateTime.Now.AddDays(-7),
                            Enddate = System.DateTime.Now.AddDays(7),
                            ProducerId = context.Producers.First().id
                        },
                        new Movie
                        {
                            Name = "Titanic",
                            Description = "This is the Titanic description",
                            Price = 35.0,
                            ImageUrl = "http://dotnethow.net/images/movies/movie-6.jpeg",
                            Startdate = System.DateTime.Now.AddDays(1),
                            Enddate = System.DateTime.Now.AddDays(30),
                            ProducerId = context.Producers.Skip(1).First().id
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
}
