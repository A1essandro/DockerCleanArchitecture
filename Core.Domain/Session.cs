using Core.Domain.Common;

namespace Core.Domain;

public class Session : Entity
{

    public Guid Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public string Token { get; set; }

}
