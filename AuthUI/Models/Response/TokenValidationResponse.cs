namespace AuthUI.Models.Response
{
    public class TokenValidationResponse
    {
        public bool IsValid { get; set; }
        public string Username { get; set; }
        public string[] Modules { get; set; }
    }
}
