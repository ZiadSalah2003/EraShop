
namespace EraShop.API.Services
{
    public class FileService (IWebHostEnvironment environment): IFileService
    {
        private readonly IWebHostEnvironment _environment = environment;

		public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions, string subfolder)
		{
			if (imageFile is null)
			{
				throw new ArgumentNullException(nameof(imageFile));
			}

            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads");

			// Ensure the directory exists
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			// Get the file extension from the uploaded file
			var extension = Path.GetExtension(imageFile.FileName);
			if (!allowedFileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
			{
				throw new ArgumentException($"Only {string.Join(", ", allowedFileExtensions)} are allowed.");
			}

			// Generate a unique name for the file
			var fileName = $"{Guid.NewGuid()}{extension}";
			var fileNamePath = Path.Combine(path, fileName);

			// Save the file to the uploads directory
			using var stream = new FileStream(fileNamePath, FileMode.Create);
			await imageFile.CopyToAsync(stream);

			return fileName; // Return the saved file name
		}

        public void DeleteFile(string file, string subfolder)
        {
           if(string.IsNullOrEmpty(file))
            {
                throw new ArgumentNullException(nameof(File));
            }

           var contentPath = _environment.ContentRootPath;

            var path = Path.Combine(contentPath, $"Uploads", file);

			if (!File.Exists(path))
			{
				throw new FileNotFoundException($"Invalid File Path");
			}
			File.Delete(path);
		}

	}
}
