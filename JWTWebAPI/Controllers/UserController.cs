using System.Collections.Generic;
using System.Threading.Tasks;
using JWTWebAPI.Data;
using JWTWebAPI.Models;
using JWTWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUser _userService;
        
        public UserController(IUser userService)
        {
            _userService = userService;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _userService.RemoveUser(id);
            return Ok("");
        }
        
        
        
        [Route("/addRole/{username}/{role}")]
        [HttpPut]
        public async Task<ActionResult<AuthenticatedUserModel>> AddRole(string username,  Roles role)
        {
            await _userService.AddRole(username, role);
            return Ok();
        }

        [Route("/getRoles/{username}")]
        [HttpGet]
        public async Task<ActionResult<List<Roles>>> GetRoles(string username)
        {
            return Ok(await _userService.GetRoles(username));
        }

        [Route("/deleteRole/{username}/{role}")]
        [HttpDelete]
        public async Task<ActionResult<AuthenticatedUserModel>> DeleteRole(string username,  Roles role)
        {
            await _userService.RemoveRole(username, role);
            return Ok();
        }

    }
}