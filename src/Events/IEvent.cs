using System;

namespace DomainEventStore.Events
{
    public interface IEvent
    {
         Guid Id { get; }
    }
}