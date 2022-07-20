using DoItFast.Domain.Core.Abstractions.Wrappers;
using MediatR;

namespace DoItFast.Domain.Core.Abstractions.Queries
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
        where TResponse : class, IResponse
    {
    }
}
