namespace Infrastructure.Contracts;

public interface IDateTimeProvider
{

    long UnixTimeSeconds { get; }

    DateTime UtcNow { get; }

}
