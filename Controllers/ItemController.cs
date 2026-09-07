using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ToDoList.DTOs;
using ToDoList.Entities;
using ToDoList.Interfaces;

namespace ToDoList.Controllers
{
    [ApiController]// он тут сам валидирует данные, поэтому не надо !ModelState.IsValid
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        IItemService _IS;
        public ItemController(IItemService itemService)
        {
            _IS=itemService;
        }
        

        [HttpGet]
        public async Task<IActionResult> GetAllItemsController()
        {
            var items = await _IS.GetAllItemAsync();
            if (items!=null && items.Count()>0)
                return Ok(items);
            return NotFound();
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById( Guid itemId)
        {
            var item = await _IS.GetItemByIdAsync(itemId);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult?> AddItem([FromBody] CreatedItem item)
        {
            System.Console.WriteLine($"{item.Name}, {item.Description}");
            if (await _IS.AddItemAsync(item))
            {
                Item addedItem= await _IS.GetLast();//он тут не вернёт null никогда
                return Created($"/item/{addedItem.Id}", addedItem);
            }
            return BadRequest();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateItemById([FromBody] ItemToBack itemToBack)
        {
            bool result = await _IS.UpdateItemAsync(itemToBack);
            if (!result)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItemById( Guid itemId)
        {
            bool result = await _IS.DeleteItemAsync(itemId);
            if (!result)
                return NotFound();
            return Ok();
        }

 
    }
}