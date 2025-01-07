using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.NetDevPack.Context;

public static class Abstractions
{
    public static IServiceCollection AddAspNetUserConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddScoped<IContextUser, ContextUser>();

        return services;
    }
}
