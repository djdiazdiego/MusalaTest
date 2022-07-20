using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Core.Abstractions.Queries;
using DoItFast.Domain.Core.Abstractions.Wrappers;

namespace DoItFast.Application.Features.Queries
{
    /// <summary>
    /// Base query.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class Query<TResponse> : IQuery<Response<TResponse>>
        where TResponse : class
    {
        protected Query() { }
    }
    /// <summary>
    /// Query with entity identifier.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class Query<TKey, TResponse> : IQuery<Response<TResponse>>
        where TResponse : class, IDto
    {
        protected Query(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// Entity identifier.
        /// </summary>
        public TKey Id { get; set; }
    }

    /// <summary>
    /// Base query
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class EnumerationQuery<Tkey, TResponse> : IQuery<Response<TResponse>>
             where TResponse : class
    {
        protected EnumerationQuery() { }
    }

    /// <summary>
    /// Query with entity filter
    /// </summary>
    /// <typeparam name="TFilterResponse"></typeparam>
    /// <typeparam name="TResponseDto"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public abstract class FilterQuery<TFilterResponse, TResponseDto, TModel> : Filter, IQuery<Response<TFilterResponse>>
        where TFilterResponse : class, IFilterResponseDto<TResponseDto>
        where TResponseDto : class, IDto
        where TModel : class, IEntity
    {
        /// <summary>
        /// To build filter
        /// </summary>
        /// <returns></returns>
        public abstract IQueryable<TModel> BuildFilter(IQueryRepository<TModel> queryRepository);

        /// <summary>
        /// To build order
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<TModel> BuildOrder(IQueryable<TModel> query) => query.ApplyOrder(this.Order);
    }
}
