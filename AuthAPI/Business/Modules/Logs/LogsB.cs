using AuthAPI.Business.Modules.Logs.Interfaces;
using AuthAPI.Data.Modules.Auth;
using AuthAPI.Data.Modules.Logs.Interfaces;

namespace AuthAPI.Business.Modules.Logs
{
    public class LogsB : ILogsB
    {
        private readonly ILogs _logs;

        public LogsB(ILogs logs)
        {
            _logs = logs;
        }
        public async Task LogAuditAsync(int userId, string action, string ipAddress, string details)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID");
            }

            if (string.IsNullOrWhiteSpace(action))
            {
                throw new ArgumentException("Action is required");
            }

            // Validaciones adicionales pueden ser aplicadas aquí
            await _logs.CreateAuditLogAsync(userId, action, ipAddress, details);
        }
    }
}
