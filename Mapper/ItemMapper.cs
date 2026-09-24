using ToDoList.DTOs;
using ToDoList.Entities;

namespace ToDoList.Mappers
{
    public class ItemMapper
    {
        public Item? ToEntity(ItemToBack? itemToBack)
        {
            Item? newItem;
            if (itemToBack==null)
                newItem=null;
            else 
            {
                newItem = new Item();
                newItem.Id=itemToBack.Id;
                newItem.Name=itemToBack.Name;
                newItem.Description=itemToBack.Description;
            }
            return newItem;
        }
        public Item? ToEntity(CreatedItem? createdItem, User? user)
        {
            Item? newItem;
            if (createdItem==null || user==null)
                newItem=null;
            else 
            {
                newItem = new Item();
                newItem.Id=Guid.NewGuid();
                newItem.Name=createdItem.Name;
                newItem.Description=createdItem.Description;
                newItem.CreatedAt=DateTime.UtcNow;
                newItem.UpdatedAt=DateTime.UtcNow;
                newItem.UserId=user.Id;
                newItem.User=user;
            }
            return newItem;
        }
        public ItemToFront ToFront(Item item)
        {
            ItemToFront newItemToFront = new ItemToFront();
             newItemToFront.Id=item.Id;
            newItemToFront.Name=item.Name;
            newItemToFront.Description=item.Description;
            newItemToFront.CreatedAt=item.CreatedAt;
            newItemToFront.UpdatedAt=item.UpdatedAt;  
            return newItemToFront;
        }




    }
}