using System;

namespace VFi.NetDevPack.Messaging;

public abstract class DomainEvent : Event
{
    protected DomainEvent(Guid aggregateId)
    {
        AggregateId = aggregateId;
    }
}
