using Microsoft.EntityFrameworkCore;
using ToDoList.Interfaces;
using ToDoList.DTOs;
using ToDoList.Mapper;
using ToDoList.Context;
using ToDoList.Entities;

namespace ToDoList.Services{
    public class ItemService:IItemService
    {
        ItemContext db;
        ItemMapper im;

        public ItemService(ItemContext _ItemService, ItemMapper _ItemMapper)
        {
            db=_ItemService;
            im= _ItemMapper;
        }

        public async Task<List<ItemToFront?>> GetAllItemAsync()
        {
            var items = await db.Items.ToListAsync();
            return items.Select(im.ToFront).ToList();
        }

        public async Task<ItemToFront?> GetItemByIdAsync(Guid ItemId)
        {
            var item = await db.Items.FirstOrDefaultAsync(item => item.Id==ItemId);
            return im.ToFront(item);
        }
        public async Task<bool> AddItemAsync(CreatedItem createdItem)
        {
            bool result = false;
            var item = im.ToEntity(createdItem);
            if (item!=null)
            {
                var itemFromDb= await db.Items.FindAsync(item.Id);
                if (itemFromDb == null)
                {
                    await db.Items.AddAsync(item);
                    await db.SaveChangesAsync(); 
                    result=!result;
                }
            }
            return result;
        }

        public async Task<Item?> GetLast()
        {
            return await db.Items
                .OrderBy(i =>i.CreatedAt)
                .LastOrDefaultAsync();
        }
        
        public async Task<bool> UpdateItemAsync(ItemToBack item)
        {
            bool result = false;
            var oldItem = await db.Items.FirstOrDefaultAsync(it => it.Id==item.Id);
            if (oldItem != null)
            {
                if (!string.IsNullOrWhiteSpace(item.Name))
                   { 
                        oldItem.Name = item.Name;
                        result = true;
                   }
                if (!string.IsNullOrWhiteSpace(item.Description))
                    {
                        oldItem.Description=item.Description;
                        result = true;
                    }
                await db.SaveChangesAsync();
            }
            return result;
        }

        public async Task<bool> DeleteItemAsync(Guid ItemId)
        {
            bool result = false;
            var item = await db.Items.FirstOrDefaultAsync(i => i.Id == ItemId);
            if (item != null)
            {
                db.Items.Remove(item);
                result = true;
                await db.SaveChangesAsync();
            }
            return result;
        }
    }

}