﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SDS.Code
{
    public static class IdentityServiceExtentions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };
              });
            return services;
        }
    }
}
