using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DoItFast.Domain.Core.Abstractions.Persistence
{
    /// <summary>
    /// Repository used only for query operations.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IQueryRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Gets a query with all the elements of the entity.
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> FindAll();

        /// <summary>
        /// Get an entity by its primary keys.
        /// </summary>
        /// <param name="keyValues"></param>
        /// <param name="cancelationToken"></param>
        /// <returns></returns>
        ValueTask<TEntity?> FindAsync(object[] keyValues, CancellationToken cancelationToken);
    }
}
