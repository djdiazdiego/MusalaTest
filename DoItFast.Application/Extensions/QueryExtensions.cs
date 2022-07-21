using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Wrappers;
using DoItFast.Domain.Core.Enums;
using System.Linq.Expressions;

namespace DoItFast.Application.Features.Queries
{
    public static class QueryExtensions
    {
        /// <summary>
        /// Apply orders.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="source"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static IQueryable<TModel> ApplyOrder<TModel>(this IQueryable<TModel> source, IOrder order)
        {
            var sortBy = typeof(TModel).GetProperties()
                .Where(p => p.Name.ToUpper() == order.SortBy.ToUpper())
                .Select(p => p.Name)
                .FirstOrDefault();

            sortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : nameof(IEntity.Id);

            var sortOperation = order.SortOperation == default ? SortOperation.ASC : order.SortOperation;

            var type = typeof(TModel);
            var property = type.GetProperty(sortBy);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var typeArguments = new Type[] { type, property.PropertyType };
            var methodName = sortOperation == SortOperation.ASC ? "OrderBy" : "OrderByDescending";
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<TModel>(resultExp);
        }

        /// <summary>
        /// Build paginator.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagging"></param>
        public static IQueryable<TModel> BuildPagging<TModel>(this IQueryable<TModel> query, IPaging pagging)
            where TModel : class
        {
            var page = pagging.Page < 1 ? 1 : pagging.Page;
            var pageSize = pagging.PageSize < 1 ? 10 : pagging.PageSize;

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
