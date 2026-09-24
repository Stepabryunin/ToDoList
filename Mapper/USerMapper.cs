using ToDoList.Entities;
using ToDoList.DTOs;

namespace ToDoList.Mappers
{
    public class UserMapper
    {
        public User ToEntity(CreatedUser userToBack)
        {
            var user = new User()
            {
              Id=Guid.NewGuid(),
              FirstName=userToBack.FirtsName,
              LastName=userToBack.LastName,
              Email=userToBack.Email,
              PasswordHash=userToBack.Password,
              CreatedAt=DateTime.UtcNow,
              UpdatedAt = DateTime.UtcNow
              
            };
            return user;
        }
        public UserToFront ToFront(User user)
        {
            var userToFront = new UserToFront()
            {
                LastName = user.LastName,
                FirstName = user.FirstName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            
            };
            return userToFront;
        }
        
    }
}