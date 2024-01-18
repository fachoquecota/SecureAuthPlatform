using System.Data.SqlClient;
using System.Data;
using AuthAPI.Data.Modules.Roles.Interfaces;

namespace AuthAPI.Data.Modules.Roles
{
    public class RolesSP : IRoles
    {
        private readonly Connection _connectionHelper;

        public RolesSP(Connection connectionHelper)
        {
            _connectionHelper = connectionHelper;
        }

        public async Task AssignRoleAsync(int userId, int roleId)
        {
            using (var connection = new SqlConnection(_connectionHelper.getCadenaSQL()))
            {
                await connection.OpenAsync();
                try
                {
                    using (var command = new SqlCommand("spAssignRole", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@userId", userId));
                        command.Parameters.Add(new SqlParameter("@roleId", roleId));

                        await command.ExecuteNonQueryAsync();
                    }
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }

        public async Task CreateRoleAsync(string roleName, string description)
        {
            using (var connection = new SqlConnection(_connectionHelper.getCadenaSQL()))
            {
                await connection.OpenAsync();
                try
                {
                    using (var command = new SqlCommand("spCreateRole", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@roleName", roleName));
                        command.Parameters.Add(new SqlParameter("@description", description));

                        await command.ExecuteNonQueryAsync();
                    }
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }
    }
}
