namespace Core.Domain.Common;

public abstract class Entity
{

    public int Id { get; set; }

    public DateTime Created { get; set; }

}