using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Entities;
using ToDoList.Interfaces;

namespace ToDoList.Services
{
    public class AuthService:IAuthService
    {
        private readonly IConfiguration _config;
        public AuthService(IConfiguration config)
        {
            _config = config;
        }
        public string ToHashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public bool VerifyPassword(string password, string Hash)
        {
            return BCrypt.Net.BCrypt.Verify(password,Hash);
        }
        public JwtSecurityToken CreateJWTToken(User user)
        {
            var claims = new List<Claim> {new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())};
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((_config["JWToptions:Key"])));
            var jwt = new JwtSecurityToken(
                issuer: _config["JWToptions:Issuer"],
                audience: _config["JWToptions:Audiencer"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature)
            );
            return jwt;
        }
    }    
}
