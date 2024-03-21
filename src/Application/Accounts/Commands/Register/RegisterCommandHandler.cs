using Application.Abstractions;
using Domain.Shared;
using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Application.Accounts.Commands.Register;

internal sealed class RegisterCommandHandler
    : ICommandHandler<RegisterCommand, bool>
{
    private readonly IAccountRepository _accountRepository;

    public RegisterCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    public async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _accountRepository.FindByEmailAsync(request.Email) is not null)
        {
            return Result.Failure<bool>(
                new Error("USER_EXISTS", "This user already exists!"));
        }
        
        string? username = String.Concat(request?.FirstName + "" + request?.LastName);
        string uniqueUsername = $"{username}{Guid.NewGuid()}";
        
        
        var user = ApplicationUser
                    .Create(
                        firstName: request.FirstName,
                        lastName: request.LastName,
                        email: request.Email,
                        userName: uniqueUsername,
                        DateTime.UtcNow
                    );

        var result = await _accountRepository.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return Result.Failure<bool>(
                new Error("REGISTER_WENT_WRONG", "Registration went wrong, please try again!"));
        }

        _ = await AddToRoleAsync(user, "Admin");

        return Result.Success(true);
    }

    private async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
    {
        return await _accountRepository.AddToRoleAsync(user, role);
    }
}