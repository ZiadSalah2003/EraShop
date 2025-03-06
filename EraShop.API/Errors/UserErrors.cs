﻿using EraShop.API.Abstractions;

namespace EraShop.API.Errors
{
	public static class UserErrors
	{
		public static readonly Error InvalidCredentails = new("User.InvalidCredentials", "Invalid Email Or Password", StatusCodes.Status401Unauthorized);

		public static readonly Error InvalidJwtToken = new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

        public static readonly Error DublicatedEmail =
         new("User.DublicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);

        public static readonly Error InvalidCode =
           new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicatedConfirmation =
      new("User.DuplicatedInformation", "Email Already Confirmed", StatusCodes.Status400BadRequest);

        public static readonly Error EmailNotConfirmed =
         new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

		public static readonly Error UserEmailNotFound = new("User.UserEmailNotFound", "User Email NotFound", StatusCodes.Status404NotFound);
		public static readonly Error InvalidRefreshToken = new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

	}
}
