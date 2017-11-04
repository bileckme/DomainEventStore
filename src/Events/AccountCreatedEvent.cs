using System;

namespace DomainEventStore.Events
{
    public class AccountCreatedEvent : IEvent
    {
        public Guid Id { get; private set;  }
        public string Name { get; private set;  }

        public AccountCreatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}