using Application.Abstractions;
using Domain.Shared;
using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Application.Accounts.Commands.Login;

internal sealed class LoginCommandHandler
	: ICommandHandler<LoginCommand, ApplicationUser>
{
    private readonly IAccountRepository _accountRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginCommandHandler(IAccountRepository accountRepository, SignInManager<ApplicationUser> signInManager)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task<Result<ApplicationUser>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if(await _accountRepository.FindByEmailAsync(request.Email) is not ApplicationUser user)
        {
            return Result.Failure<ApplicationUser>(
                new Error("USER_NOT_FOUND", "User was not found!"));
        }

        if (!user.IsActive)
        {
            return Result.Failure<ApplicationUser>(
                new Error("INVALID_USER", "User invalid, please try again!"));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            return Result.Failure<ApplicationUser>(
                new Error("ACCOUNT_LOCKED", $"Account is locked until: {user.LockoutEnd}!"));
        }

        if (!result.Succeeded)
        {
            return Result.Failure<ApplicationUser>(
                new Error("USER_CREDENTIALS_WRONG", "User credentials are wrong, please try again!"));
        }
        
        return user;
    }
}

