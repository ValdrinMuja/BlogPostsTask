using Domain.BlogPosts;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                var cn = configuration.GetConnectionString("ConnectionString");
                options.UseNpgsql(cn);
            });

            services.AddSingleton<SymmetricSecurityKey>(provider =>
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]!));
            });

            services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            }).AddRoles<ApplicationRole>()
              .AddRoleManager<RoleManager<ApplicationRole>>()
              .AddSignInManager<SignInManager<ApplicationUser>>()
              .AddRoleValidator<RoleValidator<ApplicationRole>>()
              .AddEntityFrameworkStores<DataContext>();

            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IBlogPostRepository, BlogPostRepository>();

            return services;
        }
    }
}
