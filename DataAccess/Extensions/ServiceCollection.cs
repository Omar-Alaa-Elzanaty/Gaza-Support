using DataAccess.Interface;
using DataAccess.Repo;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddPresistance(this IServiceCollection services)
        {
            services
                .AddServices();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDonationRepo, DonationRepo>()
                    .AddScoped<IUserRepo, UserRepo>()
                    .AddScoped<IRoleRepo,RoleRepo>()
                    .AddScoped<IunitOfWork, UnitOfWork>()
                    .AddScoped<IDonorRepo, DonorRepo>()
                    .AddScoped<ISubscribeRepo,SubscribeRepo>()
                    .AddScoped<IRecipientRepo, RecipientRepo>();

            return services;
        }
    }
}
