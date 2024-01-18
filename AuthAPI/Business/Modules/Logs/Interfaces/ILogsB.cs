namespace AuthAPI.Business.Modules.Logs.Interfaces
{
    public interface ILogsB
    {
        Task LogAuditAsync(int userId, string action, string ipAddress, string details);
    }
}
