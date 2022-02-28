using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JWTWebAPI.Data;
using JWTWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTWebAPI.Services
{
    public class UserService : IUser
    {
        
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public UserService(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<User> AddUser(User inputUser)
        {
            User newUser = new()
            {
                Username = inputUser.Username,
                Password = CreatePasswordHash(inputUser.Password)
            };
            
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;

        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<User> GetUser(string username)
        {
            User user = await _context.Users.FirstAsync(u => u.Username.Equals(username));
            if (user == null)
            {
                throw new Exception("User not found");
            }
            Console.WriteLine(user.Username);
            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task RemoveUser(int userId)
        {
            _context.Remove(GetUser(userId));
            await _context.SaveChangesAsync();
        }

        private static string CreatePasswordHash(string inputPassword)
        {
            
            Console.WriteLine("Input password: " + inputPassword);

            using var hmac = new HMACSHA256();
            byte[] passwordSalt = hmac.Key;
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));

            return Convert.ToBase64String(passwordSalt) + ":" +
                   Convert.ToBase64String(passwordHash);
            
        }

        public async Task<bool> VerifyPasswordHash(string inputPassword, string dbPassword)
        {
            
            byte[] dbSaltPassword = Convert.FromBase64String(dbPassword.Split(":")[0]);
            byte[] dbHashPassword = Convert.FromBase64String(dbPassword.Split(":")[1]);

            using var hmac = new HMACSHA256(dbSaltPassword);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
            return computedHash.SequenceEqual(dbHashPassword);
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            User user = await _context.Users.FirstAsync(u => u.Username.Equals(username)) ?? throw new Exception($"No user named {username}");
            if (user == null)
            {
                throw new Exception("User not found");
            }
            Console.WriteLine(user.Username);
            return user;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return user.Password.Equals(password);
        }
    }
}