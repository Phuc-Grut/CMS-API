using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Mappings;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Domain;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Messaging;

namespace VFi.Infra.CMS.Context;

public sealed class SqlCoreContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;
    private IDAMContextProcedures _procedures;
    public SqlCoreContext(DbContextOptions<SqlCoreContext> options, IMediatorHandler mediatorHandler) : base(options)
    {
        _mediatorHandler = mediatorHandler;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }
    public DbSet<AccessPermission> AccessPermission { get; set; }
    public DbSet<Activity> Activity { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<CategoryRoot> CategoryRoot { get; set; }
    public DbSet<Content> Content { get; set; }
    public DbSet<ContentCategoryMapping> ContentCategoryMapping { get; set; }
    public DbSet<ContentGroupCategoryMapping> ContentGroupCategoryMapping { get; set; }
    public DbSet<GroupCategory> GroupCategory { get; set; }
    public DbSet<GroupWebLink> GroupWebLink { get; set; }
    public DbSet<Item> Item { get; set; }
    public DbSet<OpenActivity> OpenActivity { get; set; }
    public DbSet<ContentType> ContentType { get; set; }
    public DbSet<WebLink> WebLink { get; set; }
    public DbSet<SP_GET_TOP_NEW_CONTENT> SP_GET_TOP_NEW_CONTENT { get; set; }
    public DbSet<SP_GET_TOP_CATEGORY> SP_GET_TOP_CATEGORY { get; set; }
    public DbSet<SP_GET_CONTENT_BY_CONTENTTYPE> SP_GET_CONTENT_BY_CONTENTTYPE { get; set; }
    public DbSet<ItemProduct> ItemProduct { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<FluentValidation.Results.ValidationResult>();
        modelBuilder.Ignore<Event>();

        //foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
        //    e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
        //    property.SetColumnType("nvarchar(100)");

        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.AccessPermissionConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.ActivityConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.CategoryRootConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.ContentConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.ContentCategoryMappingConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.ContentGroupCategoryMappingConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.GroupCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.ItemConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.OpenActivityConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.ContentTypeConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.ItemProductConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.GroupWebLinkConfiguration());
        modelBuilder.ApplyConfiguration(new Infra.CMS.Mappings.Configurations.WebLinkConfiguration());

        base.OnModelCreating(modelBuilder);
    }
    public IDAMContextProcedures Procedures
    {
        get
        {
            if (_procedures is null)
                _procedures = new DAMContextProcedures(this);
            return _procedures;
        }
        set
        {
            _procedures = value;
        }
    }

    public IDAMContextProcedures GetProcedures()
    {
        return Procedures;
    }

    protected void OnModelCreatingGeneratedProcedures(ModelBuilder modelBuilder)
    {
    }
    public async Task<bool> Commit()
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await _mediatorHandler.PublishDomainEvents(this).ConfigureAwait(false);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed

        var success = await SaveChangesAsync() > 0;

        return success;
    }



}


public static class MediatorExtension
{
    public static async Task PublishDomainEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        var tasks = domainEvents
            .Select(async (domainEvent) =>
            {
                await mediator.PublishEvent(domainEvent);
            });

        await Task.WhenAll(tasks);
    }
}
