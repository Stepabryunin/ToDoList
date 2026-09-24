using ToDoLsit.Migrations;
using ToDoList.DTOs;
using ToDoList.Common;

using ToDoList.Entities;
namespace ToDoList.Interfaces
{
    public interface IUserServiceInternal
    {
        

        public Task<User?> CreateUserAsync(CreatedUser createdUser);
        public Task<User?>  GetEntitybyEmailAsync(string Email);
        public Task<User?> GetEntityByIdAsync(Guid Id);
        // public Task<Guid?> GetIdByEmail(string email);
        // // public Task<bool> DeleteUserByIdAsync(Guid userId);

    }
}