namespace EraShop.API.Services
{
    public interface IFileService
    {
		Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions, string subfolder);
		void DeleteFile(string file, string subfolder);

	}
}
