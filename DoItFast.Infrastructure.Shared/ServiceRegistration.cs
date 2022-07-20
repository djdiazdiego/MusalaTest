using DoItFast.Infrastructure.Shared.Services;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DoItFast.Infrastructure.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void AddSharedInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<ISqlGuidGenerator, SequentialGuidGeneratorService>();
            //services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        }
    }

}
