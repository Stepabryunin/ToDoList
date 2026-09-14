using ToDoList.DTOs;
using ToDoList.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace ToDoList.Interfaces
{
    public interface IAuthService
    {
        public string ToHashPassword(string password);
        public bool VerifyPassword(string password, string Hash);
        
        public JwtSecurityToken CreateJWTToken(User user);
    }
}