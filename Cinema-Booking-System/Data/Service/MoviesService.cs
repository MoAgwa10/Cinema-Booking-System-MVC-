using Cinema_Booking_System;
using Cinema_Booking_System.Data.Base;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.ViewModel;
using Microsoft.EntityFrameworkCore;

public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
{
    private readonly AppDbContext _context;
    public MoviesService(AppDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<Movie> GetMovieByIdAsync(int id)
    {
        var movieDetails = await _context.Movies
            .Include(p => p.Producer)
            .Include(am => am.Actor_Movies).ThenInclude(a => a.Actor)
            .Include(mc => mc.Movies_Cinemas).ThenInclude(c => c.Cinema)
            .FirstOrDefaultAsync(n => n.id == id);

        return movieDetails;
    }

    public async Task AddNewMovieAsync(NewMovieVM data)
    {
        var newMovie = new Movie()
        {
            Name = data.Name,
            Description = data.Description,
            Price = data.Price,
            ImageUrl = data.ImageURL,
            Startdate = data.StartDate,
            Enddate = data.EndDate,
            movieCategory = data.MovieCategory,
            ProducerId = data.ProducerId
        };

        await _context.Movies.AddAsync(newMovie);
        await _context.SaveChangesAsync();

        // Add Movie Actors
        foreach (var actorId in data.ActorIds)
        {
            var newActorMovie = new Actor_Movies()
            {
                MovieId = newMovie.id,
                ActorId = actorId
            };
            await _context.Actors_Movies.AddAsync(newActorMovie);
        }

        // Add Movie Cinemas (many-to-many)
        foreach (var cinemaId in data.CinemaIds)
        {
            var newMovieCinema = new Movie_Cinema()
            {
                MovieId = newMovie.id,
                CinemaId = cinemaId
            };
            await _context.Movies_Cinemas.AddAsync(newMovieCinema);
        }

        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Movie>> GetAllMoviesWithRelationsAsync()
    {
        var movies = await _context.Movies
            .Include(m => m.Producer)
            .Include(m => m.Movies_Cinemas)
                .ThenInclude(mc => mc.Cinema)
            .Include(m => m.Actor_Movies)
                .ThenInclude(am => am.Actor)
            .ToListAsync();

        return movies;
    }

    public async Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues()
    {
        var response = new NewMovieDropdownsVM()
        {
            Actors = await _context.Actors.OrderBy(n => n.FullName).ToListAsync(),
            Cinemas = await _context.Cinemas.OrderBy(n => n.FullName).ToListAsync(),
            Producers = await _context.Producers.OrderBy(n => n.FullName).ToListAsync()
        };

        return response;
    }

    public async Task UpdateMovieAsync(NewMovieVM data)
    {
        var dbMovie = await _context.Movies.FirstOrDefaultAsync(n => n.id == data.id);

        if (dbMovie != null)
        {
            dbMovie.Name = data.Name;
            dbMovie.Description = data.Description;
            dbMovie.Price = data.Price;
            dbMovie.ImageUrl = data.ImageURL;
            dbMovie.Startdate = data.StartDate;
            dbMovie.Enddate = data.EndDate;
            dbMovie.movieCategory = data.MovieCategory;
            dbMovie.ProducerId = data.ProducerId;
            await _context.SaveChangesAsync();
        }

        // Remove existing actors
        var existingActorsDb = _context.Actors_Movies.Where(n => n.MovieId == data.id).ToList();
        _context.Actors_Movies.RemoveRange(existingActorsDb);

        // Remove existing cinemas
        var existingCinemasDb = _context.Movies_Cinemas.Where(n => n.MovieId == data.id).ToList();
        _context.Movies_Cinemas.RemoveRange(existingCinemasDb);

        await _context.SaveChangesAsync();

        // Add Movie Actors
        foreach (var actorId in data.ActorIds)
        {
            var newActorMovie = new Actor_Movies()
            {
                MovieId = data.id,
                ActorId = actorId
            };
            await _context.Actors_Movies.AddAsync(newActorMovie);
        }

        // Add Movie Cinemas
        foreach (var cinemaId in data.CinemaIds)
        {
            var newMovieCinema = new Movie_Cinema()
            {
                MovieId = data.id,
                CinemaId = cinemaId
            };
            await _context.Movies_Cinemas.AddAsync(newMovieCinema);
        }

        await _context.SaveChangesAsync();
    }


    public override async Task DeleteAsync(int id)
    {
        // First, get the movie to ensure it exists
        var movie = await _context.Movies.FirstOrDefaultAsync(n => n.id == id);
        if (movie == null) return;

        // Check if movie has any order history
        var hasOrderHistory = await _context.OrderItems.AnyAsync(oi => oi.MovieId == id);
        if (hasOrderHistory)
        {
            throw new InvalidOperationException("Cannot delete movie that has order history. Consider marking it as inactive instead.");
        }

        // Remove all Actor_Movies relationships (will cascade automatically)
        var actorMovies = _context.Actors_Movies.Where(am => am.MovieId == id);
        _context.Actors_Movies.RemoveRange(actorMovies);

        // Remove all Movie_Cinema relationships (will cascade automatically)
        var movieCinemas = _context.Movies_Cinemas.Where(mc => mc.MovieId == id);
        _context.Movies_Cinemas.RemoveRange(movieCinemas);

        // Remove all ShoppingCartItems that reference this movie (will cascade automatically)
        var shoppingCartItems = _context.ShoppingCartItems.Where(sci => sci.MovieId == id);
        _context.ShoppingCartItems.RemoveRange(shoppingCartItems);

        // Finally, remove the movie itself
        _context.Movies.Remove(movie);

        // Save all changes
        await _context.SaveChangesAsync();
    }

    // Simple method to add a Movie entity directly
    public async Task AddNewMovieAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
    }

    // Method to add actors to a movie
    public async Task AddActorsToMovieAsync(int movieId, List<int> actorIds)
    {
        foreach (var actorId in actorIds)
        {
            var newActorMovie = new Actor_Movies()
            {
                MovieId = movieId,
                ActorId = actorId
            };
            await _context.Actors_Movies.AddAsync(newActorMovie);
        }
        await _context.SaveChangesAsync();
    }

    // Method to add cinemas to a movie
    public async Task AddCinemasToMovieAsync(int movieId, List<int> cinemaIds)
    {
        foreach (var cinemaId in cinemaIds)
        {
            var newMovieCinema = new Movie_Cinema()
            {
                MovieId = movieId,
                CinemaId = cinemaId
            };
            await _context.Movies_Cinemas.AddAsync(newMovieCinema);
        }
        await _context.SaveChangesAsync();
    }
}