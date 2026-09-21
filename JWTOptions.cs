using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ToDoList
{
    public class JWTOptions
    {
        WebApplication builder;
        IConfiguration configuration;
        public JWTOptions(IConfiguration _conf)
        {
            configuration = _conf;

        }
        public TokenValidationParameters GetOptions()
        {
          var parametrs = new TokenValidationParameters()
          {
            ValidateIssuer = true,
            ValidIssuer = configuration["JWTOptions:Issuer"],
            ValidateAudience = true,
            ValidAudience =  configuration["JWTOptions:Audiencer"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTOptions:Key"]))
          };
          return parametrs;

        }
    }
}