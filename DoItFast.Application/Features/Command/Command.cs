using DoItFast.Application.Wrappers;
using DoItFast.Domain.Core.Abstractions.Commands;
using DoItFast.Domain.Core.Abstractions.Dtos;

namespace DoItFast.Application.Features.Command
{
    /// <summary>
    /// Base command.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public abstract class Command<TResult> : ICommand<Response<TResult>>
        where TResult : class, IDto
    {
    }

    /// <summary>
    /// Command with entity identifier.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class Command<TKey, TResult> : ICommand<Response<TResult>>
        where TResult : class, IDto
    {
        protected Command(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// Entity identifier.
        /// </summary>
        public TKey Id { get; set; }
    }

    /// <summary>
    /// Base update command.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public abstract class UpdateCommand<TKey, TResult> : ICommand<Response<TResult>>
        where TResult : class, IDto
    {
        /// <summary>
        /// Entity identifier.
        /// </summary>
        public TKey Id { get; set; }
    }
}
