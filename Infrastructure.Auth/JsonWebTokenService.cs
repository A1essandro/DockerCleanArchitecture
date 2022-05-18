using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Domain;
using Infrastructure.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Auth;

public class JsonWebTokenService : IJsonWebTokenService
{

    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AuthOptions _authConfig;

    public JsonWebTokenService(IDateTimeProvider dateTimeProvider, IOptions<AuthOptions> authConfig)
    {
        _dateTimeProvider = dateTimeProvider;
        _authConfig = authConfig.Value;
    }

    public TimeSpan Lifetime => TimeSpan.FromMinutes(_authConfig.Lifetime);

    public string GetToken(User user)
    {
        var jwt = new JwtSecurityToken(
               issuer: _authConfig.Issuer,
               audience: _authConfig.Audience,
               notBefore: _dateTimeProvider.UtcNow,
               claims: new List<Claim>
               {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin")
               },
               expires: _dateTimeProvider.UtcNow.Add(Lifetime),
               signingCredentials: new SigningCredentials(_authConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

}