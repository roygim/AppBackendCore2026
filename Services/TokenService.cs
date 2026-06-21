using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AppBackendCore2026.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // HS256 requires a >= 256-bit key; derive one from the (possibly short) secret.
        public static SymmetricSecurityKey GetSigningKey(string secret) =>
            new(SHA256.HashData(Encoding.UTF8.GetBytes(secret)));

        public string CreateToken(UserObj user)
        {
            var secret = _configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("Jwt:SecretKey is not configured.");

            var key = GetSigningKey(secret);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", user.Id.ToString(), ClaimValueTypes.Integer),
                //new Claim(JwtRegisteredClaimNames.Email, user.email)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                //expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
