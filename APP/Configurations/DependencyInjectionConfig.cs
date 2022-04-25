using APP.Business.Interface;
using APP.Data.Context;
using APP.Data.Repository;
using APP.Extensions;
using APP.SubscribeTableDependencies;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace APP.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddSingleton<APPDBContext>();
            services.AddSingleton<IFlight_ScheduleRepository, Flight_ScheduleRepository>();
            return services;
        }
        public static IServiceCollection ResolveDependenciesSIJ(this IServiceCollection services)
        {
            services.AddSingleton<Flight_ScheduleHub>();
            services.AddSingleton<SubscribeFlight_ScheduleTableDependency>();

            return services;
        }
    }
}