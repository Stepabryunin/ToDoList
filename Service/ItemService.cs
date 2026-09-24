using Microsoft.EntityFrameworkCore;
using ToDoList.Interfaces;
using ToDoList.DTOs;
using ToDoList.Mappers;
using ToDoList.Context;
using ToDoList.Entities;
using ToDoList.Common;

namespace ToDoList.Services{
    public class ItemService:IItemService
    {
        private readonly MyDbContext db;
        private readonly ItemMapper _IM;
        private readonly IUserServiceInternal _US;

        public ItemService(MyDbContext dbContext, ItemMapper itemMapper, IUserServiceInternal userService)
        {
            db=dbContext;
            _IM = itemMapper;
            _US =userService;
        }

        public async Task<Result<List<ItemToFront>>> GetAllItemForUserAsync(Guid userId)
        {

            var items = await db.Items
                .Where(i => i.UserId==userId)
                .ToListAsync();
            return new Result<List<ItemToFront>>(items.Select(_IM.ToFront).ToList());
        }

        public async Task<Result<ItemToFront>> GetItemByIdAsync(Guid itemId, Guid userId)
        {
            var item =  await db.Items.FirstOrDefaultAsync(item => item.Id==itemId && item.UserId==userId);
            if (item==null)
                return new Result<ItemToFront>(ResultStatus.NotFound,"item с таким id не найден");
            return new Result<ItemToFront>(_IM.ToFront(item));
        }
        public async Task<Result<ItemToFront>> AddItemAsync(CreatedItem createdItem, Guid userId)
        {
            var user = await _US.GetEntityByIdAsync(userId);
            if (user == null)
            {
                return new Result<ItemToFront>(ResultStatus.Unauthorized, "Пользователь не найден");
            }
            var item = _IM.ToEntity(createdItem, user);
            if (item!=null)
            {
                await db.Items.AddAsync(item);
                await db.SaveChangesAsync(); 
                return new Result<ItemToFront>(_IM.ToFront(item));
            }
            return new Result<ItemToFront>(ResultStatus.BadRequest, "Не удалось создать item");
        }

        // public async Task<Item?> GetLast()
        // {
        //     return await db.Items
        //         .OrderBy(i =>i.CreatedAt)
        //         .LastOrDefaultAsync();
        // }
        
        public async Task<Result<ItemToFront>> UpdateItemAsync(ItemToBack item, Guid userId)
        {
            var oldItem = await db.Items.FirstOrDefaultAsync(it => it.Id==item.Id && it.UserId==userId);
            if (oldItem != null)
            {
                bool updated = false;
                if (!string.IsNullOrWhiteSpace(item.Name))
                   { 
                        oldItem.Name = item.Name;
                        updated = true;
                        
                   }
                if (!string.IsNullOrWhiteSpace(item.Description))
                    {
                        oldItem.Description=item.Description;
                        updated = true;
                    }
                if (updated)
                    oldItem.UpdatedAt=DateTime.UtcNow;
                else 
                    return new Result<ItemToFront>(ResultStatus.BadRequest, "Нет полей для обновления");
                await db.SaveChangesAsync();
                return new Result<ItemToFront>(_IM.ToFront(oldItem));
            }
            return new Result<ItemToFront>(ResultStatus.NotFound, "item не найден");
        }

        public async Task<Result<ItemToFront>> DeleteItemAsync(Guid ItemId, Guid userId)
        {

            var item = await db.Items.FirstOrDefaultAsync(i => i.Id == ItemId && i.UserId == userId);
            if (item != null)
            {
                db.Items.Remove(item);
                await db.SaveChangesAsync();
                return Result<ItemToFront>.NoContent();
            }
            return new Result<ItemToFront>(ResultStatus.NotFound, "item не найден");
        }
    }

}