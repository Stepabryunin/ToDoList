using ToDoLsit.Migrations;
using ToDoList.DTOs;

using ToDoList.Entities;
namespace ToDoList.Interfaces
{
    public interface IUserService
    {
        
        public Task<User?> AddUserAsync(UserToBack user);
        public Task<User?> GetUserbyEmailAsync(string email);
        public Task<User?> GetUserByIdAsync(Guid Id);
        // public Task<bool> UpdateUserAsync(UpdatedUser user);
        
        // public Task<User?> GetUserByIdAsync(Guid userId);
        // public Task<bool> DeleteUserByIdAsync(Guid userId);
        // public Task<bool> UpdatePasswordAsync(string newPassword);

    }
}