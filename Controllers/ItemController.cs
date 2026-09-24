using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ToDoList.DTOs;
using ToDoList.Entities;
using ToDoList.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ToDoList.Mappers;
using ToDoList.Extentions;
using ToDoList.Common;

namespace ToDoList.Controllers
{
    [ApiController]// он тут сам валидирует данные, поэтому не надо !ModelState.IsValid
    [Route("api/[controller]")]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _IS;
        private readonly IUserService _US;
        private readonly ItemMapper _IM;
        public ItemController(IItemService iItemService, IUserService iUserService, ItemMapper itemMapper)
        {
            _IS=iItemService;
            _US=iUserService;
            _IM= itemMapper;
        }
        

        [HttpGet]
        public async Task<IActionResult> GetAllItemsController()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString==null)
                return Unauthorized();
            Guid userIdGuid;
            if (!Guid.TryParse(userIdString,out userIdGuid))
                return Unauthorized();
            var result = await _IS.GetAllItemForUserAsync(userIdGuid);
            return result.ToAction(); 
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById( Guid itemId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString==null)
                return Unauthorized();
            Guid userIdGuid;
            if (!Guid.TryParse(userIdString, out userIdGuid))
                return Unauthorized();
            var result = await _IS.GetItemByIdAsync(itemId, userIdGuid);
            return result.ToAction();
        }

        [HttpPost]
        public async Task<IActionResult?> AddItem([FromBody] CreatedItem item)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            if (userIdString==null)
                return Unauthorized();
            Guid userIdGuid;
            if (!Guid.TryParse(userIdString,out userIdGuid))
                return Unauthorized();
            var result = await _IS.AddItemAsync(item, userIdGuid);
            return result.ToAction();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateItemById([FromBody] ItemToBack itemToBack)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
                return Unauthorized();
            Guid userIdGuid;
            if (!Guid.TryParse(userIdString, out userIdGuid))
                return Unauthorized();
            var result = await _IS.UpdateItemAsync(itemToBack,userIdGuid);
            return result.ToAction();
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItemById( Guid itemId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
                return Unauthorized();
            Guid userIdGuid;
            if (!Guid.TryParse(userIdString, out userIdGuid))
                return Unauthorized();
            var result = await _IS.DeleteItemAsync(itemId, userIdGuid);
            return result.ToAction();
        }

 
    }
}