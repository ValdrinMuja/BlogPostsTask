using System;
using Application.Abstractions;
using Domain.Users;

namespace Application.Accounts.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<ApplicationUser>;