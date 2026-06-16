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
        public async Task<ActionResult<List<UserLightDto>>> GetAll()
        {
            List<UserLightDto> data = await _usersService.GetAll();
            return Ok(data);
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<UserLightDto>> AddUser([FromBody] CreateUserDto user)
        {
            UserLightDto created = await _usersService.AddUser(user);
            return Ok(created);
        }
    }
}