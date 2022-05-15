using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure.Auth;
using Infrastructure.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebServices.Auth;

namespace Auth.Controllers;

[ApiController]
[Route("/")]
public class AuthController : ControllerBase, IAuthService
{

    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<AuthController> _logger;
    private readonly AuthOptions _authConfig;

    public AuthController(IDateTimeProvider dateTimeProvider, ILogger<AuthController> logger, IOptions<AuthOptions> authConfig)
    {
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
        _authConfig = authConfig.Value;
    }

    [HttpPost(nameof(CreateToken))]
    public async Task<string> CreateToken()
    {
        _logger.LogInformation("{@Auth}", _authConfig);

        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
                issuer: _authConfig.Issuer,
                audience: _authConfig.Audience,
                notBefore: _dateTimeProvider.UtcNow,
                claims: new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "Name"),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin")
                },
                expires: _dateTimeProvider.UtcNow.Add(TimeSpan.FromMinutes(_authConfig.Lifetime)),
                signingCredentials: new SigningCredentials(_authConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        await Task.Yield();

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

}
