using AutoMapper;
using DoItFast.Application.Features.Dtos;
using DoItFast.Application.Features.Queries;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Abstractions.Queries;
using DoItFast.Domain.Core.Abstractions.Wrappers;
using DoItFast.Infrastructure.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoItFast.WebApi.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task<ActionResult<Response<TFilterResponse>>> BuildFilterAsync<TFilterRequest, TResponse, TFilterResponse>(
            this Controller controller,
            TFilterRequest filter,
            IMapper mapper,
            IMediator mediator,
            CancellationToken cancellationToken)
            where TFilterRequest : class, IFilter, IDto
            where TResponse : class, IDto
            where TFilterResponse : class, IFilterResponseDto<TResponse>, IDto
        {
            var type = typeof(IQuery<Response<TFilterResponse>>).GetConcreteTypeWithFilter(assembly: typeof(Query<>).Assembly);
            var query = Activator.CreateInstance(type);
            mapper.Map(filter, query);
            var result = await mediator.Send(query, cancellationToken);
            return controller.Ok(result);
        }

        public static Task<ActionResult<Response<TResponse>>> BuildPostPutPatchAsync<TRequest, TResponse>(
            this Controller controller,
            TRequest dto,
            IMapper mapper,
            IMediator mediator,
            Type commandType,
            CancellationToken cancellationToken)
            where TRequest : class, IDto
            where TResponse : class, IDto
        {
            return controller.BuildGenericAsync<TRequest, TResponse>(dto, mapper, mediator, commandType, cancellationToken);
        }

        public static async Task<ActionResult<Response<TResponse>>> BuildGetDeleteAsync<TKey, TResponse>(
            this Controller controller,
            TKey id,
            IMediator mediator,
            Type queryComandommandType,
            CancellationToken cancellationToken)
            where TResponse : class, IDto
        {
            var type = queryComandommandType.GetConcreteTypeWithFilter();
            var queryCommand = Activator.CreateInstance(type, new object[] { id });
            var result = await mediator.Send(queryCommand, cancellationToken);
            return controller.Ok(result);
        }

        public static async Task<ActionResult<Response<TResponse[]>>> BuildGetAllAsync<TKey, TResponse>(
            this Controller controller,
            IMediator mediator,
            CancellationToken cancellationToken)
            where TResponse : class, IDto
        {
            var type = typeof(TResponse) != typeof(EnumerationDto) ?
                typeof(Query<TResponse[]>).GetConcreteTypeWithFilter() :
                typeof(EnumerationQuery<TKey, TResponse[]>).GetConcreteTypeWithFilter();

            var query = Activator.CreateInstance(type);
            var result = await mediator.Send(query, cancellationToken);
            return controller.Ok(result);
        }

        public static async Task<ActionResult<Response<TResponse>>> BuildGenericAsync<TRequest, TResponse>(
            this Controller controller,
            TRequest dto,
            IMapper mapper,
            IMediator mediator,
            Type queryCommandType,
            CancellationToken cancellationToken)
            where TRequest : class, IDto
            where TResponse : class, IDto
        {
            var type = queryCommandType.GetConcreteTypeWithFilter();
            var command = Activator.CreateInstance(type);
            mapper.Map(dto, command);
            var result = await mediator.Send(command, cancellationToken);
            return controller.Ok(result);
        }
    }
}
