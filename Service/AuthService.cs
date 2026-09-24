using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Common;
using ToDoList.DTOs;
using ToDoList.Entities;
using ToDoList.Interfaces;
using ToDoList.Mappers;

namespace ToDoList.Services
{
    public class AuthService:IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserServiceInternal _US;
        private readonly UserMapper _UM;
        private readonly IPasswordService _PS;
        public AuthService(IConfiguration config, IUserServiceInternal iUserService, UserMapper userMapper, IPasswordService passwordService)
        {
            _config = config;
            _US = iUserService;
            _UM = userMapper;
            _PS = passwordService;
        }
        private JwtSecurityToken CreateJWTToken(Guid userId)
        {
            var claims = new List<Claim> {new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())};
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes((_config["JWToptions:Key"])));
            var jwt = new JwtSecurityToken(
                issuer: _config["JWToptions:Issuer"],
                audience: _config["JWToptions:Audiencer"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature)
            );
            return jwt;
        }
        async public Task<Result<AuthResult>> Register(CreatedUser user)
        {
            var checkUser = await  _US.GetEntitybyEmailAsync(user.Email);
            if (checkUser != null)
            {
                return new Result<AuthResult>(ResultStatus.BadRequest, "Данный email уже занят");  
            }
            user.Password = _PS.ToHashPassword(user.Password);
            var addedUser = await _US.CreateUserAsync(user); 

            if (addedUser == null)
                return new Result<AuthResult>(ResultStatus.BadRequest,"Не удалость сосздать пользователя");
            var jwtToken = CreateJWTToken(addedUser.Id);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return new Result<AuthResult>(new AuthResult(token, _UM.ToFront(addedUser)));
        }

        async public Task<Result<AuthResult>> Login(string email, string password)
        {
            var checkUser = await _US.GetEntitybyEmailAsync(email);
            if (checkUser == null)
                return new Result<AuthResult>(ResultStatus.Conflict, "Email уже занят");
            if (!_PS.VerifyPassword(password, checkUser.PasswordHash))
                return new Result<AuthResult>(ResultStatus.Unauthorized, "Неправильный логин или пароль");
            var jwtToken = CreateJWTToken(checkUser.Id);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return new Result<AuthResult>(new AuthResult(token, _UM.ToFront(checkUser)));
        }
    }    
}
