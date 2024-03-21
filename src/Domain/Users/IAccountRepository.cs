using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public interface IAccountRepository
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<ApplicationUser?> FindByIdAsync(string userId);
        Task<ApplicationUser?> FindByPasswordAsync(string passwordHash);
        Task<IReadOnlyCollection<string>> GetRolesAsync(ApplicationUser user);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> AddToRolesAsync(ApplicationUser user, IEnumerable<string> roles);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

    }

}
