using Core.Domain.Common;
using Core.Specifications;
using Infrastructure.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : Entity
    {

        protected readonly AppDbContext DbContext;

        protected abstract IQueryable<TEntity> Entities { get; }

        protected RepositoryBase(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbContext.AddAsync(entity, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbContext.Update(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            var entity = await Get(id, cancellationToken);
            await Delete(entity, cancellationToken);
        }

        public Task<TEntity> Get(int id, CancellationToken cancellationToken = default)
        {
            return Entities.SingleAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyCollection<TEntity>> GetCollection(CancellationToken cancellationToken = default)
        {
            return await Entities.ToArrayAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<TEntity>> GetCollection(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await Entities.Where(specification.ToExpression()).ToArrayAsync(cancellationToken);
        }

    }
}