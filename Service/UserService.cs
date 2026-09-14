
using ToDoList.Context;
using ToDoList.DTOs;
using ToDoList.Mappers;
using ToDoList.Entities;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Services
{
    public class UserService:ToDoList.Interfaces.IUserService
    {
        private readonly MyDbContext _db;
        private readonly UserMapper _um;

        public UserService(MyDbContext db, UserMapper um )
        {
            _db=db;
            _um=um;
        }
        async public Task<User?> AddUserAsync(UserToBack user)
        {
            var userEntity = _um.ToEntity(user);
            await _db.AddAsync(userEntity);
            await _db.SaveChangesAsync();
            return userEntity;
        }
        async public Task<User?> GetUserbyEmailAsync(string Email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email==Email);
            return user;
        }
        
    }
    
}
