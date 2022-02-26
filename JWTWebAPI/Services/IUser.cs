using System.Threading.Tasks;
using JWTWebAPI.Models;

namespace JWTWebAPI.Services
{
    public interface IUser
    {
        Task<User> AddUser(UserDto inputUser);
        Task<User> GetUser(int id);
        bool VerifyPasswordHash(string inputPassword, string dbPassword);
        string CreateToken(User user);
    }
}