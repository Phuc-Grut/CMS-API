using VFi.NetDevPack.Messaging;

namespace VFi.NetDevPack.Events;

public interface IEventStore
{
    void Save<T>(T theEvent) where T : Event;
}
