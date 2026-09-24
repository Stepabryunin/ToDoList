using ToDoList.Common;
using ToDoList.DTOs;

namespace ToDoList.Interfaces
{
 
    public interface IRefreshTokenService
    {
        
        public  Task<Result<ResultToken>> UpdateRefreshToken(string oldToken);
        public Task<Result<string>> GenerateNewRefreshToken(Guid userId);
        
       
    }
}