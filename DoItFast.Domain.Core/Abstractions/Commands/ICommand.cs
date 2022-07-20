using DoItFast.Domain.Core.Abstractions.Wrappers;
using MediatR;

namespace DoItFast.Domain.Core.Abstractions.Commands
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
        where TResponse : class, IResponse
    {
    }
}
