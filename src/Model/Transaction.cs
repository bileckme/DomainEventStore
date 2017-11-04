using System;
using System.Collections.Generic;

namespace DomainEventStore.Model
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
    }
}