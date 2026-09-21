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
            var user = await _US.GetUserByIdAsync(userIdGuid);
            if (user==null)
                return Unauthorized();
            var items = await _IS.GetAllItemAsync(user);
            if (items!=null && items.Count()>0)
                return Ok(items);
            return NotFound();
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
            var item = await _IS.GetItemByIdAsync(itemId);
            if (item == null || item.UserId!=userIdGuid)
                return NotFound();
            return Ok(item);
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
            var user = await _US.GetUserByIdAsync(userIdGuid);
            if (user==null)
                return Unauthorized();
            
            System.Console.WriteLine($"{item.Name}, {item.Description}");
            if (await _IS.AddItemAsync(item,user))
            {
                Item addedItem= await _IS.GetLast();//он тут не вернёт null никогда
                return Created($"/item/{addedItem.Id}", _IM.ToFront(addedItem));
            }
            return BadRequest();
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
            User? user = await  _US.GetUserByIdAsync(userIdGuid); 
            if (user==null)
                return Unauthorized();
            var item = await _IS.GetItemByIdAsync(itemToBack.Id);
            if (item == null || item.UserId != userIdGuid)
                return NotFound();
            bool result = await _IS.UpdateItemAsync(itemToBack);
            if (result== null)
                return BadRequest();
            return Ok();
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
            User? user = await  _US.GetUserByIdAsync(userIdGuid); 
            if (user==null)
                return Unauthorized();
            var item = await _IS.GetItemByIdAsync(itemId);
            if (item==null || item.UserId!=user.Id)
                return NotFound();
            bool result = await _IS.DeleteItemAsync(itemId);
            if (!result)
                return NotFound();
            return Ok();
        }

 
    }
}