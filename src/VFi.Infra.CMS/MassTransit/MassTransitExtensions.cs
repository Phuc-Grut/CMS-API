using System;
using System.Data;
using System.Linq;
using Consul;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using VFi.Infra.CMS.MassTransit.Consumers;
using VFi.Infra.CMS.Repository;
using VFi.NetDevPack.Configuration;

namespace VFi.Infra.CMS.Consul;

public static class MassTransitExtensions
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {

        var queueSettings = configuration.GetSection("RabbitConfig").Get<RabbitConfig>();
        //services.AddSingleton(sp => configuration.GetSection("RabbitConfig").Get<RabbitConfig>());
        if (queueSettings != null)
            if (queueSettings.RabbitEnabled)
            {
                services.AddMassTransit(x =>
                {
                    if (queueSettings.ConsumerEnabled)
                    {
                        // x.AddConsumer<UserEmailEventConsumer>();
                        // x.AddConsumer<UserLogedEventConsumer>(); 
                        x.AddConsumer<EventConsumer>();
                        //  x.AddConsumer<FaultConsumer<UserEmailEvent>>();
                    }



                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(queueSettings.RabbitHostName, queueSettings.RabbitVirtualHost, h =>
                        {
                            h.Username(queueSettings.RabbitUsername);
                            h.Password(queueSettings.RabbitPassword);
                        });
                        //cfg.ConfigureEndpoints(context);

                        if (queueSettings.ConsumerEnabled)
                        {
                            cfg.ReceiveEndpoint("user-email-event", e =>
                            {
                                e.ExchangeType = ExchangeType.Direct;


                                //e.ConfigureConsumer<UserEmailEventConsumer>(context, c =>
                                //{
                                //    //https://masstransit-project.com/usage/exceptions.html#retry-configuration
                                //    c.UseMessageRetry(r =>
                                //    {
                                //      //  r.Interval(2, TimeSpan.FromMilliseconds(1000));
                                //        r.Intervals(TimeSpan.FromMilliseconds(3000), TimeSpan.FromMilliseconds(8000), TimeSpan.FromMilliseconds(12000));
                                //        r.Ignore<ArgumentNullException>();
                                //        r.Ignore<DataException>(x => x.Message.Contains("SQL"));
                                //    });
                                //});
                            });



                            cfg.ReceiveEndpoint("user-email-event_error", e =>
                            {
                                e.ExchangeType = ExchangeType.Direct;
                                //e.ConfigureConsumer<FaultConsumer<UserEmailEvent>>(context, c =>
                                //{

                                //});
                            });

                            cfg.ReceiveEndpoint("Event", e =>
                            {
                                e.ConfigureConsumer<EventConsumer>(context);
                            });

                            //cfg.ReceiveEndpoint(typeof(UserLogedEvent).FullName, e =>
                            //{
                            //    e.ConfigureConsumer<UserLogedEventConsumer>(context);
                            //});
                        }

                    });
                });

                if (queueSettings.PublisherEnabled)
                {
                    // EndpointConvention.Map<UserEmailEvent>(queueSettings.BuildEndPoint("user-email-event")); 
                    // services.AddScoped<IEmailRepository, EmailRepository>();
                }

            }

        //var provider = services.BuildServiceProvider();
        //var busControl = provider.GetRequiredService<IBusControl>();



        return services;
    }

    public static IApplicationBuilder UseRabbitMQ(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {


        return app;
    }
}
