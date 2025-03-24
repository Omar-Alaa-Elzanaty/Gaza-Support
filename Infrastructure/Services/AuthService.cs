using Gaza_Support.Domains.Models;
using Infrastructure.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void CreatePasswordHash(User user, string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHashed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPassword(User user, string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHashed[i])
                {
                    return false;
                }
            }
            return true;
        }

        public string GenerateToken(string userId, string email, string role, bool rembemerMe = false, bool isMobile = false)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecureKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Email,email!),
                new(ClaimTypes.Role,role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: rembemerMe == true ? isMobile == true ? DateTime.Now.AddDays(double.Parse(_configuration["Jwt:MobileExpireInDays"]!)) :
                 DateTime.Now.AddDays(double.Parse(_configuration["Jwt:ExpireInDays"]!))
                : DateTime.Now.AddHours(6), // Token expiration time
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
