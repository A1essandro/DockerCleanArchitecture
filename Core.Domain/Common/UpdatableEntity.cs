namespace Core.Domain.Common
{
    public abstract class UpdatableEntity : Entity
    {

        public DateTime Updated { get; set; }

    }

}