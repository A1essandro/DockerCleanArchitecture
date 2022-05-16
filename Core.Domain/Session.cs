namespace Core.Domain;

public class Session
{

    public Guid Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public string Token { get; set; }

}
