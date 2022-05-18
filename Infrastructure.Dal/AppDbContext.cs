using Core.Domain;
using Core.Domain.Common;
using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Dal
{

    public class AppDbContext : DbContext
    {

        private static string ConnectionStringName = "App";
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<AppDbContext> _logger;

        public AppDbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider, ILogger<AppDbContext> logger) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public DbSet<User> Users { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ChangeTracker.DetectChanges();
            var added = this.ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Added)
                        .Where(t => t.Entity is Entity)
                        .Select(t => t.Entity)
                        .OfType<Entity>()
                        .ToArray();

            foreach (var entity in added)
            {
                entity.Created = _dateTimeProvider.UtcNow;
                _logger.LogInformation("Adding {EntityName} {Entity} to DB", entity.GetType().Name, entity);
            }

            var modified = this.ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Modified)
                        .Where(t => t.Entity is UpdatableEntity)
                        .Select(t => t.Entity)
                        .OfType<UpdatableEntity>()
                        .ToArray();

            foreach (var entity in modified)
            {
                entity.Updated = _dateTimeProvider.UtcNow;
                _logger.LogInformation("Updating {EntityName} {Entity} in DB", entity.GetType().Name, entity);
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }

}