using Cinema_Booking_System.Models;
using Cinema_Booking_System.ViewModel;

public interface IMoviesService : IEntityBaseRepository<Movie>
{
    Task<Movie> GetMovieByIdAsync(int id);
    Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues();
    Task AddNewMovieAsync(NewMovieVM data);
    Task AddNewMovieAsync(Movie movie);
    Task UpdateMovieAsync(NewMovieVM data);
    Task AddActorsToMovieAsync(int movieId, List<int> actorIds);
    Task AddCinemasToMovieAsync(int movieId, List<int> cinemaIds);
    Task<IEnumerable<Movie>> GetAllMoviesWithRelationsAsync();

}
