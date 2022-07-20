using DoItFast.Infrastructure.Persistence.Repositories;
using DoItFast.Infrastructure.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DoItFast.Infrastructure.Persistence.Contexts
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DbContextRead : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public DbContextRead(
            [NotNull] DbContextOptions<DbContextRead> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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
    }
}
