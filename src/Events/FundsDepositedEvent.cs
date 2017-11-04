using System;

namespace DomainEventStore.Events
{
    public class FundsDepositedEvent : IEvent
    {
        public Guid Id { get; private set;  }
        public decimal Amount { get; private set;  }

        public FundsDepositedEvent(Guid id, decimal amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}