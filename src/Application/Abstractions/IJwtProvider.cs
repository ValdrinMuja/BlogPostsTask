using System;
using Domain.Users;

namespace Application.Abstractions;

public interface IJwtProvider
{
	Task<string> GenerateAsync(ApplicationUser user);
}
