using AuthAPI.Models;

namespace AuthAPI.Business.Modules.Auth.Interfaces
{
    public interface IAuthB
    {
        Task CreateUserAsync(UserModel user);
        Task<UserModel> AuthenticateAsync(string username, string password);
    }
}
