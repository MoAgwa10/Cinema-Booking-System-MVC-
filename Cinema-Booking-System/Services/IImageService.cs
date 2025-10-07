namespace Cinema_Booking_System.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string entityType);
        string GetDefaultImagePath(string entityType);
        bool IsValidImageFile(IFormFile file);
    }
}
