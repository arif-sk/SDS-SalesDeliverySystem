using Caretaskr.Data;
using Caretaskr.Domain.Persistance;
using Microsoft.EntityFrameworkCore;
using SDS.Application.Interfaces;
using SDS.Data;
using SDS.Data.Repositories;
using SDS.Service.Services;
using SDS.Token.Interface;
using SDS.Token.Service;
using System.Reflection;

namespace SDS.Code
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection RegisterDIs(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericRepository<>));
            services.AddScoped<IGenericUnitOfWork, GenericUnitOfWork>();
            var connectionString = configuration.GetConnectionString("SDSDatabase");
            var migrationsAssembly = typeof(ApplicationContext).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly)), ServiceLifetime.Transient);
            return services;
        }
    }
}
