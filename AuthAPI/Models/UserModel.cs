namespace AuthAPI.Models
{
    public class UserModel
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string passwordHash { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime? lastLoginDate { get; set; }
        public bool isActive { get; set; }
    }
}
