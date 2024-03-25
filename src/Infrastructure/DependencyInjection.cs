using Application.Abstractions;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Authentication;
using Infrastructure.Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddHangfire(config => 
            {
                var cn = configuration.GetConnectionString("ConnectionString");
                config.UsePostgreSqlStorage(cn);
                config.UseMediatR();
            });

            services.AddHangfireServer();

            return services;
        }
    }
}
