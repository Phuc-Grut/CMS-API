using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Consul;
using VFi.Infra.CMS.Context;
using VFi.Infra.CMS.EventSourcing;
using VFi.Infra.CMS.Repository;
using VFi.Infra.CMS.Repository.EventSourcing;
using VFi.NetDevPack;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Events;
using VFi.NetDevPack.Mediator;

namespace VFi.Infra.CMS;

public class StartupApplication : IStartupApplication
{
    public int Priority => 2;
    public bool BeforeConfigure => true;

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        //services.AddConsul(configuration);
        var connectionStringPlaceHolder = configuration.GetConnectionString("DAMConnection");
        services.AddDbContext<SqlCoreContext>((serviceProvider, dbContextBuilder) =>
        {
            var context = serviceProvider.GetRequiredService<IContextUser>();
            var connectionString = connectionStringPlaceHolder.Replace("{data_zone}", context.UserClaims.Data_Zone).Replace("{data}", context.UserClaims.Data);
            //Console.WriteLine(connectionString);
            dbContextBuilder.UseSqlServer(connectionString);
        });

        var connectionEventPlaceHolder = configuration.GetConnectionString("DAMEventConnection");
        services.AddDbContext<EventStoreSqlContext>((serviceProvider, dbContextBuilder) =>
        {
            var context = serviceProvider.GetRequiredService<IContextUser>();
            var connectionString = connectionStringPlaceHolder.Replace("{data_zone}", context.UserClaims.Data_Zone).Replace("{data}", context.UserClaims.Data);
            dbContextBuilder.UseSqlServer(connectionString);
        });
        services.AddScoped<Publisher>();
        services.AddScoped<IMediatorHandler, VFi.Infra.CMS.Bus.MediatorHandler>();
        services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
        services.AddScoped<IEventStore, SqlEventStore>();
        services.AddScoped<EventStoreSqlContext>();
        // Infra - Data

        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<ICategoryRootRepository, CategoryRootRepository>();
        services.AddScoped<IContentTypeRepository, ContentTypeRepository>();
        services.AddScoped<IContentCategoryMappingRepository, ContentCategoryMappingRepository>();
        services.AddScoped<IContentRepository, ContentRepository>();
        services.AddScoped<IGroupCategoryRepository, GroupCategoryRepository>();
        services.AddScoped<IContentGroupCategoryMappingRepository, ContentGroupCategoryMappingRepository>();
        services.AddScoped<IAccessPermissionRepository, AccessPermissionRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IOpenActivityRepository, OpenActivityRepository>();
        services.AddScoped<IItemProductRepository, ItemProductRepository>();

        services.AddScoped<IGroupWebLinkRepository, GroupWebLinkRepository>();
        services.AddScoped<IWebLinkRepository, WebLinkRepository>();

        services.AddScoped<ISyntaxCodeRepository, SyntaxCodeRepository>();

        services.AddScoped<SqlCoreContext>();
        services.AddScoped<IDAMContextProcedures, DAMContextProcedures>();
        services.AddRabbitMQ(configuration);

        services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
        services.AddSingleton<MasterApiContext>();
        services.AddSingleton<IMasterRepository, MasterRepository>();

        var redisConn = configuration.GetConnectionString("Redis");
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConn;
        });
    }

    public void Configure(WebApplication application, IWebHostEnvironment webHostEnvironment)
    {
        try
        {
            application.UseConsul(application.Lifetime);
        }
        catch (Exception)
        {
        }
    }
}
