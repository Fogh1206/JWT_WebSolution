using System.Threading.Tasks;
using JWTBlazorApp.Models;

namespace JWTBlazorApp.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication);
        Task Logout();
    }
}