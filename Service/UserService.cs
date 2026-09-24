
using ToDoList.Context;
using ToDoList.DTOs;
using ToDoList.Mappers;
using ToDoList.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using ToDoList.Interfaces;
using ToDoList.Common;

namespace ToDoList.Services
{
    public class UserService: IUserService, IUserServiceInternal 
    {
        private readonly MyDbContext _db;
        private readonly UserMapper _um;
        private readonly IPasswordService _PS;

        public UserService(MyDbContext db, UserMapper um, IPasswordService PS)
        {
            _db=db;
            _um=um;
            _PS=PS;
        }
        async public Task<User?> CreateUserAsync(CreatedUser createdUser)
        {
            User user = _um.ToEntity(createdUser);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }
        async public Task<User?>  GetEntitybyEmailAsync(string Email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email==Email);
        }
        async public Task<UserToFront?> GetUserbyEmailAsync(string Email)
        {
            var user = await GetEntitybyEmailAsync(Email);
            return user == null? null : _um.ToFront(user); 
        }

        async public Task<User?> GetEntityByIdAsync(Guid Id)
        {
            return await _db.Users.FirstOrDefaultAsync(u=>u.Id==Id);
        }
        
        async public Task<UserToFront?> GetUserByIdAsync(Guid Id)
        {
            var user = await GetEntityByIdAsync(Id);
            return user == null? null: _um.ToFront(user);

        }

        async public Task<Result<UserToFront>> UpdateUserAsync(UpdatedUser updatedUser, Guid UserId)
        {
            var user = await GetEntityByIdAsync(UserId);
            if (user != null)
            {
                if (updatedUser.Email != null)
                {
                    if (null !=await GetUserbyEmailAsync(updatedUser.Email))
                        return new Result<UserToFront>(ResultStatus.Conflict, "Данный email уже занят");
                    user.Email = updatedUser.Email;
                    user.UpdatedAt = DateTime.UtcNow;
                }
                if (updatedUser.FirtName !=null)
                {
                    user.FirstName = updatedUser.FirtName;
                    user.UpdatedAt =DateTime.UtcNow;
                } 
                if (updatedUser.LastName != null)
                {
                    user.LastName = updatedUser.LastName;
                    user.UpdatedAt = DateTime.UtcNow;
                }
                if (updatedUser.Password != null)
                {
                    user.PasswordHash =(_PS.ToHashPassword(updatedUser.Password));
                    user.UpdatedAt = DateTime.UtcNow;
                }
                await _db.SaveChangesAsync();
                return  new Result<UserToFront>(_um.ToFront(user));
            }
            return new Result<UserToFront>(ResultStatus.NotFound,"Пользователь не найден");
            
        }
    }
    
}
