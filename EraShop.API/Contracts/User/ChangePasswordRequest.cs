namespace EraShop.API.Contracts.User
{
    public record ChangePasswordRequest
    (
        string CurrentPassword,
        string NewPassword
    );
}
