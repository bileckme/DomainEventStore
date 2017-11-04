using System;

namespace DomainEventStore.Events
{
    public class FundsWithdrawnEvent : IEvent
    {
        public Guid Id { get; private set;  }
        public decimal Amount { get; private set;  }

        public FundsWithdrawnEvent(Guid id, decimal amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}