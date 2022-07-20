using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Infrastructure.Persistence.Repositories;
using DoItFast.Infrastructure.Shared.Extensions;
using DoItFast.Infrastructure.Shared.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DoItFast.Infrastructure.Persistence.Contexts
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DbContextWrite : DbContext, IUnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="authenticatedUserService"></param>
        public DbContextWrite(
            [NotNull] DbContextOptions<DbContextWrite> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("DoItFast");
            builder.AddEntitiesAndConfiguration(typeof(Repository<>).Assembly);
            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.SetCreatedDate(DateTimeOffset.UtcNow);
                        break;
                    case EntityState.Modified:
                        entry.Entity.SetLastModified(DateTimeOffset.UtcNow);
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
