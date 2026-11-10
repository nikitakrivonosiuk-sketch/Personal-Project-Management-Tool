using Microsoft.AspNetCore.Identity;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.UserDTOs;

namespace PersonalProjectApi.Services
{
    public class AccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        public AccountService(UserManager<AppUser> manager, TokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = manager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(UserRegistrationRequest request)
        {
            var user = new AppUser { UserName = request.Email, Email = request.Email};
            var result = await _userManager.CreateAsync(user, request.Password);

            return result;
        }

        public async Task<UserLoginResponseDto?> LoginUserAsync(UserLoginRequestDto loginRequest)
        {
            var foundUser = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (foundUser == null)
            {
                return null;
            }

            var signIn = await _signInManager.CheckPasswordSignInAsync(foundUser, loginRequest.Password, lockoutOnFailure: true);
            if (!signIn.Succeeded)
            {
                return null;
            }

            var token = _tokenService.CreateAccessToken(foundUser);

            return new UserLoginResponseDto(token, DateTime.UtcNow.AddMinutes(15));
        }
    }
}
