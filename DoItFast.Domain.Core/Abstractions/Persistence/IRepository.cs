using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Enums;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DoItFast.Domain.Core.Abstractions.Persistence
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Remove entity from repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityState Remove(TEntity entity);

        /// <summary>
        /// Remove entities from repository.
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(params TEntity[] entities);

        /// <summary>
        /// Update entity in a repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityState Update(TEntity entity);

        /// <summary>
        /// Update a set of entities in a repository
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        void UpdateRange(params TEntity[] entities);

        /// <summary>
        /// Add entity to repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityState Add(TEntity entity);

        /// <summary>
        /// Add a set of entities to a repository.
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(params TEntity[] entities);

        /// <summary>
        /// Add entity to repository async.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EntityState> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a set of entities to a repository async.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    }
}
