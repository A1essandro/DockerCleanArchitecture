using Core.Domain.Common;
using Core.Specifications;

namespace Infrastructure.Contracts.Repositories
{
    public interface IReadOnlyRepository<TEntity> where TEntity : Entity
    {

        Task<TEntity> Get(int id, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<TEntity>> GetCollection(CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<TEntity>> GetCollection(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    }
}