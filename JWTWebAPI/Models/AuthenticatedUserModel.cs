namespace JWTWebAPI.Models
{
    public class AuthenticatedUserModel
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
    }
}