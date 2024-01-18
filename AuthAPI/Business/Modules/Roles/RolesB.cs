using AuthAPI.Business.Modules.Roles.Interfaces;
using AuthAPI.Data.Modules.Auth;
using AuthAPI.Data.Modules.Roles.Interfaces;

namespace AuthAPI.Business.Modules.Roles
{
    public class RolesB : IRolesB
    {
        private readonly IRoles _roles;

        public RolesB(IRoles roles)
        {
            _roles = roles;
        }
        public async Task AssignRoleAsync(int userId, int roleId)
        {
            await _roles.AssignRoleAsync(userId, roleId);
        }

        public async Task CreateRoleAsync(string roleName, string description)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name is required");
            }

            // Más validaciones pueden ser necesarias, como comprobar si el nombre del rol ya existe
            await _roles.CreateRoleAsync(roleName, description);
        }
    }
}
