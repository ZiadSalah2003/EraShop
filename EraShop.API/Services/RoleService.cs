using EraShop.API.Contracts.Roles;
using EraShop.API.Entities;
using EraShop.API.Errors;
using EraShop.API.Persistence;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace EraShop.API.Services
{
	public class RoleService : IRoleService
	{
		private readonly RoleManager<ApplicationRole> _roleManager;
		public RoleService(RoleManager<ApplicationRole> roleManager)
		{
			_roleManager = roleManager;
		}
		public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default)
		{
			return await _roleManager.Roles
				.Where(x => !x.IsDefault && (!x.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))
				.ProjectToType<RoleResponse>()
				.ToListAsync(cancellationToken);
		}
		public async Task<Result<RoleDetailResponse>> GetAsync(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role is null)
				return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

			var permissions = await _roleManager.GetClaimsAsync(role);

			var response = new RoleDetailResponse(role.Id, role.Name!, role.IsDefault);
			return Result.Success(response);
		}
		public async Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request)
		{
			var roleIsExists = await _roleManager.RoleExistsAsync(request.Name);
			if (roleIsExists)
				return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);

			var role = new ApplicationRole
			{
				Name = request.Name,
				ConcurrencyStamp = Guid.NewGuid().ToString()
			};
			var result = await _roleManager.CreateAsync(role);
			if (result.Succeeded)
			{
				var response = new RoleDetailResponse(role.Id, role.Name, role.IsDefault);
				return Result.Success(response);
			}

			var error = result.Errors.First();
			return Result.Failure<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
		}
		public async Task<Result> UpdateAsync(string id, RoleRequest request)
		{
			var roleIsExists = await _roleManager.Roles.AnyAsync(x => x.Name == request.Name && x.Id != id);
			if (roleIsExists)
				return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);

			var role = await _roleManager.FindByIdAsync(id);
			if (role is null)
				return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

			role.Name = request.Name;
			var result = await _roleManager.UpdateAsync(role);
			return Result.Success();
		}

		public async Task<Result> ToggleStatusAsync(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role is null)
				return Result.Failure(RoleErrors.RoleNotFound);

			role.IsDeleted = !role.IsDeleted;
			await _roleManager.UpdateAsync(role);
			return Result.Success();
		}
	}
}
