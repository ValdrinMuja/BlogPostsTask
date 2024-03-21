using System;
using Application.Abstractions;
using Domain.Users;

namespace Application.Accounts.Commands.Register;

public sealed record RegisterCommand(string FirstName, string LastName, string Email, string Password) : ICommand<bool>;