using System;
namespace Application.Accounts.Commands.Register;

public sealed record RegisterRequest(string FirstName, string LastName, string Email, string Password);
