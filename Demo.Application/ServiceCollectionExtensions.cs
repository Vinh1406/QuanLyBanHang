using Demo.Application.Services;
using Demo.Domain.ApplicationServices.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServiceApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
