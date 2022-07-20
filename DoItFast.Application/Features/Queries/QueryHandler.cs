using AutoMapper;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;
using DoItFast.Domain.Core.Abstractions.Queries;
using DoItFast.Domain.Core.Abstractions.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace DoItFast.Application.Features.Queries
{
    /// <summary>
    /// Get query handler
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TGetQuery"></typeparam>
    public abstract class GetQueryHandler<TModel, TResponse, TGetQuery> : IQueryHandler<TGetQuery, Response<TResponse>>
        where TModel : class, IEntity
        where TResponse : class, IDto
        where TGetQuery : class, IQuery<Response<TResponse>>
    {
        private readonly IQueryRepository<TModel> _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        protected GetQueryHandler(IQueryRepository<TModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<TResponse>> Handle(TGetQuery request, CancellationToken cancellationToken)
        {
            var id = request.GetType().GetProperty("Id")?.GetValue(request, null);
            var entity = await _repository.FindAsync(new object[] { id }, cancellationToken);
            var entityDto = _mapper.Map<TResponse>(entity);
            return new Response<TResponse>(entityDto);
        }
    }

    /// <summary>
    /// Get all query handler
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TGetAllQuery"></typeparam>
    public abstract class GetAllQueryHandler<TModel, TResponse, TGetAllQuery> : IQueryHandler<TGetAllQuery, Response<TResponse[]>>
          where TModel : class, IEntity
          where TResponse : class, IDto
          where TGetAllQuery : class, IQuery<Response<TResponse[]>>
    {
        private readonly IQueryRepository<TModel> _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        protected GetAllQueryHandler(IQueryRepository<TModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<TResponse[]>> Handle(TGetAllQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.FindAll()
               .ToArrayAsync(cancellationToken);
            var entitiesDto = _mapper.Map<TResponse[]>(entities);
            return new Response<TResponse[]>(entitiesDto);
        }
    }

    /// <summary>
    /// Filter query handler
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TFilterResponse"></typeparam>
    /// <typeparam name="TResponseDto"></typeparam>
    /// <typeparam name="TFilterQuery"></typeparam>
    /// <typeparam name="TQueryResponse"></typeparam>
    public abstract class FilterQueryHandler<TModel, TFilterResponse, TResponseDto, TFilterQuery> : IQueryHandler<TFilterQuery, Response<TFilterResponse>>
        where TModel : class, IEntity
        where TFilterResponse : class, IFilterResponseDto<TResponseDto>
        where TResponseDto : class, IDto
        where TFilterQuery : FilterQuery<TFilterResponse, TResponseDto, TModel>, IFilter, IQuery<Response<TFilterResponse>>
    {
        private readonly IQueryRepository<TModel> _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        protected FilterQueryHandler(IQueryRepository<TModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<TFilterResponse>> Handle(TFilterQuery request, CancellationToken cancellationToken)
        {
            var query = request.BuildFilter(_repository);
            var total = await query.CountAsync(cancellationToken);

            query = request.BuildOrder(query);
            query = query.BuildPagging(request.Paging);

            var entities = await query.ToListAsync(cancellationToken);
            var entitiesDto = _mapper.Map<List<TResponseDto>>(entities);

            var response = Activator.CreateInstance(typeof(TFilterResponse), new object[] { entitiesDto, total }) as TFilterResponse;

            return new Response<TFilterResponse>(response);
        }
    }
}
