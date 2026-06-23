using AppBackendCore2026.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsersBackend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> AddUser([FromBody] CreateUserDto user)
        {
            var response = await _usersService.AddUser(user);

            if (!response.success)
            {
                return response.error switch
                {
                    ErrorType.UserAlreadyExists => BadRequest(new { error = response.error, message = "User already exists" }),
                    _ => StatusCode(500, "error")
                };
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto credentials)
        {
            var response = await _usersService.Login(credentials.email, credentials.password);

            if (!response.success)
            {
                return response.error switch
                {
                    ErrorType.UserNotFound => NotFound(new { error = response.error, message = "User not found" }),
                    ErrorType.InvalidPassword => BadRequest(new { error = response.error, message = "Invalid password" }),
                    _ => StatusCode(500, "error")
                };
            }

            Response.Cookies.Append("userToken", response.data!.accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });

            return Ok(response);
        }

        [HttpDelete("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("userToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });

            return Ok(new ResponseObj<object>
            {
                success = true,
                message = "user logout"
            });
        }

        [Authorize]
        [HttpPost("loaduser")]
        public async Task<ActionResult> LoadUser()
        {
            var userId = User.FindFirst("userId")?.Value;
            if (!int.TryParse(userId, out var id))
                return Unauthorized();

            var response = await _usersService.GetUserById(id);

            if (!response.success)
            {
                return response.error switch
                {
                    ErrorType.UserNotFound => NotFound(new { error = response.error, message = "User not found" }),
                    _ => StatusCode(500, "error")
                };
            }

            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            var response = await _usersService.GetAll();
            return Ok(response);
        }

        [Authorize]
        [HttpPut("update/{userId}")]
        public async Task<ActionResult> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserDto user)
        {
            var response = await _usersService.UpdateUser(userId, user);

            if (!response.success)
            {
                return response.error switch
                {
                    ErrorType.UserNotFound => NotFound(new { error = response.error, message = "User not found" }),
                    _ => StatusCode(500, "error")
                };
            }

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("delete/{userId}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int userId)
        {
            var response = await _usersService.DeleteUser(userId);

            if (!response.success)
            {
                return response.error switch
                {
                    ErrorType.UserNotFound => NotFound(new { error = response.error, message = "User not found" }),
                    _ => StatusCode(500, "error")
                };
            }

            return Ok(response);
        }
    }
}