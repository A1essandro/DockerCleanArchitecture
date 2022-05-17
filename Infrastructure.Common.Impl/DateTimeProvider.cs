using Infrastructure.Contracts;

namespace Infrastructure.Common.Impl;

public class DateTimeProvider : IDateTimeProvider
{
    public long UnixTimeSeconds => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public DateTime UtcNow => DateTime.UtcNow;
}
