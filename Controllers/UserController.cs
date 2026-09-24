using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using ToDoList.DTOs;
using ToDoList.Extentions;
using ToDoList.Interfaces;




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

        [HttpPost("Registration")]
        public async Task<IActionResult> Register([FromBody] CreatedUser user)
        {
            var result = await _AS.Register(user);
            return result.ToAction();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserToLogin user)
        {
            var result = await _AS.Login(user.Email, user.Password);
            return result.ToAction();
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenToBack refreshToken)
        {
            var result = await _AS.Refresh(refreshToken.TokenRefresh);
            return result.ToAction();
        }
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdatedUser user)
        {
            string? idString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idString == null)
                return Unauthorized();
            Guid userId;
            if (!Guid.TryParse(idString, out userId))
                return Unauthorized();            
            var result = await _US.UpdateUserAsync(user, userId);
            return result.ToAction();
        }

    }
}