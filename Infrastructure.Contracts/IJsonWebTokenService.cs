using Core.Domain;

namespace Infrastructure.Contracts;

public interface IJsonWebTokenService
{
    string GetToken(User user);

    TimeSpan Lifetime { get; }

}