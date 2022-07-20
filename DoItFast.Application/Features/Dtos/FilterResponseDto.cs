using DoItFast.Domain.Core.Abstractions.Dtos;

namespace DoItFast.Application.Features.Dtos
{
    public abstract class FilterResponseDto<TDto> : IFilterResponseDto<TDto>
        where TDto : class, IDto
    {
        protected FilterResponseDto(List<TDto> entities, int total)
        {
            Data = entities;
            Total = total;
        }

        ///<inheritdoc/>
        public int Total { get; }

        ///<inheritdoc/>
        public List<TDto> Data { get; }
    }
}
