using System.Data.SqlClient;
using System.Data;
using AuthAPI.Data.Modules.Logs.Interfaces;

namespace AuthAPI.Data.Modules.Logs
{
    public class LogsSP : ILogs
    {
        private readonly Connection _connectionHelper;

        public LogsSP(Connection connectionHelper)
        {
            _connectionHelper = connectionHelper;
        }
        public async Task CreateAuditLogAsync(int userId, string action, string ipAddress, string details)
        {
            using (var connection = new SqlConnection(_connectionHelper.getCadenaSQL()))
            {
                await connection.OpenAsync();
                try
                {
                    using (var command = new SqlCommand("spCreateAuditLog", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@userId", userId));
                        command.Parameters.Add(new SqlParameter("@action", action));
                        command.Parameters.Add(new SqlParameter("@ipAddress", ipAddress));
                        command.Parameters.Add(new SqlParameter("@details", details));

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
