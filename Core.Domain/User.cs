using Core.Domain.Common;

namespace Core.Domain;

public class User : UpdatableEntity
{

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    private List<Session> _sessions = new List<Session>(0);
    public IReadOnlyCollection<Session> Sessions => _sessions;

    #region Methods

    public void AddSessionToken(string token)
    {
        _sessions.Add(new Session(token));
    }

    public bool HasSessionAfter(DateTime date) => Sessions.Any(x => x.Created > date);

    public Session GetLastSession() => Sessions.OrderBy(x => x.Created).First();

    #endregion

}
