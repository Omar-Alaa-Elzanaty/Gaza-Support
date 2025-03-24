using FluentValidation;
using Infrastructure.IServices;
using Infrastructure.Services;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddMediator()
                .AddServices()
                .AddMapping()
                .AddValidators();
            return services;
        }
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IMediaService, LocalMediaService>()
                .AddScoped<IAuthService, AuthService>();

            return services;
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            return services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        private static IServiceCollection AddMapping(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);

            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
