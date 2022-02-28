using System.ComponentModel.DataAnnotations;

namespace JWTBlazorApp.Models
{
    // This is the User data transfer object.
    public class AuthenticationUserModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        
    }
}