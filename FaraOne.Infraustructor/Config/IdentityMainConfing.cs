using FaraOne.Application.Context;
using FaraOne.Domain.User;
using FaraOne.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
namespace FaraOne.Infraustructor;

public static class IdentityMainConfing
{
    public static IServiceCollection AddIdnetityMain(this IServiceCollection services, IConfiguration configuration)
    {
        var ConString = configuration["ConnectionStrings:sqlServer"];
        services.AddDbContext<DataContext>(s => s.UseSqlServer(ConString));

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<CustomIdentityError>();

        services.Configure<IdentityOptions>(s =>
        {
            s.Lockout.MaxFailedAccessAttempts = 5;
            s.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            s.Password.RequireDigit = true;
            s.Password.RequireNonAlphanumeric = false;
            s.Password.RequiredLength = 8;
        });
        return services;
    }
}