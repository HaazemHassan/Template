using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YallaKhadra.Core.Abstracts.InfrastructureAbstracts;
using YallaKhadra.Core.Abstracts.ServicesAbstracts.InfrastrctureServicesAbstracts;
using YallaKhadra.Core.Bases.Authentication;
using YallaKhadra.Core.Entities.IdentityEntities;
using YallaKhadra.Infrastructure.Abstracts;
using YallaKhadra.Infrastructure.Data;
using YallaKhadra.Infrastructure.Repositories;
using YallaKhadra.Infrastructure.Services;
using YallaKhadra.Services.Services;

namespace YallaKhadra.Infrastructure;

public static class InfrastructureDependencyRegisteration {

    public static IServiceCollection InfrastrctureLayerDepenedencyRegistration(this IServiceCollection services, IConfiguration configuration) {

        DbContextServiceConfiguations(services, configuration);
        RepositoryServiceConfiguations(services);
        IdentityServiceConfiguations(services, configuration);

        services.AddTransient<IApplicationUserService, ApplicationUserService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();

        return services;

    }


    private static IServiceCollection DbContextServiceConfiguations(IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<AppDbContext>(options => {
            options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
        });

        return services;
    }


    private static IServiceCollection IdentityServiceConfiguations(IServiceCollection services, IConfiguration configuration) {

        // Bind Password Settings from appsettings.json
        var passwordSettings = new PasswordSettings();
        configuration.GetSection(PasswordSettings.SectionName).Bind(passwordSettings);
        services.AddSingleton(passwordSettings);

        services.AddIdentity<ApplicationUser, ApplicationRole>(option => {
            // Password settings 
            option.Password.RequireDigit = passwordSettings.RequireDigit;
            option.Password.RequireLowercase = passwordSettings.RequireLowercase;
            option.Password.RequireNonAlphanumeric = passwordSettings.RequireNonAlphanumeric;
            option.Password.RequireUppercase = passwordSettings.RequireUppercase;
            option.Password.RequiredLength = passwordSettings.MinLength;
            option.Password.RequiredUniqueChars = passwordSettings.RequiredUniqueChars;

            // Lockout settings.
            option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            option.Lockout.MaxFailedAccessAttempts = 5;
            option.Lockout.AllowedForNewUsers = true;

            // User settings.
            option.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;
            option.SignIn.RequireConfirmedEmail = false;


        }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

        return services;
    }


    private static IServiceCollection RepositoryServiceConfiguations(this IServiceCollection services) {

        // UnitOfWork should be Scoped to maintain consistency across a single request
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();



        return services;
    }
}
