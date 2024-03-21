using Domain.BlogPosts;
using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public sealed class DataContext: IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<BlogPost>()
                .HasKey(p => p.Id);

            builder.Entity<BlogPost>().HasOne(u => u.CreatedBy)
                .WithMany(bp=>bp.BlogPosts)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<ApplicationUserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });


            builder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

           
            builder.Entity<ApplicationUser>()
                .HasMany(ur => ur.BlogPosts)
                .WithOne(u => u.CreatedBy)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        }
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
