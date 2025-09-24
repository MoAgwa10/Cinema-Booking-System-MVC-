using Cinema_Booking_System.ViewModel;

public interface IMoviesService : IEntityBaseRepository<Movie>
{
    Task<Movie> GetMovieByIdAsync(int id);
    Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues();
    Task AddNewMovieAsync(NewMovieVM data);
    Task UpdateMovieAsync(NewMovieVM data);
}
