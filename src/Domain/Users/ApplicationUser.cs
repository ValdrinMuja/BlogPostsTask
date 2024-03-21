
using Domain.BlogPosts;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace Domain.Users
{
    public sealed class ApplicationUser : IdentityUser
    {
        private ApplicationUser(string firstName, string lastName, string email, string userName, bool isActive)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = userName;
            IsActive = isActive;
        }


        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public bool IsActive { get; private set; }

        public DateTime CreatedDateUtc { get; internal set; }

        public DateTime? UpdatedDateUtc { get; private set; }

        public List<ApplicationUserRole> UserRoles { get; set; }
        public List<BlogPost> BlogPosts { get; set; }

        public static ApplicationUser Create(
            string firstName,
            string lastName,
            string email,
            string userName,
            DateTime utcNow)
        {
            var user = new ApplicationUser(firstName, lastName, email, userName, true);

            user.CreatedDateUtc = utcNow;

            return user;
        }
    }
}
