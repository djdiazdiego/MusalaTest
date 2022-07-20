using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using DoItFast.Domain.Core.Abstractions.Wrappers;
using DoItFast.Domain.Core.Enums;

namespace DoItFast.Application.Wrappers
{
    ///<inheritdoc/>
    public abstract class Filter : IFilter
    {
        /// <summary>
        /// Contain page information.
        /// </summary>
        public PagingModel Paging { get; set; }
        /// <summary>
        /// Array of column, the ordenation is in the order on the array.
        /// </summary>
        public ColumnNameModel Order { get; set; }
    }

    ///<inheritdoc/>
    public class PagingModel : IPaging
    {
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 10;

        ///<inheritdoc/>
        public int Page { get; set; } = DefaultPage;

        ///<inheritdoc/>
        public int PageSize { get; set; } = DefaultPageSize;
    }

    ///<inheritdoc/>
    public class ColumnNameModel : IOrder
    {
        private const string DefaultSortBy = nameof(IEntity.Id);
        private const SortOperation DefaultSortOperation = SortOperation.ASC;

        ///<inheritdoc/>
        public string SortBy { get; set; } = DefaultSortBy;
        ///<inheritdoc/>
        public SortOperation SortOperation { get; set; } = DefaultSortOperation;
    }
}

