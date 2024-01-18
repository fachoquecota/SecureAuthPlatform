namespace AuthAPI.Models
{
    public class SecurityTokenModel
    {
        public int tokenId { get; set; }
        public int userId { get; set; }
        public UserModel user { get; set; }
        public string token { get; set; }
        public DateTime expirationDate { get; set; }
        public string type { get; set; }
    }
}
