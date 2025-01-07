using Microsoft.EntityFrameworkCore;
using VFi.Infra.CMS.Mappings;
using VFi.NetDevPack.Events;


namespace VFi.Infra.CMS.Context;

public class EventStoreSqlContext : DbContext
{
    public EventStoreSqlContext(DbContextOptions<EventStoreSqlContext> options) : base(options) { }

    public DbSet<StoredEvent> StoredEvent { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StoredEventMap());

        base.OnModelCreating(modelBuilder);
    }
}
