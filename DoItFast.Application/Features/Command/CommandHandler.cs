using AutoMapper;
using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;
using DoItFast.Domain.Core.Abstractions.Dtos;
using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Persistence;

namespace DoItFast.Application.Features.Command
{
    public abstract class CreateCommandHandler<TModel, TResponse, TCommand> : ICommandHandler<TCommand, Response<TResponse>>
        where TModel : class, IEntity
        where TResponse : class, IDto
        where TCommand : class, ICommand<Response<TResponse>>
    {
        private readonly IRepository<TModel> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        protected CreateCommandHandler(
            IRepository<TModel> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<TResponse>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var entity = Activator.CreateInstance(typeof(TModel)) as TModel;

            _mapper.Map(request, entity);

            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var entityDto = _mapper.Map<TResponse>(entity);
            return new Response<TResponse>(entityDto);
        }
    }

    public abstract class UpdateCommandHandler<TModel, TResponse, TCommand> : ICommandHandler<TCommand, Response<TResponse>>
            where TModel : class, IEntity
            where TResponse : class, IDto
            where TCommand : class, ICommand<Response<TResponse>>
    {
        private readonly IRepository<TModel> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public UpdateCommandHandler(
            IRepository<TModel> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<TResponse>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var id = request.GetType().GetProperty("Id")?.GetValue(request, null);
            var entity = await _repository.FindAsync(new object[] { id }, cancellationToken);

            _mapper.Map(request, entity);

            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var entityDto = _mapper.Map<TResponse>(entity);
            return new Response<TResponse>(entityDto);
        }
    }

    public abstract class DeleteCommandHandler<TModel, TResponse, TCommand> : ICommandHandler<TCommand, Response<TResponse>>
                where TModel : class, IEntity
                where TResponse : class, IDto
                where TCommand : class, ICommand<Response<TResponse>>
    {
        private readonly IRepository<TModel> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public DeleteCommandHandler(
            IRepository<TModel> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<TResponse>> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var id = request.GetType().GetProperty("Id")?.GetValue(request, null);
            var entity = await _repository.FindAsync(new object[] { id }, cancellationToken);

            _repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var entityDto = _mapper.Map<TResponse>(entity);
            return new Response<TResponse>(entityDto);
        }
    }
}
