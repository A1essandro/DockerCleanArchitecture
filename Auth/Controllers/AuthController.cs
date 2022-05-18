using Application.UseCases;
using Infrastructure.Auth;
using Infrastructure.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebServices.Auth;

namespace Auth.Controllers;

[ApiController]
[Route("/")]
public class AuthController : ControllerBase, IAuthService
{

    private readonly IMediator _mediator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<AuthController> _logger;
    private readonly AuthOptions _authConfig;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost(nameof(CreateToken))]
    public Task<string> CreateToken([FromBody] string email, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating token for useri with email {Email}", email);

        return _mediator.Send(new GetOrCreateUserTokenUseCase(email));
    }

}
