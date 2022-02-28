using System.Collections.Generic;
using System.Threading.Tasks;
using JWTWebAPI.Models;

namespace JWTWebAPI.Services
{
    public interface IUser
    {
        Task<User> AddUser(User inputUser);
        Task<User> GetUser(int id);
        Task<User> GetUser(string username);
        Task<List<User>> GetUsers();
        Task RemoveUser(int userId);
        Task<bool> VerifyPasswordHash(string inputPassword, string dbPassword);
        string CreateToken(User user);
        Task<User> FindByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}