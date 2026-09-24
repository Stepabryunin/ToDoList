using Microsoft.EntityFrameworkCore;
using ToDoList.Interfaces;
using ToDoList.Context;
using ToDoList.Entities;
using ToDoList.Common;
using ToDoList.DTOs;
namespace ToDoList.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly MyDbContext _DB;
        public RefreshTokenService(MyDbContext db)
        {
            _DB = db;
        }
        // public async Task<List<RefreshToken>> GetAllTokensByUserIdAsync(Guid userId)
        // {
        //     return await _DB.Tokens
        //         .Where(i=>i.UserId==userId)
        //         .ToListAsync();
        // }
        private string ToHashToken(string token)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(token));
                return Convert.ToBase64String(hash);
            }
        }
        private async Task<RefreshToken?> GetEntityByToken(string token)
        {
            string tokenHash = ToHashToken(token);
            return await _DB.Tokens.FirstOrDefaultAsync(t => t.TokenHash== tokenHash);
        }
        private async Task<string> CreateNewRefreshToken(RefreshToken? oldToken, Guid userId, DateTime expireAt)
        {
            if (oldToken != null)
                oldToken.IsRevoked = true;
            string newTokenString = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(128));
            var newToken = new RefreshToken(ToHashToken(newTokenString),oldToken==null?null:oldToken.TokenHash,userId,expireAt);
            await _DB.Tokens.AddAsync(newToken);
            await _DB.SaveChangesAsync();
            return newTokenString;
        }

        public async Task<Result<ResultToken>> UpdateRefreshToken(string oldToken)
        {
            RefreshToken? token = await GetEntityByToken(oldToken);
            if (token == null)
                return new Result<ResultToken>(ResultStatus.Unauthorized, "Неверный токен");
            if (token.IsRevoked || DateTime.UtcNow >= token.ExpireAt)
                return new Result<ResultToken>(ResultStatus.Unauthorized, "Токен истёк");
            var newToken = await CreateNewRefreshToken(token, token.UserId, DateTime.UtcNow.AddDays(7));
            return new Result<ResultToken>(new ResultToken(newToken, token.UserId));
        }
       

        public async Task<Result<string>> GenerateNewRefreshToken(Guid userId)
        {
            var refreshToken = await CreateNewRefreshToken(null,userId,DateTime.UtcNow.AddDays(7));
            return new Result<string>(refreshToken);
        }
    }
    
}