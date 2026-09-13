using ToDoList.DTOs;
using ToDoList.Entities;

namespace ToDoList.Interfaces
{
    public interface IAuthService
    {
        public string ToHashPassword(string password);
        public bool VerifyPassword(string password, string Hash);
        public string CreateJWTToken(User user);
    }
}