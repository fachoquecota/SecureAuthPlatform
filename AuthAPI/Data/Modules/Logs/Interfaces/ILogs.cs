namespace AuthAPI.Data.Modules.Logs.Interfaces
{
    public interface ILogs
    {
        Task CreateAuditLogAsync(int userId, string action, string ipAddress, string details);
    }
}
