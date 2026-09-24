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
        private readonly IRefreshTokenService _RTS;
        public AuthService(IConfiguration config, IUserServiceInternal iUserService, UserMapper userMapper, IPasswordService passwordService, IRefreshTokenService refreshTokenService)
        {
            _config = config;
            _US = iUserService;
            _UM = userMapper;
            _PS = passwordService;
            _RTS = refreshTokenService;
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
        public async Task<Result<AuthResult>> Refresh(string oldToken)
        {
            var refreshToken = await _RTS.UpdateRefreshToken(oldToken);
            if (refreshToken.Success)
            {
                var accessTokenClass = CreateJWTToken(refreshToken.Value!.UserId);
                string accessToken = new JwtSecurityTokenHandler().WriteToken(accessTokenClass);
                var user = await  _US.GetEntityByIdAsync(refreshToken.Value!.UserId);
                AuthResult result = new AuthResult(accessToken,refreshToken.Value!.Token, _UM.ToFront(user!));
                return new Result<AuthResult>(result);
            }            
            return new Result<AuthResult>(refreshToken.Status, refreshToken.Error!);
            
        }
        public async Task<Result<AuthResult>> Register(CreatedUser user)
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
            var accessToken = CreateJWTToken(addedUser.Id);
            string accesTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            var refreshToken =await _RTS.GenerateNewRefreshToken(addedUser.Id);
            if (!refreshToken.Success)
                return new Result<AuthResult>(refreshToken.Status, refreshToken.Error!);
            return new Result<AuthResult>(new AuthResult(accesTokenString, refreshToken.Value!, _UM.ToFront(addedUser)));
        }

        public async Task<Result<AuthResult>> Login(string email, string password)
        {
            var checkUser = await _US.GetEntitybyEmailAsync(email);
            if (checkUser == null)
                return new Result<AuthResult>(ResultStatus.Unauthorized, "Неправильный логин или пароль");
            if (!_PS.VerifyPassword(password, checkUser.PasswordHash))
                return new Result<AuthResult>(ResultStatus.Unauthorized, "Неправильный логин или пароль");
            var jwtToken = CreateJWTToken(checkUser.Id);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var refreshToken =await _RTS.GenerateNewRefreshToken(checkUser.Id);
            if (!refreshToken.Success)
                return new Result<AuthResult>(refreshToken.Status, refreshToken.Error!);
            return new Result<AuthResult>(new AuthResult(token, refreshToken.Value!, _UM.ToFront(checkUser)));
        }
    }    
}
