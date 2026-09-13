using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ToDoList
{
    public class JWTOptions
    {
        WebApplicationBuilder builder;
        public JWTOptions(WebApplicationBuilder _builder)
        {
            builder=_builder;
        }
        public TokenValidationParameters GetOptions()
        {
          var parametrs = new TokenValidationParameters()
          {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
            ValidateAudience = true,
            ValidAudience =  builder.Configuration["JWTOptions:Audiencer"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:Key"]))
          };
          return parametrs;

        }
    }
}