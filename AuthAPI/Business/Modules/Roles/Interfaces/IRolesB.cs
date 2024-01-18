namespace AuthAPI.Business.Modules.Roles.Interfaces
{
    public interface IRolesB
    {
        Task AssignRoleAsync(int userId, int roleId);
        Task CreateRoleAsync(string roleName, string description);
    }
}
