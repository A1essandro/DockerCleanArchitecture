using Core.Domain.Common;

namespace Infrastructure.Contracts.Repositories
{

    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : Entity
    {

        Task Update(TEntity entity, CancellationToken cancellationToken = default);

        Task Add(TEntity entity, CancellationToken cancellationToken = default);

        Task Delete(TEntity entity, CancellationToken cancellationToken = default);

        Task Delete(int id, CancellationToken cancellationToken = default);

    }
}