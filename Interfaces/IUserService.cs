using ToDoLsit.Migrations;
using ToDoList.DTOs;

using ToDoList.Entities;
namespace ToDoList.Interfaces
{
    public interface IUserService
    {
        
        public UserToFront? AddUserAsync(UserToBack user);
        public bool UpdateUserAsync(UpdatedUser user);
        public User? GetUserbyEmailAsync(string email);
        public User? GetUserByIdAsync(Guid userId);
        public bool DeleteUserByIdAsync(Guid userId);
        public bool UpdatePasswordAsync(string newPassword);

    }
}