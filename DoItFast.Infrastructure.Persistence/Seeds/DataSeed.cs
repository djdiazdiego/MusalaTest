using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DoItFast.Infrastructure.Persistence.Seeds
{
    /// <summary>
    /// Seed for entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class DataSeed<TEntity> : ISeed where TEntity : class, IEntity
    {
        protected DataSeed() { }

        /// <inheritdoc />
        public abstract Task SeedAsync(IServiceProvider provider, CancellationToken cancelationToken);
        
        protected async Task SaveChangesAsync(IServiceProvider provider, TEntity[] entities, CancellationToken cancelationToken)
        {
            using var scope = provider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DbContextWrite>();
            var dbSet = context.Set<TEntity>();
            var length = entities.Length;
            for (int i = 0; i < length; i++)
            {
                var exist = await dbSet.AnyAsync(p => p.Id == entities[i].Id, cancelationToken);
                if (!exist)
                    dbSet.Add(entities[i]);
            }

            await context.SaveChangesAsync(cancelationToken);
        }
    }

}
