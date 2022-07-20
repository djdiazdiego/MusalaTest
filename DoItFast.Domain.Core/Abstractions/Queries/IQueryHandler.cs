using DoItFast.Domain.Core.Abstractions.Wrappers;
using MediatR;

namespace DoItFast.Domain.Core.Abstractions.Queries
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : class, IQuery<TResponse>
        where TResponse : class, IResponse
    {
    }
}
