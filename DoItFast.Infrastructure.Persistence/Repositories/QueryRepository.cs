using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DoItFast.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public QueryRepository(DbContext dbContext) => _context = dbContext;

        /// <inheritdoc />
        public IQueryable<TEntity> FindAll() => _context.Set<TEntity>();

        /// <inheritdoc />
        public async ValueTask<TEntity?> FindAsync(object[] keyValues, CancellationToken cancelationToken) =>
            await _context.Set<TEntity>().FindAsync(keyValues, cancelationToken);
    }
}
