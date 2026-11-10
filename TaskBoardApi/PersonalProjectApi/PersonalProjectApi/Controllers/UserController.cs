using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalProjectApi.Models.DTO.UserDTOs;
using PersonalProjectApi.Services;

namespace PersonalProjectApi.Controllers
{
    [Route("api/auth/")]
    [ApiController]
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

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginRequestDto loginRequestDto)
        {
            var result = await _accountService.LoginUserAsync(loginRequestDto);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
