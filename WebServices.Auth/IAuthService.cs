using Refit;

namespace WebServices.Auth;

public interface IAuthService
{

    [Post("/CreateToken")]
    Task<string> CreateToken([Body] string email, CancellationToken cancellationToken = default);

}
