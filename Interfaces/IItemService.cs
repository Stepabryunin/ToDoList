
using ToDoList.DTOs;
using ToDoList.Entities;

namespace ToDoList.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemToFront?>> GetAllItemAsync(User user);
        Task<Item?> GetItemByIdAsync(Guid ItemId);
        Task<bool> AddItemAsync(CreatedItem createdItem, User user); //true - succes
        Task<bool> UpdateItemAsync(ItemToBack item);
        Task<bool> DeleteItemAsync(Guid ItemId);
        Task<Item?> GetLast();

    }
}