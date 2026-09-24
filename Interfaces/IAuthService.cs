using ToDoList.DTOs;
using ToDoList.Entities;
using System.IdentityModel.Tokens.Jwt;
using ToDoList.Common;

namespace ToDoList.Interfaces
{
    public interface IAuthService
    {

        public Task<Result<AuthResult>> Register(CreatedUser user);
        public Task<Result<AuthResult>> Login(string email, string password);
        public Task<Result<AuthResult>> Refresh(string oldToken);
        
    }
     
}