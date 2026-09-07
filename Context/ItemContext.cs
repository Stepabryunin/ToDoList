using Microsoft.EntityFrameworkCore;
using ToDoList.Entities;

namespace ToDoList.Context
{
    public class ItemContext: DbContext
{
    public DbSet<Item> Items {get; set;} = null!;

    public ItemContext(DbContextOptions<ItemContext> options) :base(options)
    {

    }



}
}
