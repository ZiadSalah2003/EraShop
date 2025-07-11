﻿using EraShop.API.Abstractions;

namespace EraShop.API.Errors
{
	public static class RoleErrors
	{
		public static readonly Error RoleNotFound = new("Role.NotFound", "Role is not found", StatusCodes.Status404NotFound);
		public static readonly Error DuplicatedRole = new("Role.DuplicatedRole", "Another role with the same name is already exists", StatusCodes.Status409Conflict);
	}
}
