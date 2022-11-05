using Application.UseCases;
using Auth.Services;
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
    private readonly IBackgroundTaskQueue _queue;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<AuthController> _logger;
    private readonly AuthOptions _authConfig;

    public AuthController(IMediator mediator, IBackgroundTaskQueue queue, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _queue = queue;
        _logger = logger;
    }

    [HttpGet]
    public async Task<string> CreateToken(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Before adding [{Thread.CurrentThread.ManagedThreadId}]");
        await _queue.QueueBackgroundWorkItemAsync(async ct =>
        {
            _logger.LogInformation($"START [{Thread.CurrentThread.ManagedThreadId}]");
            await Task.Delay(10000);
            _logger.LogInformation($"FINISH [{Thread.CurrentThread.ManagedThreadId}]");
        });
        _logger.LogInformation($"After adding [{Thread.CurrentThread.ManagedThreadId}]");

        return "OK!!!!!!!!!";
    }


    [HttpPost(nameof(CreateToken))]
    public Task<string> CreateToken([FromBody] string email, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating token for useri with email {Email}", email);

        return _mediator.Send(new GetOrCreateUserTokenUseCase(email));
    }

}
