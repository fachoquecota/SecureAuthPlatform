using AuthAPI.Business.Modules.Auth.Interfaces;
using AuthAPI.Data.Modules.Auth.Interfaces;
using AuthAPI.Models;
using BCrypt.Net;

namespace AuthAPI.Business.Modules.Auth
{
    public class AuthB : IAuthB
    {
        private readonly IAuth _auth;

        public AuthB(IAuth auth)
        {
            _auth = auth;
        }
        public async Task CreateUserAsync(UserModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(user.passwordHash))
            {
                throw new ArgumentException("Password is required");
            }

            user.passwordHash = BCrypt.Net.BCrypt.HashPassword(user.passwordHash);
            await _auth.CreateUserAsync(user);
        }

        public async Task<UserModel> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            // Obtén el usuario solo por el nombre de usuario
            var user = await _auth.AuthenticateUserAsync(username);

            // Verifica si el usuario existe y si la contraseña coincide
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.passwordHash))
            {
                return null;
            }
            return user;
        }

    }
}
