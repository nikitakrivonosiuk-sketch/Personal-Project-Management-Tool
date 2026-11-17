using Microsoft.IdentityModel.Tokens;
using PersonalProjectApi.Models.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalProjectApi.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTime ExpiresAtUtc) CreateAccessToken(AppUser user)
        {
            var secretKey = _configuration["Jwt:SigningKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };


            var minutes = _configuration.GetValue<int>("Jwt:AccessTokenMinutes");
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(minutes);

            if (minutes <= 0)
                throw new InvalidOperationException(
                    "Jwt:AccessTokenMinutes must be greater than zero.");

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                notBefore: now,
                expires: expires,
                signingCredentials: credentials,
                claims: claims);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return (accessToken, expires);
        }
    }
}
