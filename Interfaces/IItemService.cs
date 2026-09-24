
using ToDoList.DTOs;
using ToDoList.Entities;
using ToDoList.Common;

namespace ToDoList.Interfaces
{
    public interface IItemService
    {
        public Task<Result<List<ItemToFront>>> GetAllItemForUserAsync(Guid userId);
        public Task<Result<ItemToFront>> GetItemByIdAsync(Guid ItemId, Guid userId);
        public Task<Result<ItemToFront>> AddItemAsync(CreatedItem createdItem, Guid userId);
        public Task<Result<ItemToFront>> UpdateItemAsync(ItemToBack item, Guid userId);
        public Task<Result<ItemToFront>> DeleteItemAsync(Guid ItemId, Guid userId);
        // Task<Item?> GetLast();

    }
}