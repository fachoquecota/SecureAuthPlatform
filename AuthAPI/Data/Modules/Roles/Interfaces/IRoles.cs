namespace AuthAPI.Data.Modules.Roles.Interfaces
{
    public interface IRoles
    {
        Task AssignRoleAsync(int userId, int roleId);
        Task CreateRoleAsync(string roleName, string description);
    }
}
