using MediatR;
using System.Collections.Generic;

namespace DoItFast.Domain.Core.Abstractions.Entities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class AggregateRoot<TKey, TUserKey> : Entity<TKey, TUserKey>, IAggregateRoot
    {
        private readonly List<INotification> _domainEvents;

        protected AggregateRoot() => _domainEvents = new List<INotification>();
        protected AggregateRoot(TKey id) : base(id) => _domainEvents = new List<INotification>();
        

        /// <inheritdoc />
        public void AddDomainEvent(INotification @event) => _domainEvents.Add(@event);

        /// <inheritdoc />
        public void RemoveDomainEvent(INotification @event) => _domainEvents.Remove(@event);

        /// <inheritdoc />
        public void ClearDomainEvents() => _domainEvents.Clear();

        /// <inheritdoc />
        public IReadOnlyCollection<INotification> GetDomainEvents() => _domainEvents;
    }
}
