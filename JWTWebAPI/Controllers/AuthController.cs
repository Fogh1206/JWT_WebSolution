using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWTWebAPI.Data;
using JWTWebAPI.Models;
using JWTWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly DataContext _context;

        public AuthController(DataContext _context, IUser userService)
        {
            _userService = userService;
            this._context = _context;
        }

        [Route("/login")]
        [HttpPost]
        public async Task<ActionResult<AuthenticatedUserModel>> CreateToken([FromBody] AuthenticationUserModel request)
        {
            if (await IsValidUsernameAndPassword(request.Username, request.Password))
            {
                return Ok(await GenerateToken(request.Username));
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {

            User user;
            try
            {
                user = await _userService.FindByUsernameAsync(username);
            }
            catch (Exception e)
            {
                Console.WriteLine("This is a message, not an exception: " + e.Message);
                return false;
            }

            return await _userService.VerifyPasswordHash(password, user.Password);

        }

        private async Task<AuthenticatedUserModel> GenerateToken(string username)
        {
            User user;
            try
            {
                user = await _userService.FindByUsernameAsync(username);
            }
            catch (Exception e)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMinutes(10)).ToUnixTimeSeconds().ToString())
            };

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisIsASuperSecretKey")),
                        SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            AuthenticatedUserModel authenticatedUser = new()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Username = username
            };

            return authenticatedUser;

        }
        
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User request)
        {
            return Ok(await _userService.AddUser(request));
        }
        
    }
}