using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YallaKhadra.Core.Abstracts.CoreAbstracts.Services;
using YallaKhadra.Core.Behaviors;
using YallaKhadra.Core.Services;

namespace YallaKhadra.Core {
    public static class CoreServiceRegistration {
        public static IServiceCollection AddCore(this IServiceCollection services) {

            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TrimmingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            AddDomainServices(services);

            return services;
        }


        public static IServiceCollection AddDomainServices(IServiceCollection services) {
            services.AddScoped<IDomainUserService, DomainUserService>();
            return services;
        }
    }
}