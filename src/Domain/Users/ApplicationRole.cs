using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public sealed class ApplicationRole : IdentityRole
    {
        private readonly List<ApplicationUserRole> _userRoles = new();

        public IReadOnlyCollection<ApplicationUserRole> UserRoles => _userRoles.AsReadOnly();
    }
}
