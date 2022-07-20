using DoItFast.Domain.Core.Enums;

namespace DoItFast.Domain.Core.Abstractions.Wrappers
{
    /// <summary>
    /// Search filter for pagination option.
    /// </summary>
    public interface IFilter
    {
        
    }

    /// <summary>
    /// Page and page size.
    /// </summary>
    public interface IPaging
    {
        /// <summary>
        /// Current diplayed page.
        /// </summary>
        public int Page { get; }
        /// <summary>
        /// Amount of element displayed per page.
        /// </summary>
        public int PageSize { get; }
    }

    /// <summary>
    /// Column name and how is goint to sorting.
    /// </summary>
    public interface IOrder
    {
        /// <summary>
        /// Column's name.
        /// </summary>
        public string SortBy { get; }
        /// <summary>
        /// Indicate if is sorting ascending or descending.
        /// Allowed: 
        ///     Ascending -> ASC = 1
        ///     Descending -> DESC =2
        /// </summary>
        public SortOperation SortOperation { get; }
    }
}
