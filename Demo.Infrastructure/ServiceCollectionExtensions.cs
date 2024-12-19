using Demo.Domain.InfrastructureServices;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServiceInfrastucture(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
        }
        public static void AddOptionInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOption>(configuration.GetSection("JwtOption"));
        }
        public static void AddRedisInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(redisOptions =>
            {
                var connectionstring = configuration["ConnectionStrings:Redis"];
                redisOptions.Configuration = connectionstring;
            });
        }
    }
}
