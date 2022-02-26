using System;
using System.Threading.Tasks;
using JWTWebAPI.Models;
using JWTWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JWTWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUser _userService;

        public AuthController(IUser userService)
        {
            _userService = userService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            return Ok(await _userService.AddUser(request));
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(User request)
        {

            User user;
            try
            {
                user = await _userService.GetUser(request.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (!_userService.VerifyPasswordHash(request.Password, user.Password))
            {
                return BadRequest("Wrong password");
            }

            string token = _userService.CreateToken(request);
            
            return Ok(token);
        }
    }
}