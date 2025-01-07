using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VFi.Infra.CMS.Context;
using VFi.NetDevPack.Events;

namespace VFi.Infra.CMS.Repository.EventSourcing;

public class EventStoreSqlRepository : IEventStoreRepository
{
    private readonly EventStoreSqlContext _context;

    public EventStoreSqlRepository(EventStoreSqlContext context)
    {
        _context = context;
    }

    public async Task<IList<StoredEvent>> All(Guid aggregateId)
    {
        return await (from e in _context.StoredEvent where e.AggregateId == aggregateId select e).ToListAsync();
    }

    public void Store(StoredEvent theEvent)
    {
        _context.StoredEvent.Add(theEvent);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
