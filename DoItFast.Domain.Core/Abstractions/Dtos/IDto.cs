using System.Collections.Generic;

namespace DoItFast.Domain.Core.Abstractions.Dtos
{
    public interface IDto
    {
    }

    public interface IFilterResponseDto<TResponse> : IDto
       where TResponse : class, IDto
    {

        /// <summary>
        /// Total of records.
        /// </summary>
        int Total { get; }

        /// <summary>
        /// Entities list.
        /// </summary>
        List<TResponse> Data { get; }
    }
}
