using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EraShop.API.Abstractions;
using EraShop.API.Abstractions.Consts;
using EraShop.API.Contracts.Files;
using EraShop.API.Errors;
using EraShop.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EraShop.API.Services
{
	public class CloudinaryService : IFileService
	{
		private readonly Cloudinary _cloudinary;
		private readonly IWebHostEnvironment _environment;

		public CloudinaryService(IOptions<CloudinarySettings> config, IWebHostEnvironment environment)
		{
			var account = new Account(
				config.Value.CloudName,
				config.Value.ApiKey,
				config.Value.ApiSecret
			);

			_cloudinary = new Cloudinary(account);
			_environment = environment;
		}
		public async Task<Result<FileUploadResponse>> UploadToCloudinaryAsync(IFormFile file)
		{
			if (file == null || file.Length <= 0)
				return Result.Failure<FileUploadResponse>(FileErrors.FileEmpty);

			using var stream = file.OpenReadStream();
			RawUploadParams uploadParams;
			uploadParams = new ImageUploadParams
			{
				File = new FileDescription(file.FileName, stream)
			};
			var result = await _cloudinary.UploadAsync(uploadParams);

			if (result.Error != null)
				return Result.Failure<FileUploadResponse>(new Abstractions.Error(result.Error.Message, result.Error.Message, StatusCodes.Status400BadRequest));

			var response = new FileUploadResponse(
				result.Url.ToString(),
				result.PublicId,
				result.SecureUrl.ToString(),
				result.Format
			);
			return Result.Success(response);
		}
		public async Task<Result> DeleteFromCloudinaryAsync(string publicId)
		{
			if (string.IsNullOrEmpty(publicId))
				return Result.Failure(FileErrors.EmptyPublicId);

			var deleteParams = new DeletionParams(publicId);
			var result = await _cloudinary.DestroyAsync(deleteParams);

			if (result.Error != null)
				return Result.Failure(new Abstractions.Error(result.Error.Message, result.Error.Message, StatusCodes.Status400BadRequest));

			if (result.StatusCode != System.Net.HttpStatusCode.OK)
				return Result.Failure(FileErrors.DeleteFailed);

			return Result.Success();
		}
		public string ExtractPublicIdFromUrl(string url)
		{
			if (string.IsNullOrEmpty(url))
				return string.Empty;
			 
			try
			{
				var uri = new Uri(url);
				var pathSegments = uri.LocalPath.Split('/');
				var uploadIndex = Array.IndexOf(pathSegments, "upload");

				if (uploadIndex == -1)
					return string.Empty;

				var publicIdWithExtension = pathSegments[pathSegments.Length - 1];
				var lastDotIndex = publicIdWithExtension.LastIndexOf('.');

				if (lastDotIndex > 0)
					return publicIdWithExtension.Substring(0, lastDotIndex);

				return publicIdWithExtension;
			}
			catch
			{
				return string.Empty;
			}
		}
	}
}
