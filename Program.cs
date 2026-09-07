using Microsoft.EntityFrameworkCore;
using ToDoList.Mapper;
using ToDoList.Services;
using ToDoList.Interfaces;
using ToDoList.Context;

var builder = WebApplication.CreateBuilder(args);
string ConnectionString = builder.Configuration.GetConnectionString("BaseConnection");
builder.Services.AddDbContext<ItemContext>(options=>options.UseNpgsql(ConnectionString));
builder.Services.AddScoped<IItemService,ItemService>();
builder.Services.AddScoped<ItemMapper>();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers(); 

app.Run();

