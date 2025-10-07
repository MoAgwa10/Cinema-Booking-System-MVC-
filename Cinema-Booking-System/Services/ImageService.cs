namespace Cinema_Booking_System.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string entityType)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return GetDefaultImagePath(entityType);
            }

            if (!IsValidImageFile(imageFile))
            {
                throw new ArgumentException("Invalid image file. Only JPG, PNG, and WEBP files are allowed.");
            }

            if (imageFile.Length > MaxFileSize)
            {
                throw new ArgumentException("File size exceeds the maximum limit of 5MB.");
            }

            // Create entity-specific upload directory
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", entityType.ToLower());
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique filename
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Return relative path for web access
            return $"/uploads/{entityType.ToLower()}/{fileName}";
        }

        public string GetDefaultImagePath(string entityType)
        {
            return $"/images/default-{entityType.ToLower()}.png";
        }

        public bool IsValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLower();
            return _allowedExtensions.Contains(extension);
        }
    }
}
