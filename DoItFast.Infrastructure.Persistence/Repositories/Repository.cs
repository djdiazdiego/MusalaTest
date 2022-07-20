using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoItFast.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class Repository<TEntity> : QueryRepository<TEntity>, IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(DbContext dbContext) : base(dbContext) => _context = dbContext;

        /// <inheritdoc />
        public Domain.Core.Enums.EntityState Add(TEntity entity) =>
            (Domain.Core.Enums.EntityState)_context.Set<TEntity>().Add(entity).State;

        /// <inheritdoc />
        public async Task<Domain.Core.Enums.EntityState> AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
            (Domain.Core.Enums.EntityState)(await _context.Set<TEntity>().AddAsync(entity, cancellationToken)).State;

        /// <inheritdoc />
        public void AddRange(params TEntity[] entities) => _context.Set<TEntity>().AddRange(entities);

        /// <inheritdoc />
        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        /// <inheritdoc />
        public Domain.Core.Enums.EntityState Remove(TEntity entity) =>
            (Domain.Core.Enums.EntityState)_context.Set<TEntity>().Remove(entity).State;

        /// <inheritdoc />
        public void RemoveRange(params TEntity[] entities) =>
            _context.Set<TEntity>().RemoveRange(entities);

        /// <inheritdoc />
        public Domain.Core.Enums.EntityState Update(TEntity entity) =>
            (Domain.Core.Enums.EntityState)_context.Set<TEntity>().Update(entity).State;

        /// <inheritdoc />
        public void UpdateRange(params TEntity[] entities) =>
            _context.Set<TEntity>().UpdateRange(entities);
    }
}
