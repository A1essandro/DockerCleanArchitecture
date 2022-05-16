using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auth.Exceptions;
using Core.Domain;
using Infrastructure.Auth;
using Infrastructure.Common.Contracts;
using Infrastructure.Dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebServices.Auth;

namespace Auth.Controllers;

[ApiController]
[Route("/")]
public class AuthController : ControllerBase, IAuthService
{

    private readonly AppDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<AuthController> _logger;
    private readonly AuthOptions _authConfig;

    public AuthController(AppDbContext dbContext, IDateTimeProvider dateTimeProvider, ILogger<AuthController> logger, IOptions<AuthOptions> authConfig, IOptions<ConnectionStringOptions> opts)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
        _authConfig = authConfig.Value;
    }

    [HttpPost(nameof(CreateToken))]
    public async Task<string> CreateToken([FromBody] string email, CancellationToken cancellationToken = default)
    {
        var user = await GetUser(email, cancellationToken); //TODO: entities in controller!

        return GenerateToken(user);
    }

    private string GenerateToken(User user)
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
                expires: _dateTimeProvider.UtcNow.Add(TimeSpan.FromMinutes(_authConfig.Lifetime)),
                signingCredentials: new SigningCredentials(_authConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private async Task<User> GetUser(string email, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.Where(x => string.Equals(x.Email.ToUpper(), email.Trim().ToUpper())).SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new HttpResponseException(401, "User not found");
        }

        return user;
    }
}
