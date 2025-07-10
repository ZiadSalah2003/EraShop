using EraShop.API.Abstractions;
using EraShop.API.Contracts.Files;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EraShop.API.Services
{
    public interface IFileService
    {
		Task<Result<FileUploadResponse>> UploadToCloudinaryAsync(IFormFile file);
		Task<Result> DeleteFromCloudinaryAsync(string publicId);
		string ExtractPublicIdFromUrl(string url);
	}
}
