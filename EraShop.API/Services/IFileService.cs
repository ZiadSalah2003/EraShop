namespace EraShop.API.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions, string folderName);
        void DeleteFile(string file,string folderName);

    }
}
