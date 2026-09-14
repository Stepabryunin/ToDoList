using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ToDoList.Mappers;
using ToDoList.Services;
using ToDoList.Interfaces;
using ToDoList.Context;
using ToDoList;


var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("BaseConnection");


var jWTparametres= new JWTOptions(builder);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
    {
        options.TokenValidationParameters=jWTparametres.GetOptions();
    }
    );

builder.Services.AddAuthorization();
builder.Services.AddDbContext<MyDbContext>(options=>options.UseNpgsql(connectionString));
builder.Services.AddScoped<ItemMapper>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<IItemService,ItemService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IUserService,UserService>();
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); 


app.Run();

