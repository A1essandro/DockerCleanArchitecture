using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auth.Exceptions;
using Core.Domain;
using Core.Specifications;
using Infrastructure.Auth;
using Infrastructure.Contracts;
using Infrastructure.Contracts.Repositories;
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

    private readonly IRepository<User> _userRepo;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<AuthController> _logger;
    private readonly AuthOptions _authConfig;

    public AuthController(IRepository<User> userRepo, IDateTimeProvider dateTimeProvider, ILogger<AuthController> logger, IOptions<AuthOptions> authConfig, IOptions<ConnectionStringOptions> opts)
    {
        _userRepo = userRepo;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
        _authConfig = authConfig.Value;
    }

    [HttpGet]
    public string Test()
    {
        return "OK";
    }

    [HttpPost(nameof(CreateToken))]
    public async Task<string> CreateToken([FromBody] string email, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating token for useri with email {Email}", email);
        var user = await GetUser(email, cancellationToken);

        var token = GenerateToken(user);

        await AddTokenToUsersSessions(user, token, cancellationToken);

        return token;
    }

    private async Task AddTokenToUsersSessions(User user, string token, CancellationToken cancellationToken)
    {
        user.Sessions.Add(new Session
        {
            Token = token
        });
        await _userRepo.Update(user, cancellationToken);
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
        var users = await _userRepo.GetCollection(new ByEmailSpec(email), cancellationToken);

        if (users.Count == 0)
        {
            throw new HttpResponseException(401, "User not found");
        }

        return users.Single();
    }
}
