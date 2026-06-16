using AppBackendCore2026.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace UsersBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<UserLight>>> GetAll()
        {
            List<UserLight> data = await _usersService.GetAll();
            return Ok(data);
        }
    }
}