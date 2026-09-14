using Microsoft.EntityFrameworkCore;
using ToDoList.Entities;

namespace ToDoList.Context
{
    public class MyDbContext: DbContext
{
    public DbSet<Item> Items {get; set;} = null!;
    public DbSet<User> Users {get;set;} = null!;

    public MyDbContext(DbContextOptions<MyDbContext> options) :base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Item>().ToTable("Items");
    }



}
}
