using Core.Domain.Common;

namespace Core.Domain;

public class Session : Entity
{

    internal Session(string token) => Token = token;

    public int UserId { get; set; }

    public User User { get; set; }

    public string Token { get; set; }

}
