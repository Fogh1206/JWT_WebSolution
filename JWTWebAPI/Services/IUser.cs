using System.Collections.Generic;
using System.Threading.Tasks;
using JWTWebAPI.Models;

namespace JWTWebAPI.Services
{
    public interface IUser
    {
        Task<User> AddUser(AuthenticationUserModel inputUser);
        Task<User> GetUser(int id);
        Task<User> GetUser(string username);
        Task<List<User>> GetUsers();
        Task RemoveUser(int userId);
        Task<bool> VerifyPasswordHash(string inputPassword, string dbPassword);
        string CreateToken(User user);
        Task<User> FindByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(User user, string password);

        Task<User> AddRole(string user , Roles role);

        Task<User> RemoveRole(string username , Roles role);
        
        Task<List<string>> GetRoles(string username);

    }
}