using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ToDoList.DTOs;
using ToDoList.Interfaces;
using ToDoList.Services;


namespace ToDoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IUserService _US;
        private readonly IAuthService _AS;

        public UserController (IUserService iUserService, IAuthService iAuthService)
        {
            _US = iUserService;
            _AS = iAuthService;
        }

        [HttpPost("/Registration")]
        public async Task<IActionResult> Register([FromBody] UserToBack user)
        {
            var checkUser = await  _US.GetUserbyEmailAsync(user.Email);
            if (checkUser != null)
            {
                ModelState.AddModelError("Email","Данный Email уже существует");
                return BadRequest(ModelState);  
            }
            user.Password=_AS.ToHashPassword(user.Password);
            var addedUser = await _US.AddUserAsync(user); 

            if (addedUser == null)
                return BadRequest("Не удалость сосздать пользователя");
            var jwtToken = _AS.CreateJWTToken(await _US.GetUserbyEmailAsync(user.Email));
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Ok(new{Token = token,User = addedUser});
        }
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] UserToLogin user)
        {
            var checkUser = await _US.GetUserbyEmailAsync(user.Email);
            if (checkUser==null)
            {
                return BadRequest("Не правильный Email или пароль");
            }
            if (!_AS.VerifyPassword(user.Password,checkUser.PasswordHash))
            {
                return BadRequest("Не правильный Email или пароль");
            }
            var jwtToken = _AS.CreateJWTToken(checkUser);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Ok(new {Token=token});
        }

    }
}