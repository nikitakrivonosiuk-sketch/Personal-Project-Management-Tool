using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.DTO.UserDTOs;
using PersonalProjectApi.Services;

namespace PersonalProjectApi.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly AccountService _accountService;
        public UserController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegistrationRequest request)
        {
            var result = await _accountService.CreateUserAsync(request);

            if (result.Succeeded)
                return StatusCode(StatusCodes.Status201Created);

            var isDuplicate = result.Errors.Any(error => error.Code is "DuplicateEmail" or "DuplicateUserName");

            if (isDuplicate)
                return Conflict(new { message = "This email is already registrated." });

            return BadRequest(new {message = "Registration failed. Check the email format and password requirements." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginRequestDto loginRequestDto)
        {
            var result = await _accountService.LoginUserAsync(loginRequestDto);

            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
