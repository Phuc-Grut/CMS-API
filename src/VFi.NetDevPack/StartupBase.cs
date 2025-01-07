using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VFi.NetDevPack.Plugins;
using VFi.NetDevPack.TypeSearchers;

namespace VFi.NetDevPack;

public static class StartupBase
{

    /// <summary>
    /// Add and configure services
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration root of the application</param>
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //register application
        // var mvcBuilder = RegisterApplication(services, configuration);

        //register extensions 
        //RegisterExtensions(mvcBuilder, configuration);

        //find startup configurations provided by other assemblies
        var typeSearcher = new AppTypeSearcher();
        services.AddSingleton<ITypeSearcher>(typeSearcher);

        var startupConfigurations = typeSearcher.ClassesOfType<IStartupApplication>();

        //Register startup
        var instancesBefore = startupConfigurations
            .Where(startup => PluginExtensions.OnlyInstalledPlugins(startup))
            .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
            .Where(startup => startup.BeforeConfigure)
            .OrderBy(startup => startup.Priority);

        //configure services
        foreach (var instance in instancesBefore)
            instance.ConfigureServices(services, configuration);

        ////register mapper configurations
        //InitAutoMapper(typeSearcher);

        ////Register custom type converters
        //RegisterTypeConverter(typeSearcher);

        ////add mediator
        //AddMediator(services, typeSearcher);

        ////Add MassTransit
        //AddMassTransitRabbitMq(services, configuration, typeSearcher);

        ////Register startup
        //var instancesAfter = startupConfigurations
        //    .Where(startup => PluginExtensions.OnlyInstalledPlugins(startup))
        //    .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
        //    .Where(startup => !startup.BeforeConfigure)
        //    .OrderBy(startup => startup.Priority);

        ////configure services
        //foreach (var instance in instancesAfter)
        //    instance.ConfigureServices(services, configuration);

        ////Execute startupbase interface
        //ExecuteStartupBase(typeSearcher);
    }

    /// <summary>
    /// Configure HTTP request pipeline
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    /// <param name="webHostEnvironment">WebHostEnvironment</param>
    public static void ConfigureRequestPipeline(Microsoft.AspNetCore.Builder.WebApplication application, IWebHostEnvironment webHostEnvironment)
    {
        //find startup configurations provided by other assemblies
        var typeSearcher = new AppTypeSearcher();
        var startupConfigurations = typeSearcher.ClassesOfType<IStartupApplication>();

        //create and sort instances of startup configurations
        var instances = startupConfigurations
            .Where(startup => PluginExtensions.OnlyInstalledPlugins(startup))
            .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
            .OrderBy(startup => startup.Priority);

        //configure request pipeline
        foreach (var instance in instances)
            instance.Configure(application, webHostEnvironment);
    }
}
