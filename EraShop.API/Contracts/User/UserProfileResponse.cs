namespace EraShop.API.Contracts.User
{
    public record UserProfileResponse
    (
        string Email,
        string UserName,
        string FirstName,
        string LastName
    );
}
