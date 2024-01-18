using AuthAPI.Models;

namespace AuthAPI.Data.Modules.Auth.Interfaces
{
    public interface IAuth
    {
        Task CreateUserAsync(UserModel user);
        Task<UserModel> AuthenticateUserAsync(string username);
    }
}
