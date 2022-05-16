using Core.Domain.Common;

namespace Core.Domain;

public class User : Entity
{

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public IList<Session> Sessions { get; set; } = new List<Session>(0);

}
