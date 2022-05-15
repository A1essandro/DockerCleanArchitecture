using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServices.Auth;

namespace Api.Controllers;

[ApiController]
[Route("/")]
public class MainController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<MainController> _logger;

    public MainController(IAuthService authService, ILogger<MainController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpGet(Name = "/")]
    [Authorize(Roles = "Admin")]
    public string Get()
    {
        return "Admin";
    }
}
