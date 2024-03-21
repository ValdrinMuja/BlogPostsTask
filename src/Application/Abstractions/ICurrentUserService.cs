using System;
namespace Application.Abstractions;

public interface ICurrentUserService
{
    string? UserId { get; }
}

