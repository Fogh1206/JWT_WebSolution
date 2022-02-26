using System.Collections.Generic;
using System.Threading.Tasks;
using JWTWebAPI.Data;
using JWTWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly DataContext _context;
        
        public UserController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            return Ok(await _context.Users.ToListAsync());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) { 
                return BadRequest("User not found.");
            }
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<List<User>>> UpdateUser(User request)
        {
            var dbUser = await _context.Users.FindAsync(request.Id);
            if (dbUser == null) { 
                return BadRequest("User not found.");
            }

            dbUser.Username = request.Username;
            dbUser.Password = request.Password;

            await _context.SaveChangesAsync();
            
            return Ok(await _context.Users.ToListAsync());

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<User>>> Delete(int id)
        {
            var dbUser = await _context.Users.FindAsync(id);
            if (dbUser == null) { 
                return BadRequest("User not found.");
            }

            _context.Users.Remove(dbUser);
            await _context.SaveChangesAsync();

            return Ok(await _context.Users.ToListAsync());
        }


    }
}