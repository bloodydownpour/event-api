using EventList.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EventList.Infrastructure.PasswordServices;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using EventList.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using EventList.Application.CQRS.Commands;
using EventList.Infrastructure.CQRS.Queries;
using EventList.Application.JWT;
using Microsoft.AspNet.Identity;
using EventList.Application.ImageProcessing;


namespace EventList.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddAuthentication(config)
                .AddResources(config)
                .AddServices(config)
                .AddCORS(config);
            return services;
        }
        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!)),
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero
                    };
                });
            return services;
        }
        private static IServiceCollection AddResources(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<EventDbContext>(x => x.UseSqlServer(
            "Data Source=(local); Database=EventList; Persist Security Info = false; MultipleActiveResultSets=True; Trusted_Connection=True; TrustServerCertificate=True;",
            b => b.MigrationsAssembly("EventList.Inftastructure")));
            return services;
        }
        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddScoped<IPasswordService, PasswordService>()
                .AddScoped<IEventRepository, EventRepository>()
                .AddScoped<EventQueries>()
                .AddScoped<EventCommands>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<UserQueries>()
                .AddScoped<UserCommands>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddSingleton<PasswordHasher>()
                .AddSingleton<TokenProvider>()
                .AddScoped<ImageHandler>();
            return services;
            }
        private static IServiceCollection AddCORS(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            return services;
        }
    }
}
