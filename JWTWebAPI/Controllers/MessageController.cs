using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> Get()
        {
            return Ok("Hello");
        }
        
    }
}