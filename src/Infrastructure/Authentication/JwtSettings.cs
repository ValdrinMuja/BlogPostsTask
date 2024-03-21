using System;
namespace Infrastructure.Authentication;

internal class JwtSettings
{
	public string TokenKey { get; init; }
	public string Issuer { get; init; }
	public string Aud { get; init; }
	public string TokenType { get; init; }
}