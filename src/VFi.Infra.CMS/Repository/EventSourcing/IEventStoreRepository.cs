using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VFi.NetDevPack.Events;

namespace VFi.Infra.CMS.Repository.EventSourcing;

public interface IEventStoreRepository : IDisposable
{
    void Store(StoredEvent theEvent);
    Task<IList<StoredEvent>> All(Guid aggregateId);
}
