using DoItFast.Domain.Core.Abstractions.Entities.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace DoItFast.Domain.Core.Abstractions.Entities
{
    /// <summary>
    /// Represents an identifiable entity root in the CQRS system.
    /// </summary>
    public interface IAggregateRoot : IEntity
    {
        /// <summary>
        /// Clear domain event.
        /// </summary>
        void ClearDomainEvents();

        /// <summary>
        /// Add domain event.
        /// </summary>
        /// <param name="event"></param>
        void AddDomainEvent(INotification @event);

        /// <summary>
        /// Remove domain event.
        /// </summary>
        /// <param name="event"></param>
        void RemoveDomainEvent(INotification @event);

        /// <summary>
        /// Get domain events from an aggregate.
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<INotification> GetDomainEvents();
    }
}
