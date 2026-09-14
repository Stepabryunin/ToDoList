using ToDoList.Entities;
using ToDoList.DTOs;

namespace ToDoList.Mappers
{
    public class UserMapper
    {
        public User ToEntity(UserToBack userToBack)
        {
            var user = new User()
            {
              Id=Guid.NewGuid(),
              FirstName=userToBack.FirtsName,
              LastName=userToBack.LastName,
              Email=userToBack.Email,
              PasswordHash=userToBack.Password,
              CreatedAt=DateTime.UtcNow  
            };
            return user;
        }
    }
}