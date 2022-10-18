using System.Threading.Tasks;
using JWTBlazorApp.Models;

namespace JWTBlazorApp.Data
{
    public interface IUser
    {
        Task<AuthenticatedUserModel> AddUser(AuthenticationUserModel inputAuthenticationUser);
        Task<string> LoginUser(AuthenticationUserModel inputAuthenticationUser);

        Task<List<User>> RetrieveAllUsers();
    }
}