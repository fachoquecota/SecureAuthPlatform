namespace AuthAPI.Models
{
    public class AuditLogModel
    {
        public int auditLogId { get; set; }
        public int userId { get; set; }
        public UserModel user { get; set; }
        public string action { get; set; }
        public DateTime timestamp { get; set; }
        public string ipAddress { get; set; }
        public string details { get; set; }
    }
}
