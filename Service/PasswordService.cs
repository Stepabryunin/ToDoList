using ToDoList.Interfaces;

namespace ToDoList.Services
{
    
    public class PasswordService: IPasswordService
    {
        public string ToHashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public bool VerifyPassword(string password, string Hash)
        {
            return BCrypt.Net.BCrypt.Verify(password,Hash);
        }
    }
}