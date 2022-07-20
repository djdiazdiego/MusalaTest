using DoItFast.Domain.Core.Abstractions.Wrappers;
using MediatR;

namespace DoItFast.Domain.Core.Abstractions.Commands
{
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : class, ICommand<TResponse>
         where TResponse : class, IResponse
    {
    }
}
