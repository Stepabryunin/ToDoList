using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using ToDoList.Mappers;
using ToDoList.Services;
using ToDoList.Interfaces;
using ToDoList.Context;
using ToDoList;


var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("BaseConnection");


var jWTparametres= new JWTOptions(builder.Configuration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
    {
        options.TokenValidationParameters=jWTparametres.GetOptions();
    }
    );

builder.Services.AddAuthorization();
builder.Services.AddDbContext<MyDbContext>(options=>options.UseNpgsql(connectionString));
builder.Services.AddScoped<ItemMapper>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IUserService>( sp => sp.GetRequiredService<UserService>());
builder.Services.AddScoped<IUserServiceInternal>(sp => sp.GetRequiredService<UserService>());
builder.Services.AddScoped<IItemService,ItemService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ToDoList API",
        Version = "v1"
    });

    // Security scheme для JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Введите JWT токен (без слова Bearer).",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Требуем эту схему для эндпоинтов
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddEndpointsApiExplorer();


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

