

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions;
using Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtSettings _jwtSettings;
    private readonly SymmetricSecurityKey _key;
    private readonly IAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;

    public JwtProvider(IOptions<JwtSettings> jwtSettingsOptions, IAccountRepository accountRepository, IConfiguration configuration)
    {
        _jwtSettings = jwtSettingsOptions?.Value ?? throw new ArgumentNullException(nameof(jwtSettingsOptions));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]!));
    }

    public async Task<string> GenerateAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName)
            };

        var roles = await _accountRepository.GetRolesAsync(user);

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(10),
            SigningCredentials = credentials,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Aud
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

}
